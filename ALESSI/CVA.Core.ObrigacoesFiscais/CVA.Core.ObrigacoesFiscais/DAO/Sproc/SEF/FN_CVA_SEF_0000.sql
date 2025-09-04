IF EXISTS (SELECT * FROM sys.objects WHERE type = 'IF' AND name = 'FN_CVA_SEF_0000')
	DROP FUNCTION FN_CVA_SEF_0000
GO

--- SELECT * FROM FN_CVA_SEF_0000 (9, '2018-01-01', '2018-31-01')
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
		'0000'					Linha,
		'LFPD'					LFPD,
		@DataInicial			DataInicial,
		@DataFinal				DataFinal,
		OBPL.BPLName			Filial,
		OBPL.TaxIdNum			CNPJ,
		OBPL.State				UF,
		REPLICATE('0',9 - Len(OBPL.TaxIdNum2)) + RTRIM((OBPL.TaxIdNum2))			IE,
		'2611606'				CodMunicipio,
		'3680703'				IM,
		''						Vazio1,
		''						Suframa,
		'2000'					Cod_Ver,
		0						Cod_Fin,
		20						Cod_CTD,
		'Brasil'				Pais,
		OBPL.AliasName			NomeFantasia,
		'91'					CodConteudo,
		OBPL.CommerReg			Nire,
		''						CPF,
		''						Vazio2
		

	FROM OBPL WITH(NOLOCK)
	WHERE BPLId = @Filial
)
