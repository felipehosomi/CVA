
/****** Object:  StoredProcedure [dbo].[spc_CVA_ResFi_MaiorCompra]    Script Date: 31/08/2018 15:15:43 ******/
DROP PROCEDURE [dbo].[spc_CVA_ResFi_MaiorCompra]
GO

/****** Object:  StoredProcedure [dbo].[spc_CVA_ResFi_MaiorCompra]    Script Date: 31/08/2018 15:15:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[spc_CVA_ResFi_MaiorCompra]
  @Cli_Grup varchar(20)
 ,@cardtype char (1)
as

begin

if (@cardtype = 'C')
begin
select DocDate 
      ,DocTotal
	  ,Serial
  from oinv 
  where doctotal = (select MAX(DocTotal) from OINV  where CARDCODE = @Cli_Grup) 
	AND CARDCODE = @Cli_Grup
end
else
 if	(@cardtype = 'S')
 begin
	select DocDate 
      ,DocTotal
	  ,Serial
  from OPCH 
  where doctotal = (select MAX(DocTotal) from OPCH  where CARDCODE = @Cli_Grup) 
	AND CARDCODE = @Cli_Grup
 end
	
 end


GO

exec spc_CVA_ResFi_MaiorCompra 'C40000','C'







