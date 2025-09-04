IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spc_CVA_FluxoCaixa_CARGA')
	DROP PROCEDURE spc_CVA_FluxoCaixa_CARGA
GO
CREATE proc [dbo].[spc_CVA_FluxoCaixa_CARGA]

	@dataDe			smalldatetime,
	@dataAte		smalldatetime,
	@provisoes		char(1), 
	@formaPagamento nvarchar(200)
as
BEGIN
	declare @vGroupNum int
	declare @vShipDate smalldatetime
	declare @vLineTotal money
	declare @vTipo varchar(3)

	IF @provisoes = 'Y'
	BEGIN
		declare cp1 cursor local fast_forward read_only for
		select 'PRE' as tipo
				, ordr.groupnum as forma
				, rdr1.ShipDate as data_base
				, SUM(rdr1.linetotal) as valor
		from RDR1
			inner join ORDR on ORDR.DocEntry = rdr1.DocEntry 
		where rdr1.InvntSttus = 'O'
		and ordr.DocStatus = 'O'
		group by ordr.groupnum, rdr1.ShipDate

		UNION ALL

		select 'PRO' as tipo
				, oPOR.groupnum as forma
				, POR1.ShipDate as data_base
				, SUM(POR1.linetotal) as valor
		from POR1
			inner join OPOR on oPOR.DocEntry = POR1.DocEntry 
		where POR1.InvntSttus = 'O'
			and oPOR.DocStatus = 'O'   
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
	END

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
	Serial			 nvarchar(200),
	FormaPagamento   varchar(100),      
	PeyMethodNF   	 varchar(100),	
	BancoNF			 varchar(100),
	Installmnt		 varchar(100),
	OrctComments	 varchar(200),
	BankName         varchar(100),
	DocEntryNFS	 int,
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
	,DueDate datetime
	,PeyMethodNF nvarchar (40)
	)

	insert into #ContasAPagarPorVencimento
	execute spc_CVA_ContasAPagarPorVencimento 
	'*'
	,@dataDe
	,@dataAte
	,'V'
	,@provisoes
	,@formaPagamento

	insert into #lancamentos (tipo, dt_vencimento, valor)
	SELECT 'CAP'
			, dt_vencimento, 
			sum(vl_saldo)
	FROM
	(
		select 
			Saldo as 'vl_saldo' ,
			Vencimento as 'dt_vencimento'
		from 	
		#ContasAPagarPorVencimento
	) as completo
	where dt_vencimento BETWEEN @dataDe AND @dataAte 
	group by dt_vencimento 

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
	where dt_vencimento BETWEEN @dataDe AND @dataAte 
	group by dt_vencimento 

	drop table #ContasAReceberPorVencimento

	drop table #ContasAPagarPorVencimento
END