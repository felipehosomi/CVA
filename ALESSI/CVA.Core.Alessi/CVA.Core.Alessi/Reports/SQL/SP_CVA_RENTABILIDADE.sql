IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_RENTABILIDADE')
	DROP PROCEDURE SP_CVA_RENTABILIDADE
GO
-- =========================================================
-- Autor:			Felipe Hosomi
-- Criação:			25/07/2017
-- Descrição:		Relatório de Rentabilidade - Select tabela INV
-- Versao:			1.0.0.0
-- Data Versao:     25/07/2017
-- =========================================================
CREATE PROCEDURE SP_CVA_RENTABILIDADE
(
	@DataDe				NVARCHAR(MAX), -- Formato yyyyMMdd
	@DataAte			NVARCHAR(MAX), -- Formato yyyyMMdd	
	@Agrupamento		NVARCHAR(MAX) = 'Item',
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
	@ExibePorItem		INT = 0
)
AS
BEGIN
	DECLARE @Sql NVARCHAR(MAX)
	DECLARE @Total DECIMAL(19, 6)
	DECLARE @TotalTable TABLE (Total DECIMAL(19, 6))

	DECLARE @BuscaDados TABLE
	(
		ItemCode				NVARCHAR(MAX),
		Item					NVARCHAR(MAX),	
		Vendedor				NVARCHAR(MAX),
		Gerente					NVARCHAR(MAX),
		Cliente					NVARCHAR(MAX),
		Linha					INT,
		Cidade					NVARCHAR(MAX),
		GrupoItem				NVARCHAR(MAX),
		Segmento				NVARCHAR(MAX),
		Acabamento				NVARCHAR(MAX),
		Classificacao			NVARCHAR(MAX),
		Disponibilidade			NVARCHAR(MAX),
		Sistema					NVARCHAR(MAX),
		Marca					NVARCHAR(MAX),
		Preco					NUMERIC(19, 6),
		Quantidade				NUMERIC(19, 6),
		Peso					NUMERIC(19, 6),
		PrazoMedio				INT,
		MediaLitros				NUMERIC(19, 6),
		VendasLitros			NUMERIC(19, 6),
		ValorProduto			NUMERIC(19, 6),
		PrctCustoVariavel		NUMERIC(19, 6),
		CustoDia				NUMERIC(19, 6),
		U_CVA_CustoFixo			NUMERIC(19, 6),
		ValorFrete				NUMERIC(19, 6),
		U_CVA_CMV				NUMERIC(19, 6)
	)

	CREATE TABLE #Rentabilidade 
	(
		ItemCode				NVARCHAR(MAX),
		Item					NVARCHAR(MAX),
		Vendedor				NVARCHAR(MAX),
		Gerente					NVARCHAR(MAX),
		Cliente					NVARCHAR(MAX),
		Linha					INT,
		Cidade					NVARCHAR(MAX),
		GrupoItem				NVARCHAR(MAX),
		Segmento				NVARCHAR(MAX),
		Acabamento				NVARCHAR(MAX),
		Classificacao			NVARCHAR(MAX),
		Disponibilidade			NVARCHAR(MAX),
		Sistema					NVARCHAR(MAX),
		Marca					NVARCHAR(MAX),
		Preco					NUMERIC(19, 6),
		Quantidade				NUMERIC(19, 6),
		Peso					NUMERIC(19, 6),
		PrazoMedio				INT,
		MediaLitros				NUMERIC(19, 6),
		VendasLitros			NUMERIC(19, 6),
		ValorProduto			NUMERIC(19, 6),
		PrctCustoVariavel		NUMERIC(19, 6),
		CustoDia				NUMERIC(19, 6),
		U_CVA_CustoFixo			NUMERIC(19, 6),
		ValorFrete				NUMERIC(19, 6),
		U_CVA_CMV				NUMERIC(19, 6),
		VendaLiquida			NUMERIC(19, 6),
		CMV						NUMERIC(19, 6),
		Contribuicao			NUMERIC(19, 6),
		PrctContribuicao		NUMERIC(19, 6),
		CustoFixo				NUMERIC(19, 6),
		PrctCustoFixo			NUMERIC(19, 6),
		RentabilidadeLiq		NUMERIC(19, 6),
		PrctRentabilidadeLiq	NUMERIC(19, 6)
	)

	INSERT INTO @TotalTable
	EXEC SP_CVA_RENTABILIDADE_TOTAL @DataDe, @DataAte, @GrupoItem, @Item, @Vendedor, @Gerente, @Linha, @Cidade, @Segmento, @Acabamento, @Classificacao, @Disponibilidade, @Sistema, @Marca

	SELECT @Total = Total FROM @TotalTable

	SET @Sql = [dbo].FN_CVA_RENTABILIDADE_SQL(@DataDe, @DataAte, @GrupoItem, @Item, @Vendedor, @Gerente, @Linha, @Cidade, @Segmento, @Acabamento, @Classificacao, @Disponibilidade, @Sistema, @Marca, @CMV)

	--PRINT @Sql

	INSERT @BuscaDados
	EXEC (@Sql)

	; WITH VendaLiquida AS
	(
		SELECT Dados.*,
			Dados.ValorProduto - (Dados.ValorProduto * Dados.PrctCustoVariavel) - Dados.CustoDia - Dados.ValorFrete [Venda Líquida],
			CASE WHEN ISNULL(ITM1.PriceList, 0) = 0
				THEN Dados.U_CVA_CMV * Dados.Quantidade
				ELSE ITM1.Price * Dados.Quantidade
			END	[CMV]
		FROM @BuscaDados Dados
			LEFT JOIN ITM1
				ON ITM1.ItemCode = Dados.ItemCode
				AND ITM1.PriceList = @CMV
		WHERE Dados.ValorProduto > 0
	)
	, CustoFixo AS
	(
		SELECT VendaLiquida.*,
			VendaLiquida.[Venda Líquida] - VendaLiquida.CMV									[Contribuição],
			(VendaLiquida.[Venda Líquida] - VendaLiquida.CMV) / VendaLiquida.ValorProduto	[% Contribuição],
			(VendaLiquida.U_CVA_CustoFixo / 100) * VendaLiquida.ValorProduto				[Custo Fixo],
			VendaLiquida.U_CVA_CustoFixo													[% Custo Fixo]
		FROM VendaLiquida
	)
	, Rentabilidade AS
	(
		SELECT CustoFixo.*,
			CustoFixo.ValorProduto - (CustoFixo.ValorProduto * CustoFixo.PrctCustoVariavel) - 
			CustoFixo.CustoDia - CustoFixo.ValorFrete - CustoFixo.CMV - CustoFixo.[Custo Fixo]			[Rentabilidade Líquida],
			(CustoFixo.ValorProduto - (CustoFixo.ValorProduto * CustoFixo.PrctCustoVariavel) - 
			CustoFixo.CustoDia - CustoFixo.ValorFrete - CustoFixo.CMV - CustoFixo.[Custo Fixo]) / 
			CustoFixo.ValorProduto																		[% Rentabilidade Líquida]
		FROM CustoFixo
	)
	INSERT INTO #Rentabilidade
	SELECT * FROM Rentabilidade

	--SELECT * FROM #Rentabilidade
	--ORDER BY Item

	IF @ExibePorItem <> 0
	BEGIN
		SELECT
			Item,
			SUM(Peso)							VendaQuilos,
			AVG(PrazoMedio)						PrazoMedio,
			AVG(MediaLitros)					MediaLitros,
			SUM(ValorProduto) / @Total * 100	[% Rep],
			SUM(VendasLitros)					VendasLitros,
			SUM(ValorFrete)						ValorFrete,
			SUM(ValorProduto)					ValorProdutos,
			SUM(ValorProduto) * 
			AVG(PrctCustoVariavel)				CustoVariavel,
			AVG(PrctCustoVariavel) * 100		[% CV],
			SUM(CustoDia)						CustoDia,
			SUM(ValorFrete)						ValorFrete,
			SUM(VendaLiquida)					VendaLiquida,
			SUM(CMV)							CMV,
			SUM(Contribuicao)					Contribuicao,
			AVG(PrctContribuicao) * 100			[% Contribuicao],
			SUM(CustoFixo)						CustoFixo,
			SUM(RentabilidadeLiq)				RentabilidadeLiq,
			SUM(PrctRentabilidadeLiq) * 100		PrctRentabilidadeLiq
		FROM #Rentabilidade
		GROUP BY Item
		ORDER BY Item
	END

	SET @Sql =
	'SELECT ' + @Agrupamento  + ',
		SUM(Peso)						VendaQuilos,
		AVG(PrazoMedio)					PrazoMedio,
		AVG(MediaLitros)				MediaLitros,
		SUM(ValorProduto) / ' + CAST(@Total AS NVARCHAR(MAX)) 
		+ ' * 100 [% Rep],
		SUM(VendasLitros)				VendasLitros,
		SUM(ValorFrete)					ValorFrete,
		SUM(ValorProduto)				ValorProdutos,
		SUM(ValorProduto) * 
		AVG(PrctCustoVariavel)			CustoVariavel,
		AVG(PrctCustoVariavel)	* 100	[% CV],
		SUM(CustoDia)					CustoDia,
		SUM(VendaLiquida)				VendaLiquida,
		SUM(CMV)						CMV,
		SUM(Contribuicao)				Contribuicao,
		AVG(PrctContribuicao) * 100		[% Contribuicao],
		SUM(CustoFixo)					CustoFixo,
		AVG(PrctCustoFixo)				[% CustoFixo],
		SUM(RentabilidadeLiq)			RentabilidadeLiq,
		SUM(PrctRentabilidadeLiq) * 100	PrctRentabilidadeLiq
	FROM #Rentabilidade
	GROUP BY ' + @Agrupamento +
	' ORDER BY ' + @Agrupamento
	
	EXEC(@Sql)

	DROP TABLE #Rentabilidade
END