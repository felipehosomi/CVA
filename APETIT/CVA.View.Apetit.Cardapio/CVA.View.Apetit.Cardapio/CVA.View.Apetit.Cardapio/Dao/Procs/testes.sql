/*
select top 10 * from "@CVA_LN_PLANEJAMENTO";
select top 10 * from "@CVA_TURNO_QTD";
select top 10 "U_CVA_C_PADRAO", * from "@CVA_COMENSAIS";
*/
--select top 10 * from "@CVA_TURNO_QTD";
--select * from "@CVA_TURNO_QTD" WHERE "U_CVA_PLAN_ID" = 14 limit 15000;

SELECT  
		 PL."Name" AS "Dia"
		,(SELECT "U_CVA_Valor" FROM "@CVA_CUSTO_PADRAO" WHERE  "U_CVA_Mes" = TO_VARCHAR(P."U_CVA_DATA_REF",'MM/YYYY') AND  "U_CVA_Id_Servico" = P."U_CVA_ID_SERVICO") AS "ValorPadrao"
		,CASE WHEN SUM(TQ."U_CVA_QTD") > 0 THEN SUM(PL."U_CVA_TOTAL") / SUM(TQ."U_CVA_QTD") ELSE 0 END AS "ValorPerCapta"
	FROM "@CVA_PLANEJAMENTO" AS P
		INNER JOIN "@CVA_LN_PLANEJAMENTO" AS PL ON P."Code" = PL."U_CVA_PLAN_ID"
		INNER JOIN "@CVA_TURNO_QTD" AS TQ ON TQ."U_CVA_PLAN_ID" = P."Code" AND TQ."U_CVA_ID_LN_PLAN" = PL."Code"
	WHERE	P."Code" = 14
--	AND cast(PL."Name" as int)  = 1
	GROUP BY P."Code", PL."Name", P."U_CVA_ID_CONTRATO", P."U_CVA_ID_G_SERVICO",P."U_CVA_DATA_REF", P."U_CVA_ID_SERVICO"
	ORDER BY cast(PL."Name" as int) 
	
	;