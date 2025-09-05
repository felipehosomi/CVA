DECLARE @ItemCode NVARCHAR(60) = '30111048013'
DECLARE @Quantidade NUMERIC(19, 6) = 100

;WITH Components as
(
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
		CAST(Components.QtdeConsumida * STL.MENGE_VERBRAUCH AS NUMERIC(19, 6)) QtdeConsumida,
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
, OP AS
(
	SELECT 
		Components.ItemCode,
		Components.ItemName,
		Components.QtdeDisponivel,
		SUM(Components.QtdeConsumida) QtdeConsumida,
		Components.EstoqueMin,
		ISNULL(PROD.QtdeProd, 0.00)	QtdeProd
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
	GROUP BY 
		Components.ItemCode, Components.ItemName, Components.QtdeDisponivel, Components.EstoqueMin, PROD.QtdeProd
)
,Produtos AS
(
	SELECT 
		OP.ItemCode,
		OP.ItemName,
		OP.QtdeDisponivel,
		OP.QtdeConsumida,
		OP.EstoqueMin,
		OP.QtdeProd,
		STL.ItemCode ParentId, ART1_ID ChildId, MENGE_LAGER AS QtdeConsPV, POS_ID AS Lvl 
	FROM BEAS_STL STL WITH(NOLOCK)
		INNER JOIN OP
			ON OP.ItemCode = STL.ART1_ID
	--WHERE ART1_ID = '99611035057'

	UNION ALL

	SELECT 
		rc.ItemCode,
		rc.ItemName,
		rc.QtdeDisponivel,
		rc.QtdeConsumida,
		rc.EstoqueMin,
		rc.QtdeProd, 
		rh.ItemCode ParentId, rc.ChildId, CAST(RC.QtdeConsPV * RH.MENGE_LAGER AS NUMERIC(19, 6)) QtdeConsPV, RC.Lvl AS Lvl 
	FROM dbo.BEAS_STL rh WITH(NOLOCK)
	INNER JOIN Produtos rc ON rh.ART1_ID = rc.ParentId
)
,CTE_RN AS 
(
	SELECT *, ROW_NUMBER() OVER (PARTITION BY r.ChildID ORDER BY r.Lvl DESC) RN
	FROM Produtos r
)
SELECT ParentId, ChildId, SUM(QtdeConsPV) QtdeCons
FROM CTE_RN r
WHERE Lvl = 10
AND ParentId = '30111048013'
GROUP BY ParentId, ChildId

--SELECT 
--	ItemCode	Produto, 
--	ItemName	Descrição,
--	QtdeConsumida	Qtde,
--	QtdeDisponivel		[Est Fís],
--	QtdeProd			[Est Enc],
--	QtdePV				[Est Res],
--	QtdeDisponivel + QtdeProd - QtdePV	[Est Dis],
--	EstoqueMin			[Est Min]
--FROM Produtos