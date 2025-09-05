CREATE procedure [dbo].[spc_CVA_Recebimentos]
(
	@Filial				int,
	@DataInicial		datetime, 
	@DataFinal			datetime,
	@MetaDataInicial	datetime	= NULL, 
	@MetaDataFinal		datetime	= NULL
)
as
BEGIN
	set dateformat 'ymd';
	set nocount on;

	IF @MetaDataInicial IS NULL
	BEGIN
		SET @MetaDataInicial = @DataInicial
	END
	IF @MetaDataFinal IS NULL
	BEGIN
		SET @MetaDataFinal = @DataFinal
	END

	-- CRIAÇÃO DE TABELAS
	BEGIN
	CREATE TABLE #tbl_docs_recebimento
	(
		Momento CHAR(1) Collate Database_Default,
		BPLId INT,
		DocEntry INT,
		DocDate DATETIME,
		Serial INT,
		DocTotal NUMERIC(19,6),
		ObjType NVARCHAR(40) Collate Database_Default,
		ItemCode NVARCHAR(100) Collate Database_Default,
		LineNum INT,
		Quantity NUMERIC(19,6),
		LineTotal NUMERIC(19,6),
		InstlmntID INT,
		InsTotal NUMERIC(19,6),
		DueDate DATETIME,
		ItemName NVARCHAR(200) Collate Database_Default,
		ItmsGrpCod SMALLINT,
		ItmsGrpNam NVARCHAR(40) Collate Database_Default,
		CardCode NVARCHAR(30) Collate Database_Default,
		CardName NVARCHAR(200) Collate Database_Default,
		PrcCode NVARCHAR(16) Collate Database_Default,
		PrcName NVARCHAR(60) Collate Database_Default,
		SlpCode INT,
		SlpName NVARCHAR(310) Collate Database_Default,
		U_CVA_IMPADIC CHAR(1) Collate Database_Default,
		U_CVA_IMPINCL CHAR(1) Collate Database_Default,
		FirmCode SMALLINT,
		FirmName NVARCHAR(60) Collate Database_Default,
		IndCode INT,
		IndName NVARCHAR(30) Collate Database_Default,
		IndDesc NVARCHAR(60) Collate Database_Default,
		AbsId INT,
		Name NVARCHAR(200) Collate Database_Default,
		[State] NVARCHAR(6) Collate Database_Default,
		TaxDate DATETIME,
		SumApplied NUMERIC(19,6),
		Tot_Invoices INT,
		Tot_Parcelas INT,
		Tot_Itens INT,
		DocStatus CHAR(1) Collate Database_Default,
		GroupCode INT,
		TotalItens NUMERIC(19,6),
		TaxSum NUMERIC(19,6),
		BaseSum NUMERIC(19,6)
	)
	END

	INSERT INTO #tbl_docs_recebimento
	SELECT DISTINCT
		'R' AS Momento
		, T0.BPLId
		, T0.DocEntry AS DocEntry
		, T0.DocDate AS DocDate
		, ISNULL(T0.Serial, T0.DocEntry) AS Serial
		, T0.DocTotal AS DocTotal
		, T0.ObjType AS ObjType
		, T1.ItemCode AS ItemCode
		, T1.LineNum AS LineNum
		, T1.Quantity AS Quantity
		, T1.LineTotal * (T2.InstPrcnt/100)
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
		, 0 AS Tot_Invoices
		, T0.Installmnt AS Tot_Parcelas
		, OA_MAX.TotItem AS Tot_Itens
		, NULL AS DocStatus
		, T3.GroupCode AS GroupCode
		, OA_MAX.TotalItens AS TotalItens
		, ISNULL(IMPOSTO.TaxSum, 0) * (T2.InstPrcnt/100)
		, T14.SumApplied - (ISNULL(IMPOSTO.TaxSum, 0) / T0.Installmnt)
	FROM OINV T0 WITH(NOLOCK)
		INNER JOIN INV1 T1 WITH(NOLOCK) ON T0.DocEntry = T1.DocEntry AND ISNULL(T1.FreeChrgBP, 'N') = 'N'
		INNER JOIN INV6 T2 WITH(NOLOCK) ON T0.DocEntry = T2.DocEntry AND T2.InsTotal <> 0
		INNER JOIN OCRD T3 WITH(NOLOCK) ON T0.CardCode = T3.CardCode
		INNER JOIN OITM T4 WITH(NOLOCK) ON T1.ItemCode = T4.ItemCode
		INNER JOIN OITB T5 WITH(NOLOCK) ON T4.ItmsGrpCod = T5.ItmsGrpCod
		INNER JOIN OSLP T6 WITH(NOLOCK) ON T0.SlpCode = T6.SlpCode
	--	LEFT  JOIN OSLP T7 WITH(NOLOCK) ON T3.SlpCode = T7.SlpCode
		LEFT  JOIN OPRC T8 WITH(NOLOCK) ON T1.OcrCode2 = T8.PrcCode
		LEFT  JOIN OPRC T9 WITH(NOLOCK) ON T1.OcrCode = T9.PrcCode
		LEFT  JOIN OMRC T10 WITH(NOLOCK) ON T4.FirmCode = T10.FirmCode
		LEFT  JOIN OOND T11 WITH(NOLOCK) ON T3.IndustryC = T11.IndCode
		LEFT  JOIN INV12 T12 WITH(NOLOCK) ON T0.DocEntry = T12.DocEntry
		LEFT  JOIN OCNT T13 WITH(NOLOCK) ON T12.CountyB = T13.AbsId
		LEFT JOIN 
		(
			SELECT ORCT.BoeAbs, ORCT.TaxDate, ORCT.ObjType, ORCT.TransId, RCT2.SumApplied, RCT2.DocEntry, RCT2.DocNum, RCT2.InstId, RCT2.InvType FROM RCT2 WITH(NOLOCK)  
			INNER JOIN ORCT WITH(NOLOCK) ON ORCT.DocEntry = RCT2.DocNum AND ORCT.CANCELED <> 'Y'
			WHERE ORCT.TaxDate BETWEEN @DataInicial AND @DataFinal
		) T14 ON T2.DocEntry = T14.DocEntry AND T2.InstlmntID = T14.InstId AND T2.ObjType = T14.InvType

		LEFT  JOIN OBOE T16 WITH(NOLOCK) ON T14.BoeAbs = T16.BoeKey
		LEFT  JOIN OBOT T17 WITH(NOLOCK) ON T17.AbsEntry = 
		(
			SELECT TOP 1 BOT1.AbsEntry FROM BOT1 WITH(NOLOCK)
			WHERE BOT1.BOENumber = T16.BoeNum AND BOT1.BoeType = T16.BoeType
			ORDER BY BOT1.AbsEntry DESC
		) AND T17.StatusTo = 'P'
		LEFT  JOIN OJDT T18 WITH(NOLOCK) ON T17.TransId = T18.Number
		LEFT  JOIN RCT1 T19 WITH(NOLOCK) ON T14.DocNum = T19.DocNum
		LEFT  JOIN OCHH T20 WITH(NOLOCK) ON T19.CheckAbs = T20.CheckKey
		LEFT  JOIN DPS1 T21 WITH(NOLOCK) ON T20.CheckKey = T21.CheckKey AND T21.DepCancel = 'N' AND T20.DpstAbs = T21.DepositId
		LEFT  JOIN ODPS T22 WITH(NOLOCK) ON T21.DepositId = T22.DeposId AND T22.DeposType = 'K'
		LEFT JOIN 
		(
			SELECT INV4.DocEntry, INV4.LineNum, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum FROM INV4 WITH(NOLOCK)
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('IPI', 'ICMS-ST')
			GROUP BY INV4.DocEntry, INV4.LineNum
		) IMPOSTO
			ON IMPOSTO.DocEntry = T0.DocEntry
			AND IMPOSTO.LineNum = T1.LineNum
	OUTER APPLY (
		SELECT TotItem = COUNT(INV1.LineNum), TotalItens = SUM(INV1.LineTotal) 
		FROM INV1 WITH(NOLOCK) WHERE INV1.DocEntry = T0.DocEntry AND INV1.TargetType <> N'14' AND ISNULL(INV1.FreeChrgBP, 'N') = 'N'
	) AS OA_MAX
	OUTER APPLY (
		SELECT DataPagamento = 
			CASE WHEN T17.StatusTo = 'P' THEN T18.RefDate
			ELSE CASE WHEN T20.CheckKey IS NOT NULL THEN T22.DeposDate
			ELSE CASE WHEN T14.BoeAbs IS NULL AND T19.CheckAbs IS NULL AND T14.DocEntry IS NOT NULL THEN T14.TaxDate
			ELSE NULL END END END
	) AS OA_DATA
	OUTER APPLY (
		SELECT LineTotal = 
			CASE WHEN T0.DpmAmnt <> 0 THEN 
				CASE WHEN OA_MAX.TotItem = 1 THEN (T1.VatSum - (T0.DpmAmnt - T1.LineTotal)) ELSE T1.LineTotal END
			ELSE T1.LineTotal END
	) AS OA_TOTAL
	WHERE T0.CANCELED = 'N'
	AND (OA_DATA.DataPagamento BETWEEN @DataInicial AND @DataFinal OR T0.DocDate BETWEEN @MetaDataInicial AND @MetaDataFinal)
	AND (T0.BPLId = @Filial OR ISNULL(@Filial, 0) = 0)
	AND (
		SELECT COUNT(OACT.AcctCode) FROM JDT1 WITH(NOLOCK) 
		INNER JOIN OACT WITH(NOLOCK) ON JDT1.Account = OACT.AcctCode AND OACT.Finanse = 'Y'
		WHERE JDT1.TransId = T14.TransId AND JDT1.TransType = T14.ObjType
	) <> 2

	INSERT INTO #tbl_docs_recebimento
	SELECT DISTINCT
		'R' AS Momento
		, T1.BPLId
		, T0.TransId AS DocEntry
		, T0.TaxDate AS DocDate
		, T0.TransId AS Serial
		, T1.Debit AS DocTotal
		, T0.ObjType AS ObjType
		, '' AS ItemCode
		, 0 AS LineNum
		, 0 AS Quantity
		, T1.Debit
		, 1 AS InstlmntID
		, T1.Debit AS InsTotal
		, T0.DueDate AS DueDate
		, '' AS ItemName
		, '' AS ItmsGrpCod
		, '' AS ItmsGrpNam
		, T3.CardCode AS CardCode
		, T3.CardName AS CardName
		, '' AS PrcCode
		, '' AS PrcName
		, T6.SlpCode AS SlpCode
		, T6.SlpName AS SlpName
		, ISNULL(T6.U_CVA_IMPADC, 'N') AS U_CVA_IMPADIC
		, ISNULL(T6.U_CVA_IMPINCL, 'N') AS U_CVA_IMPINCL
		, '' AS FirmCode
		, '' AS FirmName
		, '' AS IndCode
		, '' AS IndName
		, '' AS IndDesc
		, 0 AS AbsId
		, '' AS Name
		, '' AS [State]
		, OA_DATA.DataPagamento AS TaxDate
		, T14.SumApplied AS SumApplied
		, 1 AS Tot_Invoices
		, 1 AS Tot_Parcelas
		, 1 AS Tot_Itens
		, NULL AS DocStatus
		, T3.GroupCode AS GroupCode
		, 1 AS TotalItens
		, (T1.U_ST + T1.U_IPI)
		, T14.SumApplied - (T1.U_ST + T1.U_IPI)
	FROM OJDT T0 WITH(NOLOCK)
		INNER JOIN JDT1 T1 WITH(NOLOCK) ON T0.TransId = T1.TransId
		INNER JOIN OCRD T3 WITH(NOLOCK) ON T1.ShortName = T3.CardCode
		INNER JOIN OSLP T6 WITH(NOLOCK) ON ISNULL(T1.U_VENDEDOR, 0) = T6.SlpCode
		LEFT  JOIN OPRC T8 WITH(NOLOCK) ON T1.OcrCode2 = T8.PrcCode
		LEFT JOIN 
		(
			SELECT ORCT.BoeAbs, ORCT.TaxDate, ORCT.ObjType, ORCT.TransId, RCT2.SumApplied, RCT2.DocEntry, RCT2.DocNum, RCT2.InstId, RCT2.InvType FROM RCT2 WITH(NOLOCK)  
			INNER JOIN ORCT WITH(NOLOCK) ON ORCT.DocEntry = RCT2.DocNum AND ORCT.CANCELED <> 'Y'
			WHERE ORCT.TaxDate BETWEEN @DataInicial AND @DataFinal
		) T14 ON T1.TransId = T14.DocEntry AND T1.ObjType = T14.InvType

		LEFT  JOIN OBOE T16 WITH(NOLOCK) ON T14.BoeAbs = T16.BoeKey
		LEFT  JOIN OBOT T17 WITH(NOLOCK) ON T17.AbsEntry = 
		(
			SELECT TOP 1 BOT1.AbsEntry FROM BOT1 WITH(NOLOCK)
			WHERE BOT1.BOENumber = T16.BoeNum AND BOT1.BoeType = T16.BoeType
			ORDER BY BOT1.AbsEntry DESC
		) AND T17.StatusTo = 'P'
		LEFT  JOIN OJDT T18 WITH(NOLOCK) ON T17.TransId = T18.Number
		LEFT  JOIN RCT1 T19 WITH(NOLOCK) ON T14.DocNum = T19.DocNum
		LEFT  JOIN OCHH T20 WITH(NOLOCK) ON T19.CheckAbs = T20.CheckKey
		LEFT  JOIN DPS1 T21 WITH(NOLOCK) ON T20.CheckKey = T21.CheckKey AND T21.DepCancel = 'N' AND T20.DpstAbs = T21.DepositId
		LEFT  JOIN ODPS T22 WITH(NOLOCK) ON T21.DepositId = T22.DeposId AND T22.DeposType = 'K'
	OUTER APPLY (
		SELECT DataPagamento = 
			CASE WHEN T17.StatusTo = 'P' THEN T18.RefDate
			ELSE CASE WHEN T20.CheckKey IS NOT NULL THEN T22.DeposDate
			ELSE CASE WHEN T14.BoeAbs IS NULL AND T19.CheckAbs IS NULL AND T14.DocEntry IS NOT NULL THEN T14.TaxDate
			ELSE NULL END END END
	) AS OA_DATA
	WHERE  (OA_DATA.DataPagamento BETWEEN @DataInicial AND @DataFinal OR T0.TaxDate BETWEEN @MetaDataInicial AND @MetaDataFinal)
	AND (T1.BPLId = @Filial OR ISNULL(@Filial, 0) = 0)

	INSERT INTO #tbl_docs_recebimento
	SELECT DISTINCT
		'R' AS Momento
		, T0.BPLId
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
		, 0 AS Tot_Invoices
		, T0.Installmnt AS Tot_Parcelas
		, OA_MAX.TotItem AS Tot_Itens
		, NULL AS DocStatus
		, T3.GroupCode AS GroupCode
		, OA_MAX.TotalItens AS TotalItens
		, ISNULL(IMPOSTO.TaxSum, 0) * (T2.InstPrcnt/100)
		, T14.SumApplied - (ISNULL(IMPOSTO.TaxSum, 0) / T0.Installmnt)
	FROM ODPI T0 WITH(NOLOCK) 
		INNER JOIN DPI1 T1 WITH(NOLOCK) ON T0.DocEntry = T1.DocEntry AND ISNULL(T1.FreeChrgBP, 'N') = 'N'
		INNER JOIN DPI6 T2 WITH(NOLOCK) ON T0.DocEntry = T2.DocEntry AND T2.InsTotal <> 0
		INNER JOIN OCRD T3 WITH(NOLOCK) ON T0.CardCode = T3.CardCode	
		LEFT JOIN 
		(
			SELECT ORCT.BoeAbs, ORCT.TaxDate, ORCT.ObjType, ORCT.TransId, RCT2.SumApplied, RCT2.DocEntry, RCT2.DocNum, RCT2.InstId, RCT2.InvType FROM RCT2 WITH(NOLOCK)  
			INNER JOIN ORCT WITH(NOLOCK) ON ORCT.DocEntry = RCT2.DocNum AND ORCT.CANCELED <> 'Y'
			WHERE ORCT.TaxDate BETWEEN @DataInicial AND @DataFinal
		) T14 ON T2.DocEntry = T14.DocEntry AND T2.InstlmntID = T14.InstId AND T2.ObjType = T14.InvType
		LEFT  JOIN OBOE T16 WITH(NOLOCK) ON T14.BoeAbs = T16.BoeKey
		LEFT  JOIN OBOT T17 WITH(NOLOCK) ON T17.AbsEntry = 
		(
			SELECT TOP 1 BOT1.AbsEntry FROM BOT1 WITH(NOLOCK) 
			WHERE BOT1.BOENumber = T16.BoeNum AND BOT1.BoeType = T16.BoeType
			ORDER BY BOT1.AbsEntry DESC
		) AND T17.StatusTo = 'P'
		LEFT  JOIN OJDT T18 WITH(NOLOCK) ON T17.TransId = T18.Number
		LEFT  JOIN RCT1 T19 WITH(NOLOCK) ON T14.DocNum = T19.DocNum
		LEFT  JOIN OCHH T20 WITH(NOLOCK) ON T19.CheckAbs = T20.CheckKey
		LEFT  JOIN DPS1 T21 WITH(NOLOCK) ON T20.CheckKey = T21.CheckKey AND T21.DepCancel = 'N' AND T20.DpstAbs = T21.DepositId
		LEFT  JOIN ODPS T22 WITH(NOLOCK) ON T21.DepositId = T22.DeposId AND T22.DeposType = 'K'
		INNER JOIN INV9 T23 WITH(NOLOCK) ON T23.BaseAbs = T0.DocEntry AND T23.ObjType = T0.ObjType
		INNER JOIN OINV T24 WITH(NOLOCK) ON T24.DocEntry = T23.DocEntry
		INNER JOIN INV1 T25 WITH(NOLOCK) ON T24.DocEntry = T25.DocEntry
		INNER JOIN OITM T4 WITH(NOLOCK) ON T25.ItemCode = T4.ItemCode
		INNER JOIN OITB T5 WITH(NOLOCK) ON T4.ItmsGrpCod = T5.ItmsGrpCod
		LEFT  JOIN OSLP T6 WITH(NOLOCK) ON T24.SlpCode = T6.SlpCode
	--	LEFT  JOIN OSLP T7 WITH(NOLOCK) ON T3.SlpCode = T7.SlpCode
		LEFT  JOIN OPRC T8 WITH(NOLOCK) ON T25.OcrCode2 = T8.PrcCode
		LEFT  JOIN OPRC T9 WITH(NOLOCK) ON T25.OcrCode = T9.PrcCode
		LEFT  JOIN OMRC T10 WITH(NOLOCK) ON T4.FirmCode = T10.FirmCode
		LEFT  JOIN OOND T11 WITH(NOLOCK) ON T3.IndustryC = T11.IndCode
		LEFT  JOIN INV12 T12 WITH(NOLOCK) ON T24.DocEntry = T12.DocEntry
		LEFT  JOIN OCNT T13 WITH(NOLOCK) ON T12.CountyB = T13.AbsId
		LEFT JOIN 
		(
			SELECT INV4.DocEntry, INV4.LineNum, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum FROM INV4 WITH(NOLOCK)
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('IPI', 'ICMS-ST')
			GROUP BY INV4.DocEntry, INV4.LineNum
		) IMPOSTO
			ON IMPOSTO.DocEntry = T25.DocEntry
			AND IMPOSTO.LineNum = T25.LineNum
	OUTER APPLY (
		SELECT TotItem = COUNT(INV1.LineNum), TotalItens = SUM(INV1.LineTotal) 
		FROM INV1 WITH(NOLOCK) WHERE INV1.DocEntry = T24.DocEntry
	) AS OA_MAX
	OUTER APPLY (
		SELECT DataPagamento = 
			CASE WHEN T17.StatusTo = 'P' THEN T18.RefDate
			ELSE CASE WHEN T20.CheckKey IS NOT NULL THEN T22.DeposDate
			ELSE CASE WHEN T14.BoeAbs IS NULL AND T19.CheckAbs IS NULL AND T14.DocEntry IS NOT NULL THEN T14.TaxDate
			ELSE NULL END END END
	) AS OA_DATA
	WHERE T0.CANCELED = 'N' AND OA_DATA.DataPagamento IS NOT NULL 
	AND (OA_DATA.DataPagamento BETWEEN @DataInicial AND @DataFinal OR T0.DocDate BETWEEN @MetaDataInicial AND @MetaDataFinal)
	AND (T0.BPLId = @Filial OR ISNULL(@Filial, 0) = 0)
	AND (
		SELECT COUNT(OACT.AcctCode) FROM JDT1 WITH(NOLOCK) 
		INNER JOIN OACT WITH(NOLOCK) ON JDT1.Account = OACT.AcctCode AND OACT.Finanse = 'Y'
		WHERE JDT1.TransId = T14.TransId AND JDT1.TransType = T14.ObjType
	) <> 2

	INSERT INTO #tbl_docs_recebimento
	SELECT DISTINCT
		'R' AS Momento
		, T0.BPLId
		, T0.DocEntry AS DocEntry
		, T0.DocDate AS DocDate
		, ISNULL(T0.Serial, T0.DocEntry) AS Serial
		, T0.DocTotal * (-1) AS DocTotal
		, T0.ObjType AS ObjType
		, T1.ItemCode AS ItemCode
		, T1.LineNum AS LineNum
		, T1.Quantity AS Quantity
		, T1.LineTotal * (T2.InstPrcnt/100) * (-1) AS LineTotal
		, T2.InstlmntID AS InstlmntID
		, T2.InsTotal * (-1) AS InsTotal 
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
		, T0.DocDate AS TaxDate
		, T0.DocTotal AS SumApplied
		, 1 AS Tot_Invoices
		, T0.Installmnt AS Tot_Parcelas
		, OA_MAX.TotItem AS Tot_Itens
		, NULL AS DocStatus
		, T3.GroupCode AS GroupCode
		, OA_MAX.TotalItens AS TotalItens
		, ISNULL(IMPOSTO.TaxSum, 0) * (T2.InstPrcnt/100) * (-1)
		, (T0.DocTotal - (ISNULL(IMPOSTO.TaxSum, 0) / T0.Installmnt)) * (-1)
	FROM ORIN T0 WITH(NOLOCK) 
		INNER JOIN RIN1 T1 WITH(NOLOCK) ON T0.DocEntry = T1.DocEntry AND ISNULL(T1.FreeChrgBP, 'N') = 'N'
		INNER JOIN RIN6 T2 WITH(NOLOCK) ON T0.DocEntry = T2.DocEntry AND T2.InsTotal <> 0
		INNER JOIN OCRD T3 WITH(NOLOCK) ON T0.CardCode = T3.CardCode
		INNER JOIN OITM T4 WITH(NOLOCK) ON T1.ItemCode = T4.ItemCode
		INNER JOIN OITB T5 WITH(NOLOCK) ON T4.ItmsGrpCod = T5.ItmsGrpCod
		INNER JOIN OSLP T6 WITH(NOLOCK) ON T0.SlpCode = T6.SlpCode
		LEFT  JOIN OPRC T8 WITH(NOLOCK) ON T1.OcrCode2 = T8.PrcCode
		LEFT  JOIN OPRC T9 WITH(NOLOCK) ON T1.OcrCode = T9.PrcCode
		LEFT  JOIN OMRC T10 WITH(NOLOCK) ON T4.FirmCode = T10.FirmCode
		LEFT  JOIN OOND T11 WITH(NOLOCK) ON T3.IndustryC = T11.IndCode
		LEFT  JOIN RIN12 T12 WITH(NOLOCK) ON T0.DocEntry = T12.DocEntry
		LEFT  JOIN OCNT T13 WITH(NOLOCK) ON T12.CountyB = T13.AbsId
		LEFT JOIN ITR1 T16 WITH(NOLOCK) ON T16.SrcObjAbs = T0.DocEntry AND T16.SrcObjTyp = T0.ObjType AND IsCredit = 'C'
		LEFT JOIN OITR T17 WITH(NOLOCK) ON T17.ReconNum = T16.ReconNum AND T17.ReconType = 0 -- Manual
		LEFT JOIN 
		(
			SELECT RIN4.DocEntry, RIN4.LineNum, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum FROM RIN4 WITH(NOLOCK)
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('IPI', 'ICMS-ST')
			GROUP BY RIN4.DocEntry, RIN4.LineNum
		) IMPOSTO
			ON IMPOSTO.DocEntry = T0.DocEntry
			AND IMPOSTO.LineNum = T1.LineNum
	OUTER APPLY (
		SELECT TotItem = COUNT(RIN1.LineNum), TotalItens = SUM(RIN1.LineTotal) 
		FROM RIN1 WITH(NOLOCK) WHERE RIN1.DocEntry = T0.DocEntry AND RIN1.TargetType <> N'14' AND ISNULL(RIN1.FreeChrgBP, 'N') = 'N'
	) AS OA_MAX
	WHERE T0.CANCELED = 'N'
	--AND (ISNULL(T17.ReconDate, T0.DocDate) BETWEEN @DataInicial AND @DataFinal OR T0.DocDate BETWEEN @MetaDataInicial AND @MetaDataFinal)
	AND (T0.DocDate BETWEEN @DataInicial AND @DataFinal OR T0.DocDate BETWEEN @MetaDataInicial AND @MetaDataFinal)
	AND (T0.BPLId = @Filial OR ISNULL(@Filial, 0) = 0)

	SELECT
		Momento,
		BPLId,
		DocEntry,
		DocDate,
		Serial,
		DocTotal,
		ObjType,
		ItemCode,
		LineNum,
		Quantity,
		LineTotal,
		InstlmntID,
		InsTotal,
		DueDate,
		ItemName,
		ItmsGrpCod,
		ItmsGrpNam,
		CardCode,
		CardName,
		PrcCode,
		PrcName,
		SlpCode,
		SlpName,
		U_CVA_IMPADIC,
		U_CVA_IMPINCL,
		FirmCode,
		FirmName,
		IndCode,
		IndName,
		IndDesc,
		AbsId,
		Name,
		[State],
		MAX(TaxDate),
		SUM(SumApplied),
		Tot_Invoices,
		Tot_Parcelas,
		Tot_Itens,
		DocStatus,
		GroupCode,
		TotalItens,
		TaxSum,
		SUM(BaseSum)
	FROM #tbl_docs_recebimento
	GROUP BY
		Momento,
		BPLId,
		DocEntry,
		DocDate,
		Serial,
		DocTotal,
		ObjType,
		ItemCode,
		LineNum,
		Quantity,
		LineTotal,
		InstlmntID,
		InsTotal,
		DueDate,
		ItemName,
		ItmsGrpCod,
		ItmsGrpNam,
		CardCode,
		CardName,
		PrcCode,
		PrcName,
		SlpCode,
		SlpName,
		U_CVA_IMPADIC,
		U_CVA_IMPINCL,
		FirmCode,
		FirmName,
		IndCode,
		IndName,
		IndDesc,
		AbsId,
		Name,
		[State],
		Tot_Invoices,
		Tot_Parcelas,
		Tot_Itens,
		DocStatus,
		GroupCode,
		TotalItens,
		TaxSum

	DROP TABLE #tbl_docs_recebimento
end
