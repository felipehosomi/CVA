ALTER PROCEDURE SP_CARREGA_REMETENTE
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
       T0."U_CVA_NumContrato" as "Numero Contrato"
      ,T0."U_CVA_NumDiretoria" as "Numero Diretoria"
      ,T0."U_CVA_CodADM" as "CodADM"
	  ,(select top 1 "BPLName" from OBPL) as "Rementente"       
	  ,CONCAT(CONCAT(T9."AddrType",'' ''),T9."Street") as "Logradouro"      
	  ,replace(T9."StreetNo",''.'','''') as "NumeroRemetente"	   
	  ,T9."Building" as "Complemento"
	  ,T9."Block" as "Bairro"
	  ,replace(T9."ZipCode",''-'','''') as "CEP"
	  ,T9."City" as "Cidade"
	  ,T9."State" as "UF"	
	  ,T0."U_CVA_NumCartao" as "Cartão Postagem"
	  ,T4."Fax" as "Fax"
	  ,T4."E_Mail" as "Email"
	  ,T0."U_CVA_WS_IDServico" as "Id Serviço"
	  ,T0."U_CVA_WS_URL" as "URL"
	  ,T0."U_CVA_WSUser" as "user"
	  ,T0."U_CVA_WSPass" as "Password"
	  

  from "@CVA_CFGDESPACHO" T0
 inner join RDR12 T1 on T0."U_CVA_CardCode" = T1."Carrier"
 inner join ORDR  T2 on T1."DocEntry" = T2."DocEntry"
 inner join RDR1  T3 on T2."DocEntry" = T3."DocEntry" 
 inner join OCRD  T4 on T2."CardCode" = T4."CardCode"
 inner join CRD1  T5 on T2."CardCode" = T5."CardCode" and T5."AdresType" = ''S''
 inner join INV1  T6 on T6."BaseType" = T2."ObjType"
   					and T6."BaseEntry"  = T2."DocEntry"
   					and T6."BaseLine" = T3."LineNum"
 inner join OINV  T7 on T7."DocEntry" =T6."DocEntry"
 inner join "@CVA_SERVPOSTAGEM" T8 on T0."U_CVA_WS_TpSrvPost" = T8."Code"
 inner join OBPL T9 on T7."BPLId" = T9."BPLId"

where T1."Carrier" = '''||:Transp ||'''
  and T7."CANCELED" = ''N''
  and T7."Serial" in ('||:Serial ||' )
  and T7."DocDate" between '''||:DataIncial||''' and '''||:DataFinal||'''

union all 

select distinct
       T0."U_CVA_NumContrato" as "Numero Contrato"
      ,T0."U_CVA_NumDiretoria" as "Numero Diretoria"
      ,T0."U_CVA_CodADM" as "CodADM"
	  ,(select top 1 "BPLName" from OBPL) as "Rementente"       
	  ,CONCAT(CONCAT(T9."AddrType",'' ''),T9."Street") as "Logradouro"      
	  ,replace(T9."StreetNo",''.'','''') as "NumeroRemetente"   
	  ,T9."Building" as "Complemento"
	  ,T9."Block" as "Bairro"
	  ,replace(T9."ZipCode",''-'','''') as "CEP"
	  ,T9."City" as "Cidade"
	  ,T9."State" as "UF"	
	  ,T0."U_CVA_NumCartao" as "Cartão Postagem"
	  ,T4."Fax" as "Fax"
	  ,T4."E_Mail" as "Email"
  	  ,T0."U_CVA_WS_IDServico" as "Id Serviço"
	  ,T0."U_CVA_WS_URL" as "URL"
  	  ,T0."U_CVA_WSUser" as "user"
	  ,T0."U_CVA_WSPass" as "Password"

 from "@CVA_CFGDESPACHO" T0
 inner join RDR12 T1 on T0."U_CVA_CardCode" = T1."Carrier"
 inner join ORDR  T2 on T1."DocEntry" = T2."DocEntry"
 inner join RDR1  T3 on T2."DocEntry" = T3."DocEntry" 
 inner join OCRD  T4 on T2."CardCode" = T4."CardCode"
 inner join CRD1  T5 on T2."CardCode" = T5."CardCode" and T5."AdresType" = ''S''
 inner join DLN1  T6 on T6."BaseType" = T2."ObjType"
   					and T6."BaseEntry"  = T2."DocEntry"
   					and T6."BaseLine" = T3."LineNum"
 inner join ODLN  T7 on T7."DocEntry" =T6."DocEntry"
 inner join "@CVA_SERVPOSTAGEM" T8 on T0."U_CVA_WS_TpSrvPost" = T8."Code"
 inner join OBPL T9 on T7."BPLId" = T9."BPLId"    
  					
where T1."Carrier" = '''||:Transp ||'''
  and T7."CANCELED" = ''N''
  and T7."Serial" in ('||:Serial ||' )
  and T7."DocDate" between '''||:DataIncial||''' and '''||:DataFinal||'''';


--select :queryStr1 from dummy;

execute immediate( :queryStr1);

END;


call SP_CARREGA_REMETENTE ('V22000','645,647,648','2000/01/01','2020/01/01')