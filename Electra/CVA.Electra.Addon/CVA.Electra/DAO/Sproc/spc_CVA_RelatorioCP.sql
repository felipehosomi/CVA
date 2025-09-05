IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spc_CVA_RelatorioCP')
	DROP PROCEDURE spc_CVA_RelatorioCP
GO
CREATE PROCEDURE [dbo].[spc_CVA_RelatorioCP]
(
	@TipoData	CHAR(1),
	@DataInicio DATE,
	@DataFim	DATE,
	@CardCode	NVARCHAR(100) = NULL,
	@GrupoPN	INT	= NULL,
	@NrRefPN	NVARCHAR(100) = NULL,
	@StatusCobr	INT = NULL,
	@Obs		NVARCHAR(MAX) = '',
	@StatusDoc  CHAR(1)
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
		NrRefPN NVARCHAR(200) Collate Database_Default,
		Endereco NVARCHAR(MAX) Collate Database_Default,
		StatusPedido NVARCHAR(50) Collate Database_Default
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
		NrRefPN NVARCHAR(200) Collate Database_Default,
		Endereco NVARCHAR(MAX) Collate Database_Default,
		StatusPedido NVARCHAR(50) Collate Database_Default
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
			WHEN N'18'  THEN 'NE'
			WHEN N'204' THEN 'AT'
			WHEN N'19'  THEN 'DE'
		END AS [TipoDoc]
		, CASE T0.TransType 
			WHEN N'30'  THEN 'OJDT'
			WHEN N'18'  THEN 'OPCH'
			WHEN N'204' THEN 'ODPO'
			WHEN N'19'  THEN 'ORPC'
		END AS [TabelaDoc]
		, T2.RefDate AS [Emissao]
		, T0.DueDate AS [Vencimento]
		, T0.Credit - T0.Debit AS [ValorParcela]
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
		, CASE WHEN OA_PGTO.DocEntry IS NOT NULL THEN 'CP' ELSE NULL END AS [TipoPgto]
		, CASE WHEN OA_PGTO.DocEntry IS NOT NULL THEN 'OVPM' ELSE NULL END AS [TabelaPgto]
		, OA_PGTO.SumApplied AS [ValorPago]
		, OA_PGTO.UndOvDiff AS [JurosDesconto]
		, T0.BalDueCred - T0.BalDueDeb AS [ValorSaldo]
		, OA_PGTO.DocCurr AS [PgtoCur]
		, OA_PGTO.DocRate AS [PgtoRate]
		, OA_PGTO.Descript AS [FormaPgto]
		, CASE WHEN T5.Comments IS NOT NULL THEN T5.Comments ELSE ISNULL(T0.U_Memo, '') + ' - ' + ISNULL(T0.Ref1, '') + ' - ' + ISNULL(T0.Ref2, '') END Comments
		, OA_PGTO.Conta AS [AcctCode]
		, OA_PGTO.NomeConta AS [AcctName]
		, CASE WHEN (T0.BalDueCred - T0.BalDueDeb) = 0 THEN 'F' ELSE 'A' END AS [DocStatus]
		, T5.NumAtCard
		, T5.U_CVA_END 
		,(SELECT TOP 1 CASE WHEN X1.DocStatus = 'O' THEN 'Aberto' ELSE 'Fechado' END FROM POR1 X0 WITH(NOLOCK) 
			INNER JOIN OPOR X1 ON X0.DocEntry  = X1.DocEntry 
		  WHERE X0.TrgetEntry = T5.DocEntry 
				AND X0.TargetType = T5.ObjType) AS [StatusPedido]
	FROM  JDT1 T0   
		INNER JOIN OCRD T1  ON  T0.ShortName = T1.CardCode AND T1.CardType = 'S'
		INNER JOIN OCRG ON OCRG.GroupCode = T1.GroupCode
		INNER JOIN OJDT T2  ON  T2.TransId = T0.TransId   
		INNER  JOIN CRD7 T3  ON  T1.CardCode = T3.CardCode AND T3.[Address] = '' AND T3.AddrType = 'S'
		LEFT JOIN PCH6 T4  ON  T0.SourceLine = T4.InstlmntID AND T0.SourceID = T4.DocEntry
		LEFT JOIN OPCH T5  ON  T4.DocEntry = T5.DocEntry
		OUTER APPLY 
		(
			SELECT DISTINCT 
				OVPM.DocEntry
				, OVPM.ObjType
				, VPM2.SumApplied + (SELECT [dbo].fn_CVA_VariacaoCambialComissao(OVPM.DocEntry, OVPM.ObjType)) AS SumApplied
				, OVPM.UndOvDiff / OA_MAX.Countt AS UndOvDiff
				, OVPM.DocCurr
				, OVPM.DocRate
				, OA_DATA.Pagamento
				, OA_FORMA.Descript
				, OA_CONTA.Conta
				, OA_CONTA.NomeConta
			FROM OVPM
				INNER JOIN VPM2 ON OVPM.DocEntry = VPM2.DocNum
				LEFT  JOIN OBOE ON OVPM.BoeAbs = OBOE.BoeKey
				LEFT  JOIN OBOT ON OBOT.AbsEntry = (
					SELECT TOP 1 BOT1.AbsEntry FROM BOT1
					WHERE BOT1.BOENumber = OBOE.BoeNum AND BOT1.BoeType = OBOE.BoeType
					ORDER BY BOT1.AbsEntry DESC
			) AND OBOT.StatusTo <> 'C'
			LEFT  JOIN OJDT ON OBOT.TransId = OJDT.Number
			OUTER APPLY 
			(
				SELECT Countt = COUNT(TX.InvoiceId) FROM VPM2 TX WHERE TX.DocNum = OVPM.DocEntry
			) AS OA_MAX
			OUTER APPLY 
			(
				SELECT Pagamento = 
					CASE WHEN OBOT.StatusTo <> 'C' THEN OJDT.RefDate
						ELSE CASE WHEN OVPM.BoeAbs IS NULL AND OVPM.DocEntry IS NOT NULL THEN OVPM.TaxDate
						ELSE NULL END END --END
			) AS OA_DATA
			OUTER APPLY 
			(
				SELECT Conta = OACT.AcctCode, NomeConta = OACT.AcctName
				FROM OACT
				WHERE OACT.Finanse = 'Y' AND OACT.AcctCode = 
					CASE WHEN OBOT.StatusTo <> 'C' THEN (SELECT OPYM.GLAccount FROM OPYM WHERE OPYM.PayMethCod = OBOE.PayMethCod)
						ELSE CASE WHEN OVPM.BoeAbs IS NULL AND OVPM.DocEntry IS NOT NuLL THEN 
							CASE WHEN OVPM.TrsfrSum <> 0 THEN OVPM.TrsfrAcct ELSE OVPM.CashAcct END END END
			) AS OA_CONTA
			OUTER APPLY 
			(
				SELECT Descript = 
					CASE WHEN ISNULL(OBOE.PayMethCod, OVPM.PayMth) IS NOT NULL THEN OPYM.Descript
						ELSE CASE WHEN OVPM.CashSum <> 0 THEN 'Dinheiro'
						ELSE CASE WHEN OVPM.TrsfrSum <> 0 THEN 'Transferência'
						ELSE CASE WHEN OVPM.BoeSum <> 0 THEN 'Boleto'
						ELSE CASE WHEN OVPM.[CheckSum] <> 0 THEN 'Cheque'
						ELSE NULL END END END END END
				FROM OPYM WHERE OPYM.PayMethCod = ISNULL(ISNULL(OBOE.PayMethCod, OVPM.PayMth), OPYM.PayMethCod)
			) AS OA_FORMA
			WHERE OVPM.Canceled = 'N' AND 
				OA_CONTA.Conta IS NOT NULL AND
				(
					(VPM2.DocEntry = T0.SourceID AND VPM2.InstId = T0.SourceLine AND VPM2.InvType = T0.TransType) OR
					(VPM2.DocEntry = T0.TransId AND VPM2.DocLine = T0.Line_ID AND VPM2.InvType = T0.TransType)
				)
	) AS OA_PGTO
	WHERE (T0.BalDueDeb - T0.BalDueCred) <> 0 
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
	AND ((T0.BalDueCred - T0.BalDueDeb = 0 AND @StatusDoc = 'F') OR @StatusDoc = 'A')

	UNION ALL

	SELECT DISTINCT
		CASE T1.InvType
			WHEN N'18' THEN OPCH.DocEntry
			WHEN N'19' THEN ORPC.DocEntry
			WHEN N'30' THEN JDT1.TransId
			WHEN N'204' THEN ODPO.DocEntry
		END AS [ChaveDoc]
		, CASE T1.InvType
			WHEN N'18' THEN CAST(OPCH.DocEntry AS NVARCHAR(10))
			WHEN N'19' THEN CAST(ORPC.DocEntry AS NVARCHAR(10))
			WHEN N'30' THEN CAST(JDT1.TransId AS NVARCHAR(10))
			WHEN N'204' THEN CAST(ODPO.DocEntry AS NVARCHAR(10))
		END AS [ChaveDocString]
		, CASE T1.InvType
			WHEN N'18' THEN ISNULL(OPCH.Serial, OPCH.DocEntry)
			WHEN N'19' THEN ISNULL(ORPC.Serial, ORPC.DocEntry)
			WHEN N'30' THEN JDT1.TransId
			WHEN N'204' THEN ISNULL(ODPO.Serial, ODPO.DocEntry)
		END AS [Numero]
		, CASE T1.InvType
			WHEN N'18' THEN PCH6.InstlmntID
			WHEN N'19' THEN RPC6.InstlmntID
			WHEN N'30' THEN JDT1.Line_ID
			WHEN N'204' THEN DPO6.InstlmntID
		END AS [Parcela]
		, CASE T1.InvType
			WHEN N'18' THEN CAST(PCH6.InstlmntID AS NVARCHAR(10)) + '/' + CAST(OPCH.Installmnt AS NVARCHAR(10))
			WHEN N'19' THEN CAST(RPC6.InstlmntID AS NVARCHAR(10)) + '/' + CAST(ORPC.Installmnt AS NVARCHAR(10))
			WHEN N'30' THEN '1/1'
			WHEN N'204' THEN CAST(DPO6.InstlmntID AS NVARCHAR(10)) + '/' + CAST(ODPO.Installmnt AS NVARCHAR(10))
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
			WHEN N'18' THEN 'NS'
			WHEN N'19' THEN 'DS'
			WHEN N'30' THEN 'LC'
			WHEN N'204' THEN 'AT'
		END AS [TipoDoc]
		, CASE T1.InvType
			WHEN N'18' THEN 'OINV'
			WHEN N'19' THEN 'ORIN'
			WHEN N'30' THEN 'OJDT'
			WHEN N'204' THEN 'ODPI'
		END AS [TabelaDoc]
		, CASE T1.InvType
			WHEN N'18' THEN OPCH.DocDate
			WHEN N'19' THEN ORPC.DocDate
			WHEN N'30' THEN JDT1.RefDate
			WHEN N'204' THEN ODPO.DocDate
		END AS [Emissao]
		, CASE T1.InvType
			WHEN N'18' THEN PCH6.DueDate
			WHEN N'19' THEN RPC6.DueDate
			WHEN N'30' THEN JDT1.DueDate
			WHEN N'204' THEN DPO6.DueDate
		END AS [Vencimento]
		, CASE T1.InvType
			WHEN N'18' THEN PCH6.InsTotal
			WHEN N'18' THEN RPC6.InsTotal
			WHEN N'30' THEN JDT1.Debit - JDT1.Credit
			WHEN N'204' THEN DPO6.InsTotal
		END AS [ValorParcela]
		, CASE T1.InvType
			WHEN N'18' THEN ISNULL(OPCH.DocCur, 'R$')
			WHEN N'19' THEN ISNULL(ORPC.DocCur, 'R$')
			WHEN N'30' THEN ISNULL(JDT1.FCCurrency, 'R$')
			WHEN N'204' THEN ISNULL(ODPO.DocCur, 'R$')
		END AS [DocCur]
		, CASE T1.InvType
			WHEN N'18' THEN OPCH.DocRate
			WHEN N'19' THEN ORPC.DocRate
			WHEN N'30' THEN CAST (
				CASE WHEN JDT1.FCCurrency IS NOT NULL THEN
					CASE WHEN JDT1.FCDebit <> 0 THEN JDT1.Debit / JDT1.FCDebit
					ELSE
						CASE WHEN JDT1.FCCredit <> 0 THEN JDT1.Credit / JDT1.FCCredit
						ELSE 0 END
					END
				ELSE 1 END
			AS NUMERIC(19,6))
			WHEN N'204' THEN ODPO.DocRate
		END AS [DocRate]
		, T0.DocEntry AS [ChavePgto]
		, CAST(T0.DocEntry AS NVARCHAR(10)) AS [ChavePgtoString]
		, OA_DATA.Pagamento AS [Pagamento]
		, T0.ObjType AS [TipoObjPgto]
		, 'CP' AS [TipoPgto]
		, 'OVPM' AS [TabelaPgto]
		, T1.SumApplied + (SELECT [dbo].[fn_CVA_VariacaoCambialComissao](T0.DocEntry, T0.ObjType)) AS [ValorPago]
		, T0.UndOvDiff / OA_MAX.Countt AS [JurosDesconto]
		, CASE T1.InvType
			WHEN N'18' THEN PCH6.InsTotal - PCH6.PaidToDate
			WHEN N'19' THEN RPC6.InsTotal - RPC6.PaidToDate
			WHEN N'30' THEN JDT1.BalDueDeb - JDT1.BalDueCred
			WHEN N'204' THEN DPO6.InsTotal - DPO6.PaidToDate
		END AS [ValorSaldo]
		, T0.DocCurr AS [PgtoCur]
		, T0.DocRate AS [PgtoRate]
		, OA_FORMA.Descript AS [FormaPgto]
		, CASE T1.InvType
			WHEN N'18' THEN CAST(OPCH.Comments AS NVARCHAR(MAX))
			WHEN N'19' THEN CAST(ORPC.Comments AS NVARCHAR(MAX))
			WHEN N'30' THEN CAST(JDT1.LineMemo AS NVARCHAR(MAX))
			WHEN N'204' THEN CAST(ODPO.Comments AS NVARCHAR(MAX))
		END AS [Comments]
		, OA_CONTA.Conta AS [AcctCode]
		, OA_CONTA.NomeConta AS [AcctName]
		, 'F' AS [DocStatus]
		, CASE T1.InvType
			WHEN N'13' THEN OPCH.NumAtCard
			WHEN N'14' THEN ORPC.NumAtCard
			WHEN N'30' THEN ''
			WHEN N'203' THEN ODPO.NumAtCard
		END AS NrRefPN
		, CASE T1.InvType
			WHEN N'13' THEN OPCH.U_CVA_END
			WHEN N'14' THEN ORPC.U_CVA_END
			WHEN N'30' THEN ''
			WHEN N'203' THEN ODPO.U_CVA_END
		END AS NrRefPN,
		(SELECT TOP 1 CASE WHEN X1.DocStatus = 'O' THEN 'Aberto' ELSE 'Fechado' END FROM POR1 X0 WITH(NOLOCK) 
			INNER JOIN OPOR X1 ON X0.DocEntry  = X1.DocEntry 
		  WHERE X0.TrgetEntry = OPCH.DocEntry 
				 AND X0.TargetType = OPCH.ObjType) AS [StatusPedido]
	FROM OVPM T0
		INNER JOIN VPM2 T1 ON T0.DocEntry = T1.DocNum
		LEFT  JOIN OBOE T2 ON T0.BoeAbs = T2.BoeKey
		LEFT  JOIN OBOT T3 ON T3.AbsEntry = (
			SELECT TOP 1 BOT1.AbsEntry FROM BOT1
			WHERE BOT1.BOENumber = T2.BoeNum AND BOT1.BoeType = T2.BoeType ORDER BY BOT1.AbsEntry DESC
		) AND T3.StatusTo = 'P'
		LEFT  JOIN OJDT T4 ON T3.TransId = T4.Number
		INNER JOIN OCRD T5 ON T0.CardCode = T5.CardCode AND T5.CardType = 'S'
		INNER JOIN OCRG ON OCRG.GroupCode = T5.GroupCode
		LEFT  JOIN CRD7 T6 ON T5.CardCode = T6.CardCode AND T6.[Address] = '' AND T6.AddrType = 'S'
		LEFT  JOIN PCH6 ON T1.DocEntry = PCH6.DocEntry AND T1.InstId = PCH6.InstlmntID AND T1.InvType = PCH6.ObjType
		LEFT  JOIN OPCH ON PCH6.DocEntry = OPCH.DocEntry AND OPCH.CANCELED = 'N'
		LEFT  JOIN PCH1 ON OPCH.DocEntry = PCH1.DocEntry AND PCH1.TargetType <> N'19'
		LEFT  JOIN DPO6 ON T1.DocEntry = DPO6.DocEntry AND T1.InstId = DPO6.InstlmntID AND T1.InvType = DPO6.ObjType
		LEFT  JOIN ODPO ON DPO6.DocEntry = ODPO.DocEntry AND ODPO.CANCELED = 'N'
		LEFT  JOIN DPO1 ON ODPO.DocEntry = DPO1.DocEntry AND DPO1.TargetType <> N'19'
		LEFT  JOIN RPC6 ON T1.DocEntry = RPC6.DocEntry AND T1.InstId = RPC6.InstlmntID AND T1.InvType = RPC6.ObjType
		LEFT  JOIN ORPC ON RPC6.DocEntry = ORPC.DocEntry AND ORPC.CANCELED = 'N'
		LEFT  JOIN RPC1 ON ORPC.DocEntry = RPC1.DocEntry AND RPC1.BaseType = N'-1'
		LEFT  JOIN JDT1 ON T1.DocEntry = JDT1.TransId AND T1.DocLine = JDT1.Line_ID AND T1.InvType = JDT1.TransType AND T1.InvType = N'30'
		LEFT  JOIN VPM1 T7 ON T0.DocEntry = T7.DocNum
		LEFT  JOIN OCHO T8 ON T7.CheckAbs = T8.CheckKey AND T8.Canceled = 'N'
		OUTER APPLY 
		(
			SELECT Countt = COUNT(TX.InvoiceId) FROM VPM2 TX WHERE TX.DocNum = T0.DocEntry
		) AS OA_MAX
		OUTER APPLY 
		(
			SELECT Pagamento = 
				CASE WHEN T3.StatusTo = 'P' THEN T4.RefDate
					ELSE CASE WHEN T8.CheckKey IS NOT NULL THEN T8.CheckDate
					ELSE CASE WHEN T0.BoeAbs IS NULL AND T8.CheckKey IS NULL AND T0.DocEntry IS NOT NULL THEN T0.TaxDate
					ELSE NULL END END END
		) AS OA_DATA
		OUTER APPLY 
		(
			SELECT Conta = OACT.AcctCode, NomeConta = OACT.AcctName
			FROM OACT
			WHERE OACT.Finanse = 'Y' AND OACT.AcctCode = 
				CASE WHEN T3.StatusTo = 'P' THEN (SELECT OPYM.GLAccount FROM OPYM WHERE OPYM.PayMethCod = T2.PayMethCod)
					ELSE CASE WHEN T8.CheckKey IS NOT NULL THEN T8.CheckAcct
					ELSE CASE WHEN T0.BoeAbs IS NULL AND T8.CheckKey IS NULL AND T0.DocEntry IS NOT NuLL THEN 
						CASE WHEN T0.TrsfrSum <> 0 THEN T0.TrsfrAcct ELSE T0.CashAcct END END END END
		) AS OA_CONTA
		OUTER APPLY 
		(
			SELECT Descript = 
				CASE WHEN ISNULL(T2.PayMethCod, T0.PayMth) IS NOT NULL THEN OPYM.Descript
					ELSE CASE WHEN T0.CashSum		<> 0 THEN 'Dinheiro'
					ELSE CASE WHEN T0.TrsfrSum		<> 0 THEN 'Transferência'
					ELSE CASE WHEN T0.BoeSum		<> 0 THEN 'Boleto'
					ELSE CASE WHEN T0.[CheckSum]	<> 0 THEN 'Cheque'
					ELSE NULL END END END END END
			FROM OPYM WHERE OPYM.PayMethCod = ISNULL(ISNULL(T2.PayMethCod, T0.PayMth), OPYM.PayMethCod)
		) AS OA_FORMA
	WHERE T0.Canceled = 'N'
	AND OA_CONTA.Conta IS NOT NULL
	AND
	(
		(@TipoData = 'E' AND ISNULL(ODPO.DocDate, ISNULL(JDT1.RefDate, ISNULL(ORPC.DocDate, OPCH.DocDate))) BETWEEN @DataInicio AND @DataFim) OR
		(@TipoData = 'V' AND ISNULL(DPO6.DueDate, ISNULL(JDT1.DueDate, ISNULL(RPC6.DueDate, PCH6.DueDate))) BETWEEN @DataInicio AND @DataFim) OR
		(@TipoData = 'P' AND OA_DATA.Pagamento BETWEEN @DataInicio AND @DataFim)
	)
	AND T0.CardCode = ISNULL(@CardCode, T0.CardCode)
	AND T5.GroupCode = ISNULL(@GrupoPN, T5.GroupCode)
	AND 
	(
		ISNULL(OPCH.NumAtCard, '') = ISNULL(@NrRefPN, ISNULL(OPCH.NumAtCard, '')) OR
		ISNULL(ORPC.NumAtCard, '') = ISNULL(@NrRefPN, ISNULL(ORPC.NumAtCard, '')) OR
		ISNULL(ODPO.NumAtCard, '') = ISNULL(@NrRefPN, ISNULL(ODPO.NumAtCard, ''))
	)
	AND 
	(
		ISNULL(OPCH.Comments, '') LIKE '%' + @Obs + '%' OR
		ISNULL(ORPC.Comments, '') LIKE '%' + @Obs + '%' OR
		ISNULL(ODPO.Comments, '') LIKE '%' + @Obs + '%'
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
			TX.NrRefPN,
			TX.Endereco,
			TX.StatusPedido
		FROM #tmp_docs TX

	--IF @Provisoes = 'Y'
	--BEGIN
	--	INSERT INTO #tmp_return
	--		SELECT DISTINCT * FROM [dbo].fn_CVA_ProvisoesFluxoCaixa (@DtLancamentoIni, @DtLancamentoFin, @DtVencimentoIni, @DtVencimentoFin)
	--		WHERE ((Emissao >= @DtLancamentoIni OR @DtLancamentoIni IS NULL) AND (Emissao <= @DtLancamentoFin OR @DtLancamentoFin IS NULL))
	--		OR ((Vencimento >= @DtVencimentoIni OR @DtVencimentoIni IS NULL) AND (Vencimento <= @DtVencimentoFin OR @DtVencimentoFin IS NULL))
	--END

	SELECT
		Numero + '-' + CAST(Parcela	AS NVARCHAR(10)) Duplicata,
		ChaveDoc				[Chave NF],
		NrRefPN					[Nr. Ref.],
		StatusPedido            [Status Pedido],
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

