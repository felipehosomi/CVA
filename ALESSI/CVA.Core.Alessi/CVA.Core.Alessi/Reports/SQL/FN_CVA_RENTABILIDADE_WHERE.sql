IF EXISTS (SELECT * FROM sys.objects WHERE type = 'FN' AND name = 'FN_CVA_RENTABILIDADE_WHERE')
	DROP FUNCTION FN_CVA_RENTABILIDADE_WHERE
GO
-- =========================================================
-- Autor:			Felipe Hosomi
-- Criação:			25/07/2017
-- Descrição:		Relatório de Rentabilidade - WHERE
-- Versao:			1.0.0.0
-- Data Versao:     25/07/2017
-- =========================================================
CREATE FUNCTION FN_CVA_RENTABILIDADE_WHERE
(
	@DataDe				NVARCHAR(10), -- Formato yyyyMMdd
	@DataAte			NVARCHAR(10), -- Formato yyyyMMdd	
	@GrupoItem			NVARCHAR(MAX) = '',
	@Item				NVARCHAR(MAX) = '',
	@Vendedor			NVARCHAR(MAX) = '',
	@Gerente			NVARCHAR(MAX) = '',
	@Linha				NVARCHAR(MAX) = '',
	@Cidade				NVARCHAR(MAX) = '',
	@Segmento			NVARCHAR(MAX) = '',
	@Acabamento			NVARCHAR(MAX) = '',
	@Classificacao		NVARCHAR(MAX) = '',
	@Disponibilidade	NVARCHAR(MAX) = '',
	@Sistema			NVARCHAR(MAX) = '',
	@Marca				NVARCHAR(MAX) = ''
)
RETURNS VARCHAR(MAX)
BEGIN
	DECLARE @Where NVARCHAR(MAX)

	---------------------------------------
	----------- Montagem Where ------------
	---------------------------------------
	SET @Where = ' AND OINV.DocDate BETWEEN CAST(''' + @DataDe + ''' AS DATETIME) AND CAST(''' + @DataAte + ''' AS DATETIME)'

	IF ISNULL(@GrupoItem, '') <> ''
	BEGIN
		SET @Where += ' AND OITM.ItmsGrpCod IN ( ' + @GrupoItem + ') '
	END
	IF ISNULL(@Item, '') <> ''
	BEGIN
		SET @Where += ' AND OITM.ItemCode IN ( ' + @Item + ') '
	END
	IF ISNULL(@Vendedor, '') <> ''
	BEGIN
		SET @Where += ' AND OINV.SlpCode IN ( ' + @Vendedor + ') '
	END
	IF ISNULL(@Linha, '') <> ''
	BEGIN
		SET @Where += ' AND INV1.LineNum IN ( ' + @Linha + ') '
	END
	IF ISNULL(@Cidade, '') <> ''
	BEGIN
		SET @Where += ' AND INV12.CityS IN ( ' + @Cidade + ') '
	END
	IF ISNULL(@Segmento, '') <> ''
	BEGIN
		SET @Where += ' AND OITM.U_Segmento IN ( ' + @Segmento + ') '
	END
	IF ISNULL(@Acabamento, '') <> ''
	BEGIN
		SET @Where += ' AND OITM.U_Acabamento IN ( ' + @Acabamento + ') '
	END
	IF ISNULL(@Classificacao, '') <> ''
	BEGIN
		SET @Where += ' AND OITM.U_ClassifItem IN ( ' + @Classificacao + ') '
	END
	IF ISNULL(@Disponibilidade, '') <> ''
	BEGIN
		SET @Where += ' AND OITM.U_Disponibilidade IN ( ' + @Disponibilidade + ') '
	END
	IF ISNULL(@Sistema, '') <> ''
	BEGIN
		SET @Where += ' AND OITM.U_Sistema IN ( ' + @Sistema + ') '
	END
	IF ISNULL(@Marca, '') <> ''
	BEGIN
		SET @Where += ' AND OITM.U_Marca IN ( ' + @Marca + ') '
	END

	RETURN @Where
END