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
				AND OITW.WhsCode = OITM.DfltWH
			INNER JOIN [@CVA_SIM_VENDA_ITEM] SIM WITH(NOLOCK)
				ON SIM.U_ItemCode = STL.ItemCode
		WHERE SIM.Code = 'manager2'

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
				AND OITW.WhsCode = OITM.DfltWH
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
			Components.ItemCode, Components.ItemName, Components.UN, Components.QtdeDisponivel, Components.EstoqueMin, PROD.QtdeProd, Components.QtdeVenda
	)
	--SELECT * FROM OPEntrada
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
			CONS.DocEntryPV,
			CONS.LineNumPV,
			CONS.QtdeConsOP
		FROM OPEntrada OP
			LEFT JOIN	(
							SELECT 
								POS.ItemCode,
								AUPT.DocEntry		DocEntryPV,
								AUPT.AUFTRAGPOS		LineNumPV,
								POS.MENGE_VERBRAUCH QtdeConsOP
							FROM BEAS_FTHAUPT AUPT WITH(NOLOCK)
								INNER JOIN BEAS_FTPOS POS WITH(NOLOCK)
									ON POS.BELNR_ID = AUPT.BELNR_ID
									AND POS.STUFE <> 0
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
			OP.DocEntryPV,
			OP.LineNumPV,
			OP.QtdeConsOP,
			STL.ItemCode ParentId, ART1_ID ChildId, MENGE_LAGER AS QtdeConsPV, POS_ID AS Lvl 
		FROM BEAS_STL STL WITH(NOLOCK)
			INNER JOIN OPSaida OP
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
			rc.DocEntryPV,
			rc.LineNumPV,
			rc.QtdeConsOP,
			rh.ItemCode ParentId, rc.ChildId, CAST(RC.QtdeConsPV * RH.MENGE_LAGER AS NUMERIC(19, 6)) QtdeConsPV, RC.Lvl AS Lvl 
		FROM dbo.BEAS_STL rh WITH(NOLOCK)
			INNER JOIN Produtos rc ON rh.ART1_ID = rc.ParentId
	)
	,CTE_RN AS 
	(
		SELECT *, ROW_NUMBER() OVER (PARTITION BY r.ChildID ORDER BY r.Lvl DESC) RN
		FROM Produtos r
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
			DocEntryPV,
			LineNumPV,
			QtdeConsOP,
			SUM(QtdeProd) QtdeProd,
			QtdeVenda,
			QtdeConsPV
		FROM CTE_RN
		WHERE Lvl = 10
		GROUP BY ParentId, ItemCode, ItemName, UN, QtdeDisponivel, QtdeConsumida, EstoqueMin, QtdeVenda, QtdeConsPV, DocEntryPV, LineNumPV, QtdeConsOP
	)
	--select * from CTE_RN_GROUPED

	SELECT * INTO #Products FROM CTE_RN_GROUPED
	SELECT ParentId, SUM(QtdeConsPV) QtdeConsPV INTO #ProductsDistinct FROM #Products GROUP BY ParentId
	SELECT * INTO #PedidosComOP FROM #Products WHERE ISNULL(DocEntryPV, 0) <> 0

	---- Busca os pedidos de venda sem OP
	SELECT 
		PROD.ParentId, 
		ISNULL(SUM(QtdeConsPV * RDR1.OpenQty), 0) [QtdePV]
	INTO #PedidosSemOP
	FROM #ProductsDistinct PROD
		LEFT JOIN	(
						SELECT RDR1.ItemCode, RDR1.OpenQty, RDR1.LineNum FROM RDR1 WITH(NOLOCK)
							INNER JOIN ORDR WITH(NOLOCK)
								ON ORDR.DocEntry = RDR1.DocEntry
							WHERE ORDR.DocStatus = 'O'
							AND RDR1.OpenQty > 0
					) RDR1
			ON RDR1.ItemCode = PROD.ParentId
			AND NOT EXISTS (
								SELECT TOP 1 1 FROM RDR1 WITH(NOLOCK) 
									INNER JOIN #PedidosComOP PED
										ON RDR1.DocEntry = PED.DocEntryPV AND RDR1.LineNum = PED.LineNumPV
							)
	GROUP BY PROD.ParentId

	--SELECT 
	--	PROD.ItemCode	[Código], 
	--	PROD.ItemName	[Produto],
	--	PROD.UN,
	--	QtdeConsumida	[Qtde],
	--	QtdeDisponivel	[Est Fís],
	--	QtdeProd		[Est Enc],
	--	ISNULL(SUM(QtdeConsOP), 0)  [Est Res]
		
	--FROM #Products PROD
	--GROUP BY PROD.ItemCode, PROD.ItemName, PROD.UN, QtdeDisponivel, QtdeConsumida, EstoqueMin, QtdeProd, EstoqueMin, QtdeVenda
	----HAVING QtdeDisponivel + QtdeProd - ISNULL((SUM(PV.QtdePV) + ISNULL(SUM(QtdeConsOP), 0)), 0) - QtdeVenda < 0
	--ORDER BY ItemName
	select * from #PedidosSemOP
	where ParentId = '30223056007'
	--SELECT 
	--	PROD.ItemCode	[Código], 
	--	PROD.ItemName	[Produto],
	--	PROD.UN,
	--	QtdeConsumida	[Qtde],
	--	QtdeDisponivel	[Est Fís],
	--	QtdeProd		[Est Enc],
	--	ISNULL(SUM(QtdeConsOP), 0) + ISNULL(SUM(PV.QtdePV), 0) [Est Res],
	--	QtdeDisponivel + QtdeProd - ISNULL((SUM(PV.QtdePV) + ISNULL(SUM(QtdeConsOP), 0)), 0) [Est Dis],
	--	EstoqueMin		[Est Mín]
	--FROM #Products PROD
	--	LEFT JOIN #PedidosSemOP PV
	--		ON PROD.ItemCode = PROD.ItemCode
	--GROUP BY PROD.ItemCode, PROD.ItemName, PROD.UN, QtdeDisponivel, QtdeConsumida, EstoqueMin, QtdeProd, EstoqueMin, QtdeVenda
	--HAVING QtdeDisponivel + QtdeProd - ISNULL((SUM(PV.QtdePV) + ISNULL(SUM(QtdeConsOP), 0)), 0) - QtdeVenda < 0
	--ORDER BY ItemName

	DROP TABLE #Products
	DROP TABLE #ProductsDistinct
	DROP TABLE #PedidosComOP
	DROP TABLE #PedidosSemOP
