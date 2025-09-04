USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[spcJBCFluxoCaixa]    Script Date: 06/09/2015 09:17:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCFluxoCaixa]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCFluxoCaixa]
GO

USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[spcJBCFluxoCaixa]    Script Date: 06/09/2015 09:17:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



  
  
-- 
CREATE proc [dbo].[spcJBCFluxoCaixa]  
  @dtInicial smalldatetime 
  ,@dt smalldatetime  
, @atrasados char(1) = 'N'  
, @caixa char(1)     = 'N'  
, @grafico char(1)   = 'S'  
, @Previsoes char(1)--C Compra, V Vendas, T Todas
,@SaldoInicial double precision
  with encryption
as  
	--if DATEDIFF(day,getdate(),cast('2016-11-20' as date))<=0 begin
	--	RAISERROr ('Claudino Software: Tempo de uso de Demostração Expirado: 41 99327681', 11,1)
	--end
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
  )  
    
  create table #saldo (acctcode varchar(72) null, saldo money null)
    
  insert into #saldo  
    exec spcJBCBalancete1 @caixa   
    
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

  --if @SaldoInicial<>0 begin 
	set @saldo_inicial=@SaldoInicial
  --end    
  select @saldo_parcial = @saldo_inicial   

    
  exec spcJBCFluxoCaixa_CARGA @dtInicial,@dt,@Previsoes,@SaldoInicial
--select distinct  tipo from #lancamentos    

--select * from  #lancamentos    
  select @dias = DATEDIFF(day, @dtInicial, @dt)  
    
  set @dia = 0  
    delete from #lancamentos where  not (dt_vencimento>=@dtInicial and dt_vencimento<=@dt)



  while @dia <= @dias  
  begin  
    
    select @vl_cap = 0  
         , @vl_car = 0  
         , @vl_prev_compra = 0  
         , @vl_prev_venda = 0  
  
    if @dia = 0 and @atrasados = 'S'  
    begin  
      select @vl_cap = sum(valor)  
        from #lancamentos  
       where tipo = 'CAP'  
         and dt_vencimento <= convert(char(10), @dtInicial, 120)  
  
      select @vl_prev_compra = sum(valor)  
        from #lancamentos  
       where tipo = 'PRO'  
         and dt_vencimento <= convert(char(10), @dtInicial, 120)  
  
      select @vl_prev_venda = sum(valor)  
        from #lancamentos  
       where tipo = 'PRE'  
         and dt_vencimento <= convert(char(10), @dtInicial, 120)  
  
      select @vl_car = sum(valor)  
        from #lancamentos  
       where tipo = 'CAR'  
         and dt_vencimento <= convert(char(10), @dtInicial, 120)  
    end  
    else  
    begin  
      select @vl_cap = sum(valor)  
        from #lancamentos  
       where tipo = 'CAP'  
         and dt_vencimento = DATEADD(DAY, @dia, CONVERT(char(10), @dtInicial, 120))  
  
      select @vl_prev_compra = sum(valor)  
        from #lancamentos  
       where tipo = 'PRO'  
         and dt_vencimento = DATEADD(DAY, @dia, CONVERT(char(10), @dtInicial, 120))  
  
      select @vl_prev_venda = sum(valor)  
        from #lancamentos  
       where tipo = 'PRE'  
         and dt_vencimento = DATEADD(DAY, @dia, CONVERT(char(10), @dtInicial, 120))  
  
      select @vl_car = sum(valor)  
        from #lancamentos  
       where tipo = 'CAR'  
         and dt_vencimento = DATEADD(DAY, @dia, CONVERT(char(10), @dtInicial, 120))  
  
  
    end  
  
    --select @vl_prev_compra = ( 350 * @dia ) + @dia * (@dia * @dia)  
    --     , @vl_prev_venda = @dia * 2 * (@dia * 2) + (@dia + 2 * 3)  
    
      
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
     select CONVERT(char(10), @dtInicial + @dia, 120)  
           , @vl_cap, @vl_prev_compra, @vl_total_despesa  
           , @vl_car, @vl_prev_venda, @vl_total_receita  
           , @saldo_parcial   
    
select @dia = @dia + 1  
    
  end  
    
  --delete from #fluxo where vl_total_receita + vl_total_despesa = 0  
  
  select * from #fluxo order by 1  
  
   


GO


exec spcJBCFluxoCaixa '04-09-2018','04-09-2018','S','S','S','T',0