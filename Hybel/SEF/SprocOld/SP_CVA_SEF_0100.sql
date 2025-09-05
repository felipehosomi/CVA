IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_SEF_0100')
	DROP PROCEDURE SP_CVA_SEF_0100
GO

--exec SP_CVA_SEF_0100 1,'2018-01-01','2018-31-01'
CREATE PROCEDURE SP_CVA_SEF_0100
(
	@Filial			INT
)
AS
BEGIN
	SELECT
		'0100'									LIN,
		OHEM.firstName + ' ' + OHEM.lastName	NOME,
		900										COD_ASSIN,
		''										CNPJ,
		ISNULL(OHEM.CPF,'')						CPF,
		OHEM.CRC								CRC,
		ISNULL(OHEM.U_CVA_CEP,'')				CEP,
		ISNULL(OHEM.U_CVA_Endereco,'')       	'END',
		ISNULL(OHEM.U_CVA_Numero,'')			NUM,
		ISNULL(OHEM.U_CVA_Complemento,'')		COMPL,
		ISNULL(OHEM.U_CVA_Bairro,'')			BAIRRO,
		ISNULL(OHEM.U_CVA_UF,'')				UF,
		ISNULL(OHEM.U_CVA_CodMunicipio,'')		COD_MUN,
		''										CEP_CP,
		''										CP,
		ISNULL(OHEM.U_CVA_Telefone,'')			FONE,
		''										FAX,
		ISNULL(OHEM.U_CVA_Email,'')				EMAIL
	FROM OHEM
	WHERE OHEM.ContResp = 'Y'
END