DECLARE @cardcode NVARCHAR(30), @dias INT, @retorno VARCHAR(10)
 
SELECT @cardcode = $[$4.0]
SET @retorno = 'FALSE'

IF EXISTS (SELECT TOP 1 1 FROM OCRD WITH(NOLOCK) WHERE CardCode = @cardcode AND ISNULL(U_CVA_Liberado, CAST('19000101' AS DATEIME)) < CAST(GETDATE() AS DATE))
BEGIN
	IF EXISTS(SELECT TOP 1 1 FROM OCRD WITH(NOLOCK) WHERE CardCode = @cardcode AND (ISNULL(CAST(Free_Text AS NVARCHAR(MAX)), '') <> '' OR Balance > CreditLine))
	BEGIN 
	 SET @retorno = 'TRUE'
	END
	ELSE IF NOT EXISTS(SELECT TOP 1 1 FROM OINV WITH(NOLOCK) WHERE CANCELED = 'N' AND CardCode = @cardcode AND DocDate > DATEADD(DAY, -31, GETDATE()))
	BEGIN
	SET @retorno = 'TRUE'
	END
	ELSE
	BEGIN
	DECLARE invcursor CURSOR FORWARD_ONLY FOR
	SELECT DATEDIFF(day, GETDATE(), T1.DueDate)
	FROM OINV T0 INNER JOIN INV6 T1 ON T1.DocEntry = T0.DocEntry 
	 WHERE T1.[Status] = 'O' AND T0.CardCode = @cardcode OR @cardcode NOT IN (SELECT Cardcode from OINV)
 
	UNION
	SELECT DATEDIFF(day, GETDATE(), DueDate)
	   FROM OBOE 
	  WHERE BoeStatus NOT IN ('P','C') AND CardCode = @cardcode
	UNION
	SELECT  DATEDIFF(day, GETDATE(), JDT1.DueDate)
	FROM JDT1        
	   INNER JOIN OJDT ON OJDT.TransId = JDT1.TransId        
	   INNER JOIN OCRD ON OCRD.CardCode = JDT1.ShortName AND OCRD.CardType = 'C'        
	  WHERE JDT1.BalDueDeb <> 0        
	   AND JDT1.TransType = 30 
	   AND JDT1.ShortName = @cardcode       
	  GROUP BY OCRD.CardName, OCRD.CardCode, OJDT.TransId, OJDT.TransType, OJDT.RefDate, JDT1.DueDate
 
	OPEN invcursor
	FETCH NEXT FROM invcursor INTO @dias
 
	WHILE @@FETCH_STATUS = 0
	BEGIN
		 IF @dias <=-1
		 BEGIN
			   SET @retorno = 'TRUE'
			   BREAK
		 END
		 FETCH NEXT FROM invcursor INTO @dias
	END
 
	CLOSE invcursor
	DEALLOCATE invcursor
	END
END
SELECT @retorno
