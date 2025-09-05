ALTER procedure [dbo].[spc_CVA_Comissoes](@Status char(1) = 'T', @View char(1) = 'T', @DataInicial datetime, @DataFinal datetime)
as
begin
set dateformat 'ymd';
set nocount on;

BEGIN -- CRIAÇÃO DE TABELAS
CREATE TABLE #tbl_docs_faturamento
(
	 Momento CHAR(1) Collate Database_Default
	,DocEntry INT
	,DocDate DATETIME
	,Serial INT
	,DocTotal NUMERIC(19,6)
	,ObjType NVARCHAR(40) Collate Database_Default
	,ItemCode NVARCHAR(100) Collate Database_Default
	,LineNum INT
	,Quantity NUMERIC(19,6)
	,LineTotal NUMERIC(19,6)
	,InstlmntID INT
	,InsTotal NUMERIC(19,6)
	,DueDate DATETIME
	,ItemName NVARCHAR(200) Collate Database_Default
	,ItmsGrpCod SMALLINT
	,ItmsGrpNam NVARCHAR(40) Collate Database_Default
	,CardCode NVARCHAR(30) Collate Database_Default
	,CardName NVARCHAR(200) Collate Database_Default
	,PrcCode NVARCHAR(16) Collate Database_Default
	,PrcName NVARCHAR(60) Collate Database_Default
	,SlpCode INT
	,SlpName NVARCHAR(310) Collate Database_Default
	,U_CVA_IMPADIC CHAR(1) Collate Database_Default
	,U_CVA_IMPINCL CHAR(1) Collate Database_Default
	,FirmCode SMALLINT
	,FirmName NVARCHAR(60) Collate Database_Default
	,IndCode INT
	,IndName NVARCHAR(30) Collate Database_Default
	,IndDesc NVARCHAR(60) Collate Database_Default
	,AbsId INT
	,Name NVARCHAR(200) Collate Database_Default
	,[State] NVARCHAR(6) Collate Database_Default
	,TaxDate DATETIME
	,SumApplied NUMERIC(19,6)
	,Tot_Invoices INT
	,TaxSum_IMPADIC_Linha NUMERIC(19,6)
	,TaxSum_IMPINCL_Linha NUMERIC(19,6)
	,TaxSum_IMPADIC_Parcela NUMERIC(19,6)
	,TaxSum_IMPINCL_Parcela NUMERIC(19,6)
	,Tot_Parcelas INT
	,Tot_Itens INT
	,DocStatus CHAR(1) Collate Database_Default
	,GroupCode INT
	,TotalItens NUMERIC(19,6)
	,LineTotal_Min_IMPINCL NUMERIC(19,6)
	,LineTotal_Min_IMPADIC NUMERIC(19,6)
	,LineTotal_Min_IMPADIC_Min_IMPINCL NUMERIC(19,6)
	,LineTotal_Plus_IMPADIC_Plus_IMPINCL NUMERIC(19,6)
	,InsTotal_Min_IMPINCL NUMERIC(19,6)
	,InsTotal_Min_IMPADIC NUMERIC(19,6)
	,InsTotal_Min_IMPINCL_Min_IMPADIC NUMERIC(19,6)
	,InsTotal_Plus_IMPINCL_Plus_IMPADIC NUMERIC(19,6)
)

CREATE TABLE #tbl_docs_pedido
(
	Momento CHAR(1) Collate Database_Default
	,DocEntry INT
	,DocDate DATETIME
	,Serial INT
	,DocTotal NUMERIC(19,6)
	,ObjType NVARCHAR(40) Collate Database_Default
	,ItemCode NVARCHAR(100) Collate Database_Default
	,LineNum INT
	,Quantity NUMERIC(19,6)
	,LineTotal NUMERIC(19,6)
	,InstlmntID INT
	,InsTotal NUMERIC(19,6)
	,DueDate DATETIME
	,ItemName NVARCHAR(200) Collate Database_Default
	,ItmsGrpCod SMALLINT
	,ItmsGrpNam NVARCHAR(40) Collate Database_Default
	,CardCode NVARCHAR(30) Collate Database_Default
	,CardName NVARCHAR(200) Collate Database_Default
	,PrcCode NVARCHAR(16) Collate Database_Default
	,PrcName NVARCHAR(60) Collate Database_Default
	,SlpCode INT
	,SlpName NVARCHAR(310) Collate Database_Default
	,U_CVA_IMPADIC CHAR(1) Collate Database_Default
	,U_CVA_IMPINCL CHAR(1) Collate Database_Default
	,FirmCode SMALLINT
	,FirmName NVARCHAR(60) Collate Database_Default
	,IndCode INT
	,IndName NVARCHAR(30) Collate Database_Default
	,IndDesc NVARCHAR(60) Collate Database_Default
	,AbsId INT
	,Name NVARCHAR(200) Collate Database_Default
	,[State] NVARCHAR(6) Collate Database_Default
	,TaxDate DATETIME
	,SumApplied NUMERIC(19,6)
	,Tot_Invoices INT
	,TaxSum_IMPADIC_Linha NUMERIC(19,6)
	,TaxSum_IMPINCL_Linha NUMERIC(19,6)
	,TaxSum_IMPADIC_Parcela NUMERIC(19,6)
	,TaxSum_IMPINCL_Parcela NUMERIC(19,6)
	,Tot_Parcelas INT
	,Tot_Itens INT
	,DocStatus CHAR(1) Collate Database_Default
	,GroupCode INT
	,TotalItens NUMERIC(19,6)
	,LineTotal_Min_IMPINCL NUMERIC(19,6)
	,LineTotal_Min_IMPADIC NUMERIC(19,6)
	,LineTotal_Min_IMPADIC_Min_IMPINCL NUMERIC(19,6)
	,LineTotal_Plus_IMPADIC_Plus_IMPINCL NUMERIC(19,6)
	,InsTotal_Min_IMPINCL NUMERIC(19,6)
	,InsTotal_Min_IMPADIC NUMERIC(19,6)
	,InsTotal_Min_IMPINCL_Min_IMPADIC NUMERIC(19,6)
	,InsTotal_Plus_IMPINCL_Plus_IMPADIC NUMERIC(19,6)
)

CREATE TABLE #tbl_docs_recebimento
(
	Momento CHAR(1) Collate Database_Default
	,DocEntry INT
	,DocDate DATETIME
	,Serial INT
	,DocTotal NUMERIC(19,6)
	,ObjType NVARCHAR(40) Collate Database_Default
	,ItemCode NVARCHAR(100) Collate Database_Default
	,LineNum INT
	,Quantity NUMERIC(19,6)
	,LineTotal NUMERIC(19,6)
	,InstlmntID INT
	,InsTotal NUMERIC(19,6)
	,DueDate DATETIME
	,ItemName NVARCHAR(200)  Collate Database_Default
	,ItmsGrpCod SMALLINT
	,ItmsGrpNam NVARCHAR(40) Collate Database_Default
	,CardCode NVARCHAR(30)   Collate Database_Default
	,CardName NVARCHAR(200)  Collate Database_Default
	,PrcCode NVARCHAR(16)	Collate Database_Default
	,PrcName NVARCHAR(60)	Collate Database_Default
	,SlpCode INT
	,SlpName NVARCHAR(310) Collate Database_Default
	,U_CVA_IMPADIC CHAR(1) Collate Database_Default
	,U_CVA_IMPINCL CHAR(1) Collate Database_Default
	,FirmCode SMALLINT
	,FirmName NVARCHAR(60) Collate Database_Default
	,IndCode INT
	,IndName NVARCHAR(30)  Collate Database_Default
	,IndDesc NVARCHAR(60)  Collate Database_Default
	,AbsId INT
	,Name NVARCHAR(200)    Collate Database_Default
	,[State] NVARCHAR(6)   Collate Database_Default
	,TaxDate DATETIME
	,SumApplied NUMERIC(19,6)
	,Tot_Invoices INT
	,TaxSum_IMPADIC_Linha NUMERIC(19,6)
	,TaxSum_IMPINCL_Linha NUMERIC(19,6)
	,TaxSum_IMPADIC_Parcela NUMERIC(19,6)
	,TaxSum_IMPINCL_Parcela NUMERIC(19,6)
	,Tot_Parcelas INT
	,Tot_Itens INT
	,DocStatus CHAR(1) Collate Database_Default
	,GroupCode INT
	,TotalItens NUMERIC(19,6)
	,LineTotal_Min_IMPINCL NUMERIC(19,6)
	,LineTotal_Min_IMPADIC NUMERIC(19,6)
	,LineTotal_Min_IMPADIC_Min_IMPINCL NUMERIC(19,6)
	,LineTotal_Plus_IMPADIC_Plus_IMPINCL NUMERIC(19,6)
	,InsTotal_Min_IMPINCL NUMERIC(19,6)
	,InsTotal_Min_IMPADIC NUMERIC(19,6)
	,InsTotal_Min_IMPINCL_Min_IMPADIC NUMERIC(19,6)
	,InsTotal_Plus_IMPINCL_Plus_IMPADIC NUMERIC(19,6)
)

