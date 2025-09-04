IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spc_CVA_ContasRecebidasPorCliente')
	DROP PROCEDURE spc_CVA_ContasRecebidasPorCliente
GO
CREATE PROC [dbo].[spc_CVA_ContasRecebidasPorCliente] (
	@cardcode varchar(30),
	@dateini datetime,
	@datefim datetime,
	@tpData varchar(2),
	@grupoEconomico nvarchar(100),
	@contaBancaria int,
	@validaDiaUtil		varchar(1)
)

as 

Begin
IF OBJECT_ID('tempdb..#ret') IS NOT NULL drop table #ret
IF OBJECT_ID('tempdb..#o') IS NOT NULL drop table #o

if rtrim(@cardcode) in ('', '*') set @cardcode = null
--if @serial   = 0  set @serial = null

create table #ret (
  operacao       varchar(100)   null
, objtype        varchar(72)    null
, metodo         varchar(100)   null
, nfserial       int            null
, nfentry        int            null
, parcela        int            null
, crentry        int            null
, emissao        smalldatetime  null
, Codigo         varchar(30)    null
, Parceiro       varchar(200)   null
, valordocumento decimal(19, 9) null
, valorparcela   decimal(19, 9) null
, vencimento     smalldatetime  null
, liquidacao     smalldatetime  null
, valorpago      decimal(19, 9) null
, transacao      varchar(100)   null
, jrnlmemo       varchar(max)   null
, transid        int            null
, boenum         int            null
, boemeth        varchar(200)   null
, ordem          int        not null identity
, Juros        decimal(19, 9)            null
, formaPagamento        nvarchar(200)    null
)

create table #o (
  operacao       varchar(100)   null
