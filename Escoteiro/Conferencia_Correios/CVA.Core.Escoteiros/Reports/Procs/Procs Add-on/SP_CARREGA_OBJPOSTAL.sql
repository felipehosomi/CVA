CREATE PROCEDURE SP_CARREGA_OBJPOSTAL
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

	
BEGIN
    queryStr1 := '';
    
queryStr1 := 'select distinct
       T7."U_CVA_CodRastreio" as "Cod Rastreio"
      ,T0."U_CVA_WS_CodServico" as "Cod Servico"
      ,(select sum(T9."CVA_PESO")from"@CVA_VOLEMB_LINHA" T9 where T9."CVA_DOCENTRY" = T2."DocNum")  as "Peso Tarifado"     
      
      ,T2."CardName" as "Destinatario"
	  ,CONCAT(replace(replace(T4."Phone2",''('',''''),'')'',''''), replace(replace(T4."Phone1",''('',''''),'')'','''')) as "Telefone"
	  ,CONCAT(replace(replace(T4."Phone2",''('',''''),'')'',''''), replace(replace(T4."Cellular",''('',''''),'')'',''''))as "Celular"
	  ,CONCAT(CONCAT(ifnull(T1."AddrTypeS",''''),'' ''),T1."StreetS") as "Logradouro"      
  	  ,TO_VARCHAR(T1."BuildingS") as "Complemento"
	  ,replace(T1."StreetNoS",''.'','''') as "NumeroDestinatario"	
	     

	  ,T1."BlockS" as "Bairro"
  	  ,T1."CityS" as "Cidade"
  	  ,T1."StateS" as "UF"
	  ,replace(T1."ZipCodeS",''-'','''') as "CEP"
	  ,T7."Serial" as "NF"
	  ,T7."SeriesStr" as "SerieNF"
	  ,REPLACE(TO_VARCHAR(TO_DECIMAL((T7."Max1099" + T7."TotalExpns"),10,2)),''.'','','') as "VlrNF"
	  
	  ,CONCAT(''00'',T10."CVA_IDVOLUME") as "TipoObjeto"
	  ,REPLACE(TO_VARCHAR(TO_DECIMAL(T11."U_CVA_Altura",10,2)),''.'','','') as "Altura"
	  ,REPLACE(TO_VARCHAR(TO_DECIMAL(T11."U_CVA_Largura",10,2)),''.'','','') as "Largura"
	  ,REPLACE(TO_VARCHAR(TO_DECIMAL(T11."U_CVA_Comprimento",10,2)),''.'','','') as "Comprimento"
	  ,T4."E_Mail" as "E_Mail"
	  
 from "@CVA_CFGDESPACHO" T0
 inner join RDR12 T1 on T0."U_CVA_CardCode" = T1."Carrier"
 inner join ORDR  T2 on T1."DocEntry" = T2."DocEntry"
 inner join RDR1  T3 on T2."DocEntry" = T3."DocEntry" 
 inner join OCRD  T4 on T2."CardCode" = T4."CardCode"
 inner join INV1  T6 on T6."BaseType" = T2."ObjType"
   					and T6."BaseEntry"  = T2."DocEntry"
   					and T6."BaseLine" = T3."LineNum"
 inner join OINV  T7 on T7."DocEntry" =T6."DocEntry"
 inner join "@CVA_SERVPOSTAGEM" T8 on T0."U_CVA_WS_TpSrvPost" = T8."Code"
 inner join OBPL T9 on T7."BPLId" = T9."BPLId"
 inner join "@CVA_VOLEMB_LINHA" T10 on T2."DocNum" = T10."CVA_DOCENTRY" 
 inner join "@CVA_TPVOL" T11 on T10."CVA_IDVOLUME" = T11."Code"
 left join OSHP T12 on T7."TrnspCode" = T12."TrnspCode"

where T1."Carrier" = '''||:Transp ||'''
  and T7."CANCELED" = ''N''
  and T7."Serial" in ('||:Serial ||' )
  and T7."DocDate" between '''||:DataIncial||''' and '''||:DataFinal||'''
  and T0."U_CVA_TipoDespacho" = T12."TrnspName"

union all

select distinct
       T7."U_CVA_CodRastreio" as "Cod Rastreio"
      ,T0."U_CVA_WS_CodServico" as "Cod Servico"
      ,(select sum(T9."CVA_PESO")from"@CVA_VOLEMB_LINHA" T9 where T9."CVA_DOCENTRY" = T2."DocNum")  as "Peso Tarifado"     
      
      ,T2."CardName" as "Destinatario"
	  ,CONCAT(replace(replace(T4."Phone2",''('',''''),'')'',''''), replace(replace(T4."Phone1",''('',''''),'')'','''')) as "Telefone"
	  ,CONCAT(replace(replace(T4."Phone2",''('',''''),'')'',''''), replace(replace(T4."Cellular",''('',''''),'')'',''''))as "Celular"
	  ,CONCAT(CONCAT(ifnull(T1."AddrTypeS",''''),'' ''),T1."StreetS") as "Logradouro"      
  	  ,TO_VARCHAR(T1."BuildingS") as "Complemento"
	  ,replace(T1."StreetNoS",''.'','''') as "NumeroDestinatario"	
	     

	  ,T1."BlockS" as "Bairro"
  	  ,T1."CityS" as "Cidade"
  	  ,T1."StateS" as "UF"
	  ,replace(T1."ZipCodeS",''-'','''') as "CEP"
	  ,T7."Serial" as "NF"
	  ,T7."SeriesStr" as "SerieNF"
	  ,REPLACE(TO_VARCHAR(TO_DECIMAL((T7."Max1099" + T7."TotalExpns"),10,2)),''.'','','') as "VlrNF"
	  
	  ,CONCAT(''00'',T10."CVA_IDVOLUME") as "TipoObjeto"
	  ,REPLACE(TO_VARCHAR(TO_DECIMAL(T11."U_CVA_Altura",10,2)),''.'','','') as "Altura"
	  ,REPLACE(TO_VARCHAR(TO_DECIMAL(T11."U_CVA_Largura",10,2)),''.'','','') as "Largura"
	  ,REPLACE(TO_VARCHAR(TO_DECIMAL(T11."U_CVA_Comprimento",10,2)),''.'','','') as "Comprimento"
	  ,T4."E_Mail" as "E_Mail"
	  
 from "@CVA_CFGDESPACHO" T0
 inner join RDR12 T1 on T0."U_CVA_CardCode" = T1."Carrier"
 inner join ORDR  T2 on T1."DocEntry" = T2."DocEntry"
 inner join RDR1  T3 on T2."DocEntry" = T3."DocEntry" 
 inner join OCRD  T4 on T2."CardCode" = T4."CardCode"
 inner join DLN1  T6 on T6."BaseType" = T2."ObjType"
   					and T6."BaseEntry"  = T2."DocEntry"
   					and T6."BaseLine" = T3."LineNum"
 inner join ODLN  T7 on T7."DocEntry" =T6."DocEntry"
 inner join "@CVA_SERVPOSTAGEM" T8 on T0."U_CVA_WS_TpSrvPost" = T8."Code"
 inner join OBPL T9 on T7."BPLId" = T9."BPLId"
 inner join "@CVA_VOLEMB_LINHA" T10 on T2."DocNum" = T10."CVA_DOCENTRY" 
 inner join "@CVA_TPVOL" T11 on T10."CVA_IDVOLUME" = T11."Code"
 left join OSHP T12 on T7."TrnspCode" = T7."TrnspCode"
where T1."Carrier" = '''||:Transp ||'''
  and T7."CANCELED" = ''N''
  and T7."Serial" in ('||:Serial ||' )
  and T7."DocDate" between '''||:DataIncial||''' and '''||:DataFinal||'''
  and T0."U_CVA_TipoDespacho" = T12."TrnspName"
  order by 14
  ';


--select :queryStr1 from dummy;

execute immediate( :queryStr1);

END;