CREATE TABLE #tbl_docs_aux
(
	 Momento CHAR(1) Collate Database_Default
	,DocEntry INT
	,DocDate DATETIME
	,Serial INT
	,DocTotal NUMERIC(19,6)
	,ObjType NVARCHAR(40)   Collate Database_Default
	,ItemCode NVARCHAR(100) Collate Database_Default
	,LineNum INT
	,Quantity NUMERIC(19,6)
	,LineTotal NUMERIC(19,6)
	,InstlmntID INT
	,InsTotal NUMERIC(19,6)
	,DueDate DATETIME
	,ItemName NVARCHAR(200)  Collate Database_Default
	,ItmsGrpCod SMALLINT
	,ItmsGrpNam NVARCHAR(40) Collate Database_Default
	,CardCode NVARCHAR(30)   Collate Database_Default
	,CardName NVARCHAR(200)  Collate Database_Default
	,PrcCode NVARCHAR(16)    Collate Database_Default
	,PrcName NVARCHAR(60)    Collate Database_Default
	,SlpCode INT
	,SlpName NVARCHAR(310) Collate Database_Default
	,U_CVA_IMPADIC CHAR(1) Collate Database_Default
	,U_CVA_IMPINCL CHAR(1) Collate Database_Default
	,FirmCode SMALLINT
	,FirmName NVARCHAR(60) Collate Database_Default
	,IndCode INT
	,IndName NVARCHAR(30)  Collate Database_Default
	,IndDesc NVARCHAR(60)  Collate Database_Default
	,AbsId INT
	,Name NVARCHAR(200)    Collate Database_Default
	,[State] NVARCHAR(6)   Collate Database_Default
	,TaxDate DATETIME
	,SumApplied NUMERIC(19,6)
	,Tot_Invoices INT
	,TaxSum_IMPADIC_Linha NUMERIC(19,6)
	,TaxSum_IMPINCL_Linha NUMERIC(19,6)
	,TaxSum_IMPADIC_Parcela NUMERIC(19,6)
	,TaxSum_IMPINCL_Parcela NUMERIC(19,6)
	,Tot_Parcelas INT
	,Tot_Itens INT
	,DocStatus CHAR(1) Collate Database_Default
	,GroupCode INT
	,TotalItens NUMERIC(19,6)
	,LineTotal_Min_IMPINCL NUMERIC(19,6)
	,LineTotal_Min_IMPADIC NUMERIC(19,6)
	,LineTotal_Min_IMPADIC_Min_IMPINCL NUMERIC(19,6)
	,LineTotal_Plus_IMPADIC_Plus_IMPINCL NUMERIC(19,6)
	,InsTotal_Min_IMPINCL NUMERIC(19,6)
	,InsTotal_Min_IMPADIC NUMERIC(19,6)
	,InsTotal_Min_IMPINCL_Min_IMPADIC NUMERIC(19,6)
	,InsTotal_Plus_IMPINCL_Plus_IMPADIC NUMERIC(19,6)
)

CREATE TABLE #tbl_docs_fat
(
	 RegrCode INT
	,Comissionado INT
	,Comissao NUMERIC(19,6)
	,Prioridade INT
	,IsItem CHAR(1)
	,Momento CHAR(1) Collate Database_Default
	,DocEntry INT
	,DocDate DATETIME
	,Serial INT
	,DocTotal NUMERIC(19,6)
	,ObjType NVARCHAR(40) Collate Database_Default
	,ItemCode NVARCHAR(100) Collate Database_Default
	,LineNum INT
	,Quantity NUMERIC(19,6)
	,LineTotal NUMERIC(19,6)
	,InstlmntID INT
	,InsTotal NUMERIC(19,6)
	,DueDate DATETIME
	,ItemName NVARCHAR(200)	Collate Database_Default
	,ItmsGrpCod SMALLINT
	,ItmsGrpNam NVARCHAR(40) Collate Database_Default
	,CardCode NVARCHAR(30)	Collate Database_Default
	,CardName NVARCHAR(200)  Collate Database_Default
	,PrcCode NVARCHAR(16)    Collate Database_Default
	,PrcName NVARCHAR(60)    Collate Database_Default
	,SlpCode INT
	,SlpName NVARCHAR(310)	Collate Database_Default
	,U_CVA_IMPADIC CHAR(1)	Collate Database_Default
	,U_CVA_IMPINCL CHAR(1)	Collate Database_Default
	,FirmCode SMALLINT
	,FirmName NVARCHAR(60)	Collate Database_Default
	,IndCode INT
	,IndName NVARCHAR(30)	Collate Database_Default
	,IndDesc NVARCHAR(60)	Collate Database_Default
	,AbsId INT
	,Name NVARCHAR(200)		Collate Database_Default
	,[State] NVARCHAR(6)		Collate Database_Default
	,TaxDate DATETIME
	,SumApplied NUMERIC(19,6)
	,Tot_Invoices INT
	,TaxSum_IMPADIC_Linha NUMERIC(19,6)
	,TaxSum_IMPINCL_Linha NUMERIC(19,6)
	,TaxSum_IMPADIC_Parcela NUMERIC(19,6)
	,TaxSum_IMPINCL_Parcela NUMERIC(19,6)
	,Tot_Parcelas INT
	,Tot_Itens INT
	,DocStatus CHAR(1) Collate Database_Default
	,GroupCode INT
	,TotalItens NUMERIC(19,6)
	,LineTotal_Min_IMPINCL NUMERIC(19,6)
	,LineTotal_Min_IMPADIC NUMERIC(19,6)
	,LineTotal_Min_IMPADIC_Min_IMPINCL NUMERIC(19,6)
	,LineTotal_Plus_IMPADIC_Plus_IMPINCL NUMERIC(19,6)
	,InsTotal_Min_IMPINCL NUMERIC(19,6)
	,InsTotal_Min_IMPADIC NUMERIC(19,6)
	,InsTotal_Min_IMPINCL_Min_IMPADIC NUMERIC(19,6)
	,InsTotal_Plus_IMPINCL_Plus_IMPADIC NUMERIC(19,6)
)

CREATE TABLE #tbl_docs_fat_aux
(
	 RegrCode INT
	,Comissionado INT
	,Comissao NUMERIC(19,6)
	,Prioridade INT
	,IsItem CHAR(1)
	,Momento CHAR(1) Collate Database_Default
	,DocEntry INT
	,DocDate DATETIME
	,Serial INT
	,DocTotal NUMERIC(19,6)
	,ObjType NVARCHAR(40)   Collate Database_Default
	,ItemCode NVARCHAR(100) Collate Database_Default
	,LineNum INT
	,Quantity NUMERIC(19,6)
	,LineTotal NUMERIC(19,6)
	,InstlmntID INT
	,InsTotal NUMERIC(19,6)
	,DueDate DATETIME
	,ItemName NVARCHAR(200)  Collate Database_Default
	,ItmsGrpCod SMALLINT
	,ItmsGrpNam NVARCHAR(40) Collate Database_Default
	,CardCode NVARCHAR(30)   Collate Database_Default
	,CardName NVARCHAR(200)  Collate Database_Default
	,PrcCode NVARCHAR(16)    Collate Database_Default
	,PrcName NVARCHAR(60)    Collate Database_Default
	,SlpCode INT
	,SlpName NVARCHAR(310)	Collate Database_Default
	,U_CVA_IMPADIC CHAR(1)	Collate Database_Default
	,U_CVA_IMPINCL CHAR(1)	Collate Database_Default
	,FirmCode SMALLINT
	,FirmName NVARCHAR(60)	Collate Database_Default
	,IndCode INT
	,IndName NVARCHAR(30)	Collate Database_Default
	,IndDesc NVARCHAR(60)	Collate Database_Default
	,AbsId INT
	,Name NVARCHAR(200)		Collate Database_Default
	,[State] NVARCHAR(6)		Collate Database_Default
	,TaxDate DATETIME
	,SumApplied NUMERIC(19,6)
	,Tot_Invoices INT
	,TaxSum_IMPADIC_Linha   NUMERIC(19,6)
	,TaxSum_IMPINCL_Linha   NUMERIC(19,6)
	,TaxSum_IMPADIC_Parcela NUMERIC(19,6)
	,TaxSum_IMPINCL_Parcela NUMERIC(19,6)
	,Tot_Parcelas INT
	,Tot_Itens INT
	,DocStatus CHAR(1) Collate Database_Default
	,GroupCode INT
	,TotalItens NUMERIC(19,6)
	,LineTotal_Min_IMPINCL NUMERIC(19,6)
	,LineTotal_Min_IMPADIC NUMERIC(19,6)
	,LineTotal_Min_IMPADIC_Min_IMPINCL NUMERIC(19,6)
	,LineTotal_Plus_IMPADIC_Plus_IMPINCL NUMERIC(19,6)
	,InsTotal_Min_IMPINCL NUMERIC(19,6)
	,InsTotal_Min_IMPADIC NUMERIC(19,6)
	,InsTotal_Min_IMPINCL_Min_IMPADIC NUMERIC(19,6)
	,InsTotal_Plus_IMPINCL_Plus_IMPADIC NUMERIC(19,6)
)

CREATE TABLE #tmp_table_where
(
	RegrCode INT,
	Comissionado INT,
	Comissao NUMERIC(19,6),
	Momento CHAR(1) Collate Database_Default,
	Prioridade INT,
	SqlWhere NVARCHAR(MAX) Collate Database_Default
)

