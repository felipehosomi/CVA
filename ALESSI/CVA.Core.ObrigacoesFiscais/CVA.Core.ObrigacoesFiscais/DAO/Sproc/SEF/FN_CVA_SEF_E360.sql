IF EXISTS (SELECT * FROM sys.objects WHERE type = 'IF' AND name = 'FN_CVA_SEF_E360')
	DROP FUNCTION FN_CVA_SEF_E360
GO

-- SELECT * FROM FN_CVA_SEF_E360 (9, '2018-01-01', '2018-01-31')
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
		'PE'			UF_OR,
		'800'			COD_OR,
		@DataFinal		PER_REF,
		''				COD_REC,
		U_Imposto		VL_ICMS_REC,
		@DataFinal		DT_VCTO,
		''				NUM_PROC,
		''				IND_PROC,
		''				DESCR_PROC,
		''				COD_INF_OBS
			FROM [@SKILL_005_APR1] 
				WHERE U_DtInicio BETWEEN @DataInicial AND @DataFinal AND U_BPLId = @Filial AND U_TipoImposto = 'ICMS'
				

		union all 

		SELECT 
			U_UF					UF_OR, 
			'803'					COD_OR,
			@DataFinal              PER_REF,
			''						COD_REC,
			U_Imposto				VL_ICMS_REC,
			@DataFinal				DT_VCTO,
		''				NUM_PROC,
		''				IND_PROC,
		''				DESCR_PROC,
		''				COD_INF_OBS  

			FROM [@SKILL_005_APR6]
				LEFT JOIN  OBPL ON [@SKILL_005_APR6].U_BPLId = OBPL.BPLId
					WHERE U_BPLId = @Filial 
					AND U_DtInicio BETWEEN @DataInicial AND @DataFinal
					AND U_Imposto <> 0.00
					AND OBPL.State = U_UF				

		union all 

		SELECT 
			U_UF					UF_OR, 
			'890'					COD_OR,
			@DataFinal              PER_REF,
			''						COD_REC,
			U_Imposto				VL_ICMS_REC,
			@DataFinal				DT_VCTO,
		''				NUM_PROC,
		''				IND_PROC,
		''				DESCR_PROC,
		''				COD_INF_OBS  

			FROM [@SKILL_005_APR6]
				LEFT JOIN  OBPL ON [@SKILL_005_APR6].U_BPLId = OBPL.BPLId
					WHERE U_BPLId = @Filial 
					AND U_DtInicio BETWEEN @DataInicial AND @DataFinal
					AND U_Imposto <> 0.00
					AND OBPL.State <> U_UF


	--SELECT
	--	U_UF			UF_OR,
	--	U_COD_OR		COD_OR,
	--	@DataInicial	PER_REF,
	--	U_COD_REC		COD_REC,
	--	U_VL_OR			VL_ICMS_REC,
	--	U_DT_VCTO		DT_VCTO,
	--	''			NUM_PROC,
	--	''			IND_PROC,
	--	''			DESCR_PROC,
	--	''			COD_INF_OBS
	--	 FROM [@SKILL_005_APR11] 
	--WHERE U_TipoImposto = 'ICMS-ST' 
	--AND MONTH(U_DtOperacao) = MONTH(@DataInicial)
	--AND YEAR(U_DtOperacao) = YEAR(@DataInicial)
	--AND U_BPLId = @Filial
)
