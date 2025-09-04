IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_RENTABILIDADE_TOTAL')
	DROP PROCEDURE SP_CVA_RENTABILIDADE_TOTAL
GO
-- =========================================================
-- Autor:			Felipe Hosomi
-- Criação:			25/07/2017
-- Descrição:		Relatório de Rentabilidade - Select tabela INV
-- Versao:			1.0.0.0
-- Data Versao:     25/07/2017
-- =========================================================
CREATE PROCEDURE SP_CVA_RENTABILIDADE_TOTAL
(
	@DataDe				NVARCHAR(MAX), -- Formato yyyyMMdd
	@DataAte			NVARCHAR(MAX), -- Formato yyyyMMdd	
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
AS
BEGIN
	DECLARE @Sql NVARCHAR(MAX)
	DECLARE @Where NVARCHAR(MAX)

	SET @Where = [dbo].FN_CVA_RENTABILIDADE_WHERE(@DataDe, @DataAte, @GrupoItem, @Item, @Vendedor, @Gerente, @Linha, @Cidade, @Segmento, @Acabamento, @Classificacao, @Disponibilidade, @Sistema, @Marca)

	SET @Sql = 
	'DECLARE @Total NUMERIC(19, 6)
	
	SELECT SUM(INV1.LineTotal) - ISNULL(SUM(RIN1.LineTotal), 0)
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
	AND INV1.Usage = 9'

	EXEC (@Sql)
END