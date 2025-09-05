IF EXISTS (SELECT * FROM sys.objects WHERE type = 'IF' AND name = 'FN_CVA_SEF_0000')
	DROP FUNCTION FN_CVA_SEF_0000
GO
CREATE FUNCTION FN_CVA_SEF_0000
(
	@Filial			INT,
	@DataInicial	DATETIME,
	@DataFinal		DATETIME
) RETURNS TABLE
AS
RETURN
(
	SELECT
		'0000'			Linha,
		'LFPD'			LFPD,
		@DataInicial	DataInicial,
		@DataFinal		DataFinal,
		OBPL.BPLName	Filial,
		OBPL.TaxIdNum	CNPJ,
		OBPL.State		UF,
		OBPL.TaxIdNum2	IE,
		'2611606'		CodMunicipio,
		'3680703'		IM,
		''              Suframa,
		''              Cod_Ver,
		''              Cod_Fin,
		''              Cod_CTD,
		'Brasil'		Pais,
		OBPL.AliasName  NomeFantasia,
		'91'			CodConteudo,
		OBPL.CommerReg  Nire,
		''              CPF,
		''				Vazio2
		

	FROM OBPL WITH(NOLOCK)
	WHERE BPLId = @Filial
)
