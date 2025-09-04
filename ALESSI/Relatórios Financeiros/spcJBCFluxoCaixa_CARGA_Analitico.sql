USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[spcJBCFluxoCaixa_CARGA_Analitico]    Script Date: 06/09/2015 09:18:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCFluxoCaixa_CARGA_Analitico]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCFluxoCaixa_CARGA_Analitico]
GO

USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[spcJBCFluxoCaixa_CARGA_Analitico]    Script Date: 06/09/2015 09:18:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- 
CREATE proc [dbo].[spcJBCFluxoCaixa_CARGA_Analitico]
  @dtInicial smalldatetime 
  ,@dt smalldatetime 
, @caixa char(1)
, @atrasos char(1)
, @Previsoes char(1)--C Compra, V Vendas, T Todas, N Nenhum
,@SaldoInicial double precision
with encryption
as 
	--if DATEDIFF(day,getdate(),cast('2016-11-20' as date))<=0 begin
	--	RAISERROr ('Claudino Software: Tempo de uso de Demostração Expirado: 41 99327681', 11,1)
	--end
	declare @PrevisoesParam char(1)

  declare @vGroupNum      int
  declare @vDocNum        int
  declare @vCardCode      varchar(30)
  declare @vCardName      varchar(200)
  declare @vShipDate      smalldatetime
  declare @vLineTotal     money
  declare @vTipo          varchar(3)
  declare @saldo_inicial  decimal(19, 2)
  declare @vdt_vencimento smalldatetime
  declare @vvalor money
  declare @vlinha int

  create table #lancamentos (
    tipo nvarchar(max) null
  , DocNum int null
  , CardCode varchar(30) null
  , CardName varchar(200) null
  , dt_vencimento smalldatetime null
  , valor decimal(19, 2) null
  , saldo decimal(19, 2) null
  )
  --if @Previsoes='S' begin
	  declare cp1 cursor local fast_forward read_only for
		select 'PV' as tipo
			 , ordr.docnum
			 , ordr.cardcode
			 , ordr.cardname
			 , ordr.groupnum as forma
			 , rdr1.ShipDate as data_base
			 , SUM(rdr1.linetotal) as valor
		  from RDR1
		 inner join ORDR
			on ORDR.DocEntry = rdr1.DocEntry 
		 where rdr1.InvntSttus = 'O'
		   and ordr.DocStatus = 'O' and (@Previsoes='V' or @Previsoes='T') and  @Previsoes<>'N'
		 group by ordr.docnum, ordr.cardcode, ordr.cardname, ordr.groupnum, rdr1.ShipDate 
		 union all
		select 'PC' as tipo
			 , opor.docnum
			 , opor.cardcode
			 , opor.cardname
			 , oPOR.groupnum as forma
			 , POR1.ShipDate as data_base
			 , SUM(POR1.linetotal) as valor
		  from POR1
		 inner join OPOR 
			on oPOR.DocEntry = POR1.DocEntry 
		 where POR1.InvntSttus = 'O'
		   and oPOR.DocStatus = 'O'   and (@Previsoes='C' or @Previsoes='T') and  @Previsoes<>'N'
		 group by opor.docnum, opor.cardcode, opor.cardname, oPOR.groupnum, POR1.ShipDate 
		order by 1, 3, 2
	  open cp1

	  while 1 = 1
	  begin
		fetch next from cp1 into @vTipo, @vDocNum, @vCardCode, @vCardName, @vGroupNum, @vShipDate, @vLineTotal
		if @@FETCH_STATUS <> 0 break
    
		insert into #lancamentos (tipo, docnum, cardcode, cardname, dt_vencimento, valor)
		  SELECT @vTipo, @vDocNum, @vCardCode, @vCardName
			   , DATEADD(day, InstDays, @vShipDate)
			   , case when @vTipo = 'PC' then ( ( @vLineTotal * InstPrcnt ) / 100 ) * -1 else (@vLineTotal * InstPrcnt) / 100 end
			from CTG1
		   where CTGCode = @vGroupNum 
       
		if @@ROWCOUNT = 0
		  insert into #lancamentos (tipo, docnum, cardcode, cardname, dt_vencimento, valor)
			select @vTipo, @vDocNum, @vCardCode, @vCardName, @vShipDate, case @vtipo when 'PC' then @vLineTotal * -1 else @vLineTotal end
    
	  end
	  close cp1
	  deallocate cp1
	--end


