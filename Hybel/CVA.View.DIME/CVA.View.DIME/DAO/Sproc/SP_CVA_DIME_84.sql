  
--EXEC [SP_CVA_DIME_84] '1990-01-01', '2018-06-30', 1  
alter PROCEDURE [dbo].[SP_CVA_DIME_84]  
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
  QUADRO	INT, 
  ITEM		VARCHAR (3),  
  DSC		VARCHAR (100),  
  VALOR		DECIMAL (19,2)  
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
   WHERE REFDATE BETWEEN @DateFrom AND @DateTo  AND BPLId = @BPlId
   GROUP BY ACCOUNT  
   )  
   T2 ON T1.AcctCode = T2.ACCOUNT  
 WHERE T0.TemplateId = 12 AND T0.Levels = 4  
 ORDER BY T0.FrgnName  
  
  
  
  
 INSERT INTO #DREDIME   
  SELECT 84, 84, FrgnName, NAME, SUM(Saldo)  FROM #DRE GROUP BY  FrgnName, NAME  
  
 INSERT INTO #DREDIME   
  SELECT 84, 84, '499', '(=) Total', SUM(Saldo)  FROM #DRE WHERE FrgnName BETWEEN '400'AND '498'   
   
  
   
    
 SELECT REG, QUADRO,ITEM, ABS(VALOR) SALDO FROM #DREDIME ORDER BY ITEM   
  
  
  
 DROP TABLE #DRE  
 DROP TABLE #DREDIME  
  
END