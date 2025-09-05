IF EXISTS (SELECT * FROM sys.objects WHERE type = 'IF' AND name = 'FN_CVA_SEF_0450')
	DROP FUNCTION FN_CVA_SEF_0450
GO
CREATE FUNCTION FN_CVA_SEF_0450
(
	@Filial			INT,
	@DataInicial	DATETIME,
	@DataFinal		DATETIME
) RETURNS TABLE
AS
RETURN
(
	SELECT 
		'1'				COD_INF_OBS,
		u_descricao		TXT
		FROM [@SKILL_005_APR3]  WHERE U_PERIODO = (Select SUBSTRING((Convert(Char(10), @DataInicial,112)),0,5) + '-' + SUBSTRING((Convert(Char(10), @DataInicial ,112)),5,2))
)
