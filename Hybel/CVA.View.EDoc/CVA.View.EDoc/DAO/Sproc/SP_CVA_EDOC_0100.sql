IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_EDOC_0100')
	DROP PROCEDURE SP_CVA_EDOC_0100
GO
CREATE PROCEDURE [dbo].[SP_CVA_EDOC_0100]
(
	@Filial			INT
)
AS
BEGIN
	SELECT
		'0100'					Linha,
		OHEM.firstName + ' ' + OHEM.lastName	Nome,
		'900' CodAssinante,
		OHEM.CPF,
		OHEM.CRC,
		OHEM.U_CVA_CEP			CEP,
		OHEM.U_CVA_Endereco		Endereco,
		OHEM.U_CVA_Numero		Numero,
		OHEM.U_CVA_Complemento	Complemento,
		OHEM.U_CVA_Bairro		Bairro,
		OHEM.U_CVA_UF			UF,
		OHEM.U_CVA_CodMunicipio	CodigoMunicipio,
		OHEM.U_CVA_Telefone		Telefone,
		OHEM.U_CVA_Email		Email
	FROM OHEM
	WHERE OHEM.ContResp = 'Y'
END

