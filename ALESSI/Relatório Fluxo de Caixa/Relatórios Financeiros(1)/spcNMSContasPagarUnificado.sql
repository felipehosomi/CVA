USE Greylogix
GO

/****** Object:  StoredProcedure [dbo].[spcNMSContasPagarUnificado]    Script Date: 06/09/2015 09:17:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcNMSContasPagarUnificado]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcNMSContasPagarUnificado]
GO

USE Greylogix
GO

/****** Object:  StoredProcedure [dbo].[spcNMSContasPagarUnificado]    Script Date: 06/09/2015 09:17:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




--select  'V' 'Tipo','Dt. Vencimento' 'Desc' union all select  'LC','Dt. Lan�amento' union all select  'L','Dt. Liquida��o'

--execute spcNMSContasPagarUnificado 'F00000018','2015-04-01 00:00:00','2015-05-31 00:00:00','L'

--execute [spcNMSContasAPagarPorVencimento] '*','2010-01-01 00:00:00','2020-01-01 00:00:00','2010-01-01 00:00:00','2020-01-01 00:00:00'

CREATE PROC [dbo].[spcNMSContasPagarUnificado] (
	@CarCode nvarchar(15),
	@dateini  date,
	@datefim  date,
	@tpdata   varchar(2)

	--@LancamentoIni datetime2,
	--@LancamentoFim datetime2,
	--@VencimentoIni datetime2,
	--@VencimentoFim datetime2,
	--@LiquidacaoIni datetime2,
	--@LiquidacaoFim datetime2
	)
with encryption
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


