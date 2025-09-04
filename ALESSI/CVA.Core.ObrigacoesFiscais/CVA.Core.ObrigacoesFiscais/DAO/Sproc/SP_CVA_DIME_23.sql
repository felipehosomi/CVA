--EXEC SP_CVA_DIME_23 1
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_DIME_23')
	DROP PROCEDURE SP_CVA_DIME_23
GO
CREATE PROCEDURE SP_CVA_DIME_23
(
	@BPLId INT
)
AS
BEGIN
	SELECT 
		CASE WHEN ISNULL(INV1.DocEntry, 0) <> 0
			THEN INV1.CFOPCode
			ELSE DLN1.CFOPCode
		END	CFOP,
		CASE WHEN ISNULL(INV1.DocEntry, 0) <> 0
			THEN INV1.LineTotal
			ELSE DLN1.LineTotal
		END					ValorContabil,
		CASE WHEN ISNULL(INV1.DocEntry, 0) <> 0
			THEN INV1.VatSum
			ELSE DLN1.VatSum
		END					ImpostoCreditado,
		CASE WHEN ISNULL(INV1.DocEntry, 0) <> 0
			THEN INV5.WTAmnt
			ELSE DLN5.WTAmnt
		END					ImpostoRetido
	INTO #DIME
	FROM [@CVA_DIME] DIME WITH(NOLOCK)
		INNER JOIN ODLN WITH(NOLOCK)
			ON ODLN.BPLId = DIME.U_Filial
		INNER JOIN DLN1 WITH(NOLOCK)
			ON DLN1.DocEntry = ODLN.DocEntry
		LEFT JOIN (
					SELECT DLN5.AbsEntry, DLN5.LineNum, SUM(DLN5.WTAmnt) WTAmnt
					FROM DLN5 WITH(NOLOCK) GROUP BY DLN5.AbsEntry, DLN5.LineNum
				) DLN5
			ON DLN5.AbsEntry = DLN1.DocEntry
			AND DLN5.LineNum = DLN1.LineNum
		LEFT JOIN INV1 WITH(NOLOCK)
			ON INV1.BaseEntry = DLN1.DocEntry
			AND INV1.BaseLine = DLN1.LineNum
			AND INV1.BaseType = ODLN.ObjType
		LEFT JOIN (
				SELECT INV5.AbsEntry, INV5.LineNum, SUM(INV5.WTAmnt) WTAmnt
				FROM INV5 WITH(NOLOCK) GROUP BY INV5.AbsEntry, INV5.LineNum
			) INV5
		ON INV5.AbsEntry = INV1.DocEntry
		AND INV5.LineNum = INV1.LineNum
	WHERE DIME.U_Filial = @BPLId	
	AND ODLN.DocDate BETWEEN DIME.U_DtDe AND DIME.U_DtAte
	
	INSERT INTO #DIME
	SELECT 
		RPD1.CFOPCode	CFOP,
		RPD1.LineTotal	ValorContabil,
		RPD1.VatSum		ImpostoCreditado,
		RPD5.WTAmnt		ImpostoRetido
	FROM [@CVA_DIME] DIME WITH(NOLOCK)
		INNER JOIN ORPD WITH(NOLOCK)
			ON ORPD.BPLId = DIME.U_Filial
		INNER JOIN RPD1 WITH(NOLOCK)
			ON RPD1.DocEntry = ORPD.DocEntry
		LEFT JOIN (
					SELECT RPD5.AbsEntry, RPD5.LineNum, SUM(RPD5.WTAmnt) WTAmnt, RPD5.TaxbleAmnt
					FROM RPD5 WITH(NOLOCK) GROUP BY RPD5.AbsEntry, RPD5.LineNum, RPD5.TaxbleAmnt
				) RPD5
			ON RPD5.AbsEntry = RPD1.DocEntry
			AND RPD5.LineNum = RPD1.LineNum
	WHERE DIME.U_Filial = @BPLId	
	AND ORPD.DocDate BETWEEN DIME.U_DtDe AND DIME.U_DtAte

	INSERT INTO #DIME
	SELECT 
		RPC1.CFOPCode	CFOP,
		RPC1.LineTotal	ValorContabil,
		RPC1.VatSum		ImpostoCreditado,
		RPC5.WTAmnt		ImpostoRetido
	FROM [@CVA_DIME] DIME WITH(NOLOCK)
		INNER JOIN ORPC WITH(NOLOCK)
			ON ORPC.BPLId = DIME.U_Filial
		INNER JOIN RPC1 WITH(NOLOCK)
			ON RPC1.DocEntry = ORPC.DocEntry
		LEFT JOIN (
					SELECT RPC5.AbsEntry, RPC5.LineNum, SUM(RPC5.WTAmnt) WTAmnt, RPC5.TaxbleAmnt
					FROM RPC5 WITH(NOLOCK) GROUP BY RPC5.AbsEntry, RPC5.LineNum, RPC5.TaxbleAmnt
				) RPC5
			ON RPC5.AbsEntry = RPC1.DocEntry
			AND RPC5.LineNum = RPC1.LineNum
	WHERE DIME.U_Filial = @BPLId	
	AND ORPC.DocDate BETWEEN DIME.U_DtDe AND DIME.U_DtAte

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
