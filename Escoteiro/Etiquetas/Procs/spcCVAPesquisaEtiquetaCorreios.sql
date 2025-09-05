CREATE PROCEDURE spcCVAPesquisaEtiquetaCorreios
(  
	  docini varchar(72)
	, docfim varchar(72)
	, dtini date
	, dtfim date
	, terms varchar(250)
	, layout varchar(250)
)

as

begin

--> Layout Sedex
IF(:layout = '001')then
	select distinct  '0' as "Sel"
	       ,T3."ObjType" as "ObjType"
	       ,T0."DocNum" as "Key"
	       ,T4."CardCode"||' - '|| T4."CardName" as "Cliente"	
	       ,T3."Serial" as "N° NF"
	       ,'SEDEX' as "Tipo Envio"
	  from ORDR T0
	 inner join RDR1 T1 on T0."DocEntry" = T1."DocEntry"
	 inner join INV1 T2 on T0."ObjType" = T2."BaseType"
	   and T0."DocEntry" = T2."BaseEntry"
	   and T1."LineNum" = T2."BaseLine"
	 inner join OINV T3 on T2."DocEntry" = T3."DocEntry"
	 Inner join OCRD T4 on T4."CardCode" = T3."CardCode"
	 where T0."DocNum"  between :docini and :docfim
	   and T0."TrnspCode" = 1
	   and T3."CANCELED" = 'N';
	   --and T3."U_nfe_STU" = 100;

 --> Layout PAC  
 end if;
  IF(:layout = '002')then 
  	select distinct  '0' as "Sel"
	       ,T3."ObjType" as "ObjType"
	       ,T0."DocNum" as "Key"
	       ,T4."CardCode"||' - '|| T4."CardName" as "Cliente"	
	       ,T3."Serial" as "N° NF"
	       ,'PAC' as "Tipo Envio"
	  from ORDR T0
	 inner join RDR1 T1 on T0."DocEntry" = T1."DocEntry"
	 inner join INV1 T2 on T0."ObjType" = T2."BaseType"
	   and T0."DocEntry" = T2."BaseEntry"
	   and T1."LineNum" = T2."BaseLine"
	 inner join OINV T3 on T2."DocEntry" = T3."DocEntry"
	 Inner join OCRD T4 on T4."CardCode" = T3."CardCode"
	 where T0."DocNum"  between :docini and :docfim
	   and T0."TrnspCode" = 2
	   and T3."CANCELED" = 'N';
	   --and T3."U_nfe_STU" = 100;
   end if;
   
--> Transportadora   
   IF(:layout = '003')then 
  	select distinct  '0' as "Sel"
	       ,T3."ObjType" as "ObjType"
	       ,T0."DocNum" as "Key"
	       ,T4."CardCode"||' - '|| T4."CardName" as "Cliente"	
	       ,T3."Serial" as "N° NF"
	       ,'TRANSPORTADORA' as "Tipo Envio"
	  from ORDR T0
	 inner join RDR1 T1 on T0."DocEntry" = T1."DocEntry"
	 inner join INV1 T2 on T0."ObjType" = T2."BaseType"
	   and T0."DocEntry" = T2."BaseEntry"
	   and T1."LineNum" = T2."BaseLine"
	 inner join OINV T3 on T2."DocEntry" = T3."DocEntry"
	 Inner join OCRD T4 on T4."CardCode" = T3."CardCode"
	 where T0."DocNum"  between :docini and :docfim
	   and T0."TrnspCode" = 3
	   and T3."CANCELED" = 'N';
	   --and T3."U_nfe_STU" = 100;
   end if;

   
end;