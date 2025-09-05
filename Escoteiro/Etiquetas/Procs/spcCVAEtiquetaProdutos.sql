CREATE PROCEDURE "spcCVAEtiquetaProdutos"
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

codItem  			varchar(100) :='';
item_1   			varchar(100):='';
item_2     			varchar(100):='';
cod_bar     		varchar(250) :='';
price  			    varchar(100):='';

script  varchar(80000) :='';
 
begin


  if rtrim(ltrim(ifnull(:campolivre1,''))) = ''
  then
    campolivre1 := '0';
  end if;

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

/* if(:campoLivre1 <> '0')*/
  select distinct T0."CodeBars"
	      ,T0."ItemCode"
	      ,T0."ItemName"      
	      ,'R$'||' '||To_DECIMAL(T1."Price",19,2) as "Price"
	  from OITM  T0
	 inner join ITM1 T1 ON T0."ItemCode" = T1."ItemCode"
	 inner join OPLN T2 ON T2."ListNum" = T1."PriceList" and T1."PriceList" = :campolivre1
	  where T0."ItemCode" = :key;	    

	  for c as cp1
	  do
	 
	 if(:campoLivre1 <> '0') then
	 
		codItem := c."ItemCode";
		item_1 	:= substring(c."ItemName",1,30);
		item_2	:= substring(c."ItemName",31,60);
		cod_bar  := c."CodeBars";
 	    price   := c."Price";
 	    
 	  else
		codItem := c."ItemCode";
		item_1 	:= substring(c."ItemName",1,30);
		item_2	:= substring(c."ItemName",31,60);
		cod_bar  := c."CodeBars"; 	    
 	  end if;
 	    
				
		script = replace(:script, '@codItem', ifnull(:codItem, ''));
		script = replace(:script, '@item_1', ifnull(:item_1, ''));
		script = replace(:script, '@item_2', ifnull(:item_2, ''));
		script = replace(:script, '@codbar', ifnull(:cod_bar,''));	 
		script = replace(:script, '@price', ifnull(:price, ''));	
		script = replace(:script, '@quantity', ifnull(:quantity, 1));	
		 
			
		 
  insert into #RESULT( id,tag) 
  
       select (select count(*)+1 from #RESULT),:script from dummy;
		 
	  end for; -- for do cursor
  end; -- begin do cursor
    

  select Id, Tag from #RESULT order by id; 
 
  drop table #RESULT;
end;

