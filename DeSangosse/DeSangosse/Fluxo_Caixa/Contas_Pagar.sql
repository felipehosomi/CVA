/*    ==Parâmetros de Script==

    Versão do Servidor de Origem : SQL Server 2014 (12.0.2000)
    Edição do Mecanismo de Banco de Dados de Origem : Microsoft SQL Server Standard Edition
    Tipo do Mecanismo de Banco de Dados de Origem : SQL Server Autônomo

    Versão do Servidor de Destino : SQL Server 2014
    Edição de Mecanismo de Banco de Dados de Destino : Microsoft SQL Server Standard Edition
    Tipo de Mecanismo de Banco de Dados de Destino : SQL Server Autônomo
*/

USE [SBOHybel]
GO

/****** Object:  StoredProcedure [dbo].[spc_CVA_RelatorioCP_Nova]    Script Date: 22/08/2018 16:20:09 ******/
DROP PROCEDURE [dbo].[spc_CVA_RelatorioCP_Nova]
GO

/****** Object:  StoredProcedure [dbo].[spc_CVA_RelatorioCP_Nova]    Script Date: 22/08/2018 16:20:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spc_CVA_RelatorioCP_Nova]
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
--spc_CVA_RelatorioCP_Nova 'T','2018/08/01','2018/08/31',NULL, NULL, 'Y'
--declare	@Tipo CHAR(1) = 'T', 
--	@DtLancamentoIni DATE = NULL,
--	@DtLancamentoFin DATE = NULL,
--	@DtVencimentoIni DATE = NULL,
--	@DtVencimentoFin DATE = NULL,
--	@Provisoes CHAR(1) = 'N'

	
--set	@Tipo = 'T'
--set	@DtLancamentoIni = NULL
--set	@DtLancamentoFin =NULL
--set	@DtVencimentoIni  = '2018/08/01'
--set	@DtVencimentoFin  ='2018/08/31'
--set @Provisoes = 'N'

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
	DocStatus CHAR(1) Collate Database_Default
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
	DocStatus CHAR(1) Collate Database_Default
)


INSERT INTO #tmp_docs
SELECT DISTINCT
	T0.DocEntry AS [ChaveDoc],
	CAST(T0.DocEntry AS NVARCHAR(10)) AS [ChaveDocString],
	CAST(T0.Serial AS NVARCHAR(50)) AS [Numero],
	T2.InstlmntID AS [Parcela],
	CAST(T2.InstlmntID AS NVARCHAR(10)) + '/' + CAST(T0.Installmnt AS NVARCHAR(10)) AS [ParcelaString],
	T3.CardCode AS [CardCode],
	T3.CardName AS [CardName],
	ISNULL(T3.CardFName, T3.CardName) AS [CardFName],
	T3.CardCode + ' - ' + T3.CardName AS [CardCodeName],
	CASE WHEN ISNULL(T4.TaxId4, '') <> '' THEN T4.TaxId4 ELSE T4.TaxId0 END AS [CNPJ],
	T0.ObjType AS [TipoObj],
	'NE' AS [TipoDoc],
	'OPCH' AS [TabelaDoc],
	T0.DocDate AS [Emissao],
	T2.DueDate as [Vencimento],
	T2.InsTotal AS [ValorParcela],
	ISNULL(T0.DocCur, 'R$') AS [DocCur],
	ISNULL(T0.DocRate, 1) AS [DocRate],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.DocEntry ELSE NULL END AS [ChavePgto],
	CAST(CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.DocEntry ELSE NULL END AS NVARCHAR(10)) AS [ChavePgtoString],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.Pagamento ELSE NULL END AS [Pagamento],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.ObjType ELSE NULL END AS [TipoObjPgto],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN 'CP' ELSE NULL END AS [TipoPgto],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN 'OVPM' ELSE NULL END AS [TabelaPgto],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.SumApplied + OA_PGTO.UndOvDiff ELSE 0 END AS [ValorPago],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.UndOvDiff ELSE 0 END AS [JurosDesconto],
	CASE WHEN T0.DocEntry = 9206 THEN T2.InsTotal - T2.PaidToDate ELSE CASE WHEN (CASE WHEN T2.InsTotal - T2.Paid = 0 THEN 0 ELSE CASE WHEN OA_DEV.DevTotal = T0.DocTotal THEN 0 ELSE (T2.InsTotal - T2.Paid) - OA_DEV.DevTotal END END) = (CASE WHEN T2.Instotal - T2.Paid = 0 THEN 0 ELSE CASE WHEN ISNULL(OA_DEVS.DocTotal, 0) = T0.DocTotal THEN 0 ELSE (T2.Instotal - T2.Paid) - ISNULL(OA_DEVS.DocTotal, 0) END END) THEN (CASE WHEN T2.InsTotal - T2.Paid = 0 THEN 0 ELSE CASE WHEN OA_DEV.DevTotal = T0.DocTotal THEN 0 ELSE (T2.InsTotal - T2.Paid) - OA_DEV.DevTotal END END) 
	ELSE (CASE WHEN T2.Instotal - T2.Paid = 0 THEN 0 ELSE CASE WHEN ISNULL(OA_DEVS.DocTotal, 0) = T0.DocTotal THEN 0 ELSE (T2.Instotal - T2.Paid) - ISNULL(OA_DEVS.DocTotal, 0) END END) END END AS [ValorSaldo],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN ISNULL(OA_PGTO.DocCurr, 'R$') ELSE NULL END AS [PgtoCur],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN ISNULL(OA_PGTO.DocRate, 1) ELSE NULL END AS [PgtoRate],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.Descript ELSE NULL END AS [FormaPgto],
	T0.Comments AS [Comments],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.Conta ELSE NULL END AS [AcctCode],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.NomeConta ELSE NULL END AS [AcctName],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN 'F' ELSE 'A' END AS [DocStatus]
FROM OPCH T0
	INNER JOIN PCH1 T1 ON T0.DocEntry = T1.DocEntry AND ISNULL(T1.FreeChrgBP, 'N') = 'N'
	INNER JOIN PCH6 T2 ON T0.DocEntry = T2.DocEntry
	INNER JOIN OCRD T3 ON T0.CardCode = T3.CardCode
	LEFT  JOIN CRD7 T4 ON T3.CardCode = T4.CardCode AND T4.[Address] = '' AND T4.AddrType = 'S'
OUTER APPLY (
	SELECT DevTotal = ISNULL(SUM(DISTINCT ISNULL(RPC6.InsTotal, 0)), 0)/T0.Installmnt
	FROM ORPC
		INNER JOIN RPC1 ON ORPC.DocEntry = RPC1.DocEntry
		INNER JOIN RPC6 ON ORPC.DocEntry = RPC6.DocEntry
	WHERE RPC1.BaseEntry = T1.DocEntry AND RPC1.BaseType = T1.ObjType
) AS OA_DEV
OUTER APPLY (
	SELECT ORPC.DocTotal, ORPC.DocDate
	FROM ORPC
		INNER JOIN RPC1 ON ORPC.DocEntry = RPC1.DocEntry
	WHERE RPC1.BaseEntry = T1.DocEntry AND RPC1.BaseType = T1.ObjType
) AS OA_DEVS
OUTER APPLY (
	SELECT DISTINCT
		OVPM.DocEntry
		, OVPM.ObjType
		, VPM2.SumApplied + ISNULL((CASE WHEN OVPM.ObjType = '46' THEN 
			(SELECT DISTINCT (X0.Debit - X0.Credit) / OA_MAX.Countt FROM JDT1 X0 WHERE X0.TransId = OVPM.TransId AND X0.Account IN ('4.1.02.001.0040', '4.1.02.001.0050', '4.1.02.002.0060'))
		  END),0) AS SumApplied
		--, VPM2.SumApplied + (SELECT [dbo].fn_CVA_VariacaoCambialComissao(CONVERT(INT,OVPM.DocEntry), CONVERT(INT,OVPM.ObjType))) AS SumApplied
		, OVPM.UndOvDiff / OA_MAX.Countt AS UndOvDiff
		, OVPM.DocCurr
		, OVPM.DocRate
		, OA_DATA.Pagamento
		, OA_FORMA.Descript
		, OA_CONTA.Conta
		, OA_CONTA.NomeConta
	FROM OVPM
		INNER JOIN VPM2 ON VPM2.DocNum = OVPM.DocEntry
		LEFT  JOIN OBOE ON OVPM.BoeAbs = OBOE.BoeKey
		LEFT  JOIN OBOT ON OBOT.AbsEntry = (
			SELECT TOP 1 BOT1.AbsEntry FROM BOT1
			WHERE BOT1.BOENumber = OBOE.BoeNum AND BOT1.BoeType = OBOE.BoeType
			ORDER BY BOT1.AbsEntry DESC
		) AND OBOT.StatusTo = 'P'
		LEFT  JOIN OJDT ON OBOT.TransId = OJDT.Number
		LEFT  JOIN VPM1 ON OVPM.DocEntry = VPM1.DocNum
		LEFT  JOIN OCHO ON VPM1.CheckAbs = OCHO.CheckKey
	OUTER APPLY (
		SELECT Countt = COUNT(TX.InvoiceId) FROM VPM2 TX WHERE TX.DocNum = OVPM.DocEntry
	) AS OA_MAX
	OUTER APPLY (
		SELECT Pagamento = 
			CASE WHEN OBOT.StatusTo = 'P' THEN OJDT.RefDate
				ELSE CASE WHEN OCHO.CheckKey IS NOT NULL THEN OCHO.CheckDate
				ELSE CASE WHEN OVPM.BoeAbs IS NULL AND OVPM.DocEntry IS NOT NULL THEN OVPM.TaxDate
				ELSE NULL END END END
	) AS OA_DATA
	OUTER APPLY (
		SELECT Conta = OACT.AcctCode, NomeConta = OACT.AcctName
		FROM OACT
		WHERE OACT.Finanse = 'Y' AND OACT.AcctCode = 
			CASE WHEN OBOT.StatusTo = 'P' THEN OBOE.BoeAcct
				ELSE CASE WHEN OCHO.CheckKey IS NOT NULL THEN OCHO.CheckAcct
				ELSE CASE WHEN OVPM.BoeAbs IS NULL AND OVPM.DocEntry IS NOT NULL THEN
					CASE WHEN OVPM.TrsfrSum <> 0 THEN OVPM.TrsfrAcct ELSE OVPM.CashAcct END END END END
	) AS OA_CONTA
	OUTER APPLY (
		SELECT Descript = 
			CASE WHEN ISNULL(OBOE.PayMethCod, OVPM.PayMth) IS NOT NULL THEN OPYM.Descript
				ELSE CASE WHEN OVPM.CashSum <> 0 THEN 'Pagamento em dinheiro'
				ELSE CASE WHEN OVPM.TrsfrSum <> 0 THEN 'Pagamento por transferência'
				ELSE CASE WHEN OVPM.BoeSum <> 0 THEN 'Pagamento por boleto'
				ELSE CASE WHEN OVPM.[CheckSum] <> 0 THEN 'Pagamento por cheque'
				ELSE NULL END END END END END
			FROM OPYM
			WHERE OPYM.PayMethCod = ISNULL(ISNULL(OBOE.PayMethCod, OVPM.PayMth), OPYM.PayMethCod)
	) AS OA_FORMA
	WHERE OVPM.Canceled = 'N'
		AND OA_CONTA.Conta IS NOT NULL
		AND VPM2.DocEntry = T0.DocEntry
		AND VPM2.InstId = T2.InstlmntID
		AND VPM2.InvType = T2.ObjType
) AS OA_PGTO
WHERE T2.InsTotal <> 0.00 AND T0.CANCELED = 'N'
	AND	(T0.DocDate >= @DtLancamentoIni OR @DtLancamentoIni IS NULL)
	AND (T0.DocDate <= @DtLancamentoFin OR @DtLancamentoFin IS NULL)
	AND (T2.DueDate >= @DtVencimentoIni OR @DtVencimentoIni IS NULL)
	AND (T2.DueDate <= @DtVencimentoFin OR @DtVencimentoFin IS NULL)

OPTION (RECOMPILE)


INSERT INTO #tmp_docs
SELECT DISTINCT
	T0.DocEntry AS [ChaveDoc],
	CAST(T0.DocEntry AS NVARCHAR(10)) AS [ChaveDocString],
	CAST(T0.Serial AS NVARCHAR(50)) AS [Numero],
	T2.InstlmntID AS [Parcela],
	CAST(T2.InstlmntID AS NVARCHAR(10)) + '/' + CAST(T0.Installmnt AS NVARCHAR(10)) AS [ParcelaString],
	T3.CardCode AS [CardCode],
	T3.CardName AS [CardName],
	ISNULL(T3.CardFName, T3.CardName) AS [CardFName],
	T3.CardCode + ' - ' + T3.CardName AS [CardCodeName],
	CASE WHEN ISNULL(T4.TaxId4, '') <> '' THEN T4.TaxId4 ELSE T4.TaxId0 END AS [CNPJ],
	T0.ObjType AS [TipoObj],
	'AT' AS [TipoDoc],
	'ODPO' AS [TabelaDoc],
	T0.DocDate AS [Emissao],
	T2.DueDate as [Vencimento],
	T2.InsTotal AS [ValorParcela],
	ISNULL(T0.DocCur, 'R$') AS [DocCur],
	ISNULL(T0.DocRate, 1) AS [DocRate],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.DocEntry ELSE NULL END AS [ChavePgto],
	CAST(CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.DocEntry ELSE NULL END AS NVARCHAR(10)) AS [ChavePgtoString],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.Pagamento ELSE NULL END AS [Pagamento],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.ObjType ELSE NULL END AS [TipoObjPgto],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN 'CP' ELSE NULL END AS [TipoPgto],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN 'OVPM' ELSE NULL END AS [TabelaPgto],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.SumApplied + OA_PGTO.UndOvDiff ELSE 0 END AS [ValorPago],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.UndOvDiff ELSE 0 END AS [JurosDesconto],	
	CASE WHEN (CASE WHEN T2.InsTotal - T2.Paid = 0 THEN 0 ELSE CASE WHEN OA_DEV.DevTotal = T0.DocTotal THEN 0 ELSE (T2.InsTotal - T2.Paid) - OA_DEV.DevTotal END END) = (CASE WHEN T2.Instotal - T2.Paid = 0 THEN 0 ELSE CASE WHEN ISNULL(OA_DEVS.DocTotal, 0) = T0.DocTotal THEN 0 ELSE (T2.Instotal - T2.Paid) - ISNULL(OA_DEVS.DocTotal, 0) END END) THEN (CASE WHEN T2.InsTotal - T2.Paid = 0 THEN 0 ELSE CASE WHEN OA_DEV.DevTotal = T0.DocTotal THEN 0 ELSE (T2.InsTotal - T2.Paid) - OA_DEV.DevTotal END END) ELSE (CASE WHEN T2.Instotal - T2.Paid = 0 THEN 0 ELSE CASE WHEN ISNULL(OA_DEVS.DocTotal, 0) = T0.DocTotal THEN 0 ELSE (T2.Instotal - T2.Paid) - ISNULL(OA_DEVS.DocTotal, 0) END END) END AS [ValorSaldo],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN ISNULL(OA_PGTO.DocCurr, 'R$') ELSE NULL END AS [PgtoCur],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN ISNULL(OA_PGTO.DocRate, 1) ELSE NULL END AS [PgtoRate],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.Descript ELSE NULL END AS [FormaPgto],
	T0.Comments AS [Comments],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.Conta ELSE NULL END AS [AcctCode],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.NomeConta ELSE NULL END AS [AcctName],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN 'F' ELSE 'A' END AS [DocStatus]
FROM ODPO T0
	INNER JOIN DPO1 T1 ON T0.DocEntry = T1.DocEntry AND ISNULL(T1.FreeChrgBP, 'N') = 'N'
	INNER JOIN DPO6 T2 ON T0.DocEntry = T2.DocEntry
	INNER JOIN OCRD T3 ON T0.CardCode = T3.CardCode
	LEFT  JOIN CRD7 T4 ON T3.CardCode = T4.CardCode AND T4.[Address] = '' AND T4.AddrType = 'S'
OUTER APPLY (
	SELECT DevTotal = ISNULL(SUM(DISTINCT ISNULL(RPC6.InsTotal, 0)), 0)/T0.Installmnt
	FROM ORPC
		INNER JOIN RPC1 ON ORPC.DocEntry = RPC1.DocEntry
		INNER JOIN RPC6 ON ORPC.DocEntry = RPC6.DocEntry
	WHERE RPC1.BaseEntry = T1.DocEntry AND RPC1.BaseType = T1.ObjType
) AS OA_DEV
OUTER APPLY (
	SELECT ORPC.DocTotal, ORPC.DocDate
	FROM ORPC
		INNER JOIN RPC1 ON ORPC.DocEntry = RPC1.DocEntry
	WHERE RPC1.BaseEntry = T1.DocEntry AND RPC1.BaseType = T1.ObjType
) AS OA_DEVS
OUTER APPLY (
	SELECT DISTINCT
		OVPM.DocEntry
		, OVPM.ObjType
		, VPM2.SumApplied + ISNULL((CASE WHEN OVPM.ObjType = '46' THEN 
			(SELECT DISTINCT (X0.Debit - X0.Credit) / OA_MAX.Countt FROM JDT1 X0 WHERE X0.TransId = OVPM.TransId AND X0.Account IN ('4.1.02.001.0040', '4.1.02.001.0050', '4.1.02.002.0060'))
		  END),0) AS SumApplied
		--, VPM2.SumApplied + (SELECT [dbo].fn_CVA_VariacaoCambialComissao(CONVERT(INT,OVPM.DocEntry), CONVERT(INT,OVPM.ObjType))) AS SumApplied
		, OVPM.UndOvDiff / OA_MAX.Countt AS UndOvDiff
		, OVPM.DocCurr
		, OVPM.DocRate
		, OA_DATA.Pagamento
		, OA_FORMA.Descript
		, OA_CONTA.Conta
		, OA_CONTA.NomeConta
	FROM OVPM
		INNER JOIN VPM2 ON VPM2.DocNum = OVPM.DocEntry
		LEFT  JOIN OBOE ON OVPM.BoeAbs = OBOE.BoeKey
		LEFT  JOIN OBOT ON OBOT.AbsEntry = (
			SELECT TOP 1 BOT1.AbsEntry FROM BOT1
			WHERE BOT1.BOENumber = OBOE.BoeNum AND BOT1.BoeType = OBOE.BoeType
			ORDER BY BOT1.AbsEntry DESC
		) AND OBOT.StatusTo = 'P'
		LEFT  JOIN OJDT ON OBOT.TransId = OJDT.Number
		LEFT  JOIN VPM1 ON OVPM.DocEntry = VPM1.DocNum
		LEFT  JOIN OCHO ON VPM1.CheckAbs = OCHO.CheckKey
	OUTER APPLY (
		SELECT Countt = COUNT(TX.InvoiceId) FROM VPM2 TX WHERE TX.DocNum = OVPM.DocEntry
	) AS OA_MAX
	OUTER APPLY (
		SELECT Pagamento = 
			CASE WHEN OBOT.StatusTo = 'P' THEN OJDT.RefDate
				ELSE CASE WHEN OCHO.CheckKey IS NOT NULL THEN OCHO.CheckDate
				ELSE CASE WHEN OVPM.BoeAbs IS NULL AND OVPM.DocEntry IS NOT NULL THEN OVPM.TaxDate
				ELSE NULL END END END
	) AS OA_DATA
	OUTER APPLY (
		SELECT Conta = OACT.AcctCode, NomeConta = OACT.AcctName
		FROM OACT
		WHERE OACT.Finanse = 'Y' AND OACT.AcctCode = 
			CASE WHEN OBOT.StatusTo = 'P' THEN OBOE.BoeAcct
				ELSE CASE WHEN OCHO.CheckKey IS NOT NULL THEN OCHO.CheckAcct
				ELSE CASE WHEN OVPM.BoeAbs IS NULL AND OVPM.DocEntry IS NOT NULL THEN
					CASE WHEN OVPM.TrsfrSum <> 0 THEN OVPM.TrsfrAcct ELSE OVPM.CashAcct END END END END
	) AS OA_CONTA
	OUTER APPLY (
		SELECT Descript = 
			CASE WHEN ISNULL(OBOE.PayMethCod, OVPM.PayMth) IS NOT NULL THEN OPYM.Descript
				ELSE CASE WHEN OVPM.CashSum <> 0 THEN 'Pagamento em dinheiro'
				ELSE CASE WHEN OVPM.TrsfrSum <> 0 THEN 'Pagamento por transferência'
				ELSE CASE WHEN OVPM.BoeSum <> 0 THEN 'Pagamento por boleto'
				ELSE CASE WHEN OVPM.[CheckSum] <> 0 THEN 'Pagamento por cheque'
				ELSE NULL END END END END END
			FROM OPYM
			WHERE OPYM.PayMethCod = ISNULL(ISNULL(OBOE.PayMethCod, OVPM.PayMth), OPYM.PayMethCod)
	) AS OA_FORMA
	WHERE OVPM.Canceled = 'N'
		AND OA_CONTA.Conta IS NOT NULL
		AND VPM2.DocEntry = T0.DocEntry
		AND VPM2.InstId = T2.InstlmntID
		AND VPM2.InvType = T2.ObjType
) AS OA_PGTO
WHERE T2.InsTotal <> 0.00 AND T0.CANCELED = 'N'
	AND	(T0.DocDate >= @DtLancamentoIni OR @DtLancamentoIni IS NULL)
	AND (T0.DocDate <= @DtLancamentoFin OR @DtLancamentoFin IS NULL)
	AND (T2.DueDate >= @DtVencimentoIni OR @DtVencimentoIni IS NULL)
	AND (T2.DueDate <= @DtVencimentoFin OR @DtVencimentoFin IS NULL)


OPTION (RECOMPILE)


INSERT INTO #tmp_docs
SELECT DISTINCT
	T0.DocEntry AS [ChaveDoc],
	CAST(T0.DocEntry AS NVARCHAR(10)) AS [ChaveDocString],
	CAST(T0.Serial AS NVARCHAR(50)) AS [Numero],
	T2.InstlmntID AS [Parcela],
	CAST(T2.InstlmntID AS NVARCHAR(10)) + '/' + CAST(T0.Installmnt AS NVARCHAR(10)) AS [ParcelaString],
	T3.CardCode AS [CardCode],
	T3.CardName AS [CardName],
	ISNULL(T3.CardFName, T3.CardName) AS [CardFName],
	T3.CardCode + ' - ' + T3.CardName AS [CardCodeName],
	CASE WHEN ISNULL(T4.TaxId4, '') <> '' THEN T4.TaxId4 ELSE T4.TaxId0 END AS [CNPJ],
	T0.ObjType AS [TipoObj],
	'DE' AS [TipoDoc],
	'ORPC' AS [TabelaDoc],
	T0.DocDate AS [Emissao],
	T2.DueDate as [Vencimento],
	-T2.InsTotal AS [ValorParcela],
	ISNULL(T0.DocCur, 'R$') AS [DocCur],
	ISNULL(T0.DocRate, 1) AS [DocRate],
	NULL AS [ChavePgto],
	NULL AS [ChavePgtoString],
	NULL AS [Pagamento],
	NULL AS [TipoObjPgto],
	NULL AS [TipoPgto],
	NULL AS [TabelaPgto],
	CASE WHEN T1.BaseType <> -1 THEN -T2.InsTotal ELSE 0 END AS [ValorPago],
	0 AS [JurosDesconto],
	CASE WHEN T1.BaseType <> -1 THEN 0 ELSE CASE WHEN T2.InsTotal - T2.Paid = 0 THEN 0 ELSE -(T2.InsTotal - T2.Paid) END END AS [ValorSaldo],
	NULL AS [PgtoCur],
	0 AS [PgtoRate],
	NULL AS [FormaPgto],
	T0.Comments AS [Comments],
	NULL AS [AcctCode],
	NULL AS [AcctName],
	CASE WHEN T1.BaseType <> -1 THEN 'C' ELSE CASE WHEN T2.InsTotal - T2.Paid = 0 THEN 'C' ELSE 'A' END END AS [DocStatus]
FROM ORPC T0
	INNER JOIN RPC1 T1 ON T0.DocEntry = T1.DocEntry AND ISNULL(T1.FreeChrgBP, 'N') = 'N'
	INNER JOIN RPC6 T2 ON T0.DocEntry = T2.DocEntry
	INNER JOIN OCRD T3 ON T0.CardCode = T3.CardCode
	LEFT  JOIN CRD7 T4 ON T3.CardCode = T4.CardCode AND T4.[Address] = '' AND T4.AddrType = 'S'
WHERE T2.InsTotal <> 0.00 AND T0.CANCELED = 'N'
	AND	(T0.DocDate >= @DtLancamentoIni OR @DtLancamentoIni IS NULL)
	AND (T0.DocDate <= @DtLancamentoFin OR @DtLancamentoFin IS NULL)
	AND (T2.DueDate >= @DtVencimentoIni OR @DtVencimentoIni IS NULL)
	AND (T2.DueDate <= @DtVencimentoFin OR @DtVencimentoFin IS NULL)

	
OPTION (RECOMPILE)


INSERT INTO #tmp_docs
SELECT DISTINCT
	T0.TransId AS [ChaveDoc],
	CAST(T0.TransId AS NVARCHAR(10)) AS [ChaveDocString],
	CASE WHEN ISNULL(T1.Ref1, '') = '' THEN CAST(T0.TransId AS NVARCHAR(10)) ELSE CAST(T1.Ref1 AS NVARCHAR(MAX)) END AS [Numero],
	T1.Line_ID AS [Parcela],
	CASE WHEN ISNULL(T1.Ref2, '') = '' THEN '1/1' ELSE CAST(T1.Ref2 AS NVARCHAR(MAX)) END AS [ParcelaString],
	T2.CardCode AS [CardCode],
	T2.CardName AS [CardName],
	ISNULL(T2.CardFName, T2.CardName) AS [CardFName],
	T2.CardCode + ' - ' + T2.CardName AS [CardCodeName],
	CASE WHEN ISNULL(T3.TaxId4, '') <> '' THEN T3.TaxId4 ELSE T3.TaxId0 END AS [CNPJ],
	T0.ObjType AS [TipoObj],
	'LC' AS [TipoDoc],
	'OJDT' AS [TabelaDoc],
	T0.RefDate AS [Emissao],
	T1.DueDate as [Vencimento],
	T1.Credit - T1.Debit AS [ValorParcela],
	ISNULL(T1.FCCurrency, 'R$') AS [DocCur],
	CAST(
		CASE WHEN T1.FCCurrency IS NOT NULL THEN
			CASE WHEN T1.FCDebit <> 0 THEN T1.Debit / T1.FCDebit
			ELSE
				CASE WHEN T1.FCCredit <> 0 THEN T1.Credit / T1.FCCredit
				ELSE 0 END
			END
		ELSE 1 END
	AS NUMERIC(19,6))  AS [DocRate],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.DocEntry ELSE NULL END AS [ChavePgto],
	CAST(CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.DocEntry ELSE NULL END AS NVARCHAR(10)) AS [ChavePgtoString],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.Pagamento when (select count(*) from itr1 where transid = T0.TransId) >= 1 then (select top 1 recondate 
from oitr A inner join itr1 B on A.reconnum = B.reconnum
where B.transid = T0.TransId) ELSE NULL END AS [Pagamento],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.ObjType ELSE NULL END AS [TipoObjPgto],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN 'CP' ELSE NULL END AS [TipoPgto],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN 'OVPM' ELSE NULL END AS [TabelaPgto],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.SumApplied + OA_PGTO.UndOvDiff when (select count(*) from itr1 where transid = T0.TransId) >= 1 then (select top 1 reconsum from itr1 where transid = T0.TransId) ELSE 0 END AS [ValorPago],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.UndOvDiff ELSE 0 END AS [JurosDesconto],
	T1.BalDueCred - T1.BalDueDeb AS [ValorSaldo],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN ISNULL(OA_PGTO.DocCurr, 'R$') ELSE NULL END AS [PgtoCur],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN ISNULL(OA_PGTO.DocRate, 1) ELSE NULL END AS [PgtoRate],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.Descript ELSE NULL END AS [FormaPgto],
	CAST(T1.LineMemo AS NVARCHAR(MAX)) AS [Comments],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.Conta ELSE NULL END AS [AcctCode],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN OA_PGTO.NomeConta ELSE NULL END AS [AcctName],
	CASE WHEN OA_PGTO.Pagamento IS NOT NULL THEN 'F' when (select count(*) from itr1 where transid = T0.TransId) >= 1 then 'F' ELSE 'A' END AS [DocStatus]
FROM OJDT T0
	INNER JOIN JDT1 T1 ON T0.TransId = T1.TransId AND T1.TransType in ('30', '243000002',  '243000003')
	INNER JOIN OCRD T2 ON T1.ShortName = T2.CardCode AND T2.CardType = 'S'
	LEFT  JOIN CRD7 T3 ON T2.CardCode = T3.CardCode AND T3.[Address] = '' AND T3.AddrType = 'S'
OUTER APPLY (
	SELECT DISTINCT
		OVPM.DocEntry
		, OVPM.ObjType
		, VPM2.SumApplied + ISNULL((CASE WHEN OVPM.ObjType = '46' THEN 
			(SELECT DISTINCT (X0.Debit - X0.Credit) / OA_MAX.Countt FROM JDT1 X0 WHERE X0.TransId = OVPM.TransId AND X0.Account IN ('4.1.02.001.0040', '4.1.02.001.0050', '4.1.02.002.0060'))
		  END),0) AS SumApplied
		--, VPM2.SumApplied + (SELECT [dbo].fn_CVA_VariacaoCambialComissao(CONVERT(INT,OVPM.DocEntry), CONVERT(INT,OVPM.ObjType))) AS SumApplied
		, OVPM.UndOvDiff / OA_MAX.Countt AS UndOvDiff
		, OVPM.DocCurr
		, OVPM.DocRate
		, OA_DATA.Pagamento
		, OA_FORMA.Descript
		, OA_CONTA.Conta
		, OA_CONTA.NomeConta
	FROM OVPM
		INNER JOIN VPM2 ON VPM2.DocNum = OVPM.DocEntry
		LEFT  JOIN OBOE ON OVPM.BoeAbs = OBOE.BoeKey
		LEFT  JOIN OBOT ON OBOT.AbsEntry = (
			SELECT TOP 1 BOT1.AbsEntry FROM BOT1
			WHERE BOT1.BOENumber = OBOE.BoeNum AND BOT1.BoeType = OBOE.BoeType
			ORDER BY BOT1.AbsEntry DESC
		) AND OBOT.StatusTo = 'P'
		LEFT  JOIN OJDT ON OBOT.TransId = OJDT.Number
		LEFT  JOIN VPM1 ON OVPM.DocEntry = VPM1.DocNum
		LEFT  JOIN OCHO ON VPM1.CheckAbs = OCHO.CheckKey
	OUTER APPLY (
		SELECT Countt = COUNT(TX.InvoiceId) FROM VPM2 TX WHERE TX.DocNum = OVPM.DocEntry
	) AS OA_MAX
	OUTER APPLY (
		SELECT Pagamento = 
			CASE WHEN OBOT.StatusTo = 'P' THEN OJDT.RefDate
				ELSE CASE WHEN OCHO.CheckKey IS NOT NULL THEN OCHO.CheckDate
				ELSE CASE WHEN OVPM.BoeAbs IS NULL AND OVPM.DocEntry IS NOT NULL THEN OVPM.TaxDate
				ELSE NULL END END END
	) AS OA_DATA
	OUTER APPLY (
		SELECT Conta = OACT.AcctCode, NomeConta = OACT.AcctName
		FROM OACT
		WHERE OACT.Finanse = 'Y' AND OACT.AcctCode = 
			CASE WHEN OBOT.StatusTo = 'P' THEN OBOE.BoeAcct
				ELSE CASE WHEN OCHO.CheckKey IS NOT NULL THEN OCHO.CheckAcct
				ELSE CASE WHEN OVPM.BoeAbs IS NULL AND OVPM.DocEntry IS NOT NULL THEN
					CASE WHEN OVPM.TrsfrSum <> 0 THEN OVPM.TrsfrAcct ELSE OVPM.CashAcct END END END END
	) AS OA_CONTA
	OUTER APPLY (
		SELECT Descript = 
			CASE WHEN ISNULL(OBOE.PayMethCod, OVPM.PayMth) IS NOT NULL THEN OPYM.Descript
				ELSE CASE WHEN OVPM.CashSum <> 0 THEN 'Pagamento em dinheiro'
				ELSE CASE WHEN OVPM.TrsfrSum <> 0 THEN 'Pagamento por transferência'
				ELSE CASE WHEN OVPM.BoeSum <> 0 THEN 'Pagamento por boleto'
				ELSE CASE WHEN OVPM.[CheckSum] <> 0 THEN 'Pagamento por cheque'
				ELSE NULL END END END END END
			FROM OPYM
			WHERE OPYM.PayMethCod = ISNULL(ISNULL(OBOE.PayMethCod, OVPM.PayMth), OPYM.PayMethCod)
	) AS OA_FORMA
	WHERE OVPM.Canceled = 'N'
		AND OA_CONTA.Conta IS NOT NULL
		AND VPM2.DocEntry = T0.TransId
		AND VPM2.DocLine = T1.Line_ID
		AND VPM2.InvType = T1.TransType
) AS OA_PGTO
WHERE T0.StornoToTr IS NULL
	AND T0.TransId NOT IN (SELECT OJDT.StornoToTr FROM OJDT WHERE OJDT.StornoToTr = T0.TransId)
	AND	(T0.RefDate >= @DtLancamentoIni OR @DtLancamentoIni IS NULL)
	AND (T0.RefDate <= @DtLancamentoFin OR @DtLancamentoFin IS NULL)
	AND (T1.DueDate >= @DtVencimentoIni OR @DtVencimentoIni IS NULL)
	AND (T1.DueDate <= @DtVencimentoFin OR @DtVencimentoFin IS NULL)

OPTION (RECOMPILE)

INSERT INTO #tmp_docs
SELECT DISTINCT
	T0.DocEntry AS [ChaveDoc],
	CAST(T0.DocEntry AS NVARCHAR(10)) AS [ChaveDocString],
	CAST(T0.DocEntry AS NVARCHAR(50)) AS [Numero],
	1 AS [Parcela],
	'1/1' AS [ParcelaString],
	ISNULL(T2.AcctCode, T1.CardCode) AS [CardCode],
	ISNULL(T2.AcctName, T1.CardName) AS [CardName],
	ISNULL(T2.AcctName, ISNULL(T1.CardFName, T1.CardName)) AS [CardFName],
	ISNULL(T2.AcctCode, T1.CardCode) + ' - ' + ISNULL(T2.AcctName, T1.CardName) AS [CardCodeName],
	CASE WHEN ISNULL(T9.TaxId4, '') <> '' THEN T9.TaxId4 ELSE T9.TaxId0 END AS [CNPJ],
	T0.ObjType AS [TipoObj],
	'CP' AS [TipoDoc],
	'OVPM' AS [TabelaDoc],
	T0.DocDate AS [Emissao],
	T0.DocDueDate as [Vencimento],
	ISNULL(T8.SumApplied, T0.DocTotal) AS [ValorParcela],
	ISNULL(T0.DocCurr, 'R$') AS [DocCur],
	ISNULL(T0.DocRate, 1) AS [DocRate],
	T0.DocEntry AS [ChavePgto],
	CAST(T0.DocEntry AS NVARCHAR(10)) AS [ChavePgtoString],
	OA_DATA.Pagamento AS [Pagamento],
	T0.ObjType AS [TipoObjPgto],
	'CP' AS [TipoPgto],
	'OVPM' AS [TabelaPgto],
	ISNULL(T8.SumApplied, T0.DocTotal) + T0.UndOvDiff AS [ValorPago],
	T0.UndOvDiff AS [JurosDesconto],
	0.00 AS [ValorSaldo],
	ISNULL(T0.DocCurr, 'R$') AS [PgtoCur],
	ISNULL(T0.DocRate, 1) AS [PgtoRate],
	OA_FORMA.Descript AS [FormaPgto],
	T0.Comments AS [Comments],
	OA_CONTA.Conta AS [AcctCode],
	OA_CONTA.NomeConta AS [AcctName],
	'F' AS [DocStatus]
FROM OVPM T0
	LEFT JOIN OCRD T1 ON T0.CardCode = T1.CardCode
	LEFT JOIN OACT T2 ON T0.CardCode = T2.AcctCode
	LEFT JOIN OBOE T3 ON T0.BoeAbs = T3.BoeKey
	LEFT JOIN OBOT T4 ON T4.AbsEntry =
	(
		SELECT TOP 1 BOT1.AbsEntry FROM BOT1
		WHERE BOT1.BOENumber = T3.BoeNum AND BOT1.BoeType = T3.BoeType
		ORDER BY BOT1.AbsEntry DESC
	) AND T4.StatusTo = 'P'
	LEFT JOIN OJDT T5 ON T4.TransId = T5.Number
	LEFT JOIN VPM1 T6 ON T0.DocEntry = T6.DocNum
	LEFT JOIN OCHO T7 ON T6.CheckAbs = T7.CheckKey
	LEFT JOIN VPM4 T8 ON T0.DocEntry = T8.DocNum
	LEFT JOIN CRD7 T9 ON T1.CardCode = T9.CardCode ANd T9.[Address] = '' ANd T9.AddrType = 'S'
OUTER APPLY (
	SELECT Pagamento = 
		CASE WHEN T4.StatusTo = 'P' THEN T5.RefDate
			ELSE CASE WHEN T7.CheckKey IS NOT NULL THEN T7.CheckDate
			ELSE CASE WHEN T0.BoeAbs IS NULL AND T0.DocEntry IS NOT NULL THEN T0.TaxDate
			ELSE NULL END END END
) AS OA_DATA
OUTER APPLY (
	SELECT Conta = OACT.AcctCode, NomeConta = OACT.AcctName
	FROM OACT
	WHERE OACT.Finanse = 'Y' AND OACT.AcctCode = 
		CASE WHEN T4.StatusTo = 'P' THEN T3.BoeAcct
			ELSE CASE WHEN T7.CheckKey IS NOT NULL THEN T7.CheckAcct 
			ELSE CASE WHEN T0.BoeAbs IS NULL AND T0.DocEntry IS NOT NULL THEN 
				CASE WHEN T0.TrsfrSum <> 0 THEN T0.TrsfrAcct
				ELSE T0.CashAcct END END
			END
		END
) AS OA_CONTA
OUTER APPLY (
	SELECT Descript = 
		CASE WHEN ISNULL(T3.PayMethCod, T0.PayMth) IS NOT NULL THEN OPYM.Descript
			ELSE CASE WHEN T0.CashSum <> 0 THEN 'Pagamento em dinheiro'
			ELSE CASE WHEN T0.TrsfrSum <> 0 THEN 'Pagamento por transferência'
			ELSE CASE WHEN T0.BoeSum <> 0 THEN 'Pagamento por boleto'
			ELSE CASE WHEN T0.[CheckSum] <> 0 THEN 'Pagamento por cheque'
			ELSE NULL END END END END
		END
	FROM OPYM WHERE OPYM.PayMethCod = ISNULL(ISNULL(T3.PayMethCod, T0.PayMth), OPYM.PayMethCod)
) AS OA_FORMA
WHERE T0.NoDocSum <> 0 AND T0.Canceled = 'N' ANd OA_CONTA.Conta IS NOT NULL
	AND (
		SELECT COUNT(OACT.AcctCode)
		FROM JDT1 INNER JOIN OACT ON JDt1.Account = OACT.AcctCode AND OACT.Finanse = 'Y'
		WHERE JDT1.TransId = T0.TransId AND JDt1.TransType = T0.ObjType
	) <> 2
	AND	(T0.DocDate >= @DtLancamentoIni OR @DtLancamentoIni IS NULL)
	AND (T0.DocDate <= @DtLancamentoFin OR @DtLancamentoFin IS NULL)
	AND (T0.DocDueDate >= @DtVencimentoIni OR @DtVencimentoIni IS NULL)
	AND (T0.DocDueDate <= @DtVencimentoFin OR @DtVencimentoFin IS NULL)

OPTION (RECOMPILE)

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
		CASE WHEN ISNULL(TX.DocStatus, '') = 'C' THEN 'C' ELSE CASE WHEN ISNULL(TX.ValorSaldo, 0) = 0 THEN 'F' ELSE 'A' END END
	FROM #tmp_docs TX

UPDATE #tmp_return SET DocStatus = CASE WHEN ISNULL(TipoPgto, '') <> '' THEN 'F' ELSE DocStatus END

IF @Provisoes = 'Y'
BEGIN
	INSERT INTO #tmp_return
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
			CASE WHEN ISNULL(ValorDoc, 0) < 0 THEN ValorDoc*-1 ELSE ValorDoc END,
			DocCur,
			DocRate,
			ValorDocME,
			ChavePgto,
			ChavePgtoString,
			Pagamento,
			TipoObjPgto,
			TipoPgto,
			TabelaPgto,
			ValorPago,
			JurosDesconto,
			CASE WHEN ISNULL(ValorSaldo, 0) < 0 THEN ValorSaldo*-1 ELSE ValorSaldo END,
			PgtoCur,
			PgtoRate,
			ValorPagoME,
			FormaPgto,
			Comments,
			AcctCode,
			AcctName,
			DocStatus
		FROM [dbo].[fn_CVA_ProvisoesFluxoCaixa](@DtLancamentoIni,@DtLancamentoFin, @DtVencimentoIni,@DtVencimentoFin)
		WHERE 
			((Emissao >= @DtLancamentoIni OR @DtLancamentoIni IS NULL) AND (Emissao <= @DtLancamentoIni OR @DtLancamentoFin IS NULL)
			 AND 
			(Vencimento >= @DtVencimentoIni OR @DtVencimentoIni IS NULL) AND (Vencimento <= @DtVencimentoFin OR @DtVencimentoFin IS NULL))
		and CardCode LIKE 'F%'

		--INSERT INTO #tmp_return
		--exec [SpcCVAFluxoCaixaLancamentosRecorrentes] /*'2018-06-05','2018-12-05','2018-06-05','2018-12-05'*/ @DtLancamentoIni,@DtLancamentoFin,@DtVencimentoIni,@DtVencimentoFin


