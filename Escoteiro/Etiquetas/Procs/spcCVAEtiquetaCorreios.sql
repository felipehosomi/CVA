CREATE PROCEDURE "spcCVAEtiquetaCorreios"
(
  layout nvarchar(72)
, Objtype nvarchar(72)
, key nvarchar(72)
, quantity int
, campolivre1 varchar(100)
, campolivre2 varchar(100)
, campolivre3 varchar(100)
, campolivre4 varchar(100)
, campolivre5 varchar(100)
, campolivre6 varchar(100)
, campolivre7 varchar(100)
, campolivre8 varchar(100)
)
LANGUAGE SQLSCRIPT
AS

contrato  			varchar(50) :='';
ano       			varchar(10):='';
uf        			varchar(2):='';
cliente  			varchar(250) :='';
endereco_Cliente   	varchar(250) :='';
end_1    			varchar(250) :='';
end_2     			varchar(250) :='';
end_3     			varchar(250) :='';
end_4     			varchar(250) :='';
end_5     			varchar(250) :='';
remetente           varchar(250) :='';
re_1	            varchar(250) :='';
endereco_Remetente  varchar(250) :='';
end_Reme_1     		varchar(250) :='';
end_Reme_2     		varchar(250) :='';
cod_bar     		varchar(250) :='';
cod_rastreio   		varchar(250) :='';
dataPost   			varchar(10) :='';

pedido int :=0;
nf     int :=0;
script  varchar(80000) :='';
 
begin


  -- Coleto o script padrão da etiqueta
  select T0."U_script" into script
    from "@CVA_ETIQUETA" T0
   where T0."Code" = :layout;
    

  -- Tabela temporária para retorno
  create local temporary 
   table #RESULT
   (   id int  
     , tag nclob null
   );

   begin   	  
	  declare cursor cp1 for
	  
	  --> Nota de Saída

select top 1 
	    T6."U_CVA_NumContrato"
	   ,Year("U_CVA_DataContrato") as "ano"
	   ,T6."U_CVA_UfContrato"
       ,T0."DocNum"
       ,T3."Serial" 
   	   ,T0."CardName"      
       ,(select distinct ifnull("AddrTypeB",'')||' '||ifnull("StreetB",'')||', '||ifnull("StreetNoB",'')||', '||ifnull("BuildingB",'')||', '||ifnull("BlockB",'')||', '||ifnull("ZipCodeB",'')||', '||ifnull("CityB",'')||' - '||ifnull("StateB",'') from RDR12 where "DocEntry" = T0."DocEntry") as "End_Cliente"
       ,(select replace("ZipCodeB",'-','') from RDR12 where "DocEntry" = T0."DocEntry") as "Cep"
       ,(select "BPLName" from OBPL where "BPLId" = T0."BPLId") as "Remetente"
       ,(select ifnull("AddrType",'')||' '||ifnull("Street",'')||', '||ifnull("StreetNo",'')||', '||ifnull("Building",'')||', '||ifnull("Block",'')||', '||ifnull("ZipCode",'')||', '||ifnull("City",'')||' - '||ifnull("State",'') from OBPL where "BPLId" = T0."BPLId") as "End_Remetente"
       ,TO_VARCHAR(Current_date ,'DD/MM/YYYY') as "dataPost"
       ,T3."U_CVA_CodRastreio"
  from ORDR T0
 inner join RDR1 T1 on T0."DocEntry" = T1."DocEntry"
 inner join INV1 T2 on T0."ObjType" = T2."BaseType"
   and T0."DocEntry" = T2."BaseEntry"
   and T1."LineNum" = T2."BaseLine"
 inner join OINV T3 on T2."DocEntry" = T3."DocEntry"
 inner join OCRD T4 on T4."CardCode" = T3."CardCode"
 inner join RDR12 T5 on T5."DocEntry" = T0."DocEntry"
 left join "@CVA_CFGDESPACHO" T6 on T6."U_CVA_CardCode" = T5."Carrier"   
 where T0."DocNum"  = :key
   and T3."ObjType" = :objtype
   and T3."CANCELED" = 'N'
   
   order by 4;
  
       
	  for c as cp1
	  do
	      
		contrato:= c."U_CVA_NumContrato";
		ano 	:= c."ano";
		uf 		:= c."U_CVA_UfContrato";
		pedido  := c."DocNum";
		cliente := c."CardName";
 	    endereco_Cliente := substring(c."End_Cliente",0,50);   
 	    end_2 := substring(c."End_Cliente",51,50); 	
 	    end_3 := substring(c."End_Cliente",102,50); 	
 	    end_4 := substring(c."End_Cliente",153,50); 	
 	    end_5 := substring(c."End_Cliente",204,50); 	
 	    
 	        
 	    cod_bar := c."Cep";
	    remetente := substring(c."Remetente",0,47);
	    re_1 := substring(c."Remetente",48,50);
	    
	    endereco_Remetente := substring(c."End_Remetente",0,42);	    
		end_Reme_2 := substring(c."End_Remetente",43,86);
		dataPost :=c."dataPost";
		cod_rastreio := c."U_CVA_CodRastreio";
		nf := c."Serial";
				
		script = replace(:script, '@contrato', ifnull(:contrato, ''));
		script = replace(:script, '@ano', ifnull(:ano, ''));
		script = replace(:script, '@uf', ifnull(:uf, ''));
		script = replace(:script, '@pedido', ifnull(:pedido,0));	 
		script = replace(:script, '@cliente', ifnull(:cliente, ''));	 
		script = replace(:script, '@end_1', ifnull(:endereco_Cliente, ''));	 
		script = replace(:script, '@end_2', ifnull(:end_2, ''));	 
		script = replace(:script, '@end_3', ifnull(:end_3, ''));	 
		script = replace(:script, '@end_4', ifnull(:end_4, ''));	 
		script = replace(:script, '@end_5', ifnull(:end_5, ''));	 						
		script = replace(:script, '@codbar', ifnull(:cod_bar, ''));	 
		script = replace(:script, '@remetente', ifnull(:remetente, ''));	 
		script = replace(:script, '@re_1', ifnull(:re_1, ''));	
		script = replace(:script, '@end_Reme_1', ifnull(:endereco_Remetente, ''));	 
		script = replace(:script, '@end_Reme_2', ifnull(:end_Reme_2, ''));	 
		script = replace(:script, '@data_post', ifnull(:dataPost, ''));	 
		script = replace(:script, '@cod_rastreio', ifnull(:cod_rastreio, ''));	 
		script = replace(:script, '@nf', ifnull(:nf, 0));	  	
		script = replace(:script, '@quantity', ifnull(:quantity, 0));	  	
		 
  insert into #RESULT( id,tag)
       select (select count(*)+1 from #RESULT),:script from dummy;
		 
	  end for; -- for do cursor
  end; -- begin do cursor
    

  select Id, Tag from #RESULT order by id; 
 
  drop table #RESULT;
end;

