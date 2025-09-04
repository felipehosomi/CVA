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
	@DataDe				NVARCHAR(10), -- Formato yyyyMMdd
	@DataAte			NVARCHAR(10), -- Formato yyyyMMdd	
	@Agrupamento		NVARCHAR(MAX) = 'OITM.ItemCode',
	@GrupoItem			NVARCHAR(MAX) = '',
	@Item				NVARCHAR(MAX) = '',
	@Vendedor			NVARCHAR(MAX) = '',
	@Gerente			NVARCHAR(MAX) = '',
	@Linha				NVARCHAR(MAX) = '',
	@Cidade				NVARCHAR(MAX) = '',
	@CMV				INT = -1,
	@SomenteAgrupado	INT = 1
)
AS
BEGIN
	DECLARE @Sql NVARCHAR(MAX)
	DECLARE @Where NVARCHAR(MAX)
	
	---------------------------------------
	----------- Filtro Gerente ------------
	---------------------------------------
	DECLARE @JoinGerente NVARCHAR(MAX)
	SET @JoinGerente = ''
	IF ISNULL(@Gerente, '') <> ''
	BEGIN
		SET @JoinGerente = 
		'INNER JOIN HTM1 GERENTE WITH(NOLOCK)
			ON GERENTE.empID IN (' + @Gerente + ')
		INNER JOIN HTM1 [TIME] WITH(NOLOCK)
			ON TIME.teamID = GERENTE.teamID
			AND OINV.OwnerCode = [TIME].empID'
	END

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

	--PRINT @Where

	SET @Sql = 
	'DECLARE @Total NUMERIC(19, 6)
	
	SELECT @Total = SUM(INV1.LineTotal)
	FROM OINV WITH(NOLOCK) 
		INNER JOIN INV1 WITH(NOLOCK)
			ON INV1.DocEntry = OINV.DocEntry
		INNER JOIN OITM WITH(NOLOCK)
			ON OITM.ItemCode = INV1.ItemCode
		INNER JOIN INV12 WITH(NOLOCK)
			ON INV12.DocEntry = OINV.DocEntry ' + 
		@JoinGerente + '
	WHERE OINV.CANCELED = ''N'' AND OINV.DocTotal > 0
	-- AND INV1.Usage = 9' +
	@Where + '

	;WITH Frete AS
	(
		SELECT
			INV1.ItemCode,
			INV1.Price,
			INV1.Weight1									[Peso],
			INV6.[Prazo Medio],
			INV1.LineTotal / INV1.Quantity					[Méd. Litros],
			INV1.LineTotal / ISNULL(@Total, 1)				[% Rep],
			INV1.Weight1 / ISNULL(INV1.U_CVA_Density, 1)	[Vendas Litros],
			INV1.Price * INV1.Quantity						[Valor Produto],
			INV1.Price * ISNULL(INV1.U_CVA_CustoVar, 1)		[Custo Variável],
			ISNULL(INV1.U_CVA_CustoVar, 0)					[% Custo Variável],
			ISNULL(INV1.U_CVA_CustoDia, 0)					[Custo Dia],
			ISNULL(INV1.U_CVA_CustoFixo, 0)					[U_CVA_CustoFixo],
			CASE WHEN ISNULL(OPCH.DocEntry, 0) = 0
				THEN ISNULL(INV1.U_akbfreight, 0)
				ELSE (INV1.Weight1 / ISNULL(NULLIF(OINV.Weight, 0), 1)) * OPCH.DocTotal
			END												[Valor Frete]
		FROM OINV WITH(NOLOCK) 
			INNER JOIN INV1 WITH(NOLOCK)
				ON INV1.DocEntry = OINV.DocEntry
			INNER JOIN OITM WITH(NOLOCK)
				ON OITM.ItemCode = INV1.ItemCode
			INNER JOIN (SELECT INV6.DocEntry, AVG(DATEDIFF(DAY, OINV.DocDate, INV6.DueDate)) [Prazo Medio]
							FROM INV6 WITH(NOLOCK)
								INNER JOIN OINV WITH(NOLOCK)
									ON OINV.DocEntry = INV6.DocEntry
							GROUP BY INV6.DocEntry
						) INV6
				ON INV6.DocEntry = OINV.DocEntry
			INNER JOIN INV12 WITH(NOLOCK)
				ON INV12.DocEntry = OINV.DocEntry
			LEFT JOIN [@CVA_NFE_FRETE_ITEM] FRETE_ITEM WITH(NOLOCK)
				ON FRETE_ITEM.U_DocNum = OINV.DocNum
			LEFT JOIN [@CVA_NFE_FRETE] FRETE WITH(NOLOCK)
				ON FRETE.Code = FRETE_ITEM.Code
			LEFT JOIN OPCH WITH(NOLOCK)
				ON OPCH.DocEntry = FRETE.U_DocEntry ' +
			@JoinGerente + '
		WHERE OINV.CANCELED = ''N'' AND OINV.DocTotal > 0
		-- AND INV1.Usage = 9' +
		@Where + '
	)
	,CMV AS
	(
		SELECT Frete.*,
			Frete.[Valor Produto] - Frete.[Custo Variável] - Frete.[Custo Dia] - Frete.[Valor Frete] [Venda Líquida],
			CASE WHEN ISNULL(ITM1.PriceList, 0) = 0
				THEN Frete.Price
				ELSE ITM1.Price
			END	[CMV]
		FROM Frete
			LEFT JOIN ITM1
				ON ITM1.ItemCode = Frete.ItemCode
				AND ITM1.PriceList = ' + CAST(@CMV AS NVARCHAR(10)) + '
	)
	, Rentabilidade AS
	(
		SELECT CMV.*,
			CMV.[Venda Líquida] - CMV.CMV	[Contribuição],
			CMV.[Venda Líquida] - CMV.CMV / CMV.[Valor Produto]	[% Contribuição],
			(CMV.U_CVA_CustoFixo / 100) * CMV.[Valor Produto]		[Custo Fixo],
			CMV.[Valor Produto] - CMV.[Custo Variável] - CMV.[Custo Dia] - CMV.[Valor Frete] - CMV.CMV
			- (CMV.U_CVA_CustoFixo / 100) * CMV.[Valor Produto]		[Rentabilidade Líquida],
			(CMV.[Valor Produto] - CMV.[Custo Variável] - CMV.[Custo Dia] - CMV.[Valor Frete] - CMV.CMV
			- (CMV.U_CVA_CustoFixo / 100) * CMV.[Valor Produto]) / CMV.[Valor Produto]	[% Rentabilidade Líquida]
		FROM CMV
	)

	SELECT *
	INTO #Rentabilidade
	FROM Rentabilidade
	
	SELECT * FROM #Rentabilidade

	DROP TABLE #Rentabilidade'

	--PRINT @Sql

	EXEC(@Sql)
END