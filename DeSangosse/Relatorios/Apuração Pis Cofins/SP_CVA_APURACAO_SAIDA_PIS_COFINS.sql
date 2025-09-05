ALTER PROCEDURE SP_CVA_APURACAO_SAIDA_PIS_COFINS 
(@ano int,
 @mes int, 
 @CST decimal(12,6))
AS
BEGIN

SELECT 'RESUMO APURAÇÃO' as Titulo,
		CST,
		SUM(LineTotal) as ValorTotalItem,
		SUM(LineTotal) AS 'Base de Calculo',
		SUM(LineTotal) * 0.0210 AS PIS,
		SUM(LineTotal) * 0.0965 AS COFINS
into #tmp
	    FROM (
select 'RESUMO APURAÇÃO' as Titulo,
		t1.CSTfCOFINS AS CST, 
		T1.LineTotal 
from OINV t0 
	INNER JOIN INV1 T1 With(nolock) on t0.docentry = t1.DocEntry  
where	   t0.CANCELED='N'
	  and  year(t0.docdate) = @ano
	  and  month(t0.docdate) = @mes
	  and  t0.docentry not in (select docentry from DPI1 T2 With(nolock) where T2.BaseEntry = T1.DocEntry AND T2.BaseType = T1.ObjType AND T2.BaseLine = T1.LineNum)) AS TMP
GROUP BY CST
union all
select 'RESUMO APURAÇÃO','02',@CST, @CST, @CST * 0.0065, @CST * 0.04
order by CST


SELECT * FROM #TMP


drop table #tmp

END
