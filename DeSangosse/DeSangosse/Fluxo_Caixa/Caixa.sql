/*    ==Parâmetros de Script==

    Versão do Servidor de Origem : SQL Server 2014 (12.0.2000)
    Edição do Mecanismo de Banco de Dados de Origem : Microsoft SQL Server Standard Edition
    Tipo do Mecanismo de Banco de Dados de Origem : SQL Server Autônomo

    Versão do Servidor de Destino : SQL Server 2014
    Edição de Mecanismo de Banco de Dados de Destino : Microsoft SQL Server Standard Edition
    Tipo de Mecanismo de Banco de Dados de Destino : SQL Server Autônomo
*/

USE [SBOHybel]
GO

/****** Object:  StoredProcedure [dbo].[spcCVAFluxoCaixa]    Script Date: 22/08/2018 16:20:58 ******/
DROP PROCEDURE [dbo].[spcCVAFluxoCaixa]
GO

/****** Object:  StoredProcedure [dbo].[spcCVAFluxoCaixa]    Script Date: 22/08/2018 16:20:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





 
 -- =============================================
-- Author:		Marco Kurpel
-- Create date: 17/05/2018
-- Description:	Fluxo de Caixa 
-- =============================================

create proc [dbo].[spcCVAFluxoCaixa]
 @DataInicial smalldatetime 
,@DataFinal smalldatetime
,@atrasados char(1) 
,@caixa char(1)     
,@Previsoes char(1)--C Compra, V Vendas, T Todas
,@SaldoInicial double precision
  
as  
	
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
    tipo char(300) null  
  , dt_vencimento smalldatetime null  
  , valor decimal(19, 2) null  
  , valornota money
  , valorADT  money
  , restante  money
  )  
    
  create table #saldo (acctcode varchar(72) null, saldo money null)
    
  insert into #saldo  
       exec spcCVABalancete @caixa   
    
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
    
 if @caixa = 'S'
   begin

	   select @saldo_inicial = SUM(saldo) from #saldo 
	   
	   select @saldo_inicial = ISNULL(@saldo_inicial, 0)
	   
	   set @SaldoInicial = @saldo_inicial 
	   
	   set @saldo_parcial = @SaldoInicial 
   end
  else
   begin 
	   
	   set @SaldoInicial = @SaldoInicial

	   set @saldo_parcial = @SaldoInicial 
   end
  
    exec spcCVAFluxo_Caixa_Carga @DataInicial,@DataFinal,@Previsoes,@SaldoInicial

    select @dias = DATEDIFF(day, @DataInicial, @DataFinal)  
    
  set @dia = 0  
    

  while @dia <= @dias  
  begin  
    
		select @vl_cap = 0  
			 , @vl_car = 0  
			 , @vl_prev_compra = 0  
			 , @vl_prev_venda = 0  
  
		if @dia =0  and @atrasados = 'S'  
		begin  
			  select distinct @vl_cap = abs(sum(valor))
				from #lancamentos  
			   where tipo = 'CAP'  
				 and dt_vencimento <=convert(char(10), @DataInicial, 120)  
  
			  select distinct @vl_prev_compra = sum(valor)  
				from #lancamentos  
			   where tipo = 'PRO'  
				 and dt_vencimento <= convert(char(10), @DataInicial, 120)  
  
			  select distinct @vl_prev_venda = sum(valor)  
				from #lancamentos  
			   where tipo = 'PRE'  
				 and dt_vencimento  <= convert(char(10), @DataInicial, 120)  
  
			  select distinct @vl_car = sum(valor)  
				from #lancamentos  
			   where tipo = 'CAR'  
				 and dt_vencimento <= convert(char(10), @DataInicial, 120)  

				  select @vl_cap = ISNULL(@vl_cap, 0)  
				 , @vl_car = ISNULL(@vl_car, 0)  
				 , @vl_prev_compra = ISNULL(@vl_prev_compra, 0)  
				 , @vl_prev_venda = ISNULL(@vl_prev_venda, 0)  
      
			select @vl_total_despesa = 0 
				 , @vl_total_receita = 0

			select @saldo_parcial = @saldo_parcial - @vl_cap + @vl_car + @vl_prev_venda - @vl_prev_compra


			insert into #fluxo (dt ,vl_cap ,vl_prev_compra ,vl_total_despesa ,vl_car ,vl_prev_venda ,vl_total_receita ,vl_total_parcial)  
     
			 select CONVERT(char(10), @DataInicial + @dia, 120)  
				   ,@vl_cap
				   ,@vl_prev_compra
				   ,@vl_total_despesa  
				   ,@vl_car
				   ,@vl_prev_venda
				   ,@vl_total_receita  
				   ,@saldo_parcial 
				   where @DataInicial = @DataInicial
		end  
		else  
		begin  
			  select @vl_cap = abs(sum(valor))  
				from #lancamentos  
			   where tipo = 'CAP'  
				 and dt_vencimento = DATEADD(DAY, @dia, CONVERT(char(10), @DataInicial, 120))  
  
			  select @vl_prev_compra = sum(valor)  
				from #lancamentos  
			   where tipo = 'PRO'  
				 and dt_vencimento = DATEADD(DAY, @dia, CONVERT(char(10), @DataInicial, 120))  
  
			  select @vl_prev_venda = sum(valor)  
				from #lancamentos  
			   where tipo = 'PRE'  
				 and dt_vencimento = DATEADD(DAY, @dia, CONVERT(char(10), @DataInicial, 120))  
  
			  select @vl_car = sum(valor)  
				from #lancamentos  
			   where tipo = 'CAR'  
				 and dt_vencimento = DATEADD(DAY, @dia, CONVERT(char(10), @DataInicial, 120))   
     
			select @vl_cap = ISNULL(@vl_cap, 0)  
				 , @vl_car = ISNULL(@vl_car, 0)  
				 , @vl_prev_compra = ISNULL(@vl_prev_compra, 0)  
				 , @vl_prev_venda = ISNULL(@vl_prev_venda, 0)  
      
		 select @vl_total_despesa = 0 
			  , @vl_total_receita = 0		

			select @saldo_parcial = @saldo_parcial - @vl_cap + @vl_car + @vl_prev_venda - @vl_prev_compra		
		
			insert into #fluxo (dt  
			  ,vl_cap
			  ,vl_prev_compra
			  ,vl_total_despesa  
			  ,vl_car
			  ,vl_prev_venda
			  ,vl_total_receita  
			  ,vl_total_parcial)  
			 select CONVERT(char(10), @DataInicial + @dia, 120)  
				   ,@vl_cap
				   ,@vl_prev_compra
				   ,@vl_total_despesa  
				   ,@vl_car
				   ,@vl_prev_venda
				   ,@vl_total_receita  
				   ,@saldo_parcial  
		end		    
    
	select @dia = @dia + 1  
    
  end      
  
  delete from #lancamentos where  not (dt_vencimento>=@DataInicial and dt_vencimento<=@DataFinal)

  select  dt	 as 'Data Vencto.'
	     ,vl_car as 'A Receber'
		 ,vl_cap as 'A Pagar'
		 ,vl_prev_venda    as 'Pedido'
		 ,vl_prev_compra   as 'Ordem de Compra'
		 ,vl_total_parcial as 'Saldo'
    from #fluxo order by 1  
  
   



GO

