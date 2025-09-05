-- EXEC [dbo].[spc_CVA_RelatorioCR_Nova] 'T', '2018-05-25','2018-05-25', '2018-05-25','2018-05-25', 'N'
ALTER PROCEDURE [dbo].[spc_CVA_RelatorioCR_Nova]
(
	@Tipo CHAR(1) = 'T', 
	@DtLancamentoIni DATE = NULL,
	@DtLancamentoFin DATE = NULL,
	@DtVencimentoIni DATE = NULL,
	@DtVencimentoFin DATE = NULL,
	@Provisoes CHAR(1) = 'N'
)
AS
BEGIN
SET NOCOUNT ON;
SET DATEFORMAT 'ymd';

CREATE TABLE #tmp_docs
(
	ChaveDoc NVARCHAR(400) Collate Database_Default,
	ChaveDocString NVARCHAR(50) Collate Database_Default,
	Numero NVARCHAR(MAX) Collate Database_Default,
	Parcela NVARCHAR(400) Collate Database_Default,
	ParcelaString NVARCHAR(20) Collate Database_Default,
	CardCode NVARCHAR(100) Collate Database_Default,
	CardName NVARCHAR(400) Collate Database_Default,
	CardFName NVARCHAR(400) Collate Database_Default,
	CardCodeName NVARCHAR(600) Collate Database_Default,
	CNPJ NVARCHAR(50) Collate Database_Default,
	TipoObj NVARCHAR(100) Collate Database_Default,
	TipoDoc NVARCHAR(10) Collate Database_Default,
	TabelaDoc NVARCHAR(10) Collate Database_Default,
	Emissao DATETIME,
	Vencimento DATETIME,
	ValorDoc NUMERIC(19,6),
	DocCur NVARCHAR(20) Collate Database_Default,
	DocRate NUMERIC(19,6),
	ChavePgto NVARCHAR(200) Collate Database_Default,
	ChavePgtoString NVARCHAR(10) Collate Database_Default,
	Pagamento DATETIME,
	TipoObjPgto NVARCHAR(100) Collate Database_Default,
	TipoPgto NVARCHAR(10) Collate Database_Default,
	TabelaPgto NVARCHAR(20) Collate Database_Default,
	ValorPago NUMERIC(19,6),
	JurosDesconto NUMERIC(19,6),
	ValorSaldo NUMERIC(19,6),
	PgtoCur NVARCHAR(20) Collate Database_Default,
	PgtoRate NUMERIC(19,6),
	FormaPgto NVARCHAR(MAX) Collate Database_Default,
	Comments NVARCHAR(MAX) Collate Database_Default,
	AcctCode NVARCHAR(100) Collate Database_Default,
	AcctName NVARCHAR(400) Collate Database_Default,
	DocStatus CHAR(1) Collate Database_Default,
	SlpCode NVARCHAR(200) Collate Database_Default,
	SlpName NVARCHAR(400) Collate Database_Default,
	GrupoEconCode NVARCHAR(100) Collate Database_Default,
	GrupoEconDesc NVARCHAR(400) Collate Database_Default
)

