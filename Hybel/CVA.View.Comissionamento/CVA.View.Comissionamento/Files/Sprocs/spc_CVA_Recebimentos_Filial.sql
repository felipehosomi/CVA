IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spc_CVA_Recebimentos_Filial')
	DROP PROCEDURE spc_CVA_Recebimentos_Filial
GO
CREATE procedure spc_CVA_Recebimentos_Filial
(
	@DataInicial datetime, 
	@DataFinal datetime
)
as
BEGIN
	SET DATEFORMAT 'ymd';
	SET NOCOUNT ON;

	SELECT
		CAST(OBPL.AliasName AS NVARCHAR(MAX)) BPlName,
		COM.U_TAXDATE TaxDate,
		CASE WHEN COM.U_VALOR > 0
			THEN COM.U_VALOR
			ELSE 0
		END Recebido,
		CASE WHEN COM.U_VALOR < 0  AND U_INDEQUIPE = '0'
			THEN COM.U_VALOR
			ELSE 0
		END Devolucoes
	INTO #tmp_result	
	FROM [@CVA_CALC_COMISSAO] COM
		INNER JOIN OBPL ON OBPL.BPLId = COM.U_BPLID
	WHERE COM.U_TAXDATE BETWEEN @DataInicial AND @DataFinal

	SELECT
		BPLName	Filial,
		CAST(MONTH(TaxDate) AS NVARCHAR) + '/' + CAST(YEAR(TaxDate) AS NVARCHAR) Periodo,
		SUM(Recebido)		Recebido,
		SUM(Devolucoes)		Devolucoes,
		SUM(Recebido) - SUM(Devolucoes) Liquido
	FROM #tmp_result
	GROUP BY
		BPLName,
		MONTH(TaxDate),
		YEAR(TaxDate)

	DROP TABLE #tmp_result
END
