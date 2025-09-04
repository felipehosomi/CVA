
CREATE PROCEDURE "SP_CVA_PERDCOMP_R12" 
 (
     Filial			INT
	,DataDe			DATE
	,DataAte		DATE
  )
AS
BEGIN

CREATE LOCAL TEMPORARY  TABLE #Docs 
	(
		TIPO    		  VARCHAR(3),
		CNPJ_DEC		  VARCHAR(14),
		CNPJ_SUC		  VARCHAR(14),
		CNPJ_CRE		  VARCHAR(14),
		PER_ANO			  VARCHAR(4),
		PER_MES			  VARCHAR(2),
		APUR			  INT,
		CFOP			  VARCHAR(4),
		VL_BC_IPI         DECIMAL (19,2),
		VL_IPI            DECIMAL (19,2),
		VL_ISNT_IPI       DECIMAL (19,2),
		VL_OUT_IPI        DECIMAL (19,2)
	);


CREATE LOCAL TEMPORARY  TABLE #Docs2 
	(
		PER_ANO			  VARCHAR(4),
		PER_MES			  VARCHAR(2),
		CFOP			  VARCHAR(4),
		VL_BC_IPI         DECIMAL (19,2),
		VL_IPI            DECIMAL (19,2),
		VL_ISNT_IPI       DECIMAL (19,2),
		VL_OUT_IPI        DECIMAL (19,2)
	);


INSERT INTO #Docs2
		SELECT 
			CAST(YEAR("DocDate") AS CHAR(4)),
			CASE WHEN length (CAST(MONTH("DocDate") AS CHAR(4))) > 2 THEN CAST(MONTH("DocDate") AS CHAR(4)) ELSE 
					 LPAD('0', 2 - length (CAST(MONTH("DocDate") AS CHAR(4)))) + RTrim(CAST(MONTH("DocDate") AS CHAR(4))) END, 
			"CFOPCode",
			SUM("IPI_BaseSum"),
			SUM("IPI_TaxSum"),
			SUM("IPI_U_ExcAmtL"),
			SUM("IPI_U_OthAmtL")
			FROM  "CVA_LIVRODESAIDA_ITENS" 
				WHERE "BPLId" = Filial AND "DocDate" BETWEEN DataDe AND DataAte 
				GROUP BY CAST(YEAR("DocDate") AS CHAR(4)), CAST(MONTH("DocDate") AS CHAR(4)), "CFOPCode"
				ORDER BY "CFOPCode";

INSERT INTO #Docs
		SELECT 
			'R12',
			(SELECT REPLACE(REPLACE(REPLACE(REPLACE("TaxIdNum",'/',''),'-',''),'-',''),'.','') FROM "OBPL" WHERE "BPLId" = 1),
			'',
			REPLACE(REPLACE(REPLACE(REPLACE("OBPL"."TaxIdNum",'/',''),'-',''),'-',''),'.',''),
			PER_ANO,
			PER_MES,
			0,
			CFOP,
			VL_BC_IPI,
			VL_IPI,
			VL_ISNT_IPI,
			VL_OUT_IPI 

			FROM #Docs2
				LEFT JOIN "OBPL" ON "OBPL"."BPLId" = Filial
			WHERE IFNULL(CFOP,'') <> '' AND VL_IPI > 0;

select * from #Docs;

	
DROP TABLE #Docs2;
DROP TABLE #Docs;


END