CREATE TABLE #tmp_return
(
	ChaveDoc NVARCHAR(400) Collate Database_Default,
	ChaveDocString NVARCHAR(50) Collate Database_Default,
	Numero NVARCHAR(MAX) Collate Database_Default,
	Parcela NVARCHAR(400) Collate Database_Default,
	ParcelaString NVARCHAR(20) Collate Database_Default,
	CardCode NVARCHAR(100) Collate Database_Default,
	CardName NVARCHAR(400) Collate Database_Default,
	CardFName NVARCHAR(400) Collate Database_Default,
	CardCodeName NVARCHAR(600) Collate Database_Default,
	CNPJ NVARCHAR(50) Collate Database_Default,
	TipoObj NVARCHAR(100) Collate Database_Default,
	TipoDoc NVARCHAR(10) Collate Database_Default,
	TabelaDoc NVARCHAR(20) Collate Database_Default,
	Emissao DATETIME,
	Vencimento DATETIME,
	ValorDoc NUMERIC(19,6),
	DocCur NVARCHAR(20) Collate Database_Default,
	DocRate NUMERIC(19,6),
	ValorDocME NUMERIC(19,6),
	ChavePgto NVARCHAR(400) Collate Database_Default,
	ChavePgtoString NVARCHAR(50) Collate Database_Default,
	Pagamento DATETIME,
	TipoObjPgto NVARCHAR(100) Collate Database_Default,
	TipoPgto NVARCHAR(10) Collate Database_Default,
	TabelaPgto NVARCHAR(20) Collate Database_Default,
	ValorPago NUMERIC(19,6),
	JurosDesconto NUMERIC(19,6),
	ValorSaldo NUMERIC(19,6),
	PgtoCur NVARCHAR(10) Collate Database_Default,
	PgtoRate NUMERIC(19,6),
	ValorPagoME NUMERIC(19,6),
	FormaPgto NVARCHAR(MAX) Collate Database_Default,
	Comments NVARCHAR(MAX) Collate Database_Default,
	AcctCode NVARCHAR(100) Collate Database_Default,
	AcctName NVARCHAR(400) Collate Database_Default,
	DocStatus CHAR(10) Collate Database_Default,
	SlpCode NVARCHAR(200) Collate Database_Default,
	SlpName NVARCHAR(400) Collate Database_Default,
	GrupoEconCode NVARCHAR(100) Collate Database_Default,
	GrupoEconDesc NVARCHAR(400) Collate Database_Default
)

CREATE TABLE #tmp_fat
(
	ChaveDoc NVARCHAR(200) Collate Database_Default,
	ChaveDocString NVARCHAR(50) Collate Database_Default,
	Numero NVARCHAR(max) Collate Database_Default,
	Parcela NVARCHAR(200) Collate Database_Default,
	ParcelaString NVARCHAR(15) Collate Database_Default,
	CardCode NVARCHAR(100) Collate Database_Default,
	CardName NVARCHAR(400) Collate Database_Default,
	CardFName NVARCHAR(400) Collate Database_Default,
	CardCodeName NVARCHAR(600) Collate Database_Default,
	CNPJ NVARCHAR(50) Collate Database_Default,
	TipoObj NVARCHAR(40) Collate Database_Default,
	TipoDoc NVARCHAR(10) Collate Database_Default,
	TabelaDoc NVARCHAR(10) Collate Database_Default,
	Emissao DATETIME,
	Vencimento DATETIME,
	ValorDoc NUMERIC(19,6),
	DocCur NVARCHAR(20) Collate Database_Default,
	DocRate NUMERIC(19,6),
	ValorSaldo NUMERIC(19,6),
	Comments NVARCHAR(MAX) Collate Database_Default,
	DocStatus CHAR(10) Collate Database_Default,
	SlpCode NVARCHAR(200) Collate Database_Default,
	SlpName NVARCHAR(400) Collate Database_Default,
	GrupoEconCode NVARCHAR(100) Collate Database_Default,
	GrupoEconDesc NVARCHAR(400) Collate Database_Default
)

CREATE TABLE #tmp_pag
(
	ChaveDoc NVARCHAR(200) Collate Database_Default,
	Parcela NVARCHAR(200) Collate Database_Default,
	TipoObj NVARCHAR(40) Collate Database_Default,
	ChavePgto NVARCHAR(200) Collate Database_Default,
	ChavePgtoString NVARCHAR(10) Collate Database_Default,
	Pagamento DATETIME,
	TipoObjPgto NVARCHAR(40) Collate Database_Default,
	TipoPgto NVARCHAR(2) Collate Database_Default,
	TabelaPgto NVARCHAR(4) Collate Database_Default,
	NumeroChequeBoleto NVARCHAR(200) Collate Database_Default,
	ValorPago NUMERIC(19,6),
	JurosDesconto NUMERIC(19,6),
	PgtoCur NVARCHAR(10) Collate Database_Default,
	PgtoRate NUMERIC(19,6),
	FormaPgto NVARCHAR(MAX) Collate Database_Default,
	AcctCode NVARCHAR(50) Collate Database_Default,
	AcctName NVARCHAR(200) Collate Database_Default,
	DocStatus CHAR(1) Collate Database_Default,
	Vencimento DATETIME
)