create table #tmp_table_ret
(
	 U_COMISSIONADO int
	,U_PRIORIDADE int
	,U_CARDCODE nvarchar(50)  Collate Database_Default
	,U_CARDNAME nvarchar(200) Collate Database_Default
	,U_REGRA int
	,U_DOCDATE datetime
	,U_DUEDATE datetime
	,U_DOCENTRY int
	,U_OBJTYPE int
	,U_ITEMCODE nvarchar(50)  Collate Database_Default
	,U_ITEMNAME nvarchar(200) Collate Database_Default
	,U_LINENUM int
	,U_VALOR numeric(19,6)
	,U_PARCELA int
	,U_IMPOSTOS numeric(19,6)
	,U_COMISSAO numeric(19,6)
	,U_VALORCOMISSAO numeric(19,6)
	,U_TAXDATE datetime
	,U_CENTROCUSTO nvarchar(100) Collate Database_Default
	,U_PAGO char(1) Collate Database_Default
	,U_DATAPAGAMENTO datetime
	,U_MOMENTO nvarchar(max)
	,U_SERIAL INT
)
END

begin --DECLARAÇÃO DE VARIÁVEIS
	declare @cur as cursor

	declare  @CritCode int
			,@CritName nvarchar(200)
			,@CritPos int
			,@CritAtivo char(1)
			,@RegrCode int
			,@RegrName nvarchar(200)
			,@Tipo int
			,@Comissionado int
			,@Momento char(1)
			,@Vendedor int
			,@Item nvarchar(40)
			,@Grupo int
			,@CentroCusto nvarchar(16)
			,@Fabricante int
			,@Cliente nvarchar(30)
			,@Cidade int
			,@Estado nvarchar(6)
			,@Setor int
			,@Comissao numeric(19,6)
			,@Prioridade int
			,@Query nvarchar(max)
			,@Params nvarchar(max)
			,@Where nvarchar(max)
			,@GroupCode INT
end

EXEC [spc_CVA_Comissao_Faturamento] @DataInicial, @DataFinal

INSERT INTO #tbl_docs_recebimento
SELECT DISTINCT
	'R' AS Momento
	, T0.DocEntry AS DocEntry
	, T0.DocDate AS DocDate
	, T0.Serial AS Serial
	, T0.DocTotal AS DocTotal
	, T0.ObjType AS ObjType
	, T1.ItemCode AS ItemCode
	, T1.LineNum AS LineNum
	, T1.Quantity AS Quantity
	, OA_TOTAL.LineTotal AS LineTotal
	, T2.InstlmntID AS InstlmntID
	, T2.InsTotal AS InsTotal
	, T2.DueDate AS DueDate
	, T4.ItemName AS ItemName
	, T5.ItmsGrpCod AS ItmsGrpCod
	, T5.ItmsGrpNam AS ItmsGrpNam
	, T0.CardCode AS CardCode
	, T0.CardName AS CardName
	, ISNULL(T8.PrcCode, T9.PrcCode) AS PrcCode
	, ISNULL(T8.PrcName, T9.PrcName) AS PrcName
	, T6.SlpCode AS SlpCode
	, T6.SlpName AS SlpName
	, ISNULL(T6.U_CVA_IMPADC, 'N') AS U_CVA_IMPADIC
	, ISNULL(T6.U_CVA_IMPINCL, 'N') AS U_CVA_IMPINCL
	, T10.FirmCode AS FirmCode
	, T10.FirmName AS FirmName
	, T11.IndCode AS IndCode
	, T11.IndName AS IndName
	, T11.IndDesc AS IndDesc
	, T13.AbsId AS AbsId
	, T13.Name AS Name
	, T13.[State] AS [State]
	, OA_DATA.DataPagamento AS TaxDate
	, T14.SumApplied AS SumApplied
	, OA_MAXX.Countt AS Tot_Invoices
	, T1.VatSum * (T2.InstPrcnt/100) AS TaxSum_IMPADIC_Linha
	, OA_IMP.TaxSum * (T2.InstPrcnt/100) AS TaxSum_IMPINCL_Linha
	, T0.VatSum * (T2.InstPrcnt/100) AS TaxSum_IMPADIC_Parcela
	, OA_IMP2.TaxSum * (T2.InstPrcnt/100) AS TaxSum_IMPINCL_Parcela
	, T0.Installmnt AS Tot_Parcelas
	, OA_MAX.TotItem AS Tot_Itens
	, NULL AS DocStatus
	, T3.GroupCode AS GroupCode
	, OA_MAX.TotalItens AS TotalItens
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN ((OA_TOTAL.LineTotal - OA_IMP.TaxSum) + T1.VatSum)* (T2.InstPrcnt/100) ELSE 0 END AS LineTotal_Min_IMPINCL
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN OA_TOTAL.LineTotal * (T2.InstPrcnt/100) ELSE 0 END AS LineTotal_Min_IMPADIC
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN (OA_TOTAL.LineTotal - OA_IMP.TaxSum)* (T2.InstPrcnt/100) ELSE 0 END AS LineTotal_Min_IMPINCL_Min_IMPADIC
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN (OA_TOTAL.LineTotal + T1.VatSum)* (T2.InstPrcnt/100) ELSE 0 END AS LineTotal_Min_IMPINCL_Min_IMPADIC
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN T14.SumApplied - (OA_IMP2.TaxSum * (T2.InstPrcnt/100)) ELSE 0 END AS InsTotal_Min_IMPINCL
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN T14.SumApplied - (T0.VatSum * (T2.InstPrcnt/100)) ELSE 0 END AS InsTotal_Min_IMPADIC
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN T14.SumApplied - (OA_IMP2.TaxSum * (T2.InstPrcnt/100)) - (T0.VatSum * (T2.InstPrcnt/100)) ELSE 0 END AS InsTotal_Min_IMPINCL_Min_IMPADIC
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN T14.SumApplied ELSE 0 END AS InsTotal_Min_IMPINCL_Min_IMPADIC
FROM OINV T0 WITH(NOLOCK)
	INNER JOIN INV1 T1   WITH(NOLOCK) ON T0.DocEntry   = T1.DocEntry AND T1.TargetType <> N'14' AND ISNULL(T1.FreeChrgBP, 'N') = 'N'
	INNER JOIN INV6 T2   WITH(NOLOCK) ON T0.DocEntry   = T2.DocEntry AND T2.InsTotal   <> 0
	INNER JOIN OCRD T3   WITH(NOLOCK) ON T0.CardCode   = T3.CardCode
	INNER JOIN OITM T4   WITH(NOLOCK) ON T1.ItemCode   = T4.ItemCode
	INNER JOIN OITB T5   WITH(NOLOCK) ON T4.ItmsGrpCod = T5.ItmsGrpCod
	INNER JOIN OSLP T6   WITH(NOLOCK) ON T0.SlpCode    = T6.SlpCode
	LEFT  JOIN OPRC T8   WITH(NOLOCK) ON T1.OcrCode2   = T8.PrcCode
	LEFT  JOIN OPRC T9   WITH(NOLOCK) ON T1.OcrCode    = T9.PrcCode
	LEFT  JOIN OMRC T10  WITH(NOLOCK) ON T4.FirmCode   = T10.FirmCode
	LEFT  JOIN OOND T11  WITH(NOLOCK) ON T3.IndustryC  = T11.IndCode
	LEFT  JOIN INV12 T12 WITH(NOLOCK) ON T0.DocEntry   = T12.DocEntry
	LEFT  JOIN OCNT T13  WITH(NOLOCK) ON T12.CountyB   = T13.AbsId
	INNER JOIN RCT2 T14  WITH(NOLOCK) ON T2.DocEntry   = T14.DocEntry AND T2.InstlmntID = T14.InstId AND T2.ObjType = T14.InvType
	INNER JOIN ORCT T15  WITH(NOLOCK) ON T14.DocNum    = T15.DocEntry
	LEFT  JOIN OBOE T16  WITH(NOLOCK) ON T15.BoeAbs    = T16.BoeKey
	LEFT  JOIN OBOT T17  WITH(NOLOCK) ON T17.AbsEntry  = 
	(
		SELECT TOP 1 BOT1.AbsEntry FROM BOT1 WITH(NOLOCK)
		WHERE BOT1.BOENumber = T16.BoeNum AND BOT1.BoeType = T16.BoeType
		ORDER BY BOT1.AbsEntry DESC
	) 
	 AND T17.StatusTo = 'P'
	LEFT JOIN OJDT T18 WITH(NOLOCK) ON T17.TransId	 = T18.Number
	LEFT JOIN RCT1 T19 WITH(NOLOCK) ON T15.DocEntry  = T19.DocNum
	LEFT JOIN OCHH T20 WITH(NOLOCK) ON T19.CheckAbs  = T20.CheckKey
	LEFT JOIN DPS1 T21 WITH(NOLOCK) ON T20.CheckKey  = T21.CheckKey AND T21.DepCancel = 'N' AND T20.DpstAbs = T21.DepositId
	LEFT JOIN ODPS T22 WITH(NOLOCK) ON T21.DepositId = T22.DeposId  AND T22.DeposType = 'K'
	
