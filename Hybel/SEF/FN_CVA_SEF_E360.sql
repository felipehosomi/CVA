IF EXISTS (SELECT * FROM sys.objects WHERE type = 'IF' AND name = 'FN_CVA_SEF_E360')
	DROP FUNCTION FN_CVA_SEF_E360
GO
CREATE FUNCTION FN_CVA_SEF_E360
(
	@Filial			INT,
	@DataInicial	DATETIME,
	@DataFinal		DATETIME
) RETURNS TABLE
AS
RETURN
(
	SELECT
		U_UF			UF_OR,
		U_COD_OR		COD_OR,
		@DataInicial	PER_REF,
		U_COD_REC		COD_REC,
		U_VL_OR			VL_ICMS_REC,
		U_DT_VCTO		DT_VCTO,
		''			NUM_PROC,
		''			IND_PROC,
		''			DESCR_PROC,
		''			COD_INF_OBS
		 FROM [@SKILL_005_APR11] 
	WHERE U_TipoImposto = 'ICMS-ST' 
	AND MONTH(U_DtOperacao) = MONTH(@DataInicial)
	AND YEAR(U_DtOperacao) = YEAR(@DataInicial)
	AND U_BPLId = @Filial
)
