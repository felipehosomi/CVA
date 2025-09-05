CREATE PROCEDURE SPC_CVA_SEPARACAO_MERCADORIA
(
	 PedidoIni int
	,PedidoFim int
    ,DataInicio date
    ,DataFinal date
    ,Cliente nvarchar(200)
    ,Impresso char(1)
)
as

Begin
/***************************************/	
declare v_COUNT int;

/***************************************/

if(:Impresso = 'N') then
	
	SELECT COUNT (*) INTO v_COUNT FROM M_TEMPORARY_TABLES WHERE SCHEMA_NAME=CURRENT_SCHEMA 
	AND UPPER(TABLE_NAME)='#TBT_RPT_SEPARACAO';
	IF v_COUNT>=1 THEN
		DROP TABLE #TBT_RPT_SEPARACAO;
	END IF;

	create local temporary 
	table #TBT_RPT_SEPARACAO
	(   	
		"N° Pedido" DECIMAL(19,6),
		"Cod Cliente" NVARCHAR(15),
		"Cliente" NVARCHAR(100),
		"Data do Pedido" Date,
		"Valor Total Pedido" DECIMAL(19,6),
		"Pedido Site" DECIMAL(19,6),
		"TotalExpns" DECIMAL(19,6),
		"TrnspName" NVARCHAR(40),
		"ItemCode" NVARCHAR(50),
		"Item" NVARCHAR(100),
		"Qtde Aberta" DECIMAL(19,6),
		"Qtde Pedido" DECIMAL(19,6),
		"CodBarras" NVARCHAR(254),
		"Imprime Cod" NVARCHAR(1),
		"State" NVARCHAR(3),
		"Piso" DECIMAL(19,6),
		"Rua" NVARCHAR(254),
		"Estante" NVARCHAR(254),
		"Prateleira" NVARCHAR(254),
		"Escaninho" NVARCHAR(254),
		"Transp" NVARCHAR(100)
	);
	
	
		insert into #TBT_RPT_SEPARACAO
			Select Distinct T0."DocNum" as "N° Pedido"
			 	  ,T0."CardCode" as "Cod Cliente"
			 	  ,T0."CardName" as "Cliente"
			 	  ,T0."TaxDate" as "Data do Pedido"
			 	  ,T0."Max1099" as "Valor Total Pedido"
			      ,T0."U_CVA_Increment_id" as "Pedido Site"
			      ,T0."TotalExpns"
			      ,T8."TrnspName"
			 	  ,T1."ItemCode"  
			 	  ,T2."ItemName" as "Item"
			 	  ,(Select SUM("OpenCreQty") From RDR1 Where RDR1."DocEntry" = T1."DocEntry" And RDR1."ItemCode" = T1."ItemCode") as "Qtde Aberta"
			 	  ,(Select SUM("Quantity") From RDR1 Where RDR1."DocEntry" = T1."DocEntry" And RDR1."ItemCode" = T1."ItemCode") as "Qtde Pedido"
			 	  ,T2."CodeBars" as "CodBarras"
			 	  ,T2."U_CVA_ImprCodBar" as "Imprime Cod"
			      ,T4."State"
			 	  ,T5."U_CVA_Piso" as "Piso"
			 	  ,T5."U_CVA_Rua"  as "Rua"
			 	  ,T5."U_CVA_Estante" as "Estante"
			 	  ,T5."U_CVA_Prateleira" as "Prateleira"
			 	  ,T5."U_CVA_Escaninho" as "Escaninho"
			 	  ,(select "CardName" from OCRD where "CardCode" = T4."Carrier") as "Transp"
			
			  from ORDR T0
			 inner join RDR1  T1 on T0."DocEntry" = T1."DocEntry"
			 inner join OITM  T2 on T1."ItemCode" = T2."ItemCode"
			 inner join OCRD  T3 on T0."CardCode" = T3."CardCode"
			 inner join RDR12 T4 on T0."DocEntry" = T4."DocEntry"
			 inner join OWHS  T5 on T1."WhsCode"  = T5."WhsCode"
			 left join DPI1   T6 on T0."DocEntry" = T6."BaseEntry"
			   and T0."ObjType" = T6."BaseType"
			   and T1."LineNum" = T6."BaseLine"
			 left join ODPI T7 on T6."DocEntry" = T7."DocEntry"
			   and T7."CANCELED" = 'N'
			left join OSHP T8 on T8."TrnspCode" = T0."TrnspCode"
			 where T0."DocNum"  between :PedidoIni and :PedidoFim
			   and T0."TaxDate" between :DataInicio and :DataFinal
			   and (T0."CardCode" =  :Cliente or :Cliente = '*')
			   and T1."OpenCreQty" > 0
			   and T1."LineStatus" = 'O'
			   and T0."U_CVA_Picking_Impresso" = 1
			   and IFNULL(T0."U_CVA_Separacao_Impresso",2) = 2
			   and T0."U_CVA_Status_Picking" <> 3
			   and T0."U_CVA_Increment_id" is null  --- Pedidos Gerados Manualmente
			   			   
			UNION ALL  
			
			Select distinct T0."DocNum" as "N° Pedido"
			 	  ,T0."CardCode" as "Cod Cliente"
			 	  ,T0."CardName" as "Cliente"
			 	  ,T0."TaxDate" as "Data do Pedido"
			 	  ,T0."Max1099" as "Valor Total Pedido"
			      ,T0."U_CVA_Increment_id" as "Pedido Site"
			      ,T0."TotalExpns"
			      ,T8."TrnspName"
			 	  ,T1."ItemCode"  
			 	  ,T2."ItemName" as "Item"
			 	  ,(Select SUM("OpenCreQty") From RDR1 Where RDR1."DocEntry" = T1."DocEntry" And RDR1."ItemCode" = T1."ItemCode") as "Qtde Aberta"
			 	  ,(Select SUM("Quantity") From RDR1 Where RDR1."DocEntry" = T1."DocEntry" And RDR1."ItemCode" = T1."ItemCode") as "Qtde Pedido"
			 	  ,T2."CodeBars" as "CodBarras"
			 	  ,T2."U_CVA_ImprCodBar" as "Imprime Cod"
			                  ,T4."State"
			 	  ,T5."U_CVA_Piso" as "Piso"
			 	  ,T5."U_CVA_Rua"  as "Rua"
			 	  ,T5."U_CVA_Estante" as "Estante"
			 	  ,T5."U_CVA_Prateleira" as "Prateleira"
			 	  ,T5."U_CVA_Escaninho" as "Escaninho"
			 	  ,(select "CardName" from OCRD where "CardCode" = T4."Carrier") as "Transp"
			
			  from ORDR T0
			 inner join RDR1  T1 on T0."DocEntry" = T1."DocEntry"
			 inner join OITM  T2 on T1."ItemCode" = T2."ItemCode"
			 inner join OCRD  T3 on T0."CardCode" = T3."CardCode"
			 inner join RDR12 T4 on T0."DocEntry" = T4."DocEntry"
			 inner join OWHS  T5 on T1."WhsCode"  = T5."WhsCode"
			 inner join DPI1   T6 on T0."DocEntry" = T6."BaseEntry"
			   and T0."ObjType" = T6."BaseType"
			   and T1."LineNum" = T6."BaseLine"
			 inner join ODPI T7 on T6."DocEntry" = T7."DocEntry"
			   and T7."CANCELED" = 'N'
			left join OSHP T8 on T8."TrnspCode" = T0."TrnspCode"
			 where T0."DocNum"  between :PedidoIni and :PedidoFim
			   and T0."TaxDate" between :DataInicio and :DataFinal
			   and (T0."CardCode" =  :Cliente or :Cliente = '*')
			   --and T1."OpenCreQty" > 0
			   --and T1."LineStatus" = 'O'
			   and T0."U_CVA_Picking_Impresso" = 1
			   and IFNULL(T0."U_CVA_Separacao_Impresso",2) = 2
			   and T0."U_CVA_Status_Picking" <> 3
			   and T0."U_CVA_Increment_id"  is not null; --- Pedidos Gerados Magento
	
	update ORDR
	set "U_CVA_Separacao_Impresso" = 1
	where "DocNum" in (select "N° Pedido" from #TBT_RPT_SEPARACAO group by "N° Pedido");
	
	--RESULT
	select * from #TBT_RPT_SEPARACAO;
	
/***************************************/		
	DROP TABLE #TBT_RPT_SEPARACAO;
/***************************************/		

 -- Pedidos Já Impressos
else if(:Impresso = 'Y') then

				Select distinct T0."DocNum" as "N° Pedido"
			 	  ,T0."CardCode" as "Cod Cliente"
			 	  ,T0."CardName" as "Cliente"
			 	  ,T0."TaxDate" as "Data do Pedido"
			 	  ,T0."Max1099" as "Valor Total Pedido"
			      ,T0."U_CVA_Increment_id" as "Pedido Site"
			      ,T0."TotalExpns"
			      ,T8."TrnspName"
			 	  ,T1."ItemCode"  
			 	  ,T2."ItemName" as "Item"
			 	  ,(Select SUM("OpenCreQty") From RDR1 Where RDR1."DocEntry" = T1."DocEntry" And RDR1."ItemCode" = T1."ItemCode") as "Qtde Aberta"
			 	  ,(Select SUM("Quantity") From RDR1 Where RDR1."DocEntry" = T1."DocEntry" And RDR1."ItemCode" = T1."ItemCode") as "Qtde Pedido"
			 	  ,T2."CodeBars" as "CodBarras"
			 	  ,T2."U_CVA_ImprCodBar" as "Imprime Cod"
			                  ,T4."State"
			 	  ,T5."U_CVA_Piso" as "Piso"
			 	  ,T5."U_CVA_Rua"  as "Rua"
			 	  ,T5."U_CVA_Estante" as "Estante"
			 	  ,T5."U_CVA_Prateleira" as "Prateleira"
			 	  ,T5."U_CVA_Escaninho" as "Escaninho"
			 	  ,(select "CardName" from OCRD where "CardCode" = T4."Carrier") as "Transp"
			
			  from ORDR T0
			 inner join RDR1  T1 on T0."DocEntry" = T1."DocEntry"
			 inner join OITM  T2 on T1."ItemCode" = T2."ItemCode"
			 inner join OCRD  T3 on T0."CardCode" = T3."CardCode"
			 inner join RDR12 T4 on T0."DocEntry" = T4."DocEntry"
			 inner join OWHS  T5 on T1."WhsCode"  = T5."WhsCode"
			 left join DPI1   T6 on T0."DocEntry" = T6."BaseEntry"
			   and T0."ObjType" = T6."BaseType"
			   and T1."LineNum" = T6."BaseLine"
			 left join ODPI T7 on T6."DocEntry" = T7."DocEntry"
			   and T7."CANCELED" = 'N'
			left join OSHP T8 on T8."TrnspCode" = T0."TrnspCode"
		 where T0."DocNum"  between :PedidoIni and :PedidoFim
		   and T0."TaxDate" between :DataInicio and :DataFinal
		   and (T0."CardCode" =  :Cliente or :Cliente = '*')
		--   and T1."OpenCreQty" > 0
		--   and T1."LineStatus" = 'O'
			   and T0."U_CVA_Picking_Impresso" = 1
		       and IFNULL(T0."U_CVA_Separacao_Impresso",2) = 1
		--   and T0."U_CVA_Status_Picking"  <> 2
			   and T0."U_CVA_Increment_id" is null--- Pedidos Gerados Manualmente
			   
			UNION ALL  
			
			Select distinct T0."DocNum" as "N° Pedido"
			 	  ,T0."CardCode" as "Cod Cliente"
			 	  ,T0."CardName" as "Cliente"
			 	  ,T0."TaxDate" as "Data do Pedido"
			 	  ,T0."Max1099" as "Valor Total Pedido"
			      ,T0."U_CVA_Increment_id" as "Pedido Site"
			      ,T0."TotalExpns"
			      ,T8."TrnspName"
			 	  ,T1."ItemCode"  
			 	  ,T2."ItemName" as "Item"
				  ,(Select SUM("OpenCreQty") From RDR1 Where RDR1."DocEntry" = T1."DocEntry" And RDR1."ItemCode" = T1."ItemCode") as "Qtde Aberta"
			 	  ,(Select SUM("Quantity") From RDR1 Where RDR1."DocEntry" = T1."DocEntry" And RDR1."ItemCode" = T1."ItemCode") as "Qtde Pedido"
			 	  ,T2."CodeBars" as "CodBarras"
			 	  ,T2."U_CVA_ImprCodBar" as "Imprime Cod"
			      ,T4."State"
			 	  ,T5."U_CVA_Piso" as "Piso"
			 	  ,T5."U_CVA_Rua"  as "Rua"
			 	  ,T5."U_CVA_Estante" as "Estante"
			 	  ,T5."U_CVA_Prateleira" as "Prateleira"
			 	  ,T5."U_CVA_Escaninho" as "Escaninho"
			 	  ,(select "CardName" from OCRD where "CardCode" = T4."Carrier") as "Transp"
			
			  from ORDR T0
			 inner join RDR1  T1 on T0."DocEntry" = T1."DocEntry"
			 inner join OITM  T2 on T1."ItemCode" = T2."ItemCode"
			 inner join OCRD  T3 on T0."CardCode" = T3."CardCode"
			 inner join RDR12 T4 on T0."DocEntry" = T4."DocEntry"
			 inner join OWHS  T5 on T1."WhsCode"  = T5."WhsCode"
			 inner join DPI1   T6 on T0."DocEntry" = T6."BaseEntry"
			   and T0."ObjType" = T6."BaseType"
			   and T1."LineNum" = T6."BaseLine"
			 inner join ODPI T7 on T6."DocEntry" = T7."DocEntry"
			   and T7."CANCELED" = 'N'
			left join OSHP T8 on T8."TrnspCode" = T0."TrnspCode"
		 where T0."DocNum"  between :PedidoIni and :PedidoFim
		   and T0."TaxDate" between :DataInicio and :DataFinal
		   and (T0."CardCode" =  :Cliente or :Cliente = '*')
		--   and T1."OpenCreQty" > 0
		--   and T1."LineStatus" = 'O'
			   and T0."U_CVA_Picking_Impresso" = 1
		       and IFNULL(T0."U_CVA_Separacao_Impresso",2) = 1
		--   and T0."U_CVA_Status_Picking" <> 2
			   and T0."U_CVA_Increment_id"  is not null; --- Pedidos Gerados Magento
			end if;
end if;
end;
