--EXEC SP_CVA_DIME_49 '0001'
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_DIME_49')
	DROP PROCEDURE SP_CVA_DIME_49
GO
CREATE PROCEDURE SP_CVA_DIME_49
(
	@Code NVARCHAR(50)
)
AS
BEGIN
	CREATE TABLE #DIME
	(
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
	EXEC SP_CVA_DIME_ENTRADA @Code
	
	SELECT
		'49'						[Tipo],
		'49'						[Quadro],
		UF,
		SUM(DIME.BaseCalculo)		[BaseCalculo],
		SUM(DIME.ValorContabil)		[Valor],
		SUM(DIME.Outras) + SUM(Isentas)	[Outras],
		SUM(DIME.ImpostoRetido)		[OutrosProdutos]
	FROM #DIME DIME
	WHERE ISNULL(CFOP, '') <> ''
	GROUP BY
		UF
	ORDER BY
		UF

	DROP TABLE #DIME
END
