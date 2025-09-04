USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[spcJBCFluxoCaixa_CARGA]    Script Date: 06/09/2015 09:17:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCFluxoCaixa_CARGA]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCFluxoCaixa_CARGA]
GO

USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[spcJBCFluxoCaixa_CARGA]    Script Date: 06/09/2015 09:17:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- 

CREATE proc [dbo].[spcJBCFluxoCaixa_CARGA]
  @dtInicial smalldatetime 
  ,@dt smalldatetime   
  , @Previsoes char(1)--C Compra, V Vendas, T Todas
  ,@SaldoInicial double precision
  with encryption
as
	declare @PrevisoesParam char(1)

	--if DATEDIFF(day,getdate(),cast('2016-11-20' as date))<=0 begin
	--	RAISERROr ('Claudino Software: Tempo de uso de Demostração Expirado: 41 99327681', 11,1)
	--end
  declare @vGroupNum int
  declare @vShipDate smalldatetime
  declare @vLineTotal money
  declare @vTipo varchar(3)

  --if  @Previsoes='S' begin
	  declare cp1 cursor local fast_forward read_only for
		select 'PRE' as tipo
			 , ordr.groupnum as forma
			 , rdr1.ShipDate as data_base
			 , SUM(rdr1.linetotal) as valor
		  from RDR1
		 inner join ORDR
			on ORDR.DocEntry = rdr1.DocEntry 
		 where rdr1.InvntSttus = 'O'
		   and ordr.DocStatus = 'O' and (@Previsoes='V' or @Previsoes='T') and  @Previsoes<>'N'
		 group by ordr.groupnum, rdr1.ShipDate 
		 union all
		select 'PRO' as tipo
			 , oPOR.groupnum as forma
			 , POR1.ShipDate as data_base
			 , SUM(POR1.linetotal) as valor
		  from POR1
		 inner join OPOR 
			on oPOR.DocEntry = POR1.DocEntry 
		 where POR1.InvntSttus = 'O'
		   and oPOR.DocStatus = 'O'    and (@Previsoes='C' or @Previsoes='T') and  @Previsoes<>'N'
		 group by oPOR.groupnum, POR1.ShipDate 
		order by 1, 3, 2
	  open cp1

	  while 1 = 1
	  begin
		fetch next from cp1 into @vTipo, @vGroupNum, @vShipDate, @vLineTotal
		if @@FETCH_STATUS <> 0 break
    
		insert into #lancamentos (tipo, dt_vencimento, valor)
		  SELECT @vTipo, DATEADD(day, InstDays, @vShipDate), ( @vLineTotal * InstPrcnt ) / 100
			from CTG1

		   where CTGCode = @vGroupNum 
       
		if @@ROWCOUNT = 0
		  insert into #lancamentos (tipo, dt_vencimento, valor)
			select @vTipo, @vShipDate, @vLineTotal 
    
	  end
	  close cp1
	  deallocate cp1
  
  --end
CREATE TABLE #ContasAReceberPorVencimento (
	TransId int, 
	Line_ID int, 
	Account nvarchar(30),
	ShortName  nvarchar(30),
	TransType nvarchar(400),
	CreatedBy int,
	BaseRef nvarchar(22),
	SourceLine smallint,
	RefDate datetime,
	DueDate datetime,
	BalDueCred decimal(19, 9),
	BalDueDeb decimal(19, 9),
	BalDueCredBalDueDeb decimal(19, 9),
	Saldo decimal(19, 9),
	LineMemo nvarchar(100),
	CardName nvarchar(200),
	CardCode nvarchar(30),
	Balance  decimal(19, 9),
	SlpCode int,
	DebitCredit  decimal(19, 9),
	IsSales nvarchar(2),
	Currency nvarchar(6),
	BPLName nvarchar(200),
	Serial      int,
	FormaPagamento   varchar(100),      
	PeyMethodNF   	 varchar(100),	
	BancoNF			 varchar(100),
	Installmnt		 varchar(100),
	OrctComments	 varchar(200),
	BankName         varchar(100)
	,DocEntryNFS	 int
	,dias int
)

--execute [spcJBCContasAReceberPorVencimento] '*','2010-01-01 00:00:00','2020-01-01 00:00:00','V','*'
select @PrevisoesParam=case when (@Previsoes='V' or @Previsoes='T') then 'S' else 'N' end 

insert  into #ContasAReceberPorVencimento
EXECUTE [spcJBCContasAReceberPorVencimento] 
   '*'
  --,'1900-11-20'
  --,'2050-11-20'
  ,@dtInicial
  ,@dt
  ,'V'
  ,'*','*',0,@PrevisoesParam--@Previsoes
  
