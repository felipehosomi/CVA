CREATE PROCEDURE [dbo].[spc_CVA_Rebate]  
(
	@DtLancamentoIni DATE = NULL,
	@DtLancamentoFin DATE = NULL,
	@DtVencimentoIni DATE = NULL,
	@DtVencimentoFin DATE = NULL
)
AS
BEGIN
--declare	@DtLancamentoIni DATE = NULL,
--	@DtLancamentoFin DATE = NULL,
--	@DtVencimentoIni DATE = NULL,
--	@DtVencimentoFin DATE = NULL


--set	@DtLancamentoIni = '2018/01/01'
--set	@DtLancamentoFin = '2018/09/30'
--set	@DtVencimentoIni = '2018/01/01'
--set	@DtVencimentoFin = '2018/09/30'

set dateformat 'ymd';

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
	JurosBank NUMERIC(19,6)
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
	JurosBank NUMERIC(19,6)
)
END

INSERT INTO #tmp_docs
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
	END AS [SlpName],
	X0.SysTotal as JurosBank
FROM ORCT T0
	INNER JOIN RCT2 T1 ON T0.DocEntry = T1.DocNum
	LEFT  JOIN OBOE T2 ON T0.BoeAbs = T2.BoeKey
	LEFT  JOIN OBOT T3 ON T3.AbsEntry = (
		SELECT TOP 1 BOT1.AbsEntry FROM BOT1
		WHERE BOT1.BOENumber = T2.BoeNum AND BOT1.BoeType = T2.BoeType ORDER BY BOT1.AbsEntry DESC
	) AND T3.StatusTo = 'P'
	LEFT  JOIN OJDT T4 ON T3.TransId = T4.Number
	INNER JOIN OCRD T5 ON T0.CardCode = T5.CardCode AND T5.CardType = 'C'
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
		WHERE -- OACT.Finanse = 'Y' AND 
		OACT.AcctCode = 
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
LEFT OUTER JOIN OJDT X0 ON X0.U_SIEBS_BoeKey = T2.BoeKey AND X0.U_SIEBS_TipoLcmDif = 2
WHERE T5.u_ds_rebate = 'S' And T0.Canceled = 'N' AND 
	OA_CONTA.Conta IS NOT NULL AND
	(ISNULL(ODPI.DocDate, ISNULL(JDT1.RefDate, ISNULL(ORIN.DocDate, OINV.DocDate))) >= @DtLancamentoIni AND ISNULL(ODPI.DocDate, ISNULL(JDT1.RefDate, ISNULL(ORIN.DocDate, OINV.DocDate))) <= @DtLancamentoFin) AND
	(ISNULL(DPI6.DueDate, ISNULL(JDT1.DueDate, ISNULL(RIN6.DueDate, INV6.DueDate))) >= @DtVencimentoIni AND ISNULL(DPI6.DueDate, ISNULL(JDT1.DueDate, ISNULL(RIN6.DueDate, INV6.DueDate))) <= @DtVencimentoFin)

