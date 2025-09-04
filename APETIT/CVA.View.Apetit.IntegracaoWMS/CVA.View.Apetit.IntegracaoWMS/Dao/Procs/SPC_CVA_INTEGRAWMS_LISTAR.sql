--"SBO_APETIT_PROD", C00663, 1 , 1
--set schema SBO_APETIT_PRODUCAO;
--CALL "SPC_CVA_INTEGRAWMS_LISTAR"('1','20200101', '20200130')
--CALL "SPC_CVA_INTEGRAWMS_LISTAR"('Rota do YOSHII', '20200101', '20200229');

drop PROCEDURE "SPC_CVA_INTEGRAWMS_LISTAR";



Create PROCEDURE "SPC_CVA_INTEGRAWMS_LISTAR"
(
	Rota varchar(250),
	De TIMESTAMP,
	Ate TIMESTAMP,
	Estocagem varchar(100),
	NaoSelecionado varchar(1),
	SelecionadoParcial varchar(1),
	LiberadoParcial varchar(1),
	Selecionado varchar(1)
	
)  
	LANGUAGE SQLSCRIPT AS
	    queryStr1 nvarchar(30000);
BEGIN  
--"SBO_APETIT_PROD", C00663, 1 , 1
--set schema SBO_APETIT_PRODUCAO;	



 queryStr1 := '';
queryStr1 := 'SELECT 
	''Y''										AS "Integrar"
	,O."DocEntry" 									AS "Nº do Pedido"
	,CONCAT(CONCAT(OB."BPLId", ''-''), OB."BPLName") 	AS "Filial de Destino"
	,TO_VARCHAR(O1."ShipDate",''DD/MM/YYYY'')	 		AS "Data de Entrega"
	,O1."ItemCode" 									AS "PRODUTO"
	,O1."Dscription"								AS "Descrição"
	,O1."UomCode"									AS "UN. Medida"
	,O1."Quantity"									AS "Quantidade"
	,O1."LineTotal"									AS "Valor Total"
	,OB."BPLId"
	,OB."BPLName"
	,O1."PickIdNo"									AS "Picking"
	,O1."PickStatus"								AS "Status Picking"
FROM ORDR AS O 
	INNER JOIN RDR1 AS O1 ON
		O."DocEntry" = O1."DocEntry"
	INNER JOIN OBPL AS OB ON
		O."BPLId" = OB."BPLId"
	INNER JOIN OITM OI ON O1."ItemCode" = OI."ItemCode"
	LEFT JOIN 
	(
		SELECT T0."U_CVA_FILIAL_DESTINO",T2."BPLName"
		 FROM "@CVA_LN_ROTAENTREGA" T0
		INNER JOIN "@CVA_ROTAENTREGA" T1 on T0."Code" = T1."Code"
		INNER JOIN OBPL T2 on T0."U_CVA_FILIAL_DESTINO" = T2."BPLId"
		WHERE T1."Code" = '''||Rota||'''
	) as OA on OA."U_CVA_FILIAL_DESTINO" = OB."BPLId"
WHERE 
		O."DocStatus" = ''O''
	AND	O1."LineStatus" = ''O''
	AND	O1."U_CVA_IntegradoOK" <> ''Y''
	AND O1."ShipDate" BETWEEN '''||De||''' AND '''||Ate||'''
	AND O."BPLId" IN (SELECT "U_CVA_FILIAL_DESTINO" FROM "@CVA_LN_ROTAENTREGA" WHERE "Code" = '''||Rota||''')
	AND OI."U_CVA_Categoria" in('||Estocagem||')
	AND O1."PickStatus" in ('''||NaoSelecionado||''','''||SelecionadoParcial||''','''||LiberadoParcial||''','''||Selecionado||''')
	
GROUP BY
	 O."DocEntry"
	,OB."BPLId"
	,OB."BPLName"
	,O1."ShipDate"
	,O1."ItemCode"
	,O1."Dscription"
	,O1."UomCode"
	,O1."Quantity"
	,O1."LineTotal"
	,O1."PickIdNo"
	,O1."PickStatus" '
;

execute immediate( :queryStr1);

END;
























OLD

--Create PROCEDURE "SPC_CVA_INTEGRAWMS_LISTAR"(
--	Rota varchar(250),
--	De TIMESTAMP,
--	Ate TIMESTAMP,
--	Estocagem varchar(100)
--)  
--	LANGUAGE SQLSCRIPT AS
--BEGIN  
----"SBO_APETIT_PROD", C00663, 1 , 1
----set schema SBO_APETIT_PRODUCAO;	

--SELECT 
--	'Y' 											AS "Integrar"
--	,O."DocEntry" 									AS "Nº do Pedido"
--	,CONCAT(CONCAT(OB."BPLId", '-'), OB."BPLName") 	AS "Filial de Destino"
--	,TO_VARCHAR(O1."ShipDate",'DD/MM/YYYY')	 		AS "Data de Entrega"
--	,O1."ItemCode" 									AS "PRODUTO"
--	,O1."Dscription"								AS "Descrição"
--	,O1."UomCode"									AS "UN. Medida"
--	,O1."Quantity"									AS "Quantidade"
--	,O1."LineTotal"									AS "Valor Total"
--	,OB."BPLId"
--	,OB."BPLName"
--FROM ORDR AS O 
--	INNER JOIN RDR1 AS O1 ON
--		O."DocEntry" = O1."DocEntry"
--	INNER JOIN OBPL AS OB ON
--		O."BPLId" = OB."BPLId"
--	INNER JOIN OITM OI ON O1."ItemCode" = OI."ItemCode"
--WHERE 
--		O."DocStatus" = 'O'
--	AND	O1."LineStatus" = 'O'
--	AND	O1."U_CVA_IntegradoOK" <> 'Y'
--	AND O1."ShipDate" BETWEEN De AND Ate
--	AND O."BPLId" IN (SELECT "U_CVA_FILIAL_DESTINO" FROM "@CVA_LN_ROTAENTREGA" WHERE "Code" = Rota)
--	AND OI."U_CVA_Categoria" = Estocagem
	
--GROUP BY
--	 O."DocEntry"
--	,OB."BPLId"
--	,OB."BPLName"
--	,O1."ShipDate"
--	,O1."ItemCode"
--	,O1."Dscription"
--	,O1."UomCode"
--	,O1."Quantity"
--	,O1."LineTotal"
--;

--END;
