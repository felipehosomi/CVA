IF EXISTS (SELECT * FROM sys.objects WHERE type = 'FN' AND name = 'FN_CVA_MascaraCNPJ')
	DROP FUNCTION FN_CVA_MascaraCNPJ
GO
CREATE FUNCTION [dbo].[FN_CVA_MascaraCNPJ] (@cnpj varchar(20))
returns varchar(18)
as
begin

  declare @retorno varchar(18)
  
  select @cnpj = replace(replace(REPLACE(@cnpj, '.', ''), '/', ''), '-', '')
  
  if len(RTRIM(ltrim(ISNULL(@cnpj, '')))) = '14'
    select @retorno = LEFT(@cnpj, 2) + '.'
  	     + SUBSTRING(@cnpj, 3, 3) + '.'
  	     + SUBSTRING(@cnpj, 6, 3) + '/'
  	     + SUBSTRING(@cnpj, 9, 4) + '-'
  	     + RIGHT(@cnpj, 2)
 
  return isnull(@retorno, '')
     
end






