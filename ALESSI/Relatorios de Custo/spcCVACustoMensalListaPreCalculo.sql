USE SBOAlessi
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcCVACustoMensalListaPreCalculo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcCVACustoMensalListaPreCalculo]
GO

USE SBOAlessi
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[spcCVACustoMensalListaPreCalculo] (
	@dtInicial smalldatetime,
	@dtFinal smalldatetime
)
with encryption
as 
begin

set language brazilian

--declare @dtInicial smalldatetime 
--declare @dtFinal smalldatetime 

--set @dtInicial=[%0]
--set @dtFinal=[%1]
declare @iTotalMes int
select @iTotalMes =DATEDIFF ( month, @dtInicial , @dtFinal )  +1


CREATE TABLE #TAB_AUXILIAR (
 LinhaItem			VARCHAR(MAX)
,CodigoItem			VARCHAR(MAX)
,NomeItem			VARCHAR(MAX)
,Unidade			VARCHAR(MAX)
,PrecoCustoFormula	NUMERIC(19,2)
,TotalVendaGrupo    NUMERIC(19,2)
,PerVendaGrupo      NUMERIC(19,2)
,PartCustoMix       NUMERIC(19,2)
,CustoMix           NUMERIC(19,2)
,TOTAL				NUMERIC(19,2)

,MediaDeVenda		NUMERIC(19,2)
,TotalGrupo			NUMERIC(19,2)
,JANEIRO			NUMERIC(19,2)
,FEVEREIRO			NUMERIC(19,2)
,MARÇO				NUMERIC(19,2)
,ABRIL				NUMERIC(19,2)
,MAIO				NUMERIC(19,2)
,JUNHO				NUMERIC(19,2)
,JULHO				NUMERIC(19,2)
,AGOSTO				NUMERIC(19,2)
,SETEMBRO			NUMERIC(19,2)
,OUTUBRO			NUMERIC(19,2)
,NOVEMBRO			NUMERIC(19,2)
,DEZEMBRO			NUMERIC(19,2)
)

	insert into #TAB_AUXILIAR
	(LinhaItem			
	,CodigoItem			
	,NomeItem			
	,Unidade			
	,PrecoCustoFormula	
	,TOTAL				
	,JANEIRO			
	,FEVEREIRO			
	,MARÇO				
	,ABRIL				
	,MAIO				
	,JUNHO				
	,JULHO				
	,AGOSTO				
	,SETEMBRO			
	,OUTUBRO			
	,NOVEMBRO			
	,DEZEMBRO)
	select 
		LinhaItem
		,CodigoItem
		,NomeItem
		,Unidade
		,(Select Top 1 B1.MATERIAL_MC From BEAS_OITM_CALCPRICE B1 Where CodigoItem = B1.ItemCode order by B1.CALCDATE Desc) PrecoCustoFormula
		,(abs(coalesce([1],0))+abs(coalesce([2],0))+abs(coalesce([3],0))+
		abs(coalesce([4],0))+abs(coalesce([5],0))+abs(coalesce([6],0))+
		abs(coalesce([7],0))+abs(coalesce([8],0))+abs(coalesce([9],0))+
		abs(coalesce([10],0))+abs(coalesce([11],0))+abs(coalesce([12],0))) as TOTAL
		,abs(coalesce([1],0)) AS JANEIRO , 
		abs(coalesce([2],0)) AS FEVEREIRO , 
		abs(coalesce([3],0)) AS MARÇO , 
		abs(coalesce([4],0)) AS ABRIL , 
		abs(coalesce([5],0)) AS MAIO , 
		abs(coalesce([6],0)) AS JUNHO , 
		abs(coalesce([7],0)) AS JULHO , 
		abs(coalesce([8],0)) AS AGOSTO , 
		abs(coalesce([9],0)) AS SETEMBRO , 
		abs(coalesce([10],0)) AS OUTUBRO , 
		abs(coalesce([11],0)) AS NOVEMBRO , 
		abs(coalesce([12],0)) AS DEZEMBRO 
		from 
			(
				SELECT
					 SUBSTRING(T0.ITEMCODE, 3, 3) + SUBSTRING(T0.ITEMCODE, 10, 2) LinhaItem
					,T0.ITEMCODE		CodigoItem       
					,T0.ITEMNAME		NomeItem       
					,T0.U_CONTEUDO1	Unidade 
					,T1.QUANTITY		QtdVendidaMes
					--,T2.DocDate	
					,DatePart(month, T2.DocDate	) Mes
				FROM OINV T2 
					INNER JOIN INV1 T1 on T1.DocEntry=T2.DocEntry and  T1.USAGE = 9
					inner join OITM T0 on T0.ItemCode=T1.ItemCode
					--INNER JOIN BEAS_OITM_CALCPRICE T3 ON T0.ITEMCODE = T3.ITEMCODE
				where 
					T2.DocDate>=@dtInicial 
					and T2.DocDate<=@dtFinal 
					and T2.CANCELED ='N' 
					--and T0.ItemCode='PA001I00101'
					and exists(select 1 from BEAS_OITM_CALCPRICE T3 where T0.ITEMCODE = T3.ITEMCODE)
			) tb
		PIVOT
		(
			sum(QtdVendidaMes)  FOR MES IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])
		) as PivotTable


				--SELECT
				--	 /*SUBSTRING(T0.ITEMCODE, 3, 3) + SUBSTRING(T0.ITEMCODE, 10, 2)*/--'' LinhaItem
				--	--,T0.ITEMCODE		CodigoItem       
				--	--,T0.ITEMNAME		NomeItem       
				--	--,/*T0.U_CONTEUDO1*/	''	Unidade 
				--	sum(T1.QUANTITY)
				--	--,T2.DocDate	
				--	--,DatePart(month, T2.DocDate	) Mes
				--FROM OINV T2 
				--	INNER JOIN INV1 T1 on T1.DocEntry=T2.DocEntry and  T1.USAGE = 9
				--	inner join OITM T0 on T0.ItemCode=T1.ItemCode
				--	--INNER JOIN BEAS_OITM_CALCPRICE T3 ON T0.ITEMCODE = T3.ITEMCODE
				--where T2.DocDate>=@dtInicial and T2.DocDate<=@dtFinal and T2.CANCELED ='N' and T0.ItemCode='PA001I00101'

