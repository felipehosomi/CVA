 --EXEC SP_CVA_DIME_22 '0001' 
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_DIME_22')
	DROP PROCEDURE SP_CVA_DIME_22
GO
CREATE PROCEDURE SP_CVA_DIME_22
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
		BaseCalculoImpostoRetido	NUMERIC(19,6)
	)
	
	INSERT INTO #DIME
	EXEC SP_CVA_DIME_ENTRADA @Code
	
	SELECT
		'22'									[Tipo],
		'01'									[Quadro],
		CFOP,
		ISNULL(SUM(DIME.ValorContabil), 0)				[ValorContabil],
		ISNULL(SUM(DIME.BaseCalculo), 0)				[BaseCalculo],
		ISNULL(SUM(DIME.ImpostoCreditado), 0)			[ImpostoCreditado],
		ISNULL(SUM(DIME.Isentas), 0)					[Isentas],
		ISNULL(SUM(DIME.Outras), 0)						[Outras],
		ISNULL(SUM(DIME.BaseCalculoImpostoRetido), 0)	[BaseCalculoImpostoRetido],
		ISNULL(SUM(DIME.ImpostoRetido), 0)				[ImpostoRetido]
	FROM #DIME DIME
	WHERE ISNULL(DIME.CFOP, '') <> ''
	GROUP BY
		CFOP

	DROP TABLE #DIME
END
