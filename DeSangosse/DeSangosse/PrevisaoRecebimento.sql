DECLARE @AnoFiltro int = 2017

DECLARE @DataIni VARCHAR(MAX) = CONCAT(@AnoFiltro    , '-09-01')
DECLARE @DataFim VARCHAR(MAX) = CONCAT(@AnoFiltro + 1, '-08-31')


CREATE TABLE #PrevisaoRecebimentoAUX(
	 ANO         INT
	,MES         INT
	,Rotulo      VARCHAR(MAX)
	,Bonificacao NUMERIC(19,4)
	,Quantidate  NUMERIC(19,4)
	,Preco       NUMERIC(19,4)
	,Valor       NUMERIC(19,4)
	,Mes01       NUMERIC(19,4)
	,Mes02       NUMERIC(19,4)
	,Mes03       NUMERIC(19,4)
	,Mes04       NUMERIC(19,4)
	,Mes05       NUMERIC(19,4)
	,Mes06       NUMERIC(19,4)
	,Mes07       NUMERIC(19,4)
	,Mes08       NUMERIC(19,4)
	,Mes09       NUMERIC(19,4)
	,Mes10       NUMERIC(19,4)
	,Mes11       NUMERIC(19,4)
	,Mes12       NUMERIC(19,4)
)


/* --- Insert na #PrevisaoRecebimentoAUX --- */
BEGIN
	INSERT INTO #PrevisaoRecebimentoAUX
	SELECT 
		 YEAR(T0.DATE)
		,MONTH(T0.DATE)
		,T3.U_RotuloLinha
		,T3.U_Bonificacao
		,(T0.Quantity - ((T3.U_Bonificacao * T0.Quantity) / 100))
		,T2.PRICE
		,((T0.Quantity - ((T3.U_Bonificacao * T0.Quantity) / 100)) * T2.PRICE)
		,((T0.Quantity - ((T3.U_Bonificacao * T0.Quantity) / 100)) * T2.PRICE) * (SELECT (U_Mes01 / 100) FROM [@CVA_REGRA_CONDPGTO] WHERE U_RotuloLinha = T3.U_RotuloLinha)
		,((T0.Quantity - ((T3.U_Bonificacao * T0.Quantity) / 100)) * T2.PRICE) * (SELECT (U_Mes02 / 100) FROM [@CVA_REGRA_CONDPGTO] WHERE U_RotuloLinha = T3.U_RotuloLinha)
		,((T0.Quantity - ((T3.U_Bonificacao * T0.Quantity) / 100)) * T2.PRICE) * (SELECT (U_Mes03 / 100) FROM [@CVA_REGRA_CONDPGTO] WHERE U_RotuloLinha = T3.U_RotuloLinha)
		,((T0.Quantity - ((T3.U_Bonificacao * T0.Quantity) / 100)) * T2.PRICE) * (SELECT (U_Mes04 / 100) FROM [@CVA_REGRA_CONDPGTO] WHERE U_RotuloLinha = T3.U_RotuloLinha)
		,((T0.Quantity - ((T3.U_Bonificacao * T0.Quantity) / 100)) * T2.PRICE) * (SELECT (U_Mes05 / 100) FROM [@CVA_REGRA_CONDPGTO] WHERE U_RotuloLinha = T3.U_RotuloLinha)
		,((T0.Quantity - ((T3.U_Bonificacao * T0.Quantity) / 100)) * T2.PRICE) * (SELECT (U_Mes06 / 100) FROM [@CVA_REGRA_CONDPGTO] WHERE U_RotuloLinha = T3.U_RotuloLinha)
		,((T0.Quantity - ((T3.U_Bonificacao * T0.Quantity) / 100)) * T2.PRICE) * (SELECT (U_Mes07 / 100) FROM [@CVA_REGRA_CONDPGTO] WHERE U_RotuloLinha = T3.U_RotuloLinha)
		,((T0.Quantity - ((T3.U_Bonificacao * T0.Quantity) / 100)) * T2.PRICE) * (SELECT (U_Mes08 / 100) FROM [@CVA_REGRA_CONDPGTO] WHERE U_RotuloLinha = T3.U_RotuloLinha)
		,((T0.Quantity - ((T3.U_Bonificacao * T0.Quantity) / 100)) * T2.PRICE) * (SELECT (U_Mes09 / 100) FROM [@CVA_REGRA_CONDPGTO] WHERE U_RotuloLinha = T3.U_RotuloLinha)
		,((T0.Quantity - ((T3.U_Bonificacao * T0.Quantity) / 100)) * T2.PRICE) * (SELECT (U_Mes10 / 100) FROM [@CVA_REGRA_CONDPGTO] WHERE U_RotuloLinha = T3.U_RotuloLinha)
		,((T0.Quantity - ((T3.U_Bonificacao * T0.Quantity) / 100)) * T2.PRICE) * (SELECT (U_Mes11 / 100) FROM [@CVA_REGRA_CONDPGTO] WHERE U_RotuloLinha = T3.U_RotuloLinha)
		,((T0.Quantity - ((T3.U_Bonificacao * T0.Quantity) / 100)) * T2.PRICE) * (SELECT (U_Mes12 / 100) FROM [@CVA_REGRA_CONDPGTO] WHERE U_RotuloLinha = T3.U_RotuloLinha)
	
	 FROM FCT1 T0
	
	INNER JOIN OITM T1 ON T1.ITEMCODE = T0.ITEMCODE
	INNER JOIN ITM1 T2 ON T2.ITEMCODE = T0.ITEMCODE
	INNER JOIN [@CVA_REGRA_CONDPGTO] T3 ON T3.U_RotuloLinha = T1.U_EASY_CCUSTO
	
	WHERE T0.DATE BETWEEN @DataIni AND @DataFim
	  AND T2.PRICELIST = 1
	
	GROUP BY  YEAR(T0.DATE), MONTH(T0.DATE), T3.U_RotuloLinha, T3.U_Bonificacao, T1.ITEMCODE, T3.U_RotuloLinha, T2.PRICE, T0.Quantity
	
	ORDER BY T3.U_RotuloLinha
