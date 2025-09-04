/****** Object:  StoredProcedure [dbo].[spc_CVA_ResFi_ColLimite]    Script Date: 31/08/2018 15:15:43 ******/
DROP PROCEDURE [dbo].[spc_CVA_ResFi_ColLimite]
GO

/****** Object:  StoredProcedure [dbo].[spc_CVA_ResFi_ColLimite]    Script Date: 31/08/2018 15:15:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[spc_CVA_ResFi_ColLimite]
  @Cli_Grup varchar(20)
 ,@cardtype char (1)
as
begin

	if (@cardtype = 'C')
		begin
			select CreditLine 
				 ,(select (select CreditLine from ocrd where cardcode = @Cli_Grup) - (sum(BalDueCred) - sum(BalDueDeb))*-1 from jdt1 where ShortName = @Cli_Grup  )
				 ,U_CVA_ValCredito
				 ,CreditLine 
			  from ocrd 
			 where cardcode =  @Cli_Grup
		end
	else 
	 if(@cardtype = 'S')
		 Begin
			select convert(decimal(19,2),CreditLine) 
				 ,(select (select CreditLine from ocrd where cardcode = @Cli_Grup) - (sum(BalDueCred) - sum(BalDueDeb))*-1 from jdt1 where ShortName = @Cli_Grup  )
				 ,U_CVA_ValCredito
				 ,CreditLine 
			  from ocrd 
			 where cardcode =  @Cli_Grup
		 end
end

GO

exec spc_CVA_ResFi_ColLimite 'C40000','C'