UNION ALL
SELECT DISTINCT
	T0.DocEntry AS [ChaveDoc],
	CAST(T0.DocEntry AS NVARCHAR(10)) AS [ChaveDocString],
	CAST(T0.DocEntry AS NVARCHAR(50)) AS [Numero],
	T8.LineId AS [Parcela],
	'1/1' AS [ParcelaString],
	ISNULL(T2.AcctCode, T1.CardCode) AS [CardCode],
	ISNULL(T2.AcctName, T1.CardName) AS [CardName],
	ISNULL(T2.AcctName, ISNULL(T1.CardFName, T1.CardName)) AS [CardFName],
	ISNULL(T2.AcctCode, T1.CardCode) + ' - ' + ISNULL(T2.AcctName, T1.CardName) AS [CardCodeName],
	CASE WHEN ISNULL(T9.TaxId4, '') <> '' THEN T9.TaxId4 ELSE T9.TaxId0 END AS [CNPJ],
	T0.ObjType AS [TipoObj],
	'CR' AS [TipoDoc],
	'ORCT' AS [TabelaDoc],
	T0.DocDate AS [Emissao],
	T0.DocDueDate as [Vencimento],
	ISNULL(T8.SumApplied, T0.DocTotal) AS [ValorParcela],
	ISNULL(T0.DocCurr, 'R$') AS [DocCur],
	ISNULL(T0.DocRate, 1) AS [DocRate],
	T0.DocEntry AS [ChavePgto],
	CAST(T0.DocEntry AS NVARCHAR(10)) AS [ChavePgtoString],
	OA_DATA.Pagamento AS [Pagamento],
	T0.ObjType AS [TipoObjPgto],
	'CR' AS [TipoPgto],
	'ORCT' AS [TabelaPgto],
	ISNULL(T8.SumApplied, T0.DocTotal) + T0.UndOvDiff AS [ValorPago],
	T0.UndOvDiff AS [JurosDesconto],
	0 AS [ValorSaldo],
	ISNULL(T0.DocCurr, 'R$') AS [PgtoCur],
	ISNULL(T0.DocRate, 1) AS [PgtoRate],
	OA_FORMA.Descript AS [FormaPgto],
	T0.Comments AS [Comments],
	OA_CONTA.Conta AS [AcctCode],
	OA_CONTA.NomeConta AS [AcctName],
	'F' AS [DocStatus],
	T10.SlpCode,
	T10.SlpName,
	X0.SysTotal as JurosBank
FROM ORCT T0
	LEFT JOIN OCRD T1 ON T0.CardCode = T1.CardCode AND T1.CardType = 'C'
	LEFT JOIN OACT T2 ON T0.CardCode = T2.AcctCode
	LEFT JOIN OBOE T3 ON T0.BoeAbs = T3.BoeKey
	LEFT JOIN OBOT T4 ON T4.AbsEntry =
	(
		SELECT TOP 1 BOT1.AbsEntry FROM BOT1
		WHERE BOT1.BOENumber = T3.BoeNum AND BOT1.BoeType = T3.BoeType
		ORDER BY BOT1.AbsEntry DESC
	) AND T4.StatusTo = 'P'
	LEFT JOIN OJDT T5 ON T4.TransId = T5.Number
	LEFT JOIN RCT1 T6 ON T0.DocEntry = T6.DocNum
	LEFT JOIN RCT4 T8 ON T0.DocEntry = T8.DocNum
	LEFT JOIN CRD7 T9 ON T1.CardCode = T9.CardCode ANd T9.[Address] = '' ANd T9.AddrType = 'S'
	LEFT  JOIN OSLP T10 ON T1.SlpCode = T10.SlpCode
OUTER APPLY (
	SELECT Pagamento = 
		CASE WHEN T4.StatusTo = 'P' THEN T5.RefDate
			ELSE CASE WHEN T0.BoeAbs IS NULL AND T0.DocEntry IS NOT NULL THEN T0.TaxDate
			ELSE NULL END END --END
) AS OA_DATA
OUTER APPLY (
	SELECT Conta = OACT.AcctCode, NomeConta = OACT.AcctName
	FROM OACT
	WHERE OACT.Finanse = 'Y' AND OACT.AcctCode = 
		CASE WHEN T4.StatusTo = 'P' THEN (SELECT OPYM.GLAccount FROM OPYM WHERE OPYM.PayMethCod = T3.PayMethCod)
			ELSE CASE WHEN T0.BoeAbs IS NULL AND T0.DocEntry IS NOT NULL THEN 
				CASE WHEN T0.TrsfrSum <> 0 THEN T0.TrsfrAcct
				ELSE T0.CashAcct END
			END
		END
) AS OA_CONTA
OUTER APPLY (
	SELECT Descript = 
		CASE WHEN ISNULL(T3.PayMethCod, T0.PayMth) IS NOT NULL THEN OPYM.Descript
			ELSE CASE WHEN T0.CashSum <> 0 THEN 'Dinheiro'
			ELSE CASE WHEN T0.TrsfrSum <> 0 THEN 'Transferência'
			ELSE CASE WHEN T0.BoeSum <> 0 THEN 'Boleto'
			ELSE CASE WHEN T0.[CheckSum] <> 0 THEN 'Cheque'
			ELSE NULL END END END END
		END
	FROM OPYM WHERE OPYM.PayMethCod = ISNULL(ISNULL(T3.PayMethCod, T0.PayMth), OPYM.PayMethCod)
) AS OA_FORMA
LEFT OUTER JOIN OJDT X0 ON X0.U_SIEBS_BoeKey = T3.BoeKey AND X0.U_SIEBS_TipoLcmDif = 2
WHERE T1.u_ds_rebate = 'S' And T0.NoDocSum <> 0 AND T0.Canceled = 'N' ANd OA_CONTA.Conta IS NOT NULL
	AND (
		SELECT COUNT(OACT.AcctCode)
		FROM JDT1 INNER JOIN OACT ON JDt1.Account = OACT.AcctCode AND OACT.Finanse = 'Y'
		WHERE JDT1.TransId = T0.TransId AND JDt1.TransType = T0.ObjType
	) <> 2 AND
	(T0.DocDate >= @DtLancamentoIni AND T0.DocDate <= @DtLancamentoFin) AND
	(T0.DocDueDate >= @DtVencimentoIni AND T0.DocDueDate <= @DtVencimentoFin)


