IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_SEF_0450')
	DROP PROCEDURE SP_CVA_SEF_0450
GO

-- exec SP_CVA_SEF_0450 1, '2018-01-01', '2018-31-01'
CREATE PROCEDURE SP_CVA_SEF_0450
(
	@Filial			INT,
	@DataDe			DATETIME,
	@DataAte		DATETIME
)
AS
BEGIN

	SELECT 
		'0450'			LIN,
		'1'				COD_INF_OBS,
		u_descricao		TXT


		 FROM [@SKILL_005_APR3]  WHERE U_PERIODO = (Select SUBSTRING((Convert(Char(10), @DataDe,112)),0,5) + '-' + SUBSTRING((Convert(Char(10), @DataDe ,112)),5,2))
	
END