OUTER APPLY
 (
	SELECT TotItem = COUNT(INV1.LineNum), TotalItens = SUM(INV1.LineTotal) 
	FROM INV1 WITH(NOLOCK) WHERE INV1.DocEntry = T0.DocEntry AND INV1.TargetType <> N'14' AND ISNULL(INV1.FreeChrgBP, 'N') = 'N'
 ) AS OA_MAX
OUTER APPLY 
 (
	SELECT Countt = COUNT(RCT2.InvoiceId) FROM RCT2 WITH(NOLOCK) WHERE RCT2.DocNum = T15.DocEntry
 ) AS OA_MAXX
OUTER APPLY 
 (
	SELECT DataPagamento = 
		CASE WHEN T17.StatusTo = 'P' THEN T18.RefDate
		ELSE CASE WHEN T20.CheckKey IS NOT NULL THEN T22.DeposDate
		ELSE CASE WHEN T15.BoeAbs IS NULL AND T19.CheckAbs IS NULL AND T15.DocEntry IS NOT NULL THEN T15.TaxDate
		ELSE NULL END END END
 ) AS OA_DATA
OUTER APPLY 
(
	SELECT TaxSum = ISNULL(SUM(DISTINCT INV4.TaxSum), 0)
	FROM INV4 WITH(NOLOCK)
		INNER JOIN OSTA WITH(NOLOCK) ON INV4.StaCode = OSTA.Code
		INNER JOIN OSTT WITH(NOLOCK) ON OSTA.[Type]  = OSTT.AbsId
		INNER JOIN ONFT WITH(NOLOCK) ON OSTT.NfTaxId = ONFT.AbsId
	WHERE INV4.TaxInPrice = 'Y' AND INV4.DocEntry	 = T2.DocEntry ANd INV4.LineNum = T1.LineNum
 ) AS OA_IMP
OUTER APPLY 
(
	SELECT TaxSum = ISNULL(SUM(DISTINCT INV4.TaxSum), 0)
	FROM INV4 WITH(NOLOCK)
		INNER JOIN OSTA WITH(NOLOCK) ON INV4.StaCode = OSTA.Code
		INNER JOIN OSTT WITH(NOLOCK) ON OSTA.[Type]  = OSTT.AbsId
		INNER JOIN ONFT WITH(NOLOCK) ON OSTT.NfTaxId = ONFT.AbsId
	WHERE INV4.TaxInPrice = 'Y' AND INV4.DocEntry	 = T2.DocEntry
 ) AS OA_IMP2
OUTER APPLY 
(
	SELECT LineTotal = 
		CASE WHEN T0.DpmAmnt <> 0 THEN 
			CASE WHEN OA_MAX.TotItem = 1 THEN (T1.VatSum - (T0.DpmAmnt - T1.LineTotal)) ELSE T1.LineTotal END
		ELSE T1.LineTotal END
 ) AS OA_TOTAL
WHERE T0.CANCELED = 'N' 
AND OA_DATA.DataPagamento IS NOT NULL 
AND OA_DATA.DataPagamento >= @DataInicial 
AND OA_DATA.DataPagamento <= @DataFinal
AND 
  (
	SELECT COUNT(OACT.AcctCode) FROM JDT1 WITH(NOLOCK)
	INNER JOIN OACT WITH(NOLOCK) ON JDT1.Account = OACT.AcctCode AND OACT.Finanse = 'Y'
	WHERE JDT1.TransId = T15.TransId AND JDT1.TransType = T15.ObjType
  ) <> 0
  
INSERT INTO #tbl_docs_recebimento
SELECT DISTINCT
	'R' AS Momento
	, T0.DocEntry AS DocEntry
	, T0.DocDate AS DocDate
	, ISNULL(T0.Serial, T0.DocEntry) AS Serial
	, T24.DocTotal AS DocTotal
	, T0.ObjType AS ObjType
	, T25.ItemCode AS ItemCode
	, T25.LineNum AS LineNum
	, T25.Quantity AS Quantity
	, ((T23.DrawnSum * T25.LineTotal)/OA_MAX.TotalItens) * (T2.InstPrcnt/100) AS LineTotal
	, T2.InstlmntID AS InstlmntID
	, T2.InsTotal AS InsTotal
	, T2.DueDate AS DueDate
	, T4.ItemName AS ItemName
	, T5.ItmsGrpCod AS ItmsGrpCod
	, T5.ItmsGrpNam AS ItmsGrpNam
	, T0.CardCode AS CardCode
	, T0.CardName AS CardName
	, ISNULL(T8.PrcCode, T9.PrcCode) AS PrcCode
	, ISNULL(T8.PrcName, T9.PrcName) AS PrcName
	, T6.SlpCode AS SlpCode
	, T6.SlpName AS SlpName
	, ISNULL(T6.U_CVA_IMPADC, 'N') AS U_CVA_IMPADIC
	, ISNULL(T6.U_CVA_IMPINCL, 'N') AS U_CVA_IMPINCL
	, T10.FirmCode AS FirmCode
	, T10.FirmName AS FirmName
	, T11.IndCode AS IndCode
	, T11.IndName AS IndName
	, T11.IndDesc AS IndDesc
	, T13.AbsId AS AbsId
	, T13.Name AS Name
	, T13.[State] AS [State]
	, OA_DATA.DataPagamento AS TaxDate
	, T14.SumApplied AS SumApplied
	, OA_MAXX.Countt AS Tot_Invoices
	, T25.VatSum * (T2.InstPrcnt/100) AS TaxSum_IMPADIC_Linha
	, OA_IMP.TaxSum * (T2.InstPrcnt/100) AS TaxSum_IMPINCL_Linha
	, T24.VatSum * (T2.InstPrcnt/100) AS TaxSum_IMPADIC_Parcela
	, OA_IMP2.TaxSum * (T2.InstPrcnt/100) AS TaxSum_IMPINCL_Parcela
	, T0.Installmnt AS Tot_Parcelas
	, OA_MAX.TotItem AS Tot_Itens
	, NULL AS DocStatus
	, T3.GroupCode AS GroupCode
	, OA_MAX.TotalItens AS TotalItens
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN ((((T23.DrawnSum * T25.LineTotal)/OA_MAX.TotalItens) - OA_IMP.TaxSum) + T1.VatSum)* (T2.InstPrcnt/100) ELSE 0 END AS LineTotal_Min_IMPINCL
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN T25.LineTotal ELSE 0 END AS LineTotal_Min_IMPADIC
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN (((T23.DrawnSum * T25.LineTotal)/OA_MAX.TotalItens) - OA_IMP.TaxSum)* (T2.InstPrcnt/100) ELSE 0 END AS LineTotal_Min_IMPINCL_Min_IMPADIC
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN (((T23.DrawnSum * T25.LineTotal)/OA_MAX.TotalItens) + T1.VatSum)* (T2.InstPrcnt/100) ELSE 0 END AS LineTotal_Min_IMPINCL_Min_IMPADIC
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN T14.SumApplied - (OA_IMP2.TaxSum * (T2.InstPrcnt/100)) ELSE 0 END AS InsTotal_Min_IMPINCL
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN T14.SumApplied - (T0.VatSum * (T2.InstPrcnt/100)) ELSE 0 END AS InsTotal_Min_IMPADIC
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN T14.SumApplied - (OA_IMP2.TaxSum * (T2.InstPrcnt/100)) - (T0.VatSum * (T2.InstPrcnt/100)) ELSE 0 END AS InsTotal_Min_IMPINCL_Min_IMPADIC
	, CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN T14.SumApplied ELSE 0 END AS InsTotal_Min_IMPINCL_Min_IMPADIC
