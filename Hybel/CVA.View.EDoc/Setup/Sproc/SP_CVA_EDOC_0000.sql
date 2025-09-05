IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_EDOC_0000')
	DROP PROCEDURE SP_CVA_EDOC_0000
GO
CREATE PROCEDURE SP_CVA_EDOC_0000
(
	@Filial			INT,
	@DataInicial	DATETIME,
	@DataFinal		DATETIME
)
AS
BEGIN
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
		'Brasil'		Pais,
		'91'			CodConteudo,
		'Hybel'			NomeFantasia
	FROM OBPL WITH(NOLOCK)
	WHERE BPLId = @Filial
END