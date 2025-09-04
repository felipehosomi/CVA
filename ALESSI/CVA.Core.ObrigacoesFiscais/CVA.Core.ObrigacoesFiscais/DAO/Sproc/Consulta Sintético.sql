;WITH Impostos AS
	(
		SELECT DISTINCT
			OPCH.DocEntry,
			OPCH.DocNum,
			OPCH.DocType,
			OPCH.CANCELED,
			OPCH.ObjType,
			OPCH.DocDate,
			OPCH.DocDueDate,
			OPCH.CardCode,
			OPCH.CardName,
			CASE WHEN ISNULL(PCH12.TaxId0, '') <> ''
				THEN PCH12.TaxId0
				ELSE PCH12.TaxId4
			END CnpjCpf,
			PCH12.TaxId1,
			PCH12.AddrTypeS,
			PCH12.StreetS,
			PCH12.StreetNoS,
			PCH12.BlockS,
			PCH12.CityS,
			PCH12.ZipCodeS,
			PCH12.StateS,
			PCH12.CountyS,
			PCH12.CountryS,
			OPCH.VatSum,
			OPCH.DiscPrcnt,
			OPCH.DiscSum,
			OPCH.DiscSumFC,
			OPCH.DocTotal,
			OPCH.Comments,
			OPCH.JrnlMemo,
			OPCH.TransId,
			OPCH.TotalExpns,
			OPCH.WTSum,
			OPCH.BPLId,
			OPCH.BPLName,
			OBPL.TaxIdNum CNPJ_FIlial,
			OBPL.TaxIdNum2 IE_Filial,
			OBPLMain.TaxIdNum	CNPJ_Matriz,
			OBPLMain.TaxIdNum2	IE_Matriz,
			OPCH.SeqCode,
			OPCH.Serial,
			OPCH.SeriesStr,
			OPCH.SubStr,
			OPCH.Model,
			OPCH.TaxOnExp,
			OPCH.Branch,
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
			INNER JOIN PCH12 WITH(NOLOCK)
				ON PCH12.DocEntry = OPCH.DocEntry
			INNER JOIN OBPL WITH(NOLOCK)
				ON OBPL.BPLId = OPCH.BPLId
			INNER JOIN OBPL OBPLMain WITH(NOLOCK)
				ON OBPLMain.MainBPL = 'Y'
			LEFT JOIN (
					SELECT PCH4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM PCH4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
					GROUP BY PCH4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) ICMS
				ON ICMS.DocEntry = OPCH.DocEntry
			LEFT JOIN (
					SELECT PCH4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM PCH4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
					GROUP BY PCH4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) ICMS_ST
				ON ICMS_ST.DocEntry = OPCH.DocEntry
			LEFT JOIN (
					SELECT PCH4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM PCH4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
					GROUP BY PCH4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) IPI
				ON IPI.DocEntry = OPCH.DocEntry
			LEFT JOIN (
					SELECT PCH4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM PCH4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'PIS'
					GROUP BY PCH4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) PIS
				ON PIS.DocEntry = OPCH.DocEntry
			LEFT JOIN (
					SELECT PCH4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM PCH4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'COFINS'
					GROUP BY PCH4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) COFINS
				ON COFINS.DocEntry = OPCH.DocEntry
			LEFT JOIN (
					SELECT PCH4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM PCH4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY PCH4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) ISS
				ON ISS.DocEntry = OPCH.DocEntry
	)
	, ImpostosRetidos AS
	(
		SELECT * FROM
		(
			SELECT 
			Impostos.*,
			PCH5.WTAmnt,
			PCH5.WTType
			FROM Impostos
				LEFT JOIN
				(
					SELECT PCH5.AbsEntry, SUM(PCH5.WTAmnt) AS WTAmnt, OWTT.WTType
					FROM PCH5 WITH(NOLOCK)
						INNER JOIN OWHT WITH(NOLOCK) ON OWHT.WTCode = PCH5.WtCode
						INNER JOIN OWTT WITH(NOLOCK) ON OWTT.WTTypeId = OWHT.WTTypeId
					GROUP BY PCH5.AbsEntry, OWTT.WTType
				) PCH5
				ON PCH5.AbsEntry = Impostos.DocEntry
		) AS P
		PIVOT
		(
			SUM(P.WTAmnt)
			FOR P.WTType IN ([IRRF], [PIS], [COFINS], [ISS], [CSLL], [CRFS])
		) AS PVT
	)
	SELECT * FROM ImpostosRetidos
	ORDER BY DocEntry

	