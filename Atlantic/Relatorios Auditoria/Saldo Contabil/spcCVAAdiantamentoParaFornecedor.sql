USE SBO_PRD_CFI0002
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcCVAAdiantamentoParaFornecedor]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].spcCVAAdiantamentoParaFornecedor
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE spcCVAAdiantamentoParaFornecedor
	@NumAdiantamento int	
	,@NumDocumento int	
	,@DataIni as DAte
	,@DataFIM as DAte
	,@CArdCode as nvarchar(max)
	,@CArdRazaoSocial as nvarchar(max)
	,@CArdNomeFantasia as nvarchar(max)
	,@CNPJ as nvarchar(max)
	,@BplId int	
	--,@TIPOCONTA AS NVARCHAR(20)
	,@SALDO AS NVARCHAR(20)
	,@AcctCode as nvarchar(max)
AS 
BEGIN
	select 
		T2.DocNum DOC
		,UPPER(T4.CardCode + ' - ' + T4.CardName)  Parceiro
		,T0.DocEntry
		,T0.DocNum 
		,T0.ObjType
		
		,'ADIANTAMENTO PARA FORNECEDOR'  as 'TP'
		--, T0.BPLId
		, UPPER(T0.BPLName) 'FILIAL DE ORIGEM'		
		
		, T2.DocDate 'DATA DO ADIANTAMENTO'
		, T2.DocTotal 'VLR DO ADIANTAMENTO'	
		, T0.DocDate 'DATA DA BAIXA'
		
		--, coalesce(cast(T1.InstId as nvarchar(10)),'1') +'/'+ coalesce(cast(t2.Installmnt as nvarchar(10)),'1')  as 'PARCELA'
		
		, T1.SumApplied as 'VALOR DA BAIXA'
		, T2.DocTotal-coalesce(T1.SumApplied,0) 'SALDO'
		, UPPER(T6.Memo)Memo
		,UPPER((case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end + ' - '+T5.AcctName )) 'CONTA CONTÁBEIL'						
		--, T1.invType  ObjTypeoRIGEM
		--, t1.DocEntry DocEntryORIGEM
		--, T4.CardCode
	from 
		OVPM T0      
		inner join VPM2 T1 on T0.docentry = T1.DocNum
		inner join ODPO T2 on T2.ObjType = T1.invType	 and T2.DocEntry=T1.DocEntry
		--inner join DPO6 T3 on T3.DocEntry = T2.DocEntry and T3.InstlmntID=T1.InstId
		inner join OCRD T4 on T4.CardCode = T0.CardCode
		inner join OACT T5 on T5.AcctCode=case 
 			when T0.CashSum>0 then T0.CashAcct
			when T0.boeSum>0 then T0.BoeAcc
			when T0.TrsfrSum>0 then  T0.TrsfrAcct
			when T0.CreditSum>0 then (select RCT3.CreditAcct from RCT3 where T0.DocEntry=RCT3.DocNum)
			when T0.CheckSum>0 then  T0.checkAcct
		end
		inner join OJDT T6 on T6.TransType=T0.ObjType and T6.BaseRef=T0.DocEntry
	where
		T0.CANCELED<>'Y' and T0.CANCELED<>'C'
		and T0.DocDate between @DataIni and @DataFIM	
		and (@CArdCode='*' or ( @CArdCode<>'*' and @CArdCode=T4.CardCode ) )
		and (@CArdRazaoSocial='*' or (@CArdRazaoSocial<>'*' and T4.CardName like '%'+ @CArdRazaoSocial +'%'))
		and (@CArdNomeFantasia='*' or (@CArdNomeFantasia<>'*' and T4.CardFName like '%'+ @CArdRazaoSocial +'%'))	
		and (@CNPJ='*' or exists ( select 1 from  CRD7 T6 where  T6.CardCode=T0.CardCode and (not T6.TaxId0 is null or not T6.TaxId4 is null) and coalesce(T6.TaxId0,T6.TaxId4)=@CNPJ  )    )
		and (@BplId=0 or @BplId=T0.BPLId)
		and (@AcctCode='*' or T5.AcctCode=@AcctCode)	
		and (@NumAdiantamento=0 or @NumAdiantamento=T2.DocNum)
		and (@NumDocumento=0 or @NumDocumento=T0.DocNum)
		and (
			@SALDO='*' 
			or (@SALDO='COM SALDO' and (T2.DocTotal-coalesce(T1.SumApplied,0))>0)
			or (@SALDO='SEM SALDO' and (T2.DocTotal-coalesce(T1.SumApplied,0))=0)
		
			)

		--SALDO@select 'COM SALDO' CODIGO,'COM SALDO' NOME UNION ALL select 'SEM SALDO' CODIGO,'SEM SALDO' NOME UNION ALL select '*', 'TODOS' ORDER BY 1
		
end

go	


execute spcCVAAdiantamentoParaFornecedor 0,0,'2000-01-01','2020-12-30','*','*','*','*',0,'*','*'		


--select TOP 1 * from OJDT where TransType=46 and BaseRef=106--TransType=46