END
/* - - - - - - - - - - - - - - - - - - - - - */


SELECT
	 ANO
	,MES
	,Rotulo
	,SUM(ISNULL(Mes01,0)) AS '01 MESES'
	,SUM(ISNULL(Mes02,0)) AS '02 MESES'
	,SUM(ISNULL(Mes03,0)) AS '03 MESES'
	,SUM(ISNULL(Mes04,0)) AS '04 MESES'
	,SUM(ISNULL(Mes05,0)) AS '05 MESES'
	,SUM(ISNULL(Mes06,0)) AS '06 MESES'
	,SUM(ISNULL(Mes07,0)) AS '07 MESES'
	,SUM(ISNULL(Mes08,0)) AS '08 MESES'
	,SUM(ISNULL(Mes09,0)) AS '09 MESES'
	,SUM(ISNULL(Mes10,0)) AS '10 MESES'
	,SUM(ISNULL(Mes11,0)) AS '11 MESES'
	,SUM(ISNULL(Mes12,0)) AS '12 MESES'
	
FROM #PrevisaoRecebimentoAUX

GROUP BY ANO, MES, ROTULO


CREATE TABLE #PrevisaoRecebimento(
	 Rotulo    VARCHAR(MAX)
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
	INSERT INTO #PrevisaoRecebimento
	SELECT Rotulo, Mes01, Mes02, Mes03, Mes04, Mes05, Mes06, Mes07, Mes08, Mes09, Mes10, Mes11, Mes12
	FROM #PrevisaoRecebimentoAUX WHERE MES = 8
	
	INSERT INTO #PrevisaoRecebimento
	SELECT Rotulo, Mes12, Mes01, Mes02, Mes03, Mes04, Mes05, Mes06, Mes07, Mes08, Mes09, Mes10, Mes11
	FROM #PrevisaoRecebimentoAUX WHERE MES = 9
	
	INSERT INTO #PrevisaoRecebimento
	SELECT Rotulo, Mes11, Mes12, Mes01, Mes02, Mes03, Mes04, Mes05, Mes06, Mes07, Mes08, Mes09, Mes10
	FROM #PrevisaoRecebimentoAUX WHERE MES = 10
	
	INSERT INTO #PrevisaoRecebimento
	SELECT Rotulo, Mes10, Mes11, Mes12, Mes01, Mes02, Mes03, Mes04, Mes05, Mes06, Mes07, Mes08, Mes09
	FROM #PrevisaoRecebimentoAUX WHERE MES = 11
	
	INSERT INTO #PrevisaoRecebimento
	SELECT Rotulo, Mes09, Mes10, Mes11, Mes12, Mes01, Mes02, Mes03, Mes04, Mes05, Mes06, Mes07, Mes08
	FROM #PrevisaoRecebimentoAUX WHERE MES = 12
	
	INSERT INTO #PrevisaoRecebimento
	SELECT Rotulo, Mes08, Mes09, Mes10, Mes11, Mes12, Mes01, Mes02, Mes03, Mes04, Mes05, Mes06, Mes07
	FROM #PrevisaoRecebimentoAUX WHERE MES = 1
	
	INSERT INTO #PrevisaoRecebimento
	SELECT Rotulo, Mes07, Mes08, Mes09, Mes10, Mes11, Mes12, Mes01, Mes02, Mes03, Mes04, Mes05, Mes06
	FROM #PrevisaoRecebimentoAUX WHERE MES = 2
	
	INSERT INTO #PrevisaoRecebimento
	SELECT Rotulo, Mes06, Mes07, Mes08, Mes09, Mes10, Mes11, Mes12, Mes01, Mes02, Mes03, Mes04, Mes05
	FROM #PrevisaoRecebimentoAUX WHERE MES = 3
	
	INSERT INTO #PrevisaoRecebimento
	SELECT Rotulo, Mes05, Mes06, Mes07, Mes08, Mes09, Mes10, Mes11, Mes12, Mes01, Mes02, Mes03, Mes04
	FROM #PrevisaoRecebimentoAUX WHERE MES = 4
	
	INSERT INTO #PrevisaoRecebimento
	SELECT Rotulo, Mes04, Mes05, Mes06, Mes07, Mes08, Mes09, Mes10, Mes11, Mes12, Mes01, Mes02, Mes03
	FROM #PrevisaoRecebimentoAUX WHERE MES = 5
	
	INSERT INTO #PrevisaoRecebimento
	SELECT Rotulo, Mes03, Mes04, Mes05, Mes06, Mes07, Mes08, Mes09, Mes10, Mes11, Mes12, Mes01, Mes02
	FROM #PrevisaoRecebimentoAUX WHERE MES = 6
	
	INSERT INTO #PrevisaoRecebimento
	SELECT Rotulo, Mes02, Mes03, Mes04, Mes05, Mes06, Mes07, Mes08, Mes09, Mes10, Mes11, Mes12, Mes01
	FROM #PrevisaoRecebimentoAUX WHERE MES = 7
END
/* - - - - - - - - -  - - - - - - - - - - */



SELECT 
	 ROTULO 
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
	 
FROM #PrevisaoRecebimento

GROUP BY ROTULO



DROP TABLE #PrevisaoRecebimentoAUX
DROP TABLE #PrevisaoRecebimento