CREATE TABLE #ContasAPagarPorVencimento (		
	ShortName nvarchar(30)
	, CardName nvarchar(200)
	, Lancamento datetime
	, Vencimento datetime
	, Origem nvarchar(400)
	, OrigemNr integer
	, Parcela smallint
	, ParcelaTotal smallint
	, Serial int
	, LineMemo nvarchar(100)
	, Debit decimal(19, 9)
	, Credit decimal(19, 9)
	, Saldo decimal(19, 9)
	,DueDate datetime
	,PeyMethodNF nvarchar (40)
)

--execute spcJBCContasAPagarPorVencimento '*','2010-01-01 00:00:00','2020-01-01 00:00:00','V'
select @PrevisoesParam=case when (@Previsoes='C' or @Previsoes='T') then 'S' else 'N' end 

insert into #ContasAPagarPorVencimento
execute spcJBCContasAPagarPorVencimento 
   '*'
  --,'1900-11-20'
  --,'2050-11-20'
  ,@dtInicial
  ,@dt 
  ,'V',@PrevisoesParam,'*'

  --select * from #ContasAPagarPorVencimento
  insert into #lancamentos (tipo, dt_vencimento, valor)
    SELECT 'CAP'
          , dt_vencimento, 
           sum(vl_saldo)
    FROM
    (
 --   select 
	--	Saldo as 'vl_saldo' ,
	--	DueDate as 'dt_vencimento'
	--from 
	--	#ContasAReceberPorVencimento
	--union all
	select 
		Saldo as 'vl_saldo' ,
		Vencimento as 'dt_vencimento'
	from 	
	#ContasAPagarPorVencimento

    ) as completo
    where dt_vencimento <= @dt 
    group by dt_vencimento 

	--o D+1 e final de semana(Ex: títulos que vencem sábado, domingo e segunda entram para mim na terça-feira)
	

	update #ContasAReceberPorVencimento set  DueDate= 
		case 
			when datepart(weekday,DueDate)=7 then dateadd(day,3,DueDate) 
			when datepart(weekday,DueDate)=6 then dateadd(day,4,DueDate) 
			when datepart(weekday,DueDate)=5 then dateadd(day,1,DueDate) 
			when datepart(weekday,DueDate)=4 then dateadd(day,1,DueDate) 
			when datepart(weekday,DueDate)=3 then dateadd(day,1,DueDate) 
			when datepart(weekday,DueDate)=2 then dateadd(day,1,DueDate) 
			when datepart(weekday,DueDate)=1 then dateadd(day,2,DueDate) 
		end	
	
	----7-é sabado -soma 3
	--update #ContasAReceberPorVencimento set  DueDate=dateadd(day,3,DueDate) 
	--where datepart(weekday,DueDate)=7
	----e sexta cai na terça, 6 soma 4
	--update #ContasAReceberPorVencimento set  DueDate=dateadd(day,4,DueDate) 
	--where datepart(weekday,DueDate)=6
	----quinta na sexta,  5 soma 1
	--update #ContasAReceberPorVencimento set  DueDate=dateadd(day,1,DueDate) 
	--where datepart(weekday,DueDate)=5
	----quarta cai na quinta, 4  soma 1
	--update #ContasAReceberPorVencimento set  DueDate=dateadd(day,1,DueDate) 
	--where datepart(weekday,DueDate)=4
	----2-é segunda - soma 1
	--update #ContasAReceberPorVencimento set  DueDate=dateadd(day,1,DueDate) 
	--where datepart(weekday,DueDate)=2
	----terça cai na quarta 3 soma 1
	--update #ContasAReceberPorVencimento set  DueDate=dateadd(day,1,DueDate) 
	--where datepart(weekday,DueDate)=3
	----1-é domingo - soma 2
	--update #ContasAReceberPorVencimento set  DueDate=dateadd(day,2,DueDate) 
	--where datepart(weekday,DueDate)=1






	--    select 
	--	Saldo as 'vl_saldo' ,
	--	DueDate as 'dt_vencimento'
	--from 
	--	#ContasAReceberPorVencimento   

  insert into #lancamentos (tipo, dt_vencimento, valor)
    SELECT 'CAR'
          , dt_vencimento, 
           sum(vl_saldo)
    FROM (
    select 
		Saldo as 'vl_saldo' ,
		DueDate as 'dt_vencimento'
	from 
		#ContasAReceberPorVencimento    
	


     ) as completo
    where dt_vencimento <= @dt 
    group by dt_vencimento 

  delete from #lancamentos
   where dt_vencimento > @dt

drop table #ContasAReceberPorVencimento

drop table #ContasAPagarPorVencimento








GO


exec spcJBCFluxoCaixa_CARGA '04-09-2018','04-09-2018','T',0