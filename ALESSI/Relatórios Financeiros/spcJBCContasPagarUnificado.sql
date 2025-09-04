USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[spcJBCContasPagarUnificado]    Script Date: 06/09/2015 09:17:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCContasPagarUnificado]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCContasPagarUnificado]
GO

USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[spcJBCContasPagarUnificado]    Script Date: 06/09/2015 09:17:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROC [dbo].[spcJBCContasPagarUnificado] (
	@CarCode nvarchar(15),
	@dateini  date,
	@datefim  date,
	@tpdata   varchar(2),
	@CVA_GRUPOECON  nvarchar(60)
	)
	with encryption
as 

begin

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
		, DueDate datetime
		,PeyMethodNF NVARCHAR(200)
	)




	--PRINT 'JOAO1'
	insert into #ContasAPagarPorVencimento
	execute spcJBCContasAPagarPorVencimento 
		@CarCode,
		@dateini,
		@datefim,
		@tpData,
		'S',@CVA_GRUPOECON
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
		, Lancamento datetime
		
	)


	insert into #ContasPagasPorFornecedor
	execute spcJBCContasPagasPorFornecedor 
		@CarCode,
		@dateini,
		@datefim,
		@tpData,
		@CVA_GRUPOECON
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
		'� Pagar' as 'TIPO'
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
		, Lancamento 'Lancamento'
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



execute spcJBCContasPagarUnificado '*','2015-01-01','2050-01-01','V','*'