CREATE FUNCTION [dbo].[fncNMSMascaraCNPJ] (@cnpj varchar(20))
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






