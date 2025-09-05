
/****** Object:  StoredProcedure [dbo].[SP_CVA_APURACAO_ENTRADA_PIS_COFINS]    Script Date: 12/12/2018 15:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--[SP_CVA_APURACAO_ENTRADA_PIS_COFINS] 2018,8

ALTER PROCEDURE [dbo].[SP_CVA_APURACAO_ENTRADA_PIS_COFINS] 
(@ano int,
 @mes int)
AS
BEGIN

select  'CR�DITOS MERCADO INTERNO' as Titulo, '1. C�DIGO DO TIPO DE CR�DITO' as Descricao, 101 as PIS, 101 AS COFINS
union all
select 'CR�DITOS MERCADO INTERNO' as Titulo, 
		'2. BASE DE C�LCULO DO CR�DITO EM REAIS' as Descricao, 
		SUM(PIS) AS PIS,
		SUM(COFINS) AS COFINS
from (
select 
	ISNULL((select sum(T3.BaseSum) FROM PCH4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 19),0) as PIS, --PIS, 
	ISNULL((select sum(T3.BaseSum) FROM PCH4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 21),0) as COFINS --PIS, 
from OPCH t0 
	INNER JOIN PCH1 T1 With(nolock) on t0.docentry = t1.DocEntry and LEFT(t1.CFOPCode,1) in (1,2)
	INNER JOIN [@AGL_SPC_NAT_BC_CRED] T5 ON T5.U_ItemCode = T1.ItemCode AND T5.U_NAT_BC_CRED in ('01','02','03','04','07')
where	   t0.CANCELED='N'
	  and  (model <> 0 or U_agl_compor='S')
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
      and seqcode <> 1
	  and  (model <> 0 or U_agl_compor='S')
	  and  year(t0.docdate) = @ano
	  and  month(t0.docdate) = @mes  
	  ) AS TMP
	  
	  ) as TMP
union all
select 'CR�DITOS MERCADO INTERNO', 
		'4. AL�QUOTA DO CR�DITO' as Descricao, 
		1.65 as PIS, 
		7.60 as COFINS 
union all
select 'CR�DITOS MERCADO INTERNO', 
		'5. VALOR TOTAL DO CR�DITO APURADO' as Descricao,
		SUM(PIS) * 0.0165 AS PIS,
		SUM(COFINS) * 0.0760 AS COFINS
from (
select 
	ISNULL((select sum(T3.BaseSum) FROM PCH4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 19),0) as PIS, --PIS, 
	ISNULL((select sum(T3.BaseSum) FROM PCH4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 21),0) as COFINS --PIS, 
from OPCH t0 
	INNER JOIN PCH1 T1 With(nolock) on t0.docentry = t1.DocEntry and LEFT(t1.CFOPCode,1) in (1,2)
	INNER JOIN [@AGL_SPC_NAT_BC_CRED] T5 ON T5.U_ItemCode = T1.ItemCode AND T5.U_NAT_BC_CRED in ('01','02','03','04','07')
where	   t0.CANCELED='N'
	  and  (model <> 0 or U_agl_compor='S')
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
      and seqcode <> 1
	  and  (model <> 0 or U_agl_compor='S')
	  and  year(t0.docdate) = @ano
	  and  month(t0.docdate) = @mes  
	  ) AS TMP
	  
	  ) as TMP
union all
--Tabela [@AGL_SPC_NAT_BC_CRED] - Trazer a soma das entradas (OPCH) com CFOP 1 e 2 de todos os itens no per�odo com o c�digo 01 no campo U_NAT_BC_CRED 
SELECT  'DETALHAMENTO DA BASE DE C�LCULO DO CR�DITO',
		CASE T5.U_NAT_BC_CRED  WHEN '01' THEN  '01 - AQUISI��O DE BENS PARA REVENDA'
							   WHEN '02' THEN  '02 - AQUISI��O DE BENS UTILIZADOS COMO INSUMO'
							   WHEN '03' THEN  '03 - AQUISI��O DE SERVI�OS UTILIZADOS COMO INSUMO'
							   WHEN '04' THEN  '04 - ENERGIA EL�TRICA E T�RMICA, INCLUSIVE SOB A FORMA DE VAPOR'
							   WHEN '07' THEN  '07 - ARMAZENAGEM DE MERCADORIA E FRETE NA OPERA��O DE VENDA'
							   end as Descricao,
		SUM(PIS),
		SUM(COFINS)
FROM (
select  T1.ItemCode,
		ISNULL((select sum(T3.BaseSum) FROM PCH4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 19),0) as PIS, --PIS, 
		ISNULL((select sum(T3.BaseSum) FROM PCH4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 21),0) as COFINS --PIS, 
from OPCH t0 
	INNER JOIN PCH1 T1 With(nolock) on t0.docentry = t1.DocEntry and LEFT(t1.CFOPCode,1) in (1,2)
where	   t0.CANCELED='N'
	  and  (model <> 0 or U_agl_compor='S')
	  and  year(t0.docdate) = @ano
	  and  month(t0.docdate) = @mes
	  and  t0.docentry not in (select docentry from RPC1 T2 With(nolock) where T2.BaseEntry = T1.DocEntry AND T2.BaseType = T1.ObjType AND T2.BaseLine = T1.LineNum)) AS TMP
INNER JOIN [@AGL_SPC_NAT_BC_CRED] T5 ON T5.U_ItemCode = TMP.ItemCode AND T5.U_NAT_BC_CRED in ('01','02','03','04','07')
group by CASE T5.U_NAT_BC_CRED  WHEN '01' THEN  '01 - AQUISI��O DE BENS PARA REVENDA'
							    WHEN '02' THEN  '02 - AQUISI��O DE BENS UTILIZADOS COMO INSUMO'
							    WHEN '03' THEN  '03 - AQUISI��O DE SERVI�OS UTILIZADOS COMO INSUMO'
							    WHEN '04' THEN  '04 - ENERGIA EL�TRICA E T�RMICA, INCLUSIVE SOB A FORMA DE VAPOR'
							    WHEN '07' THEN  '07 - ARMAZENAGEM DE MERCADORIA E FRETE NA OPERA��O DE VENDA' end
union all

select 'DETALHAMENTO DA BASE DE C�LCULO DO CR�DITO', 
		'09 - M�QUINAS, EQUIPAMENTOS E OUTROS BENS INCORPORADOS AO ATIVO IMOBILIZADO' as Descricao, 
	    ISNULL(SUM(U_ValorPIS),0) as PIS, --PIS, 
		ISNULL(SUM(U_ValorCOFINS),0) as COFINS --PIS, 
from [@CVA_APURACAO_MAQ]
where	   U_Ano = @ano
	  and  U_Mes = @mes

union all

select 'DETALHAMENTO DA BASE DE C�LCULO DO CR�DITO', 
		'12 - DEVOLU��O DE VENDAS SUJEITAS � INCID�NCIA N�O-CUMULATIVA' as Descricao,
		SUM(PIS) AS PIS,
		SUM(COFINS) AS COFINS
FROM (
SELECT 
		ISNULL((select sum(T3.BaseSum) FROM RPC4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 19),0) as PIS, --PIS, 
		ISNULL((select sum(T3.BaseSum) FROM RPC4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 21),0) as COFINS --PIS, 
from ORPC t0 
	INNER JOIN RPC1 T1 With(nolock) on t0.docentry = t1.DocEntry and  t1.CFOPCode IN ('5201','5202','6201','6202')
where	   t0.CANCELED='N'
	  and  year(t0.docdate) = @ano
	  and  month(t0.docdate) = @mes
union all
SELECT 
		ISNULL((select sum(T3.BaseSum) FROM RIN4  T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 19),0) as PIS, --PIS, 
		ISNULL((select sum(T3.BaseSum) FROM RIN4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 21),0) as COFINS --PIS, 
from ORIN t0 
	INNER JOIN RIN1 T1 With(nolock) on t0.docentry = t1.DocEntry and t1.CFOPCode IN ('1201','1202','2201','2202')
where	   t0.CANCELED='N'
      and seqcode <> 1
	  and  (model <> 0 or U_agl_compor='S')
	  and  year(t0.docdate) = @ano
	  and  month(t0.docdate) = @mes  
	  ) AS TMP

union all

select  'CR�DITOS IMPORTA��ES' as Titulo, '1. C�DIGO DO TIPO DE CR�DITO' as Descricao, 108 as PIS, 108 AS CONFINS
union all
select 'CR�DITOS IMPORTA��ES', 
		'2. BASE DE C�LCULO DO CR�DITO EM REAIS' as Descricao, 
		SUM(PIS) AS PIS,
		SUM(COFINS) AS COFINS
FROM (

SELECT 
		ISNULL((select sum(T3.BaseSum) FROM PCH4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 20),0) as PIS, --PIS, 
		ISNULL((select sum(T3.BaseSum) FROM PCH4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 22),0) as COFINS --PIS, 
from OPCH t0 
	INNER JOIN PCH1 T1 With(nolock) on t0.docentry = t1.DocEntry and LEFT(t1.CFOPCode,1) in (3,4)
where	   t0.CANCELED='N'
	  and  year(t0.docdate) = @ano
	  and  month(t0.docdate) = @mes
	  and  (model <> 0 or U_agl_compor='S')
	  and  t0.docentry not in (select docentry from RPC1 T2 With(nolock) where T2.BaseEntry = T1.DocEntry AND T2.BaseType = T1.ObjType AND T2.BaseLine = T1.LineNum)
union all
--3.	Considerar as devolu��es de nota fiscal de sa�da (ORIN) como cr�dito de imposto com os cfops: ('1201','1202','2201','2202')
SELECT 
		ISNULL((select sum(T3.BaseSum) FROM RIN4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 20),0) as PIS, --PIS, 
		ISNULL((select sum(T3.BaseSum) FROM RIN4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 22),0) as COFINS --PIS, 
from ORIN t0 
	INNER JOIN RIN1 T1 With(nolock) on t0.docentry = t1.DocEntry and t1.CFOPCode IN ('1201','1202','2201','2202')
where	   t0.CANCELED='N'
      and seqcode <> 1
	  and  (model <> 0 or U_agl_compor='S')
	  and  year(t0.docdate) = @ano
	  and  month(t0.docdate) = @mes
 ) AS TMP

union all
select 'CR�DITOS IMPORTA��ES', '4. AL�QUOTA DO CR�DITO' as Descricao, 
		2.10 as PIS, 
		9.65 as COFINS 
union all
select 'CR�DITOS IMPORTA��ES', 
		'5. VALOR TOTAL DO CR�DITO APURADO' as Descricao, 
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
	  and  (model <> 0 or U_agl_compor='S')
	  and  t0.docentry not in (select docentry from RPC1 T2 With(nolock) where T2.BaseEntry = T1.DocEntry AND T2.BaseType = T1.ObjType AND T2.BaseLine = T1.LineNum)
union all
--3.	Considerar as devolu��es de nota fiscal de sa�da (ORIN) como cr�dito de imposto com os cfops: ('1201','1202','2201','2202')
SELECT 
		ISNULL((select sum(T3.BaseSum) FROM RIN4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 20),0) as PIS, --PIS, 
		ISNULL((select sum(T3.BaseSum) FROM RIN4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 22),0) as COFINS --PIS, 
from ORIN t0 
	INNER JOIN RIN1 T1 With(nolock) on t0.docentry = t1.DocEntry and t1.CFOPCode IN ('1201','1202','2201','2202')
where	   t0.CANCELED='N'
      and seqcode <> 1
	  and  (model <> 0 or U_agl_compor='S')
	  and  year(t0.docdate) = @ano
	  and  month(t0.docdate) = @mes) AS TMP
union all	
SELECT  'DETALHAMENTO DA BASE DE CR�DITO IMPORTA��O',
		CASE T5.U_NAT_BC_CRED  WHEN '01' THEN  '01 - AQUISI��O DE BENS PARA REVENDA'
							   WHEN '02' THEN  '02 - AQUISI��O DE BENS UTILIZADOS COMO INSUMO'
							   WHEN '03' THEN  '03 - AQUISI��O DE SERVI�OS UTILIZADOS COMO INSUMO'
							   WHEN '04' THEN  '04 - ENERGIA EL�TRICA E T�RMICA, INCLUSIVE SOB A FORMA DE VAPOR'
							   WHEN '07' THEN  '07 - ARMAZENAGEM DE MERCADORIA E FRETE NA OPERA��O DE VENDA'
							   WHEN '09' THEN  '09 - M�QUINAS, EQUIPAMENTOS E OUTROS BENS INCORPORADOS AO ATIVO IMOBILIZADO'
							   WHEN '12' THEN  '12 - DEVOLU��O DE VENDAS SUJEITAS � INCID�NCIA N�O-CUMULATIVA'
							   ELSE '99 - OUTROS' end as Descricao,
		SUM(PIS),
		SUM(COFINS)
FROM (
select  T1.ItemCode,
		ISNULL((select sum(T3.BaseSum) FROM PCH4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 20),0) as PIS, --PIS, 
		ISNULL((select sum(T3.BaseSum) FROM PCH4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 22),0) as COFINS --PIS, 
from OPCH t0 
	INNER JOIN PCH1 T1 With(nolock) on t0.docentry = t1.DocEntry and LEFT(t1.CFOPCode,1) in (3,4)
where	   t0.CANCELED='N'
	  and  year(t0.docdate) = @ano
	  and  month(t0.docdate) = @mes
	  and  (model <> 0 or U_agl_compor='S')
	  and  t0.docentry not in (select docentry from RPC1 T2 With(nolock) where T2.BaseEntry = T1.DocEntry AND T2.BaseType = T1.ObjType AND T2.BaseLine = T1.LineNum)
union all
--3.	Considerar as devolu��es de nota fiscal de sa�da (ORIN) como cr�dito de imposto com os cfops: ('1201','1202','2201','2202')
SELECT  T1.ItemCode,
		ISNULL((select sum(T3.BaseSum) FROM RIN4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 20),0) as PIS, --PIS, 
		ISNULL((select sum(T3.BaseSum) FROM RIN4 T3 WHERE  T3.DocEntry = t1.DocEntry and T3.LineNum = T1.LineNum and T3.TaxRate > 0 and T3.staType = 22),0) as COFINS --PIS, 
from ORIN t0 
	INNER JOIN RIN1 T1 With(nolock) on t0.docentry = t1.DocEntry and t1.CFOPCode IN ('1201','1202','2201','2202')
where	   t0.CANCELED='N'
      and seqcode <> 1
	  and  (model <> 0 or U_agl_compor='S')
	  and  year(t0.docdate) = @ano
	  and  month(t0.docdate) = @mes	  
	  ) AS TMP
LEFT JOIN [@AGL_SPC_NAT_BC_CRED] T5 ON T5.U_ItemCode = TMP.ItemCode 
group by CASE T5.U_NAT_BC_CRED  WHEN '01' THEN  '01 - AQUISI��O DE BENS PARA REVENDA'
							    WHEN '02' THEN  '02 - AQUISI��O DE BENS UTILIZADOS COMO INSUMO'
							    WHEN '03' THEN  '03 - AQUISI��O DE SERVI�OS UTILIZADOS COMO INSUMO'
							    WHEN '04' THEN  '04 - ENERGIA EL�TRICA E T�RMICA, INCLUSIVE SOB A FORMA DE VAPOR'
							    WHEN '07' THEN  '07 - ARMAZENAGEM DE MERCADORIA E FRETE NA OPERA��O DE VENDA'
							    WHEN '09' THEN  '09 - M�QUINAS, EQUIPAMENTOS E OUTROS BENS INCORPORADOS AO ATIVO IMOBILIZADO'
							    WHEN '12' THEN  '12 - DEVOLU��O DE VENDAS SUJEITAS � INCID�NCIA N�O-CUMULATIVA'
							    ELSE '99 - OUTROS' end

END

