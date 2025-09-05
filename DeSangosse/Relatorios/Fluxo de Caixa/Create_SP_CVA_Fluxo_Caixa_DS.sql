USE [SBO_DESANGOSSE_PRD]
GO
/****** Object:  StoredProcedure [dbo].[SP_CVA_FLUXO_CAIXA_DS]    Script Date: 05/11/2018 16:11:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--SP_CVA_FLUXO_CAIXA_DS 2018, '0026',1,'S','S','S','N'
CREATE PROCEDURE [dbo].[SP_CVA_FLUXO_CAIXA_DS]
(
 @ano int,
 @cenarioMRP varchar(4),
 @priceList int,
 @vendas char(1),
 @compra char(1),
 @fluxoCaixa char(1),
 @contaCaixa char(1)
 )
as
begin

--declare @ano int
--declare @cenarioMRP varchar(4)
--declare @priceList int
--declare @vendas char(1)
--declare @compra char(1)
--declare @fluxoCaixa char(1)
--declare @contaCaixa char(1)

--set @ano = 2018
--set @cenarioMRP = '0026' 
--set @priceList = 1
--set @vendas = 'S'
--set @compra = 'S'
--set @vendas = 'S'
--set @fluxoCaixa = 'S'
--set @contaCaixa = 'S'

declare @dataini datetime
declare @datafim datetime

set @dataini = convert(varchar(4), @ano) + '/08/01'
set @datafim = convert(varchar(4), @ano + 1) + '/08/31'

create table #tmp
(Order_Row int,
 Collumn_1 varchar(150) collate database_default,
 Collumn_2 varchar(150) collate database_default,
 Ano int,
 Mes int,
 Valor decimal(19,6))
----------------------------------------------------------------------------------------------
----------------------------- PREVISÃO RECEBIMENTO (CENÁRIO MRP) -----------------------------
----------------------------------------------------------------------------------------------
if(@cenarioMRP != '-1')
Begin
	insert #tmp
	select  0,
			'PREVISÃO RECEBIMENTO (CENÁRIO MRP)',
			U_EASY_CCusto, 
			year(FCT1.Date) as Ano,
			month(FCT1.Date) as Mes,
   		   ((sum(FCT1.Quantity) -  (sum(FCT1.Quantity)  * (ISNULL(t1.U_Percentual,0) /100))) - 
		   ((sum(FCT1.Quantity) -  (sum(FCT1.Quantity)  * (ISNULL(t1.U_Percentual,0) /100))) * (((case datediff(MONTH, @dataini, FCT1.Date)  * 30
				when 30 then
					ISNULL((select U_30 From [@CVA_LPROD_DIA_PGTO] T0 Where T0.Code = U_EASY_CCusto),0)
				when 60 then
					ISNULL((select U_60 From [@CVA_LPROD_DIA_PGTO] T0 Where T0.Code = U_EASY_CCusto),0)
				when 90 then
					ISNULL((select U_90 From [@CVA_LPROD_DIA_PGTO] T0 Where T0.Code = U_EASY_CCusto),0)
				when 120 then
					ISNULL((select U_120 From [@CVA_LPROD_DIA_PGTO] T0 Where T0.Code = U_EASY_CCusto),0)
				when 150 then
					ISNULL((select U_150 From [@CVA_LPROD_DIA_PGTO] T0 Where T0.Code = U_EASY_CCusto),0)
				when 180 then
					ISNULL((select U_180 From [@CVA_LPROD_DIA_PGTO] T0 Where T0.Code = U_EASY_CCusto),0)
				when 210 then
					ISNULL((select U_210 From [@CVA_LPROD_DIA_PGTO] T0 Where T0.Code = U_EASY_CCusto),0)
				when 240 then
					ISNULL((select U_240 From [@CVA_LPROD_DIA_PGTO] T0 Where T0.Code = U_EASY_CCusto),0)
				when 270 then
					ISNULL((select U_270 From [@CVA_LPROD_DIA_PGTO] T0 Where T0.Code = U_EASY_CCusto),0)
				when 300 then
					ISNULL((select U_300 From [@CVA_LPROD_DIA_PGTO] T0 Where T0.Code = U_EASY_CCusto),0)
				when 330 then
					ISNULL((select U_330 From [@CVA_LPROD_DIA_PGTO] T0 Where T0.Code = U_EASY_CCusto),0)
				when 360 then
					ISNULL((select U_360 From [@CVA_LPROD_DIA_PGTO] T0 Where T0.Code = U_EASY_CCusto),0)
			 else 0 end)) / 100))) * isnull(sum(ITM1.Price),0) as Valor
		 
	from OFCT 
		inner join FCT1 on OFCT.AbsID =  FCT1.AbsID
		left join OITM on OITM.ItemCode = FCT1.ItemCode
		left join ITM1 on ITM1.ItemCode = OITM.ItemCode and PriceList = @priceList
		left join [@CVA_LPRD_BONIFICA] T1 ON T1.Code = U_EASY_CCusto
	Where	  StartDate >= @dataini
		  and EndDate <= @datafim
		  and OFCT.Code = @cenarioMRP 
	group by U_EASY_CCusto, datediff(MONTH, @dataini, FCT1.Date), year(FCT1.Date), month(FCT1.Date), t1.U_Percentual


	select t1.Code as Code, 
		   year(t1.U_Periodo_MRP) as Ano,  
		   month(t1.U_Periodo_MRP) as Mes, 
		   sum(Valor) as Valor
	into #tmpRegraMRP
	from #tmp t0
		inner join  [@CVA_LPRD_REGRA] t1 on  Convert(datetime,convert(varchar(4), t0.Ano)  +'/'+ convert(varchar(4), t0.Mes) + '/01') >= t1.U_Periodo_DE 
											 and  DateAdd(m,1, Convert(datetime,convert(varchar(4), t0.Ano)  +'/'+ convert(varchar(4), t0.Mes) + '/01')) -1  <= t1.U_Periodo_ATE 
	where  t0.Collumn_2 = '210' 
	group by t1.Code, year(t1.U_Periodo_MRP),  month(t1.U_Periodo_MRP)

	update #tmp set Valor = t1.Valor + t0.Valor
	from #tmpRegraMRP t0
		inner join #tmp t1 on t0.Code = t1.Collumn_2 and t0.Ano = t1.Ano and t0.Mes = t1.Mes

	update #tmp set Valor = 0
	from #tmp t0
		inner join  [@CVA_LPRD_REGRA] t1  on  Convert(datetime,convert(varchar(4), t0.Ano)  +'/'+ convert(varchar(4), t0.Mes) + '/01') >= t1.U_Periodo_DE 
											 and  DateAdd(m,1, Convert(datetime,convert(varchar(4), t0.Ano)  +'/'+ convert(varchar(4), t0.Mes) + '/01')) -1  <= t1.U_Periodo_ATE 
	where t0.Collumn_2 = '210'

	insert #tmp
	select 1, 
  		  'VALOR TOTAL',
		  'VALOR TOTAL',
		  Ano,
		  Mes,
		  sum(Valor)
	from #tmp
	group by Ano, Mes

	drop table #tmpRegraMRP
end

----------------------------------------------------------------------------------------------
----------------------------- PREVISÃO A RECEBER ---------------------------------------------
----------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------
----------------------------- PEDIDOS DE VENDAS  ---------------------------------------------
----------------------------------------------------------------------------------------------
if(@vendas = 'S')
Begin
	insert #tmp
	select  2,
			'PREVISÃO A RECEBER',
			'PEDIDOS DE VENDA', 
			year(ORDR.DocDate) as Ano,
			month(ORDR.DocDate) as Mes,
   			SUM(ORDR.DocTotal)
	from ORDR 
	Where	  DocDate >= @dataini
		  and DocDate <= @datafim
		  and DocStatus = 'O'
	group by year(ORDR.DocDate) ,month(ORDR.DocDate)
end
----------------------------------------------------------------------------------------------
----------------------------- LANÇAMENTOS PERIÓDICOS  ----------------------------------------
----------------------------------------------------------------------------------------------
create table #tmpLcmRecorrente
(DataTransacao DateTime,
 Valor decimal(19,6))

DECLARE @Debit DECIMAL(19,6)
DECLARE @NexDeu DATETIME
DECLARE @Remind INT

DECLARE recorrente_cursor CURSOR FOR   
select Remind, NextDeu, Debit  from ORCR 
	inner join RCR1 on ORCR.RcurCode = RCR1.RcurCode AND  ORCR.Instance = RCR1.Instance
	inner join OACT ON OACT.AcctCode = RCR1.AcctCode 
where     ORCR.Frequency='M'
	  AND ORCR.LimitRtrns ='Y'
	  AND OACT.Finanse ='Y'
	  AND RCR1.Debit > 0
	  AND ORCR.Instance = 0
	  AND NextDeu >= @dataini
OPEN recorrente_cursor  
FETCH NEXT FROM recorrente_cursor INTO @Remind, @NexDeu, @Debit  

WHILE @@FETCH_STATUS = 0  
BEGIN  

WHILE @Remind >= 0
BEGIN
   insert #tmpLcmRecorrente
   select DateAdd(MONTH,@Remind, @NexDeu),
		  @Debit
	SET @Remind =  @Remind - 1
END
FETCH NEXT FROM recorrente_cursor INTO @Remind, @NexDeu, @Debit 
END  

CLOSE recorrente_cursor  
DEALLOCATE recorrente_cursor  

insert #tmp
select  2,
		'PREVISÃO A RECEBER',
		'LANÇAMENTOS RECORRENTES', 
		year(DataTransacao),
		month(DataTransacao),
		sum(Valor)	
from #tmpLcmRecorrente
where DataTransacao >= @dataini
	and DataTransacao <= @datafim
group by 
	year(DataTransacao),
	month(DataTransacao)

delete from #tmpLcmRecorrente 
----------------------------------------------------------------------------------------------
-------------------------- PEDIDOS DE VENDAS RECORRENTES--------------------------------------
----------------------------------------------------------------------------------------------
DECLARE recorrente_cursor CURSOR FOR   
select datediff(month, startdate, EndDate), startdate, odrf.DocTotal from ORCP 
	inner join ODRF on ORCP.DraftEntry = ODRF.DocEntry 
where 
	DocObjType = 17 
	and IsRemoved = 'N'
	and Frequency ='M'
	and StartDate >=  @dataini
OPEN recorrente_cursor  
FETCH NEXT FROM recorrente_cursor INTO @Remind, @NexDeu, @Debit  

WHILE @@FETCH_STATUS = 0  
BEGIN  

WHILE @Remind >= 0
BEGIN
   insert #tmpLcmRecorrente
   select DateAdd(MONTH,@Remind, @NexDeu),
		  @Debit
	SET @Remind =  @Remind - 1
END
FETCH NEXT FROM recorrente_cursor INTO @Remind, @NexDeu, @Debit 
END  

CLOSE recorrente_cursor  
DEALLOCATE recorrente_cursor  

insert #tmp
select  2,
		'PREVISÃO A RECEBER',
		'TRANSAÇÕES RECORRENTES (PV)', 
		year(DataTransacao),
		month(DataTransacao),
		sum(Valor)	
from #tmpLcmRecorrente
where DataTransacao >= @dataini
	and DataTransacao <= @datafim
group by 
	year(DataTransacao),
	month(DataTransacao)

delete from #tmpLcmRecorrente 
----------------------------------------------------------------------------------------------
------  PREVISÃO A RECEBER Provisões do Fluxo de Caixa  --------------------------------------
----------------------------------------------------------------------------------------------
if(@fluxoCaixa = 'S')
Begin
	DECLARE recorrente_cursor CURSOR FOR   
	select datediff(month, dateId, EndDate), dateId, Credit  from OCFL 
	where     OCFL.Frequency='M'
		  AND dateId >= @dataini
	OPEN recorrente_cursor  
	FETCH NEXT FROM recorrente_cursor INTO @Remind, @NexDeu, @Debit  

	WHILE @@FETCH_STATUS = 0  
	BEGIN  

	WHILE @Remind >= 0
	BEGIN
	   insert #tmpLcmRecorrente
	   select DateAdd(MONTH,@Remind, @NexDeu),
			  @Debit
		SET @Remind =  @Remind - 1
	END
	FETCH NEXT FROM recorrente_cursor INTO @Remind, @NexDeu, @Debit 
	END  

	CLOSE recorrente_cursor  
	DEALLOCATE recorrente_cursor  

	insert #tmp
	select  2,
			'PREVISÃO A RECEBER',
			'PROVISÕES DO FLUXO DE CAIXA', 
			year(DataTransacao),
			month(DataTransacao),
			sum(Valor)	
	from #tmpLcmRecorrente
	where DataTransacao >= @dataini
		and DataTransacao <= @datafim
	group by 
		year(DataTransacao),
		month(DataTransacao)

	delete from #tmpLcmRecorrente 
end
----------------------------------------------------------------------------------------------
------------------   A RECEBER -  Notas Fiscais de Saídas e Devoluções REAL ------------------
----------------------------------------------------------------------------------------------
insert #tmp
select  3,
	    'A RECEBER',
		'NOTAS FISCAIS DE SAÍDAS E DEVOLUÇÕES REAL',  
		ano,
		mes,
		sum(Total)
from (select  
		year(DocDate) ano,
		month(DocDate) mes,
		sum(Total) total
from VW_CVA_Fluxo_A_Receber
where 
DocDate >= @dataini
	and DocDate <= @datafim
group by 
	year(DocDate),
	month(DocDate)
union all
select  year(DueDate),
		month(DueDate),
		sum(BoeSum)	
from OBOE T0
where BoeType = 'I' and BoeStatus = 'G'
	and DueDate >= @dataini
	and DueDate <= @datafim
group by 
	year(DueDate),
	month(DueDate)) as TMP
Group by Ano, Mes
----------------------------------------------------------------------------------------------
------------------   RECEBIDO -  Contas a Receber Efetivo  -----------------------------------
----------------------------------------------------------------------------------------------
insert #tmp
select 4,
	   'RECEBIDO',
	   'CONTAS A RECEBER EFETIVO',
	   year(ORCT.DocDate),
	   month(ORCT.DocDate),
	   sum(ORCT.DocTotal)  
from ORCT 
where     ORCT.DocDate >= @dataini
	  AND ORCT.DocDate <= @datafim
	  AND ORCT.Canceled='N'
group by 
	   year(ORCT.DocDate),
	   month(ORCT.DocDate)
----------------------------------------------------------------------------------------------
------------------   RECEBIDO -  Lançamentos Contábeis (conta caixa)  ------------------------
----------------------------------------------------------------------------------------------
insert #tmp
select 4,
	   'RECEBIDO',
	   'LANÇAMENTOS CONTÁBEIS (CONTA CAIXA)',
	   year(JDT1.TaxDate),
	   month(JDT1.TaxDate),
	   sum(Debit)  
from OJDT 
	inner join JDT1 on OJDT.TransId = JDT1.TransId
	inner join OACT ON OACT.AcctCode = JDT1.Account 
where     
		  OACT.Finanse ='Y'
	  AND JDT1.Debit > 0
      AND JDT1.TaxDate >= @dataini
	  AND JDT1.TaxDate <= @datafim
group by 
	year(JDT1.TaxDate),
	month(JDT1.TaxDate)	
----------------------------------------------------------------------------------------------
----------------------------- PREVISÃO A PAGAR -----------------------------------------------
----------------------------------------------------------------------------------------------
if(@compra = 'S')
begin
	----------------------------------------------------------------------------------------------
	----------------------------- COTAÇÃO DE COMPRA  ---------------------------------------------
	----------------------------------------------------------------------------------------------
	insert #tmp
	select  5,
			'PREVISÃO A PAGAR',
			'COTAÇÃO DE COMPRA', 
			year(OQUT.DocDate) as Ano,
			month(OQUT.DocDate) as Mes,
   			SUM(OQUT.DocTotal)
	from OQUT 
	Where	  DocDate >= @dataini
		  and DocDate <= @datafim
		  and DocStatus = 'O'
	group by year(OQUT.DocDate) ,month(OQUT.DocDate)
	----------------------------------------------------------------------------------------------
	----------------------------- SOLICITAÇÃO DE COMPRA  -----------------------------------------
	----------------------------------------------------------------------------------------------
	insert #tmp
	select  5,
			'PREVISÃO A PAGAR',
			'SOLICITAÇÃO DE COMPRA', 
			year(OPRQ.DocDate) as Ano,
			month(OPRQ.DocDate) as Mes,
   			SUM(OPRQ.DocTotal)
	from OPRQ 
	Where	  DocDate >= @dataini
		  and DocDate <= @datafim
		  and DocStatus = 'O'
	group by year(OPRQ.DocDate) ,month(OPRQ.DocDate)
	----------------------------------------------------------------------------------------------
	----------------------------- PEDIDOS DE COMPRA  ---------------------------------------------
	----------------------------------------------------------------------------------------------
	insert #tmp
	select  5,
			'PREVISÃO A PAGAR',
			'PEDIDOS DE COMPRA', 
			year(OPOR.DocDate) as Ano,
			month(OPOR.DocDate) as Mes,
   			SUM(OPOR.DocTotal)
	from OPOR 
	Where	  DocDate >= @dataini
		  and DocDate <= @datafim
		  and DocStatus = 'O'
	group by year(OPOR.DocDate) ,month(OPOR.DocDate)
end
----------------------------------------------------------------------------------------------
----------------------------- LANÇAMENTOS PERIÓDICOS  ----------------------------------------
----------------------------------------------------------------------------------------------
DECLARE recorrente_cursor CURSOR FOR   
select Remind, NextDeu, Credit  from ORCR 
	inner join RCR1 on ORCR.RcurCode = RCR1.RcurCode AND  ORCR.Instance = RCR1.Instance
	inner join OACT ON OACT.AcctCode = RCR1.AcctCode 
where     ORCR.Frequency='M'
	  AND ORCR.LimitRtrns ='Y'
	  AND OACT.Finanse ='Y'
	  AND RCR1.Credit > 0
	  AND ORCR.Instance = 0
	  AND NextDeu >= @dataini
OPEN recorrente_cursor  
FETCH NEXT FROM recorrente_cursor INTO @Remind, @NexDeu, @Debit  

WHILE @@FETCH_STATUS = 0  
BEGIN  

WHILE @Remind >= 0
BEGIN
   insert #tmpLcmRecorrente
   select DateAdd(MONTH,@Remind, @NexDeu),
		  @Debit
	SET @Remind =  @Remind - 1
END
FETCH NEXT FROM recorrente_cursor INTO @Remind, @NexDeu, @Debit 
END  

CLOSE recorrente_cursor  
DEALLOCATE recorrente_cursor  

insert #tmp
select  5,
		'PREVISÃO A PAGAR',
		'LANÇAMENTOS RECORRENTES', 
		year(DataTransacao),
		month(DataTransacao),
		sum(Valor)	
from #tmpLcmRecorrente
where DataTransacao >= @dataini
	and DataTransacao <= @datafim
group by 
	year(DataTransacao),
	month(DataTransacao)

delete from #tmpLcmRecorrente 
----------------------------------------------------------------------------------------------
-------------------------- PEDIDOS DE COMPRA RECORRENTES--------------------------------------
----------------------------------------------------------------------------------------------
DECLARE recorrente_cursor CURSOR FOR   
select datediff(month, startdate, EndDate), startdate, odrf.DocTotal from ORCP 
	inner join ODRF on ORCP.DraftEntry = ODRF.DocEntry 
where 
	DocObjType = 22 
	and IsRemoved = 'N'
	and Frequency ='M'
	and StartDate >=  @dataini
OPEN recorrente_cursor  
FETCH NEXT FROM recorrente_cursor INTO @Remind, @NexDeu, @Debit  

WHILE @@FETCH_STATUS = 0  
BEGIN  

WHILE @Remind >= 0
BEGIN
   insert #tmpLcmRecorrente
   select DateAdd(MONTH,@Remind, @NexDeu),
		  @Debit
	SET @Remind =  @Remind - 1
END
FETCH NEXT FROM recorrente_cursor INTO @Remind, @NexDeu, @Debit 
END  

CLOSE recorrente_cursor  
DEALLOCATE recorrente_cursor  


insert #tmp
select  5,
		'PREVISÃO A PAGAR',
		'TRANSAÇÕES RECORRENTES (PC)', 
		year(DataTransacao),
		month(DataTransacao),
		sum(Valor)	
from #tmpLcmRecorrente
where DataTransacao >= @dataini
	and DataTransacao <= @datafim
group by 
	year(DataTransacao),
	month(DataTransacao)

delete from #tmpLcmRecorrente 
----------------------------------------------------------------------------------------------
------  PREVISÃO A PAGAR Provisões do Fluxo de Caixa  ----------------------------------------
----------------------------------------------------------------------------------------------
if(@fluxoCaixa = 'S')
begin
	DECLARE recorrente_cursor CURSOR FOR   
	select datediff(month, dateId, EndDate), dateId, Debit  from OCFL 
	where     OCFL.Frequency='M'
		  AND dateId >= @dataini
	OPEN recorrente_cursor  
	FETCH NEXT FROM recorrente_cursor INTO @Remind, @NexDeu, @Debit  

	WHILE @@FETCH_STATUS = 0  
	BEGIN  

	WHILE @Remind >= 0
	BEGIN
	   insert #tmpLcmRecorrente
	   select DateAdd(MONTH,@Remind, @NexDeu),
			  @Debit
		SET @Remind =  @Remind - 1
	END
	FETCH NEXT FROM recorrente_cursor INTO @Remind, @NexDeu, @Debit 
	END  

	CLOSE recorrente_cursor  
	DEALLOCATE recorrente_cursor  

	insert #tmp
	select  5,
			'PREVISÃO A PAGAR',
			'PROVISÕES DO FLUXO DE CAIXA', 
			year(DataTransacao),
			month(DataTransacao),
			sum(Valor)	
	from #tmpLcmRecorrente
	where DataTransacao >= @dataini
		and DataTransacao <= @datafim
	group by 
		year(DataTransacao),
		month(DataTransacao)
end
----------------------------------------------------------------------------------------------
------------------   A PAGAR -  Notas Fiscais de Entrada e Devoluções REAL ------------------
----------------------------------------------------------------------------------------------
insert #tmp
select  6,
		'A PAGAR',
		'NOTAS FISCAIS DE ENTRADA E DEVOLUÇÕES REAL', 
		year(DocDate),
		month(DocDate),
		sum(Total)	
from VW_CVA_Fluxo_A_Pagar
where DocDate >= @dataini
	and DocDate <= @datafim
group by 
	year(DocDate),
	month(DocDate)
----------------------------------------------------------------------------------------------
------------------   PAGO -  Contas a Pagar Efetivo  -----------------------------------------
----------------------------------------------------------------------------------------------
insert #tmp
select 7,
	   'PAGO',
	   'CONTAS A PAGAR EFETIVO',
	   year(OVPM.DocDate),
	   month(OVPM.DocDate),
	   sum(OVPM.DocTotal)  
from OVPM 
where     OVPM.DocDate >= @dataini
	  AND OVPM.DocDate <= @datafim
	  AND OVPM.Canceled='N'
group by 
	   year(OVPM.DocDate),
	   month(OVPM.DocDate)
----------------------------------------------------------------------------------------------
------------------   PAGO -  Lançamentos Contábeis (conta caixa)  ----------------------------
----------------------------------------------------------------------------------------------
insert #tmp
select 7,
	   'PAGO',
	   'LANÇAMENTOS CONTÁBEIS (CONTA CAIXA)',
	   year(JDT1.TaxDate),
	   month(JDT1.TaxDate),
	   sum(Credit)  
from OJDT 
	inner join JDT1 on OJDT.TransId = JDT1.TransId
	inner join OACT ON OACT.AcctCode = JDT1.Account 
where     
		  OACT.Finanse ='Y'
	  AND JDT1.Credit > 0
      AND JDT1.TaxDate >= @dataini
	  AND JDT1.TaxDate <= @datafim
group by 
	year(JDT1.TaxDate),
	month(JDT1.TaxDate)	

delete from #tmpLcmRecorrente 
----------------------------------------------------------------------------------------------
------------------   TOTALIZADORES -  PREVISÃO A RECEBER  ------------------------------------
----------------------------------------------------------------------------------------------
insert #tmp
select 8, 'TOTAL PREVISÃO A RECEBER', 'PREVISÃO A RECEBER', Ano,Mes, sum(Valor) from #tmp 
where Order_Row = 1
group by Ano,Mes
----------------------------------------------------------------------------------------------
------------------   TOTALIZADORES -  A RECEBER  ---------------------------------------------
----------------------------------------------------------------------------------------------
insert #tmp
select 9, 'TOTAL A RECEBER', 'A RECEBER', Ano,Mes, sum(Valor) from #tmp 
where Collumn_1 in ('A RECEBER')
group by Ano,Mes
----------------------------------------------------------------------------------------------
------------------   TOTALIZADORES -  PREVISOES A PAGAR  -------------------------------------
----------------------------------------------------------------------------------------------
insert #tmp
select 10, 'TOTAL PREVISOES A PAGAR', 'PREVISOES A PAGAR', Ano,Mes, sum(Valor) * -1 from #tmp 
where Collumn_1 in ('PREVISÃO A PAGAR','A PAGAR')
group by Ano,Mes
----------------------------------------------------------------------------------------------
------------------   TOTALIZADORES -  A PAGAR  ---------------------------------------------
----------------------------------------------------------------------------------------------
insert #tmp
select 11, 'TOTAL A PAGAR', 'A PAGAR', Ano,Mes, sum(Valor) *-1 from #tmp 
where Collumn_1 = 'NOTAS FISCAIS DE ENTRADA E DEVOLUÇÕES REAL'
group by Ano,Mes
----------------------------------------------------------------------------------------------
------------------   TOTALIZADORES -  SALDO PREVISTO  ----------------------------------------
----------------------------------------------------------------------------------------------
insert #tmp
select 12, 'TOTAL SALDO PREVISTO', 'SALDO PREVISTO', Ano,Mes, sum(Valor) from #tmp 
where Collumn_2 IN ('A RECEBER','PREVISOES A PAGAR','A PAGAR','PREVISÃO A RECEBER')
group by Ano,Mes
----------------------------------------------------------------------------------------------
------------------   TOTALIZADORES -  RECEBIDO   ---------------------------------------------
----------------------------------------------------------------------------------------------
insert #tmp
select 13, 'TOTAL RECEBIDO', 'RECEBIDO', Ano,Mes, sum(Valor) from #tmp 
where  Collumn_1 in ('RECEBIDO')
group by Ano,Mes
----------------------------------------------------------------------------------------------
------------------   TOTALIZADORES -  PAGO   -------------------------------------------------
----------------------------------------------------------------------------------------------
insert #tmp
select 14, 'TOTAL PAGO', 'PAGO', Ano,Mes, sum(Valor) *-1 from #tmp 
where  Collumn_1 in ('PAGO')
group by Ano,Mes
----------------------------------------------------------------------------------------------
------------------   TOTALIZADORES -  SALDO ANT. BANCO ---------------------------------------
----------------------------------------------------------------------------------------------
declare @strSql varchar(max)
declare @dataIniTemp datetime
set @dataIniTemp = @dataini
while @dataIniTemp <= @datafim
begin
	insert #tmp
	select 15,
			'SALDO ANT. BANCO',
			'SALDO ANT. BANCO',
		    year(@dataIniTemp),
			month(@dataIniTemp),
			sum(debit - credit) as Valor 
	from ojdt 
		inner join jdt1 on ojdt.TransId = jdt1.TransId
		inner join oact on oact.AcctCode =  jdt1.Account
	where	  oact.Finanse = 'Y'
			  and ((@contaCaixa = 'S' and ojdt.TaxDate < @dataIniTemp) or (@contaCaixa = 'N' and ojdt.TaxDate >= @dataini  and ojdt.TaxDate < @dataIniTemp))

    set @dataIniTemp =  dateadd(m, 1, @dataIniTemp)
End
----------------------------------------------------------------------------------------------
------------------   TOTALIZADORES -  SALDO FINAL BANCO -------------------------------------
----------------------------------------------------------------------------------------------
insert #tmp
select  16, 
		'SALDO FINAL BANCO',
		'SALDO FINAL BANCO',
		Ano,
		Mes, 
		isnull((select valor from #tmp t1 where t0.Ano =  t1.Ano and t0.Mes = t1.Mes and t1.Collumn_1='SALDO ANT. BANCO'),0) +
		isnull((select valor from #tmp t1 where t0.Ano =  t1.Ano and t0.Mes = t1.Mes and t1.Collumn_1='TOTAL RECEBIDO'),0)  + 
		isnull((select valor from #tmp t1 where t0.Ano =  t1.Ano and t0.Mes = t1.Mes and t1.Collumn_1 ='TOTAL PAGO'),0)
From #tmp t0
group by ano, mes 
----------------------------------------------------------------------------------------------
-----------------------------------  ORÇADO X REAL -------------------------------------------
----------------------------------------------------------------------------------------------
insert #tmp
select  17, 
		'ORÇADO X REAL', 
		'ORÇADO X REAL', 
		Ano,
		Mes, 
		isnull((select valor from #tmp t1 where t0.Ano =  t1.Ano and t0.Mes = t1.Mes and t1.Collumn_2 ='SALDO PREVISTO'),0)  - 
		isnull((select valor from #tmp t1 where t0.Ano =  t1.Ano and t0.Mes = t1.Mes and t1.Collumn_2 ='SALDO FINAL BANCO'),0)
From #tmp t0
group by ano, mes 
----------------------------------------------------------------------------------------------
-----------------------------------  SEPARADORES  --------------------------------------------
----------------------------------------------------------------------------------------------
insert #tmp
select  2.1,
		'',
		'', 
		year(@dataini),
		month(@dataini),
		0	

insert #tmp
select  3.1,
		'',
		'', 
		year(@dataini),
		month(@dataini),
		0	

insert #tmp
select  4.1,
		'',
		'', 
		year(@dataini),
		month(@dataini),
		0	

insert #tmp
select  5.1,
		'',
		'', 
		year(@dataini),
		month(@dataini),
		0	

insert #tmp
select  6.1,
		'',
		'', 
		year(@dataini),
		month(@dataini),
		0	

insert #tmp
select  7.1,
		'',
		'', 
		year(@dataini),
		month(@dataini),
		0	

insert #tmp
select  8.1,
		'',
		'', 
		year(@dataini),
		month(@dataini),
		0	

select * from #tmp

drop table #tmp
drop table  #tmpLcmRecorrente


end