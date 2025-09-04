/****** Object:  StoredProcedure [dbo].[Spc_ResFiAtividadesPN]    Script Date: 01/11/2018 13:39:45 ******/
DROP PROCEDURE [dbo].[Spc_ResFiAtividadesPN]
GO

/****** Object:  StoredProcedure [dbo].[Spc_ResFiAtividadesPN]    Script Date: 01/11/2018 13:39:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[Spc_ResFiAtividadesPN]

  @cardcode varchar(20)
 as
 begin
  
  select T0.ClgCode as 'Cód Atividade'
		 ,T1.Name   as 'Tipo'
		 ,T2.Name  as 'Assunto' 
		 ,T3.U_NAME as 'Atribuido '
         ,isnull(T0.Details,'N/A') as 'Observação'
		 ,T0.Notes  as 'Conteúdo'

    from OCLG T0
   left join OCLT T1 on T0.CntctType = T1.Code
   left join OCLS T2 on T0.CntctSbjct = T2.Code
   inner join OUSR T3 on T0.AttendUser = T3.UserId
   where T0.CardCode = @cardcode
 end

GO


exec Spc_ResFiAtividadesPN 'C20000'