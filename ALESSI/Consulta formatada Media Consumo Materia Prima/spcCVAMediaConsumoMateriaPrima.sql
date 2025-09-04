USE CVA_Teste_09_05_18
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcCVAMediaConsumoMateriaPrima]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcCVAMediaConsumoMateriaPrima]
GO

USE CVA_Teste_09_05_18
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spcCVAMediaConsumoMateriaPrima] 
(
	@dtInicial int
)

as 
begin

declare @dtInicial1 int

if( @dtInicial is null )
 set @dtInicial = (select datepart(Year,getdate()))
else
 set @dtInicial = @dtInicial
 

set language brazilian

CREATE TABLE #TAB_AUXILIAR (
 CodigoItem			VARCHAR(MAX)
,DescItem			VARCHAR(MAX)
,Janeiro			NUMERIC(19,2)
,Fevereiro			NUMERIC(19,2)
,Março				NUMERIC(19,2)
,Abril				NUMERIC(19,2)
,Maio				NUMERIC(19,2)
,Junho				NUMERIC(19,2)
,Julho				NUMERIC(19,2)
,Agosto				NUMERIC(19,2)
,Setembro			NUMERIC(19,2)
,Outubro			NUMERIC(19,2)
,Novembro			NUMERIC(19,2)
,Dezembro			NUMERIC(19,2)
)

	insert into #TAB_AUXILIAR
	(CodigoItem			
	,DescItem	
	,Janeiro			
	,Fevereiro			
	,Março				
	,Abril				
	,Maio				
	,Junho				
	,Julho				
	,Agosto				
	,Setembro			
	,Outubro			
	,Novembro			
	,Dezembro)
	select 
	     CodItem
		,DescItem
		,abs(coalesce([1],0))  AS Janeiro	
		,abs(coalesce([2],0))  AS Fevereiro	  
		,abs(coalesce([3],0))  AS Março		
		,abs(coalesce([4],0))  AS Abril		
		,abs(coalesce([5],0))  AS Maio		
		,abs(coalesce([6],0))  AS Junho		
		,abs(coalesce([7],0))  AS Julho		
		,abs(coalesce([8],0))  AS Agosto		
		,abs(coalesce([9],0))  AS Setembro	
		,abs(coalesce([10],0)) AS Outubro	
		,abs(coalesce([11],0)) AS Novembro	
		,abs(coalesce([12],0)) AS Dezembro	
		from 
			(
				SELECT
					 T0.ITEMCODE		CodItem       
					,T0.Dscription		DescItem       									
					,DatePart(month, T0.DocDate	) Mes
					,(T0.InQty - T0.OutQty) media
				FROM OINM T0 
			   inner join IGE1 T1 ON T0.TRANSTYPE = T1.OBJTYPE
				 and T0.Base_Ref   = T1.Docentry
				 and T0.DocLineNum = T1.LineNum
			   
			   where (@dtInicial = (datepart(Year,T0.DocDate))) 
					and T0.ItemCode like'MP%' 
					and T1.WhsCode in(01,02)

			) tb
		PIVOT
		(
			sum(media) FOR MES IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])
		) as PivotTable

	select * from #TAB_AUXILIAR

    drop table #TAB_AUXILIAR

end

go

exec spcCVAMediaConsumoMateriaPrima null