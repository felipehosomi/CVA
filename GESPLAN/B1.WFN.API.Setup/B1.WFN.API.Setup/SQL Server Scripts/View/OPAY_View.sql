CREATE VIEW [dbo].[OPAY_View] AS 
	
	-- Contas a receber
	select ORCT.DocEntry as PmntEntry, 'ORCT' as PmntTable, ORCT.DocDate as PaymentDate, ORCT.Canceled,
		   RCT2.DocEntry, RCT2.InvType, case RCT2.InvType when '30' then RCT2.DocLine else RCT2.InstId end as InstId, 
		   RCT2.DocLine, RCT2.SumApplied - RCT2.WtAppld as SumApplied, RCT2.WtAppld, RCT2.DcntSum,
		   null as BoeNum, null as BoeDueDate, 
		   coalesce(ORCT.CashAcct, ORCT.TrsfrAcct, (select distinct CreditAcct from RCT3 where DocNum = ORCT.DocEntry), '') as PmntAcct,
		   case when ORCT.CashSum <> 0 then 'Dinheiro'
		   else case when ORCT.CheckSum <> 0 then 'Cheque'
		   else case when ORCT.TrsfrSum <> 0 then 'Transferência bancária'
		   else case when ORCT.CreditSum <> 0 then 'Cartão de crédito'
		   end end end end as PaymentType
	  from RCT2
	 inner join ORCT on ORCT.DocEntry = RCT2.DocNum
	 where ORCT.BoeSum = 0
	   
	 union all
	 
	-- Boleto a receber
	select ORCT.DocEntry as PmntEntry, 'ORCT' as PmntTable, OJDT.RefDate as PaymentDate, ORCT.Canceled,
		   RCT2.DocEntry, RCT2.InvType, case RCT2.InvType when '30' then RCT2.DocLine else RCT2.InstId end as InstId, 
		   RCT2.DocLine, RCT2.SumApplied - RCT2.WtAppld as SumApplied, RCT2.WtAppld, RCT2.DcntSum,
		   OBOE.BoeNum, OBOE.DueDate as BoeDueDate, ORCT.BoeAcc as PmntAcct, 'Boleto' as PaymentType
	  from RCT2
	 inner join ORCT on ORCT.DocEntry = RCT2.DocNum
	 inner join OBOE on OBOE.BoeKey = ORCT.BoeAbs 
	   and OBOE.BoeType = 'I' -- Incoming
	 inner join OBOT on OBOT.AbsEntry = (select top(1) BOT1.[AbsEntry]
												 from [dbo].[BOT1]
												where BOT1.[BOENumber] = OBOE.[BoeNum] 
												  and BOT1.[BoeType] = OBOE.[BoeType]
												order by BOT1.[AbsEntry] desc)
	   and OBOT.StatusTo = 'P'
	   and OBOT.Reconciled = 'N'
	 inner join OJDT on OBOT.TransId = OJDT.TransId	
	 where ORCT.BoeAbs is not null
	   
	 union all
	 
	-- Contas a pagar
	select OVPM.DocEntry as PmntEntry, 'OVPM' as PmntTable, OVPM.DocDate as PaymentDate, OVPM.Canceled,
		   VPM2.DocEntry, VPM2.InvType, case VPM2.InvType when '30' then VPM2.DocLine else VPM2.InstId end as InstId, 
		   VPM2.DocLine, VPM2.SumApplied - VPM2.WtAppld as SumApplied, VPM2.WtAppld, VPM2.DcntSum,
		   null as BoeNum, null as BoeDueDate, 
		   coalesce(OVPM.CashAcct, OVPM.TrsfrAcct, (select distinct CreditAcct from VPM3 where DocNum = OVPM.DocEntry), '') as PmntAcct,
		   case when OVPM.CashSum <> 0 then 'Dinheiro'
		   else case when OVPM.CheckSum <> 0 then 'Cheque'
		   else case when OVPM.TrsfrSum <> 0 then 'Transferência bancária'
		   else case when OVPM.CreditSum <> 0 then 'Cartão de crédito'
		   end end end end as PaymentType
	  from VPM2
	 inner join OVPM on OVPM.DocEntry = VPM2.DocNum
	 where OVPM.BoeSum = 0
		
	 union all
	 
	-- Boleto a pagar
	select OVPM.DocEntry as PmntEntry, 'OVPM' as PmntTable, OJDT.RefDate as PaymentDate, OVPM.Canceled,
		   VPM2.DocEntry, VPM2.InvType, case VPM2.InvType when '30' then VPM2.DocLine else VPM2.InstId end as InstId, 
		   VPM2.DocLine, VPM2.SumApplied - VPM2.WtAppld as SumApplied, VPM2.WtAppld, VPM2.DcntSum,
		   OBOE.BoeNum, OBOE.DueDate as BoeDueDate, OPYM.GLAccount as PmntAcct, 'Boleto' as PaymentType
	  from VPM2
	 inner join OVPM on OVPM.DocEntry = VPM2.DocNum
	 inner join OBOE on OBOE.BoeKey = OVPM.BoeAbs
	 inner join OPYM on OBOE.PayMethCod = OPYM.PayMethCod
	   and OBOE.BoeType = 'O' -- Outgoing
	 inner join OBOT on OBOT.AbsEntry = (select top(1) BOT1.[AbsEntry]
												 from [dbo].[BOT1]
												where BOT1.[BOENumber] = OBOE.[BoeNum] 
												  and BOT1.[BoeType] = OBOE.[BoeType]
												order by BOT1.[AbsEntry] desc)
	   and OBOT.StatusTo = 'P'
	   and OBOT.Reconciled = 'N'
	 inner join OJDT on OBOT.TransId = OJDT.TransId	
	 where OVPM.BoeAbs is not null
	   
	 union all
   
	-- Reconciliação
	select OITR.ReconNum as PmntEntry, 'OITR' as PmntTable, OITR.ReconDate as PaymentDate, OITR.Canceled,
		   ITR1.SrcObjAbs as DocEntry, ITR1.SrcObjTyp as InvType,
		   case ITR1.SrcObjTyp when '30' then ITR1.TransRowId else ITR1.TransRowId + 1 end as InstId, 
		   ITR1.TransRowId + 1 as DocLine, ITR1.ReconSum as SumApplied, 0.0 as WtAppld, 0.0 as DcntSum,
		   null as BoeNum, null as BoeDueDate, null as PmntAcct, 'Reconciliação' as PaymentType
	  from ITR1 
	 inner join OITR on OITR.ReconNum = ITR1.ReconNum 
	 where OITR.IsCard = 'C'
	   and OITR.ReconType in (0)
	   
	  union all
   
	-- Adiantamento
	select OITR.ReconNum as PmntEntry, 'OITR' as PmntTable, OITR.ReconDate as PaymentDate, NULL as PmntAcct,
		   ITR1.SrcObjAbs as DocEntry, ITR1.SrcObjTyp as InvType,
		   case ITR1.SrcObjTyp when '30' then ITR1.TransRowId else ITR1.TransRowId + 1 end as InstId, 
		   ITR1.TransRowId + 1 as DocLine, ITR1.ReconSum as SumApplied, 0.0 as WtAppld, 0.0 as DcntSum,
		   null as BoeNum, null as BoeDueDate, null as PmntAcct, 'Adiantamento' as PaymentType
	  from ITR1 
	 inner join OITR on OITR.ReconNum = ITR1.ReconNum 
	 where OITR.IsCard = 'A'
	   and OITR.Canceled = 'N' 
	   and OITR.ReconType in (16)	   