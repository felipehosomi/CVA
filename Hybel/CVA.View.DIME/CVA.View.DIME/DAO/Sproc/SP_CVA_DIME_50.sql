IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_DIME_50')
	DROP PROCEDURE SP_CVA_DIME_50
GO
CREATE PROCEDURE SP_CVA_DIME_50
(
	@Code NVARCHAR(50)
)
AS
BEGIN
	CREATE TABLE #DIME
	(
		CardCode					NVARCHAR(MAX) COLLATE DATABASE_DEFAULT,
		CFOP						NVARCHAR(MAX),
		Cidade						NVARCHAR(MAX),
		UF							NVARCHAR(2),
		ValorContabil				NUMERIC(19,6),
		BaseCalculo					NUMERIC(19,6),
		ImpostoCreditado			NUMERIC(19,6),
		Isentas						NUMERIC(19,6),
		Outras						NUMERIC(19,6),
		ImpostoRetido				NUMERIC(19,6),
		BaseCalculoImpostoRetido	NUMERIC(19,6),
	)
	
	INSERT INTO #DIME
	EXEC SP_CVA_DIME_SAIDA @Code, 1
	
	SELECT
		'50'							[Tipo],
		'50'							[Quadro],
		ISNULL(NULLIF(UF, ''), 'EX')	UF,
		SUM(CASE WHEN ISNULL(CRD7.CardCode, '') <> '' THEN DIME.BaseCalculo ELSE 0 END)			[BaseContribuinte],
		SUM(CASE WHEN ISNULL(CRD7.CardCode, '') <> '' THEN DIME.ValorContabil ELSE 0 END)		[ValorContribuinte],
		SUM(CASE WHEN ISNULL(CRD7.CardCode, '') = '' THEN DIME.BaseCalculo ELSE 0 END)			[BaseNaoContribuinte],
		SUM(CASE WHEN ISNULL(CRD7.CardCode, '') = '' THEN DIME.ValorContabil ELSE 0 END)		[ValorNaoContribuinte],
		SUM(DIME.Outras) + SUM(DIME.Isentas)													[Outras],
		SUM(DIME.ImpostoRetido)																	[IcmsST]
	FROM #DIME DIME
		LEFT JOIN CRD7 WITH(NOLOCK)
			ON CRD7.CardCode = DIME.CardCode
			AND CRD7.Address = ''
			AND ISNULL(CRD7.TaxId1, '') <> '' AND ISNULL(CRD7.TaxId1, '') <> 'Isento'

	GROUP BY
		ISNULL(NULLIF(UF, ''), 'EX')
	ORDER BY
		ISNULL(NULLIF(UF, ''), 'EX')

	DROP TABLE #DIME
END