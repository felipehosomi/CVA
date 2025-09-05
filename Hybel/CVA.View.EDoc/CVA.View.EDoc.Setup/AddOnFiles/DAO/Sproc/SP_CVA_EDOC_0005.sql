IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_EDOC_0005')
	DROP PROCEDURE SP_CVA_EDOC_0005
GO
CREATE PROCEDURE SP_CVA_EDOC_0005
(
	@Filial			INT
)
AS
BEGIN
	SELECT
		'0005'				Linha,
		COMP.U_Nome			Nome,
		COMP.U_CodAssin		CodAssinante,
		COMP.U_CPF			CPF,
		COMP.U_CEP			CEP,
		COMP.U_Endereco		Endereco,
		COMP.U_Numero		Numero,
		COMP.U_Complemento	Complemento,
		COMP.U_Bairro		Bairro,
		COMP.U_Telefone		Telefone,
		COMP.U_Email		Email
	FROM [@CVA_EDOC_COMPLEM] COMP
	WHERE COMP.U_Filial = @Filial
END