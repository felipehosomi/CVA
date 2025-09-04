DO BEGIN
--set schema SBO_APETIT_PRODUCAO;

DECLARE Rota 	varchar(50) = '1';
DECLARE De   	TIMESTAMP = '20200101';
DECLARE Ate 	TIMESTAMP = '20200130';

SELECT 
	'Y' 											AS "Integrar"
	,O."DocEntry" 									AS "Nº do Pedido"
	,CONCAT(CONCAT(OB."BPLId", '-'), OB."BPLName") 	AS "Filial de Destino"
	,TO_VARCHAR(O1."ShipDate",'DD/MM/YYYY')	 		AS "Data de Entrega"
	,O1."ItemCode" 									AS "PRODUTO"
	,O1."Dscription"								AS "Descrição"
	,O1."UomCode"									AS "UN. Medida"
	,O1."Quantity"									AS "Quantidade"
	,O1."LineTotal"									AS "Valor Total"
FROM ORDR AS O 
	INNER JOIN RDR1 AS O1 ON
		O."DocEntry" = O1."DocEntry"
	INNER JOIN OBPL AS OB ON
		O."BPLId" = OB."BPLId"
WHERE 
		O."DocStatus" = 'O'
	AND	O1."LineStatus" = 'O'
	AND	O1."U_CVA_IntegradoOK" <> 'Y'
	AND O1."ShipDate" BETWEEN De AND Ate
	
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
;

END;