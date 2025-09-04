IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'P' AND name = 'spc_CVA_FluxoCaixa')
	DROP PROCEDURE spc_CVA_FluxoCaixa
GO
CREATE proc [dbo].[spc_CVA_FluxoCaixa]  
	@dataDe			smalldatetime,
	@dataAte		smalldatetime,
	@saldoInicial	numeric(19, 2),
	@atrasados		char(1) = 'N',
	@caixa			char(1) = 'N',
	@grafico		char(1) = 'Y',
	@provisoes		char(1) = 'N',
	@formaPagamento nvarchar(200)
as  
BEGIN
SET DATEFORMAT 'ymd';  
	create table #fluxo (  
	dt smalldatetime null  
	, vl_cap numeric(19, 2) null  
	, vl_prev_compra numeric(19, 2) null  
	, vl_total_despesa numeric(19, 2) null  
	, vl_car numeric(19, 2) null  
	, vl_prev_venda numeric(19, 2) null  
	, vl_total_receita numeric(19, 2) null  
	, vl_total_parcial numeric(19, 2) null  
	)  
  
	create table #lancamentos (  
	tipo char(3) null  
	, dt_vencimento smalldatetime null  
	, valor decimal(19, 2) null  
	)  
    
	create table #saldo (acctcode varchar(MAX) null, saldo money null)  
	
	IF @caixa = 'Y'
	BEGIN    	
		insert into #saldo  
		exec spc_CVA_Balancete @caixa, @saldoInicial
    END
	ELSE
	BEGIN
		insert into #saldo 
		SELECT 'Saldo', @saldoInicial
	END
	declare @dias int  
	declare @dia int  
	declare @saldo_inicial decimal(19, 2)  
	declare @vl_cap decimal(19, 2)  
	declare @vl_prev_compra decimal(19, 2)  
	declare @vl_total_despesa decimal(19, 2)  
	declare @vl_car decimal(19, 2)  
	declare @vl_prev_venda decimal(19, 2)  
	declare @vl_total_receita decimal(19, 2)  
	declare @saldo_parcial decimal(19, 2)  
	declare @saldo_final decimal(19, 2)  
    
	select @saldo_inicial = SUM(saldo) from #saldo   
    
	select @saldo_inicial = ISNULL(@saldo_inicial, 0)  
    
	select @saldo_parcial = @saldo_inicial   
    
	exec spc_CVA_FluxoCaixa_CARGA @dataDe, @dataAte, @provisoes, @formaPagamento
    
	select @dias = DATEDIFF(day, @dataDe, @dataAte)  
    
	set @dia = 0  
    
	while @dia <= @dias  
	begin  
    
		select @vl_cap = 0  
				, @vl_car = 0  
				, @vl_prev_compra = 0  
				, @vl_prev_venda = 0  
  
		if @dia = 0 and @atrasados = 'Y'  
		begin  
			select @vl_cap = sum(valor)  
			from #lancamentos  
			where tipo = 'CAP'  
				and dt_vencimento BETWEEN @dataDe AND @dataAte
  
			select @vl_prev_compra = sum(valor)  
			from #lancamentos  
			where tipo = 'PRO'  
				and dt_vencimento BETWEEN @dataDe AND @dataAte
  
			select @vl_prev_venda = sum(valor)  
			from #lancamentos  
			where tipo = 'PRE'  
				and dt_vencimento BETWEEN @dataDe AND @dataAte
  
			select @vl_car = sum(valor)  
			from #lancamentos  
			where tipo = 'CAR'  
				and dt_vencimento BETWEEN @dataDe AND @dataAte
		end  
		else  
		begin  
			select @vl_cap = sum(valor)  
			from #lancamentos  
			where tipo = 'CAP'  
				and dt_vencimento = DATEADD(DAY, @dia, CONVERT(char(10), @dataDe, 120))  
  
			select @vl_prev_compra = sum(valor)  
			from #lancamentos  
			where tipo = 'PRO'  
				and dt_vencimento = DATEADD(DAY, @dia, CONVERT(char(10), @dataDe, 120))  
  
			select @vl_prev_venda = sum(valor)  
			from #lancamentos  
			where tipo = 'PRE'  
				and dt_vencimento = DATEADD(DAY, @dia, CONVERT(char(10), @dataDe, 120))  
  
			select @vl_car = sum(valor)  
			from #lancamentos  
			where tipo = 'CAR'  
				and dt_vencimento = DATEADD(DAY, @dia, CONVERT(char(10), @dataDe, 120))  
		end  
      
		select @vl_cap = ISNULL(@vl_cap, 0)  
				, @vl_car = ISNULL(@vl_car, 0)  
				, @vl_prev_compra = ISNULL(@vl_prev_compra, 0)  
				, @vl_prev_venda = ISNULL(@vl_prev_venda, 0)  
      
		select @vl_total_despesa = @vl_cap + @vl_prev_compra  
				, @vl_total_receita = @vl_car + @vl_prev_venda  
           
		select @saldo_parcial = @saldo_parcial - @vl_total_despesa + @vl_total_receita   
      
		insert into #fluxo (dt  
			, vl_cap, vl_prev_compra, vl_total_despesa  
							, vl_car, vl_prev_venda, vl_total_receita  
							, vl_total_parcial)  
			select CONVERT(char(10), @dataDe + @dia, 120)  
				, @vl_cap, @vl_prev_compra, @vl_total_despesa  
				, @vl_car, @vl_prev_venda, @vl_total_receita  
				, @saldo_parcial   
    
		select @dia = @dia + 1  
    
	end  
    
	delete from #fluxo where vl_total_receita + vl_total_despesa = 0  
  
	select * from #fluxo order by 1  
END