IF EXISTS (SELECT * FROM sys.objects WHERE type = 'IF' AND name = 'FN_CVA_SEF_E340')
	DROP FUNCTION FN_CVA_SEF_E340
GO
CREATE FUNCTION FN_CVA_SEF_E340
(
	@Filial			INT,
	@DataInicial	DATETIME,
	@DataFinal		DATETIME
) RETURNS TABLE
AS
RETURN
(
	WITH ICMS AS
	(
		SELECT
			ICMS_ST.U_Entradas	VL_01,
			0.00				VL_02,
			0.00				VL_03,
			0.00				VL_04,
			0.00				VL_05,
			0.00				VL_06,
			ICMS_ANT.U_Saldo	VL_07,
			ICMS_ST.U_Saidas	VL_09,
			0.00		VL_10,
			0.00		VL_11,
			ICMS_ST.U_Deducoes	VL_15,
			0.00		VL_17,
			0.00		VL_18,
			ICMS_UF.U_Imposto	VL_19,
			0.00		VL_20,
			0.00		VL_21,
			0.00		VL_23,
			ICMS_OUT_UF.Imposto	VL_99
			FROM OBPL WITH(NOLOCK)
				LEFT JOIN [@SKILL_005_APR1] ICMS_ST WITH(NOLOCK)
					ON	ICMS_ST.U_TipoImposto = 'ICMS-ST' 
					AND ICMS_ST.U_DtInicio = @DataInicial
					AND ICMS_ST.U_BPLId = OBPL.BPLId
				LEFT JOIN [@SKILL_005_APR1] ICMS_ANT WITH(NOLOCK)
					ON ICMS_ANT.U_TipoImposto = 'ICMS' 
					AND ICMS_ANT.U_DtInicio = DATEADD(MONTH, -1, @DataInicial)
					AND ICMS_ANT.U_BPLId = OBPL.BPLId
				LEFT JOIN [@SKILL_005_APR6] ICMS_UF WITH(NOLOCK)
					ON ICMS_UF.U_DtInicio = @DataInicial
					AND ICMS_UF.U_BPLId = OBPL.BPLId
					AND ICMS_UF.U_UF = OBPL.State
				OUTER APPLY
				(
					SELECT SUM(U_Imposto) Imposto FROM [@SKILL_005_APR6] ICMS_OUT_UF WITH(NOLOCK)
					WHERE ICMS_OUT_UF.U_DtInicio = @DataInicial
					AND ICMS_OUT_UF.U_BPLId = OBPL.BPLId
					AND ICMS_OUT_UF.U_UF <> OBPL.State
					GROUP BY U_DtInicio, U_BPLId
				) ICMS_OUT_UF
			WHERE OBPL.BPLId = @Filial
	)
	, Somatorio AS
	(
		SELECT *,
			VL_01 + VL_02 + VL_03 + VL_04 + VL_05 + VL_06 + VL_07 AS VL_08,
			VL_09 + VL_10 + VL_11	AS VL_12
		FROM ICMS
	)
	, Saldo AS
	(
		SELECT *,
		CASE WHEN VL_08 - VL_12 > 0
			THEN VL_08- VL_12
			ELSE 0
		END AS VL_13,
		CASE WHEN VL_12 - VL_08 > 0
			THEN VL_12 - VL_08
			ELSE 0
		END AS VL_14,
		CASE WHEN VL_12 - VL_08 > 0
			THEN VL_12 - VL_08 - VL_15
			ELSE 0
		END AS VL_16
		FROM Somatorio
	)
	, Total AS
	(
		SELECT *,
			VL_16 + VL_17 + VL_18 + VL_19 + VL_20 + VL_21 AS VL_22
		FROM Saldo
	)
	SELECT
		VL_01,
		VL_02,
		VL_03,
		VL_04,
		VL_05,
		VL_06,
		VL_07,
		VL_08,
		VL_09,
		VL_10,
		VL_11,
		VL_12,
		VL_13,
		VL_14,
		VL_15,
		VL_16,
		VL_17,
		VL_18,
		VL_19,
		VL_20,
		VL_21,
		VL_22,
		VL_99
	FROM Total
)
