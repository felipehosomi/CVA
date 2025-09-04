;WITH Impostos AS
(
	SELECT DISTINCT
		OPCH.DocEntry,
		OPCH.ObjType,
		PCH1.LineNum,
		PCH1.ItemCode,
		PCH1.Dscription,
		PCH1.Quantity,
		PCH1.Price,
		PCH1.DiscPrcnt,
		PCH1.LineTotal,
		PCH1.WhsCode,
		PCH1.AcctCode,
		PCH1.TotalSumSy,
		PCH1.VatSum,
		PCH1.FinncPriod,
		PCH1.DistribSum,
		PCH1.TaxCode,
		PCH1.TaxType,
		PCH1.CFOPCode,
		PCH1.CSTCode,
		PCH1.Usage,
		PCH1.TaxOnly,
		PCH1.CSTfIPI,
		PCH1.CSTfPIS,
		PCH1.CSTfCOFINS,
		ICMS.TaxSum			ICMS_NR,
		ICMS.BaseSum		ICMS_BC,
		ICMS.U_Isento		ICMS_Isento,
		ICMS.U_Outros		ICMS_Outros,
		ICMS_ST.TaxSum		ICMS_ST,
		ICMS_ST.BaseSum		ICMS_ST_BC,
		ICMS_ST.U_Isento	ICMS_ST_Isento,
		ICMS_ST.U_Outros	ICMS_ST_Outros,
		IPI.TaxSum			IPI_NR,
		IPI.BaseSum			IPI_BC,
		IPI.NonDdctPrc		IPI_NaoDedutivel,
		IPI.U_Isento		IPI_Isento,
		IPI.U_Outros		IPI_Outros,
		PIS.TaxSum			PIS_NR,
		PIS.BaseSum			PIS_BC,
		PIS.U_Isento		PIS_Isento,
		PIS.U_Outros		PIS_Outros,
		COFINS.TaxSum		COFINS_NR,
		COFINS.BaseSum		COFINS_BC,
		COFINS.U_Isento		COFINS_Isento,
		COFINS.U_Outros		COFINS_Outros,
		ISS.TaxSum			ISS_NR,
		ISS.BaseSum			ISS_BC,
		ISS.U_Isento		ISS_Isento,
		ISS.U_Outros		ISS_Outros
	FROM OPCH WITH(NOLOCK)
		INNER JOIN PCH1 WITH(NOLOCK)
			ON PCH1.DocEntry = OPCH.DocEntry
		LEFT JOIN (
			SELECT PCH4.* FROM PCH4 WITH(NOLOCK)
				INNER JOIN OSTT WITH(NOLOCK)
					ON OSTT.AbsId = PCH4.StaType
				INNER JOIN ONFT WITH(NOLOCK)
					ON ONFT.AbsId = OSTT.NfTaxId
					AND ONFT.Code = 'ICMS'
			) ICMS
			ON ICMS.DocEntry = PCH1.DocEntry
			AND ICMS.LineNum = PCH1.LineNum
		LEFT JOIN (
			SELECT PCH4.* FROM PCH4 WITH(NOLOCK)
				INNER JOIN OSTT WITH(NOLOCK)
					ON OSTT.AbsId = PCH4.StaType
				INNER JOIN ONFT WITH(NOLOCK)
					ON ONFT.AbsId = OSTT.NfTaxId
					AND ONFT.Code = 'ICMS-ST'
			) ICMS_ST
			ON ICMS_ST.DocEntry = PCH1.DocEntry
			AND ICMS_ST.LineNum = PCH1.LineNum
		LEFT JOIN (
			SELECT PCH4.* FROM PCH4 WITH(NOLOCK)
				INNER JOIN OSTT WITH(NOLOCK)
					ON OSTT.AbsId = PCH4.StaType
				--INNER JOIN OSTA WITH(NOLOCK)
				--	ON OSTA.Code = PCH4.StaCode
				--	AND OSTA.[Type] = OSTT.AbsId
				INNER JOIN ONFT WITH(NOLOCK)
					ON ONFT.AbsId = OSTT.NfTaxId
					AND ONFT.Code = 'IPI'
			) IPI
			ON IPI.DocEntry = PCH1.DocEntry
			AND IPI.LineNum = PCH1.LineNum
		LEFT JOIN (
			SELECT PCH4.* FROM PCH4 WITH(NOLOCK)
				INNER JOIN OSTT WITH(NOLOCK)
					ON OSTT.AbsId = PCH4.StaType
				INNER JOIN ONFT WITH(NOLOCK)
					ON ONFT.AbsId = OSTT.NfTaxId
					AND ONFT.Code = 'PIS'
			) PIS
			ON PIS.DocEntry = PCH1.DocEntry
			AND PIS.LineNum = PCH1.LineNum
		LEFT JOIN (
			SELECT PCH4.* FROM PCH4 WITH(NOLOCK)
				INNER JOIN OSTT WITH(NOLOCK)
					ON OSTT.AbsId = PCH4.StaType
				INNER JOIN ONFT WITH(NOLOCK)
					ON ONFT.AbsId = OSTT.NfTaxId
					AND ONFT.Code = 'COFINS'
			) COFINS
			ON COFINS.DocEntry = PCH1.DocEntry
			AND COFINS.LineNum = PCH1.LineNum
		LEFT JOIN (
			SELECT PCH4.* FROM PCH4 WITH(NOLOCK)
				INNER JOIN OSTT WITH(NOLOCK)
					ON OSTT.AbsId = PCH4.StaType
				INNER JOIN ONFT WITH(NOLOCK)
					ON ONFT.AbsId = OSTT.NfTaxId
					AND ONFT.Code IN ('ISS', 'ISSQN')
			) ISS
			ON ISS.DocEntry = PCH1.DocEntry
			AND ISS.LineNum = PCH1.LineNum
)
, ImpostosRetidos AS
(
	SELECT * FROM
	(
		SELECT 
		Impostos.*,
		PCH5.WTAmnt,
		OWTT.WTType
		FROM Impostos
			LEFT JOIN PCH5 WITH(NOLOCK)
				ON PCH5.AbsEntry = Impostos.DocEntry
				AND PCH5.LineNum = Impostos.LineNum		
			LEFT JOIN OWHT WITH(NOLOCK)
				ON OWHT.WTCode = PCH5.WtCode
			LEFT JOIN OWTT WITH(NOLOCK)
				ON OWTT.WTTypeId = OWHT.WTTypeId
	) AS P
	PIVOT
	(
		SUM(P.WTAmnt)
		FOR P.WTType IN ([IRRF], [PIS], [COFINS], [ISS], [CSLL], [CRFS])
	) AS PVT
)
SELECT * FROM ImpostosRetidos
ORDER BY DocEntry