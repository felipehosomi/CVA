IF EXISTS (SELECT * FROM sys.objects WHERE type = 'IF' AND name = 'FN_CVA_SEF_E540')
	DROP FUNCTION FN_CVA_SEF_E540
GO

--- SELECT * FROM FN_CVA_SEF_E540 (9, '2018-02-01' , '2018-02-28')
CREATE FUNCTION FN_CVA_SEF_E540
(
	@Filial			INT,
	@DataInicial	DATETIME,
	@DataFinal		DATETIME
) RETURNS TABLE
AS
RETURN
(
	WITH IPI AS
	(
		SELECT
			IPI.U_Entradas	VL_01,
			0.00			VL_02,
			0.00			VL_03,
			0.00			VL_04,
			0.00			VL_05,
			IPI_ANT.U_Saldo	VL_07,
			IPI.U_Saidas	VL_09,
			0.00			VL_10,
			0.00			VL_11,
			0.00			VL_12
			FROM OBPL WITH(NOLOCK)
				LEFT JOIN [@SKILL_005_APR1] IPI WITH(NOLOCK)
					ON	IPI.U_TipoImposto = 'IPI' 
					AND IPI.U_DtInicio = @DataInicial
					AND IPI.U_BPLId = OBPL.BPLId
				LEFT JOIN [@SKILL_005_APR1] IPI_ANT WITH(NOLOCK)
					ON IPI_ANT.U_TipoImposto = 'IPI' 
					AND IPI_ANT.U_DtInicio = DATEADD(MONTH, -1, @DataInicial)
					AND IPI_ANT.U_BPLId = OBPL.BPLId
			WHERE OBPL.BPLId = @Filial
	)
	, Somatorio AS
	(
		SELECT *,
			VL_01 + VL_02 + VL_03 + VL_04 + VL_05 AS VL_06,
			VL_01 + VL_02 + VL_03 + VL_04 + VL_05 + VL_07 AS VL_08,
			VL_09 + VL_10 + VL_11 + VL_12 AS VL_13
		FROM IPI
	)
	, Saldo AS
	(
		SELECT *,
		CASE WHEN VL_13 >= VL_08 
			THEN  VL_13 - VL_08
			ELSE 0
		END AS VL_16,
		CASE WHEN VL_08 >= VL_13 
			THEN VL_08 - VL_13
			ELSE 0
		END AS VL_17
		FROM Somatorio
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
		VL_16,
		VL_17
	FROM Saldo
)