FROM ODPI T0 WITH(NOLOCK)
	INNER JOIN DPI1 T1  WITH(NOLOCK) ON T0.DocEntry  = T1.DocEntry  AND T1.TargetType <> N'14'
	INNER JOIN DPI6 T2  WITH(NOLOCK) ON T0.DocEntry  = T2.DocEntry  AND T2.InsTotal <> 0
	INNER JOIN OCRD T3  WITH(NOLOCK) ON T0.CardCode  = T3.CardCode	
	INNER JOIN RCT2 T14 WITH(NOLOCK) ON T2.DocEntry  = T14.DocEntry AND T2.InstlmntID = T14.InstId AND T2.ObjType = T14.InvType
	INNER JOIN ORCT T15 WITH(NOLOCK) ON T14.DocNum   = T15.DocEntry
	LEFT  JOIN OBOE T16 WITH(NOLOCK) ON T15.BoeAbs   = T16.BoeKey
	LEFT  JOIN OBOT T17 WITH(NOLOCK) ON T17.AbsEntry = 
	(
		SELECT TOP 1 BOT1.AbsEntry FROM BOT1 WITH(NOLOCK)
		WHERE BOT1.BOENumber = T16.BoeNum AND BOT1.BoeType = T16.BoeType
		ORDER BY BOT1.AbsEntry DESC
	) 
	 AND T17.StatusTo = 'P'
	 LEFT JOIN OJDT T18  WITH(NOLOCK) ON T17.TransId   = T18.Number
	 LEFT JOIN RCT1 T19  WITH(NOLOCK) ON T15.DocEntry  = T19.DocNum
	 LEFT JOIN OCHH T20  WITH(NOLOCK) ON T19.CheckAbs  = T20.CheckKey
	 LEFT JOIN DPS1 T21  WITH(NOLOCK) ON T20.CheckKey  = T21.CheckKey AND T21.DepCancel = 'N' AND T20.DpstAbs = T21.DepositId
	 LEFT JOIN ODPS T22  WITH(NOLOCK) ON T21.DepositId = T22.DeposId  AND T22.DeposType = 'K'
	INNER JOIN INV9 T23  WITH(NOLOCK) ON T23.BaseAbs   = T0.DocEntry  AND T23.ObjType   = T0.ObjType
	INNER JOIN OINV T24  WITH(NOLOCK) ON T24.DocEntry  = T23.DocEntry
	INNER JOIN INV1 T25  WITH(NOLOCK) ON T24.DocEntry  = T25.DocEntry
	INNER JOIN OITM T4   WITH(NOLOCK) ON T25.ItemCode  = T4.ItemCode
	INNER JOIN OITB T5   WITH(NOLOCK) ON T4.ItmsGrpCod = T5.ItmsGrpCod
	LEFT  JOIN OSLP T6   WITH(NOLOCK) ON T24.SlpCode   = T6.SlpCode
	LEFT  JOIN OPRC T8   WITH(NOLOCK) ON T25.OcrCode2  = T8.PrcCode
	LEFT  JOIN OPRC T9   WITH(NOLOCK) ON T25.OcrCode   = T9.PrcCode
	LEFT  JOIN OMRC T10  WITH(NOLOCK) ON T4.FirmCode   = T10.FirmCode
	LEFT  JOIN OOND T11  WITH(NOLOCK) ON T3.IndustryC  = T11.IndCode
	LEFT  JOIN INV12 T12 WITH(NOLOCK) ON T24.DocEntry  = T12.DocEntry
	LEFT  JOIN OCNT T13  WITH(NOLOCK) ON T12.CountyB   = T13.AbsId
 OUTER APPLY
 (
	SELECT TotItem = COUNT(INV1.LineNum), TotalItens = SUM(INV1.LineTotal) 
	FROM INV1 WITH(NOLOCK) WHERE INV1.DocEntry = T24.DocEntry
 ) AS OA_MAX
 OUTER APPLY
 (
	SELECT Countt = COUNT(RCT2.InvoiceId) FROM RCT2 WITH(NOLOCK) WHERE RCT2.DocNum = T15.DocEntry
 ) AS OA_MAXX
 OUTER APPLY
 (
	SELECT DataPagamento = 
		CASE WHEN T17.StatusTo = 'P' THEN T18.RefDate
		ELSE CASE WHEN T20.CheckKey IS NOT NULL THEN T22.DeposDate
		ELSE CASE WHEN T15.BoeAbs IS NULL AND T19.CheckAbs IS NULL AND T15.DocEntry IS NOT NULL THEN T15.TaxDate
		ELSE NULL END END END
 ) AS OA_DATA
 OUTER APPLY
 (
	SELECT TaxSum = ISNULL(SUM(DISTINCT INV4.TaxSum), 0)
	FROM INV4 WITH(NOLOCK)
		INNER JOIN OSTA WITH(NOLOCK) ON INV4.StaCode = OSTA.Code
		INNER JOIN OSTT WITH(NOLOCK) ON OSTA.[Type]  = OSTT.AbsId
		INNER JOIN ONFT WITH(NOLOCK) ON OSTT.NfTaxId = ONFT.AbsId
	WHERE INV4.TaxInPrice = 'Y' AND INV4.DocEntry	 = T25.DocEntry AND INV4.LineNum = T25.LineNum
 ) AS OA_IMP
 OUTER APPLY 
 (
	SELECT TaxSum = ISNULL(SUM(DISTINCT INV4.TaxSum), 0)
	FROM INV4 WITH(NOLOCK)
		INNER JOIN OSTA WITH(NOLOCK) ON INV4.StaCode = OSTA.Code
		INNER JOIN OSTT WITH(NOLOCK) ON OSTA.[Type] = OSTT.AbsId
		INNER JOIN ONFT WITH(NOLOCK) ON OSTT.NfTaxId = ONFT.AbsId
	WHERE INV4.TaxInPrice = 'Y' AND INV4.DocEntry = T25.DocEntry
 ) AS OA_IMP2
WHERE T0.CANCELED = 'N'
 AND OA_DATA.DataPagamento IS NOT NULL 
 AND OA_DATA.DataPagamento >= @DataInicial 
 AND OA_DATA.DataPagamento <= @DataFinal
 AND
(
	SELECT COUNT(OACT.AcctCode) FROM JDT1 WITH(NOLOCK)
	INNER JOIN OACT WITH(NOLOCK) ON JDT1.Account = OACT.AcctCode AND OACT.Finanse <> 'Y'
	WHERE JDT1.TransId = T15.TransId AND JDT1.TransType = T15.ObjType
) <> 2
INSERT INTO #tbl_docs_recebimento
SELECT DISTINCT
	'R' AS Momento
	, T0.DocEntry AS DocEntry
	, T0.DocDate AS DocDate
	, ISNULL(T0.Serial, T0.DocEntry) AS Serial
	, T0.DocTotal AS DocTotal
	, T0.ObjType AS ObjType
	, T1.ItemCode AS ItemCode
	, T1.LineNum AS LineNum
	, T1.Quantity AS Quantity
	, T1.LineTotal * (T2.InstPrcnt/100) AS LineTotal
	, T2.InstlmntID AS InstlmntID
	, T2.InsTotal AS InsTotal
	, T2.DueDate AS DueDate
	, T4.ItemName AS ItemName
	, T5.ItmsGrpCod AS ItmsGrpCod
	, T5.ItmsGrpNam AS ItmsGrpNam
	, T0.CardCode AS CardCode
	, T0.CardName AS CardName
	, ISNULL(T8.PrcCode, T9.PrcCode) AS PrcCode
	, ISNULL(T8.PrcName, T9.PrcName) AS PrcName
	, T6.SlpCode AS SlpCode
	, T6.SlpName AS SlpName
	, ISNULL(T6.U_CVA_IMPADC, 'N') AS U_CVA_IMPADIC
	, ISNULL(T6.U_CVA_IMPINCL, 'N') AS U_CVA_IMPINCL
	, T10.FirmCode AS FirmCode
	, T10.FirmName AS FirmName
	, T11.IndCode AS IndCode
	, T11.IndName AS IndName
	, T11.IndDesc AS IndDesc
	, T13.AbsId AS AbsId
	, T13.Name AS Name
	, T13.[State] AS [State]
	, OA_DATA.DataPagamento AS TaxDate
	, T14.SumApplied AS SumApplied
	, OA_MAXX.Countt AS Tot_Invoices
	, T1.VatSum * (T2.InstPrcnt/100) AS TaxSum_IMPADIC_Linha
	, OA_IMP.TaxSum * (T2.InstPrcnt/100) AS TaxSum_IMPINCL_Linha
	, T0.VatSum * (T2.InstPrcnt/100) AS TaxSum_IMPADIC_Parcela
	, OA_IMP2.TaxSum * (T2.InstPrcnt/100) AS TaxSum_IMPINCL_Parcela
	, T0.Installmnt AS Tot_Parcelas
	, OA_MAX.TotItem AS Tot_Itens
	, NULL AS DocStatus
	, T3.GroupCode AS GroupCode
	, OA_MAX.TotalItens AS TotalItens
	, -CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN ((T1.LineTotal - OA_IMP.TaxSum) + T1.VatSum)* (T2.InstPrcnt/100) ELSE 0 END AS LineTotal_Min_IMPINCL
	, -CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN T1.LineTotal* (T2.InstPrcnt/100) ELSE 0 END AS LineTotal_Min_IMPADIC
	, -CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN (T1.LineTotal - OA_IMP.TaxSum)* (T2.InstPrcnt/100) ELSE 0 END AS LineTotal_Min_IMPINCL_Min_IMPADIC
	, -CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN (T1.LineTotal + T1.VatSum)* (T2.InstPrcnt/100) ELSE 0 END AS LineTotal_Min_IMPINCL_Min_IMPADIC
	, -CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN T14.SumApplied - (OA_IMP2.TaxSum * (T2.InstPrcnt/100)) ELSE 0 END AS InsTotal_Min_IMPINCL
	, -CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN T14.SumApplied - (T0.VatSum * (T2.InstPrcnt/100)) ELSE 0 END AS InsTotal_Min_IMPADIC
	, -CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN T14.SumApplied - (OA_IMP2.TaxSum * (T2.InstPrcnt/100)) - (T0.VatSum * (T2.InstPrcnt/100)) ELSE 0 END AS InsTotal_Min_IMPINCL_Min_IMPADIC
	, -CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN T14.SumApplied ELSE 0 END AS InsTotal_Min_IMPINCL_Min_IMPADIC
