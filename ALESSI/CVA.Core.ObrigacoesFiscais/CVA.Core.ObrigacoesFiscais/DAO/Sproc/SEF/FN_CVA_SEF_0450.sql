IF EXISTS (SELECT * FROM sys.objects WHERE type = 'IF' AND name = 'FN_CVA_SEF_0450')
	DROP FUNCTION FN_CVA_SEF_0450
GO

--- select * from FN_CVA_SEF_0450 (9,'2018-01-01','2018-01-31')
CREATE FUNCTION FN_CVA_SEF_0450
(
	@Filial			INT,
	@DataInicial	DATETIME,
	@DataFinal		DATETIME
) RETURNS TABLE
AS
RETURN
(



	SELECT top 1
		'1'				COD_INF_OBS,
		u_obsICMS		TXT
		FROM [@SKILL_005_APR2]   
		WHERE U_PERIODO = (Select SUBSTRING((Convert(Char(10), @DataInicial,112)),0,5) + '-' + SUBSTRING((Convert(Char(10), @DataInicial ,112)),5,2))
		AND U_BPLID = @Filial



)



