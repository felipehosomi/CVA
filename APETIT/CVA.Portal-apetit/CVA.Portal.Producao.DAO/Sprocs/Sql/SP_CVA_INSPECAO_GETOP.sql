ALTER PROCEDURE SP_CVA_INSPECAO_GETOP
(
	@DataDe		DATE,
	@DataAte	DATE,
	@Status		VARCHAR(1),
	@Pedido		INT,
	@OP			INT,
	@Item		NVARCHAR(50)
)
AS
BEGIN
	IF @Status = 'P'
	BEGIN
		SELECT
			'' CodFicha,
			'P' StatusFicha,
			OWOR.DocEntry,
			OWOR.DocNum,
			OWOR.StartDate DocDate,
			OWOR.OriginNum Serial,
			OWOR.ItemCode,
			OITM.ItemName,
			OWOR.PlannedQty Quantity,
			FICHA.U_CodEtapa	CodEtapa,
			CAST(APON.U_QtdeCQ AS INT)		Sequencia
		FROM OWOR
			INNER JOIN WOR4
				ON WOR4.DocEntry = OWOR.DocEntry
			INNER JOIN OITM
				ON OITM.ItemCode = OWOR.ItemCode
			INNER JOIN [@CVA_FICHA_PRODUTO] FICHA
				ON (FICHA.U_CodItem = OITM.ItemCode OR FICHA.U_CodGrupo = OITM.ItmsGrpCod)
				AND FICHA.U_CodEtapa = WOR4.Name
				AND FICHA.U_Ativo = 1
			LEFT JOIN [@CVA_APONTAMENTO] APON
				ON APON.U_NrOP = OWOR.DocNum
				AND APON.U_CodEtapa = WOR4.Name
		WHERE OWOR.StartDate BETWEEN @DataDe AND @DataAte
		AND OWOR.PlannedQty - ISNULL(APON.U_QtdeCQ , 0) > 0
		AND (OWOR.DocNum = @OP OR @OP IS NULL)
		AND (OWOR.OriginNum = @Pedido OR @Pedido IS NULL)
		AND (OWOR.ItemCode = @Item OR @Item IS NULL)
	END
	ELSE
	BEGIN
		SELECT
			INSP.Code CodFicha,
			INSP.U_Status StatusFicha,
			OWOR.DocEntry,
			OWOR.DocNum,
			OWOR.StartDate DocDate,
			OWOR.OriginNum Serial,
			OWOR.ItemCode,
			OITM.ItemName,
			OWOR.CmpltQty Quantity,
			INSP.U_CodEtapa	CodEtapa,
			INSP.U_Sequencia	Sequencia
		FROM OWOR
			INNER JOIN WOR4
				ON WOR4.DocEntry = OWOR.DocEntry
			INNER JOIN OITM
				ON OITM.ItemCode = OWOR.ItemCode
			LEFT JOIN [@CVA_FICHA_INSPECAO] INSP
				ON INSP.U_DocEntry = OWOR.DocEntry
				AND INSP.U_CodEtapa = WOR4.Name
		WHERE INSP.U_DataInsp BETWEEN @DataDe AND @DataAte
		AND INSP.U_Status = @Status
		AND (OWOR.DocNum = @OP OR @OP IS NULL)
		AND (OWOR.OriginNum = @Pedido OR @Pedido IS NULL)
		AND (OWOR.ItemCode = @Item OR @Item IS NULL)
	END
END