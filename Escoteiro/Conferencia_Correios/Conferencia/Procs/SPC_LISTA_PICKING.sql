

/*CREATE PROCEDURE SPC_LISTA_PICKING
(
	 DataInicial date
	,DataFinal   date
	,Filial      int
	,Status      int
	,Transp      nvarchar(50)
	,N_Pedido    int
)
as 
begin 
      -- Todos
	if :Status = 0 then	
		select distinct ORDR."DocNum"
				  ,COALESCE(ORDR."U_CVA_Increment_id",0) as "PedidoMagento"		   				
				  ,ORDR."DocDate"
				  ,case when ORDR."U_OrigemPedido"  = 1 then 'Pedido E-Commerce'
				        when ORDR."U_OrigemPedido"  = 2 then 'Pedido Interno'
				        when ORDR."U_OrigemPedido"  = 3 then 'Distribuidor'
				   end as "Origem"
				  ,OCRD."CardName" as "Cliente"
				  ,OCRD."E_Mail"
				  ,OSHP."TrnspName" as "TipoEntrega"
				  ,case when (OSHP."TrnspCode" = 1) then 'SEDEX' 
				  when  (OSHP."TrnspCode" = 2) then 'PAC' 
  				  when  (OSHP."TrnspCode" = 4) then 'RETIRA LOJA'
				  else (select "CardName" From OCRD where OCRD."CardCode" = RDR12."Carrier") end as "Transportadora"
				  ,(select "CardFName" From OCRD where OCRD."CardCode" = RDR12."Carrier") as "NomeFantasia"
				  ,RDR1."ShipDate" as "DataEntrega"
				  ,ORDR."U_CVA_Aber_1Conf" as "AberturaPicking"
				  ,ORDR."U_CVA_User_1Conf" as "UsuarioPicking"
                  ,COALESCE((Select Case When(ifnull(conf."CVA_STATUS_1",0) <> 1) Then 'Iniciado/Conf 1' Else 'Iniciado/Conf 2' End From "@CVA_CONFERENCIA" conf Where ORDR."DocNum" = conf."CVA_DOCENTRY"),'Iniciado/Conf1') as "StatusPicking"
				  ,case when (ORDR."U_CVA_Separacao_Impresso" IS NULL OR ORDR."U_CVA_Separacao_Impresso" = 2) then 'Não Impresso' 
				  		when ORDR."U_CVA_Separacao_Impresso" = 1 then 'Impresso' 
				  end as "Impresso"
				  ,'' as "CodigoRastreio"		
			  from ORDR
			 inner join RDR1  on ORDR."DocEntry"  = RDR1."DocEntry"
			 inner join OCRD  on ORDR."CardCode"  = OCRD."CardCode"
			  left join OSHP  on OSHP."TrnspCode" = ORDR."TrnspCode"
			  left join RDR12 on ORDR."DocEntry"  = RDR12."DocEntry"
			 where ORDR."U_CVA_Picking" = 'Y'
			   and ((:N_Pedido = 0 and ORDR."DocDate" between :DataInicial and :DataFinal) or (ORDR."DocNum" = :N_Pedido))
			   and (ORDR."BPLId" = :Filial or :Filial = 0)
			   and (ORDR."TrnspCode" =:Transp or :Transp = 0)

			   and  ORDR."U_CVA_Status_Picking" = 1
			   and ORDR."DocStatus" <> 'C'

		union all  
		   				select distinct ORDR."DocNum"
				  	  ,COALESCE(ORDR."U_CVA_Increment_id",0) as "PedidoMagento"		   				
					  ,ORDR."DocDate"
					  ,case when ORDR."U_OrigemPedido"  = 1 then 'Pedido E-Commerce'
					        when ORDR."U_OrigemPedido"  = 2 then 'Pedido Interno'
					        when ORDR."U_OrigemPedido"  = 3 then 'Distribuidor'
					   end as "Origem"
					  ,OCRD."CardName" as "Cliente"
					  ,OCRD."E_Mail"
					  ,OSHP."TrnspName" as "TipoEntrega"
				      ,case when (OSHP."TrnspCode" = 1) then 'SEDEX' 
				      when  (OSHP."TrnspCode" = 2) then 'PAC' 
       				  when  (OSHP."TrnspCode" = 4) then 'RETIRA LOJA'
				      else (select "CardName" From OCRD where OCRD."CardCode" = RDR12."Carrier") end as "Transportadora"
				      ,(select "CardFName" From OCRD where OCRD."CardCode" = RDR12."Carrier") as "NomeFantasia"				      
					  ,RDR1."ShipDate" as "DataEntrega"
					  ,ORDR."U_CVA_Aber_1Conf" as "AberturaPicking"
					  ,ORDR."U_CVA_User_1Conf" as "UsuarioPicking"
					  ,'Pendente' as "StatusPicking"
				  ,case when (ORDR."U_CVA_Separacao_Impresso" IS NULL OR ORDR."U_CVA_Separacao_Impresso" = 2) then 'Não Impresso' 
				  		when ORDR."U_CVA_Separacao_Impresso" = 1 then 'Impresso' 
				  end as "Impresso"	
				  ,'' as "CodigoRastreio"		
				  from ORDR
				 inner join RDR1  on ORDR."DocEntry"  = RDR1."DocEntry"
				 inner join OCRD  on ORDR."CardCode"  = OCRD."CardCode"
				  left join OSHP  on OSHP."TrnspCode" = ORDR."TrnspCode"
				  left join RDR12 on ORDR."DocEntry"  = RDR12."DocEntry"
				 where ORDR."U_CVA_Picking" = 'Y'
 				   and ((:N_Pedido = 0 and ORDR."DocDate" between :DataInicial and :DataFinal) or (ORDR."DocNum" = :N_Pedido))
				   and (ORDR."BPLId" = :Filial or :Filial = 0)
				   and (ORDR."TrnspCode" =:Transp or :Transp = 0)
				   and (ORDR."U_CVA_Aber_1Conf" is null or ORDR."U_CVA_Aber_1Conf" = '')
				   and (ORDR."U_CVA_User_1Conf" is null or ORDR."U_CVA_User_1Conf" = '')
				   and  ORDR."U_CVA_Status_Picking" = 2
				   and ORDR."DocStatus" <> 'C'
		 union all
		 	select distinct ORDR."DocNum"
		 			  ,COALESCE(ORDR."U_CVA_Increment_id",0) as "PedidoMagento"		   				
					  ,ORDR."DocDate"
					  ,case when ORDR."U_OrigemPedido"  = 1 then 'Pedido E-Commerce'
					        when ORDR."U_OrigemPedido"  = 2 then 'Pedido Interno'
					        when ORDR."U_OrigemPedido"  = 3 then 'Distribuidor'
					   end as "Origem"
					  ,OCRD."CardName" as "Cliente"
					  ,OCRD."E_Mail"
					  ,OSHP."TrnspName" as "TipoEntrega"
				      ,case when (OSHP."TrnspCode" = 1) then 'SEDEX' 
				      when  (OSHP."TrnspCode" = 2) then 'PAC' 
	  				  when  (OSHP."TrnspCode" = 4) then 'RETIRA LOJA'				      
				      else (select "CardName" From OCRD where OCRD."CardCode" = RDR12."Carrier") end as "Transportadora"
					  ,(select "CardFName" From OCRD where OCRD."CardCode" = RDR12."Carrier") as "NomeFantasia"				      
				 	  ,RDR1."ShipDate" as "DataEntrega"
					  ,ORDR."U_CVA_Aber_1Conf" as "AberturaPicking"
					  ,ORDR."U_CVA_User_1Conf" as "UsuarioPicking"
					  ,'Finalizado' as "StatusPicking"
				  ,'Impresso' as "Impresso"	
				  ,OINV."U_CVA_CodRastreio" as "CodigoRastreio"		
				  from ORDR
				 inner join RDR1  on ORDR."DocEntry"  = RDR1."DocEntry"
				 inner join OCRD  on ORDR."CardCode"  = OCRD."CardCode"
				  left join OSHP  on OSHP."TrnspCode" = ORDR."TrnspCode"
				  left join RDR12 on ORDR."DocEntry"  = RDR12."DocEntry"
				 inner join INV1  on INV1."BaseType"  = ORDR."ObjType" 
				        and INV1."BaseLine" = RDR1."LineNum" 
				        and INV1."BaseEntry" = ORDR."DocEntry"	
				 inner join OINV on OINV."DocEntry" =  INV1."DocEntry" And OINV."CANCELED" = 'N'			  
				 where ORDR."U_CVA_Picking" = 'Y'
				   and ((:N_Pedido = 0 and ORDR."DocDate" between :DataInicial and :DataFinal) or (ORDR."DocNum" = :N_Pedido))
				   and (ORDR."BPLId" = :Filial or :Filial = 0)
				   and (ORDR."TrnspCode" =:Transp or :Transp = 0)
				   and  ORDR."U_CVA_Status_Picking" = 3;
		 		   --and ORDR."DocStatus" <> 'C';
		   
		   
		   
    end if;
    
    -- Iniciado
	if :Status = 1 then	
			select distinct ORDR."DocNum"
				  ,COALESCE(ORDR."U_CVA_Increment_id",0) as "PedidoMagento"		   				
				  ,ORDR."DocDate"
				  ,case when ORDR."U_OrigemPedido"  = 1 then 'Pedido E-Commerce'
				        when ORDR."U_OrigemPedido"  = 2 then 'Pedido Interno'
				        when ORDR."U_OrigemPedido"  = 3 then 'Distribuidor'
				   end as "Origem"
				  ,OCRD."CardName" as "Cliente"
				  ,OCRD."E_Mail"
				  ,OSHP."TrnspName" as "TipoEntrega"
				  ,case when (OSHP."TrnspCode" = 1) then 'SEDEX' 
				  when  (OSHP."TrnspCode" = 2) then 'PAC' 
  				  when  (OSHP."TrnspCode" = 4) then 'RETIRA LOJA'				  
				  else (select "CardName" From OCRD where OCRD."CardCode" = RDR12."Carrier") end as "Transportadora"
				  ,(select "CardFName" From OCRD where OCRD."CardCode" = RDR12."Carrier") as "NomeFantasia"				  
				  ,RDR1."ShipDate" as "DataEntrega"
				  ,ORDR."U_CVA_Aber_1Conf" as "AberturaPicking"
				  ,ORDR."U_CVA_User_1Conf" as "UsuarioPicking"
                  ,COALESCE((Select Case When(ifnull(conf."CVA_STATUS_1",0) <> 1) Then 'Iniciado/Conf 1' Else 'Iniciado/Conf 2' End From "@CVA_CONFERENCIA" conf Where ORDR."DocNum" = conf."CVA_DOCENTRY"),'Iniciado/Conf1') as "StatusPicking"
				  ,case when (ORDR."U_CVA_Separacao_Impresso" IS NULL OR ORDR."U_CVA_Separacao_Impresso" = 2) then 'Não Impresso' 
				  		when ORDR."U_CVA_Separacao_Impresso" = 1 then 'Impresso' 
				  end as "Impresso"	
				  ,'' as "CodigoRastreio"		
			  from ORDR
			 inner join RDR1  on ORDR."DocEntry"  = RDR1."DocEntry"
			 inner join OCRD  on ORDR."CardCode"  = OCRD."CardCode"
			  left join OSHP  on OSHP."TrnspCode" = ORDR."TrnspCode"
			  left join RDR12 on ORDR."DocEntry"  = RDR12."DocEntry"
			 where ORDR."U_CVA_Picking" = 'Y'
			   and ((:N_Pedido = 0 and ORDR."DocDate" between :DataInicial and :DataFinal) or (ORDR."DocNum" = :N_Pedido))
			   and (ORDR."BPLId" = :Filial or :Filial = 0)
			   and (ORDR."TrnspCode" =:Transp or :Transp = 0)
			   and  ORDR."U_CVA_Status_Picking" = 1
			   and ORDR."DocStatus" <> 'C';
	    end if;
	    
	    -- Pendentes
	    if :Status = 2 then	
				select distinct ORDR."DocNum"
					  ,COALESCE(ORDR."U_CVA_Increment_id",0) as "PedidoMagento"		   				
					  ,ORDR."DocDate"
					  ,case when ORDR."U_OrigemPedido"  = 1 then 'Pedido E-Commerce'
					        when ORDR."U_OrigemPedido"  = 2 then 'Pedido Interno'
					        when ORDR."U_OrigemPedido"  = 3 then 'Distribuidor'
					   end as "Origem"
					  ,OCRD."CardName" as "Cliente"
					  ,OCRD."E_Mail"
					  ,OSHP."TrnspName" as "TipoEntrega"
				      ,case when (OSHP."TrnspCode" = 1) then 'SEDEX' 
				      when  (OSHP."TrnspCode" = 2) then 'PAC' 
	  				  when  (OSHP."TrnspCode" = 4) then 'RETIRA LOJA'				      
				      else (select "CardName" From OCRD where OCRD."CardCode" = RDR12."Carrier") end as "Transportadora"
					  ,(select "CardFName" From OCRD where OCRD."CardCode" = RDR12."Carrier") as "NomeFantasia"				      
					  ,RDR1."ShipDate" as "DataEntrega"
					  ,ORDR."U_CVA_Aber_1Conf" as "AberturaPicking"
					  ,ORDR."U_CVA_User_1Conf" as "UsuarioPicking"
					  ,'Pendente' as "StatusPicking"
				  ,case when (ORDR."U_CVA_Separacao_Impresso" IS NULL OR ORDR."U_CVA_Separacao_Impresso" = 2) then 'Não Impresso' 
				  		when ORDR."U_CVA_Separacao_Impresso" = 1 then 'Impresso' 
				  end as "Impresso"
				  ,'' as "CodigoRastreio"			
				  from ORDR
				 inner join RDR1  on ORDR."DocEntry"  = RDR1."DocEntry"
				 inner join OCRD  on ORDR."CardCode"  = OCRD."CardCode"
				  left join OSHP  on OSHP."TrnspCode" = ORDR."TrnspCode"
				  left join RDR12 on ORDR."DocEntry"  = RDR12."DocEntry"
				 where ORDR."U_CVA_Picking" = 'Y'
				   and ((:N_Pedido = 0 and ORDR."DocDate" between :DataInicial and :DataFinal) or (ORDR."DocNum" = :N_Pedido))
				   and (ORDR."BPLId" = :Filial or :Filial = 0)
				   and (ORDR."TrnspCode" =:Transp or :Transp = 0)
				   and (ORDR."U_CVA_Aber_1Conf" is null or ORDR."U_CVA_Aber_1Conf" = '')
				   and (ORDR."U_CVA_User_1Conf" is null or ORDR."U_CVA_User_1Conf" = '')
 			       and  ORDR."U_CVA_Status_Picking" = 2
 			       and ORDR."DocStatus" <> 'C';
		    end if;
		    
		    -- Finalizados
		    if :Status = 3 then	
				select distinct ORDR."DocNum"
				  	  ,COALESCE(ORDR."U_CVA_Increment_id",0) as "PedidoMagento"		   				
					  ,ORDR."DocDate"
					  ,case when ORDR."U_OrigemPedido"  = 1 then 'Pedido E-Commerce'
					        when ORDR."U_OrigemPedido"  = 2 then 'Pedido Interno'
					        when ORDR."U_OrigemPedido"  = 3 then 'Distribuidor'
					   end as "Origem"
					  ,OCRD."CardName" as "Cliente"
					  ,OCRD."E_Mail"
					  ,OSHP."TrnspName" as "TipoEntrega"
				      ,case when (OSHP."TrnspCode" = 1) then 'SEDEX' 
				      when  (OSHP."TrnspCode" = 2) then 'PAC' 
	  				  when  (OSHP."TrnspCode" = 4) then 'RETIRA LOJA'				      
				      else (select "CardName" From OCRD where OCRD."CardCode" = RDR12."Carrier") end as "Transportadora"
					  ,(select "CardFName" From OCRD where OCRD."CardCode" = RDR12."Carrier") as "NomeFantasia"				      
					  ,RDR1."ShipDate" as "DataEntrega"
					  ,ORDR."U_CVA_Aber_1Conf" as "AberturaPicking"
					  ,ORDR."U_CVA_User_1Conf" as "UsuarioPicking"
					  ,'Finalizado' as "StatusPicking"
				  	  ,'Impresso' as "Impresso"	
				  	  ,OINV."U_CVA_CodRastreio" as "CodigoRastreio"			
				  from ORDR
				 inner join RDR1  on ORDR."DocEntry"  = RDR1."DocEntry"
				 inner join OCRD  on ORDR."CardCode"  = OCRD."CardCode"
				  left join OSHP  on OSHP."TrnspCode" = ORDR."TrnspCode"
				  left join RDR12 on ORDR."DocEntry"  = RDR12."DocEntry"
				 inner join INV1  on INV1."BaseType"  = ORDR."ObjType" 
				        and INV1."BaseLine" = RDR1."LineNum" 
				        and INV1."BaseEntry" = ORDR."DocEntry"
				 inner join OINV on OINV."DocEntry" =  INV1."DocEntry" And OINV."CANCELED" = 'N'        				  
				 where ORDR."U_CVA_Picking" = 'Y'
				   and ((:N_Pedido = 0 and ORDR."DocDate" between :DataInicial and :DataFinal) or (ORDR."DocNum" = :N_Pedido))
				   and (ORDR."BPLId" = :Filial or :Filial = 0)
				   and (ORDR."TrnspCode" =:Transp or :Transp = 0)
					   and  ORDR."U_CVA_Status_Picking" = 3;
				   --and ORDR."DocStatus" <> 'C';
	 		end if;
 end;

 */

 alter PROCEDURE SPC_LISTA_PICKING
