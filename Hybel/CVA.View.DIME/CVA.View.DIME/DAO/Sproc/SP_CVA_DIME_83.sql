
--EXEC [SP_CVA_DIME_83] '2018-01-01', '2018-06-30', 1
ALTER PROCEDURE [dbo].[SP_CVA_DIME_83]
(
	@DateFrom DATETIME = NULL,
	@DateTo DATETIME = NULL,
	@BPlId INT = NULL
)
AS
BEGIN


--SET @DateFrom = '2018-06-01'
--SET @DateTo  =  '2018-06-30'



CREATE TABLE #DREDIME
	(
		REG		INT,
		QUADRO  INT,
		ITEM	VARCHAR (3),
		DSC		VARCHAR (100),
		VALOR	DECIMAL (19,2)
	)


SELECT 
	T0.CatId,
	T0.Name,
	T0.Levels,
	T0.FrgnName,
	T1.AcctCode,
	ISNULL(T2.Saldo,0.00) Saldo

	INTO #DRE
	FROM OFRC T0 WITH(NOLOCK) 
		LEFT JOIN FRC1 T1 WITH(NOLOCK) ON T0.TemplateId = T1.TemplateId AND T0.CatId = T1.CatId
		LEFT  JOIN 
			( 
			SELECT ACCOUNT,  SUM(Debit) - SUM(Credit) AS Saldo FROM JDT1 WITH(NOLOCK) 
			WHERE REFDATE BETWEEN @DateFrom AND @DateTo
			GROUP BY ACCOUNT
			)
			T2 ON T1.AcctCode = T2.ACCOUNT
	WHERE T0.TemplateId = 9 AND T0.Levels = 4
	ORDER BY T0.FrgnName




	INSERT INTO #DREDIME 
		SELECT 83,83, FrgnName, NAME, SUM(Saldo)  FROM #DRE GROUP BY  FrgnName, NAME

	INSERT INTO #DREDIME 
		SELECT 83,83, '320', '(=) Receita líquida vendas/serviços', SUM(Saldo)  FROM #DRE WHERE FrgnName IN ('310','311') 

	INSERT INTO #DREDIME 
		SELECT 83,83, '330', '(=) Lucro bruto', 
			   CASE WHEN SUM(Saldo)	< 0 THEN  SUM(Saldo) ELSE 0.00 END 		 FROM #DRE WHERE FrgnName IN ('310','311','323') 
	INSERT INTO #DREDIME 
		SELECT 83,83, '331', '(=) Prejuízo bruto', 
			   CASE WHEN SUM(Saldo)	> 0 THEN  SUM(Saldo) ELSE 0.00 END 		 FROM #DRE WHERE FrgnName IN ('310','311','323') 

	INSERT INTO #DREDIME 
		SELECT 83,83, '340', '(=) Lucro operacional', 
			   CASE WHEN SUM(Saldo)	< 0 THEN  SUM(Saldo) ELSE 0.00 END 		 FROM #DRE WHERE FrgnName IN ('310','311','323', '333', '335') 

	INSERT INTO #DREDIME 
		SELECT 83,83, '341', '(=) Prejuízo operacional', 
			   CASE WHEN SUM(Saldo)	> 0 THEN  SUM(Saldo) ELSE 0.00 END 		 FROM #DRE WHERE FrgnName IN ('310','311','323', '333', '335') 

	INSERT INTO #DREDIME 
		SELECT 83,83, '350', '(=) Resultado antes do I.R. e da contribuição social', 
			   CASE WHEN SUM(Saldo)	< 0 THEN  SUM(Saldo) ELSE 0.00 END 		 FROM #DRE WHERE FrgnName IN ('310','311','323', '333', '335', '343','345') 

	INSERT INTO #DREDIME 
		SELECT 83,83, '351', '(=) Resultado antes do I.R. e da contribuição social', 
			   CASE WHEN SUM(Saldo)	> 0 THEN  SUM(Saldo) ELSE 0.00 END 		 FROM #DRE WHERE FrgnName IN ('310','311','323', '333', '335', '343','345') 

	INSERT INTO #DREDIME 
		SELECT 83,83, '360', '(=) Resultado após o I.R. e a contribuição social', 
			   CASE WHEN SUM(Saldo)	< 0 THEN  SUM(Saldo) ELSE 0.00 END 		 FROM #DRE WHERE FrgnName IN ('310','311','323', '333', '335', '343','345','353','354') 

	INSERT INTO #DREDIME 
		SELECT 83,83, '361', '(=) Resultado negativo após o I.R. e a contribuição social', 
			   CASE WHEN SUM(Saldo)	> 0 THEN  SUM(Saldo) ELSE 0.00 END 		 FROM #DRE WHERE FrgnName IN ('310','311','323', '333', '335', '343','345','353','354') 

	INSERT INTO #DREDIME 
		SELECT 83,83, '398', '(=) Prejuízo do exercício', 
			   CASE WHEN SUM(Saldo)	> 0 THEN  SUM(Saldo) ELSE 0.00 END 		 FROM #DRE WHERE FrgnName IN ('310','311','323', '333', '335', '343','345','353','354') 

	INSERT INTO #DREDIME 
		SELECT 83,83, '399', '(=) Lucro  do exercício', 
			   CASE WHEN SUM(Saldo)	< 0 THEN  SUM(Saldo) ELSE 0.00 END 		 FROM #DRE WHERE FrgnName IN ('310','311','323', '333', '335', '343','345','353','354') 


	


		
	SELECT REG, QUADRO, ITEM, ABS(VALOR) VALOR FROM #DREDIME ORDER BY ITEM 



	DROP TABLE #DRE
	DROP TABLE #DREDIME

END