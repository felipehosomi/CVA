IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_DIME_SAIDA')
	DROP PROCEDURE SP_CVA_DIME_SAIDA
GO
CREATE PROCEDURE [dbo].[SP_CVA_DIME_SAIDA]
(
	@Code		NVARCHAR(50),
	@ST_Total	INT
)
AS
BEGIN
	SELECT 
		ODLN.CardCode,
		DLN1.CFOPCode	CFOP,
		DLN12.CityS		Cidade,
		DLN12.StateS	UF,
		DLN1.LineTotal	+ DLN1.VatSum + ISNULL(0, 0) + ISNULL(DistribSum, 0) ValorContabil,
		ISNULL(ICMS.BaseSum,0)	BaseCalculo,
		ISNULL(ICMS.TaxSum,0)		ImpostoCreditado,
		ISNULL(ICMS.Isentas,0) Isentas,
		ISNULL(ICMS.Outras,0) Outras,
		ISNULL(ICMS_ST.TaxSum,0)		ImpostoRetido,
		ISNULL(ICMS_ST.BaseSum,0)	BaseCalculoImpostoRetido
	FROM [@CVA_DIME] DIME WITH(NOLOCK)
		INNER JOIN ODLN WITH(NOLOCK)
			ON ODLN.BPLId = DIME.U_Filial OR DIME.U_Filial = 0
		INNER JOIN OBPL WITH(NOLOCK)
			ON OBPL.BPLId = ODLN.BPLId
		INNER JOIN DLN1 WITH(NOLOCK)
			ON DLN1.DocEntry = ODLN.DocEntry
		INNER JOIN DLN12 WITH(NOLOCK)
			ON DLN12.DocEntry = ODLN.DocEntry
		LEFT JOIN DLN2 WITH(NOLOCK)
			ON DLN2.DocEntry = DLN1.DocEntry
			AND DLN2.LineNum = DLN1.LineNum
		LEFT JOIN (
					SELECT DLN4.DocEntry, DLN4.LineNum, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM DLN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
					GROUP BY DLN4.DocEntry, DLN4.LineNum
				) ICMS
			ON ICMS.DocEntry = DLN1.DocEntry
			AND ICMS.LineNum = DLN1.LineNum
		LEFT JOIN (
					SELECT DLN4.DocEntry, DLN4.LineNum, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM DLN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
					GROUP BY DLN4.DocEntry, DLN4.LineNum
				) ICMS_ST
			ON ICMS_ST.DocEntry = DLN1.DocEntry	AND ICMS_ST.LineNum = DLN1.LineNum 
			AND ((DLN12.[State] = OBPL.[State] OR @ST_Total = 1) AND DLN1.CFOPCODE='5401') 
	WHERE DIME.Code = @Code
	AND ODLN.DocDate BETWEEN DIME.U_DtDe AND DIME.U_DtAte
	AND ODLN.CANCELED = 'N'
	AND ODLN.Model NOT IN (0, 28, 37, 3)

	UNION ALL

	SELECT 
		OINV.CardCode,
		INV1.CFOPCode	CFOP,
		INV12.CityS		Cidade,
		INV12.StateS	UF,+.
		INV1.LineTotal	+ INV1.VatSum + ISNULL(0, 0) + ISNULL(DistribSum, 0) ValorContabil,
		ISNULL(ICMS.BaseSum,0)	BaseCalculo,
		ISNULL(ICMS.TaxSum,0)		ImpostoCreditado,
		ISNULL(ICMS.Isentas,0) Isentas,
		ISNULL(ICMS.Outras,0) Outras,
		ISNULL(ICMS_ST.TaxSum,0)		ImpostoRetido,
		ISNULL(ICMS_ST.BaseSum,0)	BaseCalculoImpostoRetido
	FROM [@CVA_DIME] DIME WITH(NOLOCK)
		INNER JOIN OINV WITH(NOLOCK)
			ON OINV.BPLId = DIME.U_Filial OR DIME.U_Filial = 0
		INNER JOIN OBPL WITH(NOLOCK)
			ON OBPL.BPLId = OINV.BPLId
		INNER JOIN INV1 WITH(NOLOCK)
			ON INV1.DocEntry = OINV.DocEntry
		INNER JOIN INV12 WITH(NOLOCK)
			ON INV12.DocEntry = OINV.DocEntry
		LEFT JOIN INV2 WITH(NOLOCK)
			ON INV2.DocEntry = INV1.DocEntry
			AND INV2.LineNum = INV1.LineNum
		LEFT JOIN (
					SELECT INV4.DocEntry, INV4.LineNum, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM INV4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
					GROUP BY INV4.DocEntry, INV4.LineNum
				) ICMS
			ON ICMS.DocEntry = INV1.DocEntry
			AND ICMS.LineNum = INV1.LineNum
		LEFT JOIN (
					SELECT INV4.DocEntry, INV4.LineNum, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM INV4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
					GROUP BY INV4.DocEntry, INV4.LineNum
				) ICMS_ST
			ON ICMS_ST.DocEntry = INV1.DocEntry
			AND ICMS_ST.LineNum = INV1.LineNum
			AND ((INV12.[State] = OBPL.[State] OR @ST_Total = 1) AND INV1.CFOPCODE='5401') 
	WHERE DIME.Code = @Code
	AND OINV.DocDate BETWEEN DIME.U_DtDe AND DIME.U_DtAte
	AND OINV.CANCELED = 'N'
	AND OINV.Model NOT IN (0, 28, 37, 3)

	UNION ALL

	SELECT 
		ORPC.CardCode,
		RPC1.CFOPCode	CFOP,
		RPC12.CityS		Cidade,
		RPC12.StateS	UF,
		RPC1.LineTotal	+ RPC1.VatSum + ISNULL(0, 0) + ISNULL(DistribSum, 0) ValorContabil,
		ISNULL(ICMS.BaseSum,0)	BaseCalculo,
		ISNULL(ICMS.TaxSum,0)		ImpostoCreditado,
		ISNULL(ICMS.Isentas,0),
		ISNULL(ICMS.Outras,0),
		ISNULL(ICMS_ST.TaxSum,0)		ImpostoRetido,
		ISNULL(ICMS_ST.BaseSum,0)	BaseCalculoImpostoRetido
	FROM [@CVA_DIME] DIME WITH(NOLOCK)
		INNER JOIN ORPC WITH(NOLOCK)
			ON ORPC.BPLId = DIME.U_Filial OR DIME.U_Filial = 0
		INNER JOIN OBPL WITH(NOLOCK)
			ON OBPL.BPLId = ORPC.BPLId
		INNER JOIN RPC1 WITH(NOLOCK)
			ON RPC1.DocEntry = ORPC.DocEntry
		LEFT JOIN RPC2 WITH(NOLOCK)
			ON RPC2.DocEntry = RPC1.DocEntry
			AND RPC2.LineNum = RPC1.LineNum
		INNER JOIN RPC12 WITH(NOLOCK)
			ON RPC12.DocEntry = ORPC.DocEntry
		LEFT JOIN (
					SELECT RPC4.DocEntry, RPC4.LineNum, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM RPC4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
					GROUP BY RPC4.DocEntry, RPC4.LineNum
				) ICMS
			ON ICMS.DocEntry = RPC1.DocEntry
			AND ICMS.LineNum = RPC1.LineNum
		LEFT JOIN (
					SELECT RPC4.DocEntry, RPC4.LineNum, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM RPC4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
					GROUP BY RPC4.DocEntry, RPC4.LineNum
				) ICMS_ST
			ON ICMS_ST.DocEntry = RPC1.DocEntry
			AND ICMS_ST.LineNum = RPC1.LineNum
			AND ((RPC12.[State] = OBPL.[State] OR @ST_Total = 1) AND RPC1.CFOPCODE='5401') 
	WHERE DIME.Code = @Code
	AND ORPC.DocDate BETWEEN DIME.U_DtDe AND DIME.U_DtAte
	AND ORPC.CANCELED = 'N'
	AND ORPC.Model NOT IN (0, 28, 37, 3)

	UNION ALL

	SELECT 
		ORPD.CardCode,
		RPD1.CFOPCode	CFOP,
		RPD12.CityS		Cidade,
		RPD12.StateS	UF,
		RPD1.LineTotal	+ RPD1.VatSum + ISNULL(0, 0) + ISNULL(DistribSum, 0) ValorContabil,
		ISNULL(ICMS.BaseSum,0)	BaseCalculo,
		ISNULL(ICMS.TaxSum,0)		ImpostoCreditado,
		ISNULL(ICMS.Isentas,0),
		ISNULL(ICMS.Outras,0),
		ISNULL(ICMS_ST.TaxSum,0)		ImpostoRetido,
		ISNULL(ICMS_ST.BaseSum,0)	BaseCalculoImpostoRetido
	FROM [@CVA_DIME] DIME WITH(NOLOCK)
		INNER JOIN ORPD WITH(NOLOCK)
			ON ORPD.BPLId = DIME.U_Filial OR DIME.U_Filial = 0
		INNER JOIN OBPL WITH(NOLOCK)
			ON OBPL.BPLId = ORPD.BPLId
		INNER JOIN RPD1 WITH(NOLOCK)
			ON RPD1.DocEntry = ORPD.DocEntry
		LEFT JOIN RPD2 WITH(NOLOCK)
			ON RPD2.DocEntry = RPD1.DocEntry
			AND RPD2.LineNum = RPD1.LineNum
		INNER JOIN RPD12 WITH(NOLOCK)
			ON RPD12.DocEntry = ORPD.DocEntry
		LEFT JOIN (
					SELECT RPD4.DocEntry, RPD4.LineNum, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM RPD4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
					GROUP BY RPD4.DocEntry, RPD4.LineNum
				) ICMS
			ON ICMS.DocEntry = RPD1.DocEntry
			AND ICMS.LineNum = RPD1.LineNum
		LEFT JOIN (
					SELECT RPD4.DocEntry, RPD4.LineNum, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM RPD4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
					GROUP BY RPD4.DocEntry, RPD4.LineNum
				) ICMS_ST
			ON ICMS_ST.DocEntry = RPD1.DocEntry
			AND ICMS_ST.LineNum = RPD1.LineNum
			AND ((RPD12.[State] = OBPL.[State] OR @ST_Total = 1) AND RPD1.CFOPCODE='5401') 
	WHERE DIME.Code = @Code
	AND ORPD.DocDate BETWEEN DIME.U_DtDe AND DIME.U_DtAte
	AND ORPD.CANCELED = 'N'
	AND ORPD.Model NOT IN (0, 28, 37, 3)

END