FROM ORIN T0 WITH(NOLOCK)
	INNER JOIN RIN1 T1   WITH(NOLOCK) ON T0.DocEntry   = T1.DocEntry AND T1.TargetType <> N'14' AND ISNULL(T1.FreeChrgBP, 'N') = 'N'
	INNER JOIN RIN6 T2   WITH(NOLOCK) ON T0.DocEntry   = T2.DocEntry AND T2.InsTotal   <> 0
	INNER JOIN OCRD T3   WITH(NOLOCK) ON T0.CardCode   = T3.CardCode
	INNER JOIN OITM T4   WITH(NOLOCK) ON T1.ItemCode   = T4.ItemCode
	INNER JOIN OITB T5   WITH(NOLOCK) ON T4.ItmsGrpCod = T5.ItmsGrpCod
	INNER JOIN OSLP T6   WITH(NOLOCK) ON T0.SlpCode    = T6.SlpCode
	LEFT  JOIN OPRC T8   WITH(NOLOCK) ON T1.OcrCode2   = T8.PrcCode
	LEFT  JOIN OPRC T9   WITH(NOLOCK) ON T1.OcrCode    = T9.PrcCode
	LEFT  JOIN OMRC T10  WITH(NOLOCK) ON T4.FirmCode   = T10.FirmCode
	LEFT  JOIN OOND T11  WITH(NOLOCK) ON T3.IndustryC  = T11.IndCode
	LEFT  JOIN RIN12 T12 WITH(NOLOCK) ON T0.DocEntry   = T12.DocEntry
	LEFT  JOIN OCNT T13  WITH(NOLOCK) ON T12.CountyB   = T13.AbsId
	INNER JOIN RCT2 T14  WITH(NOLOCK) ON T2.DocEntry   = T14.DocEntry AND T2.InstlmntID = T14.InstId AND T2.ObjType = T14.InvType
	INNER JOIN ORCT T15  WITH(NOLOCK) ON T14.DocNum    = T15.DocEntry
	LEFT  JOIN OBOE T16  WITH(NOLOCK) ON T15.BoeAbs    = T16.BoeKey
	LEFT  JOIN OBOT T17  WITH(NOLOCK) ON T17.AbsEntry  = 
	(
		SELECT TOP 1 BOT1.AbsEntry FROM BOT1 WITH(NOLOCK)
		WHERE BOT1.BOENumber = T16.BoeNum AND BOT1.BoeType = T16.BoeType
		ORDER BY BOT1.AbsEntry DESC
	) 
	AND T17.StatusTo = 'P'
	LEFT JOIN OJDT T18 WITH(NOLOCK) ON T17.TransId   = T18.Number
	LEFT JOIN RCT1 T19 WITH(NOLOCK) ON T15.DocEntry  = T19.DocNum
	LEFT JOIN OCHH T20 WITH(NOLOCK) ON T19.CheckAbs  = T20.CheckKey
	LEFT JOIN DPS1 T21 WITH(NOLOCK) ON T20.CheckKey  = T21.CheckKey AND T21.DepCancel = 'N' AND T20.DpstAbs = T21.DepositId
	LEFT JOIN ODPS T22 WITH(NOLOCK) ON T21.DepositId = T22.DeposId  AND T22.DeposType = 'K'
OUTER APPLY 
 (
	SELECT TotItem = COUNT(RIN1.LineNum), TotalItens = SUM(RIN1.LineTotal) 
	FROM RIN1 WITH(NOLOCK) WHERE RIN1.DocEntry = T0.DocEntry AND RIN1.TargetType <> N'14' AND ISNULL(RIN1.FreeChrgBP, 'N') = 'N'
 ) AS OA_MAX
OUTER APPLY 
 (
	SELECT Countt = COUNT(RCT2.InvoiceId) FROM RCT2 WITH(NOLOCK) WHERE RCT2.DocNum = T15.DocEntry
 ) AS OA_MAXX
OUTER APPLY 
 (
	SELECT DataPagamento = 
		CASE WHEN T17.StatusTo = 'P' THEN T18.RefDate
		ELSE CASE WHEN T20.CheckKey IS NOT NULL THEN T22.DeposDate
		ELSE CASE WHEN T15.BoeAbs IS NULL AND T19.CheckAbs IS NULL AND T15.DocEntry IS NOT NULL THEN T15.TaxDate
		ELSE NULL END END END
 ) AS OA_DATA
OUTER APPLY 
 (
	SELECT TaxSum = ISNULL(SUM(DISTINCT RIN4.TaxSum), 0)
	FROM RIN4 WITH(NOLOCK)
		INNER JOIN OSTA WITH(NOLOCK) ON RIN4.StaCode = OSTA.Code
		INNER JOIN OSTT WITH(NOLOCK) ON OSTA.[Type] = OSTT.AbsId
		INNER JOIN ONFT WITH(NOLOCK) ON OSTT.NfTaxId = ONFT.AbsId
	WHERE RIN4.TaxInPrice = 'Y' AND RIN4.DocEntry = T2.DocEntry ANd RIN4.LineNum = T1.LineNum
 ) AS OA_IMP
OUTER APPLY
 (
	SELECT TaxSum = ISNULL(SUM(DISTINCT RIN4.TaxSum), 0)
	FROM RIN4 WITH(NOLOCK)
		INNER JOIN OSTA WITH(NOLOCK) ON RIN4.StaCode = OSTA.Code
		INNER JOIN OSTT WITH(NOLOCK) ON OSTA.[Type] = OSTT.AbsId
		INNER JOIN ONFT WITH(NOLOCK) ON OSTT.NfTaxId = ONFT.AbsId
	WHERE RIN4.TaxInPrice = 'Y' AND RIN4.DocEntry = T2.DocEntry
 ) AS OA_IMP2
WHERE T0.CANCELED = 'N' 
AND OA_DATA.DataPagamento IS NOT NULL 
AND OA_DATA.DataPagamento >= @DataInicial 
AND OA_DATA.DataPagamento <= @DataFinal
AND 
 (
	SELECT COUNT(OACT.AcctCode) FROM JDT1 WITH(NOLOCK)
	INNER JOIN OACT WITH(NOLOCK) ON JDT1.Account = OACT.AcctCode AND OACT.Finanse <> 'Y'
	WHERE JDT1.TransId = T15.TransId AND JDT1.TransType = T15.ObjType
 ) <> 2

INSERT INTO #tbl_docs_aux 
SELECT DISTINCT 
	T0.Momento
	, T0.DocEntry
	, T0.DocDate
	, T0.Serial
	, T0.DocTotal
	, T0.ObjType
	, T0.ItemCode
	, T0.LineNum
	, T0.Quantity
	, T0.LineTotal
	, 0
	, 0
	, NULL
	, T0.ItemName
	, T0.ItmsGrpCod
	, T0.ItmsGrpNam
	, T0.CardCode
	, T0.CardName
	, T0.PrcCode
	, T0.PrcName
	, T0.SlpCode
	, T0.SlpName
	, T0.U_CVA_IMPADIC
	, T0.U_CVA_IMPINCL
	, T0.FirmCode
	, T0.FirmName
	, T0.IndCode
	, T0.IndName
	, T0.IndDesc
	, T0.AbsId
	, T0.Name
	, T0.[State]
	, T0.TaxDate
	, T0.SumApplied
	, T0.Tot_Invoices
	, T0.TaxSum_IMPADIC_Linha
	, T0.TaxSum_IMPINCL_Linha
	, T0.TaxSum_IMPADIC_Parcela
	, T0.TaxSum_IMPINCL_Parcela
	, T0.Tot_Parcelas
	, T0.Tot_Itens
	, T0.DocStatus
	, T0.GroupCode
	, T0.TotalItens
	, T0.LineTotal_Min_IMPINCL
	, T0.LineTotal_Min_IMPADIC
	, T0.LineTotal_Min_IMPADIC_Min_IMPINCL
	, T0.LineTotal_Plus_IMPADIC_Plus_IMPINCL
	, T0.InsTotal_Min_IMPINCL
	, T0.InsTotal_Min_IMPADIC
	, T0.InsTotal_Min_IMPINCL_Min_IMPADIC
	, T0.InsTotal_Plus_IMPINCL_Plus_IMPADIC
FROM #tbl_docs_faturamento T0
UNION ALL
SELECT DISTINCT * FROM #tbl_docs_pedido T0
UNION ALL
SELECT DISTINCT * FROM #tbl_docs_recebimento T0
ORDER BY 1, 6, 2, 11, 8

BEGIN --CURSOR PARA BUSCAR CONDIÇÕES DE CRITÉRIOS WHERE
SET @cur = CURSOR FAST_FORWARD FOR
	SELECT DISTINCT
		T0.Code AS CritCode, T0.Name AS CritName, T0.U_POS AS CritPos, T0.U_ATIVO AS CritAtivo,
		T1.Code AS RegrCode, T1.Name AS RegrName, T1.U_TIPO, T1.U_COMISSIONADO, T1.U_MOMENTO, 
		T1.U_VENDEDOR, T1.U_ITEM, T1.U_GRUPO, T1.U_CENTROCUSTO, T1.U_FABRICANTE, T1.U_CLIENTE, 
		T1.U_CIDADE, T1.U_ESTADO, T1.U_SETOR, T1.U_COMISSAO, T1.U_GROUP
	FROM [@CVA_CRIT_COMISSAO] T0  WITH(NOLOCK)
	INNER JOIN [@CVA_REGR_COMISSAO] T1 WITH(NOLOCK) ON T0.Code = T1.U_PRIORIDADE
	WHERE T0.U_ATIVO = 'Y' AND T1.U_ATIVO = 'Y'
	ORDER BY T1.U_COMISSIONADO, T1.U_MOMENTO, T0.U_POS		

