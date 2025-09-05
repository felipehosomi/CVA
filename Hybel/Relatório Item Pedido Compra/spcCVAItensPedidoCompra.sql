/****** Object:  StoredProcedure [dbo].[spcCVAItensPedidoCompra]    Script Date: 11/05/2018 09:51:46 ******/
DROP PROCEDURE [dbo].[spcCVAItensPedidoCompra]
GO

/****** Object:  StoredProcedure [dbo].[sspcCVAItensPedidoCompra]    Script Date: 11/05/2018 09:51:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

  

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Marco Kurpel
-- Create date: 11/05/2018
-- Description:	Relatório de Item Pedido de Compra
-- =============================================
CREATE PROCEDURE spcCVAItensPedidoCompra
	
  @dataInicial dateTime
 ,@dataFinal   dateTime
 ,@filial	   int  
 ,@Status	   char(1)  
 
AS
BEGIN

create table #Tmp
(
	 NPedido   int
	,DtLancamento  datetime
	,DtEntrega datetime
	,status   nvarchar(50)
	,cardcode nvarchar(100)
	,cardname nvarchar(100)
	,itemcode nvarchar(150)
	,itemname nvarchar(250)
	,UM      nvarchar(250)
    ,QtdPedido decimal(19,2)
    ,QtdEntregue decimal(19,2)
    ,QtdPendente decimal(19,2)
    ,Valorunitário decimal(19,2)
    ,Imposto decimal(19,2)		  
    ,Total decimal(19,2)
    ,DocDate datetime
    ,BPLId int
    ,SlpCode int
	,ObjType int
)

insert into #tmp
	select OPOR.DocEntry   as 'Nº Pedido'
		  ,OPOR.DocDate    as 'Data Lançamento'
		  ,POR1.ShipDate   as 'Data Entrega' 
		  , case when (POR1.Quantity - POR1.OpenCreQty) = 0 and POR1.LineStatus = 'O' then 'A'
		         when (POR1.Quantity - POR1.OpenCreQty) = 0 and OPOR.CANCELED   = 'Y' then 'C'
				 when (POR1.Quantity - POR1.OpenCreQty) >= POR1.Quantity then 'R'
				 when (POR1.Quantity - POR1.OpenCreQty) <  POR1.Quantity then 'P'				
			 end		   as 'Status'
		  ,OPOR.CardCode   as 'Cód. Fornecedor'
		  ,OPOR.CardName   as'Nome Fornecedor'
		  ,POR1.ItemCode   as'Cód. Produto; '
		  ,POR1.Dscription as'Descrição Produto;'
		  ,POR1.UnitMsr    as 'UM'
		  ,POR1.Quantity   as'Qtd Pedido;'
		  ,POR1.Quantity - POR1.OpenCreQty	as'Qtd Entregue;'
		  ,POR1.OpenCreQty as 'Qtd Pendente'
		  ,POR1.Price	   as'Valor unitário;'
		  ,isnull((IPI.TaxSum + ICMSST.TaxSum),0)	as 'Imposto'		  
		  ,POR1.LineTotal  as'Total;'(

		  ,OPOR.DocDate
		  ,OPOR.BPLId
		  ,OPOR.SlpCode
		  ,OPOR.ObjType
	 from OPOR
	inner join POR1 on OPOR.docentry = POR1.docentry
	 LEFT JOIN 
			   (
			   SELECT POR4.DocEntry, POR4.LineNum, POR4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  POR4 WITH(NOLOCK)           
				  INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = POR4.StaType          
				  INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'   
				  INNER JOIN POR1 WITH(NOLOCK) ON POR4.DocEntry = POR1.DocEntry AND POR4.LineNum = POR1.LineNum     
				 GROUP BY POR4.DocEntry, POR4.LineNum, POR4.NonDdctPrc
			   ) IPI ON IPI.DocEntry = POR1.DocEntry AND IPI.LineNum = POR1.LineNum   
    LEFT JOIN 
			   (
			   SELECT POR4.DocEntry, POR4.LineNum, POR4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  POR4 WITH(NOLOCK)  
				  INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = POR4.StaType          
				  INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'   
				  INNER JOIN POR1 WITH(NOLOCK) ON POR4.DocEntry = POR1.DocEntry AND POR4.LineNum = POR1.LineNum     
				 GROUP BY POR4.DocEntry, POR4.LineNum, POR4.NonDdctPrc
			   ) ICMSST ON ICMSST.DocEntry = POR1.DocEntry AND ICMSST.LineNum = POR1.LineNum 

	

	 
	 select  NPedido    as 'Nº Pedido'
	,DtLancamento		as 'Data Lançamento'
	,DtEntrega			as 'Data Entrega' 
	,case when status = 'A'  then 'Aberto'
	      when status = 'C'  then 'Cancelado'
		  when status = 'P' then 'Recebido Parcial'
		  when status = 'R' then 'Recebido'
	  end      as 'Status'
	,cardcode  as 'Cód. Fornecedor'
	,cardname  as'Nome Fornecedor'
	,itemcode  as'Cód. Produto; '
	,itemname  as'Descrição Produto;'
	,UM        as 'UM'
    ,QtdPedido as'Qtd Pedido;'
    ,QtdEntregue   as'Qtd Entregue;'
    ,QtdPendente   as 'Qtd Pendente'
    ,Valorunitário as'Valor unitário;'
    ,Imposto       as 'Imposto'		  
    ,Total         as'Total;'
    ,DocDate 
    ,BPLId 
    ,SlpCode 
	,ObjType
	   from #Tmp 
	  where #Tmp.DtLancamento between @dataInicial and @dataFinal 	
	    and ( @filial = #Tmp.BPLId or @filial = 0 )	 	
		and ( @Status = 'T' or @Status = #Tmp.status)
			 
	 drop table #tmp
END
GO


exec spcCVAItensPedidoCompra '2016-01-01','2019-01-01',0,'T'

---------------------------------- filtros ------------------------------------------------------------------------------

-- Filiais
 -- @select bplId,BPLName from OBPL union all select 0 ,'Todos' order by 1

-- Comprdor
 -- @select SlpCode, SlpName from OSLP union all select 0, 'Todos' order by 1

-- Status
 -- select 'T','Todos'union all select 'A','Aberto' union all select'C','Cancelado' union all select'P','Recebido Parcial' union all select 'R','Recebido Total'  
 
 -- Item
  -- @select Itemcode, ItemName from oitm union all select '*','Todos' order by 1

  -- Fornecedor
    -- @select Cardcode, Cardname from ocrd where cardtype = 'S' union all select '*', 'Todos' order by 1