--Select 
--	sum(T1.Quantity)
--FROM 
--	OINV T2 
--	INNER JOIN INV1 T1 on T1.DocEntry=T2.DocEntry and  T1.USAGE = 9
--where 
--T2.DocDate>=@dtInicial and T2.DocDate<=@dtFinal 
--	--DATEPART(year ,T2.DocDate)=2018 
--	--and  DATEPART(month ,T2.DocDate)=03 
--	and ItemCode='PA001I00101' and T2.CANCELED ='N'

--) as TB1

	--INNER JOIN BEAS_OITM_CALCPRICE T3 ON T0.ITEMCODE = T3.ITEMCODE

	update #TAB_AUXILIAR set TotalVendaGrupo=(select sum(TOTAL) from #TAB_AUXILIAR T0 where T0.LinhaItem=#TAB_AUXILIAR.LinhaItem)

	update #TAB_AUXILIAR set PerVendaGrupo=  (Total*100)/TotalVendaGrupo

	UPDATE #TAB_AUXILIAR SET PartCustoMix  = ((PerVendaGrupo / 100) * PrecoCustoFormula)

	update #TAB_AUXILIAR set CustoMix=(select sum(PartCustoMix) from #TAB_AUXILIAR T0 where T0.LinhaItem=#TAB_AUXILIAR.LinhaItem)


	update #TAB_AUXILIAR set MediaDeVenda=(select sum(Total) /@iTotalMes from #TAB_AUXILIAR T0 where T0.CodigoItem=#TAB_AUXILIAR.CodigoItem)

	update #TAB_AUXILIAR set TotalGrupo=(select sum(Total) /@iTotalMes from #TAB_AUXILIAR T0 where T0.LinhaItem=#TAB_AUXILIAR.LinhaItem)

	--update #TAB_AUXILIAR set MediaDeVenda=Total/@iTotalMes
	/*
	TotalVendaGrupo		100
	Total				x

	TotalVendaGrupo * x = Total * 100
	x = (Total * 100)/TotalVendaGrupo

	*/

select 	
	JANEIRO			
	,FEVEREIRO			
	,MARÇO				
	,ABRIL				
	,MAIO				
	,JUNHO				
	,JULHO				
	,AGOSTO				
	,SETEMBRO			
	,OUTUBRO			
	,NOVEMBRO			
	,DEZEMBRO
	,TOTAL
	,MediaDeVenda
	,TotalGrupo
	,LinhaItem			
	,CodigoItem			
	,NomeItem			
	,Unidade			
	,PrecoCustoFormula	
	,TotalVendaGrupo
	,PerVendaGrupo
	,PartCustoMix
	,CustoMix

 from #TAB_AUXILIAR
 ORDER BY LinhaItem,CodigoItem

drop TABLE #TAB_AUXILIAR

END

GO


--execute spcCVACustoMensalListaPreCalculo '2000-08-18','2050-10-25'