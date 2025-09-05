IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_ROTEIRO_OP_LAYOUT')
	DROP PROCEDURE SP_ROTEIRO_OP_LAYOUT
GO
CREATE PROCEDURE SP_ROTEIRO_OP_LAYOUT
(
	@Tipo	NVARCHAR(2),
	@NrDoc	INT = NULL
)
AS
BEGIN
	SELECT
		OBPL.BPLName,
		OBPL.BPLFrName,
		OBPL.AliasName BPLAliasName,
		OADP.LogoImage LogoImage,
		OITM.ItemCode,
		OITM.ItemName,
		OITM.InvntryUom			[UN],
		OITM.BWeight1			[PesoLiquido],
		OITM.U_h_peso_bruto		[PesoBruto],
		OITM.U_h_desc_localizacao [Localizacao],
		OITM.OnHand		[Est Fís],
		PROD.QtdeProd	[Est Enc],
		OITW.MinStock	[Est Mín],
		RDR1.QtdePV,
		CONS.QtdeConsOP,
		ONCM.NcmCode,
		UFD1.Descr		[Grupo],
		OITB.ItmsGrpNam	[SubGrupo],
		AUPT.ErfUser	[Usuario],
		AUPT.BELNR_ID	[OP],
		AUPT.BELDAT		[Dt. Cadastro],
		AUPT.LFGDAT		[Dt. Entrega],
		AUPT.ANFZEIT	[Dt. Início],
		AUPT.KND_ID		[Cód. Cliente],
		AUPT.KNDNAME	[Cliente],
		POS.MENGE_VERBRAUCH	[Qtde],
		APL.POS_ID		[N Proc.],
		APL.[G.M.],
		APL.Instrução,
		APL.[% Concluído],
		APL.[Dt. Inicial],
		APL.[Dt. Final],
		APL.QtdeProduzida
	
	INTO #RoteiroOP
	FROM BEAS_FTHAUPT AUPT WITH(NOLOCK)
	
		INNER JOIN OBPL WITH(NOLOCK)
			ON OBPL.BPLId = 1
		INNER JOIN OITM WITH(NOLOCK)
			ON OITM.ItemCode = AUPT.ItemCode
		INNER JOIN OITW WITH(NOLOCK)
			ON OITW.ItemCode = OITM.ItemCode
			AND OITW.WhsCode = OBPL.DflWhs
		INNER JOIN OITB WITH(NOLOCK)
			ON OITB.ItmsGrpCod = OITM.ItmsGrpCod
		INNER JOIN BEAS_FTPOS POS WITH(NOLOCK)
			ON POS.BELNR_ID = AUPT.BELNR_ID
			AND POS.STUFE = 0
		INNER JOIN -- Roteiro
		(
			SELECT APL.BELNR_ID, APL.BELPOS_ID, APL.POS_ID,
				CAST(APLATZ.BEZ AS NVARCHAR(MAX))	[G.M.],
				CAST(APL.BEZ AS NVARCHAR(MAX))		[Instrução],
				SUM(CASE WHEN APL.ABGKZ = 'J' THEN 1.00 ELSE 0.00 END) / COUNT(APL.ABGKZ) * 100 [% Concluído],
				SUM(ZEIT.MENGE_GUT)	[QtdeProduzida],
				MIN(ZEIT.ANFZEIT)	[Dt. Inicial],
				MAX(ZEIT.ENDZEIT)	[Dt. Final]
				FROM BEAS_FTAPL APL WITH(NOLOCK)
					INNER JOIN BEAS_APLATZ APLATZ WITH(NOLOCK)
						ON APLATZ.APLATZ_ID = APL.APLATZ_ID
					LEFT JOIN BEAS_ARBZEIT ZEIT WITH(NOLOCK)
						ON ZEIT.BELNR_ID = APL.BELNR_ID
						AND ZEIT.BELPOS_ID = APL.BELPOS_ID
				GROUP BY APL.BELNR_ID, APL.BELPOS_ID, APL.POS_ID,
				CAST(APLATZ.BEZ AS NVARCHAR(MAX)),
				CAST(APL.BEZ AS NVARCHAR(MAX))
		 ) APL
			ON APL.BELNR_ID = AUPT.BELNR_ID
			AND APL.BELPOS_ID = POS.BELPOS_ID
		LEFT JOIN -- Quantidades sendo produzidas
		(
			SELECT 
				AUPT.ItemCode,
				SUM(POS.MENGE_VERBRAUCH) QtdeProd
			FROM BEAS_FTHAUPT AUPT WITH(NOLOCK)
				LEFT JOIN BEAS_FTPOS POS WITH(NOLOCK)
					ON POS.BELNR_ID = AUPT.BELNR_ID
					AND POS.ItemCode = AUPT.ItemCode 
					AND POS.STUFE = 0 -- Item produzido
			WHERE AUPT.ABGKZ = 'N'
			AND @NrDoc <> CASE WHEN @Tipo = 'OP' THEN AUPT.BELNR_ID ELSE AUPT.DocEntry END -- Remove a própria OP
			GROUP BY AUPT.ItemCode 
		) PROD ON PROD.ItemCode = AUPT.ItemCode
		LEFT JOIN-- Quantidades sendo consumidas
		(
			SELECT POS.ItemCode, SUM(POS.MENGE_VERBRAUCH) QtdeConsOP
			FROM BEAS_FTHAUPT AUPT WITH(NOLOCK)
				INNER JOIN BEAS_FTPOS POS WITH(NOLOCK)
					ON POS.BELNR_ID = AUPT.BELNR_ID
					AND POS.STUFE <> 0 -- Componente
			WHERE AUPT.ABGKZ = 'N'
			GROUP BY POS.ItemCode
		) CONS ON CONS.ItemCode = AUPT.ItemCode
		LEFT JOIN -- Pedidos de venda que não tem OP
		(
			SELECT RDR1.ItemCode, SUM(RDR1.OpenQty) QtdePV FROM RDR1 WITH(NOLOCK)
				INNER JOIN ORDR WITH(NOLOCK)
					ON ORDR.DocEntry = RDR1.DocEntry
				WHERE ORDR.DocStatus = 'O'
				AND RDR1.OpenQty > 0
				AND NOT EXISTS 
				(
					SELECT TOP 1 1 FROM RDR1 WITH(NOLOCK) 
						INNER JOIN BEAS_FTHAUPT AUPT WITH(NOLOCK) 
							ON AUPT.DocEntry = RDR1.DocEntry
							AND AUPT.AUFTRAGPOS = RDR1.LineNum
				)
				GROUP BY RDR1.ItemCode
		) RDR1 ON RDR1.ItemCode = AUPT.ItemCode
		LEFT JOIN ONCM WITH(NOLOCK)
			ON ONCM.AbsEntry = OITM.NcmCode
		LEFT JOIN  CUFD WITH(NOLOCK)
			ON CUFD.TableId = 'OITM'
			AND CUFD.AliasId = 'h_cod_grupo'
		INNER JOIN UFD1 WITH(NOLOCK)
			ON UFD1.TableId = CUFD.TableId
			AND UFD1.FieldId = CUFD.FieldId
			AND UFD1.FldValue = OITM.U_h_cod_grupo
		LEFT JOIN OADP WITH(NOLOCK)
			ON 1 = 1
	WHERE @NrDoc = CASE WHEN @Tipo = 'OP' THEN AUPT.BELNR_ID ELSE AUPT.DocEntry END

	-- Busca os produtos que usam o item como componente
	;WITH Produtos AS
	(
		SELECT 
			OP.ItemCode,
			OP.ItemName,
			OP.UN,
			STL.ItemCode ParentId, ART1_ID ChildId, MENGE_LAGER AS QtdeConsumida, POS_ID AS Lvl 
		FROM BEAS_STL STL WITH(NOLOCK)
			INNER JOIN #RoteiroOP OP
				ON OP.ItemCode = STL.ART1_ID

		UNION ALL

		SELECT 
			rc.ItemCode,
			rc.ItemName,
			rc.UN,
			rh.ItemCode ParentId, rc.ChildId, CAST(RC.QtdeConsumida * RH.MENGE_LAGER AS NUMERIC(19, 6)) QtdeConsumida, RC.Lvl AS Lvl 
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
			QtdeConsumida
		FROM CTE_RN
		--WHERE Lvl = 10
		GROUP BY ParentId, ItemCode, ItemName, UN, QtdeConsumida
	)
	-- Busca os pedidos de venda dos produtos para  chegar no estoque reservado
	SELECT 
		OP.*,
		ISNULL(OP.QtdeConsOP, 0) + ISNULL(OP.QtdePV, 0) + ISNULL(QtdePVPai, 0) [Est Res],
		OP.[Est Fís] + OP.[Est Enc] - (ISNULL(OP.QtdeConsOP, 0) + ISNULL(OP.QtdePV, 0) + ISNULL(QtdePVPai, 0)) [Est Dis]
	FROM #RoteiroOP OP 
		LEFT JOIN CTE_RN_GROUPED CTE
			ON CTE.ItemCode = OP.ItemCode
		LEFT JOIN	
		(
			SELECT RDR1.ItemCode, SUM(RDR1.OpenQty) QtdePVPai FROM RDR1 WITH(NOLOCK)
				INNER JOIN ORDR WITH(NOLOCK)
					ON ORDR.DocEntry = RDR1.DocEntry
				WHERE ORDR.DocStatus = 'O'
				AND RDR1.OpenQty > 0
				AND NOT EXISTS 
				(
					SELECT TOP 1 1 FROM RDR1 WITH(NOLOCK) 
						INNER JOIN BEAS_FTHAUPT AUPT WITH(NOLOCK) 
							ON AUPT.DocEntry = RDR1.DocEntry
							AND AUPT.AUFTRAGPOS = RDR1.LineNum
							AND AUPT.ABGKZ = 'N'
				)
			GROUP BY RDR1.ItemCode
		) RDR1 ON RDR1.ItemCode = CTE.ParentId
END