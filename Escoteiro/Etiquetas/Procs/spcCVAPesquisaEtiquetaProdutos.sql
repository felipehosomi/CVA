CREATE PROCEDURE spcCVAPesquisaEtiquetaProdutos
(  
	  docini varchar(72)
	, docfim varchar(72)
	, dtini date
	, dtfim date
	, terms varchar(250)
)

as

begin

   select distinct '0' as "Sel"
   				   ,T0."ObjType" as "ObjType"
   				   ,T0."ItemCode" as "Key"
   				   ,T0."ItemCode"||' - ' || T0."ItemName" as "Item"
         
   from OITM T0
   where T0."ItemCode"  between :docini and :docfim
   order by 4;
   
end;

