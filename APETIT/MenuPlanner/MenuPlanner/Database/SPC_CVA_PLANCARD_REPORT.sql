ALTER PROCEDURE "SPC_CVA_PLANCARD_REPORT"(
	PlanId varchar(250)
)  
	LANGUAGE SQLSCRIPT AS
BEGIN  
	comsumption = SELECT IFNULL(SUM(OITW."AvgPrice"), 0.00) "Value"  FROM OWOR
	    INNER JOIN WOR1 ON WOR1."DocEntry" = OWOR."DocEntry"
	    INNER JOIN OITW ON OITW."ItemCode" = WOR1."ItemCode" AND OITW."WhsCode" = WOR1."wareHouse"
	WHERE OWOR."Type" = 'P'
	AND OWOR."ItemCode" = '09.99.99.999'
	AND OWOR."U_CVA_PlanCode" = PlanId;
	
	rawMaterial = 
		SELECT SUM("Value") "Value", "U_Day"
		FROM
		(
			select (sum(OITW."AvgPrice" * ITT1."Quantity") * PL."U_CVA_PERCENT" / 100) *
			(SELECT sum(MNP2."U_Quantity") FROM "@CVA_MNP2" as MNP2 WHERE MNP2."DocEntry" = P."DocEntry" AND MNP2."U_Day" = PL."U_Day") "Value"
			,
			PL."U_Day"
			FROM "@CVA_PLANEJAMENTO" P
				inner join "@CVA_LN_PLANEJAMENTO" as PL on PL."DocEntry" = P."DocEntry"
				inner join OITT ON OITT."Code" = PL."U_CVA_INSUMO"
				inner join ITT1 on ITT1."Father" = OITT."Code"
				inner join OITW on OITW."ItemCode" = ITT1."Code"
			where PL."DocEntry" = PlanId
			and OITW."WhsCode" = (select OWHS."WhsCode" from OWHS inner join OOAT on OOAT."U_CVA_FILIAL" = OWHS."BPLid" where OOAT."AbsID" = P."U_AbsID")
			GROUP BY P."DocEntry", PL."U_Day", PL."U_CVA_PERCENT"
		) GROUP BY "U_Day";
	
	select RM."U_Day" as "Dia", TO_VARCHAR(P."U_CVA_DATA_REF",'MM/YYYY'),
		   (SELECT AVG("U_CVA_Valor")
				FROM "@CVA_CUSTO_PADRAO" 
			   WHERE "U_CVA_Mes" = TO_VARCHAR(P."U_CVA_DATA_REF",'MM/YYYY') 
			     AND "U_CVA_Id_Servico" = P."U_CVA_ID_SERVICO") AS "ValorPadrao",
			CASE WHEN ((SELECT "Value" FROM :comsumption) + "Value") > 0
		   	THEN ((SELECT "Value" FROM :comsumption) + "Value") /(SELECT sum(MNP2."U_Quantity") FROM "@CVA_MNP2" as MNP2 WHERE MNP2."DocEntry" = PlanId AND MNP2."U_Day" = RM."U_Day") 
		   	ELSE 0.00
		   	END "ValorPerCapta"
	  from "@CVA_PLANEJAMENTO" as P
			 INNER JOIN :rawMaterial RM ON 1 = 1
	 where P."DocEntry" = PlanId;
END;