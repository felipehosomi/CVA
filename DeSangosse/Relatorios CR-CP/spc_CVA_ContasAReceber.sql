IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spc_CVA_ContasAReceber')
	DROP PROCEDURE spc_CVA_ContasAReceber
GO
CREATE PROCEDURE [dbo].[spc_CVA_ContasAReceber]
(
	@DtLancamentoIni DATE = NULL,
	@DtLancamentoFin DATE = NULL,
	@DtVencimentoIni DATE = NULL,
	@DtVencimentoFin DATE = NULL
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
	SlpName NVARCHAR(100) Collate Database_Default
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
	SlpName NVARCHAR(100) Collate Database_Default
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
	, T0.LineMemo AS [Comments]
	, OA_PGTO.Conta AS [AcctCode]
	, OA_PGTO.NomeConta AS [AcctName]
	, CASE WHEN (T0.BalDueDeb - T0.BalDueCred) = 0 THEN 'F' ELSE 'A' END AS [DocStatus]
	, ISNULL(T6.SlpCode, T7.SlpCode) AS [SlpCode]
	, ISNULL(T6.SlpName, T7.SlpName) AS [SlpName]
FROM  JDT1 T0   
INNER JOIN OCRD T1  ON  T0.ShortName = T1.CardCode AND T1.CardType = 'C'    
INNER JOIN OJDT T2  ON  T2.TransId = T0.TransId   
LEFT  JOIN CRD7 T3  ON  T1.CardCode = T3.CardCode AND T3.[Address] = '' AND T3.AddrType = 'S'
LEFT  JOIN INV6 T4  ON  T0.SourceLine = T4.InstlmntID AND T0.SourceID = T4.DocEntry
LEFT  JOIN OINV T5  ON  T4.DocEntry = T5.DocEntry
LEFT  JOIN OSLP T6  ON  T5.SlpCode = T6.SlpCode
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
WHERE (T0.BalDueDeb - T0.BalDueCred) <> 0 AND
	(T0.RefDate >= @DtLancamentoIni AND T0.RefDate <= @DtLancamentoFin) AND
	(T0.DueDate >= @DtVencimentoIni AND T0.DueDate <= @DtVencimentoFin)

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
		TX.SlpName
	FROM #tmp_docs TX


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
		MAX(ChavePgto) ChavePgto,
		MAX(ChavePgtoString) ChavePgtoString,
		MAX(Pagamento) Pagamento,
		TipoObjPgto,
		TipoPgto,
		TabelaPgto,
		SUM(ValorPago) ValorPago,
		JurosDesconto,
		ValorSaldo,
		PgtoCur,
		PgtoRate,
		ValorPagoME,
		FormaPgto,
		Comments,
		AcctCode,
		AcctName,
		DocStatus,
		DiffDias = case when DocStatus = 'C' then 0 else case when DocStatus = 'F' then datediff(day, isnull(MAX(Pagamento), Vencimento), Vencimento) else datediff(day, getdate(), Vencimento) end end,
		SlpCode,
		SlpName
	from #tmp_return
	GROUP BY
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
		TipoObjPgto,
		TipoPgto,
		TabelaPgto,
		JurosDesconto,
		ValorSaldo,
		PgtoCur,
		PgtoRate,
		ValorPagoME,
		FormaPgto,
		Comments,
		AcctCode,
		AcctName,
		DocStatus,
		SlpCode,
		SlpName
	order by cardcode, tipoobj, chavedoc, parcela

DROP TABLE #tmp_docs
DROP TABLE #tmp_return
END