INSERT INTO #tmp_fat
SELECT DISTINCT 
	T0.DocEntry AS 'ChaveDoc'
	, CAST(T0.DocEntry AS NVARCHAR(10)) AS 'ChaveDocString'
	, CAST(T0.Serial AS NVARCHAR(MAX)) AS 'Numero'
	, T2.InstlmntID AS 'Parcela'
	, CAST(T2.InstlmntID AS NVARCHAR(10)) + '/' + CAST(T0.Installmnt AS NVARCHAR(10)) AS 'ParcelaString'
	, T3.CardCode AS 'CardCode'
	, T3.CardName AS 'CardName'
	, ISNULL(T3.CardFName, T3.CardName) AS 'CardFName'
	, T3.CardCode + ' - ' + T3.CardName AS 'CardCodeName'
	, CASE WHEN ISNULL(T5.TaxId4, '') <> '' THEN T5.TaxId4 ELSE T5.TaxId0 END AS 'CNPJ'
	, T0.ObjType AS 'TipoObj'
	, 'NS' AS 'TipoDoc'
	, 'OINV' AS 'TabelaDoc'
	, T0.DocDate AS 'Emissao'
	, T2.DueDate AS 'Vencimento'
	, T2.InsTotal AS 'ValorParcela'
	, T0.DocCur AS 'DocCur'
	, T0.DocRate AS 'DocRate'
	, T2.InsTotal - T2.PaidToDate AS 'ValorSaldo'
	, T0.Comments AS 'Comments'
	, CASE WHEN (T2.InsTotal - T2.PaidToDate) = 0 THEN 'F' ELSE 'A' END AS 'DocStatus'
	, T4.SlpCode AS 'SlpCode'
	, T4.SlpName AS 'SlpName'
	, '001' AS 'GrupoEconCode'
	, 'N SE APLICA' AS 'GrupoEconName'
FROM OINV T0
	INNER JOIN INV1 T1 ON T0.DocEntry = T1.DocEntry AND T1.TargetType <> N'14' AND ISNULL(T1.FreeChrgBP, 'N') = 'N'
	INNER JOIN INV6 T2 ON T0.DocEntry = T2.DocEntry AND T2.InsTotal <> 0.00
	INNER JOIN OCRD T3 ON T0.CardCode = T3.CardCode
	LEFT  JOIN OSLP T4 ON T0.SlpCode = T4.SlpCode
	LEFT  JOIN CRD7 T5 ON T3.CardCode = T5.CardCode AND T5.[Address] = '' ANd T5.AddrType = 'S'
WHERE T0.CANCELED = 'N'

--> Passou

UNION ALL
SELECT DISTINCT 
	T0.DocEntry AS 'ChaveDoc'
	, CAST(T0.DocEntry AS NVARCHAR(10)) AS 'ChaveDocString'
	, CAST(ISNULL(T0.Serial, T0.DocEntry) AS NVARCHAR(MAX)) AS 'Numero'
	, T2.InstlmntID AS 'Parcela'
	, CAST(T2.InstlmntID AS NVARCHAR(10)) + '/' + CAST(T0.Installmnt AS NVARCHAR(10)) AS 'ParcelaString'
	, T3.CardCode AS 'CardCode'
	, T3.CardName AS 'CardName'
	, ISNULL(T3.CardFName, T3.CardName) AS 'CardFName'
	, T3.CardCode + ' - ' + T3.CardName AS 'CardCodeName'
	, CASE WHEN ISNULL(T5.TaxId4, '') <> '' THEN T5.TaxId4 ELSE T5.TaxId0 END AS 'CNPJ'
	, T0.ObjType AS 'TipoObj'
	, 'AT' AS 'TipoDoc'
	, 'ODPI' AS 'TabelaDoc'
	, T0.DocDate AS 'Emissao'
	, T2.DueDate AS 'Vencimento'
	, T2.InsTotal AS 'ValorParcela'
	, T0.DocCur AS 'DocCur'
	, T0.DocRate AS 'DocRate'
	, T2.InsTotal - T2.PaidToDate AS 'ValorSaldo'
	, T0.Comments AS 'Comments'
	, CASE WHEN (T2.InsTotal - T2.PaidToDate) = 0 THEN 'F' ELSE 'A' END AS 'DocStatus'
	, T4.SlpCode AS 'SlpCode'
	, T4.SlpName AS 'SlpName'
	, '001' AS 'GrupoEconCode'
	, 'N SE APLICA' AS 'GrupoEconName'
