
SELECT 
	W."AvgPrice"
FROM OCRD AS O
	INNER JOIN OBPL AS B ON
		O."U_CVA_FILIAL" = B."BPLId"
	INNER JOIN OITW AS W ON
		B."DflWhs" = W."WhsCode"
WHERE 
		O."CardCode" = 'C00001'
	AND	O."U_CVA_FILIAL" = '1'
	AND W."ItemCode" = '01.09.01.271'
GROUP BY
	 	B."DflWhs"
	 	,W."ItemCode"
		
;	
/*
SELECT 
	"DflWhs",
	*
FROM OBPL
;
*/