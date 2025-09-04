IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_ADIANTAMENTO_FORNECEDOR')
	DROP PROCEDURE SP_CVA_ADIANTAMENTO_FORNECEDOR
GO
CREATE PROCEDURE [dbo].[SP_CVA_ADIANTAMENTO_FORNECEDOR]
(
	@DocNum		INT,
	@NrRef		NVARCHAR(50),
	@DataDe		DATETIME,
	@DataAte	DATETIME,
	@Fornecedor	NVARCHAR(MAX),
	@Filial		INT,
	@Saldo		NVARCHAR(1),
	@Conta		NVARCHAR(100)
)
AS
BEGIN
	IF @DocNum = 0
	BEGIN
		SET @DocNum = NULL
	END
	IF @NrRef = '0'
	BEGIN
		SET @NrRef = NULL
	END

	IF @Fornecedor IS NULL OR @Fornecedor = '*'
	BEGIN
		SET @Fornecedor = ''
	END
	IF @Filial = 0
	BEGIN
		SET @Filial = NULL
	END
	IF @Conta = '*'
	BEGIN
		SET @Conta = NULL
	END

	SELECT DISTINCT
		ODPO.DocNum,
		OCRD.CardCode,
		OCRD.CardName,
		ODPO.U_CVANREF NrRef,
		ODPO.DocDate,
		ISNULL(UFD1.Descr, 'Sem Status') Origem,
		OBPL.BPLName,
		ODPO.DocTotal,
		ISNULL(VPM2.SumApplied, 0.00) Pago,
		ODPO.DocTotal - ISNULL(VPM2.SumApplied, 0.00) Saldo
	FROM ODPO WITH(NOLOCK)
		INNER JOIN DPO1 WITH(NOLOCK)
			ON DPO1.DocEntry = ODPO.DocEntry
		INNER JOIN OBPL WITH(NOLOCK)
			ON OBPL.BPLId = ODPO.BPLId
		INNER JOIN OCRD WITH(NOLOCK)
			ON OCRD.CardCode = ODPO.CardCode
		INNER JOIN CRD7 WITH(NOLOCK)
			ON CRD7.CardCode = OCRD.CardCode
		LEFT JOIN VPM2 WITH(NOLOCK)
			ON VPM2.DocEntry = ODPO.DocEntry
			AND VPM2.InvType = ODPO.ObjType
		LEFT JOIN CUFD WITH(NOLOCK)
			ON TableID = 'ODPO'
			AND AliasID = 'CVAORI'
		LEFT JOIN UFD1 WITH(NOLOCK)
			ON UFD1.TableID = CUFD.TableID
			AND UFD1.FieldID = CUFD.FieldID
			AND UFD1.FldValue = ODPO.U_CVAORI
	WHERE ODPO.DocNum				= ISNULL(@DocNum, ODPO.DocNum)
	AND ODPO.DocDate BETWEEN ISNULL(@DataDe, DATEADD(YEAR, -100, GETDATE())) AND ISNULL(@DataAte, DATEADD(YEAR, 100, GETDATE()))
	AND ISNULL(ODPO.U_CVANREF, '')	= ISNULL(@DocNum, ISNULL(ODPO.U_CVANREF, ''))
	AND ODPO.BPLId					= ISNULL(@Filial, ODPO.BPLId)
	AND DPO1.AcctCode				= ISNULL(@Conta, DPO1.AcctCode)
	AND 
	(
		OCRD.CardCode				LIKE '%' + @Fornecedor + '%'
		OR OCRD.CardName			LIKE '%' + @Fornecedor + '%'
		OR OCRD.CardFName			LIKE '%' + @Fornecedor + '%'
		OR CRD7.TaxId0				LIKE '%' + @Fornecedor + '%'
	)
	AND
	(
		(ODPO.DocTotal - ISNULL(VPM2.SumApplied, 0.00) > 0 AND @Saldo = 'C')
		OR (ODPO.DocTotal - ISNULL(VPM2.SumApplied, 0.00) = 0 AND @Saldo = 'S')
		OR @Saldo = 'T'
	)
END