FROM ODPI T0
	INNER JOIN DPI1 T1 ON T0.DocEntry = T1.DocEntry AND T1.TargetType <> N'14' AND ISNULL(T1.FreeChrgBP, 'N') = 'N'
	INNER JOIN DPI6 T2 ON T0.DocEntry = T2.DocEntry AND T2.InsTotal <> 0.00
	INNER JOIN OCRD T3 ON T0.CardCode = T3.CardCode
	LEFT  JOIN OSLP T4 ON T0.SlpCode = T4.SlpCode
	LEFT  JOIN CRD7 T5 ON T3.CardCode = T5.CardCode AND T5.[Address] = '' ANd T5.AddrType = 'S'
WHERE T0.CANCELED = 'N'

--> Passou

UNION ALL
SELECT DISTINCT 
	T0.DocEntry AS 'ChaveDoc'
	, CAST(T0.DocEntry AS NVARCHAR(10)) AS 'ChaveDocString'
	, CAST(ISNULL(T0.Serial, T0.DocEntry) AS NVARCHAR(MAX)) AS 'Numero'
	, T2.InstlmntID AS 'Parcela'
	, CAST(T2.InstlmntID AS NVARCHAR(10)) + '/' + CAST(T0.Installmnt AS NVARCHAR(10)) AS 'ParcelaString'
	, T3.CardCode AS 'CardCode'
	, T3.CardName AS 'CardName'
	, ISNULL(T3.CardFName, T3.CardName) AS 'CardFName'
	, T3.CardCode + ' - ' + T3.CardName AS 'CardCodeName'
	, CASE WHEN ISNULL(T5.TaxId4, '') <> '' THEN T5.TaxId4 ELSE T5.TaxId0 END AS 'CNPJ'
	, T0.ObjType AS 'TipoObj'
	, 'DS' AS 'TipoDoc'
	, 'ORIN' AS 'TabelaDoc'
	, T0.DocDate AS 'Emissao'
	, T2.DueDate AS 'Vencimento'
	, T2.InsTotal AS 'ValorParcela'
	, T0.DocCur AS 'DocCur'
	, T0.DocRate AS 'DocRate'
	, (T2.InsTotal - T2.PaidToDate) AS 'ValorSaldo'
	, T0.Comments AS 'Comments'
	, CASE WHEN (T2.InsTotal - T2.PaidToDate) = 0 THEN 'F' ELSE 'A' END AS 'DocStatus'
	, T4.SlpCode AS 'SlpCode'
	, T4.SlpName AS 'SlpName'
	, '001' AS 'GrupoEconCode'
	, 'N SE APLICA' AS 'GrupoEconName'
FROM ORIN T0
	INNER JOIN RIN1 T1 ON T0.DocEntry = T1.DocEntry AND T1.BaseType = N'-1' AND ISNULL(T1.FreeChrgBP, 'N') = 'N'
	INNER JOIN RIN6 T2 ON T0.DocEntry = T2.DocEntry AND T2.InsTotal <> 0.00
	INNER JOIN OCRD T3 ON T0.CardCode = T3.CardCode
	LEFT  JOIN OSLP T4 ON T0.SlpCode = T4.SlpCode
	LEFT  JOIN CRD7 T5 ON T3.CardCode = T5.CardCode AND T5.[Address] = '' ANd T5.AddrType = 'S'
WHERE T0.CANCELED = 'N'

--> passou


