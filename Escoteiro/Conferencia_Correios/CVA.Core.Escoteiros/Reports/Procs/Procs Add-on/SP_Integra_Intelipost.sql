ALTER Procedure SP_Integra_Intelipost
(
	 DocEntry int
)
 as 
Begin
select -- Parte I
       distinct T3."U_CVA_Increment_id" as "order_number"
      ,(SELECT SUM(Y0."LineTotal") 
          FROM INV3 Y0 INNER JOIN OEXD Y1 ON Y1."ExpnsCode" = Y0."ExpnsCode"
         WHERE Y0."DocEntry" = T3."DocEntry" AND Y1."ExpnsType" = 1     
         GROUP BY Y0."DocEntry", Y1."ExpnsCode" ) as "costumer_shipping_costs"
	   ,CASE WHEN (T3."U_OrigemPedido" = 1) THEN 'E-Commerce'
       WHEN (T3."U_OrigemPedido" = 2) THEN 'Interno'
       WHEN (T3."U_OrigemPedido" = 3) THEN 'Distribuidor'
       END as "sales_channel"
	  ,'False' as "scheduled"
      ,T3."DocDate" as "created"
      ,T3."DocDate" as "shipped_date"
      ,'NORMAL' as "shipped_order_type"
      
      ,case when T8."TrnspName" = 'SEDEX POR CONTA REMETENTE' then 2
    		when T8."TrnspName" = 'SEDEX POR CONTA DESTINATÁRIO' then 2
    		when T8."TrnspName" = 'PAC POR CONTA REMETENTE' then 1
    		when T8."TrnspName" = 'PAC POR CONTA DESTINATÁRIO' then 1
    		when T8."TrnspName" = 'TRANSPORTADORA POR CONTA REMETENTE' then T10."U_CVA_ID_Intelipost"
    		when T8."TrnspName" = 'TRANSPORTADORA POR CONTA DESTINATÁRIO' then T10."U_CVA_ID_Intelipost"	
        end as "delivery_method_id"            
      
--	  ,case when ifnull(T10."U_CVA_ID_Intelipost",0) as "delivery_method_id"
      ,CASE WHEN (T8."TrnspName" = 'PAC') THEN 1 WHEN (T8."TrnspName" = 'SEDEX') THEN 2 ELSE T3."TrnspCode" END as "delivery_method_External_id"
      -- Parte II
	 ,T3."CardName" as "first_name"
	 ,T3."CardName" as "last_name"
	 ,T4."E_Mail" as "email"
	 ,replace(T4."Phone2"||replace(replace(T4."Phone1",'(',''),')',''),'-','') as "phone"
	 ,replace(T4."Phone2"||replace(replace(T4."Cellular",'(',''),')',''),'-','') as "cellphone"
	 ,case when ifnull(T5."TaxId4",'') <> '' then 'False'
	       when ifnull(T5."TaxId0",'') <> '' then 'True' 
	  end as "is_company"
	 ,case when ifnull(T5."TaxId4",'') <> '' then replace(replace(replace(T5."TaxId4",'.',''),'/',''),'-','')
	       when ifnull(T5."TaxId0",'') <> '' then replace(replace(replace(T5."TaxId0",'.',''),'/',''),'-','')
	  end as "federal_tax_payer_id"   
     ,T7."Name" as "shipping_country"
     ,T6."State" as "shipping_state"
     ,T6."City" as "shipping_city"
     ,T6."AddrType"||' '||T6."Street" as "shipping_address"
     ,T6."StreetNo" as "shipping_number"
     ,T6."Block" as "shipping_quarter"
     , case when ifnull(T6."U_CVA_AddresRef",'') = '' then 'SEM REFRENCIA'
       end as "shipping_reference"
     ,TO_VARCHAR(T6."Building") as "shipping_additional"
     ,replace(T6."ZipCode",'-','') as "shipping_zip_code"
	 ,(SELECT replace(T0."ZipCode",'-','') FROM OBPL T0 where T0."BPLId" = T3."BPLId") as "origin_zip_code"
	 ,(SELECT T0."WhsName" FROM OWHS T0 Where T0."WhsCode" = T2."WhsCode") as "origin_warehouse_code"

	 -- Parte IV
	 ,T3."SeriesStr" as "invoice_serie"
	 ,T3."Serial" as "invoice_number"
	 ,/*(SELECT S1."U_ChaveAcesso"  
	     FROM "@SKL25NFE" S1 
	    WHERE S1."U_DocEntry" = T3."DocEntry" 
	      AND "U_cdErro" = '100') */
	  T3."U_nfe_ChaveAcesso" as "invoice_key"
	 ,T3."DocDate" as "invoice_date"
	 ,T3."DocTotal" as "invoice_total_value"
	 ,T3."DocTotal" as "invoice_products_value"
	 ,T2."CFOPCode" as "invoice_cfop"
	 ,T3."U_CVA_CodRastreio" as "tracking_code"
	 
	 -- Parte V
     ,T2."ShipDate" as "estimate_delivery_date"
     
     -- Parte VI
     ,T0."DocNum" as "erp"	 
	 ,T9."Carrier"
 from ORDR T0
inner join RDR1  T1 on T0."DocEntry" = T1."DocEntry"
inner join INV1  T2 on T0."DocEntry" = T2."BaseEntry"
  and T0."ObjType" = T2."BaseType"
  and T1."LineNum" = T2."BaseLine"
inner join OINV  T3 on T2."DocEntry" = T3."DocEntry"
inner join OCRD  T4 on T3."CardCode" = T4."CardCode"
inner join INV12 T5 on T3."DocEntry" = T5."DocEntry"
inner join CRD1  T6 on T3."CardCode" = T6."CardCode" and T3."ShipToCode" = T6."Address" 
inner join OCRY  T7 on T6."Country" = T7."Code"
 left join OSHP  T8 on T3."TrnspCode" = T8."TrnspCode"
INNER JOIN RDR12 T9 on T0."DocEntry" = T9."DocEntry"
INNER JOIN OCRD T10 on T5."Carrier" = T10."CardCode"
where T2."LineNum" = 0
  and T3."DocEntry"= :DocEntry

order by T3."DocDate",T0."DocNum";

end;
