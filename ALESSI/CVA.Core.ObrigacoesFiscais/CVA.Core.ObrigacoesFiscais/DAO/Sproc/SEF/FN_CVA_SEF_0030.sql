IF EXISTS (SELECT * FROM sys.objects WHERE type = 'IF' AND name = 'FN_CVA_SEF_0030')
	DROP FUNCTION FN_CVA_SEF_0030
GO
CREATE FUNCTION FN_CVA_SEF_0030
(
	@Filial			INT,
	@DataInicial	DATETIME,
	@DataFinal		DATETIME
) RETURNS TABLE
AS
RETURN
(
	SELECT
		1       			IND_ED,
		5					IND_ARQ,
		9					PRF_ISS,
		2					PRF_ICMS,
		1					PRF_RIDF,
		1					PRF_RUDF,
		1					PRF_LMC,
		1					PRF_RV,
		0					PRF_RI,
		9					IND_EC,
		1					IND_ISS,
		1					IND_RT,
		1					IND_ICMS,
		1					IND_ST,
		1					IND_AT,
		0					IND_IPI,
		1					IND_RI
)
