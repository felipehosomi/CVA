IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_TAXA_CONVERSAO_VENDA')
	DROP PROCEDURE SP_CVA_TAXA_CONVERSAO_VENDA
GO
CREATE PROCEDURE SP_CVA_TAXA_CONVERSAO_VENDA
(
	@Motivo		NVARCHAR(100),
	@DataDe		DATETIME,
	@DataAte	DATETIME,
	@Filial		INT
)
AS
BEGIN
	IF @Motivo = '*'
	BEGIN
		SET @Motivo = NULL
	END
	IF @Filial = 0
	BEGIN
		SET @Filial = NULL
	END

	CREATE TABLE #DADOS
	(
		CanceladosCount		INT,
		FaturadosCount		INT,
		AtrasadosCount		INT,
		NaoFinalizadosCount	INT,
		TotalCount			INT,
		PorcCount			NUMERIC(19, 2),
		Cancelados			NUMERIC(19, 2),
		Faturados			NUMERIC(19, 2),
		Atrasados			NUMERIC(19, 2),
		NaoFinalizados		NUMERIC(19, 2),
		Total				NUMERIC(19, 2),
		Porc				NUMERIC(19, 2)
	)

	INSERT INTO #DADOS (CanceladosCount, Cancelados)
	SELECT COUNT(*), SUM(DocTotal) FROM ORDR WHERE CANCELED = 'Y'
	AND ISNULL(@Motivo, ORDR.U_CVA_Motivo_Canc) = ORDR.U_CVA_Motivo_Canc
	AND ISNULL(@Filial, ORDR.BPLId) = ORDR.BPLId
	AND ORDR.DocDate BETWEEN @DataDe AND @DataAte

	UPDATE #DADOS
	SET CanceladosCount	=	ISNULL(CanceladosCount, 0)	+	Quantidade,
		Cancelados		=	ISNULL(Cancelados, 0)		+	DocTotal
	FROM 
	(
		SELECT  COUNT(*) Quantidade, SUM(DocTotal) DocTotal 
		FROM OQUT WHERE CANCELED = 'Y' 
		AND ISNULL(@Motivo, OQUT.U_CVA_Motivo_Canc) = OQUT.U_CVA_Motivo_Canc
		AND ISNULL(@Filial, OQUT.BPLId) = OQUT.BPLId
		AND OQUT.DocDate BETWEEN @DataDe AND @DataAte
	) OQUT

	UPDATE #DADOS
	SET NaoFinalizadosCount	=	ISNULL(Quantidade, 0),
		NaoFinalizados		=	ISNULL(DocTotal, 0)
	FROM 
	(
		SELECT  COUNT(*) Quantidade, SUM(DocTotal) DocTotal 
		FROM ORDR WHERE DocStatus = 'O'
		AND ISNULL(@Filial, ORDR.BPLId) = ORDR.BPLId
		AND ORDR.DocDate BETWEEN @DataDe AND @DataAte
	) ORDR
	
	UPDATE #DADOS
	SET NaoFinalizadosCount	=	ISNULL(NaoFinalizadosCount, 0)	+	ISNULL(Quantidade, 0),
		NaoFinalizados		=	ISNULL(NaoFinalizados, 0)		+	ISNULL(DocTotal, 0)
	FROM 
	(
		SELECT  COUNT(*) Quantidade, SUM(DocTotal) DocTotal FROM OQUT WHERE DocStatus = 'O'
		AND ISNULL(@Filial, OQUT.BPLId) = OQUT.BPLId
		AND OQUT.DocDate BETWEEN @DataDe AND @DataAte
	) OQUT

	UPDATE #DADOS
	SET FaturadosCount	=	ISNULL(Quantidade, 0),
		Faturados		=	ISNULL(DocTotal, 0)
	FROM 
	(
		SELECT  COUNT(*) Quantidade, SUM(DocTotal) DocTotal FROM OINV WHERE CANCELED <> 'Y'
		AND ISNULL(@Filial, OINV.BPLId) = OINV.BPLId
		AND OINV.DocDate BETWEEN @DataDe AND @DataAte
	) OINV

	UPDATE #DADOS
	SET AtrasadosCount	=	ISNULL(Quantidade, 0), 
		Atrasados		=	ISNULL(InsTotal, 0)
	FROM 
	(
		SELECT COUNT(*) Quantidade, SUM(InsTotal) InsTotal 
		FROM OINV INNER JOIN INV6 ON INV6.DocEntry = OINV.DocEntry
		WHERE OINV.CANCELED <> 'Y' AND INV6.PaidToDate = 0 AND INV6.DueDate < CONVERT(DATE, GETDATE())
		AND ISNULL(@Filial, OINV.BPLId) = OINV.BPLId
		AND INV6.DueDate BETWEEN @DataDe AND @DataAte
	) INV6

	UPDATE #DADOS
	SET TotalCount	= CanceladosCount + FaturadosCount + AtrasadosCount + NaoFinalizadosCount,
		Total		= Cancelados + Faturados + Atrasados + NaoFinalizados
	FROM #DADOS

	UPDATE #DADOS
	SET PorcCount	=	((FaturadosCount * 100.00) / TotalCount),
		Porc		=	((Faturados * 100.00) / Total)
	FROM #DADOS

	SELECT * FROM #DADOS

	DROP TABLE #DADOS
END