UNION ALL
SELECT DISTINCT 
	T0.TransId AS 'ChaveDoc'
	, CAST(T0.TransId AS NVARCHAR(10)) AS 'ChaveDocString'
	, CASE WHEN CAST(ISNULL(T1.Ref1, '') AS NVARCHAR(MAX)) = '' THEN CAST(T0.TransId AS NVARCHAR(10)) ELSE CAST(T1.Ref1 AS NVARCHAR(MAX)) END AS [Numero]
	, T1.Line_ID AS [Parcela]
	, CASE WHEN ISNULL(T1.Ref2, '') = '' THEN '1/1' ELSE CAST(T1.Ref2 AS NVARCHAR(MAX)) END AS [ParcelaString]
	, T2.CardCode AS 'CardCode'
	, T2.CardName AS 'CardName'
	, ISNULL(T2.CardFName, T2.CardName) AS 'CardFName'
	, T2.CardCode + ' - ' + T2.CardName AS 'CardCodeName'
	, CASE WHEN ISNULL(T4.TaxId4, '') <> '' THEN T4.TaxId4 ELSE T4.TaxId0 END AS 'CNPJ'
	, T1.TransType AS 'TipoObj'
	, 'LC' AS 'TipoDoc'
	, 'OJDT' AS 'TabelaDoc'
	, T0.RefDate AS 'Emissao'
	, T1.DueDate AS 'Vencimento'
	, T1.Debit - T1.Credit AS 'ValorParcela'
	, T1.FCCurrency AS 'DocCur'
	, CASE WHEN T1.FCCurrency IS NOT NULL THEN 
		CASE WHEN T1.FCDebit <> 0 THEN T1.Debit / T1.FCDebit
		ELSE CASE WHEN T1.FCCredit <> 0 THEN T1.Credit / T1.FCCredit
		ELSE 0 END END ELSE 1 END AS 'DocRate'
	, T1.BalDueDeb - T1.BalDueCred AS 'ValorSaldo'
	, CAST(T1.LineMemo AS NVARCHAR(MAX)) AS 'Comments'
	, CASE WHEN (T1.BalDueDeb - T1.BalDueCred) = 0 THEN 'F' ELSE 'A' END AS 'DocStatus'
	, T5.SlpCode AS 'SlpCode'
	, T5.SlpName AS 'SlpName'
	, '001' AS 'GrupoEconCode'
	, 'N SE APLICA' AS 'GrupoEconName'
FROM OJDT T0
	INNER JOIN JDT1 T1 ON T0.TransId = T1.TransId AND T1.TransType = N'30'
	INNER JOIN OCRD T2 ON T1.ShortName = T2.CardCode AND T2.CardType = 'C'
	LEFT  JOIN CRD7 T4 ON T2.CardCode = T4.CardCode AND T4.[Address] = '' AND T4.AddrType = 'S'
	LEFT  JOIN OSLP T5 ON T1.U_VENDEDOR = T5.SlpCode
WHERE T0.StornoToTr IS NULL
	AND T0.TransId NOT IN (SELECT TX.StornoToTr FROM OJDT TX WHERE TX.StornoToTr = T0.TransId)
	and T0.TransId not in (select A.transid from ITR1 A inner join OITR B on A.ReconNum = B.ReconNum where ReconType = '1')

	
	
INSERT INTO #tmp_pag
SELECT DISTINCT
	T1.DocEntry AS 'ChaveDoc'
	, CASE WHEN T1.InvType = N'30' THEN T1.DocLine ELSE T1.InstId END AS 'Parcela'
	, T1.InvType AS 'TipoObj'
	, T0.DocNum AS 'ChavePgto'
	, CAST(T0.DocNum AS NVARCHAR(10)) AS 'ChavePgtoString'
	, T0.TaxDate AS 'Pagamento'
	, T0.ObjType AS 'TipoObjPgto'
	, 'CR' AS 'TipoPgto'
	, 'ORCT' AS 'TabelaPgto'
	, NULL AS 'NumeroChequeBoleto'
	, T1.SumApplied + (T0.UndOvDiff / OA_MAX.MaxCount) + (T0.NoDocSum / OA_MAX.MaxCount) AS 'ValorPago'
	, T0.UndOvDiff / OA_MAX.MaxCount AS 'JurosDesconto'
	, T0.DocCurr AS 'PgtoCur'
	, T0.DocRate AS 'PgtoRate'
	, 'Recebimento Dinheiro' AS 'FormaPgto'
	, T0.CashAcct AS 'AcctCode'
	, T2.AcctName AS 'AcctName'
	, 'F' AS 'DocStatus'
	, NULL AS 'Vencimento'
FROM ORCT T0
	INNER JOIN RCT2 T1 ON T0.DocEntry = T1.DocNum
	INNER JOIN OACT T2 ON T0.CashAcct = T2.AcctCode
OUTER APPLY (
	SELECT MaxCount = COUNT(TX.InvoiceId) FROM RCT2 TX WHERE TX.DocNum = T0.DocEntry
) AS OA_MAX
WHERE T0.Canceled = 'N'
	AND T0.CashSum <> 0
