
/****** Object:  StoredProcedure [dbo].[spc_CVA_ResFi_MaiorCompraGrupo]    Script Date: 31/08/2018 15:15:43 ******/
DROP PROCEDURE [dbo].[spc_CVA_ResFi_MaiorCompraGrupo]
GO

/****** Object:  StoredProcedure [dbo].[spc_CVA_ResFi_MaiorCompraGrupo]    Script Date: 31/08/2018 15:15:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[spc_CVA_ResFi_MaiorCompraGrupo]
  @grupo varchar(20)

as

begin

select DocDate 
      ,DocTotal
	  ,Serial
  from oinv 
 inner join OCRD on oinv.CardCode = ocrd.CardCode
  where doctotal = (select MAX(DocTotal) from OINV inner join OCRD on oinv.CardCode = ocrd.CardCode  where OCRD.U_CVA_GRUPOECON = @grupo) 
	AND OCRD.U_CVA_GRUPOECON = @grupo
		
 end
GO

exec spc_CVA_ResFi_MaiorCompraGrupo '002'







