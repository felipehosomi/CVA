IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spc_CVA_ContasPagarUnificado')
	DROP PROCEDURE spc_CVA_ContasPagarUnificado
GO
CREATE PROC [dbo].spc_CVA_ContasPagarUnificado (
	@CarCode nvarchar(15),
	@dateini		datetime,
	@datefim		datetime,
	@tpdata			varchar(2)
	--@LancamentoIni datetime2,
	--@LancamentoFim datetime2,
	--@VencimentoIni datetime2,
	--@VencimentoFim datetime2,
	--@LiquidacaoIni datetime2,
	--@LiquidacaoFim datetime2
	)
as 
begin
	CREATE TABLE #ContasAPagarPorVencimento (		
		ShortName nvarchar(30)
		, CardName nvarchar(200)
		, Lancamento datetime
		, Vencimento datetime
		, Origem nvarchar(40)
		, OrigemNr integer
		, Parcela smallint
		, ParcelaTotal smallint
		, Serial int
		, LineMemo nvarchar(100)
		, Debit decimal(19, 9)
		, Credit decimal(19, 9)
		, Saldo decimal(19, 9)
		, DueDate datetime
		,PeyMethodNF NVARCHAR(200)
	)
	--PRINT 'JOAO1'
	insert into #ContasAPagarPorVencimento
	execute spcNMSContasAPagarPorVencimento 
		@CarCode,
		@dateini,
		@datefim,
		@tpData
		--@LancamentoIni,
		--@LancamentoFim,
		--@VencimentoIni,
		--@VencimentoFim
	
	CREATE TABLE #ContasPagasPorFornecedor(
		FormaPagamento nvarchar(200)
		, CP int
		, CardCode nvarchar(200)
		, CardName nvarchar(200)
		, TransId int
		, Memo nvarchar(200)
		, InvType nvarchar(200)
		, TPDoc nvarchar(200)
		, NDoc int
		, Parcela smallint
		, ParcelaTotal smallint
		, ValorParcela  decimal(19, 9)
		, Vencimento datetime
		, Liquidacao datetime
		, ValorPago  decimal(19, 9)
		, Serial int
		
	)


	insert into #ContasPagasPorFornecedor
	execute spcNMSContasPagasPorFornecedor 
		@CarCode,
		@dateini,
		@datefim,
		@tpData
		--, @VencimentoIni
		--, @VencimentoFim
		--, @LiquidacaoIni
		--, @LiquidacaoFim
		--, 0


	--select top 2
	--	--ShortName
	--	--, CardName
	--	--, Lancamento
	--	--, Vencimento
	--	--, 
	--	--Origem
	--	--OrigemNr
	--	--, Parcela
	--	--, ParcelaTotal
	--	--, Serial
	--	--, 
	--	--LineMemo
	--	--, 
	--	Debit
	--	, Credit
	--	, Saldo
	-- from #ContasAPagarPorVencimento
	--select top 2
	--	--FormaPagamento
	--	--, 
	--	--CP
	--	----, CardCode
	--	----, CardName
	--	--, TransId
	--	--, Memo
	--	--, InvType
	--	--, TPDoc
	--	--, NDoc
	--	--, Parcela
	--	ValorParcela
	--	--, Vencimento
	--	--, Liquidacao
	--	, ValorPago
	--	--, Serial
	--from #ContasPagasPorFornecedor	
	select 
		'À Pagar' as 'TIPO'
		, ShortName
		, CardName
		, Lancamento
		, Vencimento	
		, null 'Liquidacao'
		, Parcela
		, ParcelaTotal
		, Serial		
		, Origem
		, OrigemNr
		--, null TPDoc
		, LineMemo
		, null as 'FormaPagamento'
		, null 'CP'
		, null 'TransId'
		
		, null 'ValorParcela'
		, null 'ValorPago'
		
		, Debit
		, Credit
		, Saldo		
		,DueDate
	from
		#ContasAPagarPorVencimento
	union all
	select 
		'Pago' as 'TIPO'
		, CardCode
		, CardName	
		, null 'Lancamento'
		, Vencimento
		, Liquidacao
		, Parcela
		, ParcelaTotal
		, Serial
		, InvType
		, NDoc
		--, TPDoc
		, Memo
		, FormaPagamento
		, CP
		, TransId
		
		, ValorParcela
		, ValorPago		
		
		, null 'Debit'
		, null 'Credit'
		, null 'Saldo'				
		,null
	from #ContasPagasPorFornecedor
	

	drop table #ContasAPagarPorVencimento
	drop TABLE #ContasPagasPorFornecedor

end 







GO


