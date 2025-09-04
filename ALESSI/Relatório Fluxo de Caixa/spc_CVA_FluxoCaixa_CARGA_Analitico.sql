IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'P' AND name = 'spc_CVA_FluxoCaixa_CARGA_Analitico')
	DROP PROCEDURE spc_CVA_FluxoCaixa_CARGA_Analitico
GO
CREATE proc [dbo].[spc_CVA_FluxoCaixa_CARGA_Analitico]
	@dataDe			smalldatetime,
	@dataAte		smalldatetime,
	@saldoInicial	numeric(19, 2),
	@atrasos		char(1) = 'N',
	@caixa			char(1) = 'N',
	@provisoes		char(1) = 'N',
	@formaPagamento nvarchar(200)
as 
BEGIN
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

	create table #lancamentos 
	(
		  tipo char(4) null
		, DocNum varchar(100) null
		, CardCode varchar(30) null
		, CardName varchar(200) null
		, dt_vencimento smalldatetime null
		, valor decimal(19, 2) null
		, saldo decimal(19, 2) null
	)

	declare cp1 cursor local fast_forward read_only for
	select 'PV' as tipo
			, ordr.docnum
			, ordr.cardcode
			, ordr.cardname
			, ordr.groupnum as forma
			, rdr1.ShipDate as data_base
			, SUM(rdr1.linetotal) as valor
	from RDR1
		inner join ORDR on ORDR.DocEntry = rdr1.DocEntry 
	where rdr1.InvntSttus = 'O'
		and ordr.DocStatus = 'O'
		and (ordr.PeyMethod = @formaPagamento or isnull(@formaPagamento, '*') = '*')
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
		inner join OPOR  on oPOR.DocEntry = POR1.DocEntry 
	where POR1.InvntSttus = 'O'
		and oPOR.DocStatus = 'O'   
		and (oPOR.PeyMethod = @formaPagamento or isnull(@formaPagamento, '*') = '*')
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


	CREATE TABLE #ContasAPagarPorVencimento (		
	ShortName nvarchar(30)
	, CardName nvarchar(200)
	, Lancamento datetime
	, Vencimento datetime
	, Origem nvarchar(40)
	, OrigemNr nvarchar(100)
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

	insert into #ContasAPagarPorVencimento
	execute spc_CVA_ContasAPagarPorVencimento
	'*'
	,@dataDe
	,@dataAte
	,'V'
	,@provisoes
	,@formaPagamento
  
	CREATE TABLE #ContasAReceberPorVencimento (
	TransId int, 
	Line_ID int, 
	Account nvarchar(30),
	ShortName  nvarchar(30),
	TransType nvarchar(40),
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
	Serial  nvarchar(200),
	FormaPagamento nvarchar(100),
	PeyMethodNF    nvarchar(100),
	BancoNF        nvarchar(100),
	Installmnt	   nvarchar(200),
	OrctComments   nvarchar(500),
	BankName	   nvarchar(100),
	Nf			int,
	LateDays	int
	)

	insert  into #ContasAReceberPorVencimento
	EXECUTE [spc_CVA_ContasAReceberPorVencimento] 
	'*'
	,@dataDe
	,@dataAte
	,'V'
	,'*'
	,@formaPagamento
	,'*'
	,''
	,'Y'

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
				when Origem='-1' then 'OCFL'
				when Origem='-2' then 'ORCP'
				else '????'
				end AS 'tipo' 
			, OrigemNr as 'docnum'
			, ShortName as 'CardCode'
			, CardName as 'cardname'
			, Vencimento as 'dt_vencimento'
			, Saldo as 'vl_saldo'

			from
			#ContasAPagarPorVencimento   
	) as completo
	where completo.dt_vencimento BETWEEN @dataDe AND @dataAte 
	group by tipo, docnum, cardcode, cardname, dt_vencimento
  
  

	insert into #lancamentos (tipo, docnum, cardcode, cardname, dt_vencimento, valor)
		SELECT tipo
			, docnum
			, CardCode
			, cardname
			, dt_vencimento 
			, sum(vl_saldo)
	FROM (

			select
				CASE 
				when TransType='14' then 'ORIN'
				when TransType='203' then 'ODPI'
				when TransType='30' then 'OJDT'
				when TransType='13' then 'OINV'
				when TransType='24' then 'ORCT'
				when TransType='14' then 'ORIN'
				when TransType='-1' then 'OCFL'
				when TransType='-2' then 'ORCP'
				else '????'
				end AS 'tipo' 
			, BaseRef as 'docnum'
			, ShortName as 'CardCode'
			, CardName as 'cardname'
			, DueDate as 'dt_vencimento'
			, Saldo as 'vl_saldo'

			from
			#ContasAReceberPorVencimento

	) as completo
	where completo.dt_vencimento BETWEEN @dataDe AND @dataAte 
	group by tipo, docnum, cardcode, cardname, dt_vencimento

	delete from #lancamentos
	where dt_vencimento > @dataAte
   
	if @atrasos = 'N'
	delete from #lancamentos
		where convert(char(10), dt_vencimento, 120) < convert(char(10), GETDATE(), 120)

	declare @saldo_final decimal(19, 2)
  
	create table #saldo (acctcode varchar(MAX) null, saldo money null)
  
	if @caixa = 'Y'
	begin
		insert into #saldo
		exec spc_CVA_Balancete @caixa, @saldoInicial
  
		select @saldo_inicial = SUM(saldo) from #saldo 
		select @saldo_inicial = ISNULL(@saldo_inicial, 0)  
	end
	else
	begin
	select @saldo_inicial = @saldoInicial
	end

	---------------
	select dt_vencimento as 'Data'
		, tipo as 'Tipo'
		, CASE tipo
			when 'OJDT' then 'L.C.'
			when 'ORIN' then 'Dev. NF Venda'
			when 'OINV' then 'NF Venda'
			when 'OPCH' then 'NF Compra'
			when 'OBOE' then 'Boleto'
			when 'PV' then 'P. Venda'
			when 'PC' then 'P. Compra'
			when 'OCHH' then 'Cheque'
			when 'ORCT' then 'Contas À Receber'
			when 'OCFL' then 'Lanç. Previsto'
			when 'ORCP' then 'Trans. Recorrente'
			when 'ODPI' then 'Adiant. Cliente'
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

	select * from #final order by linha 
END