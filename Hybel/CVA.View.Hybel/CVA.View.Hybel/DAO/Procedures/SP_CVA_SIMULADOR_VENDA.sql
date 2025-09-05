IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_SIMULADOR_VENDA')
	DROP PROCEDURE SP_CVA_SIMULADOR_VENDA
GO
CREATE PROCEDURE SP_CVA_SIMULADOR_VENDA
AS
BEGIN
	-- Busca todos os componentes do produto
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
			SIM.U_Quantidade	QtdeVenda
		FROM BEAS_STL STL WITH(NOLOCK)
			INNER JOIN OITM WITH(NOLOCK)
				ON OITM.ItemCode = STL.ART1_ID
			INNER JOIN OITW WITH(NOLOCK)
				ON OITW.ItemCode = OITM.ItemCode
				AND OITW.WhsCode = STL.WhsCode
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
			Components.QtdeVenda
		FROM BEAS_STL STL WITH(NOLOCK)
			INNER JOIN OITM WITH(NOLOCK)
				ON OITM.ItemCode = STL.ART1_ID
			INNER JOIN OITW WITH(NOLOCK)
				ON OITW.ItemCode = OITM.ItemCode
				AND OITW.WhsCode = STL.WhsCode
			INNER JOIN Components 
				ON STL.ItemCode = Components.ItemCode	
	)
	-- Busca as OP's que estão GERANDO os componentes
	, OPEntrada AS
	(
		SELECT 
			Components.ItemCode,
			Components.ItemName,
			Components.UN,
			Components.QtdeDisponivel,
			SUM(Components.QtdeConsumida) QtdeConsumida,
			Components.EstoqueMin,
			ISNULL(PROD.QtdeProd, 0.00)	QtdeProd,
			SUM(Components.QtdeConsumida) * Components.QtdeVenda QtdeVenda
		FROM Components WITH(NOLOCK)
			LEFT JOIN	(
							SELECT 
								AUPT.ItemCode,
								SUM(POS.MENGE_VERBRAUCH) QtdeProd
							FROM BEAS_FTHAUPT AUPT WITH(NOLOCK)
								LEFT JOIN BEAS_FTPOS POS WITH(NOLOCK)
									ON POS.BELNR_ID = AUPT.BELNR_ID
									AND POS.ItemCode = AUPT.ItemCode 
									AND POS.STUFE = 0 -- Item produzido
							WHERE AUPT.ABGKZ = 'N'
							GROUP BY AUPT.ItemCode 
						) PROD ON PROD.ItemCode = Components.ItemCode
		GROUP BY 
			Components.ItemCode, Components.ItemName, Components.UN, Components.QtdeDisponivel, Components.EstoqueMin, PROD.QtdeProd, Components.QtdeVenda
	)
	--SELECT * FROM OPEntrada
	-- Busca a saída dos componentes nas OP
	, OPSaida AS
	(
		SELECT 
			OP.ItemCode,
			OP.ItemName,
			OP.UN,
			OP.QtdeDisponivel,
			OP.QtdeConsumida,
			OP.EstoqueMin,
			OP.QtdeProd,
			OP.QtdeVenda,
			CONS.OP,
			CONS.DocEntryPV,
			CONS.LineNumPV,
			CONS.QtdeConsOP
		FROM OPEntrada OP WITH(NOLOCK)
			LEFT JOIN	(
							SELECT 
								POS.ItemCode,
								AUPT.BELNR_ID		OP,
								AUPT.DocEntry		DocEntryPV,
								AUPT.AUFTRAGPOS		LineNumPV,
								POS.MENGE_VERBRAUCH QtdeConsOP
							FROM BEAS_FTHAUPT AUPT WITH(NOLOCK)
								INNER JOIN BEAS_FTPOS POS WITH(NOLOCK)
									ON POS.BELNR_ID = AUPT.BELNR_ID
									AND POS.STUFE <> 0 -- Componente
							WHERE AUPT.ABGKZ = 'N'
						) CONS ON CONS.ItemCode = OP.ItemCode
	) 
	 --SELECT * FROM OPSaida
	-- Busca todos os PRODUTOS que utilizam os componentes
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
			OP.QtdeVenda,
			OP.OP,
			OP.DocEntryPV,
			OP.LineNumPV,
			OP.QtdeConsOP,
			STL.ItemCode ParentId, ART1_ID ChildId, MENGE_LAGER AS QtdeConsPV, POS_ID AS Lvl 
		FROM BEAS_STL STL WITH(NOLOCK)
			INNER JOIN OPSaida OP WITH(NOLOCK)
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
			rc.QtdeVenda,
			rc.OP,
			rc.DocEntryPV,
			rc.LineNumPV,
			rc.QtdeConsOP,
			rh.ItemCode ParentId, rc.ChildId, CAST(RC.QtdeConsPV * RH.MENGE_LAGER AS NUMERIC(19, 6)) QtdeConsPV, RC.Lvl AS Lvl 
		FROM dbo.BEAS_STL rh WITH(NOLOCK)
			INNER JOIN Produtos rc ON rh.ART1_ID = rc.ParentId AND rc.ChildId <> rc.ItemCode
	)
	,CTE_RN AS 
	(
		SELECT *, ROW_NUMBER() OVER (PARTITION BY r.ChildID ORDER BY r.Lvl DESC) RN
		FROM Produtos r WITH(NOLOCK)
	)
	-- Agrupa os produtos para evitar duplicidade
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
			OP,
			DocEntryPV,
			LineNumPV,
			QtdeConsOP,
			QtdeProd,
			QtdeVenda,
			QtdeConsPV
		FROM CTE_RN WITH(NOLOCK)
		--WHERE Lvl = 10
		GROUP BY ParentId, ItemCode, ItemName, UN, QtdeDisponivel, QtdeConsumida, EstoqueMin, QtdeProd, QtdeVenda, QtdeConsPV, OP, DocEntryPV, LineNumPV, QtdeConsOP
	)
	-- Busca os pedidos de venda
	SELECT 
		CTE.ItemCode	[Código],
		CTE.ItemName	[Produto],
		CTE.UN,
		QtdeConsumida	[Qtde],
		QtdeDisponivel	[Est Fís],
		QtdeProd		[Est Enc],
		ISNULL(SUM(QtdeConsOP), 0) + ISNULL(SUM(QtdeConsPV * RDR1.OpenQty), 0) [Est Res],
		QtdeDisponivel - (SUM(ISNULL(QtdeConsPV * RDR1.OpenQty, 0)) + ISNULL(SUM(QtdeConsOP), 0)) [Est Dis],
		EstoqueMin		[Est Mín]
	FROM CTE_RN_GROUPED CTE WITH(NOLOCK)
		INNER JOIN OITM WITH(NOLOCK)
			ON OITM.ItemCode = CTE.ItemCode
			AND OITM.Phantom <> 'Y'
		LEFT JOIN	(
						SELECT RDR1.ItemCode, RDR1.OpenQty, RDR1.LineNum FROM RDR1 WITH(NOLOCK)
							INNER JOIN ORDR WITH(NOLOCK)
								ON ORDR.DocEntry = RDR1.DocEntry
							WHERE ORDR.DocStatus = 'O'
							AND RDR1.OpenQty > 0
					) RDR1
			ON RDR1.ItemCode = CTE.ParentId
			AND NOT EXISTS (SELECT TOP 1 1 FROM RDR1 WITH(NOLOCK) WHERE RDR1.DocEntry = CTE.DocEntryPV AND RDR1.LineNum = CTE.LineNumPV)
	GROUP BY CTE.ItemCode, CTE.ItemName, CTE.UN, QtdeDisponivel, QtdeConsumida, QtdeProd, EstoqueMin, QtdeVenda
	HAVING QtdeDisponivel - ISNULL((SUM(QtdeConsPV * RDR1.OpenQty) + ISNULL(SUM(QtdeConsOP), 0)), 0) - QtdeVenda < 0
	ORDER BY CTE.ItemName
	OPTION (MAXRECURSION 0)
END
