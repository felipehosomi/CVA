IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_FRETE')
	DROP PROCEDURE SP_CVA_FRETE
GO
CREATE PROCEDURE SP_CVA_FRETE
(
	@DateFrom DATETIME = NULL,
	@DateTo DATETIME = NULL,
	@DocNumFrom INT = NULL,
	@DocNumTo INT = NULL,
	@CardCode NVARCHAR(60) = '*',
	@State NVARCHAR(2) = '*',
	@City NVARCHAR(MAX) = '*',
	@Carrier NVARCHAR(60) = '*'
)
AS
BEGIN
;WITH Frete AS
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
		INV12.Carrier,
		OINV.DocTotal,
		INV12.GrsWeight,
		SUM(INV1.U_CVA_Frete_Estimado)	FreteEstimado,
		CASE WHEN ISNULL(INV12.GrsWeight, 0) > 0 AND  ISNULL(SUM(CTE_PESO.GrsWeight), 0) > 0
			THEN SUM(CTE.DocTotal) * (INV12.GrsWeight / SUM(CTE_PESO.GrsWeight))	
			ELSE SUM(CTE.DocTotal)
		END FreteReal
	FROM OINV WITH(NOLOCK)
		INNER JOIN INV1 WITH(NOLOCK)
			ON INV1.DocEntry = OINV.DocEntry
		INNER JOIN INV12 WITH(NOLOCK)
			ON INV12.DocEntry = OINV.DocEntry
		INNER JOIN [@CVA_NFE_FRETE_ITEM] FRETE_ITEM WITH(NOLOCK)
			ON FRETE_ITEM.U_DocNum = OINV.DocNum
		INNER JOIN [@CVA_NFE_FRETE] FRETE WITH(NOLOCK)
			ON FRETE.Code = FRETE_ITEM.Code
		INNER JOIN [@CVA_NFE_FRETE_ITEM] FRETE_ITEM_TODOS WITH(NOLOCK)
			ON FRETE_ITEM_TODOS.Code = FRETE.Code
		INNER JOIN OINV CTE WITH(NOLOCK)
			ON CTE.DocNum = FRETE_ITEM_TODOS.U_DocNum
		INNER JOIN INV12 CTE_PESO WITH(NOLOCK)
			ON CTE_PESO.DocEntry = CTE.DocEntry
		INNER JOIN OPCH WITH(NOLOCK)
			ON OPCH.DocEntry = FRETE.U_DocEntry
		LEFT JOIN OSLP WITH(NOLOCK)
			ON OSLP.SlpCode = OINV.SlpCode
	--WHERE OINV.DocDate BETWEEN @DateFrom AND @DateTo
	--	AND OINV.DocNum BETWEEN @DocNumFrom AND @DocNumTo
	--	AND (@CardCode = '*' OR @CardCode = OINV.CardCode)
	--	AND (@State = '*' OR @City = INV12.StateS)
	--	AND (@City = '*' OR @City = INV12.CityS)
	--	AND (@Carrier = '*' OR @Carrier = INV12.Carrier)
	GROUP BY
		OINV.DocDate,
		OINV.DocNum,
		OINV.Serial,
		OINV.CardCode,
		OINV.CardName,
		INV12.StateS,
		INV12.CityS,
		OSLP.SlpName,
		INV12.Carrier,
		OINV.DocTotal,
		INV12.GrsWeight
)
SELECT 
	*,
	((FreteReal - FreteEstimado) / FreteReal) * 100 Desvio
FROM FRETE
END