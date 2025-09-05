/*    ==Parâmetros de Script==

    Versão do Servidor de Origem : SQL Server 2014 (12.0.2000)
    Edição do Mecanismo de Banco de Dados de Origem : Microsoft SQL Server Standard Edition
    Tipo do Mecanismo de Banco de Dados de Origem : SQL Server Autônomo

    Versão do Servidor de Destino : SQL Server 2014
    Edição de Mecanismo de Banco de Dados de Destino : Microsoft SQL Server Standard Edition
    Tipo de Mecanismo de Banco de Dados de Destino : SQL Server Autônomo
*/

USE [SBOHybel]
GO

/****** Object:  StoredProcedure [dbo].[spcCVABalancete]    Script Date: 22/08/2018 16:20:31 ******/
DROP PROCEDURE [dbo].[spcCVABalancete]
GO

/****** Object:  StoredProcedure [dbo].[spcCVABalancete]    Script Date: 22/08/2018 16:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






  
-- 
CREATE procedure [dbo].[spcCVABalancete]  
  @caixa char(1) = 'N'  
as  
 -- 	if DATEDIFF(day,getdate(),cast('2016-11-20' as date))<=0 begin
	--	RAISERROr ('Claudino Software: Tempo de uso de Demostração Expirado: 41 99327681', 11,1)
	--end
  if @caixa = 'S'  
    select OACT.AcctCode  + ' ' + upper(OACT.AcctName) 'AcctName'  
         , oact.CurrTotal as 'Saldo'  
      from OACT   
     where ( OACT.FINANSE = 'Y' )  
  else  
    select OACT.AcctCode  + ' ' + upper(OACT.AcctName) 'AcctName'  
         , oact.CurrTotal as 'Saldo'  
      from OACT   
     where ( OACT.FINANSE = 'Y'  )  
       and oact.AcctName not like 'Caixa%'  
  
  
  


GO

