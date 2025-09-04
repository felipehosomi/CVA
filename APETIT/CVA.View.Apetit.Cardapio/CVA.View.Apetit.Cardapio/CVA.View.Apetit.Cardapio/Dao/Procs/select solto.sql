/*

SELECT  		 
 		 parceiro."CardCode" as "ParceiroId"
		,empresa."BPLId" as "EmpresaId"
		,contrato."BpCode" as "ContratoId"
		,filial."WhsCode" as "FilialId"
		,parceiro."CardName" as "Parceiro"
		,empresa."AliasName" as "Empresa"
		,filial."WhsName" as "Filial"
		,contrato."BpName" as "ContratoName"
		,PL."U_CVA_DES_GRPSERVICO" as "Grupo"
		,PL."U_CVA_DATA_REF" as "DataReferencia"
		,PL."U_CVA_DIA_SEMANA" as "DiaReferencia"
		,EXTRACT(DAY FROM PL."U_CVA_DATA_REF") as "DiaReferenciaNum"
		,PL."U_CVA_ID_MODEL_CARD" as "ModeloCardId"
		,lin_modelo."LineId" as "ModeloLinId"
		,lin_modelo."U_CVA_TIPO_PRATO" as "TipoPratoId"
		,lin_modelo."U_CVA_TIPO_PRATO_DES" as "TipoPrato"
		,CONCAT(MONTH(PL."U_CVA_DATA_REF"), CONCAT('/', YEAR(PL."U_CVA_DATA_REF"))) as "MesReferencia"
		,PLLin."U_CVA_INSUMO_DES" as "InsumoDes"
		,PLLin."U_CVA_PERCENT" as "InsumoPer"
		,PL."U_CVA_QTD_COMENSAIS" as "ComensaisQtd"
		,PL."U_CVA_CUSTO_PADRAO" as "CP_Unitário"
		,(PL."U_CVA_CUSTO_PADRAO" * PL."U_CVA_QTD_COMENSAIS") as "CP_Total"
		,PL."U_CVA_TOT_C_PADRAO" as "CM_Unitário"
		,PL."U_CVA_TOT_C_MEDIO" as "CM_Total"
		,PL."U_CVA_TOT_VARIACAO_V" as "VAR_Unit"
		,(PL."U_CVA_TOT_VARIACAO_V" * PL."U_CVA_QTD_COMENSAIS") as "VAR_Total"
		,PL."U_CVA_TOT_VARIACAO_P" as "VAR_Per"
	FROM "@CVA_PLANEJAMENTO" AS PL
		INNER JOIN OOAT AS contrato ON 
			contrato."Number" = PL."U_CVA_ID_CONTRATO"
		INNER JOIN OCRD AS parceiro ON
			parceiro."CardCode" = contrato."BpCode"
		INNER JOIN OWHS as filial ON
			filial."WhsCode" = parceiro."U_CVA_FILIAL"
		INNER JOIN OBPL as empresa ON
			empresa."BPLId" = filial."BPLid"
		INNER JOIN "@CVA_MCARDAPIO" as modelo on
			PL."U_CVA_ID_MODEL_CARD" = modelo."Code"
		INNER JOIN "@CVA_LIN_MCARDAPIO" as lin_modelo on
			modelo."Code" = lin_modelo."Code"
		LEFT JOIN "@CVA_LN_PLANEJAMENTO" AS PLLin on
				PLLin."U_CVA_MODELO_LIN_ID" = lin_modelo."LineId" 
			AND PLLin."Code" = PL."Code"
			
WHERE EXTRACT(DAY FROM PL."U_CVA_DATA_REF") = 21
order by lin_modelo."LineId", PL."U_CVA_DATA_REF"

;*/


SELECT 

		 CONCAT(MONTH(PL."U_CVA_DATA_REF"), CONCAT('/', YEAR(PL."U_CVA_DATA_REF"))) as "MesReferencia"
		,EXTRACT(DAY FROM PL."U_CVA_DATA_REF") as "DiaReferenciaNum"
		,PL."U_CVA_DIA_SEMANA" as "DiaReferencia"
		--,PL."U_CVA_DES_GRPSERVICO" as "Grupo"
		--,PL."U_CVA_DATA_REF" as "DataReferencia"
		--,PL."U_CVA_ID_MODEL_CARD" as "ModeloCardId"
		,PL."U_CVA_QTD_COMENSAIS" as "ComensaisQtd"
 		 /*,parceiro."CardCode" as "ParceiroId"
		,empresa."BPLId" as "EmpresaId"
		,contrato."BpCode" as "ContratoId"
		,filial."WhsCode" as "FilialId"
		,parceiro."CardName" as "Parceiro"
		,empresa."AliasName" as "Empresa"
		,filial."WhsName" as "Filial"
		,contrato."BpName" as "ContratoName"
		,lin_modelo."LineId" as "ModeloLinId"
		,lin_modelo."U_CVA_TIPO_PRATO" as "TipoPratoId"
		,lin_modelo."U_CVA_TIPO_PRATO_DES" as "TipoPrato"
		,PLLin."U_CVA_INSUMO_DES" as "InsumoDes"
		,PLLin."U_CVA_PERCENT" as "InsumoPer"*/
	FROM "@CVA_PLANEJAMENTO" AS PL
		/*INNER JOIN OOAT AS contrato ON 
			contrato."Number" = PL."U_CVA_ID_CONTRATO"
		INNER JOIN OCRD AS parceiro ON
			parceiro."CardCode" = contrato."BpCode"
		INNER JOIN OWHS as filial ON
			filial."WhsCode" = parceiro."U_CVA_FILIAL"
		INNER JOIN OBPL as empresa ON
			empresa."BPLId" = filial."BPLid"
		INNER JOIN "@CVA_MCARDAPIO" as modelo on
			PL."U_CVA_ID_MODEL_CARD" = modelo."Code"
		INNER JOIN "@CVA_LIN_MCARDAPIO" as lin_modelo on
			modelo."Code" = lin_modelo."Code"
		LEFT JOIN "@CVA_LN_PLANEJAMENTO" AS PLLin on
			PLLin."U_CVA_MODELO_LIN_ID" = lin_modelo."LineId"
			
			
WHERE EXTRACT(DAY FROM PL."U_CVA_DATA_REF") = 21
order by lin_modelo."LineId", PL."U_CVA_DATA_REF"
*/
;

--SELECT * FROM "@CVA_LN_PLANEJAMENTO"



--SELECT * FROM "@CVA_LN_PLANEJAMENTO"

