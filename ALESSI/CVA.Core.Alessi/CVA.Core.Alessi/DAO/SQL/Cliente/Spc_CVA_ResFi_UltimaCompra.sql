/****** Object:  StoredProcedure [dbo].[spc_CVA_ResFi_UltimaCompra]    Script Date: 31/08/2018 15:15:43 ******/
DROP PROCEDURE [dbo].[spc_CVA_ResFi_UltimaCompra]
GO

/****** Object:  StoredProcedure [dbo].[spc_CVA_ResFi_UltimaCompra]    Script Date: 31/08/2018 15:15:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[spc_CVA_ResFi_UltimaCompra]
  @Cli_Grup varchar(20)
 ,@cardtype char (1)
as
begin

 select 
	 max(Serial) as 'Serial'
	,max(DocTotal) as 'Valor'
	,convert(date,TaxDate)
 from oinv

 WHERE TaxDate = (select MAX(TaxDate)  from oinv where CardCode = @Cli_Grup)
   AND CARDCODE = @Cli_Grup
   group by TaxDate
 end



GO

exec spc_CVA_ResFi_UltimaCompra 'C20000','C'

