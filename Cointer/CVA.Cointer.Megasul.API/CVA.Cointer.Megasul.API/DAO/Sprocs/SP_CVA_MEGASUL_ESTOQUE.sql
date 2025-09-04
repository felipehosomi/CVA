ALTER PROCEDURE "SP_CVA_MEGASUL_ESTOQUE"
(
    limitRecords 	INT,
    offsetRecords 	INT,
    dataDe			DATE,
    itemCode		NVARCHAR(200)
)
LANGUAGE SQLSCRIPT SQL SECURITY INVOKER 
AS
execSQL nvarchar(5000);	
BEGIN
	CREATE LOCAL TEMPORARY TABLE #TMP_ITEM ("ItemCode" NVARCHAR(200));
	CREATE LOCAL TEMPORARY TABLE #TMP_ESTOQUE ("codigo_produto_sap" NVARCHAR(200), "lote" NVARCHAR(200), "numero_serie" NVARCHAR(200), "quantidade" NUMERIC(19, 2));
	CREATE LOCAL TEMPORARY TABLE #TMP_TOTAL ("TotalRecord" BIGINT);

	execSQL :=
	'INSERT INTO #TMP_ITEM
	SELECT "ItemCode" 
	FROM OITM 
	WHERE OITM."QryGroup2" = ''Y'' AND ("ManBtchNum" = ''Y'' OR "ManSerNum" = ''Y'') AND ("ItemCode" = ''' || itemCode || ''' OR  '''' = ''' || itemCode || ''')
	ORDER BY "ItemCode";';
	
	EXEC (:execSQL);
	
	--SELECT * FROM #TMP_ITEM;
	
	INSERT INTO #TMP_TOTAL
	SELECT	1
	FROM #TMP_ITEM "ITEM"
		LEFT JOIN ITL1 ON ITL1."ItemCode" = ITEM."ItemCode"
		LEFT JOIN OITL ON OITL."LogEntry" = ITL1."LogEntry"
	WHERE IFNULL(OITL."DocDate", '19000101') <= dataDe AND OITL."LocCode" = '101'
	GROUP BY
		ITEM."ItemCode",
		ITL1."MdAbsEntry";
	
	execSQL :=
	'
	INSERT INTO #TMP_ESTOQUE
	SELECT
		ITEM."ItemCode"			"codigo_produto_sap",
		OBTN."DistNumber" 		"lote",
		OSRN."DistNumber" 		"numero_serie",
		SUM(ITL1."Quantity")	"quantidade"
	FROM #TMP_ITEM "ITEM"
		LEFT JOIN ITL1 ON ITL1."ItemCode" = ITEM."ItemCode"
		LEFT JOIN OITL ON OITL."LogEntry" = ITL1."LogEntry"
		LEFT JOIN OBTN
			ON OBTN."AbsEntry" = ITL1."MdAbsEntry"
			AND OBTN."ItemCode" = ITL1."ItemCode"
		LEFT JOIN OSRN
			ON OSRN."AbsEntry" = ITL1."MdAbsEntry"
			AND OSRN."ItemCode" = ITL1."ItemCode"
			AND IFNULL(OBTN."DistNumber", '''') = ''''
	WHERE IFNULL(OITL."DocDate", ''19000101'') <= ''' || TO_VARCHAR(dataDe, 'YYYYMMDD') || ''' AND OITL."LocCode" = ''101''
	GROUP BY
		ITEM."ItemCode",
		OBTN."DistNumber",
		OSRN."DistNumber"
	ORDER BY ITEM."ItemCode" limit ' || limitRecords || ' offset ' || offsetRecords || ';';
	
	EXEC (:execSQL);
	
	SELECT 
	(SELECT COUNT(*) FROM #TMP_TOTAL) "TotalRecords",
	* FROM #TMP_ESTOQUE;
	
	DROP TABLE #TMP_ITEM;
	DROP TABLE #TMP_ESTOQUE;
	DROP TABLE #TMP_TOTAL;
END;