--UNION ALL

--SELECT DISTINCT
--	T0.DocEntry AS [ChaveDoc],
--	CAST(T0.DocEntry AS NVARCHAR(10)) AS [ChaveDocString],
--	CAST(T0.DocEntry AS NVARCHAR(50)) AS [Numero],
--	T6.LineId AS [Parcela],
--	'1/1' AS [ParcelaString],
--	T2.AcctCode AS [CardCode],
--	T2.AcctName AS [CardName],
--	T2.AcctName AS [CardFName],
--	T2.AcctCode + ' ' + T2.AcctName AS [CardCodeName],
--	'' [CNPJ],
--	T0.ObjType AS [TipoObj],
--	'CR' AS [TipoDoc],
--	'ORCT' AS [TabelaDoc],
--	T0.DocDate AS [Emissao],
--	T0.DocDueDate as [Vencimento],
--	ISNULL(T6.SumApplied, T0.DocTotal) AS [ValorParcela],
--	ISNULL(T0.DocCurr, 'R$') AS [DocCur],
--	ISNULL(T0.DocRate, 1) AS [DocRate],
--	T0.DocEntry AS [ChavePgto],
--	CAST(T0.DocEntry AS NVARCHAR(10)) AS [ChavePgtoString],
--	T0.DocDate AS [Pagamento],
--	T0.ObjType AS [TipoObjPgto],
--	'CR' AS [TipoPgto],
--	'ORCT' AS [TabelaPgto],
--	ISNULL(T6.SumApplied, T0.DocTotal) + T0.UndOvDiff AS [ValorPago],
--	T0.UndOvDiff AS [JurosDesconto],
--	0 AS [ValorSaldo],
--	ISNULL(T0.DocCurr, 'R$') AS [PgtoCur],
--	ISNULL(T0.DocRate, 1) AS [PgtoRate],
--	'Transferência' AS [FormaPgto],
--	T0.Comments AS [Comments],
--	T2.AcctCode AS [AcctCode],
--	T2.AcctName AS [AcctName],
--	'F' AS [DocStatus],
--	0,
--	'',
--	0
--FROM OVPM T0
--	LEFT JOIN OACT T2 ON T0.CardCode = T2.AcctCode
--	LEFT JOIN OJDT T5 ON T5.TransId = T0.TransId
--	LEFT JOIN VPM4 T6 ON T0.DocEntry = T6.DocNum
--	LEFT JOIN JDT1 T7 ON T7.TransId = T5.TransId AND T7.Account = T6.AcctCode AND T7.Debit > 0
--WHERE T0.NoDocSum <> 0 AND T0.Canceled = 'N'
--	AND (T0.DocDate >= @DtLancamentoIni AND T0.DocDate <= @DtLancamentoFin) AND
--	(T0.DocDueDate >= @DtVencimentoIni AND T0.DocDueDate <= @DtVencimentoFin)
--	AND T0.DocNum IN
--	(10964, 10987, 11094, 11146, 11193, 11218, 11268, 11434, 11596, 11630, 11699, 11782, 11788, 11837, 11997, 12054, 12082, 12123, 12159, 12163, 12251, 12322, 12386, 12441, 12492, 12577, 12585)

