IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_GIA_ST')
	DROP PROCEDURE SP_CVA_GIA_ST
GO
CREATE PROCEDURE SP_CVA_GIA_ST
(
	@DocKey NVARCHAR(30)
)
AS
BEGIN
	CREATE TABLE #EC8715
	(
		BPLName		NVARCHAR(255),
		IEFilial	NVARCHAR(255),
		DataVenc	DATETIME,
		DataDe		DATETIME,
		UF			NVARCHAR(255),
		ICMS		NUMERIC(19, 2),
		FCP			NUMERIC(19, 2)
	)

	INSERT INTO #EC8715
	EXEC SP_CVA_GIA_ST_EC8715 @DocKey

	SELECT 
		OBPL.TaxIdNum				CNPJ,
		OBPL.TaxIdNum2				IE,
		CASE WHEN GIAST.U_Sem_Mov = 'Y'
			THEN 'SEM MOVIMENTO'
			ELSE ''
		END							Sem_Mov,
		CASE WHEN GIAST.U_Retificacao = 'Y'
			THEN 'GIA-ST DE RETIFICAÇÃO'
			ELSE 'GIA-ST NORMAL'
		END							Tipo_Gia,
		GIAST.U_DtDe				DataDe,
		GIAST.U_DtAte				DateAte,
		GIAST.U_DtVenc				Vencimento,
		GIAST.U_Ressarcimento		Ressarcimento,
		GIAST.U_Credito				Credito,
		GIAST.U_Antecipado			Antecipado,
		CASE WHEN GIAST.U_Petroleo = 'Y'
			THEN 'Sim'
			ELSE 'Não'
		END							Petroleo,
		CASE WHEN GIAST.U_Efet_Transf = 'Y'
			THEN 'Sim'
			ELSE 'Não'
		END							Efetuou_Transf,
		CASE WHEN GIAST.U_EC_8715 = 'Y'
			THEN 'Sim'
			ELSE 'Não'
		END							EC_8715,
		CAST(GIAST.U_Obs AS NVARCHAR(MAX)) Obs,
		OCST.Code					UF,
		OCST.Name					UF_Desc,
		SUM(INV1.LineTotal)			Total,
		SUM(IPI.TaxSum)				IPI,
		SUM(EXPENSE.ExpenseTotal)	ExpenseTotal,
		SUM(ICMS.BaseSum)			ICMS_Base,
		SUM(ICMS.TaxSum)			ICMS,
		SUM(ICMS_ST.BaseSum)		ICMS_ST_Base,
		SUM(ICMS_ST.TaxSum)			ICMS_ST,
		SUM(ICMSDest.TaxSum)		ICMSDest,
		SUM(OBTA.VlIcmsSc)			Adiantamento,
		SUM(ICMSDest.TaxSum)		TotalICMSDest,
		ISNULL(SUM(ICMSSTRetido.TaxSum), 0)	ICMS_ST_Retido,
		ISNULL((SELECT SUM(ICMS) FROM #EC8715), 0) ICMS_EC8715,
		0.0							ICMS_Combust,
		0.0							Cred_ProxPeriodo,
		0.0							FECOP_Estorno,
		ISNULL(SUM(ORIN.TaxSum), 0) ICMS_Dev,
		--ISNULL(SUM(ORIN.TaxSum), 0) + 
		--ISNULL(SUM(ORDN.TaxSum), 0)	ICMS_Dev,
		SUM(FCP.TaxSum)				FCP,
		OBPL.BPLName,
		OADM.Phone1,
		OBPL.[Address],
		OBPL.City,
		OBPL.ZipCode,
		OHEM.firstName + ' ' + OHEM.lastName Nome,
		OHEM.CPF,
		OHPS.name					Cargo,
		OHEM.officeTel				Phone,
		OHEM.email					Email,
		OHEM.fax					Fax
	FROM [@CVA_GIA_ST] GIAST WITH(NOLOCK)
		INNER JOIN OBPL WITH(NOLOCK)
			ON OBPL.BPLId = GIAST.U_Filial
		INNER JOIN OADM WITH(NOLOCK)
			ON 1 = 1
		INNER JOIN OCST WITH(NOLOCK)
			ON OCST.Code = GIAST.U_UF
		LEFT JOIN OHEM WITH(NOLOCK)
			ON OHEM.userId = GIAST.U_Declarante
		LEFT JOIN OHPS WITH(NOLOCK)
			ON OHPS.posID = OHEM.position
		INNER JOIN OINV WITH(NOLOCK)
			ON OINV.BPLId = GIAST.U_Filial 
			AND OINV.DocDate BETWEEN GIAST.U_DtDe AND GIAST.U_DtAte
			AND OINV.CANCELED = 'N'
		INNER JOIN INV12 WITH(NOLOCK)
			ON INV12.DocEntry = OINV.DocEntry
			AND INV12.[State] = OCST.Code
		INNER JOIN INV1 WITH(NOLOCK)
			ON INV1.DocEntry = OINV.DocEntry
		INNER JOIN (
				SELECT INV4.DocEntry, INV4.LineNum, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum FROM INV4 WITH(NOLOCK)
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
				GROUP BY INV4.DocEntry, INV4.LineNum
			) ICMS_ST
			ON ICMS_ST.DocEntry = OINV.DocEntry
			AND ICMS_ST.LineNum = INV1.LineNum
		LEFT JOIN (
				SELECT INV4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum FROM INV4 WITH(NOLOCK)
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY INV4.DocEntry
			) ICMS
			ON ICMS.DocEntry = OINV.DocEntry
		LEFT JOIN (
				SELECT INV4.DocEntry, INV4.LineNum, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum FROM INV4 WITH(NOLOCK)
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
				GROUP BY INV4.DocEntry, INV4.LineNum
			) IPI
			ON IPI.DocEntry = OINV.DocEntry
			AND IPI.LineNum = INV1.LineNum
		LEFT JOIN (
				SELECT INV4.DocEntry, INV4.LineNum, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum FROM INV4 WITH(NOLOCK)
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMSDest'
				GROUP BY INV4.DocEntry, INV4.LineNum
			) ICMSDest
			ON ICMSDest.DocEntry = OINV.DocEntry
			AND ICMSDest.LineNum = INV1.LineNum
		LEFT JOIN (
				SELECT INV4.DocEntry, INV4.LineNum, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum FROM INV4 WITH(NOLOCK)
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'FCP'
				GROUP BY INV4.DocEntry, INV4.LineNum
			) FCP
			ON FCP.DocEntry = OINV.DocEntry
			AND FCP.LineNum = INV1.LineNum
		LEFT JOIN
		(
			SELECT INV5.AbsEntry, INV5.LineNum, SUM(WTAmnt) TaxSum, SUM(TaxbleAmnt) BaseSum FROM INV5
				INNER JOIN OWHT ON OWHT.WTCode = INV5.WTCode
				INNER JOIN OWTT ON OWTT.WTTypeId = OWHT.WTTypeId AND OWTT.WTType = 'ICMS-ST'
			GROUP BY  INV5.AbsEntry, INV5.LineNum
		) ICMSSTRetido 
		ON ICMSSTRetido.AbsEntry = OINV.DocEntry
		AND ICMSSTRetido.LineNum = INV1.LineNum
		LEFT JOIN (
				SELECT INV13.DocEntry, SUM(INV13.LineTotal) ExpenseTotal FROM INV13 WITH(NOLOCK)
				GROUP BY INV13.DocEntry
			) EXPENSE
			ON EXPENSE.DocEntry = OINV.DocEntry
		LEFT JOIN (
			SELECT ORIN.DocDate, SUM(RIN4.TaxSum) TaxSum FROM ORIN WITH(NOLOCK)
				INNER JOIN RIN4 WITH(NOLOCK) ON RIN4.DocEntry = ORIN.DocEntry
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY DocDate
		) ORIN
		ON ORIN.DocDate BETWEEN GIAST.U_DtDe AND GIAST.U_DtAte
		--LEFT JOIN (
		--	SELECT ORDN.DocDate, SUM(RDN4.TaxSum) TaxSum FROM ORDN WITH(NOLOCK)
		--		INNER JOIN RDN4 WITH(NOLOCK) ON RDN4.DocEntry = ORDN.DocEntry
		--		INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
		--		INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
		--	GROUP BY DocDate
		--) ORDN
		--ON ORDN.DocDate BETWEEN GIAST.U_DtDe AND GIAST.U_DtAte
		LEFT JOIN OBTA WITH(NOLOCK)
			ON OBTA.RefDocEnt = OINV.DocEntry
			AND OBTA.RefObjType = OINV.ObjType
	GROUP BY
		OBPL.TaxIdNum,
		OBPL.TaxIdNum2,
		GIAST.U_Sem_Mov,
		GIAST.U_Retificacao,
		GIAST.U_DtDe,
		GIAST.U_DtAte,
		GIAST.U_DtVenc,
		GIAST.U_Ressarcimento,
		GIAST.U_Credito,
		GIAST.U_Antecipado,
		GIAST.U_Petroleo,
		GIAST.U_Efet_Transf,
		GIAST.U_EC_8715,
		CAST(GIAST.U_Obs AS NVARCHAR(MAX)),
		OCST.Code,
		OCST.Name,
		OBPL.BPLName,
		OADM.Phone1,
		OBPL.[Address],
		OBPL.City,
		OBPL.ZipCode,
		OHEM.firstName,
		OHEM.lastName,
		OHEM.CPF,
		OHPS.name,
		OHEM.officeTel,
		OHEM.email,
		OHEM.fax
END