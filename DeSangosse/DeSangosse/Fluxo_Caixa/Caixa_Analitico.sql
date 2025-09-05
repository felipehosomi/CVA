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

/****** Object:  StoredProcedure [dbo].[spcCVAFluxoCaixa_CARGA_Analitico]    Script Date: 22/08/2018 16:21:11 ******/
DROP PROCEDURE [dbo].[spcCVAFluxoCaixa_CARGA_Analitico]
GO

/****** Object:  StoredProcedure [dbo].[spcCVAFluxoCaixa_CARGA_Analitico]    Script Date: 22/08/2018 16:21:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE proc [dbo].[spcCVAFluxoCaixa_CARGA_Analitico]
  @dataInicial smalldatetime 
 ,@dataFinal smalldatetime 
 ,@caixa char(1)
 ,@atrasos char(1)
 ,@Provisoes char(1)--C Compra, V Vendas, T Todas, N Nenhum
 ,@SaldoInicial double precision
as 

declare @DtVencimentoIni DATE
declare @DtVencimentoFin DATE

SELECT @DtVencimentoIni = @DataInicial
      ,@DtVencimentoFin = @DataFinal

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
  declare @vAdiantamento  money
  declare @vRestante money
  declare @vDocEntry int

 create table #lancamentos 
  (
	  tipo nvarchar(max) null
	, DocNum int null
	, CardCode varchar(30) null
	, CardName varchar(200) null
	, dt_vencimento datetime null
	, valor decimal(19, 2) null
	, saldo decimal(19, 2) null
	, valornota money
	, valorADT  money
	, restante  money
	, DocEntry int
  )
  
 declare cp1 cursor local fast_forward read_only for
		select 'PV' as tipo
			 , ordr.docnum
			 , ordr.cardcode
			 , ordr.cardname
			 , ordr.groupnum as forma
			 , rdr1.ShipDate as data_base
			 , SUM(rdr1.linetotal) as valor
			 ,null
			 ,null
			 ,ordr.DocEntry
		  from RDR1
		 inner join ORDR
			on ORDR.DocEntry = rdr1.DocEntry 
		 where rdr1.InvntSttus = 'O'
		   and ordr.DocStatus = 'O'
		   and (@Provisoes='P' or @Provisoes='T') 
		   and  @Provisoes<>'N'
		 group by ordr.docnum, ordr.cardcode, ordr.cardname, ordr.groupnum, rdr1.ShipDate,ordr.DocEntry
		 union all
		select distinct 'PC' as tipo	
			 , OPOR.docnum	
			 , OPOR.cardcode
			 , OPOR.cardname		 
			 , oPOR.groupnum as forma
			 , POR1.ShipDate as data_base 
			 , sum(POR1.linetotal)*-1 as valor
			 , ODPO.DpmAmnt as adiantamento	
			 ,null		 
			 ,opor.DocEntry
        from opor 
	   inner join por1 
		  on opor.docentry = por1.docentry
	    left join dpo1 
		  on por1.docentry = dpo1.baseEntry
	     and por1.linenum = dpo1.baseline 
	     and por1.objtype = dpo1.basetype
	    left join odpo
		  on dpo1.docentry = odpo.docentry   
	   where POR1.InvntSttus = 'O'
	     and oPOR.DocStatus = 'O'  
		 --and por1.ShipDate = '2018-06-18'
		 and (@Provisoes='C' or @Provisoes='T') 
		 and  @Provisoes<>'N'
		 and OPOR.CANCELED <> 'Y'
		-- and ODPO.CANCELED <> 'Y'
		 and ODPO.DocEntry not in (select BaseEntry  from rpc1 where rpc1.BaseEntry = odpo.DocEntry and rpc1.BaseType = 204 )
	   group by OPOR.docnum , OPOR.cardcode , OPOR.cardname, oPOR.groupnum, POR1.ShipDate, ODPO.DpmAmnt,opor.DocEntry
	   order by 1, 3, 2
	 open cp1  
 while 1 = 1
	  begin
		fetch next from cp1 into @vTipo, @vDocNum, @vCardCode, @vCardName, @vGroupNum, @vShipDate, @vLineTotal,@vAdiantamento,@vRestante,@vDocEntry
		if @@FETCH_STATUS <> 0 break
    --------------------------------------------------------------------------------------------------------------------------------------------------
	

	insert into #lancamentos (tipo,docnum, cardcode, cardname, dt_vencimento, valor,valornota,valorADT,restante,DocEntry)
		  SELECT @vTipo,@vDocNum, @vCardCode, @vCardName
			   , DATEADD(day, InstDays, @vShipDate)			   
			   , case when @vTipo = 'PC' then ((((@vLineTotal)* InstPrcnt) / 100)- isnull(@vAdiantamento*-1,0)) else ((@vLineTotal * InstPrcnt) / 100) end
			   ,@vlinetotal
			   ,isnull(@vAdiantamento,0) 
			   , (case when @vTipo = 'PC' then ((((@vLineTotal)* InstPrcnt) / 100)) else ((@vLineTotal * InstPrcnt) / 100) end)- isnull(@vAdiantamento*-1,0)
			   ,@vDocEntry
			from CTG1
		   where CTGCode = @vGroupNum 
		  	
       
		if @@ROWCOUNT = 0
		  insert into #lancamentos (tipo,docnum,cardcode,cardname,dt_vencimento, valor,valornota,valorADT,restante,DocEntry)
			select @vTipo, @vDocNum, @vCardCode, @vCardName,@vShipDate, @vLineTotal,@vlinetotal,@vAdiantamento ,@vRestante ,@vDocEntry
	  end
	  close cp1
	  deallocate cp1
	  
	
	--select * from #lancamentos where tipo = 'PC' order by 2


Create Table #ContasReceber
(
	ChaveDoc INT,		
	ChaveDocString NVARCHAR(10) Collate Database_Default,
	Numero NVARCHAR(50) Collate Database_Default,
	Parcela INT,
	ParcelaString NVARCHAR(10) Collate Database_Default,
	CardCode NVARCHAR(50) Collate Database_Default,
	CardName NVARCHAR(200) Collate Database_Default,
	CardFName NVARCHAR(200) Collate Database_Default,
	CardCodeName NVARCHAR(300) Collate Database_Default,
	CNPJ NVARCHAR(20) Collate Database_Default,
	TipoObj NVARCHAR(40) Collate Database_Default,
	TipoDoc NVARCHAR(2) Collate Database_Default,
	TabelaDoc NVARCHAR(4) Collate Database_Default,
	Emissao DATETIME,
	Vencimento DATETIME,
	ValorDoc NUMERIC(19,6),
	DocCur NVARCHAR(10) Collate Database_Default,
	DocRate NUMERIC(19,6),
	ValorDocME NUMERIC(19,6),
	ChavePgto INT,
	ChavePgtoString NVARCHAR(10) Collate Database_Default,
	Pagamento DATETIME,
	TipoObjPgto NVARCHAR(40) Collate Database_Default,
	TipoPgto NVARCHAR(2) Collate Database_Default,
	TabelaPgto NVARCHAR(4) Collate Database_Default,
	ValorPago NUMERIC(19,6),
	JurosDesconto NUMERIC(19,6),
	ValorSaldo NUMERIC(19,6),
	PgtoCur NVARCHAR(10) Collate Database_Default,
	PgtoRate NUMERIC(19,6),
	ValorPagoME NUMERIC(19,6),
	FormaPgto NVARCHAR(MAX) Collate Database_Default,
	Comments NVARCHAR(MAX) Collate Database_Default,
	AcctCode NVARCHAR(50) Collate Database_Default,
	AcctName NVARCHAR(200) Collate Database_Default,
	DocStatus CHAR(1) Collate Database_Default,	
	DiffDias  int,
	SlpCode INT,
	SlpName NVARCHAR(100) Collate Database_Default,
	GrupoEconCode NVARCHAR(50) Collate Database_Default,
	GrupoEconDesc NVARCHAR(200) Collate Database_Default
														
)

select @PrevisoesParam=case when (@Provisoes='P' or @Provisoes='T') then 'Y' else 'N' end 
insert into #contasReceber
exec spc_CVA_RelatorioCR_Nova 
 'T'
 --,'2018-05-25'
 --,'2018-05-25'
 --,'2018-05-25'
 --,'2018-05-25'
 --,'N'

 ,null
 ,null
 ,@dataInicial
 ,@dataFinaL 
 --,@DtVencimentoIni 
 --,@DtVencimentoFin 
 ,@PrevisoesParam 

CREATE TABLE #ContasPagar
(
	ChaveDoc INT,
	ChaveDocString NVARCHAR(10) Collate Database_Default,
	Numero NVARCHAR(50) Collate Database_Default,
	Parcela INT,
	ParcelaString NVARCHAR(10) Collate Database_Default,
	CardCode NVARCHAR(50) Collate Database_Default,
	CardName NVARCHAR(200) Collate Database_Default,
	CardFName NVARCHAR(200) Collate Database_Default,
	CardCodeName NVARCHAR(300) Collate Database_Default,
	CNPJ NVARCHAR(20) Collate Database_Default,
	TipoObj NVARCHAR(40) Collate Database_Default,
	TipoDoc NVARCHAR(2) Collate Database_Default,
	TabelaDoc NVARCHAR(4) Collate Database_Default,
	Emissao DATETIME,
	Vencimento DATETIME,
	ValorDoc NUMERIC(19,6),
	DocCur NVARCHAR(10) Collate Database_Default,
	DocRate NUMERIC(19,6),
	ValorDocME NUMERIC(19,6),
	ChavePgto INT,
	ChavePgtoString NVARCHAR(10) Collate Database_Default,
	Pagamento DATETIME,
	TipoObjPgto NVARCHAR(40) Collate Database_Default,
	TipoPgto NVARCHAR(2) Collate Database_Default,
	TabelaPgto NVARCHAR(4) Collate Database_Default,
	ValorPago NUMERIC(19,6),
	JurosDesconto NUMERIC(19,6),
	ValorSaldo NUMERIC(19,6),
	PgtoCur NVARCHAR(10) Collate Database_Default,
	PgtoRate NUMERIC(19,6),
	ValorPagoME NUMERIC(19,6),
	FormaPgto NVARCHAR(MAX) Collate Database_Default,
	Comments NVARCHAR(MAX) Collate Database_Default,
	AcctCode NVARCHAR(50) Collate Database_Default,
	AcctName NVARCHAR(200) Collate Database_Default,
	DocStatus CHAR(1) Collate Database_Default,
	DiffDias  int
)
select @PrevisoesParam=case when (@Provisoes='C' or @Provisoes='T') then 'Y' else 'N' end 
insert into #ContasPagar
exec spc_CVA_RelatorioCP_Nova 
 'T'
 ,null
 ,null
 ,@dataInicial
 ,@dataFinal
 --,@DtVencimentoIni 
 --,@DtVencimentoFin 
 ,@PrevisoesParam 

 insert into #lancamentos (tipo, docnum, cardcode, cardname, dt_vencimento, valor,valornota,valorADT,restante,DocEntry)
       SELECT tipo
          , docnum
          , CardCode
          , cardname
          , dt_vencimento 
          , sum(vl_saldo)* -1
		  ,null
		  ,null
		  ,null
		  ,docentry
    FROM (
		 select
			 CASE 
				when TipoObj='30' then 'OJDT'
				when TipoObj='18' then 'OPCH'
				when TipoObj='19' then 'ORPC'
				when TipoObj='204' then 'ODPO'
				when TipoObj='203' then 'ODPI'
				when TipoObj='243000002' then 'Imposto'	
				when TipoObj='-3'then 'LR'
				when TipoObj='-2'then 'TR'		
				else TipoObj+'????'
			 end AS 'tipo' 
			, ChaveDoc as 'docnum'
			, CardCode as 'CardCode'
			, CardName as 'cardname'
			, Vencimento as 'dt_vencimento'
			, ValorSaldo as 'vl_saldo'	
			, ChaveDoc as 'docentry'		
		 from
			#ContasPagar
  ) as completo
    where completo.dt_vencimento <= @dataFinal 
    group by tipo, docnum, cardcode, cardname, dt_vencimento,docentry
  
  

  insert into #lancamentos (tipo, docnum, cardcode, cardname, dt_vencimento, valor ,valornota,valorADT,restante,DocEntry)
       SELECT tipo
          , docnum
          , CardCode
          , cardname
          , dt_vencimento 
          , sum(vl_saldo)
		  ,null
		  ,null
		  ,null
		  ,docentry
    FROM (
		 select
			 CASE 
				when TipoObj='30' then 'OJDT'
				when TipoObj='13' then 'OINV'
				when TipoObj='14' then 'ORIN'
				when TipoObj='24' then 'ORCT'
				when TipoObj='204' then 'ODPO'
				when TipoObj='203' then 'ODPI'
				when TipoObj='Cheque' then 'Cheque'				
				else TipoObj+'????'
			 end AS 'tipo' 
			, ChaveDoc as 'docnum'
			, CardCode as 'CardCode'
			, CardName as 'cardname'
			, Vencimento as 'dt_vencimento'
			, ValorSaldo as 'vl_saldo'
			, ChaveDoc as 'docentry'
		 from
			#ContasReceber
  ) as completo
    where completo.dt_vencimento <= @dataFinal 
    group by tipo, docnum, cardcode, cardname, dt_vencimento,docentry

  delete from #lancamentos
   where dt_vencimento > @dataFinal
     
   
  IF @atrasos = 'N'
	begin
			delete from #lancamentos  where convert(char(10), dt_vencimento, 120) < convert(char(10), @dataInicial, 120)
	end
  ELSE
    Begin    
		
		
		update #lancamentos set dt_vencimento = @dataInicial where dt_vencimento < convert(char(10), @dataInicial, 120) 
		--select * from #lancamentos where   dt_vencimento  = '2018-06-19'  order by 2
	End

  declare @saldo_final decimal(19, 2)
  
  create table #saldo (acctcode varchar(72) null, saldo money null)
  
  insert into #saldo
    exec spcCVABalancete @caixa   

if @caixa = 'S'
   begin

	   select @saldo_inicial = SUM(saldo) from #saldo 
	   
	   select @saldo_inicial = ISNULL(@saldo_inicial, 0)
	   
	   set @SaldoInicial = @saldo_inicial 	   
	   
   end
  else
   begin 
	   
	   set @SaldoInicial = @SaldoInicial
	   
   end
   print @saldoInicial

  delete from #lancamentos where  not (dt_vencimento>=@dataInicial and dt_vencimento<=@dataFinal)

---------------
  select distinct dt_vencimento as 'Data'
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
		   when 'LR' then 'Lançamentos Periodicos'
		   when 'TR' then 'Trasações Recorrentes'		   
           else tipo end 'TP'
       , DocNum as 'Numero'
       , CardCode as 'Cliente'
       , CardName as 'Razão'
       , valor as 'Valor'
       , saldo as 'Saldo'
	   , dt_vencimento as 'Vencimento'
       , ROW_NUMBER() over (order by dt_vencimento, tipo, docnum, cardcode) as linha	
	   ,DocEntry    
     into #final
    from #lancamentos 
   order by dt_vencimento, tipo, docnum, cardcode  

  declare cp1 cursor local fast_forward for
    select linha, Valor
      from #final
     order by 1
     
  open cp1
  
  while 1 = 1
  begin
    fetch next from cp1 into @vlinha, @vvalor
    if @@FETCH_STATUS <> 0 break
        
    select @SaldoInicial = @SaldoInicial+ isnull(@vvalor, 0)
    
    update #final 
       set saldo =@SaldoInicial
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

 select distinct *
        ,isnull((select sum(valor) from #lancamentos where Tipo = 'PC' and dt_vencimento = #final.Vencimento),0) as 'PC'  
		,isnull((select sum(valor) from #lancamentos where Tipo = 'PV' and dt_vencimento = #final.Data),0) as 'PV'
		,isnull((select sum(Valor) from #lancamentos where CardCode like 'C%' and Tipo <> 'PV' and dt_vencimento = #final.Data),0) as 'Receber'
		,isnull((select sum(Valor) from #lancamentos where CardCode like 'F%'and Tipo <> 'PC' and dt_vencimento = #final.Data),0) as 'Pagar'
   from #final 


 order by linha 


GO