CREATE TABLE #ContasAPagarPorVencimento (		
	ShortName nvarchar(30)
	, CardName nvarchar(200)
	, Lancamento datetime
	, Vencimento datetime
	, Origem nvarchar(max)
	, OrigemNr integer
	, Parcela smallint
	, ParcelaTotal smallint
	, Serial int
	, LineMemo nvarchar(100)
	, Debit decimal(19, 9)
	, Credit decimal(19, 9)
	, Saldo decimal(19, 9)
	, DueDate datetime
	, PeyMethodNF nvarchar(40)
)
-- execute spcJBCContasAPagarPorVencimento '*','01-01-2000 00:00:00','01-01-2050 00:00:00','V'


select @PrevisoesParam=case when (@Previsoes='C' or @Previsoes='T') then 'S' else 'N' end 

insert into #ContasAPagarPorVencimento
execute spcJBCContasAPagarPorVencimento 
   '*'
  ,@dtInicial
  ,@dt 
  ,'V'
  --,@Previsoes
  ,@PrevisoesParam
  ,'*'
  
CREATE TABLE #ContasAReceberPorVencimento (
	TransId int, 
	Line_ID int, 
	Account nvarchar(30),
	ShortName  nvarchar(30),
	TransType nvarchar(max),
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
	Serial  int,
	FormaPagamento nvarchar(100),
	PeyMethodNF    nvarchar(100),
	BancoNF        nvarchar(100),
	Installmnt	   nvarchar(200),
	OrctComments   nvarchar(500),
	BankName	   nvarchar(100),
	Nf int,
	Dias int
)
-- execute [spcJBCContasAReceberPorVencimento] '*','01-01-2000 00:00:00','01-01-2050 00:00:00','V','*'

select @PrevisoesParam=case when (@Previsoes='V' or @Previsoes='T') then 'S' else 'N' end 

insert  into #ContasAReceberPorVencimento
EXECUTE [spcJBCContasAReceberPorVencimento] 
   '*'
  ,@dtInicial
  --,'2050-11-20'
  --,'1900-11-16'
  ,@dt
  ,'V'
  ,'*'
  ,'*'
  ,0
  --,@Previsoes
  ,@PrevisoesParam
  ,'N'

   --'*','2018-04-09','2018-04-19','V','*','*',0,'N','N'
  -- select    '*'
  --,cast(@dtInicial as date)
  --,cast(@dt as date)
  --,'V'
  --,'*'
  --,'*'
  --,0
  --,@PrevisoesParam
  --,'N'
--execute spcJBCContasAReceberPorVencimento '*','2018-04-09','2018-04-19','V','*','*',0,'N','N'
  --select @dtInicial,@dt,@PrevisoesParam
  --select * from #ContasAReceberPorVencimento

	--o D+1 e final de semana(Ex: títulos que vencem sábado, domingo e segunda entram para mim na terça-feira)
	--


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
	--where 	
	
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
	----terça cai na quarta 3 soma 1
	--update #ContasAReceberPorVencimento set  DueDate=dateadd(day,1,DueDate) 
	--where datepart(weekday,DueDate)=3
	----2-é segunda - soma 1
	--update #ContasAReceberPorVencimento set  DueDate=dateadd(day,1,DueDate) 
	--where datepart(weekday,DueDate)=2
	----1-é domingo - soma 2
	--update #ContasAReceberPorVencimento set  DueDate=dateadd(day,2,DueDate) 
	--where datepart(weekday,DueDate)=1






	--select * from ORPC
  insert into #lancamentos (tipo, docnum, cardcode, cardname, dt_vencimento, valor)
       SELECT tipo
          , docnum
          , CardCode
          , cardname
          , dt_vencimento 
          , sum(vl_saldo)* -1
    FROM (
		 select
			 CASE 
				when Origem='30' then 'OJDT'
				when Origem='18' then 'OPCH'
				when Origem='19' then 'ORPC'
				when Origem='204' then 'ODPO'
				when Origem='203' then 'ODPI'
				when Origem='243000002' then 'Imposto'
				--243000002????
				when CHARINDEX('DRFP', Origem)>0 then 'RecP'
				when CHARINDEX('DRFV', Origem)>0 then 'RecR'
				when CHARINDEX('LCMRecorrenteP', Origem)>0 then 'RecP'
				when CHARINDEX('LCMRecorrenteV', Origem)>0 then 'RecR'
				--else Origem

				else Origem+'????'
			 end AS 'tipo' 
			, OrigemNr as 'docnum'
			, ShortName as 'CardCode'
			, CardName as 'cardname'
			, Vencimento as 'dt_vencimento'
			, Saldo as 'vl_saldo'

		 from
			#ContasAPagarPorVencimento

  ) as completo
    where completo.dt_vencimento <= @dt 
    group by tipo, docnum, cardcode, cardname, dt_vencimento
  
  

  insert into #lancamentos (tipo, docnum, cardcode, cardname, dt_vencimento, valor)
       SELECT tipo
          , docnum
          , CardCode
          , cardname
          , dt_vencimento 
          , sum(vl_saldo)
    FROM (
	--select * from ODPO
		 select
			 CASE 
				when TransType='30' then 'OJDT'
				when TransType='13' then 'OINV'
				when TransType='14' then 'ORIN'
				when TransType='24' then 'ORCT'
				when TransType='204' then 'ODPO'
				when TransType='203' then 'ODPI'
				when TransType='Cheque' then 'Cheque'
				--select top 1 * from ODPO
				when CHARINDEX('DRFP', TransType)>0 then 'RecP'
				when CHARINDEX('DRFV', TransType)>0 then 'RecR'
				when CHARINDEX('LCMRecorrenteP', TransType)>0 then 'RecP'
				when CHARINDEX('LCMRecorrenteV', TransType)>0 then 'RecR'
				--else TransType
				else TransType+'????'
			 end AS 'tipo' 
			, BaseRef as 'docnum'
			, ShortName as 'CardCode'
			, CardName as 'cardname'
			, DueDate as 'dt_vencimento'
			, Saldo as 'vl_saldo'


		 from
			#ContasAReceberPorVencimento


  ) as completo
    where completo.dt_vencimento <= @dt 
    group by tipo, docnum, cardcode, cardname, dt_vencimento

  