END

SELECT DISTINCT 
	ChaveDoc,
	ChaveDocString,
	CASE WHEN ISNULL(Numero, '') = '' THEN ChaveDocString ELSE Numero END AS Numero,
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
	CASE WHEN ISNULL(ValorSaldo, 0) = 0 AND ISNULL(ValorPago, 0) = 0 THEN ISNULL(ValorDoc, 0) ELSE ISNULL(ValorPago, 0) END AS ValorPago,
	ISNULL(JurosDesconto, 0) AS JurosDesconto,
	ISNULL(ValorSaldo, 0) AS ValorSaldo,
	PgtoCur,
	PgtoRate,
	ValorPagoME,
	FormaPgto,
	Comments,
	AcctCode,
	AcctName,
	CASE WHEN ValorSaldo = 0 AND DocStatus = 'A' THEN 'F' ELSE DocStatus END AS DocStatus,
	DiffDias = 
		CASE WHEN DocStatus = 'C' THEN 0
		ELSE CASE WHEN DocStatus = 'F' THEN DATEDIFF(DAY, ISNULL(Pagamento, Vencimento), Vencimento)
		ELSE DATEDIFF(DAY, GETDATE(), Vencimento) END END
FROM #tmp_return 
WHERE (DocStatus = @Tipo OR @Tipo = 'T')
ORDER BY DocStatus, ValorSaldo

DROP TABLE #tmp_docs
DROP TABLE #tmp_return

END
GO

