USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[spcJBCContasReceberUnificado]    Script Date: 06/09/2015 09:17:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCContasReceberUnificado]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCContasReceberUnificado]
GO

USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[spcJBCContasReceberUnificado]    Script Date: 06/09/2015 09:17:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--select  'V' 'Tipo','Dt. Vencimento' 'Desc' union all select  'LC','Dt. Lan�amento' union all select  'L','Dt. Liquida��o'

CREATE PROC [dbo].[spcJBCContasReceberUnificado] (
	@cardcode varchar(30),	
	@dateini  date,
	@datefim  date,
	@tpData   varchar(2)
	,@CVA_GRUPOECON  nvarchar(60)
	,@slpCode int	
)
with encryption
as 
begin
--go
----execute [spcJBCContasReceberUnificado] '*','2010-01-01 00:00:00','2020-01-01 00:00:00','V'
--	if DATEDIFF(day,getdate(),cast('2016-11-20' as date))<=0 begin
--		RAISERROr ('Claudino Software: Tempo de uso de Demostra��o Expirado: 41 99327681', 11,1)
--	end
CREATE TABLE #ContasAReceberPorVencimento (
	TransId int, 
	Line_ID int, 
	Account nvarchar(60),
	ShortName  nvarchar(60),
	TransType nvarchar(100),
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
	, Dias int null	
)
--go

insert  into #ContasAReceberPorVencimento
EXECUTE [spcJBCContasAReceberPorVencimento] 
@CardCode,
@dateini,@datefim,@tpData,'*',@CVA_GRUPOECON,@slpCode,'S'


--@RefDateIni,
--@RefDateFim,
--@DueDateIni,
--@DueDateFim
  -- '*'
  --,'2000-11-20'
  --,'2030-11-20'
  --,'2000-11-16'
  --,'2030-11-24'

--go

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
	, Dias int null	
)
--go

insert into #ContasRecebidasPorCliente
EXECUTE [spcJBCContasRecebidasPorCliente] 
	@cardcode,
	@dateini,@datefim,@tpData,@CVA_GRUPOECON,@slpCode
	----0,
	--@duedateini,
	--@duedatefim,
	--@paydateini,
	--@paydatefim
  -- '*'
  --,0
  --,'2014-11-20'
  --,'2030-11-20'
  --,'2014-11-16'
  --,'2030-11-24'  
  
--go

select 	
	'AR' as Tipo
	, CardCode 
	, CardName
	, DueDate as 'Vencimento'
	, RefDate as 'Lancamento'
	, null as 'liquidacao'
	, TransId

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
	,Dias
	
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
	,Dias
from 
	#ContasRecebidasPorCliente
order by 2,4

--go

--select top 3
--	--TransType,
--	--BaseRef,
--	--SourceLine,
--	--RefDate,--lan�amento
--	--DueDate,--vencimento
--	BalDueCred,
--	BalDueDeb,
--	Saldo,
--	LineMemo
--	--,CardName,
--	--CardCode 
--from 
--	#ContasAReceberPorVencimento

--go  
--select top 3
--	--operacao,
--	parcela,
--	--crentry,
--	--Codigo,
--	--Parceiro,
--	valorparcela
--	--, vencimento,
--	--liquidacao,
--	--valorpago,
--	--transacao,
--	----transid,
--	--boenum,
--	--boemeth
--	--, notas
--	--,Juros
-- from #ContasRecebidasPorCliente

--go 
drop table #ContasAReceberPorVencimento

--go
drop table #ContasRecebidasPorCliente

end 

GO


execute spcJBCContasReceberUnificado '*','2015-01-01','2018-02-28','V','*',0