UNION ALL
SELECT DISTINCT
	T1.DocEntry AS 'ChaveDoc'
	, CASE WHEN T1.InvType = N'30' THEN T1.DocLine ELSE T1.InstId END AS 'Parcela'
	, T1.InvType AS 'TipoObj'
	, T0.DocNum AS 'ChavePgto'
	, CAST(T0.DocNum AS NVARCHAR(10)) AS 'ChavePgtoString'
	, T0.TaxDate AS 'Pagamento'
	, T0.ObjType AS 'TipoObjPgto'
	, 'CR' AS 'TipoPgto'
	, 'ORCT' AS 'TabelaPgto'
	, NULL AS 'NumeroChequeBoleto'
	, T1.SumApplied + (T0.UndOvDiff / OA_MAX.MaxCount) + (T0.NoDocSum / OA_MAX.MaxCount) AS 'ValorPago'
	, T0.UndOvDiff / OA_MAX.MaxCount AS 'JurosDesconto'
	, T0.DocCurr AS 'PgtoCur'
	, T0.DocRate AS 'PgtoRate'
	, 'Recebimento Transferência Bancária' AS 'FormaPgto'
	, T0.TrsfrAcct AS 'AcctCode'
	, T2.AcctName AS 'AcctName'
	, 'F' AS 'DocStatus'
	, NULL AS 'Vencimento'
FROM ORCT T0
	INNER JOIN RCT2 T1 ON T0.DocEntry = T1.DocNum
	INNER JOIN OACT T2 ON T0.TrsfrAcct = T2.AcctCode
OUTER APPLY (
	SELECT MaxCount = COUNT(TX.InvoiceId) FROM RCT2 TX WHERE TX.DocNum = T0.DocEntry
) AS OA_MAX
WHERE T0.Canceled = 'N'
	AND T0.TrsfrSum <> 0
UNION ALL
SELECT DISTINCT
	T1.DocEntry AS 'ChaveDoc'
	, CASE WHEN T1.InvType = N'30' THEN T1.DocLine ELSE T1.InstId END AS 'Parcela'
	, T1.InvType AS 'TipoObj'
	, T0.DocNum AS 'ChavePgto'
	, CAST(T0.DocNum AS NVARCHAR(10)) AS 'ChavePgtoString'
	, CASE WHEN ISNULL(T3.StatusTo, '') = 'P' THEN T4.RefDate ELSE NULL END AS 'Pagamento'
	, T0.ObjType AS 'TipoObjPgto'
	, 'CR' AS 'TipoPgto'
	, 'ORCT' AS 'TabelaPgto'
	, T2.BoeNum AS 'NumeroChequeBoleto'
	, T1.SumApplied + (T0.UndOvDiff / OA_MAX.MaxCount) + (T0.NoDocSum / OA_MAX.MaxCount) AS 'ValorPago'
	, T0.UndOvDiff / OA_MAX.MaxCount AS 'JurosDesconto'
	, T0.DocCurr AS 'PgtoCur'
	, T0.DocRate AS 'PgtoRate'
	, T5.Descript AS 'FormaPgto'
	, T6.AcctCode AS 'AcctCode'
	, T6.AcctName AS 'AcctName'
	, CASE WHEN ISNULL(T3.StatusTo, '') = 'P' THEN 'F' ELSE 'A' END AS 'DocStatus'
	, T2.DueDate AS 'Vencimento'
FROM ORCT T0
	INNER JOIN RCT2 T1 ON T0.DocEntry = T1.DocNum
	INNER JOIN OBOE T2 ON T0.BoeAbs = T2.BoeKey
	LEFT  JOIN OBOT T3 ON T3.AbsEntry =
	(
		SELECT TOP 1 BOT1.AbsEntry FROM BOT1
		WHERE BOT1.BOENumber = T2.BoeNum AND BOT1.BoeType = T2.BoeType
		ORDER BY BOT1.AbsEntry DESC
	) AND ISNULL(T3.StatusTo, '') = 'P'
	LEFT  JOIN OJDT T4 ON T3.TransId = T4.Number
	LEFT  JOIN OPYM T5 ON T2.PayMethCod = T5.PayMethCod
	LEFT  JOIN OACT T6 ON T5.GLAccount = T6.AcctCode
