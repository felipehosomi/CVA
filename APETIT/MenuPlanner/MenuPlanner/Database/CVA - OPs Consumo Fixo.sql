SELECT T0."DocNum", T0."Status", T0."ItemCode", T0."ProdName", T0."PlannedQty", T0."Warehouse", T1."Consumption" "Total Consumo", T0."PostDate", T0."DueDate", T0."RlsDate"
FROM OWOR T0
INNER JOIN
(
	SELECT WOR1."DocEntry", SUM(OITW."AvgPrice" * WOR1."PlannedQty") "Consumption"
	FROM WOR1
		INNER JOIN OITW ON OITW."ItemCode" = WOR1."ItemCode" AND OITW."WhsCode" = WOR1."wareHouse"
	GROUP BY WOR1."DocEntry"
) T1 ON T1."DocEntry" = T0."DocEntry"

WHERE T0."Type" = 'P' AND T0."ItemCode" = '09.99.99.999' AND T0."U_CVA_PlanCode" = $[$etDocNum.0.TEXT]