---Dev. Notas Fiscais
UNION ALL
Select DISTINCT  T0.DocEntry AS [ChaveDoc]
		,CAST(T0.DocEntry AS NVARCHAR(10)) AS [ChaveDocString]
		,ISNULL(T0.Serial, T0.DocEntry)  AS [Numero]
		,T1.InstlmntID AS [Parcela]
		,CAST(T1.InstlmntID AS NVARCHAR(10)) + '/' + CAST(T0.Installmnt AS NVARCHAR(10)) AS [ParcelaString]
		,T2.CardCode AS [CardCode]
		,T2.CardName AS [CardName]
		,T2.CardFName AS [CardFName]
		,T2.CardCode + ' - ' + T2.CardName AS [CardCodeName]
		,CASE WHEN ISNULL(T4.TaxId4, '') <> '' THEN T4.TaxId4 ELSE T4.TaxId0 END AS [CNPJ]
		,T0.ObjType AS [TipoObj]
		,'DV' AS [TipoDoc]
		,'ORIN' AS [TabelaDoc]
		,T0.DocDate AS [Emissao]
		,T1.DueDate AS [Vencimento]
		,(T1.InsTotal * -1) AS [ValorParcela]
		,ISNULL(T0.DocCur, 'R$') AS [DocCur]
		,T0.DocRate AS [DocRate]
		,T0.DocEntry AS [ChavePgto]
		,CAST(T0.DocEntry AS NVARCHAR(10)) AS [ChavePgtoString]
		,T0.DocDate AS [Pagamento]
		,T0.ObjType AS [TipoObjPgto]
		,'DV' AS [TipoPgto]
		,'ORIN' AS [TabelaPgto]
		,0 AS [ValorPago]
		--,(T0.DocTotal * -1) AS [ValorPago]
		,T0.DiscSum AS [JurosDesconto]
		,0 AS [ValorSaldo]
		--,(T1.InsTotal - T1.PaidToDate) * -1 AS [ValorSaldo]
		,T0.DocCur AS [PgtoCur]
		,T0.DocRate AS [PgtoRate]
		,T0.PeyMethod AS [FormaPgto]
		,CAST(T0.Comments AS NVARCHAR(MAX))  AS [Comments]
		,T3.AcctCode AS [AcctCode]
		,T3.Acctname AS [AcctName]
		,'F' AS [DocStatus]
		, T0.SlpCode AS [SlpCode]
		,(SELECT SlpName FROM OSLP WHERE SlpCode = T0.SlpCode) [SlpName]
		,0 as JurosBank
From ORIN T0
INNER JOIN RIN6 T1 On T0.DocEntry = T1.DocEntry
INNER JOIN OCRD T2 On T0.CardCode = T2.CardCode
INNER JOIN OACT T3 ON T0.CtlAccount = T3.AcctCode
LEFT  JOIN CRD7 T4 ON T2.CardCode = T4.CardCode AND T4.[Address] = '' AND T4.AddrType = 'S'
Where T2.u_ds_rebate = 'S' And T0.CANCELED = 'N' 
AND (T0.DocDate >= @DtLancamentoIni AND T0.DocDate <= @DtLancamentoFin) AND
(T0.DocDueDate >= @DtVencimentoIni AND T0.DocDueDate <= @DtVencimentoFin)
ORDER BY 6, 11, 1, 4

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
		TX.JurosBank 
	FROM #tmp_docs TX

