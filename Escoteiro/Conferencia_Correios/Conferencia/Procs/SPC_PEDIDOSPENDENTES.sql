ALTER PROCEDURE SPC_PEDIDOSPENDENTES
(
	 Status int
	,Filial int
	,TipoEnvio int
	,UfCliente varchar(10)
	,DataDe date
	,DataAte date
)

as 
begin

CREATE LOCAL TEMPORARY TABLE
 #PedidosPendentes
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
 	,Quantidade_Estoque int
 );

insert into #PedidosPendentes
Select distinct 
       T0."DocNum" as "Nº Pedido"
      ,case when T0."U_OrigemPedido" = 1 then 'Pedido E-Commerce'
            when T0."U_OrigemPedido" = 2 then 'Pedido Interno'
            when T0."U_OrigemPedido" = 3 then 'Distribuidor'
       else 'Pedido Interno'
       end as "Origem"
      ,T8."CardName" as "Cliente"
      ,T8."CardFName" as "Nome Fantasia"      
      ,T0."DocDate" as "Data Pedido"
      ,T0."DocTotal" as "Total Pedido"
      , case when T0."PeyMethod" = 'R_BOLETO' then 'Boleto'
	         when T0."PeyMethod" = 'R_CARTAO' then 'Cartão'
	     end as "Forma Pagamento"
      ,case when  (T0."TrnspCode" = 1) then 'SEDEX POR CONTA DO REMETENTE' 
				 when  (T0."TrnspCode" = 2) then 'PAC POR CONTA DO REMETENTE' 
				 when  (T0."TrnspCode" = 4) then 'RETIRA LOJA'
				 when  (T0."TrnspCode" = 6) then 'SEDEX POR CONTA DO DESTINATÁRIO'
				 when  (T0."TrnspCode" = 7) then 'PAC POR CONTA DO DESTINATÁRIO'
				 when  (T0."TrnspCode" = 9) then 'SEDEX POR CONTA DO DESTINATÁRIO'
				 when  (T0."TrnspCode" = 10) then 'PAC POR CONTA DO DESTINATÁRIO'	  	  	  
				 else (select "CardName" From OCRD where OCRD."CardCode" = T10."Carrier")
				 end as "Transportadora" 
      ,'Pendente Financeiro' as "Status Pedido"
      ,0 as "Quantidade Estoque"
  from ORDR T0 
 inner join RDR1  T1 on T0."DocEntry" = T1."DocEntry"
 inner join OCRD  T8  on T0."CardCode"  = T8."CardCode" 
 inner join OITM  T9  on T1."ItemCode"  = T9."ItemCode"
 inner join RDR12 T10 on T0."DocEntry"  = T10."DocEntry" 
  --left join DPI1  T6 on T0."DocEntry" = T6."BaseEntry" 
   --and T0."ObjType" = T6."BaseType"
   --and T1."LineNum" = T6."BaseLine"
  --left join ODPI  T7  on T6."DocEntry"  = T7."DocEntry"  
  left join OSHP  T11 on T11."TrnspCode" = T0."TrnspCode" 
   where T0."CANCELED" = 'N'
   --and T7."DocStatus" <> 'C'
   and ifnull(T0."U_CVA_Status_Picking",2) <> 3
   and ifnull(T0."U_CVA_Picking",'N') = 'N'
   and not exists( Select DPI1."DocEntry" From DPI1
   inner Join ODPI on ODPI."DocEntry"  = DPI1."DocEntry" 
   inner join RCT2 On DPI1."DocEntry" = RCT2."DocEntry" And RCT2."InvType" = 203
   inner join ORCT  T12 on T12."DocEntry" = RCT2."DocNum" And T12."Canceled" = 'N'
   where T0."DocEntry" = DPI1."BaseEntry" and T0."ObjType" = DPI1."BaseType")
   and (ifnull(T0."U_OrigemPedido",2) = :Status or :Status = 0)
   and (T0."BPLId" = :Filial or :Filial = 0)
   and (T0."TrnspCode" = :TipoEnvio or :TipoEnvio = 0)
   and ((select "State" from CRD1 where T0."CardCode" = CRD1."CardCode" and "AdresType" = 'S' and "Address" = T8."ShipToDef") = :UfCliente or :UfCliente = '0')
   and T0."DocDate" between :DataDe and :DataAte
    
union all

