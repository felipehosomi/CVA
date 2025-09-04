/****** Object:  StoredProcedure [dbo].[spc_CVA_ResFi_UltimaCompraGrupo]    Script Date: 31/08/2018 15:15:43 ******/
DROP PROCEDURE [dbo].[spc_CVA_ResFi_UltimaCompraGrupo]
GO

/****** Object:  StoredProcedure [dbo].[spc_CVA_ResFi_UltimaCompraGrupo]    Script Date: 31/08/2018 15:15:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[spc_CVA_ResFi_UltimaCompraGrupo]
  @Grupo varchar(20) 
as
begin

  select 
	 T0.Serial as 'Serial'
	,DocTotal  as 'Valor'
	,T0.TaxDate
 from oinv T0
inner join OCRD T1 on T0.CardCode = T1.CardCode
 WHERE T0.TaxDate = (select MAX(TaxDate)  from oinv inner join OCRD on oinv.CardCode = OCRD.CardCode where OCRD.U_CVA_GRUPOECON = @Grupo)
   AND t0.Serial = (select MAX(Serial)  from oinv inner join OCRD on oinv.CardCode = OCRD.CardCode where OCRD.U_CVA_GRUPOECON = @Grupo)
   AND T1.U_CVA_GRUPOECON = @Grupo

 end



GO

exec spc_CVA_ResFi_UltimaCompraGrupo '002'

