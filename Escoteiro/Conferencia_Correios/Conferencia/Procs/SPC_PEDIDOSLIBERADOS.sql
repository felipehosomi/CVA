ALTER PROCEDURE SPC_PEDIDOSLIBERADOS
(
	 Status int,
	 Filial int,
	 TipoEnvio int,
	 UfCliente varchar(10),
	 DataDe date,
	 DataAte date
)
as 
begin	
	 CREATE LOCAL TEMPORARY TABLE
	 #PedidosLiberados
	 (
	 	 N_Pedido int
	 	,Origem varchar(50)
	 	,Cliente nvarchar(250)
	 	,NomeFantasia nvarchar(250)
	 	,Data_Pedido date
	 	,Total_Pedido decimal(19,2)
	 	,Forma_Pagamento nvarchar(250)
	 	,Transportadora nvarchar(250)
	 	,Status nvarchar(50)
	 );
 
	insert into #PedidosLiberados
	select distinct ORDR."DocNum" as "Nº Pedido", 
		   case ORDR."U_OrigemPedido" when 1 then 'Pedido E-Commerce'
	            					  when 2 then 'Pedido Interno'
	            					  when 3 then 'Distribuidor'
	       else 
	       		'Pedido Interno'     
	       end as "Origem"
	       ,OCRD."CardName" as "Cliente"
     	   , OCRD."CardFName" as "Nome Fantasia"	       
	       ,ORDR."DocDate" as "Data Pedido"
	       ,TO_DECIMAL(ORDR."DocTotal") as "Total Pedido"
	       , case when ORDR."PeyMethod" = 'R_BOLETO' then 'Boleto'
	            when ORDR."PeyMethod" = 'R_CARTAO' then 'Cartão'
	       end as "Forma Pagamento"
	       ,case when  (OSHP."TrnspCode" = 1) then 'SEDEX POR CONTA DO REMETENTE' 
				 when  (OSHP."TrnspCode" = 2) then 'PAC POR CONTA DO REMETENTE' 
				 when  (OSHP."TrnspCode" = 4) then 'RETIRA LOJA'
				 when  (OSHP."TrnspCode" = 6) then 'SEDEX POR CONTA DO DESTINATÁRIO'
				 when  (OSHP."TrnspCode" = 7) then 'PAC POR CONTA DO DESTINATÁRIO'	
				 when  (OSHP."TrnspCode" = 9) then 'SEDEX POR CONTA DO DESTINATÁRIO'
				 when  (OSHP."TrnspCode" = 10) then 'PAC POR CONTA DO DESTINATÁRIO'	  	  	  
				 else (select "CardName" From OCRD where OCRD."CardCode" = RDR12."Carrier")
				 end as "Transportadora"
	       , 'Liberado' as "Status Pedido"
	  from ORDR
	 inner join RDR1 on ORDR."DocEntry" = RDR1."DocEntry"
	 inner join DPI1 on ORDR."DocEntry" = DPI1."BaseEntry" 
	   and ORDR."ObjType" = DPI1."BaseType"
	   --and RDR1."LineNum" = DPI1."BaseLine"
	 inner join ODPI on DPI1."DocEntry" = ODPI."DocEntry"  
	   and ODPI."CANCELED" = 'N' 
	 inner join OCRD on ORDR."CardCode" = OCRD."CardCode" 
	 left join OSHP  on OSHP."TrnspCode" = ORDR."TrnspCode"
	 inner join OITM on RDR1."ItemCode" = OITM."ItemCode"
	 inner join RDR12 on ORDR."DocEntry" = RDR12."DocEntry"
	 inner join RCT2 on DPI1."DocEntry" = RCT2."DocEntry" And RCT2."InvType" = 203 
	 inner join ORCT on ORCT."DocEntry" = RCT2."DocNum" And ORCT."Canceled" = 'N'
	 inner join OITW on OITM."ItemCode"  = OITW."ItemCode"
	 where ORDR."CANCELED" = 'N'
	   and ORDR."U_CVA_Status_Picking" <> 3
	   and ORDR."U_CVA_Picking" = 'N' 
	   and (OITW."OnHand" - OITW."IsCommited") >= 0
	   and RDR1."WhsCode" = OITW."WhsCode"
	   and (ifnull(ORDR."U_OrigemPedido",2) = :Status or :Status = 0)
	   and (ORDR."BPLId" = :Filial or :Filial = 0)
	   and (ORDR."TrnspCode" = :TipoEnvio or :TipoEnvio = 0)
	   and (RDR12."State" = :UfCliente or :UfCliente = '0')
	   and ORDR."DocDate" between :DataDe and :DataAte;
 	   	
	 select distinct N_Pedido 
	 		,Origem  
	 		,Cliente  
	 		,NomeFantasia
	 		,Data_Pedido 
	 		,Total_Pedido
	 		,Forma_Pagamento
	 		,Transportadora	 		
	 		,Status
	   from #PedidosLiberados
	   order by 1;
   
    drop table #PedidosLiberados;
end;