Select distinct 
       T0."DocNum" as "Nº Pedido"
      ,case when T0."U_OrigemPedido" = 1 then 'Pedido E-Commerce'
            when T0."U_OrigemPedido" = 2 then 'Pedido Interno'
            when T0."U_OrigemPedido" = 3 then 'Distribuidor'
        else 'Pedido Interno'
       end as "Origem"
      ,T8."CardName" as "Cliente"
      ,T8."CardFName" as "Nome Fantasia"            
      ,T0."DocDate" as "Data Pedido"
      ,T0."DocTotal" as "Total Pedido"
      ,case when T0."PeyMethod" = 'R_BOLETO' then 'Boleto'
	        when T0."PeyMethod" = 'R_CARTAO' then 'Cartão'
	    end as "Forma Pagamento"
      ,case when  (T0."TrnspCode" = 1) then 'SEDEX POR CONTA DO REMETENTE' 
				 when  (T0."TrnspCode" = 2) then 'PAC POR CONTA DO REMETENTE' 
				 when  (T0."TrnspCode" = 4) then 'RETIRA LOJA'
				 when  (T0."TrnspCode" = 6) then 'SEDEX POR CONTA DO DESTINATÁRIO'
				 when  (T0."TrnspCode" = 7) then 'PAC POR CONTA DO DESTINATÁRIO'
				 when  (T0."TrnspCode" = 9) then 'SEDEX POR CONTA DO DESTINATÁRIO'
				 when  (T0."TrnspCode" = 10) then 'PAC POR CONTA DO DESTINATÁRIO'
				 	  	  	  
				 else (select "CardName" From OCRD where OCRD."CardCode" = T10."Carrier")
				 end as "Transportadora"
      ,'Pendente Estoque' as "Status Pedido"
	  ,(T11."OnHand" - T11."IsCommited") as "Quantidade Estoque"
  from ORDR T0
 inner join RDR1  T1 on T0."DocEntry" = T1."DocEntry" 
 inner join OCRD  T8  on T0."CardCode"  = T8."CardCode" 
 inner join OITM  T9  on T1."ItemCode"  = T9."ItemCode"
 inner join RDR12 T10 on T0."DocEntry"  = T10."DocEntry"
 inner join OITW  T11 on T9."ItemCode"  = T11."ItemCode"
  left join DPI1  T6 on T0."DocEntry" = T6."BaseEntry" 
   and T0."ObjType" = T6."BaseType"
   and T1."LineNum" = T6."BaseLine"
  left join ODPI  T7  on T6."DocEntry"  = T7."DocEntry"   
  left join OSHP  T12 on T12."TrnspCode" = T0."TrnspCode" 
 where T0."CANCELED" = 'N'
   --and T7."DocStatus" <> 'C'
   and ifnull(T0."U_CVA_Status_Picking",2) <> 3
   and ifnull(T0."U_CVA_Picking",'N') = 'N'
   --and not exists(select RCT2."DocEntry" from RCT2 inner join ORCT  T12 on T12."DocEntry" = RCT2."DocNum" where T6."DocEntry" = RCT2."DocEntry" And RCT2."InvType" = 203 And T12."Canceled" = 'N')
   and (T11."OnHand" - T11."IsCommited") <= 0
   and T1."WhsCode" = T11."WhsCode"
   and (ifnull(T0."U_OrigemPedido",2) = :Status or :Status = 0)
   and (T0."BPLId" = :Filial or :Filial = 0)
   and (T0."TrnspCode" = :TipoEnvio or :TipoEnvio = 0)
   and ((select "State" from CRD1 where T0."CardCode" = CRD1."CardCode" and "AdresType" = 'S' and "Address" = T8."ShipToDef") = :UfCliente or :UfCliente = '0')
   and T0."DocDate" between :DataDe and :DataAte;
--   and T1."WhsCode" = T11."WhsCode";
 

 
 select distinct N_Pedido 
 		,Origem  
 		,Cliente  
 		,NomeFantasia
 		,Data_Pedido 
 		,Total_Pedido
 		,Forma_Pagamento
 		,Transportadora
 		,case when ((select count(*) from #PedidosPendentes T1 where T0.N_Pedido = T1.N_Pedido and T1.Status = 'Pendente Financeiro') > 0)
 		then 'Pendente Financeiro' else 'Pendente Estoque' end as "Status"
   from #PedidosPendentes T0
   order by 1;
   
    drop table #PedidosPendentes;
 
end;