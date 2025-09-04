
--call "FN_CVA_PERDCOMP_R13" (1,'2018-01-01','2018-12-31')
CREATE  PROCEDURE "SP_CVA_PERDCOMP_R13"
(
     Filial	 INT
	,DataDe	 DATE
	,DataAte DATE
 )



AS
BEGIN

CREATE GLOBAL TEMPORARY TABLE Docs
(
	 TIPO    		  VARCHAR(3)
	,CNPJ_DEC		  VARCHAR(14)
	,CNPJ_SUC		  VARCHAR(14)
	,CNPJ_CRE		  VARCHAR(14)
	,CNPJ_EMI		  VARCHAR(14)
	,NUMNR   		  VARCHAR(9)
	,SERNF			  VARCHAR(3)
	,DTEMISSA0		  DATE
	,DTENTREGA		  DATE
	,CFOP			  VARCHAR(4)
	,VL_TOTAL		  DECIMAL (19,2)
	,VL_IPI           DECIMAL (19,2)
	,VL_IPI_RAIPI     DECIMAL (19,2)
);

CREATE GLOBAL TEMPORARY TABLE Docs2
(
	 CNPJ_EMI		  VARCHAR(14)
	,NUMNR   		  VARCHAR(9)
	,SERNF			  VARCHAR(3)
	,DTEMISSA0		  DATE
	,DTENTREGA		  DATE
	,CFOP			  VARCHAR(4)
	,VL_TOTAL		  DECIMAL (19,2)
	,VL_IPI           DECIMAL (19,2)
	,VL_IPI_RAIPI     DECIMAL (19,2)
);


INSERT INTO Docs2
		SELECT 
		    CASE WHEN 
				T0."CountryB" = 'BR' THEN REPLACE(REPLACE(REPLACE(REPLACE("CRD7"."TaxId0",'/',''),'-',''),'-',''),'.','')
				ELSE '0' END,
			LPAD('0', 9 - length(T0."Serial")) + RTrim(T0."Serial") "SERIAL",
			IFNULL(T0."SeriesStr",'') "SERIESSTR",					
			T0."DocDate",
			T0."DocDate",
			T0."CFOPCode",
			SUM("LineTotal") + SUM("DistribSum") + SUM("IPI_TaxSum") + SUM("ICMS_TaxSum") "TOTAL",
			SUM(T0."IPI_TaxSum"),					
			SUM(T0."IPI_TaxSum")
			FROM "CVA_LIVRODEENTRADA_ITENS" T0
					INNER JOIN "OCRD" 
						ON "OCRD"."CardCode" = T0."CardCode"
				LEFT JOIN 
					/*(
						SELECT max(*) FROM "CRD1" 
						WHERE "CRD1"."CardCode" = "OCRD"."CardCode"
						AND "CRD1"."AdresType" = 'B'
					)*/ "CRD1" ON "CRD1"."CardCode" = "OCRD"."CardCode"
					LEFT JOIN 
					/*(
						SELECT max(*) FROM "CRD7" 
						WHERE "CRD7"."CardCode" = "OCRD"."CardCode"
						AND ISNULL(CRD7.TaxId0, '') <> '' 
					)*/ "CRD7" ON "CRD7"."CardCode" = "OCRD"."CardCode"
				WHERE T0."BPLId" = Filial 
				  and T0."DocDate" BETWEEN DataDe AND DataAte 
				GROUP BY T0."CardCode", T0."Serial", CRD7."TaxId0", T0."SeriesStr",T0."DocDate", T0."CFOPCode",T0."CountryB" ;

INSERT INTO Docs
		SELECT 
			'R13',
			(SELECT REPLACE(REPLACE(REPLACE(REPLACE("TaxIdNum",'/',''),'-',''),'-',''),'.','') FROM "OBPL" WHERE "BPLId" = '1'),
			'',
			REPLACE(REPLACE(REPLACE(REPLACE("OBPL"."TaxIdNum",'/',''),'-',''),'-',''),'.',''),
			CASE WHEN CNPJ_EMI =  '0' THEN REPLACE(REPLACE(REPLACE(REPLACE("OBPL"."TaxIdNum",'/',''),'-',''),'-',''),'.','') ELSE CNPJ_EMI END  ,
			NUMNR,
			SERNF,
			DTEMISSA0,
			DTENTREGA,
			CFOP,
			VL_TOTAL,
			VL_IPI,
			VL_IPI_RAIPI
			
			FROM Docs2
				LEFT JOIN "OBPL" ON "OBPL"."BPLId" = Filial
			WHERE IFNULL(CFOP,'') <> '' AND VL_IPI > 0;

select * from Docs;
drop table Docs;
drop table Docs2;

END