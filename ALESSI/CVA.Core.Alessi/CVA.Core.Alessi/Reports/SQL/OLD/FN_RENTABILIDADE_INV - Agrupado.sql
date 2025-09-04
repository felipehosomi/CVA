IF EXISTS (SELECT * FROM sys.objects WHERE type = 'FN' AND name = 'FN_RENTABILIDADE_INV')
	DROP FUNCTION FN_RENTABILIDADE_INV
GO
-- =========================================================
-- Autor:			Felipe Hosomi
-- Criação:			25/07/2017
-- Descrição:		Relatório de Rentabilidade - Select tabela INV
-- Versao:			1.0.0.0
-- Data Versao:     25/07/2017
-- =========================================================
CREATE FUNCTION FN_RENTABILIDADE_INV
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
	IF ISNULL(@Gerente, '') <> ''
	BEGIN
		SET @Where += ' AND GERENTE.empID IN ( ' + @Gerente + ') '
	END

	SET @Sql = 
	'DECLARE @Total NUMERIC(19, 6)
	
	SELECT @Total = SUM(INV1.LineTotal) - SUM(RIN1.LineTotal)
	FROM OINV WITH(NOLOCK) 
		INNER JOIN INV1 WITH(NOLOCK)
			ON INV1.DocEntry = OINV.DocEntry
		INNER JOIN OITM WITH(NOLOCK)
			ON OITM.ItemCode = INV1.ItemCode
		INNER JOIN INV12 WITH(NOLOCK)
			ON INV12.DocEntry = OINV.DocEntry 
		INNER JOIN HTM1 [TIME] WITH(NOLOCK)
				ON [TIME].empID = OINV.OwnerCode
		INNER JOIN HTM1 GERENTE WITH(NOLOCK)
			ON GERENTE.teamID = [TIME].teamID
		INNER JOIN OHEM WITH(NOLOCK)
			ON OHEM.empID = GERENTE.empID
		INNER JOIN HEM6 WITH(NOLOCK)
			ON HEM6.empID = GERENTE.empID
		INNER JOIN OHTY WITH(NOLOCK)
			ON OHTY.typeID = HEM6.roleID
			AND CAST(OHTY.descriptio AS NVARCHAR(MAX)) = ''Gerente''
		LEFT JOIN RIN1 WITH(NOLOCK)
			ON RIN1.BaseEntry = INV1.DocEntry
			AND RIN1.BaseLine = INV1.LineNum
			AND RIN1.BaseType = 13
	WHERE OINV.CANCELED = ''N'' AND OINV.DocTotal > 0 ' +
	@Where + '
	-- AND INV1.Usage = 9' + '
	
	;WITH Frete AS
	(
		SELECT
			INV1.ItemCode,
			OSLP.SlpCode,
			OSLP.SlpName,
			OHEM.empID	ManagerCode,
			OHEM.firstName + '' '' + OHEM.lastName			AS ManagerName,
			OINV.CardCode,
			OINV.CardName,
			INV1.LineNum,
			INV12.CityS,
			OITM.ItmsGrpCod,
			OITM.U_Segmento,
			OITM.U_Acabamento,
			OITM.U_ClassifItem,
			OITM.U_Disponibilidade,
			OITM.U_Sistema,
			OITM.U_Marca,
			INV1.Price,
			CASE WHEN ISNULL(RIN1.DocEntry, 0) = 0
				THEN INV1.Weight1
				ELSE INV1.Weight1 - RIN1. Weight1 
			END [Peso],
			INV6.[Prazo Medio],
			CASE WHEN ISNULL(RIN1.DocEntry, 0) = 0
				THEN	INV1.LineTotal / ISNULL(NULLIF((INV1.Weight1 / ISNULL(INV1.U_CVA_Density, 1)), 0), 1)
				ELSE	INV1.LineTotal / ISNULL(NULLIF((INV1.Weight1 / ISNULL(INV1.U_CVA_Density, 1)), 0), 1) - 
						RIN1.LineTotal / ISNULL(NULLIF((RIN1.Weight1 / ISNULL(RIN1.U_CVA_Density, 1)), 0), 1)
			END [Méd. Litros],
			CASE WHEN ISNULL(RIN1.DocEntry, 0) = 0
				THEN INV1.LineTotal / ISNULL(@Total, 1) * 100		
				ELSE (INV1.LineTotal - RIN1.LineTotal) / ISNULL(@Total, 1) * 100		
			END [% Rep],
			CASE WHEN ISNULL(RIN1.DocEntry, 0) = 0
				THEN INV1.Weight1 / ISNULL(INV1.U_CVA_Density, 1)	
				ELSE INV1.Weight1 / ISNULL(INV1.U_CVA_Density, 1) - RIN1.Weight1 / ISNULL(RIN1.U_CVA_Density, 1)
			END [Vendas Litros],
			CASE WHEN ISNULL(RIN1.DocEntry, 0) = 0
				THEN INV1.Price * INV1.Quantity
				ELSE (INV1.Price * INV1.Quantity) - (RIN1.Price * RIN1.Quantity)
			END [Valor Produto],
			CASE WHEN ISNULL(RIN1.DocEntry, 0) = 0
				THEN INV1.Price * ISNULL(INV1.U_CVA_CustoVar, 1)
				ELSE (INV1.Price * ISNULL(INV1.U_CVA_CustoVar, 1)) - (RIN1.Price * ISNULL(RIN1.U_CVA_CustoVar, 1))
			END [Custo Variável],
			CASE WHEN ISNULL(RIN1.DocEntry, 0) = 0
				THEN ISNULL(INV1.U_CVA_CustoVar, 0)
				ELSE ISNULL(INV1.U_CVA_CustoVar, 0) - ISNULL(RIN1.U_CVA_CustoVar, 0)
			END [% Custo Variável],
			CASE WHEN ISNULL(RIN1.DocEntry, 0) = 0
				THEN ISNULL(INV1.U_CVA_CustoDia, 0)
				ELSE ISNULL(INV1.U_CVA_CustoDia, 0) - ISNULL(RIN1.U_CVA_CustoDia, 0)
			END [Custo Dia],
			CASE WHEN ISNULL(RIN1.DocEntry, 0) = 0
				THEN ISNULL(INV1.U_CVA_CustoFixo, 0)
				ELSE ISNULL(INV1.U_CVA_CustoFixo, 0) - ISNULL(RIN1.U_CVA_CustoFixo, 0)
			END [U_CVA_CustoFixo],
			CASE WHEN ISNULL(RIN1.DocEntry, 0) = 0
				THEN	CASE WHEN ISNULL(OPCH.DocEntry, 0) = 0
							THEN ISNULL(INV1.U_akbfreight, 0)
							ELSE (INV1.Weight1 / ISNULL(NULLIF(OINV.Weight, 0), 1)) * OPCH.DocTotal
						END
				ELSE	CASE WHEN ISNULL(OPCH.DocEntry, 0) = 0
							THEN ISNULL(INV1.U_akbfreight, 0) - ISNULL(RIN1.U_akbfreight, 0)
							ELSE (RIN1.Weight1 / ISNULL(NULLIF(OINV.Weight, 0), 1)) * OPCH.DocTotal
						END
			END [Valor Frete]
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
			INNER JOIN OSLP WITH(NOLOCK)
				ON OSLP.SlpCode = OINV.SlpCode
			INNER JOIN HTM1 [TIME] WITH(NOLOCK)
				ON [TIME].empID = OINV.OwnerCode
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
				ON FRETE_ITEM.U_DocNum = OINV.DocNum
			LEFT JOIN [@CVA_NFE_FRETE] FRETE WITH(NOLOCK)
				ON FRETE.Code = FRETE_ITEM.Code
			LEFT JOIN OPCH WITH(NOLOCK)
				ON OPCH.DocEntry = FRETE.U_DocEntry 
			LEFT JOIN RIN1 WITH(NOLOCK)
				ON RIN1.BaseEntry = INV1.DocEntry
				AND RIN1.BaseLine = INV1.LineNum
				AND RIN1.BaseType = 13
		WHERE OINV.CANCELED = ''N'' AND OINV.DocTotal > 0 ' +
		@Where + '
		-- AND INV1.Usage = 9' + '
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