;WITH Retorno AS
(
	SELECT 
		ChaveDoc,
		ChaveDocString,
		Numero,
		Parcela,
		ParcelaString,
		CardCode,
		CardName,
		CardFName,
		CardCodeName,
		CNPJ,
		TipoObj,
		TipoDoc,
		TabelaDoc,
		Emissao,
		Vencimento,
		ISNULL(ValorDoc, 0) AS ValorDoc,
		ISNULL(DocCur, 'R$') AS DocCur,
		ISNULL(DocRate, 0) AS DocRate,
		ISNULL(ValorDocME, 0) AS ValorDocME,
		MAX(ChavePgto) ChavePgto,
		MAX(ChavePgtoString) ChavePgtoString,
		MAX(Pagamento) Pagamento,
		TipoObjPgto,
		TipoPgto,
		TabelaPgto,
		ValorPago as ValorPago,
		ISNULL(JurosDesconto, 0) AS JurosDesconto,
		ValorSaldo	SaldoAtual,
		ISNULL(ValorDoc, 0) - SUM(ValorPago) AS ValorSaldo,
		PgtoCur,
		PgtoRate,
		ValorPagoME,
		FormaPgto,
		MAX(Comments) Comments,
		AcctCode,
		AcctName,
		MAX(DocStatus) DocStatus,
		DiffDias = 
			CASE WHEN MAX(DocStatus) = 'A' THEN
				CASE WHEN Vencimento >= GETDATE() THEN 0
				ELSE DATEDIFF(DAY, GETDATE(), Vencimento) END
			ELSE 0 END,
		SlpCode,
		SlpName,
		JurosBank
	FROM #tmp_return 
	WHERE  (Emissao >= @DtLancamentoIni OR @DtLancamentoIni IS NULL)
		AND (Emissao <= @DtLancamentoFin OR @DtLancamentoFin IS NULL)
		AND (Vencimento >= @DtVencimentoIni OR @DtVencimentoIni IS NULL)
		AND (Vencimento <= @DtVencimentoFin OR @DtVencimentoFin IS NULL)
		--AND CardCode = 'C000512'
	GROUP BY
		ChaveDoc,
		ChaveDocString,
		ValorPago,
		Numero,
		Parcela,
		ParcelaString,
		CardCode,
		CardName,
		CardFName,
		CardCodeName,
		CNPJ,
		TipoObj,
		TipoDoc,
		TabelaDoc,
		Emissao,
		Vencimento,
		ValorDoc,
		ChaveDocString,
		Numero,
		Parcela,
		ParcelaString,
		CardCode,
		CardName,
		CardFName,
		CardCodeName,
		CNPJ,
		TipoObj,
		TipoDoc,
		TabelaDoc,
		Emissao,
		ValorDoc,
		DocCur,
		DocRate,
		ValorDocME,
		TipoObjPgto,
		TipoPgto,
		TabelaPgto,
		JurosDesconto,
		ValorSaldo,	
		PgtoCur,
		PgtoRate,
		ValorPagoME,
		FormaPgto,
		AcctCode,
		AcctName,
		Vencimento,
		SlpCode,
		SlpName,
		JurosBank
)
select distinct 
	ChaveDoc,
	ChaveDocString,
	Numero,
	Parcela,
	ParcelaString,
	CardCode,
	CardName,
	CardFName,
	CardCodeName,
	CNPJ,
	TipoObj,
	TipoDoc,
	TabelaDoc,
	Emissao,
	Vencimento,
	ValorDoc,
	DocCur,
	DocRate,
	ValorDocME,
	ChavePgto,
	ChavePgtoString,
	Pagamento,
	TipoObjPgto,
	TipoPgto,
	TabelaPgto,
	case when TipoDoc = 'DV' then 0 else case when ValorSaldo = 0 and ValorPago = 0 then ValorDoc  else ValorPago end  + ISNULL(JurosBank,0) end as ValorPago,
	JurosDesconto,
	--ValorDoc - SUM(ValorPago) OVER (PARTITION BY  ChaveDoc, Parcela
	--									ORDER BY ChaveDoc, Parcela
	--								  ROWS UNBOUNDED PRECEDING) ValorSaldo,
	ValorSaldo,
	PgtoCur,
	PgtoRate,
	ValorPagoME,
	FormaPgto,
	Comments,
	AcctCode,
	AcctName,
	DocStatus,
	DiffDias = case when DocStatus = 'C' then 0 else case when DocStatus = 'F' then datediff(day, isnull(Pagamento, Vencimento), Vencimento) else datediff(day, getdate(), Vencimento) end end,
	SlpCode,
	SlpName
FROM #tmp_return
--WHERE [ChaveDoc] = 4544
ORDER BY Numero

DROP TABLE #tmp_docs
DROP TABLE #tmp_return


END