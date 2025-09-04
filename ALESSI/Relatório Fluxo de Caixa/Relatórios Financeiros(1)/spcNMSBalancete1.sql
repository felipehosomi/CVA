USE Greylogix
GO

/****** Object:  StoredProcedure [dbo].[spcNMSBalancete1]    Script Date: 06/09/2015 09:17:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcNMSBalancete1]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcNMSBalancete1]
GO

USE Greylogix
GO

/****** Object:  StoredProcedure [dbo].[spcNMSBalancete1]    Script Date: 06/09/2015 09:17:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



  
-- exec spcNMSBalancete1 'N'  
CREATE procedure [dbo].[spcNMSBalancete1]  
  @caixa char(1) = 'N'  
with encryption
as
    select OACT.AcctCode  + ' ' + upper(OACT.AcctName) 'AcctName'  
         , oact.CurrTotal as 'Saldo'  
      from OACT   
     where ( OACT.FINANSE = 'Y' )
GO


