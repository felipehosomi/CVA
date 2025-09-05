DECLARE @AnoFiltro int = 2017

DECLARE @DataIni VARCHAR(MAX) = CONCAT(@AnoFiltro    , '-09-01')
DECLARE @DataFim VARCHAR(MAX) = CONCAT(@AnoFiltro + 1, '-08-31')

CREATE TABLE #PrevisaoReceberAUX(
	 Tipo  VARCHAR(100)
	,Ano   INT
	,Mes   INT
	,Valor DECIMAL(16,8)
)


INSERT INTO #PrevisaoReceberAUX
SELECT 
	 'PEDIDO DE VENDA'
	,YEAR(T0.DocDate) 
	,MONTH(T0.DocDate)
	,SUM(T1.OpenQty * T1.Price)

FROM ORDR T0
INNER JOIN RDR1 T1 ON T0.DocEntry = T1.DocEntry

WHERE T0.DocDate BETWEEN @dataIni AND @DataFim
  AND T1.LineStatus = 'O'

GROUP BY T0.DocDate	



CREATE TABLE #PrevisaoReceber(
	 Tipo      VARCHAR(MAX)
	,Setembro  NUMERIC(19,4)
	,Outubro   NUMERIC(19,4)
	,Novembro  NUMERIC(19,4)
	,Dezembro  NUMERIC(19,4)
	,Janeiro   NUMERIC(19,4)
	,Fevereiro NUMERIC(19,4)
	,Marco     NUMERIC(19,4)
	,Abril     NUMERIC(19,4)
	,Maio      NUMERIC(19,4)
	,Junho     NUMERIC(19,4)
	,Julho     NUMERIC(19,4)
	,Agosto    NUMERIC(19,4))


	
/* --- Insert na #PrevisaoRecebimento --- */
BEGIN
	INSERT INTO #PrevisaoReceber
	SELECT Tipo, Valor, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	FROM #PrevisaoReceberAUX WHERE MES = 9
	
	INSERT INTO #PrevisaoReceber
	SELECT Tipo, 0, Valor, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	FROM #PrevisaoReceberAUX WHERE MES = 10
	
	INSERT INTO #PrevisaoReceber
	SELECT Tipo, 0, 0, Valor, 0, 0, 0, 0, 0, 0, 0, 0, 0
	FROM #PrevisaoReceberAUX WHERE MES = 11
	
	INSERT INTO #PrevisaoReceber
	SELECT Tipo, 0, 0, 0, Valor, 0, 0, 0, 0, 0, 0, 0, 0
	FROM #PrevisaoReceberAUX WHERE MES = 12
	
	INSERT INTO #PrevisaoReceber
	SELECT Tipo, 0, 0, 0, 0, Valor, 0, 0, 0, 0, 0, 0, 0
	FROM #PrevisaoReceberAUX WHERE MES = 1
	
	INSERT INTO #PrevisaoReceber
	SELECT Tipo, 0, 0, 0, 0, 0, Valor, 0, 0, 0, 0, 0, 0
	FROM #PrevisaoReceberAUX WHERE MES = 2
	
	INSERT INTO #PrevisaoReceber
	SELECT Tipo, 0, 0, 0, 0, 0, 0, Valor, 0, 0, 0, 0, 0
	FROM #PrevisaoReceberAUX WHERE MES = 3
	
	INSERT INTO #PrevisaoReceber
	SELECT Tipo, 0, 0, 0, 0, 0, 0, 0, Valor, 0, 0, 0, 0
	FROM #PrevisaoReceberAUX WHERE MES = 4
	
	INSERT INTO #PrevisaoReceber
	SELECT Tipo, 0, 0, 0, 0, 0, 0, 0, 0, Valor, 0, 0, 0
	FROM #PrevisaoReceberAUX WHERE MES = 5
	
	INSERT INTO #PrevisaoReceber
	SELECT Tipo, 0, 0, 0, 0, 0, 0, 0, 0, 0, Valor, 0, 0
	FROM #PrevisaoReceberAUX WHERE MES = 6
	
	INSERT INTO #PrevisaoReceber
	SELECT Tipo, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Valor, 0
	FROM #PrevisaoReceberAUX WHERE MES = 7
	
	INSERT INTO #PrevisaoReceber
	SELECT Tipo, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Valor
	FROM #PrevisaoReceberAUX WHERE MES = 8
END
/* - - - - - - - - -  - - - - - - - - - - */


SELECT
	 TIPO 
	,ISNULL(SUM(Setembro),0)  AS 'Setembro'
	,ISNULL(SUM(Outubro),0)	  AS 'Outubro'
	,ISNULL(SUM(Novembro),0)  AS 'Novembro'
	,ISNULL(SUM(Dezembro),0)  AS 'Dezembro'
	,ISNULL(SUM(Janeiro),0)   AS 'Janeiro'
	,ISNULL(SUM(Fevereiro),0) AS 'Fevereiro'
	,ISNULL(SUM(Marco),0)	  AS 'Marco'
	,ISNULL(SUM(Abril),0)	  AS 'Abril'
	,ISNULL(SUM(Maio),0)	  AS 'Maio'
	,ISNULL(SUM(Junho),0)	  AS 'Junho'
	,ISNULL(SUM(Julho),0)	  AS 'Julho'
	,ISNULL(SUM(Agosto),0)    AS 'Agosto'
	
FROM #PrevisaoReceber
GROUP BY TIPO

DROP TABLE #PrevisaoReceberAUX
DROP TABLE #PrevisaoReceber