IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_SIMULADOR_SAIDA')
	DROP PROCEDURE SP_CVA_SIMULADOR_SAIDA
GO
CREATE PROCEDURE SP_CVA_SIMULADOR_SAIDA
(
	@ItemCode		NVARCHAR(60)
)
AS
BEGIN
	SELECT 
		TT.ItemCode,
		CONS.[Nr. OP],
		CONS.DocEntryPV,
		CONS.LineNumPV,
		CONS.CardCodePV,
		CONS.QtdeConsOP,
		CONS.[Dt. Lanc],
		CONS.[Dt. Reserva]
	INTO #OP
	FROM (SELECT @ItemCode ItemCode) TT
		LEFT JOIN	(
						SELECT
							POS.ItemCode,
							AUPT.BELNR_ID		[Nr. OP],
							AUPT.LFGDAT			[Dt. Lanc],
							AUPT.BELDAT			[Dt. Reserva],
							AUPT.KND_ID			CardCodePV,
							AUPT.DocEntry		DocEntryPV,
							AUPT.AUFTRAGPOS		LineNumPV,
							POS.MENGE_VERBRAUCH QtdeConsOP
						FROM BEAS_FTHAUPT AUPT WITH(NOLOCK)
							INNER JOIN BEAS_FTPOS POS WITH(NOLOCK)
								ON POS.BELNR_ID = AUPT.BELNR_ID
								AND POS.STUFE <> 0
						WHERE AUPT.ABGKZ = 'N'
					) CONS ON CONS.ItemCode = @ItemCode
	
	;WITH Produtos AS
	(
		SELECT 
			STL.ItemCode ParentId, ART1_ID ChildId, MENGE_LAGER AS QtdeConsPV, POS_ID AS Lvl 
		FROM BEAS_STL STL WITH(NOLOCK)
			WHERE STL.ItemCode = @ItemCode

		UNION ALL

		SELECT 
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
			QtdeConsPV
		FROM CTE_RN
		WHERE Lvl = 10
		GROUP BY ParentId, QtdeConsPV
	)
	-- Busca os pedidos de venda
	SELECT 
		RDR1.WhsCode	[Depósito],
		ORDR.DocNum		[Nr. Doc],
		ORDR.DocDate	[Dt Reserva],
		ORDR.DocDueDate	[Dt Lanc],
		RDR1.OpenQty [Qtde],
		ORDR.CardName	[Cliente]
	INTO #Pedido
	FROM CTE_RN_GROUPED CTE
		INNER JOIN RDR1 WITH(NOLOCK)
			ON RDR1.ItemCode = CTE.ParentId
			AND RDR1.OpenQty > 0
		INNER JOIN ORDR WITH(NOLOCK)
			ON ORDR.DocEntry = RDR1.DocEntry
			AND ORDR.DocStatus = 'O'
	WHERE NOT EXISTS
	(
		SELECT TOP 1 1 FROM RDR1 WITH(NOLOCK) 
			INNER JOIN #OP OP
				ON OP.DocEntryPV = RDR1.DocEntry
				AND OP.LineNumPV = RDR1.LineNum
	)
	ORDER BY ORDR.DocNum

	SELECT * FROM
	(
		SELECT DISTINCT
			OP.[Nr. OP]	[Nr. Doc],
			OP.[Dt. Reserva],
			OP.[Dt. Lanc],
			CASE WHEN ISNULL(DocEntryPV, 0) = 0
				THEN 'OP'
				ELSE CASE WHEN ISNULL(OBPL.BPlId, 0) = 0
					THEN 'PC'
					ELSE 'PE'
				END
			END Tipo,
			OP.QtdeConsOP	[Qtde],
			CAST(OBPL.AliasName	AS NVARCHAR(MAX)) [Cliente]
		FROM #OP OP
			LEFT JOIN CRD7 WITH(NOLOCK)
				ON CRD7.CardCode = OP.CardCodePV
			LEFT JOIN OBPL WITH(NOLOCK)
				ON OBPL.TaxIdNum = CRD7.TaxId0
		WHERE OP.[Nr. OP] IS NOT NULL

		UNION ALL

		SELECT 
			Pedido.[Nr. Doc],
			Pedido.[Dt Reserva],
			Pedido.[Dt Lanc],
			'PV',
			SUM(Pedido.[Qtde]),
			Pedido.[Cliente]
		FROM #Pedido Pedido
		GROUP BY
			Pedido.[Nr. Doc],
			Pedido.[Dt Reserva],
			Pedido.[Dt Lanc],
			Pedido.[Cliente]
	) Reserva
	ORDER BY [Nr. Doc]

	DROP TABLE #OP
	DROP TABLE #Pedido
END