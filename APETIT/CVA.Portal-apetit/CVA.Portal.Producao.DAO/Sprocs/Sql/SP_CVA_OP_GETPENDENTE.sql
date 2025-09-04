ALTER PROCEDURE SP_CVA_OP_GETPENDENTE
(
	@Usuario	NVARCHAR(254),
	@DataDe 	DATE,
	@DataAte 	DATE,
	@NrOP		INT,
	@NrPedido	INT,
	@Item		NVARCHAR(1000),
	@Etapa		NVARCHAR(254)
)
AS
BEGIN
	SELECT
		OWOR.DocEntry,
		OWOR.DocNum		NrOP,
		OWOR.OriginAbs	DocEntryPedido,
		OWOR.OriginNum 	NrPedido,
		OWOR.PlannedQty	- OWOR.CmpltQty Quantidade,
		(SELECT TOP 1 (WOR1.IssuedQty / WOR1.BaseQty ) FROM WOR1 WHERE WOR1.DocEntry = OWOR.DocEntry AND WOR1.StageId = WOR4.StageId) QtdeRealizada,
		OWOR.ItemCode,
		CASE WHEN RDR1.Dscription IS NOT NULL 
			THEN RDR1.Dscription
			ELSE OITM.ItemName	
		END ItemName,
		OITM.U_modelo	Modelo,
		OITM.SLength1	Comprimento,
		OITM.SWidth1	Largura,
		OITM.SHeight1	Altura,
		CAST(CAST(OITM.SLength1 AS NUMERIC(10, 2)) AS NVARCHAR(10)) + ' x ' + CAST(CAST(OITM.SWidth1 AS NUMERIC(10, 2)) AS NVARCHAR(10)) + ' x ' + CAST(CAST(OITM.SHeight1 AS NUMERIC(10, 2)) AS NVARCHAR(10)) Medidas,
		OITM.U_CVA_MEDIDAS	MedidasMaxflex,
		WOR4.StageId	CodEtapa,
		WOR4.Name		Etapa,
		OWOR.Comments	ObsOP,
		ORDR.Comments	ObsPedido
	FROM OWOR 
		INNER JOIN WOR4
			ON WOR4.DocEntry = OWOR.DocEntry
		INNER JOIN [@CVA_USUARIO_ETAPA]
			ON [@CVA_USUARIO_ETAPA].U_CodEtapa = WOR4.Name
		INNER JOIN [@CVA_USUARIO]
			ON [@CVA_USUARIO].Code = [@CVA_USUARIO_ETAPA].U_CodUsuario
		INNER JOIN OITM
			ON OITM.ItemCode = OWOR.ItemCode
		LEFT JOIN RDR1
			ON RDR1.DocEntry = OWOR.OriginAbs
			AND RDR1.ItemCode = OWOR.ItemCode
		LEFT JOIN ORDR
			ON ORDR.DocEntry = RDR1.DocEntry
	WHERE OWOR.Status = 'R'
	AND NOT EXISTS
	(
		SELECT  1 FROM WOR4 WOR4TMP
		WHERE WOR4TMP.DocEntry = WOR4.DocEntry
		AND WOR4TMP.SeqNum < WOR4.SeqNum
		AND WOR4TMP.U_CVA_Status = 'O'
	)
	AND WOR4.U_CVA_Status <> 'C'
	AND [@CVA_USUARIO].U_Usuario = @Usuario
	AND (OWOR.DueDate BETWEEN @DataDe AND @DataAte)
	AND (OWOR.DocNum = @NrOP OR @NrOP IS NULL)
	AND (OWOR.OriginNum = @NrPedido OR @NrPedido IS NULL)
	AND (ISNULL(RDR1.Dscription, '') LIKE '%' + @Item + '%')
	AND (WOR4.Name = @Etapa OR @Etapa IS NULL)

	ORDER BY ORDR.DocNum, OWOR.DocNum, WOR4.SeqNum
END