--select * from #ContasAReceberPorVencimento

  delete from #lancamentos
   where dt_vencimento > @dt
   
   
   
  if @atrasos = 'N'
    delete from #lancamentos
     where convert(char(10), dt_vencimento, 120) < convert(char(10), @dtInicial, 120)

  declare @saldo_final decimal(19, 2)
  
  create table #saldo (acctcode varchar(72) null, saldo money null)
  
  insert into #saldo
    exec spcJBCBalancete1 @caixa 
  
  select @saldo_inicial = SUM(saldo) from #saldo 
  

  select @saldo_inicial = ISNULL(@saldo_inicial, 0)  
  --if @SaldoInicial<>0 begin 
	set @saldo_inicial=@SaldoInicial
  --end
  delete from #lancamentos where  not (dt_vencimento>=@dtInicial and dt_vencimento<=@dt)
---------------
  select dt_vencimento as 'Data'
       , tipo as 'Tipo'
       , CASE tipo
           when 'OJDT' then 'L.C.'
           when 'OINV' then 'NF Venda'
           when 'OPCH' then 'NF Compra'
           when 'OBOE' then 'Boleto'
           when 'PV' then 'P. Venda'
           when 'PC' then 'P. Compra'
           when 'OCHH' then 'Cheque'
           when 'ORCT' then 'Contas À Receber'
           else tipo end 'TP'
       , DocNum as 'Numero'
       , CardCode as 'Cliente'
       , CardName as 'Razão'
       , valor as 'Valor'
       , saldo as 'Saldo'
       , ROW_NUMBER() over (order by dt_vencimento, tipo, docnum, cardcode) as linha
     into #final
    from #lancamentos 
   order by dt_vencimento, tipo, docnum, cardcode

  declare cp1 cursor local fast_forward for
    select linha, valor
      from #final
     order by 1
     
  open cp1
  
  while 1 = 1
  begin
    fetch next from cp1 into @vlinha, @vvalor
    if @@FETCH_STATUS <> 0 break
        
    select @saldo_inicial = @saldo_inicial + isnull(@vvalor, 0)
    
    update #final 
       set saldo = @saldo_inicial
     where linha = @vlinha 

  end
  close cp1
  deallocate cp1

  update #final set TP ='Dev. Nota Fiscal Saída' where TP='ORIN'
  update #final set TP ='Adiantamento Para Fornecedor' where TP='ODPO'
  update #final set TP ='Dev. Nota Fiscal Entrada' where TP='ORPC'
  update #final set TP ='Recorrente' where TP='RecP'
  update #final set TP ='Adiantamento de Cliente' where TP='ODPI'

  update #final set TP ='BN' where TP='-4????'
  
  --select objtype from ODPI

  --select count(*) Tipo,TP from #final  group by Tipo,TP
  --order by linha 

 select * from #final  --where tp='-4????'
 --where tipo like '%?%'
 order by linha 





GO


exec spcJBCFluxoCaixa_CARGA_Analitico '04-09-2018','04-19-2018', 'N', 'N','N',1

