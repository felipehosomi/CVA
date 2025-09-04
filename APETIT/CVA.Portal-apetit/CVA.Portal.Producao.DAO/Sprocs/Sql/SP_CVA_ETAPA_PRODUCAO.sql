ALTER PROCEDURE SP_CVA_ETAPA_PRODUCAO
(
	@NrOP		INT,
	@CodEtapa	INT
)
AS
BEGIN
	DECLARE @QtdeOP NUMERIC(19, 6)

	-- Se houve apontamento parcial da etapa anterior, busca a quantidade apontada
	SELECT @QtdeOP = WOR_PARCIAL.IssuedQty / WOR_PARCIAL.BaseQty 
	FROM WOR1
		INNER JOIN WOR4
			ON WOR4.DocEntry = WOR1.DocEntry
			AND WOR4.StageId < WOR1.StageId
		INNER JOIN WOR1 WOR_PARCIAL
			ON WOR_PARCIAL.DocEntry = WOR4.DocEntry
			AND WOR_PARCIAL.StageId = WOR4.StageId
			AND WOR_PARCIAL.[Status] = 'P'
	WHERE WOR1.DocEntry = @NrOP
	AND WOR1.StageId = @CodEtapa

	SELECT 
		OWOR.DocEntry,
		OWOR.DocNum,
		OWOR.PlannedQty QtdeOriginal,
		CASE WHEN ISNULL(@QtdeOP, 0) > 0
			THEN @QtdeOP - WOR1.IssuedQty
			ELSE OWOR.PlannedQty - (WOR1.IssuedQty / WOR1.BaseQty )
		END QtdeOP,
		OWOR.StartDate DocDate,
		CONVERT(char(10), OWOR.StartDate, 126) DocDateStr,
		OWOR.OriginAbs DocEntryPedido,
		OWOR.OriginNum NrPedido,
		WOR1.LineNum,
		WOR1.wareHouse	Deposito,
		WOR1.IssueType MetodoBaixa,
		OITM.ItemCode,
		OITM.ItemName,
		WOR4.StageId 	CodEtapa,
		WOR4.Name	 	DescEtapa,
		WOR1.ItemCode	CodMP,
		CASE WHEN WOR1.ItemType = 4
			THEN MP.ItemName
			ELSE ORSC.ResName
		END DescMP,
		MP.ManBtchNum 	ControlePorLote,
		MP.ManSerNum  	ControlePorSerie,
		CASE WHEN ISNULL(@QtdeOP, 0) > 0
			THEN @QtdeOP * WOR1.BaseQty
			ELSE WOR1.PlannedQty - WOR1.IssuedQty	
		END Quantidade,
		WOR1.BaseQty	QtdeBase,
		WOR1.IssuedQty	QtdeEmitida,
		CASE WHEN ISNULL(@QtdeOP, 0) > 0
			THEN (@QtdeOP * WOR1.BaseQty) - WOR1.IssuedQty
			ELSE WOR1.PlannedQty - WOR1.IssuedQty	
		END QtdeRealizada
	FROM OWOR
		INNER JOIN OITM
			ON OITM.ItemCode = OWOR.ItemCode
		INNER JOIN WOR4 
			ON WOR4.DocEntry = OWOR.DocEntry
		INNER JOIN WOR1
			ON WOR1.DocEntry = WOR4.DocEntry
			AND WOR1.StageId = WOR4.StageId
		LEFT JOIN OITM MP
			ON MP.ItemCode = WOR1.ItemCode
			AND WOR1.ItemType = 4
		LEFT JOIN ORSC
			ON ORSC.ResCode = WOR1.ItemCode
			AND WOR1.ItemType = 290
	
	WHERE OWOR.DocNum = @NrOP 
	AND WOR1.StageId = @CodEtapa

END