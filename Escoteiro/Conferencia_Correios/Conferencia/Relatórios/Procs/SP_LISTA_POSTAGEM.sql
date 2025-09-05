alter PROCEDURE SP_LISTA_POSTAGEM  
(
	  Transp varchar(20)
	 ,Serial varchar(5000)
	 ,DataIncial Date
	 ,DataFinal Date
)

LANGUAGE SQLSCRIPT 
SQL SECURITY INVOKER
As

    queryStr1 nvarchar(30000);
	queryStr2 nvarchar(5000);
	
BEGIN
    queryStr1 := '';
    queryStr2 := '';
    
    
queryStr1 := 'select distinct
       T2."DocNum"	
      ,T0."U_CVA_CodADM" as "CodADM"
	  ,T0."U_CVA_UnidPostagem" as "Unid Postagem"
	  ,Current_date as "Data Postagem"
	  ,T0."U_CVA_NumContrato" as "Numero Contrato"
	  ,T0."U_CVA_CepPostagem" as "CEP"
	  ,T7."U_CVA_NumIdlista" as "Numero Lista"	  
	  ,T2."CardCode" as "Cód Cliente"
	  ,T2."CardName" as "Cliente"
	  ,T5."ZipCode" as "CEP Dstino"
	  ,T2."DocTotal" as "valor Declarado"
	  ,T2."Comments" as "OBS:"
	  ,T7."U_CVA_CodRastreio" as "Cód Rastreio"
	  ,T7."Serial" as "N° NF"
	  ,UPPER(T10."TrnspName") as "Name"
	  ,(select sum(T9."CVA_PESO")from"@CVA_VOLEMB_LINHA" T9 where T9."CVA_DOCENTRY" = T2."DocNum")  as "Peso Tarifado"
	  ,T0."U_CVA_NumCartao" as "Cartão Postagem"
	  ,(select "BPLName" from OBPL where "BPLId" = T2."BPLId") as "Rementente" 
	  ,T9."AddrType" as "AddrType"
	  ,T9."Street" as "Street"
	  ,T9."StreetNo" as "StreetNo"	   
	  ,T9."Building" as "Building"
	  ,T9."ZipCode" as "ZipCode"
	  ,T9."Block" as "Block"
	  ,T9."City" as "City"
	  ,T9."State" as "State"	  
 from "@CVA_CFGDESPACHO" T0
 inner join RDR12 T1 on T0."U_CVA_CardCode" = T1."Carrier"
 inner join ORDR  T2 on T1."DocEntry" = T2."DocEntry" And T0."DocEntry" = T2."TrnspCode"
 inner join RDR1  T3 on T2."DocEntry" = T3."DocEntry" 
 inner join OCRD  T4 on T2."CardCode" = T4."CardCode"
 inner join INV1  T6 on T6."BaseType" = T2."ObjType"
   					and T6."BaseEntry"  = T2."DocEntry"
   					and T6."BaseLine" = T3."LineNum"
 inner join OINV  T7 on T7."DocEntry" =T6."DocEntry"
 inner join CRD1  T5 on T2."CardCode" = T5."CardCode" and T5."AdresType" = ''S'' And T7."ShipToCode" = T5."Address"
 left join "@CVA_SERVPOSTAGEM" T8 on T0."U_CVA_WS_TpSrvPost" = T8."Code"
 inner join OBPL T9 on T7."BPLId" = T9."BPLId"
 left join OSHP T10 on T10."TrnspCode" = T7."TrnspCode" 

where T1."Carrier" = '''||:Transp ||'''
  and T7."CANCELED" = ''N''
  and T7."Serial" in ('||:Serial ||' )
  and T7."DocDate" between '''||:DataIncial||''' and '''||:DataFinal||''';
 
  
--  union all


--select distinct
--    T2."DocNum"	
--    ,T0."U_CVA_CodADM" as "CodADM"
--	  ,T0."U_CVA_UnidPostagem" as "Unid Postagem"
--	  ,Current_date as "Data Postagem"
--	  ,T0."U_CVA_NumContrato" as "Numero Contrato"
--	  ,T0."U_CVA_CepPostagem" as "CEP"
--	  ,T6."U_CVA_NumIdLista" as "Numero Lista"	  
--	  ,T2."CardCode" as "Cód Cliente"
--	  ,T2."CardName" as "Cliente"
--	  ,T5."ZipCode" as "CEP Dstino"
--	  ,T0."U_CVA_DeclareVlr" as "valor Declarado"
--	  ,T2."Comments" as "OBS:"
--	  ,T7."U_CVA_CodRastreio" as "Cód Rastreio"
--	  ,T7."Serial" as "N° NF"
--	  ,UPPER(T8."Name") as "Name"
--	  ,(select sum(T9."CVA_PESO")from"@CVA_VOLEMB_LINHA" T9 where T9."CVA_DOCENTRY" = T2."DocNum")  as "Peso Tarifado"
--	  ,T0."U_CVA_NumCartao" as "Cartão Postagem"
--	  ,(select top 1 "BPLName" from OBPL) as "Rementente" 
--	  ,T9."AddrType" as "AddrType"
--	  ,T9."Street" as "Street"
--	  ,T9."StreetNo" as "StreetNo"	   
--	  ,T9."Building" as "Building"
--	  ,T9."ZipCode" as "ZipCode"
--	  ,T9."Block" as "Block"
--	  ,T9."City" as "City"
--	  ,T9."State" as "State"
-- from "@CVA_CFGDESPACHO" T0
-- inner join RDR12 T1 on T0."U_CVA_CardCode" = T1."Carrier"
-- inner join ORDR  T2 on T1."DocEntry" = T2."DocEntry"
-- inner join RDR1  T3 on T2."DocEntry" = T3."DocEntry" 
-- inner join OCRD  T4 on T2."CardCode" = T4."CardCode"
-- inner join CRD1  T5 on T2."CardCode" = T5."CardCode" and T5."AdresType" = ''S''
-- inner join DLN1  T6 on T6."BaseType" = T2."ObjType"
--   					and T6."BaseEntry"  = T2."DocEntry"
--   					and T6."BaseLine" = T3."LineNum"
-- inner join ODLN  T7 on T7."DocEntry" =T6."DocEntry"
-- inner join "@CVA_SERVPOSTAGEM" T8 on T0."U_CVA_WS_TpSrvPost" = T8."Code"
-- inner join OBPL T9 on T7."BPLId" = T9."BPLId"    
  					
--where T1."Carrier" = '''||:Transp ||'''
--  and T7."CANCELED" = ''N''
--  and T7."Serial" in ('||:Serial ||' )
--  and T7."DocDate" between '''||:DataIncial||''' and '''||:DataFinal||'''
--  order  by 14';


--select :queryStr1 from dummy;

execute immediate( :queryStr1);

END;
