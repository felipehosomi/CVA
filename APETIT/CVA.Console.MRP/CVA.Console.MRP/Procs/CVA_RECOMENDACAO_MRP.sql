CREATE PROCEDURE "CVA_RECOMENDACAO_MRP"
(
	IN msnCode NVARCHAR(30)
)
AS BEGIN

	SELECT
		ORCM."OrderType",
		OWHS."BPLid",
		ORCM."Warehouse",
		ORCM."DueDate",
		ORCM."CardCode",
		OUOM."UomEntry",
		OUOM."UomCode",
		ORCM."ItemCode",
		ORCM."Price",
		SUM(ORCM."Quantity") / ORCM."NumPerMsr" AS "Quantity",
		ORCM."ObjAbs"
	FROM ORCM
	INNER JOIN OMSN ON OMSN."AbsEntry" = ORCM."ObjAbs"
	INNER JOIN OUOM ON OUOM."UomEntry" = ORCM."UomEntry"
	INNER JOIN OWHS ON OWHS."WhsCode"  = ORCM."Warehouse"
	WHERE 1=1
		AND (ORCM."CardCode" IS NOT NULL OR ORCM."OrderType" = 'R')
		AND OMSN."MsnCode" = msnCode
		AND ORCM."Status" = 'O' 
		AND ORCM."OrderType" IN ('P', 'R') 
	GROUP BY
		ORCM."OrderType",
		OWHS."BPLid",
		ORCM."Warehouse",
		ORCM."ItemCode",
		ORCM."DueDate",
		ORCM."CardCode",
		OUOM."UomEntry",
		OUOM."UomCode",
		ORCM."Price",
		ORCM."ObjAbs",
		ORCM."NumPerMsr"
	ORDER BY
		OWHS."BPLid",
		ORCM."Warehouse",
		ORCM."ItemCode";

END;