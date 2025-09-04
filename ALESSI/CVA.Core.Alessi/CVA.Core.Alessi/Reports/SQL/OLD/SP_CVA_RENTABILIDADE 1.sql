IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_RENTABILIDADE')
	DROP PROCEDURE SP_CVA_RENTABILIDADE
GO
-- =========================================================
-- Autor:			Felipe Hosomi
-- Criação:			25/07/2017
-- Descrição:		Relatório de Rentabilidade
-- Versao:			1.0.0.0r
-- Data Versao:     25/07/2017
-- =========================================================
CREATE PROCEDURE SP_CVA_RENTABILIDADE
(
	@DataDe				NVARCHAR(MAX), -- Formato yyyyMMdd
	@DataAte			NVARCHAR(MAX), -- Formato yyyyMMdd	
	@Agrupamento		NVARCHAR(MAX) = 'ItemCode',
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
	@Marca				NVARCHAR(MAX) = '',
	@CMV				INT = -1,
	@SomenteAgrupado	INT = 1
)
AS
BEGIN
	DECLARE @Sql NVARCHAR(MAX)
	DECLARE @Total DECIMAL(19, 6)
	DECLARE @TotalTable TABLE (Total DECIMAL(19, 6))

	DECLARE @BuscaDados TABLE
	(
		ItemCode				NVARCHAR(MAX),	
		SlpCode					INT,
		SlpName					NVARCHAR(MAX),
		ManagerCode				NVARCHAR(MAX),
		ManagerName				NVARCHAR(MAX),
		CardCode				NVARCHAR(MAX),
		CardName				NVARCHAR(MAX),
		LineNum					INT,
		City					NVARCHAR(MAX),
		ItemGroup				NVARCHAR(MAX),
		Segmento				NVARCHAR(MAX),
		Acabamento				NVARCHAR(MAX),
		Classificacao			NVARCHAR(MAX),
		Disponibilidade			NVARCHAR(MAX),
		Sistema					NVARCHAR(MAX),
		Marca					NVARCHAR(MAX),
		Price					NUMERIC(19, 6),
		Peso					NUMERIC(19, 6),
		PrazoMedio				INT,
		MediaLitros				NUMERIC(19, 6),
		VendasLitros			NUMERIC(19, 6),
		ValorProduto			NUMERIC(19, 6),
		CustoVariavel			NUMERIC(19, 6),
		PrctCustoVariavel		NUMERIC(19, 6),
		CustoDia				NUMERIC(19, 6),
		U_CVA_CustoFixo			NUMERIC(19, 6),
		ValorFrete				NUMERIC(19, 6)
	)

	DECLARE @Rentabilidade TABLE 
	(
		ItemCode				NVARCHAR(MAX),
		SlpCode					INT,
		SlpName					NVARCHAR(MAX),
		ManagerCode				NVARCHAR(MAX),
		ManagerName				NVARCHAR(MAX),
		CardCode				NVARCHAR(MAX),
		CardName				NVARCHAR(MAX),
		LineNum					INT,
		City					NVARCHAR(MAX),
		ItemGroup				NVARCHAR(MAX),
		Segmento				NVARCHAR(MAX),
		Acabamento				NVARCHAR(MAX),
		Classificacao			NVARCHAR(MAX),
		Disponibilidade			NVARCHAR(MAX),
		Sistema					NVARCHAR(MAX),
		Marca					NVARCHAR(MAX),
		Price					NUMERIC(19, 6),
		Peso					NUMERIC(19, 6),
		PrazoMedio				INT,
		MediaLitros				NUMERIC(19, 6),
		VendasLitros			NUMERIC(19, 6),
		ValorProduto			NUMERIC(19, 6),
		CustoVariavel			NUMERIC(19, 6),
		PrctCustoVariavel		NUMERIC(19, 6),
		CustoDia				NUMERIC(19, 6),
		U_CVA_CustoFixo			NUMERIC(19, 6),
		ValorFrete				NUMERIC(19, 6),
		VendaLiquida			NUMERIC(19, 6),
		CMV						NUMERIC(19, 6),
		Contribuicao			NUMERIC(19, 6),
		PrctContribuicao		NUMERIC(19, 6),
		CustoFixo				NUMERIC(19, 6),
		RentabilidadeLiq		NUMERIC(19, 6),
		PrctRentabilidadeLiq	NUMERIC(19, 6)
	)

	INSERT INTO @TotalTable
	EXEC SP_RENTABILIDADE_TOTAL @DataDe, @DataAte, @Item, @GrupoItem, @Vendedor, @Gerente, @Linha, @Cidade, @Segmento, @Acabamento, @Classificacao, @Disponibilidade, @Sistema, @Marca

	SELECT @Total = Total FROM @TotalTable

	SET @Sql = [dbo].FN_RENTABILIDADE_INV(@DataDe, @DataAte, @Item, @GrupoItem, @Vendedor, @Gerente, @Linha, @Cidade, @Segmento, @Acabamento, @Classificacao, @Disponibilidade, @Sistema, @Marca, @CMV)

	PRINT @Sql

	INSERT @BuscaDados
	EXEC (@Sql)

	; WITH VendaLiquida AS
	(
		SELECT Dados.*,
			Dados.ValorProduto - Dados.CustoVariavel - Dados.CustoDia - Dados.ValorFrete [Venda Líquida],
			CASE WHEN ISNULL(ITM1.PriceList, 0) = 0
				THEN Dados.Price
				ELSE ITM1.Price
			END	[CMV]
		FROM @BuscaDados Dados
			LEFT JOIN ITM1
				ON ITM1.ItemCode = Dados.ItemCode
				AND ITM1.PriceList = @CMV
	)
	, Rentabilidade AS
	(
		SELECT VendaLiquida.*,
			VendaLiquida.[Venda Líquida] - VendaLiquida.CMV								[Contribuição],
			VendaLiquida.[Venda Líquida] - VendaLiquida.CMV / VendaLiquida.ValorProduto	[% Contribuição],
			(VendaLiquida.U_CVA_CustoFixo / 100) * VendaLiquida.ValorProduto			[Custo Fixo],
			VendaLiquida.ValorFrete - VendaLiquida.CustoVariavel - VendaLiquida.CustoDia - VendaLiquida.ValorFrete - VendaLiquida.CMV
			- (VendaLiquida.U_CVA_CustoFixo / 100) * VendaLiquida.ValorProduto			[Rentabilidade Líquida],
			(VendaLiquida.ValorProduto - VendaLiquida.CustoVariavel - VendaLiquida.CustoDia - VendaLiquida.ValorFrete - VendaLiquida.CMV
			- (VendaLiquida.U_CVA_CustoFixo / 100) * VendaLiquida.ValorProduto) / VendaLiquida.ValorProduto	[% Rentabilidade Líquida]
		FROM VendaLiquida
	)
	INSERT INTO @Rentabilidade
	SELECT * FROM Rentabilidade

	IF @SomenteAgrupado <> 1
	BEGIN
		SELECT
			ItemCode,
			SUM(Peso)					VendaQuilos,
			AVG(PrazoMedio)				PrazoMedio,
			AVG(MediaLitros)			MediaLitros,
			SUM(ValorProduto) / @Total	[% Rep],
			SUM(VendasLitros)			VendasLitros,
			SUM(ValorFrete)				ValorFrete,
			SUM(CustoVariavel)			CustoVariavel,
			SUM(PrctCustoVariavel)		[% CV],
			SUM(CustoDia)				CustoDia,
			SUM(ValorFrete)				ValorFrete,
			SUM(VendaLiquida)			VendaLiquida,
			AVG(CMV)					CMV,
			SUM(Contribuicao)			Contribuicao,
			SUM(PrctContribuicao)		[% Contribuicao],
			SUM(CustoFixo)				CustoFixo,
			SUM(RentabilidadeLiq)		RentabilidadeLiq,
			SUM(PrctRentabilidadeLiq)	PrctRentabilidadeLiq
		FROM @Rentabilidade
		GROUP BY ItemCode
	END

	SET @Sql =
	'SELECT ' + @Agrupamento  + ',
		SUM(Peso)					VendaQuilos,
		AVG(PrazoMedio)				PrazoMedio,
		AVG(MediaLitros)			MediaLitros,
		SUM(ValorProduto) / ' + @Total + '
		[% Rep],
		SUM(VendasLitros)			VendasLitros,
		SUM(ValorFrete)				ValorFrete,
		SUM(CustoVariavel)			CustoVariavel,
		SUM(PrctCustoVariavel)		[% CV],
		SUM(CustoDia)				CustoDia,
		SUM(ValorFrete)				ValorFrete,
		SUM(VendaLiquida)			VendaLiquida,
		AVG(CMV)					CMV,
		SUM(Contribuicao)			Contribuicao,
		SUM(PrctContribuicao)		[% Contribuicao],
		SUM(CustoFixo)				CustoFixo,
		SUM(RentabilidadeLiq)		RentabilidadeLiq,
		SUM(PrctRentabilidadeLiq)	PrctRentabilidadeLiq
	FROM @Rentabilidade
	GROUP BY ' + @Agrupamento

	EXEC(@Sql)
END