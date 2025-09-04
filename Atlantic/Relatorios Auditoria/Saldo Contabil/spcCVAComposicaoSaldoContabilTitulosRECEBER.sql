USE SBO_PRD_CFI0002
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcCVAComposicaoSaldoContabilTitulosRECEBER]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].spcCVAComposicaoSaldoContabilTitulosRECEBER
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE spcCVAComposicaoSaldoContabilTitulosRECEBER
	@DataIni as DAte
	,@DataFIM as DAte
	,@CArdCode as nvarchar(max)
	,@CArdRazaoSocial as nvarchar(max)
	,@CArdNomeFantasia as nvarchar(max)
	,@CNPJ as nvarchar(max)
	,@BplId int	
	--,@TIPOCONTA AS NVARCHAR(20)
	,@SALDO AS NVARCHAR(20)
AS 
BEGIN
--select top 1 ObjType  from ODPI		
--203 A/R Down Payment 
	SELECT 
		TPMOV
		,C
		,DOC 
		,TP
		,upper([FILIAL DE ORIGEM]) [FILIAL DE ORIGEM]
		,[EMISSÃO]
		,VENCIMENTO
		,MOVIMENTO
		,PARCELA PARC
		,PRINCIPAL
		,[VALOR PAGO]
		, CASE WHEN  PRINCIPAL>[VALOR PAGO]  THEN PRINCIPAL-[VALOR PAGO]  ELSE 0 END AS 'PENDENTE'
		, CASE WHEN  PRINCIPAL<[VALOR PAGO]  THEN (PRINCIPAL-[VALOR PAGO])*-1  ELSE 0 END AS 'JUROS'
		,UPPER([CONTA CONTÁBEIL]) [CONTA CONTÁBEIL]
		, UPPER(CardCode)CardCode
		,UPPER(Parceiro)Parceiro
	FROM (
	select 
		'RECEBER' as 'TPMOV' 
		,T0.DocEntry
		,T0.DocNum  'C'
		,T0.ObjType
		,T2.DocNum DOC
		,'ADIANTAMENTO DE CLIENTE'  as 'TP'
		--, T0.BPLId
		, upper(T0.BPLName) 'FILIAL DE ORIGEM'		
		
		, T0.DocDate 'EMISSÃO'
		, T3.DueDate 'VENCIMENTO'	
		, T0.TaxDate 'MOVIMENTO'
		, coalesce(cast(T1.InstId as nvarchar(10)),'1') +'/'+ coalesce(cast(t2.Installmnt as nvarchar(10)),'1')  as 'PARCELA'
		
		, T1.SumApplied as 'VALOR PAGO'
		, T3.InsTotal 'PRINCIPAL'
		
		
		
		,upper((case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end + ' - '+T5.AcctName )) 'CONTA CONTÁBEIL'						
		--,case 
 	--		when T0.CashSum>0 then T0.CashSum
		--	when T0.boeSum>0 then T0.boeSum
		--	when T0.TrsfrSum>0 then  T0.TrsfrSum
		--	when T0.CreditSum>0 then T0.CreditSum
		--	when T0.CheckSum>0 then  T0.CheckSum
		--end ValorTotal
		, T1.invType  ObjTypeoRIGEM
		, t1.DocEntry DocEntryORIGEM
		, T4.CardCode
		, upper((T4.CardCode + ' - ' + T4.CardName) ) Parceiro
		
	from 
		ORCT T0      
		inner join RCT2 T1 on T0.docentry = T1.DocNum
		inner join ODPI T2 on T2.ObjType = T1.invType	 and T2.DocEntry=T1.DocEntry
		inner join DPI6 T3 on T3.DocEntry = T2.DocEntry and T3.InstlmntID=T1.InstId
		inner join OCRD T4 on T4.CardCode = T0.CardCode
		inner join OACT T5 on T5.AcctCode=case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end
	where
		T0.CANCELED<>'Y' and T0.CANCELED<>'C'
		and T0.DocDate between @DataIni and @DataFIM	
		and (@CArdCode='*' or ( @CArdCode<>'*' and @CArdCode=T4.CardCode ) )
		and (@CArdRazaoSocial='*' or (@CArdRazaoSocial<>'*' and T4.CardName like '%'+ @CArdRazaoSocial +'%'))
		and (@CArdNomeFantasia='*' or (@CArdNomeFantasia<>'*' and T4.CardFName like '%'+ @CArdRazaoSocial +'%'))	
		and (@CNPJ='*' or exists ( select 1 from  CRD7 T6 where  T6.CardCode=T0.CardCode and (not T6.TaxId0 is null or not T6.TaxId4 is null) and coalesce(T6.TaxId0,T6.TaxId4)=@CNPJ  )    )
		and (@BplId=0 or @BplId=T0.BPLId)
		--AND (@TIPOCONTA='*' OR @TIPOCONTA='RECEBER' )
	UNION ALL
--select top 1 ObjType  from OINV		
--13 A/R Invoice 
	select 
		'RECEBER' as 'TPMov' 
		,T0.DocEntry
		,T0.DocNum 
		,T0.ObjType
		,T2.DocNum DOC
		,'NOTA FISCAL DE VENDA'  as 'TP'
		--, T0.BPLId
		, T0.BPLName 'FILIAL DE ORIGEM'		
		
		, T0.DocDate 'EMISSÃO'
		, T3.DueDate 'VENCIMENTO'	
		, T0.TaxDate 'MOVIMENTO'
		, coalesce(cast(T1.InstId as nvarchar(10)),'1') +'/'+ coalesce(cast(t2.Installmnt as nvarchar(10)),'1')  as 'PARCELA'
		
		, T1.SumApplied as 'VALOR PAGO'
		, T3.InsTotal 'PRINCIPAL'
		
		,(case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end + ' - '+T5.AcctName ) 'CONTA CONTÁBEIL'						
		--,case 
 	--		when T0.CashSum>0 then T0.CashSum
		--	when T0.boeSum>0 then T0.boeSum
		--	when T0.TrsfrSum>0 then  T0.TrsfrSum
		--	when T0.CreditSum>0 then T0.CreditSum
		--	when T0.CheckSum>0 then  T0.CheckSum
		--end ValorTotal
		, T1.invType  ObjTypeoRIGEM
		, t1.DocEntry DocEntryORIGEM
		, T4.CardCode
		, (T4.CardCode + ' - ' + T4.CardName)  Parceiro
	from 
		ORCT T0      
		inner join RCT2 T1 on T0.docentry = T1.DocNum
		inner join OINV T2 on T2.ObjType = T1.invType	 and T2.DocEntry=T1.DocEntry
		inner join INV6 T3 on T3.DocEntry = T2.DocEntry and T3.InstlmntID=T1.InstId
		inner join OCRD T4 on T4.CardCode = T0.CardCode
		inner join OACT T5 on T5.AcctCode=case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end
	where
		T0.CANCELED<>'Y' and T0.CANCELED<>'C'
		and T0.DocDate between @DataIni and @DataFIM	
		and (@CArdCode='*' or ( @CArdCode<>'*' and @CArdCode=T4.CardCode ) )
		and (@CArdRazaoSocial='*' or (@CArdRazaoSocial<>'*' and T4.CardName like '%'+ @CArdRazaoSocial +'%'))
		and (@CArdNomeFantasia='*' or (@CArdNomeFantasia<>'*' and T4.CardFName like '%'+ @CArdRazaoSocial +'%'))	
		and (@CNPJ='*' or exists ( select 1 from  CRD7 T6 where  T6.CardCode=T0.CardCode and (not T6.TaxId0 is null or not T6.TaxId4 is null) and coalesce(T6.TaxId0,T6.TaxId4)=@CNPJ  )    )
		and (@BplId=0 or @BplId=T0.BPLId)
		--AND (@TIPOCONTA='*' OR @TIPOCONTA='RECEBER' )
--select top 1 ObjType  from ORIN
--14 A/R Credit Memo 	
--SELECT * FROM RCT2 WHERE invType=14
	UNION ALL
	select 
		'RECEBER' as 'TPMov' 
		,T0.DocEntry
		,T0.DocNum 
		,T0.ObjType
		,T2.DocNum DOC
		,'DEV. NOTA FISCAL DE SAÍDA'  as 'TP'
		--, T0.BPLId
		, T0.BPLName 'FILIAL DE ORIGEM'		
		
		, T0.DocDate 'EMISSÃO'
		, T3.DueDate 'VENCIMENTO'	
		, T0.TaxDate 'MOVIMENTO'
		, coalesce(cast(T1.InstId as nvarchar(10)),'1') +'/'+ coalesce(cast(t2.Installmnt as nvarchar(10)),'1')  as 'PARCELA'
		
		, T1.SumApplied as 'VALOR PAGO'
		, T3.InsTotal 'PRINCIPAL'
		
		,(case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end + ' - '+T5.AcctName ) 'CONTA CONTÁBEIL'						
		--,case 
 	--		when T0.CashSum>0 then T0.CashSum
		--	when T0.boeSum>0 then T0.boeSum
		--	when T0.TrsfrSum>0 then  T0.TrsfrSum
		--	when T0.CreditSum>0 then T0.CreditSum
		--	when T0.CheckSum>0 then  T0.CheckSum
		--end ValorTotal
		, T1.invType  ObjTypeoRIGEM
		, t1.DocEntry DocEntryORIGEM
		, T4.CardCode
		, (T4.CardCode + ' - ' + T4.CardName)  Parceiro
	from 
		ORCT T0      
		inner join RCT2 T1 on T0.docentry = T1.DocNum
		inner join ORIN T2 on T2.ObjType = T1.invType	 and T2.DocEntry=T1.DocEntry
		inner join RIN6 T3 on T3.DocEntry = T2.DocEntry and T3.InstlmntID=T1.InstId
		inner join OCRD T4 on T4.CardCode = T0.CardCode
		inner join OACT T5 on T5.AcctCode=case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end
	where
		T0.CANCELED<>'Y' and T0.CANCELED<>'C'
		and T0.DocDate between @DataIni and @DataFIM	
		and (@CArdCode='*' or ( @CArdCode<>'*' and @CArdCode=T4.CardCode ) )
		and (@CArdRazaoSocial='*' or (@CArdRazaoSocial<>'*' and T4.CardName like '%'+ @CArdRazaoSocial +'%'))
		and (@CArdNomeFantasia='*' or (@CArdNomeFantasia<>'*' and T4.CardFName like '%'+ @CArdRazaoSocial +'%'))	
		and (@CNPJ='*' or exists ( select 1 from  CRD7 T6 where  T6.CardCode=T0.CardCode and (not T6.TaxId0 is null or not T6.TaxId4 is null) and coalesce(T6.TaxId0,T6.TaxId4)=@CNPJ  )    )
		and (@BplId=0 or @BplId=T0.BPLId)	
		--AND (@TIPOCONTA='*' OR @TIPOCONTA='RECEBER' )
--204 A/P Down Payment 
--select top 1 ObjType from ODPO
	UNION ALL
	select 
		'RECEBER' as 'TPMov' 
		,T0.DocEntry
		,T0.DocNum 
		,T0.ObjType
		,T2.DocNum DOC
		,'ADIANTAMENTO PARA FORNECEDOR'  as 'TP'
		--, T0.BPLId
		, T0.BPLName 'FILIAL DE ORIGEM'		
		
		, T0.DocDate 'EMISSÃO'
		, T3.DueDate 'VENCIMENTO'	
		, T0.TaxDate 'MOVIMENTO'
		, coalesce(cast(T1.InstId as nvarchar(10)),'1') +'/'+ coalesce(cast(t2.Installmnt as nvarchar(10)),'1')  as 'PARCELA'
		
		, T1.SumApplied as 'VALOR PAGO'
		, T3.InsTotal 'PRINCIPAL'
		
		,(case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end + ' - '+T5.AcctName ) 'CONTA CONTÁBEIL'						
		--,case 
 	--		when T0.CashSum>0 then T0.CashSum
		--	when T0.boeSum>0 then T0.boeSum
		--	when T0.TrsfrSum>0 then  T0.TrsfrSum
		--	when T0.CreditSum>0 then T0.CreditSum
		--	when T0.CheckSum>0 then  T0.CheckSum
		--end ValorTotal
		, T1.invType  ObjTypeoRIGEM
		, t1.DocEntry DocEntryORIGEM
		, T4.CardCode
		, (T4.CardCode + ' - ' + T4.CardName)  Parceiro
	from 
		ORCT T0      
		inner join RCT2 T1 on T0.docentry = T1.DocNum
		inner join ODPO T2 on T2.ObjType = T1.invType	 and T2.DocEntry=T1.DocEntry
		inner join DPO6 T3 on T3.DocEntry = T2.DocEntry and T3.InstlmntID=T1.InstId
		inner join OCRD T4 on T4.CardCode = T0.CardCode
		inner join OACT T5 on T5.AcctCode=case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end
	where
		T0.CANCELED<>'Y' and T0.CANCELED<>'C'
		and T0.DocDate between @DataIni and @DataFIM	
		and (@CArdCode='*' or ( @CArdCode<>'*' and @CArdCode=T4.CardCode ) )
		and (@CArdRazaoSocial='*' or (@CArdRazaoSocial<>'*' and T4.CardName like '%'+ @CArdRazaoSocial +'%'))
		and (@CArdNomeFantasia='*' or (@CArdNomeFantasia<>'*' and T4.CardFName like '%'+ @CArdRazaoSocial +'%'))	
		and (@CNPJ='*' or exists ( select 1 from  CRD7 T6 where  T6.CardCode=T0.CardCode and (not T6.TaxId0 is null or not T6.TaxId4 is null) and coalesce(T6.TaxId0,T6.TaxId4)=@CNPJ  )    )
		and (@BplId=0 or @BplId=T0.BPLId)	
		--AND (@TIPOCONTA='*' OR @TIPOCONTA='RECEBER' )
--select top 1 ObjType  from OPCH
--18 A/P Invoice 		
	UNION ALL
	select 
		'RECEBER' as 'TPMov' 
		,T0.DocEntry
		,T0.DocNum 
		,T0.ObjType
		,T2.DocNum DOC
		,'NOTA FISCAL DE ENTRADA'  as 'TP'
		--, T0.BPLId
		, T0.BPLName 'FILIAL DE ORIGEM'		
		
		, T0.DocDate 'EMISSÃO'
		, T3.DueDate 'VENCIMENTO'	
		, T0.TaxDate 'MOVIMENTO'
		, coalesce(cast(T1.InstId as nvarchar(10)),'1') +'/'+ coalesce(cast(t2.Installmnt as nvarchar(10)),'1')  as 'PARCELA'
		
		, T1.SumApplied as 'VALOR PAGO'
		, T3.InsTotal 'PRINCIPAL'
		
		,(case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end + ' - '+T5.AcctName ) 'CONTA CONTÁBEIL'						
		--,case 
 	--		when T0.CashSum>0 then T0.CashSum
		--	when T0.boeSum>0 then T0.boeSum
		--	when T0.TrsfrSum>0 then  T0.TrsfrSum
		--	when T0.CreditSum>0 then T0.CreditSum
		--	when T0.CheckSum>0 then  T0.CheckSum
		--end ValorTotal
		, T1.invType  ObjTypeoRIGEM
		, t1.DocEntry DocEntryORIGEM
		, T4.CardCode
		, (T4.CardCode + ' - ' + T4.CardName)  Parceiro
	from 
		ORCT T0      
		inner join RCT2 T1 on T0.docentry = T1.DocNum
		inner join OPCH T2 on T2.ObjType = T1.invType	 and T2.DocEntry=T1.DocEntry
		inner join PCH6 T3 on T3.DocEntry = T2.DocEntry and T3.InstlmntID=T1.InstId
		inner join OCRD T4 on T4.CardCode = T0.CardCode
		inner join OACT T5 on T5.AcctCode=case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end
	where
		T0.CANCELED<>'Y' and T0.CANCELED<>'C'
		and T0.DocDate between @DataIni and @DataFIM	
		and (@CArdCode='*' or ( @CArdCode<>'*' and @CArdCode=T4.CardCode ) )
		and (@CArdRazaoSocial='*' or (@CArdRazaoSocial<>'*' and T4.CardName like '%'+ @CArdRazaoSocial +'%'))
		and (@CArdNomeFantasia='*' or (@CArdNomeFantasia<>'*' and T4.CardFName like '%'+ @CArdRazaoSocial +'%'))	
		and (@CNPJ='*' or exists ( select 1 from  CRD7 T6 where  T6.CardCode=T0.CardCode and (not T6.TaxId0 is null or not T6.TaxId4 is null) and coalesce(T6.TaxId0,T6.TaxId4)=@CNPJ  )    )
		and (@BplId=0 or @BplId=T0.BPLId)
		--AND (@TIPOCONTA='*' OR @TIPOCONTA='RECEBER' )
--select top 1 ObjType  from ORPC
--19 A/P Credit Memo 
	UNION ALL
	select 
		'RECEBER' as 'TPMov' 
		,T0.DocEntry
		,T0.DocNum 
		,T0.ObjType
		,T2.DocNum DOC
		,'DEV. DE NOTA FISCAL DE ENTRADA'  as 'TP'
		--, T0.BPLId
		, T0.BPLName 'FILIAL DE ORIGEM'		
		
		, T0.DocDate 'EMISSÃO'
		, T3.DueDate 'VENCIMENTO'	
		, T0.TaxDate 'MOVIMENTO'
		, coalesce(cast(T1.InstId as nvarchar(10)),'1') +'/'+ coalesce(cast(t2.Installmnt as nvarchar(10)),'1')  as 'PARCELA'
		
		, T1.SumApplied as 'VALOR PAGO'
		, T3.InsTotal 'PRINCIPAL'
		
		,(case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end + ' - '+T5.AcctName ) 'CONTA CONTÁBEIL'						
		--,case 
 	--		when T0.CashSum>0 then T0.CashSum
		--	when T0.boeSum>0 then T0.boeSum
		--	when T0.TrsfrSum>0 then  T0.TrsfrSum
		--	when T0.CreditSum>0 then T0.CreditSum
		--	when T0.CheckSum>0 then  T0.CheckSum
		--end ValorTotal
		, T1.invType  ObjTypeoRIGEM
		, t1.DocEntry DocEntryORIGEM
		, T4.CardCode
		, (T4.CardCode + ' - ' + T4.CardName)  Parceiro
	from 
		ORCT T0      
		inner join RCT2 T1 on T0.docentry = T1.DocNum
		inner join ORPC T2 on T2.ObjType = T1.invType	 and T2.DocEntry=T1.DocEntry
		inner join RPC6 T3 on T3.DocEntry = T2.DocEntry and T3.InstlmntID=T1.InstId
		inner join OCRD T4 on T4.CardCode = T0.CardCode
		inner join OACT T5 on T5.AcctCode=case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end
	where
		T0.CANCELED<>'Y' and T0.CANCELED<>'C'
		and T0.DocDate between @DataIni and @DataFIM	
		and (@CArdCode='*' or ( @CArdCode<>'*' and @CArdCode=T4.CardCode ) )
		and (@CArdRazaoSocial='*' or (@CArdRazaoSocial<>'*' and T4.CardName like '%'+ @CArdRazaoSocial +'%'))
		and (@CArdNomeFantasia='*' or (@CArdNomeFantasia<>'*' and T4.CardFName like '%'+ @CArdRazaoSocial +'%'))	
		and (@CNPJ='*' or exists ( select 1 from  CRD7 T6 where  T6.CardCode=T0.CardCode and (not T6.TaxId0 is null or not T6.TaxId4 is null) and coalesce(T6.TaxId0,T6.TaxId4)=@CNPJ  )    )
		and (@BplId=0 or @BplId=T0.BPLId)	
		--AND (@TIPOCONTA='*' OR @TIPOCONTA='RECEBER' )
--select top 1 ObjType  from OJDT
--30 Journal Entry 	
	UNION ALL
	select 
		'RECEBER' as 'TPMov' 
		,T0.DocEntry
		,T0.DocNum 
		,T0.ObjType
		,T2.TransId DOC
		,'LANÇAMENTO CONTÁBIL MANUAL'  as 'TP'
		--, T0.BPLId
		, T0.BPLName 'FILIAL DE ORIGEM'		
		
		, T0.DocDate 'EMISSÃO'
		, T2.DueDate 'VENCIMENTO'	
		, T0.TaxDate 'MOVIMENTO'
		, '1/1'  as 'PARCELA'
		
		, T1.SumApplied as 'VALOR PAGO'
		, CASE WHEN T22.Debit>0 THEN T22.Debit ELSE T22.Credit END  'PRINCIPAL'
		
		,(case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end + ' - '+T5.AcctName ) 'CONTA CONTÁBEIL'						
		--,case 
 	--		when T0.CashSum>0 then T0.CashSum
		--	when T0.boeSum>0 then T0.boeSum
		--	when T0.TrsfrSum>0 then  T0.TrsfrSum
		--	when T0.CreditSum>0 then T0.CreditSum
		--	when T0.CheckSum>0 then  T0.CheckSum
		--end ValorTotal
		, T1.invType  ObjTypeoRIGEM
		, t1.DocEntry DocEntryORIGEM
		, T4.CardCode
		, (T4.CardCode + ' - ' + T4.CardName)  Parceiro
	from 
		ORCT T0      
		inner join RCT2 T1 on T0.docentry = T1.DocNum
		inner join OJDT T2 on T2.ObjType = T1.invType	 and T2.TransId=T1.DocEntry
		inner join jdt1 T22 on t2.TransId = t22.TransId		AND T22.ShortName=T0.CardCode AND T1.DocLine=T22.Line_ID
		--inner join JDT6 T3 on T3.DocEntry = T2.DocEntry and T3.InstlmntID=T1.InstId
		inner join OCRD T4 on T4.CardCode = T0.CardCode
		inner join OACT T5 on T5.AcctCode=case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end
	where
		T0.CANCELED<>'Y' and T0.CANCELED<>'C'
		and T0.DocDate between @DataIni and @DataFIM	
		and (@CArdCode='*' or ( @CArdCode<>'*' and @CArdCode=T4.CardCode ) )
		and (@CArdRazaoSocial='*' or (@CArdRazaoSocial<>'*' and T4.CardName like '%'+ @CArdRazaoSocial +'%'))
		and (@CArdNomeFantasia='*' or (@CArdNomeFantasia<>'*' and T4.CardFName like '%'+ @CArdRazaoSocial +'%'))	
		and (@CNPJ='*' or exists ( select 1 from  CRD7 T6 where  T6.CardCode=T0.CardCode and (not T6.TaxId0 is null or not T6.TaxId4 is null) and coalesce(T6.TaxId0,T6.TaxId4)=@CNPJ  )    )
		and (@BplId=0 or @BplId=T0.BPLId)	
		--AND (@TIPOCONTA='*' OR @TIPOCONTA='RECEBER' )
	UNION ALL
	select 
		'RECEBER' as 'TPMov' 
		,T0.DocEntry
		,T0.DocNum 
		,T0.ObjType
		,T2.DocNum DOC
		,'DEVOLUÇÃO'  as 'TP'
		--, T0.BPLId
		, T0.BPLName 'FILIAL DE ORIGEM'		
		
		, T0.DocDate 'EMISSÃO'
		, T3.DueDate 'VENCIMENTO'	
		, T0.TaxDate 'MOVIMENTO'
		, coalesce(cast(T1.InstId as nvarchar(10)),'1') +'/'+ coalesce(cast(t2.Installmnt as nvarchar(10)),'1')  as 'PARCELA'
		
		, T1.SumApplied as 'VALOR PAGO'
		, T3.InsTotal 'PRINCIPAL'
		
		,(case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end + ' - '+T5.AcctName ) 'CONTA CONTÁBEIL'						
		--,case 
 	--		when T0.CashSum>0 then T0.CashSum
		--	when T0.boeSum>0 then T0.boeSum
		--	when T0.TrsfrSum>0 then  T0.TrsfrSum
		--	when T0.CreditSum>0 then T0.CreditSum
		--	when T0.CheckSum>0 then  T0.CheckSum
		--end ValorTotal
		, T1.invType  ObjTypeoRIGEM
		, t1.DocEntry DocEntryORIGEM
		, T4.CardCode
		, (T4.CardCode + ' - ' + T4.CardName)  Parceiro
	from 
		ORCT T0      
		inner join RCT2 T1 on T0.docentry = T1.DocNum
		inner join ORDN T2 on T2.ObjType = T1.invType	 and T2.DocEntry=T1.DocEntry
		inner join RDN6 T3 on T3.DocEntry = T2.DocEntry and T3.InstlmntID=T1.InstId
		inner join OCRD T4 on T4.CardCode = T0.CardCode
		inner join OACT T5 on T5.AcctCode=case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end
	where
		T0.CANCELED<>'Y' and T0.CANCELED<>'C'
		and T0.DocDate between @DataIni and @DataFIM	
		and (@CArdCode='*' or ( @CArdCode<>'*' and @CArdCode=T4.CardCode ) )
		and (@CArdRazaoSocial='*' or (@CArdRazaoSocial<>'*' and T4.CardName like '%'+ @CArdRazaoSocial +'%'))
		and (@CArdNomeFantasia='*' or (@CArdNomeFantasia<>'*' and T4.CardFName like '%'+ @CArdRazaoSocial +'%'))	
		and (@CNPJ='*' or exists ( select 1 from  CRD7 T6 where  T6.CardCode=T0.CardCode and (not T6.TaxId0 is null or not T6.TaxId4 is null) and coalesce(T6.TaxId0,T6.TaxId4)=@CNPJ  )    )
		and (@BplId=0 or @BplId=T0.BPLId)
		--AND (@TIPOCONTA='*' OR @TIPOCONTA='RECEBER' )

	) AS tb
	WHERE 
		(@SALDO = '*' OR (PRINCIPAL>[VALOR PAGO] AND @SALDO='COM SALDO')  OR (PRINCIPAL=[VALOR PAGO] AND @SALDO='SEM SALDO'))
	ORDER BY [EMISSÃO]
	
end

go	


execute spcCVAComposicaoSaldoContabilTitulosRECEBER '2000-01-01','2020-12-30','*','*','*','*',0,'*'


--SELECT * FROM OJDT WHERE TransId=64518
--select top 1 ObjType  from ORDN
--select top 1 ObjType  from OPCH

select T0.AcctCode,T0.AcctName from OACT T0 order by 1