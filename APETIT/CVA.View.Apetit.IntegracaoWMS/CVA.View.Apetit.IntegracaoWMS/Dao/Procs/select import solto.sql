DO BEGIN
--set schema SBO_APETIT_PRODUCAO;

DECLARE De   	TIMESTAMP = '20200101';
DECLARE Ate 	TIMESTAMP = '20200130';

SELECT 
	 O."DocEntry"							AS "DocEntry"
	,O1."ItemCode" 							AS "ItemCode"
	,OB."TaxIdNum"							AS "CNPJ_CPF_CLI"
	,OB."BPLName"							AS "CLI_NOME"
	,CURRENT_DATE							AS "DATA_ARQ"
	,'  '									AS "COD_DESTINATARIO"
	,OB."BPLName"							AS "RAZ_SOC_DESTINATARIO"
	,CONCAT(CONCAT(CONCAT(OB."AddrType", ' '), CONCAT(OB."Street", ',')),OB."StreetNo")	
											AS "ENDE_ENTREGA"
	,OB."Block"								AS "BAIRRO_ENTREGA"
	,OB."ZipCode"							AS "CEP_ENTREGA"
	,OB."City"								AS "CIDADE_ENTREGA"
	,OB."State"								AS "ESTADO_ENTREGA"
	,O."DocNum"								AS "NUM_PEDIDO"
	,O1."ShipDate"							AS "DATA_PEDIDO"
	--,SUM(O1."InvQty")							AS "QTDE_ITEMS"
	--,SUM(O1."LineTotal")						AS "VLR_TOT_PEDIDO"
	--,SUM(O1."InvQty" * IT."SWeight1")			AS "PESO_LIQ_PEDIDO"
	,'          '							AS "NUM_NF"
	,'          '							AS "SIF"
	,'          '							AS "NUM_CARGA"
	--,SUM(O1."InvQty" * IT."SWeight1")			AS "PESO_BRT_PEDIDO"
	,OB."TaxIdNum"							AS "CNPJ_CPF_DEST"
	,'          '							AS "COD_PRODUTO_ANT"
	,O1."Dscription"						AS "DESCR_PRODUTO"
	,O1."InvQty"							AS "QTDE_PRODUTO"
	,O1."LineTotal"	/ O1."InvQty"			AS "VLR_UNITARIO"
	,O1."InvQty" * IT."SWeight1"			AS "PESO_LIQ_ITEM"
	,O."DocNum"								AS "NUM_PEDIDO"
	,'           '							AS "PLACA_VEICULO"
	,O1."ItemCode"							AS "COD_PRODUTO"
	,O1."PackQty"							AS "QTDE_VOLUMES"
	,'          '							AS "NUM_LOTE_FABR"
	,O1."LineTotal"							AS "LineTotal"
	,IT."SWeight1"							AS "SWeight1"
	
FROM ORDR AS O 
	INNER JOIN RDR1 AS O1 ON
		O."DocEntry" = O1."DocEntry"
	INNER JOIN OBPL AS OB ON
		O."BPLId" = OB."BPLId"
	INNER JOIN OITM AS IT ON
		O1."ItemCode" = IT."ItemCode"
WHERE 
		O."DocStatus" = 'O'
	AND	O1."LineStatus" = 'O'
	AND	O1."U_CVA_IntegradoOK" <> 'Y'
	AND O1."ShipDate" BETWEEN De AND Ate
	
	AND O1."ItemCode" IN ('10.01.01.001.00', '10.01.01.001.00', '10.01.05.010.00')
	AND O."DocEntry" IN (14,15,15)					
;

END;

--SELECT * FROM OITM
