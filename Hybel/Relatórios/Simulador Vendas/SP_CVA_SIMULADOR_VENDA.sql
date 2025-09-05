IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_SIMULADOR_VENDA')
	DROP PROCEDURE SP_CVA_SIMULADOR_VENDA
GO

CREATE PROCEDURE SP_CVA_SIMULADOR_VENDA
(
	@Code		NVARCHAR(60)
)
AS
BEGIN
	;WITH Components AS
	(
		SELECT
			1	Nivel,
			STL.ItemCode Pai,
			OITM.ItemCode,
			OITM.ItemName,
			OITM.SalUnitMsr UN,
			STL.MENGE_VERBRAUCH QtdeConsumida,
			OITW.OnHand QtdeDisponivel,
			OITW.MinStock EstoqueMin,
			OITW.WhsCode
		FROM BEAS_STL STL WITH(NOLOCK)
			INNER JOIN OITM WITH(NOLOCK)
				ON OITM.ItemCode = STL.ART1_ID
			INNER JOIN OITW WITH(NOLOCK)
				ON OITW.ItemCode = OITM.ItemCode
				--AND OITW.WhsCode = OITM.DfltWH
			INNER JOIN [@CVA_SIM_VENDA_ITEM] SIM WITH(NOLOCK)
				ON SIM.U_ItemCode = STL.ItemCode
		WHERE SIM.Code = @Code

		UNION ALL

		SELECT
			Components.Nivel + 1 Nivel,
			STL.ItemCode	Pai,
			OITM.ItemCode,
			OITM.ItemName,
			OITM.SalUnitMsr UN,
			CAST(Components.QtdeConsumida * STL.MENGE_VERBRAUCH AS NUMERIC(19, 6)) QtdeConsumida,
			OITW.OnHand QtdeDisponivel,
			OITW.MinStock EstoqueMin,
			OITW.WhsCode
		FROM BEAS_STL STL WITH(NOLOCK)
			INNER JOIN OITM WITH(NOLOCK)
				ON OITM.ItemCode = STL.ART1_ID
			INNER JOIN OITW WITH(NOLOCK)
				ON OITW.ItemCode = OITM.ItemCode
				--AND OITW.WhsCode = OITM.DfltWH
			INNER JOIN Components
				ON STL.ItemCode = Components.ItemCode	
				AND STL.WhsCode = Components.WhsCode
	)
	, ComponentsSum AS
	(
		SELECT 
			Components.ItemCode,
			Components.ItemName,
			Components.UN,
			SUM(Components.QtdeDisponivel) QtdeDisponivel,
			Components.QtdeConsumida,
			SUM(Components.EstoqueMin) EstoqueMin
		FROM Components
		GROUP BY 
			Components.ItemCode, Components.ItemName, Components.UN, Components.QtdeConsumida
	)
	, OP AS
	(
		SELECT 
			Components.ItemCode,
			Components.ItemName,
			Components.UN,
			Components.QtdeDisponivel,
			SUM(Components.QtdeConsumida) QtdeConsumida,
			Components.EstoqueMin,
			ISNULL(PROD.QtdeProd, 0.00)	QtdeProd
		FROM ComponentsSum Components
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
			Components.ItemCode, Components.ItemName, Components.UN, Components.QtdeDisponivel, Components.EstoqueMin, PROD.QtdeProd
	)
	,Produtos AS
	(
		SELECT 
			OP.ItemCode,
			OP.ItemName,
			OP.UN,
			OP.QtdeDisponivel,
			OP.QtdeConsumida,
			OP.EstoqueMin,
			OP.QtdeProd,
			STL.ItemCode ParentId, ART1_ID ChildId, MENGE_LAGER AS QtdeConsPV, POS_ID AS Lvl 
		FROM BEAS_STL STL WITH(NOLOCK)
			INNER JOIN OP
				ON OP.ItemCode = STL.ART1_ID

		UNION ALL

		SELECT 
			rc.ItemCode,
			rc.ItemName,
			rc.UN,
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
	,CTE_RN_GROUPED AS 
	(
		SELECT 
			ParentId,
			ItemCode, 
			ItemName,
			UN,
			QtdeDisponivel,
			QtdeConsumida,
			EstoqueMin,
			SUM(QtdeProd) QtdeProd,
			QtdeConsPV
		FROM CTE_RN
		WHERE Lvl = 10
		GROUP BY ParentId, ItemCode, ItemName, UN, QtdeDisponivel, QtdeConsumida, EstoqueMin, QtdeConsPV
	)
	SELECT 
		--ParentId,
		CTE.ItemCode	[Código], 
		CTE.ItemName	[Produto],
		CTE.UN,
		QtdeConsumida	[Qtde],
		QtdeDisponivel	[Est Fís],
		QtdeProd		[Est Enc],
		SUM(QtdeConsPV * RDR1.OpenQty) [Est Res],
		QtdeDisponivel + QtdeProd - SUM(QtdeConsPV * RDR1.OpenQty) [Est Dis]
	FROM CTE_RN_GROUPED CTE
		LEFT JOIN	(
						SELECT RDR1.ItemCode, RDR1.OpenQty FROM RDR1 WITH(NOLOCK)
							INNER JOIN ORDR WITH(NOLOCK)
								ON ORDR.DocEntry = RDR1.DocEntry
							WHERE ORDR.DocStatus = 'O'
					) RDR1
			ON RDR1.ItemCode = CTE.ParentId
	GROUP BY CTE.ItemCode, CTE.ItemName, CTE.UN, QtdeDisponivel, QtdeConsumida, EstoqueMin, QtdeProd
	HAVING QtdeDisponivel + QtdeProd - SUM(QtdeConsPV * RDR1.OpenQty) < 0
	ORDER BY ItemName
END