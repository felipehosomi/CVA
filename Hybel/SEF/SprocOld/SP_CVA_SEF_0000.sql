IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_SEF_0000')
	DROP PROCEDURE SP_CVA_SEF_0000
GO

--exec SP_CVA_SEF_0000 1,'2018-01-01','2018-31-01'
CREATE PROCEDURE SP_CVA_SEF_0000
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
END