OPEN @cur
FETCH NEXT FROM @cur INTO @CritCode, 
                         @CritName, 
						@CritPos, 
					   @CritAtivo, 
					  @RegrCode, 
					 @RegrName, 
					@Tipo, 
				   @Comissionado, 
				  @Momento, 
				 @Vendedor, 
				  @Item, 
				   @Grupo, 
				    @CentroCusto, 
					 @Fabricante, 
					  @Cliente, 
					   @Cidade, 
					    @Estado, 
						 @Setor, 
						  @Comissao, 
						   @GroupCode

SET @Where = ''

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @WHERE = 'Momento = ''' + @Momento + ''''

	if isnull(@Vendedor, isnull(@Comissionado, 0)) <> 0  set @Where = @Where + ' AND SlpCode = ' + cast(isnull(@Vendedor, @Comissionado) as nvarchar(10))
	if isnull(@Item, '') <> ''						   set @Where = @Where + ' AND ItemCode = ''' + @Item + ''''
	if isnull(@Grupo, 0) <> 0						 set @Where = @Where + ' AND ItmsGrpCod = ' + cast(@Grupo as nvarchar(10))
	if isnull(@CentroCusto, '') <> ''			   set @Where = @Where + ' AND PrcCode = ''' + @CentroCusto + ''''
	if isnull(@Fabricante, 0) <> 0				 set @Where = @Where + ' AND FirmCode = ' + cast(@Fabricante as nvarchar(10))
	if isnull(@Cliente, '') <> ''			   set @Where = @Where + ' AND CardCode = ''' + @Cliente + ''''
	if isnull(@Cidade, 0) <> 0				 set @Where = @Where + ' AND AbsId = ' + cast(@Cidade as nvarchar(10))
	if isnull(@Estado, '') <> ''		   set @Where = @Where + ' AND [State] = ''' + @Estado + ''''
	if isnull(@Setor, 0) <> 0            set @Where = @Where + ' AND IndCode = ''' + cast(@Setor as nvarchar(10)) + ''''
	if isnull(@GroupCode, 0) <> 0	   set @Where = @Where + ' AND GroupCode = ' + cast(@GroupCode as nvarchar(10))

	insert into #tmp_table_where values(@RegrCode, @Comissionado, @Comissao, @Momento, @CritPos, @Where)

	set @Where = ''
	fetch next from @cur into @CritCode, 
							@CritName, 
						   @CritPos, 
						  @CritAtivo, 
						 @RegrCode, 
						@RegrName, 
					   @Tipo, 
					  @Comissionado, 
					 @Momento, 
					@Vendedor,
				     @Item, 
				      @Grupo, 
					   @CentroCusto, 
					    @Fabricante, 
						 @Cliente, 
						  @Cidade, 
						   @Estado, 
						    @Setor, 
	                         @Comissao, 
	                          @GroupCode
end

close @cur
deallocate @cur
end

set @cur = cursor fast_forward for
	select RegrCode, Comissionado, Comissao/100, Momento, Prioridade, SqlWhere from #tmp_table_where order by Momento, Comissionado, Prioridade

open @cur
fetch next from @cur into @RegrCode, 
				           @Comissionado, 
						    @Comissao, 
						     @Momento, 
							  @Prioridade, 
							   @Where

while @@fetch_status = 0
begin
	declare @isItem char(1) = 'N'

		    if @Where like '%ItemCode%'    begin set @isItem = 'Y' end
	else   if @Where like '%ItmsGrpCod%' begin set @isItem = 'Y' end
	else  if @Where like '%FirmCode%'  begin set @isItem = 'Y' end
	else if @Where like '%PrcCode%' begin set @isItem = 'Y' end
	else begin set @isItem = 'N' end

	SET @Query = N'SELECT DISTINCT @P1, @P2, @P3, @P4, @P5, * FROM #tbl_docs_aux WHERE ' + @Where
	SET @Params = N'@P1 INT, @P2 INT, @P3 NUMERIC(19,6), @P4 INT, @P5 CHAR(1)'

	INSERT INTO #tbl_docs_fat_aux
		EXEC sp_executesql @Query, @Params, @P1 = @Regrcode, @P2 = @Comissionado, @P3 = @Comissao, @P4 = @Prioridade, @P5 = @isItem
		
	fetch next from @cur into @RegrCode, @Comissionado, @Comissao, @Momento, @Prioridade, @Where
END

CLOSE @cur
DEALLOCATE @cur

;with cte as (
				select *
					, RN = row_number() over (partition by Comissionado, Momento, ObjType, DocEntry, InstlmntID, LineNum order by Prioridade)
				from #tbl_docs_fat_aux
			)
delete from cte 
where RN > 1

insert into #tbl_docs_fat
	select * from #tbl_docs_fat_aux

insert into #tmp_table_ret
	select 
		 T0.U_COMISSIONADO
		,T2.U_POS AS U_PRIORIDADE
		,T0.U_CARDCODE
		,T0.U_CARDNAME
		,T0.U_REGRA
		,T0.U_DOCDATE
		,T0.U_DUEDATE
		,T0.U_DOCENTRY
		,T0.U_OBJTYPE
		,T0.U_ITEMCODE
		,T0.U_ITEMNAME
		,T0.U_LINENUM
		,T0.U_VALOR
		,T0.U_PARCELA
		,T0.U_IMPOSTOS
		,T0.U_COMISSAO
		,T0.U_VALORCOMISSAO
		,T0.U_TAXDATE
		,T0.U_CENTROCUSTO
		,T0.U_PAGO
		,T0.U_DATAPAGAMENTO
		,T1.U_MOMENTO
		,CASE WHEN T0.U_OBJTYPE = N'13' THEN
			(SELECT Serial FROM OINV WITH(NOLOCK) WHERE DocEntry = T0.U_DOCENTRY)
		 ELSE T0.U_DOCENTRY END AS U_SERIAL
	from [@CVA_CALC_COMISSAO] T0 WITH(NOLOCK)
   inner join [@CVA_REGR_COMISSAO] T1 WITH(NOLOCK) 
      ON T0.U_REGRA = T1.Code 
	 AND T1.U_ATIVO = 'Y'
   inner join [@CVA_CRIT_COMISSAO] T2 WITH(NOLOCK) 
      ON T1.U_PRIORIDADE = T2.Code
   where T0.U_PAGO = 'Y'
	 and T0.U_DATAPAGAMENTO >= @DataInicial
	 and T0.U_DATAPAGAMENTO <= @DataFinal
	 and @Status IN ('P', 'T')
		
INSERT INTO #tmp_table_ret
	SELECT DISTINCT
		T0.Comissionado
		,T0.Prioridade
		,T0.CardCode
		,T0.CardName
		,T0.RegrCode
		,T0.DocDate
		,T0.DocDate
		,T0.DocEntry
		,T0.ObjType
		,T0.ItemCode
		,T0.ItemName
		,T0.LineNum	
		,CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'N' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'N' THEN 
			CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal_Min_IMPADIC_Min_IMPINCL ELSE T0.InsTotal_Min_IMPINCL_Min_IMPADIC END
		ELSE CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'Y' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'N' THEN 
			CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal_Min_IMPINCL ELSE T0.InsTotal_Min_IMPINCL END
		ELSE CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'N' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'Y' THEN 
			CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal_Min_IMPADIC ELSE T0.InsTotal_Min_IMPADIC END
		ELSE CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'Y' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'Y' THEN 
			CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal_Plus_IMPADIC_Plus_IMPINCL ELSE T0.InsTotal_Plus_IMPINCL_Plus_IMPADIC END
		END END END END AS Valor
		,T0.InstlmntID
		,CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'N' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'N' THEN 
			CASE WHEN T0.IsItem = 'Y' THEN -T0.TaxSum_IMPINCL_Linha - T0.TaxSum_IMPADIC_Linha ELSE -T0.TaxSum_IMPINCL_Parcela - T0.TaxSum_IMPADIC_Parcela END
		ELSE CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'Y' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'N' THEN 
			CASE WHEN T0.IsItem = 'Y' THEN -T0.TaxSum_IMPINCL_Linha + T0.TaxSum_IMPADIC_Linha ELSE -T0.TaxSum_IMPINCL_Parcela + T0.TaxSum_IMPADIC_Parcela END
		ELSE CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'N' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'Y' THEN 
			CASE WHEN T0.IsItem = 'Y' THEN +T0.TaxSum_IMPINCL_Linha - T0.TaxSum_IMPADIC_Linha ELSE +T0.TaxSum_IMPINCL_Parcela - T0.TaxSum_IMPADIC_Parcela END
		ELSE CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'Y' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'Y' THEN 
			CASE WHEN T0.IsItem = 'Y' THEN +T0.TaxSum_IMPADIC_Linha + T0.TaxSum_IMPINCL_Linha ELSE +T0.TaxSum_IMPADIC_Parcela + T0.TaxSum_IMPINCL_Parcela END
		END END END END AS Impostos
		,T0.Comissao*100 AS Comissao
		,(
		CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'N' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'N' THEN 
			CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal_Min_IMPADIC_Min_IMPINCL ELSE T0.InsTotal_Min_IMPINCL_Min_IMPADIC END
		ELSE CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'Y' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'N' THEN 
			CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal_Min_IMPINCL ELSE T0.InsTotal_Min_IMPINCL END
		ELSE CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'N' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'Y' THEN 
			CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal_Min_IMPADIC ELSE T0.InsTotal_Min_IMPADIC END
		ELSE CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'Y' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'Y' THEN 
			CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal_Plus_IMPADIC_Plus_IMPINCL ELSE T0.InsTotal_Plus_IMPINCL_Plus_IMPADIC END
		END END END END
		)*T0.Comissao AS ValorComissao
		,T0.TaxDate
		,T0.PrcCode
		,'N' AS Pago
		,NULL AS DataPagamento
		,T0.Momento
		,T0.Serial 
	FROM #tbl_docs_fat T0
	LEFT JOIN #tmp_table_ret T1 WITH(NOLOCK) 
	  ON T0.DocEntry = T1.U_DOCENTRY 
	 AND T0.ObjType = T1.U_OBJTYPE 
	 AND T0.LineNum = T1.U_LINENUM 
	 AND T0.InstlmntID = T1.U_PARCELA 
	 AND T0.RegrCode = T1.U_REGRA
   WHERE @Status IN ('N', 'T') 
     AND T1.U_DOCENTRY IS NULL

	if @View = 'T'
	begin
		select distinct
			 U_COMISSIONADO
			,U_PRIORIDADE
			,U_CARDCODE
			,U_CARDNAME
			,U_REGRA
			,U_DOCDATE
			,U_DUEDATE
			,U_DOCENTRY
			,U_OBJTYPE
			,U_ITEMCODE
			,U_ITEMNAME
			,U_LINENUM
			,U_VALOR
			,U_PARCELA
			,U_IMPOSTOS
			,U_COMISSAO
			,U_VALORCOMISSAO
			,ISNULL(U_TAXDATE, '1900-01-01') AS U_TAXDATE
			,isnull(U_CENTROCUSTO, '') AS U_CENTROCUSTO
			,MAX(U_PAGO) AS U_PAGO
			,MAX(isnull(U_DATAPAGAMENTO, '1900-01-01')) as U_DATAPAGAMENTO
			,CASE U_MOMENTO WHEN 'R' THEN 'Recebimento' WHEN 'P' THEN 'Pedido' WHEN 'F' THEN 'Faturamento' END AS U_MOMENTO
			,U_SERIAL
		from #tmp_table_ret	
		where U_VALORCOMISSAO <> 0 
		AND (
				((U_DOCDATE >= @DataInicial AND U_DOCDATE <= @DataFinal) AND U_MOMENTO <> 'R')
				 OR
				((ISNULL(U_TAXDATE, '1900-01-01')>= @DataInicial AND ISNULL(U_TAXDATE, '1900-01-01')<= @DataFinal) AND U_MOMENTO = 'R')
			)
		group by 
			 U_COMISSIONADO
			,U_PRIORIDADE
			,U_CARDCODE
			,U_CARDNAME
			,U_REGRA
			,U_DOCDATE
			,U_DUEDATE
			,U_DOCENTRY
			,U_OBJTYPE
			,U_ITEMCODE
			,U_ITEMNAME
			,U_LINENUM
			,U_VALOR
			,U_PARCELA
			,U_IMPOSTOS
			,U_COMISSAO
			,U_VALORCOMISSAO
			,ISNULL(U_TAXDATE, '1900-01-01')
			,isnull(U_CENTROCUSTO, '')
			,CASE U_MOMENTO WHEN 'R' THEN 'Recebimento' WHEN 'P' THEN 'Pedido' WHEN 'F' THEN 'Faturamento' END
			,U_SERIAL			
	end
	else if @View = 'N'
	begin
		select distinct 
			 U_COMISSIONADO
			,U_PRIORIDADE
			,U_CARDCODE
			,U_CARDNAME
			,U_REGRA
			,U_DOCDATE
			,U_DUEDATE
			,U_DOCENTRY
			,U_OBJTYPE
			,null as U_ITEMCODE
			,null as U_ITEMNAME
			,null as U_LINENUM
			,SUM(DISTINCT U_VALOR) AS U_VALOR
			,U_PARCELA
			,SUM(DISTINCT U_IMPOSTOS) AS U_IMPOSTOS
			,AVG(U_COMISSAO) AS U_COMISSAO
			,SUM(DISTINCT U_VALORCOMISSAO) AS U_VALORCOMISSAO
			,ISNULL(U_TAXDATE, '1900-01-01') AS U_TAXDATE
			,NULL AS U_CENTROCUSTO
			,MAX(U_PAGO) AS U_PAGO
			,MAX(isnull(U_DATAPAGAMENTO, '1900-01-01')) as U_DATAPAGAMENTO
			,CASE U_MOMENTO WHEN 'R' THEN 'Recebimento' WHEN 'P' THEN 'Pedido' WHEN 'F' THEN 'Faturamento' END AS U_MOMENTO
			,U_SERIAL
		from #tmp_table_ret		
		where U_VALORCOMISSAO <> 0 
		AND (
				((U_DOCDATE >= @DataInicial AND U_DOCDATE <= @DataFinal) AND U_MOMENTO <> 'R')
				 OR
				((ISNULL(U_TAXDATE, '1900-01-01')>= @DataInicial AND ISNULL(U_TAXDATE, '1900-01-01')<= @DataFinal) AND U_MOMENTO = 'R')
			)
		group by 
			 U_COMISSIONADO
			,U_PRIORIDADE
			,U_CARDCODE
			,U_CARDNAME 
			,U_REGRA 
			,U_DOCDATE 
			,U_DUEDATE 
			,U_DOCENTRY 
			,U_OBJTYPE 
			,U_PARCELA 
			,CASE U_MOMENTO WHEN 'R' THEN 'Recebimento' WHEN 'P' THEN 'Pedido' WHEN 'F' THEN 'Faturamento' END
			,U_SERIAL
			,ISNULL(U_TAXDATE, '1900-01-01')
	end
	else if @View = 'I'
	begin
		select distinct
			 U_COMISSIONADO 
			,U_PRIORIDADE
			,U_CARDCODE
			,U_CARDNAME
			,U_REGRA
			,U_DOCDATE
			,U_DUEDATE
			,U_DOCENTRY
			,U_OBJTYPE
			,U_ITEMCODE
			,U_ITEMNAME
			,U_LINENUM
			,U_VALOR
			,U_PARCELA
			,U_IMPOSTOS
			,U_COMISSAO
			,U_VALORCOMISSAO
			,ISNULL(U_TAXDATE, '1900-01-01') AS U_TAXDATE
			,isnull(U_CENTROCUSTO, '') AS U_CENTROCUSTO
			,MAX(U_PAGO) AS U_PAGO
			,MAX(isnull(U_DATAPAGAMENTO, '1900-01-01')) as U_DATAPAGAMENTO
			,CASE U_MOMENTO WHEN 'R' THEN 'Recebimento' WHEN 'P' THEN 'Pedido' WHEN 'F' THEN 'Faturamento' END AS U_MOMENTO
			,U_SERIAL
		from #tmp_table_ret
		where U_VALORCOMISSAO <> 0 
		AND (
				((U_DOCDATE >= @DataInicial AND U_DOCDATE <= @DataFinal) AND U_MOMENTO <> 'R')
				 OR
				((ISNULL(U_TAXDATE, '1900-01-01')>= @DataInicial AND ISNULL(U_TAXDATE, '1900-01-01')<= @DataFinal) AND U_MOMENTO = 'R')
			)
		group by 
			 U_COMISSIONADO 
			,U_PRIORIDADE
			,U_CARDCODE
			,U_CARDNAME
			,U_REGRA
			,U_DOCDATE
			,U_DUEDATE
			,U_DOCENTRY
			,U_OBJTYPE
			,U_ITEMCODE
			,U_ITEMNAME
			,U_LINENUM
			,U_VALOR
			,U_PARCELA
			,U_IMPOSTOS
			,U_COMISSAO
			,U_VALORCOMISSAO
			,ISNULL(U_TAXDATE, '1900-01-01')
			,isnull(U_CENTROCUSTO, '')
			,CASE U_MOMENTO WHEN 'R' THEN 'Recebimento' WHEN 'P' THEN 'Pedido' WHEN 'F' THEN 'Faturamento' END
			,U_SERIAL
	end

DROP TABLE #tbl_docs_aux
DROP TABLE #tbl_docs_faturamento
DROP TABLE #tbl_docs_pedido
DROP TABLE #tbl_docs_recebimento
DROP TABLE #tmp_table_where
DROP TABLE #tbl_docs_fat
DROP TABLE #tmp_table_ret
DROP TABLE #tbl_docs_fat_aux
end

GO

EXEC [spc_CVA_Comissoes] 'T','T','2018-04-01','2018-04-30'