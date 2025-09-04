--EXEC SP_CVA_DIME_22 1
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_DIME_22')
	DROP PROCEDURE SP_CVA_DIME_22
GO
CREATE PROCEDURE SP_CVA_DIME_22
(
	@BPLId INT
)
AS
BEGIN
	SELECT 
		CASE WHEN ISNULL(PCH1.DocEntry, 0) <> 0
			THEN PCH1.CFOPCode
			ELSE PDN1.CFOPCode
		END	CFOP,
		CASE WHEN ISNULL(PCH1.DocEntry, 0) <> 0
			THEN PCH1.LineTotal
			ELSE PDN1.LineTotal
		END					ValorContabil,
		CASE WHEN ISNULL(PCH1.DocEntry, 0) <> 0
			THEN PCH1.VatSum
			ELSE PDN1.VatSum
		END					ImpostoCreditado,
		CASE WHEN ISNULL(PCH1.DocEntry, 0) <> 0
			THEN PCH5.WTAmnt
			ELSE PDN5.WTAmnt
		END					ImpostoRetido
	INTO #DIME
	FROM [@CVA_DIME] DIME WITH(NOLOCK)
		INNER JOIN OPDN WITH(NOLOCK)
			ON OPDN.BPLId = DIME.U_Filial
		INNER JOIN PDN1 WITH(NOLOCK)
			ON PDN1.DocEntry = OPDN.DocEntry
		LEFT JOIN (
					SELECT PDN5.AbsEntry, PDN5.LineNum, SUM(PDN5.WTAmnt) WTAmnt
					FROM PDN5 WITH(NOLOCK) GROUP BY PDN5.AbsEntry, PDN5.LineNum
				) PDN5
			ON PDN5.AbsEntry = PDN1.DocEntry
			AND PDN5.LineNum = PDN1.LineNum
		LEFT JOIN PCH1 WITH(NOLOCK)
			ON PCH1.BaseEntry = PDN1.DocEntry
			AND PCH1.BaseLine = PDN1.LineNum
			AND PCH1.BaseType = OPDN.ObjType
		LEFT JOIN (
				SELECT PCH5.AbsEntry, PCH5.LineNum, SUM(PCH5.WTAmnt) WTAmnt
				FROM PCH5 WITH(NOLOCK) GROUP BY PCH5.AbsEntry, PCH5.LineNum
			) PCH5
		ON PCH5.AbsEntry = PCH1.DocEntry
		AND PCH5.LineNum = PCH1.LineNum
	WHERE DIME.U_Filial = @BPLId	
	AND OPDN.DocDate BETWEEN DIME.U_DtDe AND DIME.U_DtAte
	
	INSERT INTO #DIME
	SELECT 
		RDN1.CFOPCode	CFOP,
		RDN1.LineTotal	ValorContabil,
		RDN1.VatSum		ImpostoCreditado,
		RDN5.WTAmnt		ImpostoRetido
	FROM [@CVA_DIME] DIME WITH(NOLOCK)
		INNER JOIN ORDN WITH(NOLOCK)
			ON ORDN.BPLId = DIME.U_Filial
		INNER JOIN RDN1 WITH(NOLOCK)
			ON RDN1.DocEntry = ORDN.DocEntry
		LEFT JOIN (
					SELECT RDN5.AbsEntry, RDN5.LineNum, SUM(RDN5.WTAmnt) WTAmnt, RDN5.TaxbleAmnt
					FROM RDN5 WITH(NOLOCK) GROUP BY RDN5.AbsEntry, RDN5.LineNum, RDN5.TaxbleAmnt
				) RDN5
			ON RDN5.AbsEntry = RDN1.DocEntry
			AND RDN5.LineNum = RDN1.LineNum
	WHERE DIME.U_Filial = @BPLId	
	AND ORDN.DocDate BETWEEN DIME.U_DtDe AND DIME.U_DtAte

	INSERT INTO #DIME
	SELECT 
		RIN1.CFOPCode	CFOP,
		RIN1.LineTotal	ValorContabil,
		RIN1.VatSum		ImpostoCreditado,
		RIN5.WTAmnt		ImpostoRetido
	FROM [@CVA_DIME] DIME WITH(NOLOCK)
		INNER JOIN ORIN WITH(NOLOCK)
			ON ORIN.BPLId = DIME.U_Filial
		INNER JOIN RIN1 WITH(NOLOCK)
			ON RIN1.DocEntry = ORIN.DocEntry
		LEFT JOIN (
					SELECT RIN5.AbsEntry, RIN5.LineNum, SUM(RIN5.WTAmnt) WTAmnt, RIN5.TaxbleAmnt
					FROM RIN5 WITH(NOLOCK) GROUP BY RIN5.AbsEntry, RIN5.LineNum, RIN5.TaxbleAmnt
				) RIN5
			ON RIN5.AbsEntry = RIN1.DocEntry
			AND RIN5.LineNum = RIN1.LineNum
	WHERE DIME.U_Filial = @BPLId	
	AND ORIN.DocDate BETWEEN DIME.U_DtDe AND DIME.U_DtAte

	SELECT
		CFOP,
		SUM(DIME.ValorContabil)									[Valor Contábil],
		SUM(DIME.ValorContabil)	- SUM(DIME.ImpostoCreditado)	[Base Cálculo],
		SUM(DIME.ImpostoCreditado)								[Imposto Creditado],
		SUM(DIME.ValorContabil)									[Base Imposto Retido],
		SUM(DIME.ImpostoRetido)									[Imposto Retido]
	FROM #DIME DIME
	GROUP BY
		CFOP

	DROP TABLE #DIME
END