OUTER APPLY (
	SELECT MaxCount = COUNT(TX.InvoiceId) FROM RCT2 TX WHERE TX.DocNum = T0.DocEntry
) AS OA_MAX
WHERE T0.Canceled = 'N'
	AND T0.BoeSum <> 0
UNION ALL
SELECT DISTINCT
	T1.DocEntry AS 'ChaveDoc'
	, CASE WHEN T1.InvType = N'30' THEN T1.DocLine ELSE T1.InstId END AS 'Parcela'
	, T1.InvType AS 'TipoObj'
	, T0.DocNum AS 'ChavePgto'
	, CAST(T0.DocNum AS NVARCHAR(10)) AS 'ChavePgtoString'
	, CASE WHEN ISNULL(T5.DeposType, '') = 'K' THEN T5.DeposDate ELSE NULL END AS 'Pagamento'
	, T0.ObjType AS 'TipoObjPgto'
	, 'CR' AS 'TipoPgto'
	, 'ORCT' AS 'TabelaPgto'
	, T3.CheckNum AS 'NumeroChequeBoleto'
	, T3.[CheckSum] AS 'ValorPago'
	, T0.UndOvDiff / OA_MAX.MaxCount AS 'JurosDesconto'
	, T0.DocCurr AS 'PgtoCur'
	, T0.DocRate AS 'PgtoRate'
	, 'Recebimento Cheque' AS 'FormaPgto'
	, T6.AcctCode AS 'AcctCode'
	, T6.AcctName AS 'AcctName'
	, CASE WHEN ISNULL(T5.DeposType, '') = 'K' THEN 'F' ELSE 'A' END AS 'DocStatus'
	, T2.DueDate AS 'Vencimento'
FROM ORCT T0
	INNER JOIN RCT2 T1 ON T0.DocEntry = T1.DocNum
	INNER JOIN RCT1 T2 ON T0.DocEntry = T2.DocNum
	INNER JOIN OCHH T3 ON T2.CheckAbs = T3.CheckKey
	LEFT  JOIN DPS1 T4 ON T3.CheckKey = T4.CheckKey AND T4.DepCancel = 'N' AND T3.DpstAbs = T4.DepositId
	LEFT  JOIN ODPS T5 ON T4.DepositId = T5.DeposId AND T5.DeposType = 'K'
	LEFT  JOIN OACT T6 ON T3.BankAcct = T6.AcctCode
OUTER APPLY (
	SELECT MaxCount = COUNT(TX.InvoiceId) FROM RCT2 TX WHERE TX.DocNum = T0.DocEntry
) AS OA_MAX
WHERE T0.Canceled = 'N'
	AND T0.[CheckSum] <> 0

UNION ALL

SELECT DISTINCT
	T1.SrcObjAbs AS 'ChaveDoc'
	, 1  AS 'Parcela'
	, T1.SrcObjTyp AS 'TipoObj'
	, T0.ReconNum AS 'ChavePgto'
	, CAST(T0.ReconNum AS NVARCHAR(10)) AS 'ChavePgtoString'
	, T0.ReconDate AS 'Pagamento'
	, '' AS 'TipoObjPgto'
	, 'RM' AS 'TipoPgto'
	, 'OITR' AS 'TabelaPgto'
	, '' AS 'NumeroChequeBoleto'
	, T1.ReconSum AS 'ValorPago'
	, 0.00 AS 'JurosDesconto'
	, T0.ReconCurr AS 'PgtoCur'
	, 100 AS 'PgtoRate'
	, 'Reconciliação' AS 'FormaPgto'
	, T2.AcctCode AS 'AcctCode'
	, T2.AcctName AS 'AcctName'
	, 'F' AS 'DocStatus'
	, NULL AS 'Vencimento'
FROM OITR T0
	INNER JOIN ITR1 T1 ON T0.ReconNum = T1.ReconNum
	LEFT  JOIN OACT T2 ON T2.AcctCode = T1.ShortName
WHERE T0.Canceled = 'N'
	AND T0.ReconType = 0 -- Manual
ORDER BY 3, 1, 2

