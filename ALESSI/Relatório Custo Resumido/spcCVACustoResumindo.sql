
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Marco Kurpel>
-- Create date: <05/03/2018>
-- Description:	<Relatório de Custos Resumido>
-- =============================================
alter PROCEDURE spcCVACustoResumindo

  @dt_inicial date
 ,@dt_final   date

AS
BEGIN

CREATE TABLE #TAB_CustoMensalListaPreCalculo 
(
JANEIRO			    VARCHAR(MAX)
,FEVEREIRO			VARCHAR(MAX)
,MARÇO				VARCHAR(MAX)
,ABRIL			   	VARCHAR(MAX)
,MAIO				VARCHAR(MAX)
,JUNHO				VARCHAR(MAX)
,JULHO				VARCHAR(MAX)
,AGOSTO				VARCHAR(MAX)
,SETEMBRO			VARCHAR(MAX)
,OUTUBRO			VARCHAR(MAX)
,NOVEMBRO			VARCHAR(MAX)
,DEZEMBRO			VARCHAR(MAX)
,TOTAL				VARCHAR(MAX)
,MediaDeVenda		VARCHAR(MAX)
,TotalGrupo			VARCHAR(MAX)
,LinhaItem			VARCHAR(MAX)
,CodigoItem			VARCHAR(MAX)
,NomeItem			VARCHAR(MAX)
,Unidade			VARCHAR(MAX)
,PrecoCustoFormula	VARCHAR(MAX)
,TotalVendaGrupo	VARCHAR(MAX)
,PerVendaGrupo		VARCHAR(MAX)
,PartCustoMix		VARCHAR(MAX)
,CustoMix			VARCHAR(MAX) 
)

insert into #TAB_CustoMensalListaPreCalculo 
exec [spcCVACustoMensalListaPreCalculo]  @dt_inicial, @dt_final

CREATE TABLE #TAB_CustoMensalListaDePreco
 (
 JANEIRO			   VARCHAR(MAX)
,FEVEREIRO			   VARCHAR(MAX)
,MARÇO				   VARCHAR(MAX)
,ABRIL				   VARCHAR(MAX)
,MAIO				   VARCHAR(MAX)
,JUNHO				   VARCHAR(MAX)
,JULHO				   VARCHAR(MAX)
,AGOSTO				   VARCHAR(MAX)
,SETEMBRO			   VARCHAR(MAX)
,OUTUBRO			   VARCHAR(MAX)
,NOVEMBRO			   VARCHAR(MAX)
,DEZEMBRO			   VARCHAR(MAX)
,TOTAL				   VARCHAR(MAX)
,MediaDeVenda		   VARCHAR(MAX)
,TotalGrupo			   VARCHAR(MAX)
,LinhaItem			   VARCHAR(MAX)
,CodigoItem			   VARCHAR(MAX)
,NomeItem			   VARCHAR(MAX)
,Unidade			   VARCHAR(MAX)
,PrecoCustoListaDePrec VARCHAR(MAX)
,TotalVendaGrupo	   VARCHAR(MAX)
,PerVendaGrupo		   VARCHAR(MAX)
,PartCustoMix		   VARCHAR(MAX)
,CustoMix			   VARCHAR(MAX)
)

insert into #TAB_CustoMensalListaDePreco
exec [spcCVACustoMensalListaDePreco]  @dt_inicial, @dt_final


 
select distinct  
				 oitm.itemcode as 'Codigo SAP'
		        ,oitm.itemname as 'Descrição'				
			    ,T0.MediaDeVenda as 'Media Venda' 
			    ,BEAS_OITM_CALCPRICE.MATERIAL_MC as 'Custo fórmula Item'
			    ,T1.PrecoCustoFormula  as 'Custo Fórmula Mix'
			    ,oitw.avgprice as 'Custo Medio Item'
			    ,T0.CustoMix as'Custo Médio Mix'
			    ,ITM1.price as 'Tabela de Preço'
				
		  from  oinv
		 inner JOIN INV1  on oinv.DocEntry = inv1.DocEntry
		 inner join oitm  on inv1.itemcode = oitm.itemcode
		 inner join BEAS_OITM_CALCPRICE  on BEAS_OITM_CALCPRICE.itemcode = oitm.itemcode
		 inner join OITW  on oitm.itemcode = oitw.itemcode
		 inner join ITM1  on inv1.ITEMCODE = itm1.ITEMCODE
		  LEFT join #TAB_CustoMensalListaDePreco    T0 on t0.CodigoItem = oitm.itemcode COLLATE SQL_Latin1_General_CP850_CI_AS
		  LEFT join #TAB_CustoMensalListaPreCalculo T1 on T1.CodigoItem = OITM.itemcode COLLATE SQL_Latin1_General_CP850_CI_AS

		  where oinv.docdate between @dt_inicial and @dt_final
			and oinv.CANCELED ='N'
			and ITM1.PRICELIST = '4'
			and oitw.whscode = '03'

drop table #TAB_CustoMensalListaDePreco		
drop table #TAB_CustoMensalListaPreCalculo	

END
GO


exec spcCVACustoResumindo '2000-01-01','2020-01-01'
