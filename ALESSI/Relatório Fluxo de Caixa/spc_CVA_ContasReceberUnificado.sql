IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spc_CVA_ContasReceberUnificado')
	DROP PROCEDURE spc_CVA_ContasReceberUnificado
GO
CREATE PROC [dbo].[spc_CVA_ContasReceberUnificado] (
	@cardcode		varchar(30),	
	@dateini		datetime,
	@datefim		datetime,
	@tpData			varchar(2),
	@grupoEconomico	nvarchar(100),
	@validaDiaUtil	varchar(1)
)
as 
begin

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
    Serial int,
	FormaPagamento nvarchar(100),
	PeyMethodNF nvarchar (300),
	BancoNF nvarchar (600),
	Installmnt nvarchar (600),
	Comments nvarchar (508),
	BankName nvarchar (500)
	,DocEntryNFS	 int
	,LateDays		 int
)
--go
insert  into #ContasAReceberPorVencimento

EXECUTE [spc_CVA_ContasAReceberPorVencimento]
	@CardCode, @dateini, @datefim, @tpData, '*', '*', @grupoEconomico, '', @validaDiaUtil

CREATE TABLE #ContasRecebidasPorCliente (
	  operacao       varchar(100)   null
	, objtype        varchar(72)    null
	, metodo         varchar(200)   null
	, parcela        int            null
	, crentry        int            null
	, Codigo         varchar(30)    null
	, Parceiro       varchar(200)   null
	, valorparcela   decimal(19, 9) null
	, vencimento     smalldatetime  null
	, liquidacao     smalldatetime  null
	, valorpago      decimal(19, 9) null
	, transacao      varchar(1000)   null
	, jrnlmemo       varchar(max)   null
	, transid        int            null
	, boenum         int            null
	, boemeth        varchar(200)   null
	, notas          varchar(200)   null
	, Juros        decimal(19, 9)   null
	, formaPagamento varchar(200)	null
)
--go

insert into #ContasRecebidasPorCliente
EXECUTE [spc_CVA_ContasRecebidasPorCliente] 
	@cardcode, @dateini, @datefim, @tpData, @grupoEconomico, 0, @validaDiaUtil

select 	
	'AR' as Tipo
	, CardCode 
	, CardName
	, DueDate as 'Vencimento'
	, RefDate as 'Lancamento'
	, null as 'liquidacao'
	, TransId
	, null as 'Juros'
	, null 'crentry'
	, null 'notas'	
	, TransType
	, BaseRef
	, SourceLine	
    , null as 'parcela'
	, null as 'valorparcela'
	, Saldo as 'valorpago'
	, Saldo
	, Serial
	,FormaPagamento
	,PeyMethodNF
	,BancoNF 
	,Installmnt
	,Comments
	,BankName
	
from 
	#ContasAReceberPorVencimento
union all
select
	'R' as Tipo
	, Codigo
	, Parceiro
	, vencimento
	, null as 'Lancamento'
	, liquidacao
	, transid
	, Juros
	, crentry
	, notas
	, '24' as 'TransType'
	, null as 'BaseRef'
	, null as 'SourceLine'
    , parcela
	, valorparcela
	, valorpago
	, null as 'Saldo'
	,null
	,null as 'FormaPagamento'
	,null
	,null
	,null
	,null
	,null
from 
	#ContasRecebidasPorCliente
order by 2,4


drop table #ContasAReceberPorVencimento
drop table #ContasRecebidasPorCliente

end 
GO