INSERT INTO #tmp_docs
SELECT DISTINCT 
	T0.ChaveDoc,
	T0.ChaveDocString,
	T0.Numero,
	T0.Parcela,
	T0.ParcelaString,
	T0.CardCode,
	T0.CardName,
	T0.CardFName,
	T0.CardCodeName,
	T0.CNPJ,
	T0.TipoObj,
	T0.TipoDoc,
	T0.TabelaDoc,
	T0.Emissao,
	ISNULL(T1.Vencimento, T0.Vencimento),
	T0.ValorDoc,
	ISNULL(T0.DocCur, 'R$'),
	T0.DocRate,
	T1.ChavePgto,
	T1.ChavePgtoString,
	T1.Pagamento,
	T1.TipoObjPgto,
	T1.TipoPgto,
	T1.TabelaPgto,
	T1.ValorPago,
	T1.JurosDesconto,
	T0.ValorSaldo,
	T1.PgtoCur,
	T1.PgtoRate,
	T1.FormaPgto,
	T0.Comments,
	T1.AcctCode,
	T1.AcctName,
	ISNULL(T1.DocStatus, T0.DocStatus),
	T0.SlpCode,
	T0.SlpName,
	T0.GrupoEconCode,
	T0.GrupoEconDesc
FROM #tmp_fat T0
	LEFT JOIN #tmp_pag T1 ON T0.ChaveDoc = T1.ChaveDoc AND T0.TipoObj = T1.TipoObj AND T0.Parcela = T1.Parcela

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
		CASE WHEN TX.DocStatus = 'A' THEN 0 ELSE TX.ValorPago END AS ValorPago,
		TX.JurosDesconto,
		CASE WHEN TX.DocStatus = 'A' AND ISNULL(TX.ValorSaldo, 0) = 0 THEN CASE WHEN ISNULL(TX.ValorPago, 0) = 0 THEN TX.ValorDoc ELSE TX.ValorPago END ELSE TX.ValorSaldo END AS ValorSaldo,
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
		TX.GrupoEconCode,
		TX.GrupoEconDesc
	FROM #tmp_docs TX
	
--IF @Provisoes = 'Y'
--BEGIN
--	INSERT INTO #tmp_return
--		SELECT DISTINCT * FROM [dbo].[fn_CVA_ProvisoesFluxoCaixaCR](@DtLancamentoIni, @DtLancamentoFin, @DtVencimentoIni, @DtVencimentoFin)
--		WHERE ((Emissao >= @DtLancamentoIni OR @DtLancamentoIni IS NULL) AND (Emissao <= @DtLancamentoFin OR @DtLancamentoFin IS NULL))
--		OR ((Vencimento >= @DtVencimentoIni OR @DtVencimentoIni IS NULL) AND (Vencimento <= @DtVencimentoFin OR @DtVencimentoFin IS NULL))
--END

SELECT DISTINCT 
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
	ChavePgto,
	ChavePgtoString,
	Pagamento,
	TipoObjPgto,
	TipoPgto,
	TabelaPgto,
	ISNULL(ValorPago, 0) AS ValorPago,
	ISNULL(JurosDesconto, 0) AS JurosDesconto,
	ISNULL(ValorSaldo, 0) AS ValorSaldo,
	PgtoCur,
	PgtoRate,
	ValorPagoME,
	ISNULL(FormaPgto, '--') AS FormaPgto,
	Comments,
	AcctCode,
	AcctName,
	DocStatus,
	DiffDias = 
		CASE WHEN DocStatus = 'A' THEN
			CASE WHEN Vencimento >= GETDATE() THEN 0
			ELSE DATEDIFF(DAY, GETDATE(), Vencimento) END
		ELSE 0 END,
	SlpCode,
	SlpName,
	GrupoEconCode,
	GrupoEconDesc
FROM #tmp_return 
WHERE (DocStatus = @Tipo OR @Tipo = 'T')
	AND (Emissao >= @DtLancamentoIni OR @DtLancamentoIni IS NULL)
	AND (Emissao <= @DtLancamentoFin OR @DtLancamentoFin IS NULL)
	AND (Vencimento >= @DtVencimentoIni OR @DtVencimentoIni IS NULL)
	AND (Vencimento <= @DtVencimentoFin OR @DtVencimentoFin IS NULL)
	--AND ChaveDoc <> '200516'
		
		order by Vencimento
--ORDER BY CardCode, TipoDoc, ChaveDoc, Parcela

DROP TABLE #tmp_docs
DROP TABLE #tmp_return
DROP TABLE #tmp_fat
DROP TABLE #tmp_pag

END
