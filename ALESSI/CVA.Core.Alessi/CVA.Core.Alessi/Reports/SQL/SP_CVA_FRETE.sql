--EXEC SP_CVA_FRETE '2010-01-01', '2020-12-31', 'T', 'T'
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_FRETE')
	DROP PROCEDURE SP_CVA_FRETE
GO
CREATE PROCEDURE SP_CVA_FRETE
(
	@DateFrom		DATETIME,
	@DateTo			DATETIME,
	@FreteEstimado	VARCHAR(1),
	@FreteReal		VARCHAR(1)
)
AS
BEGIN
	SET DATEFORMAT 'ymd';

	DECLARE @dtFrom DATE, @dtTo DATE
	SELECT @dtFrom = CAST(@DateFrom AS DATE)
	SELECT @dtTo = CAST(@DateTo AS DATE)

	;WITH Nota AS
	(
		SELECT
			OINV.DocDate,
			OINV.DocNum,
			OINV.Serial,
			OINV.CardCode,
			OINV.CardName,
			INV12.StateS,
			INV12.CityS,
			OSLP.SlpName,
			OINV.DocTotal,
			ISNULL(NULLIF(INV12.NetWeight, 0), 1)				[Weight], 
			ISNULL(NULLIF(SUM(INV1.U_akbfreight), 0), 0) 		FreteEstimado,
			FRETE.Code					CodeFrete,
			FRETE.U_DocEntry			CTeDocEntry
		FROM OINV WITH(NOLOCK)
			INNER JOIN INV1 WITH(NOLOCK)
				ON INV1.DocEntry = OINV.DocEntry
			INNER JOIN INV12 WITH(NOLOCK)
				ON INV12.DocEntry = OINV.DocEntry
			LEFT JOIN [@CVA_NFE_FRETE_ITEM] FRETE_ITEM WITH(NOLOCK)
				ON FRETE_ITEM.U_DocNum = OINV.DocNum
			LEFT JOIN [@CVA_NFE_FRETE] FRETE WITH(NOLOCK)
				ON FRETE.Code = FRETE_ITEM.Code
			LEFT JOIN OSLP WITH(NOLOCK)
				ON OSLP.SlpCode = OINV.SlpCode
		WHERE CAST(OINV.DocDate AS DATE) >= @dtFrom AND CAST(OINV.DocDate AS DATE) <= @dtTo
		GROUP BY
			OINV.DocDate,
			OINV.DocNum,
			OINV.Serial,
			OINV.CardCode,
			OINV.CardName,
			INV12.StateS,
			INV12.CityS,
			OSLP.SlpName,
			OINV.DocTotal,
			INV12.NetWeight,
			FRETE.Code,
			FRETE.U_DocEntry
	)
	, Frete AS
	(
		SELECT
			Nota.DocDate,
			Nota.DocNum,
			Nota.Serial ,
			Nota.CardCode,
			Nota.CardName,
			Nota.StateS,
			Nota.CityS,
			Nota.SlpName,
			Nota.DocTotal,
			Nota.[Weight]	GrsWeight,
			Nota.FreteEstimado,
			OPCH.serial nrcte,
			OPCH.CardCode Carrier,
			OPCH.CardName CarrierName,
			ISNULL(OPCH.DocTotal, 0) * Nota.[Weight] / ISNULL(NULLIF(SUM(INV12.NetWeight), 0), 1) FreteReal
		FROM Nota
			LEFT JOIN OPCH WITH(NOLOCK)
				ON OPCH.DocEntry = Nota.CTeDocEntry
			LEFT JOIN [@CVA_NFE_FRETE_ITEM] FRETE_ITEM_TODOS WITH(NOLOCK)
				ON FRETE_ITEM_TODOS.Code = Nota.CodeFrete
				AND ISNULL(U_DocNum, 0) <> 0
			LEFT JOIN OINV WITH(NOLOCK)
				ON OINV.DocNum = FRETE_ITEM_TODOS.U_DocNum
			LEFT JOIN INV12 WITH(NOLOCK)
				ON INV12.DocEntry = OINV.DocEntry
		GROUP BY
			Nota.DocDate,
			Nota.DocNum,
			Nota.Serial,
			Nota.CardCode,
			Nota.CardName,
			Nota.StateS,
			Nota.CityS,
			Nota.SlpName,
			Nota.DocTotal,
			Nota.[Weight],
			Nota.FreteEstimado,
			OPCH.serial,
			OPCH.CardCode,
			OPCH.CardName,
			OPCH.DocTotal
	)
	SELECT 
		*,
		((FreteReal - FreteEstimado) / ISNULL(NULLIF(FreteReal, 0), 1)) * 100 Desvio
		INTO #FRETE
	FROM FRETE

	DECLARE @SQL VARCHAR(MAX)

	SET @SQL = 'SELECT * FROM #FRETE WHERE 1 = 1 '
	IF @FreteEstimado = 'N'
	BEGIN
		SET @SQL += ' AND FreteEstimado = 0'
	END
	IF @FreteEstimado = 'I'
	BEGIN
		SET @SQL += ' AND FreteEstimado > 0'
	END
	
	IF @FreteReal = 'N'
	BEGIN
		SET @SQL += ' AND FreteReal = 0'
	END
	IF @FreteReal = 'I'
	BEGIN
		SET @SQL += ' AND FreteReal > 0'
	END

	EXEC (@SQL)

	DROP TABLE #FRETE
END