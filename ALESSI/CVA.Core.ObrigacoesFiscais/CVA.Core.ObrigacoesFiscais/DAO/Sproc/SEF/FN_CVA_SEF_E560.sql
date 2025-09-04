IF EXISTS (SELECT * FROM sys.objects WHERE type = 'IF' AND name = 'FN_CVA_SEF_E560')
	DROP FUNCTION FN_CVA_SEF_E560
GO
CREATE FUNCTION FN_CVA_SEF_E560
(
	@Filial			INT,
	@DataInicial	DATETIME,
	@DataFinal		DATETIME
) RETURNS TABLE
AS
RETURN
(
	SELECT
		''				COD_OR_IPI,
		@DataInicial	PER_REF,
		5123			COD_REC_IPI,
		0.00			VL_IPI_REC,
		@DataInicial	DT_VCTO,
		''				IND_DOC,
		''				NUM_DOC,
		''				DESCR_AJ,
		''				COD_INF_OBS
)
