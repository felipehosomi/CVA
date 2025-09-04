USE SBO_PRD_CFI0002
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcCVAControleFinanceiroComprasNF]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].spcCVAControleFinanceiroComprasNF
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE spcCVAControleFinanceiroComprasNF
	@DocEntryPC int
WITH ENCRYPTION 
AS 
BEGIN
	select distinct
		T0.DocEntry
		,T0.Serial
		,cast(T6.InstlmntID as nvarchar(100)) +'/'+cast(T0.Installmnt as nvarchar(100)) 'Parcela'
		,T6.DueDate 'Data'
		,T6.InsTotal 'Valor'
	from 
		OPCH T0 
		inner join PCH1 T1 on T1.DocEntry=T0.DocEntry 
		inner join POR1 T2 on T2.DocEntry=T1.BaseEntry
		inner join OPOR T3 on T3.DocEntry=T2.DocEntry and T1.BaseType=T3.ObjType and T1.BaseLine=T2.LineNum
		inner join OCRD T4 on T4.CardCode=T0.CardCode and T4.CardCode=T3.CardCode
		inner join OBPL T5 on T5.BPLId=T0.BPLId and T3.BPLId=T5.BPLId
		inner join PCH6 T6 on T6.DocEntry=T0.DocEntry
	where
		T3.DocEntry=@DocEntryPC
		and T0.CANCELED='N'
	--order by
	--	T6.InstlmntID

		
end

go

execute spcCVAControleFinanceiroComprasNF 1



--select * from PCH6 T0 where T0.DocEntry=1


--select * from OPCH where docnum=16442



--select * from INV6 T0 where T0.DocEntry=16442