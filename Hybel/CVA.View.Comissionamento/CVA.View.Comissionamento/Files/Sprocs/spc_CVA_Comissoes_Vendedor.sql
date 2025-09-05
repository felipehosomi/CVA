IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spc_CVA_Comissoes_Vendedor')
	DROP PROCEDURE spc_CVA_Comissoes_Vendedor
GO
CREATE procedure spc_CVA_Comissoes_Vendedor
(
	@DataInicial datetime, 
	@DataFinal datetime,
	@Vendedor INT = 0
)
AS
BEGIN
	SET DATEFORMAT 'ymd';
	SET NOCOUNT ON;

	SELECT
		CAST(OBPL.AliasName AS NVARCHAR(MAX)) BPlName,
		COM.U_COMNAME		Comissionado,
		COM.U_META			Meta,
		COM.U_TOTALVENDAS	Vendas,
		COM.U_PORCMETA		PercentualMeta,
		COM.U_COMISSAO		PercentualComissao,
		COM.U_TAXDATE TaxDate,
		CASE WHEN COM.U_VALOR > 0
			THEN COM.U_VALOR
			ELSE 0
		END Recebido,
		CASE WHEN COM.U_VALOR < 0
			THEN COM.U_VALOR
			ELSE 0
		END Devolucoes,
		COM.U_IMPOSTOS Impostos,
		COM.U_VALORCOMISSAO		ComissaoIndividual,
		COM.U_VALORCOMEQUIP		ComissaoEquipe,
		COM.U_VALORCOMISSAO + COM.U_VALORCOMEQUIP Comissao,
		COM.U_DSR DSR,
		OADM.CompnyName,
		OADP.LogoImage
	INTO #tmp_result	
	FROM [@CVA_CALC_COMISSAO] COM
		LEFT JOIN OBPL WITH(NOLOCK) ON OBPL.BPLId = COM.U_BPLID
		LEFT JOIN OADM WITH(NOLOCK) ON 1 = 1
		LEFT JOIN OADP WITH(NOLOCK) ON 1 = 1
	WHERE COM.U_TAXDATE BETWEEN @DataInicial AND @DataFinal
	AND (COM.U_COMISSIONADO = @Vendedor OR ISNULL(@Vendedor, 0) = 0)

	SELECT
		Comissionado,
		BPLName	Filial,
		CAST(MONTH(TaxDate) AS NVARCHAR) + '/' + CAST(YEAR(TaxDate) AS NVARCHAR) Periodo,
		SUM(Recebido)		Recebido,
		SUM(Devolucoes)		Devolucoes,
		SUM(Recebido) + SUM(Devolucoes)		Liquido,
		Meta,
		Vendas,
		PercentualMeta,
		PercentualComissao,
		SUM(Impostos)		Impostos,
		SUM(ComissaoIndividual) ComissaoIndividual,
		SUM(ComissaoEquipe) ComissaoEquipe,
		SUM(Comissao)		Comissao,
		SUM(DSR)			DSR,
		SUM(Comissao) + SUM(DSR) Total,
		MAX(CompnyName) Empresa,
		(SELECT LogoImage FROM OADP) Logo
	FROM #tmp_result
	GROUP BY
		Vendas,
		BPLName,
		Comissionado,
		Meta,
		PercentualMeta,
		PercentualComissao,
		MONTH(TaxDate),
		YEAR(TaxDate)

	DROP TABLE #tmp_result
END
