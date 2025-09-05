IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spc_CVA_RelatorioCR')
	DROP PROCEDURE spc_CVA_RelatorioCR
GO
CREATE PROCEDURE [dbo].[spc_CVA_RelatorioCR]
(
	@TipoData		CHAR(1),
	@DataInicio		DATE,
	@DataFim		DATE,
	@CardCode		NVARCHAR(100) = NULL,
	@GrupoPN		INT	= NULL,
	@NrRefPN		NVARCHAR(100) = NULL,
	@StatusCobr		INT = NULL,
	@Obs			NVARCHAR(MAX) = '',
	@StatusDoc		CHAR(1)
)
AS
BEGIN
	set dateformat 'ymd';
	set nocount on;

	BEGIN -- Criação de tabelas temporárias
	CREATE TABLE #tmp_docs
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
		BPGroupCode INT,
		BPGroupName NVARCHAR(80) Collate Database_Default,
		CNPJ NVARCHAR(20) Collate Database_Default,
		TipoObj NVARCHAR(40) Collate Database_Default,
		TipoDoc NVARCHAR(2) Collate Database_Default,
		TabelaDoc NVARCHAR(4) Collate Database_Default,
		Emissao DATETIME,
		Vencimento DATETIME,
		ValorDoc NUMERIC(19,6),
		DocCur NVARCHAR(10) Collate Database_Default,
		DocRate NUMERIC(19,6),
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
		FormaPgto NVARCHAR(MAX) Collate Database_Default,
		Comments NVARCHAR(MAX) Collate Database_Default,
		AcctCode NVARCHAR(50) Collate Database_Default,
		AcctName NVARCHAR(200) Collate Database_Default,
		DocStatus CHAR(1) Collate Database_Default,
		SlpCode INT,
		SlpName NVARCHAR(100) Collate Database_Default,
		StatusCobr INT,
		NrRefPN NVARCHAR(200) Collate Database_Default,
		Endereco NVARCHAR(MAX) Collate Database_Default,
		StatusPedido NVARCHAR(50) Collate Database_Default,
		Autorizacao NVARCHAR(50) Collate Database_Default,
		NumeroDocumento NVARCHAR(50) Collate Database_Default,
		TipoLicitacao NVARCHAR(50) Collate Database_Default,
		NrLicitacao NVARCHAR(50) Collate Database_Default
	)

	CREATE TABLE #tmp_return
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
		BPGroupCode INT,
		BPGroupName NVARCHAR(80) Collate Database_Default,
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
		SlpCode INT,
		SlpName NVARCHAR(100) Collate Database_Default,
		StatusCobr INT,
		NrRefPN NVARCHAR(200) Collate Database_Default,
		Endereco NVARCHAR(MAX) Collate Database_Default,	
		StatusPedido NVARCHAR(50) Collate Database_Default,			
		Autorizacao NVARCHAR(50) Collate Database_Default,
		NumeroDocumento NVARCHAR(50) Collate Database_Default,
		TipoLicitacao NVARCHAR(50) Collate Database_Default,
		NrLicitacao NVARCHAR(50) Collate Database_Default
	)
	END

	INSERT INTO #tmp_docs
	SELECT DISTINCT
		ISNULL(T0.SourceID, T0.TransId) AS [ChaveDoc]
		, CAST(ISNULL(T0.SourceID, T0.TransId) AS NVARCHAR(10)) AS [ChaveDocString]
		, ISNULL(T5.Serial, T0.TransId) AS [Numero]
		, ISNULL(T0.SourceLine, T0.Line_ID) AS [Parcela]
		, CASE WHEN T0.TransType = N'30' THEN '1/1' ELSE CAST(T0.SourceLine AS NVARCHAR(10)) + '/' + CAST(T5.Installmnt AS NVARCHAR(10)) END AS [ParcelaString]
		, T1.CardCode AS [CardCode]
		, T1.CardName AS [CardName]
		, T1.CardFName AS [CardFName]
		, T1.CardCode + ' - ' + T1.CardName AS [CardCodeName]
		, T1.GroupCode
		, OCRG.GroupName
		, CASE WHEN ISNULL(T3.TaxId4, '') <> '' THEN T3.TaxId4 ELSE T3.TaxId0 END AS [CNPJ]
		, T0.TransType AS [TipoObj]
		, CASE T0.TransType 
			WHEN N'30'  THEN 'LC'
			WHEN N'13'  THEN 'NS'
			WHEN N'203' THEN 'AT'
			WHEN N'14'  THEN 'DS'
		END AS [TipoDoc]
		, CASE T0.TransType 
			WHEN N'30'  THEN 'OJDT'
			WHEN N'13'  THEN 'OINV'
			WHEN N'203' THEN 'ODPI'
			WHEN N'14'  THEN 'ORIN'
		END AS [TabelaDoc]
		, T2.RefDate AS [Emissao]
		, T0.DueDate AS [Vencimento]
		, T0.Debit - T0.Credit AS [ValorParcela]
		, ISNULL(T0.FCCurrency, 'R$') AS [DocCur]
		, CAST(
			CASE WHEN T0.FCCurrency IS NOT NULL THEN
				CASE WHEN T0.FCDebit <> 0 THEN T0.Debit / T0.FCDebit
				ELSE
					CASE WHEN T0.FCCredit <> 0 THEN T0.Credit / T0.FCCredit
					ELSE 0 END
				END
			ELSE 1 END
		AS NUMERIC(19,6)) AS [DocRate]
		, OA_PGTO.DocEntry AS [ChavePgto]
		, CAST(OA_PGTO.DocEntry AS NVARCHAR(10)) AS [ChavePgtoString]
		, OA_PGTO.Pagamento AS [Pagamento]
		, OA_PGTO.ObjType AS [TipoObjPgto]
		, CASE WHEN OA_PGTO.DocEntry IS NOT NULL THEN 'CR' ELSE NULL END AS [TipoPgto]
		, CASE WHEN OA_PGTO.DocEntry IS NOT NULL THEN 'ORCT' ELSE NULL END AS [TabelaPgto]
		, CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.SumApplied ELSE 0.00 END AS [ValorPago]
		, OA_PGTO.UndOvDiff AS [JurosDesconto]
		, T0.BalDueDeb - T0.BalDueCred AS [ValorSaldo]
		, OA_PGTO.DocCurr AS [PgtoCur]
		, OA_PGTO.DocRate AS [PgtoRate]
		, OA_PGTO.Descript AS [FormaPgto]
		--, T0.LineMemo AS [Comments]
		, CASE WHEN T5.Comments IS NOT NULL THEN T5.Comments ELSE ISNULL(T0.U_Memo, '') + ' - ' + ISNULL(T0.Ref1, '') + ' - ' + ISNULL(T0.Ref2, '') END Comments
		, OA_PGTO.Conta AS [AcctCode]
		, OA_PGTO.NomeConta AS [AcctName]
		, CASE WHEN (T0.BalDueDeb - T0.BalDueCred) = 0 THEN 'F' ELSE 'A' END AS [DocStatus]
		, ISNULL(T6.SlpCode, T7.SlpCode) AS [SlpCode]
		, ISNULL(T6.SlpName, T7.SlpName) AS [SlpName]
		, CASE WHEN T0.TransType = '13' 
			THEN T4.U_CVA_Status
			ELSE T2.U_CVA_Status
		END StatusCobr
		, T5.NumAtCard
		, T5.U_CVA_END 
		,(SELECT TOP 1 CASE WHEN X1.DocStatus = 'O' THEN 'Aberto' ELSE 'Fechado' END FROM RDR1 X0 WITH(NOLOCK) 
			INNER JOIN ORDR X1 ON X0.DocEntry  = X1.DocEntry 
		  WHERE X0.TrgetEntry = T5.DocEntry 
				AND X0.TargetType = T5.ObjType) AS [StatusPedido],
		CASE T5.U_TipoAutorizacao 
		WHEN 1 THEN 'Autorização de Compra'
		WHEN 2 THEN 'Autorização de Despesa' 
		WHEN 3 THEN 'Autorização de Fornecimento' 
		WHEN 4 THEN 'Empenho' 
		WHEN 5 THEN 'Ordem de Compra' 
		WHEN 6 THEN 'Ordem de Despesa' 
		WHEN 7 THEN 'Ordem de Fornecimento' 
		WHEN 8 THEN 'Pedido' 
		WHEN 9 THEN 'Requisição' 
		WHEN 10 THEN 'Solicitação' 
		WHEN 11 THEN 'Não possui' END,
		T5.U_Numerodocumento,
		CASE T5.U_TipoLicitacao 
		WHEN '1' THEN 'Carta Convite'
		WHEN '2' THEN 'Concorrência Pública'
		WHEN '3' THEN 'Pregão Eletrônico'
		WHEN '4' THEN 'Pregão Presencial'
		ELSE T5.U_TipoLicitacao 
		END,
		T5.U_NumeroLicitacao
	FROM  JDT1 T0
	INNER JOIN OCRD T1  ON  T0.ShortName = T1.CardCode AND T1.CardType = 'C'
	INNER JOIN OCRG ON OCRG.GroupCode = T1.GroupCode
	INNER JOIN OJDT T2  ON  T2.TransId = T0.TransId   
	INNER JOIN CRD7 T3  ON  T1.CardCode = T3.CardCode AND T3.[Address] = '' AND T3.AddrType = 'S'
	LEFT JOIN INV6 T4  ON  T0.SourceLine = T4.InstlmntID AND T0.SourceID = T4.DocEntry
	LEFT JOIN OINV T5  ON  T4.DocEntry = T5.DocEntry
	LEFT JOIN OSLP T6  ON  T5.SlpCode = T6.SlpCode
	LEFT  JOIN OSLP T7  ON  T1.SlpCode = T7.SlpCode
	OUTER APPLY (
		SELECT DISTINCT 
			ORCT.DocEntry
			, ORCT.ObjType
			, RCT2.SumApplied + (SELECT [dbo].fn_CVA_VariacaoCambialComissao(ORCT.DocEntry, ORCT.ObjType)) AS SumApplied
			, ORCT.UndOvDiff / OA_MAX.Countt AS UndOvDiff
			, ORCT.DocCurr
			, ORCT.DocRate
			, OA_DATA.Pagamento
			, OA_FORMA.Descript
			, OA_CONTA.Conta
			, OA_CONTA.NomeConta
		FROM ORCT
			INNER JOIN RCT2 ON ORCT.DocEntry = RCT2.DocNum
			LEFT  JOIN OBOE ON ORCT.BoeAbs = OBOE.BoeKey
			LEFT  JOIN OBOT ON OBOT.AbsEntry = (
				SELECT TOP 1 BOT1.AbsEntry FROM BOT1
				WHERE BOT1.BOENumber = OBOE.BoeNum AND BOT1.BoeType = OBOE.BoeType
				ORDER BY BOT1.AbsEntry DESC
			) AND OBOT.StatusTo <> 'C'
			LEFT  JOIN OJDT On OBOT.TransId = OJDT.Number
		OUTER APPLY (
			SELECT Countt = COUNT(TX.InvoiceId) FROM RCT2 TX WHERE TX.DocNum = ORCT.DocEntry
		) AS OA_MAX
		OUTER APPLY (
			SELECT Pagamento = 
				CASE WHEN OBOT.StatusTo <> 'C' THEN OJDT.RefDate
					ELSE CASE WHEN ORCT.BoeAbs IS NULL AND ORCT.DocEntry IS NOT NULL THEN ORCT.TaxDate
					ELSE NULL END END --END
		) AS OA_DATA
		OUTER APPLY (
			SELECT Conta = OACT.AcctCode, NomeConta = OACT.AcctName
			FROM OACT
			WHERE OACT.Finanse = 'Y' AND OACT.AcctCode = 
				CASE WHEN OBOT.StatusTo <> 'C' THEN (SELECT OPYM.GLAccount FROM OPYM WHERE OPYM.PayMethCod = OBOE.PayMethCod)
					ELSE CASE WHEN ORCT.BoeAbs IS NULL AND ORCT.DocEntry IS NOT NuLL THEN 
						CASE WHEN ORCT.TrsfrSum <> 0 THEN ORCT.TrsfrAcct ELSE ORCT.CashAcct END END END
		) AS OA_CONTA
		OUTER APPLY (
			SELECT Descript = 
				CASE WHEN ISNULL(OBOE.PayMethCod, ORCT.PayMth) IS NOT NULL THEN OPYM.Descript
					ELSE CASE WHEN ORCT.CashSum <> 0 THEN 'Dinheiro'
					ELSE CASE WHEN ORCT.TrsfrSum <> 0 THEN 'Transferência'
					ELSE CASE WHEN ORCT.BoeSum <> 0 THEN 'Boleto'
					ELSE CASE WHEN ORCT.[CheckSum] <> 0 THEN 'Cheque'
					ELSE NULL END END END END END
			FROM OPYM WHERE OPYM.PayMethCod = ISNULL(ISNULL(OBOE.PayMethCod, ORCT.PayMth), OPYM.PayMethCod)
		) AS OA_FORMA
		WHERE ORCT.Canceled = 'N' AND 
			OA_CONTA.Conta IS NOT NULL AND
			(
				(RCT2.DocEntry = T0.SourceID AND RCT2.InstId = T0.SourceLine AND RCT2.InvType = T0.TransType) OR
				(RCT2.DocEntry = T0.TransId AND RCT2.DocLine = T0.Line_ID AND RCT2.InvType = T0.TransType)
			)
	) AS OA_PGTO
	WHERE
		(T0.BalDueDeb - T0.BalDueCred) <> 0
		AND
		(
			(@TipoData = 'E' AND T0.RefDate BETWEEN @DataInicio AND @DataFim) OR
			(@TipoData = 'V' AND T0.DueDate BETWEEN @DataInicio AND @DataFim) OR
			(@TipoData = 'P' AND OA_PGTO.Pagamento BETWEEN @DataInicio AND @DataFim)
		)
		AND T1.CardCode = ISNULL(@CardCode, T1.CardCode)
		AND T1.GroupCode = ISNULL(@GrupoPN, T1.GroupCode)
		AND (ISNULL(T5.NumAtCard, '') = ISNULL(@NrRefPN, ISNULL(T5.NumAtCard, '')) OR ISNULL(T0.Ref1, '') = ISNULL(@NrRefPN, ISNULL(T0.Ref1, '')))
		AND (ISNULL(T5.Comments, '') LIKE '%' + @Obs + '%' OR ISNULL(T0.U_Memo, '') + ' - ' + ISNULL(T0.Ref1, '') + ' ' + ISNULL(T0.Ref2, '') LIKE '%' + @Obs + '%')
		AND ISNULL(T4.U_CVA_Status, 0) = ISNULL(@StatusCobr, ISNULL(T4.U_CVA_Status, 0))
		AND @StatusDoc = 'A'

	UNION ALL

	SELECT DISTINCT
		CASE T1.InvType
			WHEN N'13' THEN OINV.DocEntry
			WHEN N'14' THEN ORIN.DocEntry
			WHEN N'30' THEN JDT1.TransId
			WHEN N'203' THEN ODPI.DocEntry
		END AS [ChaveDoc]
		, CASE T1.InvType
			WHEN N'13' THEN CAST(OINV.DocEntry AS NVARCHAR(10))
			WHEN N'14' THEN CAST(ORIN.DocEntry AS NVARCHAR(10))
			WHEN N'30' THEN CAST(JDT1.TransId AS NVARCHAR(10))
			WHEN N'203' THEN CAST(ODPI.DocEntry AS NVARCHAR(10))
		END AS [ChaveDocString]
		, CASE T1.InvType
			WHEN N'13' THEN ISNULL(OINV.Serial, OINV.DocEntry)
			WHEN N'14' THEN ISNULL(ORIN.Serial, ORIN.DocEntry)
			WHEN N'30' THEN JDT1.TransId
			WHEN N'203' THEN ISNULL(ODPI.Serial, ODPI.DocEntry)
		END AS [Numero]
		, CASE T1.InvType
			WHEN N'13' THEN INV6.InstlmntID
			WHEN N'14' THEN RIN6.InstlmntID
			WHEN N'30' THEN JDT1.Line_ID
			WHEN N'203' THEN DPI6.InstlmntID
		END AS [Parcela]
		, CASE T1.InvType
			WHEN N'13' THEN CAST(INV6.InstlmntID AS NVARCHAR(10)) + '/' + CAST(OINV.Installmnt AS NVARCHAR(10))
			WHEN N'14' THEN CAST(RIN6.InstlmntID AS NVARCHAR(10)) + '/' + CAST(ORIN.Installmnt AS NVARCHAR(10))
			WHEN N'30' THEN '1/1'
			WHEN N'203' THEN CAST(DPI6.InstlmntID AS NVARCHAR(10)) + '/' + CAST(ODPI.Installmnt AS NVARCHAR(10))
		END AS [ParcelaString]
		, T5.CardCode AS [CardCode]
		, T5.CardName AS [CardName]
		, T5.CardFName AS [CardFName]
		, T5.CardCode + ' - ' + T5.CardName AS [CardCodeName]
		, T5.GroupCode
		, OCRG.GroupName
		, CASE WHEN ISNULL(T6.TaxId4, '') <> '' THEN T6.TaxId4 ELSE T6.TaxId0 END AS [CNPJ]
		, T1.InvType AS [TipoObj]
		, CASE T1.InvType
			WHEN N'13' THEN 'NS'
			WHEN N'14' THEN 'DS'
			WHEN N'30' THEN 'LC'
			WHEN N'203' THEN 'AT'
		END AS [TipoDoc]
		, CASE T1.InvType
			WHEN N'13' THEN 'OINV'
			WHEN N'14' THEN 'ORIN'
			WHEN N'30' THEN 'OJDT'
			WHEN N'203' THEN 'ODPI'
		END AS [TabelaDoc]
		, CASE T1.InvType
			WHEN N'13' THEN OINV.DocDate
			WHEN N'14' THEN ORIN.DocDate
			WHEN N'30' THEN JDT1.RefDate
			WHEN N'203' THEN ODPI.DocDate
		END AS [Emissao]
		, CASE T1.InvType
			WHEN N'13' THEN INV6.DueDate
			WHEN N'14' THEN RIN6.DueDate
			WHEN N'30' THEN JDT1.DueDate
			WHEN N'203' THEN DPI6.DueDate
		END AS [Vencimento]
		, CASE T1.InvType
			WHEN N'13' THEN INV6.InsTotal
			WHEN N'14' THEN RIN6.InsTotal
			WHEN N'30' THEN JDT1.Debit - JDT1.Credit
			WHEN N'203' THEN DPI6.InsTotal
		END AS [ValorParcela]
		, CASE T1.InvType
			WHEN N'13' THEN ISNULL(OINV.DocCur, 'R$')
			WHEN N'14' THEN ISNULL(ORIN.DocCur, 'R$')
			WHEN N'30' THEN ISNULL(JDT1.FCCurrency, 'R$')
			WHEN N'203' THEN ISNULL(ODPI.DocCur, 'R$')
		END AS [DocCur]
		, CASE T1.InvType
			WHEN N'13' THEN OINV.DocRate
			WHEN N'14' THEN ORIN.DocRate
			WHEN N'30' THEN CAST (
				CASE WHEN JDT1.FCCurrency IS NOT NULL THEN
					CASE WHEN JDT1.FCDebit <> 0 THEN JDT1.Debit / JDT1.FCDebit
					ELSE
						CASE WHEN JDT1.FCCredit <> 0 THEN JDT1.Credit / JDT1.FCCredit
						ELSE 0 END
					END
				ELSE 1 END
			AS NUMERIC(19,6))
			WHEN N'203' THEN ODPI.DocRate
		END AS [DocRate]
		, T0.DocEntry AS [ChavePgto]
		, CAST(T0.DocEntry AS NVARCHAR(10)) AS [ChavePgtoString]
		, OA_DATA.Pagamento AS [Pagamento]
		, T0.ObjType AS [TipoObjPgto]
		, 'CR' AS [TipoPgto]
		, 'ORCT' AS [TabelaPgto]
		, T1.SumApplied + (T0.UndOvDiff / OA_MAX.Countt) + (SELECT [dbo].[fn_CVA_VariacaoCambialComissao](T0.DocEntry, T0.ObjType)) AS [ValorPago]
		, T0.UndOvDiff / OA_MAX.Countt AS [JurosDesconto]
		, CASE T1.InvType
			WHEN N'13' THEN INV6.InsTotal - INV6.PaidToDate
			WHEN N'14' THEN RIN6.InsTotal - RIN6.PaidToDate
			WHEN N'30' THEN JDT1.BalDueDeb - JDT1.BalDueCred
			WHEN N'203' THEN DPI6.InsTotal - DPI6.PaidToDate
		END AS [ValorSaldo]
		, T0.DocCurr AS [PgtoCur]
		, T0.DocRate AS [PgtoRate]
		, OA_FORMA.Descript AS [FormaPgto]
		, CASE T1.InvType
			WHEN N'13' THEN CAST(OINV.Comments AS NVARCHAR(MAX))
			WHEN N'14' THEN CAST(ORIN.Comments AS NVARCHAR(MAX))
			WHEN N'30' THEN CAST(JDT1.LineMemo AS NVARCHAR(MAX))
			WHEN N'203' THEN CAST(ODPI.Comments AS NVARCHAR(MAX))
		END AS [Comments]
		, OA_CONTA.Conta AS [AcctCode]
		, OA_CONTA.NomeConta AS [AcctName]
		, 'F' AS [DocStatus]
		, CASE T1.InvType
			WHEN N'13' THEN OINV.SlpCode
			WHEN N'14' THEN ORIN.SlpCode
			WHEN N'30' THEN T5.SlpCode
			WHEN N'203' THEN ODPI.SlpCode
		END AS [SlpCode]
		, CASE T1.InvType
			WHEN N'13' THEN (SELECT SlpName FROM OSLP WHERE SlpCode = OINV.SlpCode)
			WHEN N'14' THEN (SELECT SlpName FROM OSLP WHERE SlpCode = ORIN.SlpCode)
			WHEN N'30' THEN (SELECT SlpName FROM OSLP WHERE SlpCode = T5.SlpCode)
			WHEN N'203' THEN (SELECT SlpName FROM OSLP WHERE SlpCode = ODPI.SlpCode)
		END AS [SlpName]
		,CASE T1.InvType
			WHEN N'13' THEN INV6.U_CVA_Status
			WHEN N'14' THEN RIN6.U_CVA_Status
			WHEN N'30' THEN ''
			WHEN N'203' THEN DPI6.U_CVA_Status
		END AS NrRefPN
		, CASE T1.InvType
			WHEN N'13' THEN OINV.NumAtCard
			WHEN N'14' THEN ORIN.NumAtCard
			WHEN N'30' THEN ''
			WHEN N'203' THEN ODPI.NumAtCard
		END AS NrRefPN
		, CASE T1.InvType
			WHEN N'13' THEN OINV.U_CVA_END
			WHEN N'14' THEN ORIN.U_CVA_END
			WHEN N'30' THEN ''
			WHEN N'203' THEN ODPI.U_CVA_END
		END AS NrRefPN,
		(SELECT TOP 1 CASE WHEN X1.DocStatus = 'O' THEN 'Aberto' ELSE 'Fechado' END FROM RDR1 X0 WITH(NOLOCK) 
			INNER JOIN ORDR X1 ON X0.DocEntry  = X1.DocEntry 
		  WHERE X0.TrgetEntry = OINV.DocEntry 
				 AND X0.TargetType = OINV.ObjType) AS [StatusPedido],
		CASE OINV.U_TipoAutorizacao 
		WHEN 1 THEN 'Autorização de Compra'
		WHEN 2 THEN 'Autorização de Despesa' 
		WHEN 3 THEN 'Autorização de Fornecimento' 
		WHEN 4 THEN 'Empenho' 
		WHEN 5 THEN 'Ordem de Compra' 
		WHEN 6 THEN 'Ordem de Despesa' 
		WHEN 7 THEN 'Ordem de Fornecimento' 
		WHEN 8 THEN 'Pedido' 
		WHEN 9 THEN 'Requisição' 
		WHEN 10 THEN 'Solicitação' 
		WHEN 11 THEN 'Não possui' END,
		OINV.U_Numerodocumento,
		CASE OINV.U_TipoLicitacao 
		WHEN '1' THEN 'Carta Convite'
		WHEN '2' THEN 'Concorrência Pública'
		WHEN '3' THEN 'Pregão Eletrônico'
		WHEN '4' THEN 'Pregão Presencial'
		ELSE OINV.U_TipoLicitacao 
		END,
		OINV.U_NumeroLicitacao
	FROM ORCT T0
		INNER JOIN RCT2 T1 ON T0.DocEntry = T1.DocNum
		LEFT  JOIN OBOE T2 ON T0.BoeAbs = T2.BoeKey
		LEFT  JOIN OBOT T3 ON T3.AbsEntry = (
			SELECT TOP 1 BOT1.AbsEntry FROM BOT1
			WHERE BOT1.BOENumber = T2.BoeNum AND BOT1.BoeType = T2.BoeType ORDER BY BOT1.AbsEntry DESC
		) AND T3.StatusTo = 'P'
		LEFT  JOIN OJDT T4 ON T3.TransId = T4.Number
		INNER JOIN OCRD T5 ON T0.CardCode = T5.CardCode AND T5.CardType = 'C'
		INNER JOIN OCRG ON OCRG.GroupCode = T5.GroupCode
		LEFT  JOIN CRD7 T6 ON T5.CardCode = T6.CardCode AND T6.[Address] = '' AND T6.AddrType = 'S'
		LEFT  JOIN INV6 ON T1.DocEntry = INV6.DocEntry AND T1.InstId = INV6.InstlmntID AND T1.InvType = INV6.ObjType
		LEFT  JOIN OINV ON INV6.DocEntry = OINV.DocEntry AND OINV.CANCELED = 'N'
		LEFT  JOIN INV1 ON OINV.DocEntry = INV1.DocEntry AND INV1.TargetType <> N'14'
		LEFT  JOIN DPI6 ON T1.DocEntry = DPI6.DocEntry AND T1.InstId = DPI6.InstlmntID AND T1.InvType = DPI6.ObjType
		LEFT  JOIN ODPI ON DPI6.DocEntry = ODPI.DocEntry AND ODPI.CANCELED = 'N'
		LEFT  JOIN DPI1 ON ODPI.DocEntry = DPI1.DocEntry AND DPI1.TargetType <> N'14'
		LEFT  JOIN RIN6 ON T1.DocEntry = RIN6.DocEntry AND T1.InstId = RIN6.InstlmntID AND T1.InvType = RIN6.ObjType
		LEFT  JOIN ORIN ON RIN6.DocEntry = ORIN.DocEntry AND ORIN.CANCELED = 'N'
		LEFT  JOIN RIN1 ON ORIN.DocEntry = RIN1.DocEntry AND RIN1.BaseType = N'-1'
		LEFT  JOIN JDT1 ON T1.DocEntry = JDT1.TransId AND T1.DocLine = JDT1.Line_ID AND T1.InvType = JDT1.TransType AND T1.InvType = N'30'
		OUTER APPLY (
				SELECT Countt = COUNT(TX.InvoiceId) FROM RCT2 TX WHERE TX.DocNum = T0.DocEntry
		) AS OA_MAX
		OUTER APPLY (
			SELECT Pagamento = 
				CASE WHEN T3.StatusTo = 'P' THEN T4.RefDate
					ELSE CASE WHEN T0.BoeAbs IS NULL AND T0.DocEntry IS NOT NULL THEN T0.TaxDate
					ELSE NULL END END --END
		) AS OA_DATA
		OUTER APPLY (
			SELECT Conta = OACT.AcctCode, NomeConta = OACT.AcctName
			FROM OACT
			WHERE OACT.Finanse = 'Y' AND OACT.AcctCode = 
				CASE WHEN T3.StatusTo = 'P' THEN (SELECT OPYM.GLAccount FROM OPYM WHERE OPYM.PayMethCod = T2.PayMethCod)
					ELSE CASE WHEN T0.BoeAbs IS NULL AND T0.DocEntry IS NOT NuLL THEN 
						CASE WHEN T0.TrsfrSum <> 0 THEN T0.TrsfrAcct ELSE T0.CashAcct END END END
		) AS OA_CONTA
		OUTER APPLY (
			SELECT Descript = 
				CASE WHEN ISNULL(T2.PayMethCod, T0.PayMth) IS NOT NULL THEN OPYM.Descript
					ELSE CASE WHEN T0.CashSum <> 0 THEN 'Dinheiro'
					ELSE CASE WHEN T0.TrsfrSum <> 0 THEN 'Transferência'
					ELSE CASE WHEN T0.BoeSum <> 0 THEN 'Boleto'
					ELSE CASE WHEN T0.[CheckSum] <> 0 THEN 'Cheque'
					ELSE NULL END END END END END
			FROM OPYM WHERE OPYM.PayMethCod = ISNULL(ISNULL(T2.PayMethCod, T0.PayMth), OPYM.PayMethCod)
		) AS OA_FORMA
	WHERE T0.Canceled = 'N' 
		--AND OA_CONTA.Conta IS NOT NULL
		AND
		(
			(@TipoData = 'E' AND ISNULL(ODPI.DocDate, ISNULL(JDT1.RefDate, ISNULL(ORIN.DocDate, OINV.DocDate))) BETWEEN @DataInicio AND @DataFim) OR
			(@TipoData = 'V' AND ISNULL(DPI6.DueDate, ISNULL(JDT1.DueDate, ISNULL(RIN6.DueDate, INV6.DueDate))) BETWEEN @DataInicio AND @DataFim) OR
			(@TipoData = 'P' AND OA_DATA.Pagamento BETWEEN @DataInicio AND @DataFim)
		)
		AND T0.CardCode = ISNULL(@CardCode, T0.CardCode)
		AND T5.GroupCode = ISNULL(@GrupoPN, T5.GroupCode)
		AND 
		(
			ISNULL(OINV.NumAtCard, '') = ISNULL(@NrRefPN, ISNULL(OINV.NumAtCard, '')) OR
			ISNULL(ORIN.NumAtCard, '') = ISNULL(@NrRefPN, ISNULL(ORIN.NumAtCard, '')) OR
			ISNULL(ODPI.NumAtCard, '') = ISNULL(@NrRefPN, ISNULL(ODPI.NumAtCard, ''))
		)
		AND 
		(
			ISNULL(INV6.U_CVA_Status, 0) = ISNULL(@StatusCobr, ISNULL(INV6.U_CVA_Status, 0)) OR
			ISNULL(DPI6.U_CVA_Status, 0) = ISNULL(@StatusCobr, ISNULL(DPI6.U_CVA_Status, 0)) OR
			ISNULL(RIN6.U_CVA_Status, 0) = ISNULL(@StatusCobr, ISNULL(RIN6.U_CVA_Status, 0))
		)
		AND 
		(
			ISNULL(OINV.Comments, '') LIKE '%' + @Obs + '%' OR
			ISNULL(ORIN.Comments, '') LIKE '%' + @Obs + '%' OR
			ISNULL(ODPI.Comments, '') LIKE '%' + @Obs + '%'
		)
		AND @StatusDoc = 'F'

	INSERT INTO #tmp_return
		SELECT DISTINCT
			TX.ChaveDoc,
			TX.ChaveDocString,
			TX.Numero,
			TX.Parcela,
			TX.ParcelaString,
			TX.CardCode,
			TX.CardName,
			TX.CardFName,
			TX.CardCodeName,
			TX.BPGroupCode,
			TX.BPGroupName,
			TX.CNPJ,
			TX.TipoObj,
			TX.TipoDoc,
			TX.TabelaDoc,
			TX.Emissao,
			TX.Vencimento,
			TX.ValorDoc,
			TX.DocCur,
			TX.DocRate,
			CASE WHEN ISNULL(TX.DocCur, 'R$') = 'R$' THEN 0 
				ELSE CASE WHEN ISNULL(TX.DocRate, 0) = 0 THEN CAST(TX.ValorDoc AS NUMERIC(19,6))
				ELSE CAST(TX.ValorDoc / TX.DocRate AS NUMERIC(19,6)) END END,
			TX.ChavePgto,
			TX.ChavePgtoString,
			TX.Pagamento,
			TX.TipoObjPgto,
			TX.TipoPgto,
			TX.TabelaPgto,
			TX.ValorPago,
			TX.JurosDesconto,
			TX.ValorSaldo,
			TX.PgtoCur,
			TX.PgtoRate,
			CASE WHEN ISNULL(TX.PgtoCur, 'R$') = 'R$' THEN 0 
				ELSE CASE WHEN ISNULL(TX.PgtoRate, 0) = 0 THEN CAST(TX.ValorPago AS NUMERIC(19,6))
				ELSE CAST(TX.ValorPago / TX.PgtoRate AS NUMERIC(19,6)) END END,
			TX.FormaPgto,
			TX.Comments,
			TX.AcctCode,
			TX.AcctName,
			TX.DocStatus,
			TX.SlpCode,
			TX.SlpName,
			TX.StatusCobr,
			TX.NrRefPN,
			TX.Endereco,
			TX.StatusPedido,
			TX.Autorizacao,
			TX.NumeroDocumento,
			TX.TipoLicitacao,
			TX.NrLicitacao
		FROM #tmp_docs TX

	--IF @Provisoes = 'Y'
	--BEGIN
	--	INSERT INTO #tmp_return
	--		SELECT DISTINCT * FROM [dbo].[fn_CVA_ProvisoesFluxoCaixa_CR](@DtLancamentoIni, @DtLancamentoFin, @DtVencimentoIni, @DtVencimentoFin)
	--		WHERE ((Emissao >= @DtLancamentoIni OR @DtLancamentoIni IS NULL) AND (Emissao <= @DtLancamentoFin OR @DtLancamentoFin IS NULL))
	--		OR ((Vencimento >= @DtVencimentoIni OR @DtVencimentoIni IS NULL) AND (Vencimento <= @DtVencimentoFin OR @DtVencimentoFin IS NULL))
	--END

	SELECT
		Numero + '-' + CAST(Parcela	AS NVARCHAR(10)) Duplicata,
		CAST(StatusCobr	AS NVARCHAR(50))	Status,
		ChaveDoc				[Chave NF],
		NrRefPN					[Nr. Ref.],
		StatusPedido            [Status Pedido],
		Autorizacao				[Tipo Autorização],
		NumeroDocumento			[Nr. Documento],
		TipoLicitacao			[Tipo Licitacao],
		NrLicitacao				[Nr. Licitacao],
		BPGroupName				[Grupo Cliente],
		CardCode				[Cód. Cliente],
		CardName				Cliente,
		DocStatus				Situação,
		Emissao,
		ISNULL(ValorDoc, 0)		Valor,
		Vencimento,
		ChavePgto				[Título],
		Pagamento,
		ValorPago + ISNULL(JurosDesconto, 0) AS ValorPago,
		ISNULL(ValorSaldo, 0)	ValorSaldo,
		FormaPgto				[Forma Pgto],
		Comments				Observações, 
		Endereco				[Endereço Entrega],
		CASE WHEN DocStatus = 'A' THEN
			CASE WHEN Vencimento >= GETDATE() THEN 0
			ELSE DATEDIFF(DAY, GETDATE(), Vencimento) END
		ELSE 0 END Atraso,
		BPGroupCode,
		Parcela,
		TipoObj
	FROM #tmp_return
	ORDER BY CAST(Numero AS INT), Parcela

	DROP TABLE #tmp_docs
	DROP TABLE #tmp_return
END

