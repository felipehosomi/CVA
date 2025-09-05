--EXEC SP_CVA_DIME_49 '0001'
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_DIME_32')
	DROP PROCEDURE SP_CVA_DIME_32
GO
CREATE PROCEDURE SP_CVA_DIME_32
(
	@Code NVARCHAR(50)
)
AS
BEGIN
	CREATE TABLE #DIME_ENTRADA
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

	CREATE TABLE #DIME_SAIDA
	(
		CardCode					NVARCHAR(MAX),
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
	
	DECLARE @DCPI NUMERIC(19,6)

	INSERT INTO #DIME_ENTRADA
	EXEC SP_CVA_DIME_ENTRADA @Code

	INSERT INTO #DIME_SAIDA
	EXEC SP_CVA_DIME_SAIDA @Code, 0
	
	SELECT @DCPI = SUM(ITEM.U_Valor) FROM [@CVA_DIME_46_ITEM] ITEM
		INNER JOIN [@CVA_DIME_46] DIME46
			ON DIME46.Code = ITEM.Code
		INNER JOIN [@CVA_DIME] DIME
			ON DIME.U_Periodo = DIME46.U_Periodo
			AND DIME.U_Filial = DIME46.U_Filial
	WHERE DIME.Code = @Code
	AND ITEM.U_Origem = 16

	CREATE TABLE #DIME_32
	(
		Item	 NVARCHAR(MAX),
		Descricao NVARCHAR(MAX),
		Valor	 NUMERIC(19,6)
	)

	INSERT INTO #DIME_32
	SELECT '060', 'Base cálculo do imposto retido', SUM(BaseCalculoImpostoRetido) FROM #DIME_SAIDA

	INSERT INTO #DIME_32
	SELECT '070', 'Imposto Retido com apuração mensal', SUM(ImpostoRetido) FROM #DIME_SAIDA

	INSERT INTO #DIME_32
	SELECT '080', 'Total de débitos', SUM(Valor) FROM #DIME_32 WHERE Item IN ('070', '073', '075')

	INSERT INTO #DIME_32
	SELECT '105', 'Créditos declarados no DCIP', @DCPI

	INSERT INTO #DIME_32
	SELECT '130', 'Total de créditos', SUM(Valor) FROM #DIME_32 WHERE Item IN ('090', '100', '105', '110', '120', '125')

	INSERT INTO #DIME_32
	SELECT '170', 'Saldo devedor(Total de Débito – (Total de Créditos + Ajustes das antecip. combustíveis)', (SELECT Valor FROM #DIME_32 WHERE Item = '080') - (SELECT Valor FROM #DIME_32 WHERE Item = '130')

	INSERT INTO #DIME_32
	SELECT '999', ' Imposto a recolher sobre a substituição tributária', SUM(Valor) FROM #DIME_32 WHERE Item IN ('170', '180')

	SELECT
		'32'	[Tipo],
		'11'	[Quadro],
		* 
	FROM #DIME_32
	WHERE Valor > 0

	DROP TABLE #DIME_ENTRADA
	DROP TABLE #DIME_SAIDA
	DROP TABLE #DIME_32
END

