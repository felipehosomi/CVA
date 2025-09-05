DECLARE @ItemCode NVARCHAR(60) = '30111048013'
DECLARE @Quantidade NUMERIC(19, 6) = 100

;WITH Components as
(
	--SELECT
	--	0	Nivel,
	--	'' Pai,
	--	OITM.ItemCode,
	--	OITM.ItemName,
	--	0 QtdeConsumida,
	--	OITW.OnHand QtdeDisponivel,
	--	OITW.MinStock EstoqueMin
	--FROM OITM WITH(NOLOCK)
	--	INNER JOIN OITW WITH(NOLOCK)
	--		ON OITW.ItemCode = OITM.ItemCode
	--		AND OITW.WhsCode = OITM.DfltWH
	--WHERE OITM.ItemCode = @ItemCode

	--UNION ALL

	SELECT
		1	Nivel,
		STL.ItemCode Pai,
		OITM.ItemCode,
		OITM.ItemName,
		STL.MENGE_VERBRAUCH QtdeConsumida,
		OITW.OnHand QtdeDisponivel,
		OITW.MinStock EstoqueMin
	FROM BEAS_STL STL WITH(NOLOCK)
		INNER JOIN OITM WITH(NOLOCK)
			ON OITM.ItemCode = STL.ART1_ID
		INNER JOIN OITW WITH(NOLOCK)
			ON OITW.ItemCode = OITM.ItemCode
			AND OITW.WhsCode = OITM.DfltWH
	WHERE STL.ItemCode = @ItemCode

	UNION ALL

	SELECT
		Components.Nivel + 1 Nivel,
		STL.ItemCode	Pai,
		OITM.ItemCode,
		OITM.ItemName,
		STL.MENGE_VERBRAUCH QtdeConsumida,
		OITW.OnHand QtdeDisponivel,
		OITW.MinStock EstoqueMin
	FROM BEAS_STL STL WITH(NOLOCK)
		INNER JOIN OITM WITH(NOLOCK)
			ON OITM.ItemCode = STL.ART1_ID
		INNER JOIN OITW WITH(NOLOCK)
			ON OITW.ItemCode = OITM.ItemCode
			AND OITW.WhsCode = OITM.DfltWH
		INNER JOIN Components
			ON STL.ItemCode = Components.ItemCode	
)
, Resultado AS
(
	SELECT 
		Components.ItemCode,
		Components.ItemName,
		Components.QtdeDisponivel,
		SUM(Components.QtdeConsumida) QtdeConsumida,
		Components.EstoqueMin,
		ISNULL(PROD.QtdeProd, 0.00)	QtdeProd,
		SUM(Components.QtdeConsumida) * ISNULL(SUM(PV.QtdePV), 0) QtdePV
		--Components.QtdeDisponivel + ISNULL(PROD.QtdeProd, 0) - Components.QtdeConsumida * ISNULL(PV.QtdePV, 0) QtdeDisp
	FROM Components
		LEFT JOIN	(
						SELECT 
							AUPT.ItemCode,
							SUM(POS.MENGE_VERBRAUCH) QtdeProd
						FROM BEAS_FTHAUPT AUPT WITH(NOLOCK)
							LEFT JOIN BEAS_FTPOS POS WITH(NOLOCK)
								ON POS.BELNR_ID = AUPT.BELNR_ID
								AND POS.ItemCode = AUPT.ItemCode 
								AND POS.STUFE = 0
						WHERE AUPT.ABGKZ = 'N'
						GROUP BY AUPT.ItemCode 
					) PROD ON PROD.ItemCode = Components.ItemCode
		LEFT JOIN	(
						SELECT 
							RDR1.ItemCode,
							SUM(OpenQty) QtdePV
						FROM ORDR WITH(NOLOCK)
							INNER JOIN RDR1 WITH(NOLOCK)
								ON RDR1.DocEntry = ORDR.DocEntry
						WHERE ORDR.DocStatus = 'O'
						GROUP BY RDR1.ItemCode 
					) PV ON PV.ItemCode = Components.Pai
	WHERE Components.QtdeDisponivel + ISNULL(PROD.QtdeProd, 0) - (Components.QtdeConsumida * ISNULL(PV.QtdePV, 0)) - @Quantidade  < 0
	GROUP BY 
		Components.ItemCode, Components.ItemName, Components.QtdeDisponivel, Components.EstoqueMin, PROD.QtdeProd
)

SELECT 
	ItemCode	Produto, 
	ItemName	Descrição,
	QtdeConsumida	Qtde,
	QtdeDisponivel		[Est Fís],
	QtdeProd			[Est Enc],
	QtdePV				[Est Res],
	QtdeDisponivel + QtdeProd - QtdePV	[Est Dis],
	EstoqueMin			[Est Min]
FROM Resultado