CREATE PROCEDURE [dbo].[SP_CVA_APURACAO_SAIDA_DETALHE_PIS_COFINS] 
(@ano int,
 @mes int, 
 @CST decimal(12,6))
AS
BEGIN

select 'CRÉDITOS MERCADO INTERNO' as Tipo, 
		'5. VALOR TOTAL DO CRÉDITO APURADO' as Descricao,
		SUM(PIS) * 0.0165 AS PIS,
		SUM(COFINS) * 0.0760 AS COFINS
into #tmpApurado
from (
select 
	ISNULL((select sum(T3.BaseSum) FROM PCH4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 19),0) as PIS, --PIS, 
	ISNULL((select sum(T3.BaseSum) FROM PCH4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 21),0) as COFINS --PIS, 
from OPCH t0 
	INNER JOIN PCH1 T1 With(nolock) on t0.docentry = t1.DocEntry and LEFT(t1.CFOPCode,1) in (1,2)
	INNER JOIN [@AGL_SPC_NAT_BC_CRED] T5 ON T5.U_ItemCode = T1.ItemCode AND T5.U_NAT_BC_CRED in ('01','02','03','04','07')
where	   t0.CANCELED='N'
	  and  year(t0.docdate) = @ano
	  and  month(t0.docdate) = @mes
	  and  t0.docentry not in (select docentry from RPC1 T2 With(nolock) where T2.BaseEntry = T1.DocEntry AND T2.BaseType = T1.ObjType AND T2.BaseLine = T1.LineNum)

union all

select  ISNULL(SUM(U_ValorPIS),0) as PIS, --PIS, 
		ISNULL(SUM(U_ValorCOFINS),0) as COFINS --PIS, 
from [@CVA_APURACAO_MAQ]
where	   U_Ano = @ano
	  and  U_Mes = @mes

union all

select  SUM(PIS) AS PIS,
		SUM(COFINS) AS COFINS
FROM (
SELECT 
		ISNULL((select sum(T3.BaseSum) FROM RPC4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 19),0) as PIS, --PIS, 
		ISNULL((select sum(T3.BaseSum) FROM RPC4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 21),0) as COFINS --PIS, 
from ORPC t0 
	INNER JOIN RPC1 T1 With(nolock) on t0.docentry = t1.DocEntry and  t1.CFOPCode IN ('5201','5202','6201','6202')
where	   t0.CANCELED='N'
	  and  year(t0.docdate) = @ano
	  and  month(t0.docdate) = @mes) AS TMP
	  
union all

select  SUM(PIS) AS PIS,
		SUM(COFINS) AS COFINS
FROM (
SELECT 
		ISNULL((select sum(T3.BaseSum) FROM RIN4  T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 19),0) as PIS, --PIS, 
		ISNULL((select sum(T3.BaseSum) FROM RIN4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 21),0) as COFINS --PIS, 
from ORIN t0 
	INNER JOIN RIN1 T1 With(nolock) on t0.docentry = t1.DocEntry and t1.CFOPCode IN ('1201','1202','2201','2202')
where	   t0.CANCELED='N'
	  and  year(t0.docdate) = @ano
	  and  month(t0.docdate) = @mes  
	  ) AS TMP
	  ) AS TMP

insert into #tmpApurado
select 'CRÉDITOS IMPORTAÇÕES', 
		'5. VALOR TOTAL DO CRÉDITO APURADO' as Descricao, 
		SUM(PIS)  * 0.0210 AS PIS,
		SUM(COFINS) * 0.0965 AS COFINS
FROM (SELECT
		ISNULL((select sum(T3.BaseSum) FROM PCH4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 20),0) as PIS, --PIS, 
		ISNULL((select sum(T3.BaseSum) FROM PCH4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 22),0) as COFINS --PIS, 
from OPCH t0 
	INNER JOIN PCH1 T1 With(nolock) on t0.docentry = t1.DocEntry and LEFT(t1.CFOPCode,1) in (3,4)
where	   t0.CANCELED='N'
	  and  year(t0.docdate) = @ano
	  and  month(t0.docdate) = @mes
	  and  t0.docentry not in (select docentry from RPC1 T2 With(nolock) where T2.BaseEntry = T1.DocEntry AND T2.BaseType = T1.ObjType AND T2.BaseLine = T1.LineNum)) AS TMP

SELECT 'RESUMO APURAÇÃO' as Titulo,
		CST,
		SUM(LineTotal) as ValorTotalItem,
		SUM(LineTotal) AS 'Base de Calculo',
		SUM(LineTotal) * 0.021 AS PIS,
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

select 'DETALHAMENTO VALORES A RECOLHER' as Titulo, 
	   'DÉBITOS SAÍDAS' as Titulo3,
	   (select sum(PIS) FROM #tmp) AS PIS,  
	   (select sum(COFINS) FROM #tmp) AS COFINS
into #tmp2
union all
select 'DETALHAMENTO VALORES A RECOLHER', 
	   'CRÉDITOS DESCONTADOS REF. PERÍODOS ANTERIORES',
	   ISNULL((select TOP 1 U_Credito from [@CVA_APURACAO] Where U_Tipo = 101 and U_Imposto='PIS'  and U_Mes = @mes and U_Ano = @ano),0),  
	   ISNULL((select TOP 1 U_Credito from [@CVA_APURACAO] Where U_Tipo = 108 and U_Imposto='COFINS' and U_Mes = @mes and U_Ano = @ano),0)
union all
select 'DETALHAMENTO VALORES A RECOLHER', 
	   'CRÉDITOS DESCONTADOS REF. PERÍODO CORRENTE',
	   ISNULL((select PIS from #tmpApurado where Tipo='CRÉDITOS MERCADO INTERNO'),0) +  ISNULL((select PIS from #tmpApurado where Tipo='CRÉDITOS IMPORTAÇÕES'),0),
	   ISNULL((select COFINS from #tmpApurado where Tipo='CRÉDITOS MERCADO INTERNO'),0) +  ISNULL((select COFINS from #tmpApurado where Tipo='CRÉDITOS IMPORTAÇÕES'),0)
insert #tmp2
select 'DETALHAMENTO VALORES A RECOLHER',
		'SALDO A RECOLHER',
		(select PIS from #tmp2 where Titulo3 ='DÉBITOS SAÍDAS') - (select PIS from #tmp2 where Titulo3 ='CRÉDITOS DESCONTADOS REF. PERÍODOS ANTERIORES')- (select PIS from #tmp2 where Titulo3 ='CRÉDITOS DESCONTADOS REF. PERÍODO CORRENTE'),
		(select COFINS from #tmp2 where Titulo3 ='DÉBITOS SAÍDAS') - (select COFINS from #tmp2 where Titulo3 ='CRÉDITOS DESCONTADOS REF. PERÍODOS ANTERIORES')- (select COFINS from #tmp2 where Titulo3 ='CRÉDITOS DESCONTADOS REF. PERÍODO CORRENTE')

select * from #tmp2
order by Titulo3

drop table #tmp
drop table #tmp2
drop table #tmpApurado

END


