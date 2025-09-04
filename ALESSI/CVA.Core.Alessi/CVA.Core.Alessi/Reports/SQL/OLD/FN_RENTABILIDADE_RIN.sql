IF EXISTS (SELECT * FROM sys.objects WHERE type = 'FN' AND name = 'FN_RENTABILIDADE_RIN')
	DROP FUNCTION FN_RENTABILIDADE_RIN
GO
-- =========================================================
-- Autor:			Felipe Hosomi
-- Criação:			25/07/2017
-- Descrição:		Relatório de Rentabilidade - Select tabela RIN
-- Versao:			1.0.0.0
-- Data Versao:     25/07/2017
-- =========================================================
CREATE FUNCTION FN_RENTABILIDADE_RIN
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
	@Marca				NVARCHAR(MAX) = '',
	@CMV				INT = -1
)
RETURNS VARCHAR(MAX)
BEGIN
	DECLARE @Sql NVARCHAR(MAX)
	DECLARE @Where NVARCHAR(MAX)

	---------------------------------------
	----------- Montagem Where ------------
	---------------------------------------
	SET @Where = ' AND ORIN.DocDate BETWEEN CAST(''' + @DataDe + ''' AS DATETIME) AND CAST(''' + @DataAte + ''' AS DATETIME)'

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
		SET @Where += ' AND ORIN.SlpCode IN ( ' + @Vendedor + ') '
	END
	IF ISNULL(@Linha, '') <> ''
	BEGIN
		SET @Where += ' AND RIN1.LineNum IN ( ' + @Linha + ') '
	END
	IF ISNULL(@Cidade, '') <> ''
	BEGIN
		SET @Where += ' AND RIN12.CityS IN ( ' + @Cidade + ') '
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
	IF ISNULL(@Gerente, '') <> ''
	BEGIN
		SET @Where += ' AND GERENTE.empID IN ( ' + @Gerente + ') '
	END

	SET @Sql = 
	'DECLARE @Total NUMERIC(19, 6)
	
	SELECT @Total = SUM(RIN1.LineTotal)
	FROM ORIN WITH(NOLOCK) 
		INNER JOIN RIN1 WITH(NOLOCK)
			ON RIN1.DocEntry = ORIN.DocEntry
		INNER JOIN OITM WITH(NOLOCK)
			ON OITM.ItemCode = RIN1.ItemCode
		INNER JOIN RIN12 WITH(NOLOCK)
			ON RIN12.DocEntry = ORIN.DocEntry 
		INNER JOIN HTM1 [TIME] WITH(NOLOCK)
				ON [TIME].empID = ORIN.OwnerCode
		INNER JOIN HTM1 GERENTE WITH(NOLOCK)
			ON GERENTE.teamID = [TIME].teamID
		INNER JOIN OHEM WITH(NOLOCK)
			ON OHEM.empID = GERENTE.empID
		INNER JOIN HEM6 WITH(NOLOCK)
			ON HEM6.empID = GERENTE.empID
		INNER JOIN OHTY WITH(NOLOCK)
			ON OHTY.typeID = HEM6.roleID
			AND CAST(OHTY.descriptio AS NVARCHAR(MAX)) = ''Gerente''
	WHERE ORIN.CANCELED = ''N'' AND ORIN.DocTotal > 0 ' +
	@Where + '
	-- AND RIN1.Usage = 9' + '
	

	;WITH Frete AS
	(
		SELECT
			RIN1.ItemCode,
			OSLP.SlpCode,
			OSLP.SlpName,
			OHEM.empID	ManagerCode,
			OHEM.firstName + '' '' + OHEM.lastName			AS ManagerName,
			ORIN.CardCode,
			ORIN.CardName,
			RIN1.LineNum,
			RIN12.CityS,
			OITM.ItmsGrpCod,
			OITM.U_Segmento,
			OITM.U_Acabamento,
			OITM.U_ClassifItem,
			OITM.U_Disponibilidade,
			OITM.U_Sistema,
			OITM.U_Marca,
			RIN1.Price,
			RIN1.Weight1 * (-1)									[Peso],
			RIN6.[Prazo Medio],
			RIN1.LineTotal / RIN1.Quantity * (-1)				[Méd. Litros],
			RIN1.LineTotal / ISNULL(@Total, 1) * 100 * (-1)		[% Rep],
			RIN1.Weight1 / ISNULL(RIN1.U_CVA_Density, 1) * (-1)	[Vendas Litros],
			RIN1.Price * RIN1.Quantity	* (-1)					[Valor Produto],
			RIN1.Price * ISNULL(RIN1.U_CVA_CustoVar, 1)	* (-1)	[Custo Variável],
			ISNULL(RIN1.U_CVA_CustoVar, 0)	* (-1)				[% Custo Variável],
			ISNULL(RIN1.U_CVA_CustoDia, 0)	* (-1)				[Custo Dia],
			ISNULL(RIN1.U_CVA_CustoFixo, 0)	* (-1)				[U_CVA_CustoFixo],
			CASE WHEN ISNULL(OPCH.DocEntry, 0) = 0
				THEN ISNULL(RIN1.U_akbfreight, 0) * (-1)
				ELSE (RIN1.Weight1 / ISNULL(NULLIF(ORIN.Weight, 0), 1)) * OPCH.DocTotal * (-1)
			END												[Valor Frete]
		FROM ORIN WITH(NOLOCK) 
			INNER JOIN RIN1 WITH(NOLOCK)
				ON RIN1.DocEntry = ORIN.DocEntry
			INNER JOIN OITM WITH(NOLOCK)
				ON OITM.ItemCode = RIN1.ItemCode
			INNER JOIN (SELECT RIN6.DocEntry, AVG(DATEDIFF(DAY, ORIN.DocDate, RIN6.DueDate)) [Prazo Medio]
							FROM RIN6 WITH(NOLOCK)
								INNER JOIN ORIN WITH(NOLOCK)
									ON ORIN.DocEntry = RIN6.DocEntry
							GROUP BY RIN6.DocEntry
						) RIN6
				ON RIN6.DocEntry = ORIN.DocEntry
			INNER JOIN RIN12 WITH(NOLOCK)
				ON RIN12.DocEntry = ORIN.DocEntry
			INNER JOIN OSLP WITH(NOLOCK)
				ON OSLP.SlpCode = ORIN.SlpCode
			INNER JOIN HTM1 [TIME] WITH(NOLOCK)
				ON [TIME].empID = ORIN.OwnerCode
			INNER JOIN HTM1 GERENTE WITH(NOLOCK)
				ON GERENTE.teamID = [TIME].teamID
			INNER JOIN OHEM WITH(NOLOCK)
				ON OHEM.empID = GERENTE.empID
			INNER JOIN HEM6 WITH(NOLOCK)
				ON HEM6.empID = GERENTE.empID
			INNER JOIN OHTY WITH(NOLOCK)
				ON OHTY.typeID = HEM6.roleID
				AND CAST(OHTY.descriptio AS NVARCHAR(MAX)) = ''Gerente''
			LEFT JOIN [@CVA_NFE_FRETE_ITEM] FRETE_ITEM WITH(NOLOCK)
				ON FRETE_ITEM.U_DocNum = ORIN.DocNum
			LEFT JOIN [@CVA_NFE_FRETE] FRETE WITH(NOLOCK)
				ON FRETE.Code = FRETE_ITEM.Code
			LEFT JOIN OPCH WITH(NOLOCK)
				ON OPCH.DocEntry = FRETE.U_DocEntry 
		WHERE ORIN.CANCELED = ''N'' AND ORIN.DocTotal > 0 ' +
		@Where + '
		-- AND RIN1.Usage = 9' + '
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
	SELECT * FROM Rentabilidade'

	RETURN @Sql
END