(
	 DataInicial date
	,DataFinal   date
	,Filial      int
	,Status      int
	,Transp      nvarchar(50)
	,N_Pedido    int
)
as 
begin 
      -- Todos
	if :Status = 0 then	
		select distinct ORDR."DocNum"
				  ,COALESCE(ORDR."U_CVA_Increment_id",0) as "PedidoMagento"		   				
				  ,ORDR."DocDate"
				  ,case when ORDR."U_OrigemPedido"  = 1 then 'Pedido E-Commerce'
				        when ORDR."U_OrigemPedido"  = 2 then 'Pedido Interno'
				        when ORDR."U_OrigemPedido"  = 3 then 'Distribuidor'
				   end as "Origem"
				  ,OCRD."CardName" as "Cliente"
				  ,OCRD."E_Mail"
				  ,OSHP."TrnspName" as "TipoEntrega"
				  ,case when  (OSHP."TrnspCode" = 1) then 'SEDEX POR CONTA DO REMETENTE' 
							when  (OSHP."TrnspCode" = 2) then 'PAC POR CONTA DO REMETENTE' 
							--when  (OSHP."TrnspCode" = 4) then 'RETIRA LOJA'
							when  (OSHP."TrnspCode" = 6) then 'SEDEX POR CONTA DO DESTINATÁRIO'
							when  (OSHP."TrnspCode" = 7) then 'PAC POR CONTA DO DESTINATÁRIO'	  	  	  
							when  (OSHP."TrnspCode" = 9) then 'SEDEX POR CONTA DO DESTINATÁRIO'
							when  (OSHP."TrnspCode" = 10) then 'PAC POR CONTA DO DESTINATÁRIO'	  	  	  
						else (select "CardName" From OCRD where OCRD."CardCode" = RDR12."Carrier")
						end as "Transportadora"
				  ,(select "CardFName" From OCRD where OCRD."CardCode" = RDR12."Carrier") as "NomeFantasia"
				  ,RDR1."ShipDate" as "DataEntrega"
				  ,ORDR."U_CVA_Aber_1Conf" as "AberturaPicking"
				  ,ORDR."U_CVA_User_1Conf" as "UsuarioPicking"
                  ,COALESCE((Select Case When(ifnull(conf."CVA_STATUS_1",0) <> 1) Then 'Iniciado/Conf 1' Else 'Iniciado/Conf 2' End From "@CVA_CONFERENCIA" conf Where ORDR."DocNum" = conf."CVA_DOCENTRY"),'Iniciado/Conf1') as "StatusPicking"
				  ,case when (ORDR."U_CVA_Separacao_Impresso" IS NULL OR ORDR."U_CVA_Separacao_Impresso" = 2) then 'Não Impresso' 
				  		when ORDR."U_CVA_Separacao_Impresso" = 1 then 'Impresso' 
				  end as "Impresso"
				  ,'' as "CodigoRastreio"		
			  from ORDR
			 inner join RDR1  on ORDR."DocEntry"  = RDR1."DocEntry"
			 inner join OCRD  on ORDR."CardCode"  = OCRD."CardCode"
			  left join OSHP  on OSHP."TrnspCode" = ORDR."TrnspCode"
			  left join RDR12 on ORDR."DocEntry"  = RDR12."DocEntry"
			 where ORDR."U_CVA_Picking" = 'Y'
			   and ((:N_Pedido = 0 and ORDR."DocDate" between :DataInicial and :DataFinal) or (ORDR."DocNum" = :N_Pedido))
			   and (ORDR."BPLId" = :Filial or :Filial = 0)
			   and (ORDR."TrnspCode" =:Transp or :Transp = 0)

			   and  ORDR."U_CVA_Status_Picking" = 1
			   and ORDR."DocStatus" <> 'C'

		union all  
		   				select distinct ORDR."DocNum"
				  	  ,COALESCE(ORDR."U_CVA_Increment_id",0) as "PedidoMagento"		   				
					  ,ORDR."DocDate"
					  ,case when ORDR."U_OrigemPedido"  = 1 then 'Pedido E-Commerce'
					        when ORDR."U_OrigemPedido"  = 2 then 'Pedido Interno'
					        when ORDR."U_OrigemPedido"  = 3 then 'Distribuidor'
					   end as "Origem"
					  ,OCRD."CardName" as "Cliente"
					  ,OCRD."E_Mail"
					  ,OSHP."TrnspName" as "TipoEntrega"
				      ,case when  (OSHP."TrnspCode" = 1) then 'SEDEX POR CONTA DO REMETENTE' 
							when  (OSHP."TrnspCode" = 2) then 'PAC POR CONTA DO REMETENTE' 
							--when  (OSHP."TrnspCode" = 4) then 'RETIRA LOJA'
							when  (OSHP."TrnspCode" = 6) then 'SEDEX POR CONTA DO DESTINATÁRIO'
							when  (OSHP."TrnspCode" = 7) then 'PAC POR CONTA DO DESTINATÁRIO'	  	  	  
							when  (OSHP."TrnspCode" = 9) then 'SEDEX POR CONTA DO DESTINATÁRIO'
							when  (OSHP."TrnspCode" = 10) then 'PAC POR CONTA DO DESTINATÁRIO'	  	  	  
							else (select "CardName" From OCRD where OCRD."CardCode" = RDR12."Carrier")
						end as "Transportadora"
				      ,(select "CardFName" From OCRD where OCRD."CardCode" = RDR12."Carrier") as "NomeFantasia"				      
					  ,RDR1."ShipDate" as "DataEntrega"
					  ,ORDR."U_CVA_Aber_1Conf" as "AberturaPicking"
					  ,ORDR."U_CVA_User_1Conf" as "UsuarioPicking"
					  ,'Pendente' as "StatusPicking"
				  ,case when (ORDR."U_CVA_Separacao_Impresso" IS NULL OR ORDR."U_CVA_Separacao_Impresso" = 2) then 'Não Impresso' 
				  		when ORDR."U_CVA_Separacao_Impresso" = 1 then 'Impresso' 
				  end as "Impresso"	
				  ,'' as "CodigoRastreio"		
				  from ORDR
				 inner join RDR1  on ORDR."DocEntry"  = RDR1."DocEntry"
				 inner join OCRD  on ORDR."CardCode"  = OCRD."CardCode"
				  left join OSHP  on OSHP."TrnspCode" = ORDR."TrnspCode"
				  left join RDR12 on ORDR."DocEntry"  = RDR12."DocEntry"
				 where ORDR."U_CVA_Picking" = 'Y'
 				   and ((:N_Pedido = 0 and ORDR."DocDate" between :DataInicial and :DataFinal) or (ORDR."DocNum" = :N_Pedido))
				   and (ORDR."BPLId" = :Filial or :Filial = 0)
				   and (ORDR."TrnspCode" =:Transp or :Transp = 0)
				   and (ORDR."U_CVA_Aber_1Conf" is null or ORDR."U_CVA_Aber_1Conf" = '')
				   and (ORDR."U_CVA_User_1Conf" is null or ORDR."U_CVA_User_1Conf" = '')
				   and  ORDR."U_CVA_Status_Picking" = 2
				   and ORDR."DocStatus" <> 'C'
		 union all
		 	select distinct ORDR."DocNum"
		 			  ,COALESCE(ORDR."U_CVA_Increment_id",0) as "PedidoMagento"		   				
					  ,ORDR."DocDate"
					  ,case when ORDR."U_OrigemPedido"  = 1 then 'Pedido E-Commerce'
					        when ORDR."U_OrigemPedido"  = 2 then 'Pedido Interno'
					        when ORDR."U_OrigemPedido"  = 3 then 'Distribuidor'
					   end as "Origem"
					  ,OCRD."CardName" as "Cliente"
					  ,OCRD."E_Mail"
					  ,OSHP."TrnspName" as "TipoEntrega"
				      ,case when  (OSHP."TrnspCode" = 1) then 'SEDEX POR CONTA DO REMETENTE' 
							when  (OSHP."TrnspCode" = 2) then 'PAC POR CONTA DO REMETENTE' 
							--when  (OSHP."TrnspCode" = 4) then 'RETIRA LOJA'
							when  (OSHP."TrnspCode" = 6) then 'SEDEX POR CONTA DO DESTINATÁRIO'
							when  (OSHP."TrnspCode" = 7) then 'PAC POR CONTA DO DESTINATÁRIO'	  	  	  
							when  (OSHP."TrnspCode" = 9) then 'SEDEX POR CONTA DO DESTINATÁRIO'
							when  (OSHP."TrnspCode" = 10) then 'PAC POR CONTA DO DESTINATÁRIO'	  	  	  
							else (select "CardName" From OCRD where OCRD."CardCode" = RDR12."Carrier")
						end as "Transportadora"
					  ,(select "CardFName" From OCRD where OCRD."CardCode" = RDR12."Carrier") as "NomeFantasia"				      
				 	  ,RDR1."ShipDate" as "DataEntrega"
					  ,ORDR."U_CVA_Aber_1Conf" as "AberturaPicking"
					  ,ORDR."U_CVA_User_1Conf" as "UsuarioPicking"
					  ,'Finalizado' as "StatusPicking"
				  ,'Impresso' as "Impresso"	
				  ,OINV."U_CVA_CodRastreio" as "CodigoRastreio"		
				  from ORDR
				 inner join RDR1  on ORDR."DocEntry"  = RDR1."DocEntry"
				 inner join OCRD  on ORDR."CardCode"  = OCRD."CardCode"
				  left join OSHP  on OSHP."TrnspCode" = ORDR."TrnspCode"
				  left join RDR12 on ORDR."DocEntry"  = RDR12."DocEntry"
				 inner join INV1  on INV1."BaseType"  = ORDR."ObjType" 
				        and INV1."BaseLine" = RDR1."LineNum" 
				        and INV1."BaseEntry" = ORDR."DocEntry"	
				 inner join OINV on OINV."DocEntry" =  INV1."DocEntry" And OINV."CANCELED" = 'N'			  
				 where ORDR."U_CVA_Picking" = 'Y'
				   and ((:N_Pedido = 0 and ORDR."DocDate" between :DataInicial and :DataFinal) or (ORDR."DocNum" = :N_Pedido))
				   and (ORDR."BPLId" = :Filial or :Filial = 0)
				   and (ORDR."TrnspCode" =:Transp or :Transp = 0)
				   and  ORDR."U_CVA_Status_Picking" = 3;
		 		   --and ORDR."DocStatus" <> 'C';
		   
		   
		   
    end if;
    
    -- Iniciado
	if :Status = 1 then	
			select distinct ORDR."DocNum"
				  ,COALESCE(ORDR."U_CVA_Increment_id",0) as "PedidoMagento"		   				
				  ,ORDR."DocDate"
				  ,case when ORDR."U_OrigemPedido"  = 1 then 'Pedido E-Commerce'
				        when ORDR."U_OrigemPedido"  = 2 then 'Pedido Interno'
				        when ORDR."U_OrigemPedido"  = 3 then 'Distribuidor'
				   end as "Origem"
				  ,OCRD."CardName" as "Cliente"
				  ,OCRD."E_Mail"
				  ,OSHP."TrnspName" as "TipoEntrega"
				  ,case when  (OSHP."TrnspCode" = 1) then 'SEDEX POR CONTA DO REMETENTE' 
							when  (OSHP."TrnspCode" = 2) then 'PAC POR CONTA DO REMETENTE' 
							--when  (OSHP."TrnspCode" = 4) then 'RETIRA LOJA'
							when  (OSHP."TrnspCode" = 6) then 'SEDEX POR CONTA DO DESTINATÁRIO'
							when  (OSHP."TrnspCode" = 7) then 'PAC POR CONTA DO DESTINATÁRIO'	  	  	  
							when  (OSHP."TrnspCode" = 9) then 'SEDEX POR CONTA DO DESTINATÁRIO'
							when  (OSHP."TrnspCode" = 10) then 'PAC POR CONTA DO DESTINATÁRIO'	  	  	  
						else (select "CardName" From OCRD where OCRD."CardCode" = RDR12."Carrier")
						end as "Transportadora"
				  ,(select "CardFName" From OCRD where OCRD."CardCode" = RDR12."Carrier") as "NomeFantasia"				  
				  ,RDR1."ShipDate" as "DataEntrega"
				  ,ORDR."U_CVA_Aber_1Conf" as "AberturaPicking"
				  ,ORDR."U_CVA_User_1Conf" as "UsuarioPicking"
                  ,COALESCE((Select Case When(ifnull(conf."CVA_STATUS_1",0) <> 1) Then 'Iniciado/Conf 1' Else 'Iniciado/Conf 2' End From "@CVA_CONFERENCIA" conf Where ORDR."DocNum" = conf."CVA_DOCENTRY"),'Iniciado/Conf1') as "StatusPicking"
				  ,case when (ORDR."U_CVA_Separacao_Impresso" IS NULL OR ORDR."U_CVA_Separacao_Impresso" = 2) then 'Não Impresso' 
				  		when ORDR."U_CVA_Separacao_Impresso" = 1 then 'Impresso' 
				  end as "Impresso"	
				  ,'' as "CodigoRastreio"		
			  from ORDR
			 inner join RDR1  on ORDR."DocEntry"  = RDR1."DocEntry"
			 inner join OCRD  on ORDR."CardCode"  = OCRD."CardCode"
			  left join OSHP  on OSHP."TrnspCode" = ORDR."TrnspCode"
			  left join RDR12 on ORDR."DocEntry"  = RDR12."DocEntry"
			 where ORDR."U_CVA_Picking" = 'Y'
			   and ((:N_Pedido = 0 and ORDR."DocDate" between :DataInicial and :DataFinal) or (ORDR."DocNum" = :N_Pedido))
			   and (ORDR."BPLId" = :Filial or :Filial = 0)
			   and (ORDR."TrnspCode" =:Transp or :Transp = 0)
			   and  ORDR."U_CVA_Status_Picking" = 1
			   and ORDR."DocStatus" <> 'C';
	    end if;
	    
	    -- Pendentes
	    if :Status = 2 then	
				select distinct ORDR."DocNum"
					  ,COALESCE(ORDR."U_CVA_Increment_id",0) as "PedidoMagento"		   				
					  ,ORDR."DocDate"
					  ,case when ORDR."U_OrigemPedido"  = 1 then 'Pedido E-Commerce'
					        when ORDR."U_OrigemPedido"  = 2 then 'Pedido Interno'
					        when ORDR."U_OrigemPedido"  = 3 then 'Distribuidor'
					   end as "Origem"
					  ,OCRD."CardName" as "Cliente"
					  ,OCRD."E_Mail"
					  ,OSHP."TrnspName" as "TipoEntrega"
				      ,case when  (OSHP."TrnspCode" = 1) then 'SEDEX POR CONTA DO REMETENTE' 
							when  (OSHP."TrnspCode" = 2) then 'PAC POR CONTA DO REMETENTE' 
							--when  (OSHP."TrnspCode" = 4) then 'RETIRA LOJA'
							when  (OSHP."TrnspCode" = 6) then 'SEDEX POR CONTA DO DESTINATÁRIO'
							when  (OSHP."TrnspCode" = 7) then 'PAC POR CONTA DO DESTINATÁRIO'	  	  	  
							when  (OSHP."TrnspCode" = 9) then 'SEDEX POR CONTA DO DESTINATÁRIO'
							when  (OSHP."TrnspCode" = 10) then 'PAC POR CONTA DO DESTINATÁRIO'	  	  	  
							else (select "CardName" From OCRD where OCRD."CardCode" = RDR12."Carrier")
						end as "Transportadora"
					  ,(select "CardFName" From OCRD where OCRD."CardCode" = RDR12."Carrier") as "NomeFantasia"				      
					  ,RDR1."ShipDate" as "DataEntrega"
					  ,ORDR."U_CVA_Aber_1Conf" as "AberturaPicking"
					  ,ORDR."U_CVA_User_1Conf" as "UsuarioPicking"
					  ,'Pendente' as "StatusPicking"
				  ,case when (ORDR."U_CVA_Separacao_Impresso" IS NULL OR ORDR."U_CVA_Separacao_Impresso" = 2) then 'Não Impresso' 
				  		when ORDR."U_CVA_Separacao_Impresso" = 1 then 'Impresso' 
				  end as "Impresso"
				  ,'' as "CodigoRastreio"			
				  from ORDR
				 inner join RDR1  on ORDR."DocEntry"  = RDR1."DocEntry"
				 inner join OCRD  on ORDR."CardCode"  = OCRD."CardCode"
				  left join OSHP  on OSHP."TrnspCode" = ORDR."TrnspCode"
				  left join RDR12 on ORDR."DocEntry"  = RDR12."DocEntry"
				 where ORDR."U_CVA_Picking" = 'Y'
				   and ((:N_Pedido = 0 and ORDR."DocDate" between :DataInicial and :DataFinal) or (ORDR."DocNum" = :N_Pedido))
				   and (ORDR."BPLId" = :Filial or :Filial = 0)
				   and (ORDR."TrnspCode" =:Transp or :Transp = 0)
				   and (ORDR."U_CVA_Aber_1Conf" is null or ORDR."U_CVA_Aber_1Conf" = '')
				   and (ORDR."U_CVA_User_1Conf" is null or ORDR."U_CVA_User_1Conf" = '')
 			       and  ORDR."U_CVA_Status_Picking" = 2
 			       and ORDR."DocStatus" <> 'C';
		    end if;
		    
		    -- Finalizados
		    if :Status = 3 then	
				select distinct ORDR."DocNum"
				  	  ,COALESCE(ORDR."U_CVA_Increment_id",0) as "PedidoMagento"		   				
					  ,ORDR."DocDate"
					  ,case when ORDR."U_OrigemPedido"  = 1 then 'Pedido E-Commerce'
					        when ORDR."U_OrigemPedido"  = 2 then 'Pedido Interno'
					        when ORDR."U_OrigemPedido"  = 3 then 'Distribuidor'
					   end as "Origem"
					  ,OCRD."CardName" as "Cliente"
					  ,OCRD."E_Mail"
					  ,OSHP."TrnspName" as "TipoEntrega"
				      ,case when  (OSHP."TrnspCode" = 1) then 'SEDEX POR CONTA DO REMETENTE' 
							when  (OSHP."TrnspCode" = 2) then 'PAC POR CONTA DO REMETENTE' 
							--when  (OSHP."TrnspCode" = 4) then 'RETIRA LOJA'
							when  (OSHP."TrnspCode" = 6) then 'SEDEX POR CONTA DO DESTINATÁRIO'
							when  (OSHP."TrnspCode" = 7) then 'PAC POR CONTA DO DESTINATÁRIO'	  	  	  
							when  (OSHP."TrnspCode" = 9) then 'SEDEX POR CONTA DO DESTINATÁRIO'
							when  (OSHP."TrnspCode" = 10) then 'PAC POR CONTA DO DESTINATÁRIO'
							else (select "CardName" From OCRD where OCRD."CardCode" = RDR12."Carrier")
						end as "Transportadora"
					  ,(select "CardFName" From OCRD where OCRD."CardCode" = RDR12."Carrier") as "NomeFantasia"				      
					  ,RDR1."ShipDate" as "DataEntrega"
					  ,ORDR."U_CVA_Aber_1Conf" as "AberturaPicking"
					  ,ORDR."U_CVA_User_1Conf" as "UsuarioPicking"
					  ,'Finalizado' as "StatusPicking"
				  	  ,'Impresso' as "Impresso"	
				  	  ,OINV."U_CVA_CodRastreio" as "CodigoRastreio"			
				  from ORDR
				 inner join RDR1  on ORDR."DocEntry"  = RDR1."DocEntry"
				 inner join OCRD  on ORDR."CardCode"  = OCRD."CardCode"
				  left join OSHP  on OSHP."TrnspCode" = ORDR."TrnspCode"
				  left join RDR12 on ORDR."DocEntry"  = RDR12."DocEntry"
				 inner join INV1  on INV1."BaseType"  = ORDR."ObjType" 
				        and INV1."BaseLine" = RDR1."LineNum" 
				        and INV1."BaseEntry" = ORDR."DocEntry"
				 inner join OINV on OINV."DocEntry" =  INV1."DocEntry" And OINV."CANCELED" = 'N'        				  
				 where ORDR."U_CVA_Picking" = 'Y'
				   and ((:N_Pedido = 0 and ORDR."DocDate" between :DataInicial and :DataFinal) or (ORDR."DocNum" = :N_Pedido))
				   and (ORDR."BPLId" = :Filial or :Filial = 0)
				   and (ORDR."TrnspCode" =:Transp or :Transp = 0)
					   and  ORDR."U_CVA_Status_Picking" = 3;
				   --and ORDR."DocStatus" <> 'C';
	 		end if;
 end;