, objtype        varchar(72)    null
, metodo         varchar(200)   null
, parcela        int            null
, crentry        int            null
, Codigo         varchar(30)    null
, Parceiro       varchar(200)   null
, valorparcela   decimal(19, 9) null
, vencimento     smalldatetime  null
, liquidacao     smalldatetime  null
, valorpago      decimal(19, 9) null
, transacao      varchar(1000)   null
, jrnlmemo       varchar(max)   null
, transid        int            null
, boenum         int            null
, boemeth        varchar(200)   null
, notas          varchar(200)   null
, Juros        decimal(19, 9)            null
, formaPagamento        nvarchar(200)    null
)
insert into #ret (operacao, objtype, metodo, nfserial, nfentry, parcela, crentry, emissao, Codigo, Parceiro, valordocumento 
                , valorparcela, vencimento, liquidacao, valorpago, transacao, jrnlmemo, transid, boenum, boemeth,Juros,formaPagamento)
  -- Adiantamentos de clientes
  select 'ADTO - Boleto bancário' as 'Operacao'
       , odpi.ObjType as 'ObjType'
       , 'Boleto' as 'Metodo'
       , odpi.Serial as 'NFSerial'
       , odpi.DocEntry as 'NFEntry'
       , rct2.InstId as 'Parcela'
       , orct.DocEntry as 'CREntry'
       , odpi.DocDate as 'Emissao'
       , odpi.CardCode as 'Codigo'
       , ocrd.CardName as 'Parceiro'
       , odpi.DocTotal as 'ValorDocumento'
       , rct2.SumApplied as 'ValorParcela'
       , CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](oboe.DueDate)) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](oboe.DueDate)) ELSE oboe.DueDate END as 'Vencimento'
       , oboe.PmntDate as 'Liquidacao'
       , oboe.BoeSum as 'ValorPago'
       , 'Boleto ' + rtrim(oboe.BoeNum) as 'Transacao'
       , orct.JrnlMemo
       , orct.TransId
       , oboe.BoeNum as 'BoeNum'
       , oboe.PymMethNam as 'BoeMeth'
       , 0 as 'Juros'
	   , OBOE.PayMethCod
    from Odpi
   inner join RCT2
      on odpi.ObjType = rct2.invType
     and odpi.DocEntry = rct2.DocEntry
   inner join ORCT
      on orct.docentry = rct2.DocNum
   inner join OBOE
      on orct.BoeNum = oboe.BoeNum
   inner join OCRD
      on odpi.CardCode = ocrd.CardCode
   where orct.Canceled = 'N'
     and oboe.BoeStatus = 'P'
     and orct.BoeSum > 0
     and odpi.CardCode = ISNULL(@cardcode, odpi.cardcode)
	 and (ISNULL(@grupoEconomico, '*') = '*' OR  OCRD.U_CVA_GRUPOECON = @grupoEconomico)
     --and isnull(odpi.Serial, 0) = ISNULL(@serial, isnull(odpi.serial, 0))
     --and oboe.DueDate between ISNULL(@duedateini, oboe.duedate) and ISNULL(@duedatefim, oboe.duedate)
     --and oboe.PmntDate between ISNULL(@paydateini, oboe.pmntdate) and ISNULL(@paydatefim, oboe.pmntdate)
  union all
  select 'ADTO - Transferência bancária' as 'operacao'
       , odpi.ObjType as 'ObjType'
       , 'Transferência' as 'PayMethod'
       , odpi.Serial
       , odpi.DocEntry
       , rct2.InstId
       , orct.DocEntry
       , odpi.DocDate
       , odpi.CardCode
       , OCRD.CardName
       , odpi.DocTotal
       , rct2.SumApplied as 'ValorParcela'
	   , CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](orct.DocDueDate)) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](orct.DocDueDate)) ELSE orct.DocDueDate END as 'Vencimento'
       , orct.TrsfrDate as 'Liquidacao'
       , orct.TrsfrSum as 'ValorPago'
       , orct.TrsfrRef as 'Transacao'
       , orct.JrnlMemo
       , orct.TransId
       , null as 'BoeNum'
       , '' as 'BoeMeth'
       , 0 as 'Juros'
	   , ODPI.PeyMethod
    from Odpi
   inner join RCT2
      on odpi.ObjType = rct2.invType
     and odpi.DocEntry = rct2.DocEntry
   inner join ORCT
      on orct.docentry = rct2.DocNum
   inner join OCRD
      on odpi.CardCode = ocrd.CardCode
   where orct.Canceled = 'N'
     and orct.TrsfrSum > 0
     and odpi.CardCode = ISNULL(@cardcode, odpi.cardcode)
	 and (ISNULL(@grupoEconomico, '*') = '*' OR  OCRD.U_CVA_GRUPOECON = @grupoEconomico)
     --and isnull(odpi.Serial, 0) = ISNULL(@serial, isnull(odpi.serial, 0))
     --and orct.DocDueDate between ISNULL(@duedateini, orct.DocDueDate) and ISNULL(@duedatefim, orct.DocDueDate)
     --and orct.TrsfrDate between ISNULL(@paydateini, orct.TrsfrDate) and ISNULL(@paydatefim, orct.TrsfrDate)
        
  union all

  select 'ADTO - Dinheiro' as 'operacao'
       , odpi.ObjType as 'ObjType'
       , 'Dinheiro' as 'PayMethod'
       , odpi.Serial
       , odpi.DocEntry
       , rct2.InstId
       , orct.DocEntry
       , odpi.DocDate
       , odpi.CardCode
       , ocrd.CardName
       , odpi.DocTotal
       , rct2.SumApplied as 'ValorParcela'
       , CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](orct.DocDueDate)) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](orct.DocDueDate)) ELSE orct.DocDueDate END as 'Vencimento'
       , orct.DocDate as 'Liquidacao'
       , orct.CashSum as 'ValorPago'
       , null  as 'Transacao'
       , orct.JrnlMemo
       , orct.TransId
       , null as 'BoeNum'
       , '' as 'BoeMeth'
       , 0 as 'Juros'
	   , ODPI.PeyMethod
    from Odpi
   inner join RCT2
      on odpi.ObjType = rct2.invType
     and odpi.DocEntry = rct2.DocEntry
   inner join ORCT
      on orct.docentry = rct2.DocNum
   inner join OCRD
      on odpi.CardCode = ocrd.CardCode
   where orct.Canceled = 'N'
     and orct.CashSum > 0
     and odpi.CardCode = ISNULL(@cardcode, odpi.cardcode)
	 and (ISNULL(@grupoEconomico, '*') = '*' OR  OCRD.U_CVA_GRUPOECON = @grupoEconomico)
     --and isnull(odpi.Serial, 0) = ISNULL(@serial, isnull(odpi.serial, 0))
     --and orct.DocDueDate between ISNULL(@duedateini, orct.DocDueDate) and ISNULL(@duedatefim, orct.DocDueDate)
     --and orct.DocDate between ISNULL(@paydateini, orct.DocDate) and ISNULL(@paydatefim, orct.DocDate)
        
  union all

  select 'ADTO - Cheque' as 'operacao'
       , odpi.ObjType as 'ObjType'
       , 'Cheque' as 'PayMethod'
       , odpi.Serial
       , odpi.DocEntry
       , rct2.InstId
       , orct.DocEntry
       , odpi.DocDate
       , odpi.CardCode
       , ocrd.CardName
       , odpi.DocTotal
       , rct2.SumApplied as 'ValorParcela'
       , CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](orct.DocDueDate)) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](orct.DocDueDate)) ELSE orct.DocDueDate END as 'Vencimento'
       , orct.DocDate as 'Liquidacao'
       , orct.CheckSum as 'ValorPago'
       , null  as 'Transacao'
       , orct.JrnlMemo
       , orct.TransId
       , null as 'BoeNum'
       , '' as 'BoeMeth'
       , 0 as 'Juros'
	   , ODPI.PeyMethod
    from Odpi
   inner join RCT2
      on odpi.ObjType = rct2.invType
     and odpi.DocEntry = rct2.DocEntry
   inner join ORCT
      on orct.docentry = rct2.DocNum
   inner join OCRD
      on odpi.CardCode = ocrd.CardCode
   where orct.Canceled = 'N'
     and orct.CheckSum > 0
     and odpi.CardCode = ISNULL(@cardcode, odpi.cardcode)
	 and (ISNULL(@grupoEconomico, '*') = '*' OR  OCRD.U_CVA_GRUPOECON = @grupoEconomico)
     --and isnull(odpi.Serial, 0) = ISNULL(@serial, isnull(odpi.serial, 0))
     --and orct.DocDueDate between ISNULL(@duedateini, orct.DocDueDate) and ISNULL(@duedatefim, orct.DocDueDate)
     --and orct.DocDate between ISNULL(@paydateini, orct.DocDate) and ISNULL(@paydatefim, orct.DocDate)
     
  union all

  -- Notas fiscais

  select 'NF - Boleto bancário' as 'operacao'
       , oinv.ObjType as 'ObjType'
       , 'Boleto' as 'PayMethod'
       , oinv.Serial
       , oinv.DocEntry
       , rct2.InstId
       , orct.DocEntry
       , oinv.DocDate
       , oinv.CardCode
       , ocrd.CardName
       , oinv.DocTotal
       , rct2.SumApplied as 'ValorParcela'
	   , CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](oboe.DueDate)) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](oboe.DueDate)) ELSE oboe.DueDate END as 'Vencimento'
       --, oboe.PmntDate as 'Liquidacao'
		,(SELECT 
			max(T1.[TranDate] )
		FROM  
			[dbo].[BOT1] T0  
			INNER  JOIN [dbo].[OBOT] T1  ON  T1.[AbsEntry] = T0.[AbsEntry]   
		WHERE 
			T0.[BOENumber] = oboe.BoeNum  
			and T1.[StatusTo]='P') as 'Liquidacao' --JBC       
       , oboe.BoeSum as 'ValorPago'
       , 'Boleto ' + rtrim(oboe.BoeNum) as 'Transacao'
       , orct.JrnlMemo
       , orct.TransId
       , oboe.BoeNum as 'BoeNum'
       , oboe.PymMethNam as 'BoeMeth'
       , 0 as 'Juros'
	   , oboe.PayMethCod
    from OINV
   inner join RCT2
      on oinv.ObjType = rct2.InvType
     and oinv.DocEntry = rct2.DocEntry
   inner join ORCT
      on orct.docentry = rct2.DocNum
   inner join OBOE
      on orct.BoeNum = oboe.BoeNum
   inner join OCRD
      on oinv.CardCode = ocrd.CardCode
   where orct.Canceled = 'N'
     and oboe.BoeStatus = 'P'
     and orct.BoeSum > 0
     and oinv.CardCode = ISNULL(@cardcode, oinv.cardcode)
	 and (ISNULL(@grupoEconomico, '*') = '*' OR  OCRD.U_CVA_GRUPOECON = @grupoEconomico)
     --and isnull(oinv.Serial, 0) = ISNULL(@serial, isnull(oinv.serial, 0))
     --and oboe.DueDate between ISNULL(@duedateini, oboe.duedate) and ISNULL(@duedatefim, oboe.duedate)
     --and oboe.PmntDate between ISNULL(@paydateini, oboe.pmntdate) and ISNULL(@paydatefim, oboe.pmntdate)
     
  union all

  select 'NF - Transferência bancária' as 'operacao'
       , oinv.ObjType as 'ObjType'
       , 'Transferência' as 'PayMethod'
       , oinv.Serial
       , oinv.DocEntry
       , rct2.InstId
       , orct.DocEntry
       , oinv.DocDate
       , oinv.CardCode
       , ocrd.CardName
       , oinv.DocTotal
       , rct2.SumApplied as 'ValorParcela'
       --, OINV.DocDueDate as 'Vencimento'
       --, orct.DocDueDate as 'Vencimento'--jbc
       ,(
			select CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](INV6.DueDate)) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](INV6.DueDate)) ELSE INV6.DueDate END 
			from INV6 where INV6.DocEntry = oinv.DocEntry and INV6.InstlmntID=rct2.InstId 
		) as 'Vencimento'
       , orct.TrsfrDate as 'Liquidacao'
       --, orct.TrsfrSum as 'ValorPago' JBC
       , rct2.SumApplied as 'ValorPago'
       , orct.TrsfrRef as 'Transacao'
       , orct.JrnlMemo
       , orct.TransId
       , null as 'BoeNum'
       , '' as 'BoeMeth'
       , 0 as 'Juros'
	   , OINV.PeyMethod
    from OINV
   inner join RCT2
      on oinv.ObjType = rct2.InvType
     and oinv.DocEntry = rct2.DocEntry
   inner join ORCT
      on orct.docentry = rct2.DocNum
   inner join OCRD
      on oinv.CardCode = ocrd.CardCode
   where orct.Canceled = 'N'
     and orct.TrsfrSum > 0
     and oinv.CardCode = ISNULL(@cardcode, oinv.cardcode)
	 and (ISNULL(@grupoEconomico, '*') = '*' OR  OCRD.U_CVA_GRUPOECON = @grupoEconomico)
     --and isnull(oinv.Serial, 0) = ISNULL(@serial, isnull(oinv.serial, 0))
     --and ORCT.TrsfrAcct<>'4.2.01.02.0012'
     --and orct.DocDueDate between ISNULL(@duedateini, orct.DocDueDate) and ISNULL(@duedatefim, orct.DocDueDate)
     --and orct.TrsfrDate between ISNULL(@paydateini, orct.TrsfrDate) and ISNULL(@paydatefim, orct.TrsfrDate)
  union all

  --select 'NF - Desconto Concedido' as 'operacao'
  --     , oinv.ObjType as 'ObjType'
  --     , 'Transferência' as 'PayMethod'
  --     , oinv.Serial
  --     , oinv.DocEntry
  --     , rct2.InstId
  --     , orct.DocEntry
  --     , oinv.DocDate
  --     , oinv.CardCode
  --     , ocrd.CardName
  --     , oinv.DocTotal
  --     , rct2.SumApplied as 'ValorParcela'
  --     , OINV.DocDueDate as 'Vencimento'
  --     --, orct.DocDueDate as 'Vencimento'--jbc
  --     , orct.TrsfrDate as 'Liquidacao'
  --     --, orct.TrsfrSum as 'ValorPago' JBC
  --     , rct2.SumApplied as 'ValorPago'
  --     , orct.TrsfrRef as 'Transacao'
  --     , orct.JrnlMemo
  --     , orct.TransId
  --     , null as 'BoeNum'
  --     , '' as 'BoeMeth'
  --     , (select sum(SysTotal) SysTotal from OJDT where TransId=orct.U_TransId_Juros) as 'Juros'
  --  from OINV
  -- inner join RCT2
  --    on oinv.ObjType = rct2.InvType
  --   and oinv.DocEntry = rct2.DocEntry
  -- inner join ORCT
  --    on orct.docentry = rct2.DocNum
  -- inner join OCRD
  --    on oinv.CardCode = ocrd.CardCode
  -- where orct.Canceled = 'N'
  --   and orct.TrsfrSum > 0
  --   and oinv.CardCode = ISNULL(@cardcode, oinv.cardcode)
  --   --and isnull(oinv.Serial, 0) = ISNULL(@serial, isnull(oinv.serial, 0))
  --   --and ORCT.TrsfrAcct='4.2.01.02.0012'     
  --union all

  select 'NF - Dinheiro' as 'operacao'
       , oinv.ObjType as 'ObjType'
       , 'Dinheiro' as 'PayMethod'
       , oinv.Serial
       , oinv.DocEntry
       , rct2.InstId
       , orct.DocEntry
       , oinv.DocDate
       , oinv.CardCode
       , ocrd.CardName
       , oinv.DocTotal
       , rct2.SumApplied as 'ValorParcela'
       , CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](orct.DocDueDate)) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](orct.DocDueDate)) ELSE orct.DocDueDate END as 'Vencimento'
       , orct.DocDate as 'Liquidacao'
       , orct.CashSum as 'ValorPago'
       , null  as 'Transacao'
       , orct.JrnlMemo
       , orct.TransId
       , null as 'BoeNum'
       , '' as 'BoeMeth'
       , 0 as 'Juros'
	   , OINV.PeyMethod
    from OINV
   inner join RCT2
      on oinv.ObjType = rct2.InvType
     and oinv.DocEntry = rct2.DocEntry
   inner join ORCT
      on orct.docentry = rct2.DocNum
   inner join OCRD
      on oinv.CardCode = ocrd.CardCode
   where orct.Canceled = 'N'
     and orct.CashSum > 0
     and oinv.CardCode = ISNULL(@cardcode, oinv.cardcode)
	 and (ISNULL(@grupoEconomico, '*') = '*' OR  OCRD.U_CVA_GRUPOECON = @grupoEconomico)
     --and isnull(oinv.Serial, 0) = ISNULL(@serial, isnull(oinv.serial, 0))
     --and orct.DocDueDate between ISNULL(@duedateini, orct.DocDueDate) and ISNULL(@duedatefim, orct.DocDueDate)
     --and orct.DocDate between ISNULL(@paydateini, orct.DocDate) and ISNULL(@paydatefim, orct.DocDate)
        
  union all

  select 'NF - Cheque' as 'operacao'
       , oinv.ObjType as 'ObjType'
       , 'Cheque' as 'PayMethod'
       , oinv.Serial
       , oinv.DocEntry
       , rct2.InstId
       , orct.DocEntry
       , oinv.DocDate
       , oinv.CardCode
       , ocrd.CardName
       , oinv.DocTotal
       , rct2.SumApplied as 'ValorParcela'
       , CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](orct.DocDueDate)) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](orct.DocDueDate)) ELSE orct.DocDueDate END as 'Vencimento'
       , orct.DocDate as 'Liquidacao'
       , orct.CheckSum as 'ValorPago'
       , null  as 'Transacao'
       , orct.JrnlMemo
       , orct.TransId
       , null as 'BoeNum'
       , '' as 'BoeMeth'
       , 0 as 'Juros'
	   , OINV.PeyMethod
    from OINV
   inner join RCT2
      on oinv.ObjType = rct2.InvType
     and oinv.DocEntry = rct2.DocEntry
   inner join ORCT
      on orct.docentry = rct2.DocNum
   inner join OCRD
      on oinv.CardCode = ocrd.CardCode
   where orct.Canceled = 'N'
     and orct.CheckSum > 0
     and oinv.CardCode = ISNULL(@cardcode, oinv.cardcode)
	 and (ISNULL(@grupoEconomico, '*') = '*' OR  OCRD.U_CVA_GRUPOECON = @grupoEconomico)
     --and isnull(oinv.Serial, 0) = ISNULL(@serial, isnull(oinv.serial, 0))
     --and orct.DocDueDate between ISNULL(@duedateini, orct.DocDueDate) and ISNULL(@duedatefim, orct.DocDueDate)
     --and orct.DocDate between ISNULL(@paydateini, orct.DocDate) and ISNULL(@paydatefim, orct.DocDate)
     
  union all

  select distinct 
         'LCM - Boleto bancário' as 'operacao'
       , ojdt.ObjType as 'ObjType'
       , 'Boleto' as 'PayMethod'
       , jdt1.TransId
       , jdt1.BaseRef
       , rct2.InstId
       , orct.DocEntry
       , jdt1.TaxDate
       , jdt1.ShortName
       , ocrd.CardName
       , jdt1.Debit
       , rct2.SumApplied as 'ValorParcela'
	   , CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](oboe.DueDate)) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](oboe.DueDate)) ELSE oboe.DueDate END as 'Vencimento'
       , oboe.PmntDate as 'Liquidacao'
       , oboe.BoeSum as 'ValorPago'
       , 'Boleto ' + rtrim(oboe.BoeNum) as 'Transacao'
       , orct.JrnlMemo
       , orct.TransId
       , oboe.BoeNum as 'BoeNum'
       , oboe.PymMethNam as 'BoeMeth'
       , 0 as 'Juros'
	   , OBOE.PayMethCod
    from OJDT
   inner join jdt1
      on ojdt.TransId = jdt1.TransId
   inner join RCT2
      on ojdt.ObjType = rct2.InvType
     and ojdt.TransId = rct2.DocEntry
   inner join ORCT
      on orct.docentry = rct2.DocNum
   inner join OBOE
      on orct.BoeNum = oboe.BoeNum
   inner join OCRD
      on jdt1.ShortName = OCRD.CardCode
   where orct.Canceled = 'N'
     and oboe.BoeStatus = 'P'
     and orct.BoeSum > 0
     and ocrd.CardCode = ISNULL(@cardcode, ocrd.CardCode)
	 and (ISNULL(@grupoEconomico, '*') = '*' OR  OCRD.U_CVA_GRUPOECON = @grupoEconomico)
     --and oboe.DueDate between ISNULL(@duedateini, oboe.duedate) and ISNULL(@duedatefim, oboe.duedate)
     --and oboe.PmntDate between ISNULL(@paydateini, oboe.pmntdate) and ISNULL(@paydatefim, oboe.pmntdate)
     
  union all

  select distinct 
         'LCM - Transferência bancária' as 'operacao'
       , ojdt.ObjType as 'ObjType'
       , 'Transferência' as 'PayMethod'
       , jdt1.TransId
       , jdt1.BaseRef
       , rct2.InstId
       , orct.DocEntry
       , jdt1.TaxDate
       , jdt1.ShortName
       , ocrd.CardName
       , jdt1.Debit
       , rct2.SumApplied as 'ValorParcela'
       , CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](orct.DocDueDate)) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](orct.DocDueDate)) ELSE orct.DocDueDate END as 'Vencimento'
       , orct.TrsfrDate as 'Liquidacao'
       --, orct.TrsfrSum as 'ValorPago' -- JBC
       , rct2.SumApplied as 'ValorPago'       
       , orct.TrsfrRef as 'Transacao'
       , orct.JrnlMemo
       , orct.TransId
       , null as 'BoeNum'
       , '' as 'BoeMeth'
       , 0 as 'Juros'
	   , ''
    from OJDT
   inner join jdt1
      on ojdt.TransId = jdt1.TransId
   inner join RCT2
      on ojdt.ObjType = rct2.InvType
     and ojdt.TransId = rct2.DocEntry
   inner join ORCT
      on orct.docentry = rct2.DocNum
   inner join OCRD
      on jdt1.ShortName = OCRD.CardCode
   where orct.Canceled = 'N'
     and orct.TrsfrSum > 0
     and ocrd.CardCode = ISNULL(@cardcode, ocrd.CardCode)
	 and (ISNULL(@grupoEconomico, '*') = '*' OR  OCRD.U_CVA_GRUPOECON = @grupoEconomico)
     --and orct.DocDueDate between ISNULL(@duedateini, orct.DocDueDate) and ISNULL(@duedatefim, orct.DocDueDate)
     --and orct.TrsfrDate between ISNULL(@paydateini, orct.TrsfrDate) and ISNULL(@paydatefim, orct.TrsfrDate)

  union all

  select distinct 
         'LCM - Dinheiro' as 'operacao'
       , ojdt.ObjType as 'ObjType'
       , 'Dinheiro' as 'PayMethod'
       , jdt1.TransId
       , jdt1.BaseRef
       , rct2.InstId
       , orct.DocEntry
       , jdt1.TaxDate
       , jdt1.ShortName
       , ocrd.CardName
       , jdt1.Debit
       , rct2.SumApplied as 'ValorParcela'
       , CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](orct.DocDueDate)) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](orct.DocDueDate)) ELSE orct.DocDueDate END as 'Vencimento'
       , orct.DocDate as 'Liquidacao'
       , orct.CashSum as 'ValorPago'
       , null  as 'Transacao'
       , orct.JrnlMemo
       , orct.TransId
       , null as 'BoeNum'
       , '' as 'BoeMeth'
       , 0 as 'Juros'
	   , ''
    from OJDT
   inner join jdt1
      on ojdt.TransId = jdt1.TransId
   inner join RCT2
      on ojdt.ObjType = rct2.InvType
     and ojdt.TransId = rct2.DocEntry
   inner join ORCT
      on orct.docentry = rct2.DocNum
   inner join OCRD
      on jdt1.ShortName = OCRD.CardCode
   where orct.Canceled = 'N'
     and orct.CashSum > 0
     and ocrd.CardCode = ISNULL(@cardcode, ocrd.CardCode)
	 and (ISNULL(@grupoEconomico, '*') = '*' OR  OCRD.U_CVA_GRUPOECON = @grupoEconomico)
     --and orct.DocDueDate between ISNULL(@duedateini, orct.DocDueDate) and ISNULL(@duedatefim, orct.DocDueDate)
     --and orct.DocDate between ISNULL(@paydateini, orct.DocDate) and ISNULL(@paydatefim, orct.DocDate)
     
  union all

  select distinct 
         'LCM - Cheque' as 'operacao'
       , ojdt.ObjType as 'ObjType'
       , 'Boleto' as 'PayMethod'
       , jdt1.TransId
       , jdt1.BaseRef
       , rct2.InstId
       , orct.DocEntry
       , jdt1.TaxDate
       , jdt1.ShortName
       , ocrd.CardName
       , jdt1.Debit
       , rct2.SumApplied as 'ValorParcela'
       , CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](orct.DocDueDate)) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](orct.DocDueDate)) ELSE orct.DocDueDate END as 'Vencimento'
       , orct.DocDate as 'Liquidacao'
       , orct.CheckSum as 'ValorPago'
       , null  as 'Transacao'
       
       , orct.JrnlMemo
       , orct.TransId

       , null as 'BoeNum'
       , '' as 'BoeMeth'
       , 0 as 'Juros'
	   , ''

    from OJDT
   inner join jdt1
      on ojdt.TransId = jdt1.TransId
   inner join RCT2
      on ojdt.ObjType = rct2.InvType
     and ojdt.TransId = rct2.DocEntry
   inner join ORCT
      on orct.docentry = rct2.DocNum
   inner join OCRD
      on jdt1.ShortName = OCRD.CardCode
   where orct.Canceled = 'N'
     and orct.CheckSum > 0
     and ocrd.CardCode = ISNULL(@cardcode, ocrd.CardCode)
	 and (ISNULL(@grupoEconomico, '*') = '*' OR  OCRD.U_CVA_GRUPOECON = @grupoEconomico)
     --and orct.DocDueDate between ISNULL(@duedateini, orct.DocDueDate) and ISNULL(@duedatefim, orct.DocDueDate)
     --and orct.DocDate between ISNULL(@paydateini, orct.DocDate) and ISNULL(@paydatefim, orct.DocDate)
     
   order by 4, 5, 6
 
 declare @voperacao       varchar(100)
 declare @vobjtype        varchar(72)
 declare @vmetodo         varchar(100)
 declare @vcrentry        int
 declare @vparcela        int
 declare @vvencimento     smalldatetime
 declare @vvalorparcela   decimal(19, 9)
 declare @vliquidacao     smalldatetime
 declare @vcodigo         varchar(30)
 declare @vparceiro       varchar(200)
 declare @vvalorpago      decimal(19, 9)
 declare @vtransacao      varchar(200)
 declare @vjrnlmemo       varchar(max)
 declare @vtransid        int
 declare @vboenum         int
 declare @vboemeth        varchar(200)
 declare @notas           varchar(max)
 declare @vnfserial       int
 declare @vnfentry        int
 declare @vemissao        smalldatetime
 declare @vvalordocumento decimal(19, 9)
 declare @Juros decimal(19, 9)
 declare @formaPagamento nvarchar(200)
 

 
 declare cp1 cursor local fast_forward read_only for
   select operacao, objtype, metodo, crentry, parcela, vencimento, liquidacao, sum(valorparcela), codigo, parceiro, valorpago, transacao, jrnlmemo, transid, boenum, boemeth,Juros,formaPagamento
     from #ret
    group by operacao, objtype, metodo, crentry, parcela, vencimento, liquidacao, codigo, parceiro, valorpago, transacao, jrnlmemo, transid, boenum, boemeth,Juros,formaPagamento

 open cp1
 while 1 = 1
 begin
  fetch next from cp1 into @voperacao, @vobjtype, @vmetodo, @vcrentry, @vparcela, @vvencimento, @vliquidacao, @vvalorparcela, @vcodigo, @vparceiro, @vvalorpago, @vtransacao, @vjrnlmemo, @vtransid, @vboenum, @vboemeth,@Juros,@formaPagamento
  if @@FETCH_STATUS <> 0 break
 
  set @notas = ''
  
  declare cp2 cursor local fast_forward read_only for
    select nfserial, nfentry, emissao, valordocumento
      from #ret
     where crentry = @vcrentry
       and objtype = '13'
     
  open cp2
  
  while 1 = 1
  begin
    fetch next from cp2 into @vnfserial, @vnfentry, @vemissao, @vvalordocumento
    if @@FETCH_STATUS <> 0 break
    
    if @notas = ''
      set @notas = rtrim(@vnfserial) --+ ' (' + CONVERT(char(10), @vemissao, 103) + ')'
    else
      set @notas = @notas + ', ' + rtrim(@vnfserial) --+ ' (' + CONVERT(char(10), @vemissao, 103) + ')'
    
  end
  close cp2
  deallocate cp2
    
  insert into #o (operacao, objtype, metodo, parcela, crentry, Codigo, parceiro, valorparcela
                , vencimento, liquidacao, valorpago, transacao, jrnlmemo, transid, boenum
                , boemeth, notas,Juros, formaPagamento)    
    select @voperacao, @vobjtype, @vmetodo, @vparcela, @vcrentry, @vCodigo, @vparceiro, @vvalorparcela
         , @vvencimento, @vliquidacao, @vvalorpago, @vtransacao, @vjrnlmemo, @vtransid, @vboenum
         , @vboemeth, @notas,coalesce(@Juros,0),@formaPagamento
  
end
close cp1
deallocate cp1
 
select #o.* from #o
	LEFT JOIN OPYM
		ON #o.formaPagamento = OPYM.PayMethCod collate DATABASE_DEFAULT
	LEFT JOIN DSC1
		ON DSC1.BankCode = OPYM.BnkDflt
		AND DSC1.Account = OPYM.DflAccount
		AND DSC1.Branch = OPYM.Branch
where  
 (
	((Vencimento between ISNULL(@dateini, Vencimento) and ISNULL(@datefim, Vencimento))  and @tpData='V')
	or
	((liquidacao between ISNULL(@dateini, liquidacao) and ISNULL(@datefim, liquidacao))and @tpData='L' )
 )
 and
 (ISNULL(@contaBancaria, 0) = 0 OR DSC1.AbsEntry = @contaBancaria)

--or
--	(ISNULL(@dateini, liquidacao)<>ISNULL(@dateini, liquidacao) and @tpData<>'L' and @tpData<>'V')--opção invalida não traz nada

--@tpData='L'
--@tpData='V'

order by transid
     
drop table #ret
drop table #o

end
GO