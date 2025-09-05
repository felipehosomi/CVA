IF EXISTS (SELECT * FROM sys.objects WHERE type = 'IF' AND name = 'FN_CVA_SEF_E120')
	DROP FUNCTION FN_CVA_SEF_E120
GO
CREATE FUNCTION FN_CVA_SEF_E120
(
	@Filial			INT,
	@DataInicial	DATETIME,
	@DataFinal		DATETIME
) RETURNS TABLE
AS
RETURN
(
	SELECT DISTINCT
		CASE WHEN PDN1.CFOPCode LIKE '1%' OR PDN1.CFOPCode LIKE '2%' OR PDN1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END									IND_OPER,
		0									IND_EMIT,
		OPDN.CardCode						COD_PART,
		ONFM.NfmCode						COD_MOD,
		'00'								COD_SIT,
		OPDN.SeriesStr						SER,
		OPDN.SubStr							SUB,
		OPDN.Serial							NUM_DOC,
		OPDN.U_chaveacesso					CHV_NFE,
		OPDN.TaxDate						DT_EMIS,
		OPDN.DocDate						DT_DOC,
		OUSG.U_CVA_ICMS						COP,
		PDN1.Usage							COD_NAT,
		CASE WHEN OPDN.Installmnt = 1
			THEN 0
			ELSE 1
		END									IND_PGTO,
		PDN1.LineTotal						VL_CONT,
		PDN1.CFOPCode						CFOP,
		ISNULL(ICMS.BaseSum,0.00)			VL_BC_ICMS,
		ISNULL(ICMS.TaxSum,0.00)			VL_ICMS,
		ISNULL(ICMS.TaxRate,0.00)			ALIQ_ICMS,
		ISNULL(ICMS_ST.TaxSum,0.00)			VL_ICMS_ST,	
		ICMS_ST.TaxSum		VL_AT,
		ICMS.Isentas		VL_ISNT_ICMS,
		ICMS.Outras			VL_OUT_ICMS,
		''					COD_INF_OBS
	FROM OPDN WITH(NOLOCK)
		INNER JOIN PDN1 WITH(NOLOCK)
			ON PDN1.DocEntry = OPDN.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = OPDN.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = PDN1.Usage

		LEFT JOIN (
				SELECT PDN4.DocEntry, PDN4.TaxRate, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
				FROM PDN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY PDN4.DocEntry, PDN4.TaxRate
			) ICMS
			ON ICMS.DocEntry = PDN1.DocEntry
		LEFT JOIN (
			SELECT PDN4.DocEntry, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
			FROM PDN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY PDN4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = PDN1.DocEntry
	WHERE OPDN.DocDate BETWEEN @DataInicial AND @DataFinal
	AND OPDN.BPLId = @Filial
	AND ISNULL(PDN1.TargetType, 0) <> 18
	AND OPDN.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	UNION ALL

	SELECT DISTINCT
		CASE WHEN PCH1.CFOPCode LIKE '1%' OR PCH1.CFOPCode LIKE '2%' OR PCH1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END									IND_OPER,
		0									IND_EMIT,
		OPCH.CardCode						COD_PART,
		ONFM.NfmCode						COD_MOD,
		'00'								COD_SIT,
		OPCH.SeriesStr						SER,
		OPCH.SubStr							SUB,
		OPCH.Serial							NUM_DOC,
		OPCH.U_chaveacesso					CHV_NFE,
		OPCH.TaxDate						DT_EMIS,
		OPCH.DocDate						DT_DOC,
		OUSG.U_CVA_ICMS						COP,
		PCH1.Usage							COD_NAT,
		CASE WHEN OPCH.Installmnt = 1
			THEN 0
			ELSE 1
		END									IND_PGTO,
		PCH1.LineTotal						VL_CONT,
		PCH1.CFOPCode						CFOP,
		ISNULL(ICMS.BaseSum,0.00)			VL_BC_ICMS,
		ISNULL(ICMS.TaxSum,0.00)			VL_ICMS,
		ISNULL(ICMS.TaxRate,0.00)			ALIQ_ICMS,
		ISNULL(ICMS_ST.TaxSum,0.00)			VL_ICMS_ST,	
		ICMS_ST.TaxSum		VL_AT,
		ICMS.Isentas		VL_ISNT_ICMS,
		ICMS.Outras			VL_OUT_ICMS,
		''					COD_INF_OBS
	FROM OPCH WITH(NOLOCK)
		INNER JOIN PCH1 WITH(NOLOCK)
			ON PCH1.DocEntry = OPCH.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = OPCH.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = PCH1.Usage

		LEFT JOIN (
				SELECT PCH4.DocEntry, PCH4.TaxRate, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
				FROM PCH4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY PCH4.DocEntry, PCH4.TaxRate
			) ICMS
			ON ICMS.DocEntry = PCH1.DocEntry
		LEFT JOIN (
			SELECT PCH4.DocEntry, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
			FROM PCH4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY PCH4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = PCH1.DocEntry
	WHERE OPCH.DocDate BETWEEN @DataInicial AND @DataFinal
	AND OPCH.BPLId = @Filial
	AND OPCH.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	UNION ALL

	SELECT DISTINCT
		CASE WHEN RDN1.CFOPCode LIKE '1%' OR RDN1.CFOPCode LIKE '2%' OR RDN1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END									IND_OPER,
		0									IND_EMIT,
		ORDN.CardCode						COD_PART,
		ONFM.NfmCode						COD_MOD,
		'00'								COD_SIT,
		ORDN.SeriesStr						SER,
		ORDN.SubStr							SUB,
		ORDN.Serial							NUM_DOC,
		ORDN.U_chaveacesso					CHV_NFE,
		ORDN.TaxDate						DT_EMIS,
		ORDN.DocDate						DT_DOC,
		OUSG.U_CVA_ICMS						COP,
		RDN1.Usage							COD_NAT,
		CASE WHEN ORDN.Installmnt = 1
			THEN 0
			ELSE 1
		END									IND_PGTO,
		RDN1.LineTotal						VL_CONT,
		RDN1.CFOPCode						CFOP,
		ISNULL(ICMS.BaseSum,0.00)			VL_BC_ICMS,
		ISNULL(ICMS.TaxSum,0.00)			VL_ICMS,
		ISNULL(ICMS.TaxRate,0.00)			ALIQ_ICMS,
		ISNULL(ICMS_ST.TaxSum,0.00)			VL_ICMS_ST,	
		ICMS_ST.TaxSum		VL_AT,
		ICMS.Isentas		VL_ISNT_ICMS,
		ICMS.Outras			VL_OUT_ICMS,
		''					COD_INF_OBS
	FROM ORDN WITH(NOLOCK)
		INNER JOIN RDN1 WITH(NOLOCK)
			ON RDN1.DocEntry = ORDN.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = ORDN.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = RDN1.Usage

		LEFT JOIN (
				SELECT RDN4.DocEntry, RDN4.TaxRate, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
				FROM RDN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RDN4.DocEntry, RDN4.TaxRate
			) ICMS
			ON ICMS.DocEntry = RDN1.DocEntry
		LEFT JOIN (
			SELECT RDN4.DocEntry, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
			FROM RDN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RDN4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = RDN1.DocEntry
	WHERE ORDN.DocDate BETWEEN @DataInicial AND @DataFinal
	AND ORDN.BPLId = @Filial
	AND ORDN.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	UNION ALL

	SELECT DISTINCT
		CASE WHEN RIN1.CFOPCode LIKE '1%' OR RIN1.CFOPCode LIKE '2%' OR RIN1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END									IND_OPER,
		0									IND_EMIT,
		ORIN.CardCode						COD_PART,
		ONFM.NfmCode						COD_MOD,
		'00'								COD_SIT,
		ORIN.SeriesStr						SER,
		ORIN.SubStr							SUB,
		ORIN.Serial							NUM_DOC,
		ORIN.U_chaveacesso					CHV_NFE,
		ORIN.TaxDate						DT_EMIS,
		ORIN.DocDate						DT_DOC,
		OUSG.U_CVA_ICMS						COP,
		RIN1.Usage							COD_NAT,
		CASE WHEN ORIN.Installmnt = 1
			THEN 0
			ELSE 1
		END									IND_PGTO,
		RIN1.LineTotal						VL_CONT,
		RIN1.CFOPCode						CFOP,
		ISNULL(ICMS.BaseSum,0.00)			VL_BC_ICMS,
		ISNULL(ICMS.TaxSum,0.00)			VL_ICMS,
		ISNULL(ICMS.TaxRate,0.00)			ALIQ_ICMS,
		ISNULL(ICMS_ST.TaxSum,0.00)			VL_ICMS_ST,	
		ICMS_ST.TaxSum		VL_AT,
		ICMS.Isentas		VL_ISNT_ICMS,
		ICMS.Outras			VL_OUT_ICMS,
		''					COD_INF_OBS
	FROM ORIN WITH(NOLOCK)
		INNER JOIN RIN1 WITH(NOLOCK)
			ON RIN1.DocEntry = ORIN.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = ORIN.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = RIN1.Usage

		LEFT JOIN (
				SELECT RIN4.DocEntry, RIN4.TaxRate, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
				FROM RIN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RIN4.DocEntry, RIN4.TaxRate
			) ICMS
			ON ICMS.DocEntry = RIN1.DocEntry
		LEFT JOIN (
			SELECT RIN4.DocEntry, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
			FROM RIN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RIN4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = RIN1.DocEntry
	WHERE ORIN.DocDate BETWEEN @DataInicial AND @DataFinal
	AND ORIN.BPLId = @Filial
	AND ORIN.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	UNION ALL

	SELECT DISTINCT
		CASE WHEN DLN1.CFOPCode LIKE '1%' OR DLN1.CFOPCode LIKE '2%' OR DLN1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END									IND_OPER,
		0									IND_EMIT,
		ODLN.CardCode						COD_PART,
		ONFM.NfmCode						COD_MOD,
		'00'								COD_SIT,
		ODLN.SeriesStr						SER,
		ODLN.SubStr							SUB,
		ODLN.Serial							NUM_DOC,
		ODLN.U_chaveacesso					CHV_NFE,
		ODLN.TaxDate						DT_EMIS,
		ODLN.DocDate						DT_DOC,
		OUSG.U_CVA_ICMS						COP,
		DLN1.Usage							COD_NAT,
		CASE WHEN ODLN.Installmnt = 1
			THEN 0
			ELSE 1
		END									IND_PGTO,
		DLN1.LineTotal						VL_CONT,
		DLN1.CFOPCode						CFOP,
		ISNULL(ICMS.BaseSum,0.00)			VL_BC_ICMS,
		ISNULL(ICMS.TaxSum,0.00)			VL_ICMS,
		ISNULL(ICMS.TaxRate,0.00)			ALIQ_ICMS,
		ISNULL(ICMS_ST.TaxSum,0.00)			VL_ICMS_ST,	
		ICMS_ST.TaxSum		VL_AT,
		ICMS.Isentas		VL_ISNT_ICMS,
		ICMS.Outras			VL_OUT_ICMS,
		''					COD_INF_OBS
	FROM ODLN WITH(NOLOCK)
		INNER JOIN DLN1 WITH(NOLOCK)
			ON DLN1.DocEntry = ODLN.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = ODLN.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = DLN1.Usage

		LEFT JOIN (
				SELECT DLN4.DocEntry, DLN4.TaxRate, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
				FROM DLN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY DLN4.DocEntry, DLN4.TaxRate
			) ICMS
			ON ICMS.DocEntry = DLN1.DocEntry
		LEFT JOIN (
			SELECT DLN4.DocEntry, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
			FROM DLN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY DLN4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = DLN1.DocEntry
	WHERE ODLN.DocDate BETWEEN @DataInicial AND @DataFinal
	AND ODLN.BPLId = @Filial
	AND ISNULL(DLN1.TargetType, 0) <> 13
	AND ODLN.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	UNION ALL

	SELECT DISTINCT
		CASE WHEN INV1.CFOPCode LIKE '1%' OR INV1.CFOPCode LIKE '2%' OR INV1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END									IND_OPER,
		0									IND_EMIT,
		OINV.CardCode						COD_PART,
		ONFM.NfmCode						COD_MOD,
		'00'								COD_SIT,
		OINV.SeriesStr						SER,
		OINV.SubStr							SUB,
		OINV.Serial							NUM_DOC,
		OINV.U_chaveacesso					CHV_NFE,
		OINV.TaxDate						DT_EMIS,
		OINV.DocDate						DT_DOC,
		OUSG.U_CVA_ICMS						COP,
		INV1.Usage							COD_NAT,
		CASE WHEN OINV.Installmnt = 1
			THEN 0
			ELSE 1
		END									IND_PGTO,
		INV1.LineTotal						VL_CONT,
		INV1.CFOPCode						CFOP,
		ISNULL(ICMS.BaseSum,0.00)			VL_BC_ICMS,
		ISNULL(ICMS.TaxSum,0.00)			VL_ICMS,
		ISNULL(ICMS.TaxRate,0.00)			ALIQ_ICMS,
		ISNULL(ICMS_ST.TaxSum,0.00)			VL_ICMS_ST,	
		ICMS_ST.TaxSum		VL_AT,
		ICMS.Isentas		VL_ISNT_ICMS,
		ICMS.Outras			VL_OUT_ICMS,
		''					COD_INF_OBS
	FROM OINV WITH(NOLOCK)
		INNER JOIN INV1 WITH(NOLOCK)
			ON INV1.DocEntry = OINV.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = OINV.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = INV1.Usage

		LEFT JOIN (
				SELECT INV4.DocEntry, INV4.TaxRate, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
				FROM INV4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY INV4.DocEntry, INV4.TaxRate
			) ICMS
			ON ICMS.DocEntry = INV1.DocEntry
		LEFT JOIN (
			SELECT INV4.DocEntry, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
			FROM INV4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY INV4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = INV1.DocEntry
	WHERE OINV.DocDate BETWEEN @DataInicial AND @DataFinal
	AND OINV.BPLId = @Filial
	AND OINV.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	UNION ALL

	SELECT DISTINCT
		CASE WHEN RPC1.CFOPCode LIKE '1%' OR RPC1.CFOPCode LIKE '2%' OR RPC1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END									IND_OPER,
		0									IND_EMIT,
		ORPC.CardCode						COD_PART,
		ONFM.NfmCode						COD_MOD,
		'00'								COD_SIT,
		ORPC.SeriesStr						SER,
		ORPC.SubStr							SUB,
		ORPC.Serial							NUM_DOC,
		ORPC.U_chaveacesso					CHV_NFE,
		ORPC.TaxDate						DT_EMIS,
		ORPC.DocDate						DT_DOC,
		OUSG.U_CVA_ICMS						COP,
		RPC1.Usage							COD_NAT,
		CASE WHEN ORPC.Installmnt = 1
			THEN 0
			ELSE 1
		END									IND_PGTO,
		RPC1.LineTotal						VL_CONT,
		RPC1.CFOPCode						CFOP,
		ISNULL(ICMS.BaseSum,0.00)			VL_BC_ICMS,
		ISNULL(ICMS.TaxSum,0.00)			VL_ICMS,
		ISNULL(ICMS.TaxRate,0.00)			ALIQ_ICMS,
		ISNULL(ICMS_ST.TaxSum,0.00)			VL_ICMS_ST,	
		ICMS_ST.TaxSum		VL_AT,
		ICMS.Isentas		VL_ISNT_ICMS,
		ICMS.Outras			VL_OUT_ICMS,
		''					COD_INF_OBS
	FROM ORPC WITH(NOLOCK)
		INNER JOIN RPC1 WITH(NOLOCK)
			ON RPC1.DocEntry = ORPC.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = ORPC.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = RPC1.Usage

		LEFT JOIN (
				SELECT RPC4.DocEntry, RPC4.TaxRate, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
				FROM RPC4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RPC4.DocEntry, RPC4.TaxRate
			) ICMS
			ON ICMS.DocEntry = RPC1.DocEntry
		LEFT JOIN (
			SELECT RPC4.DocEntry, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
			FROM RPC4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RPC4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = RPC1.DocEntry
	WHERE ORPC.DocDate BETWEEN @DataInicial AND @DataFinal
	AND ORPC.BPLId = @Filial
	AND ORPC.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	UNION ALL

	SELECT DISTINCT
		CASE WHEN RPD1.CFOPCode LIKE '1%' OR RPD1.CFOPCode LIKE '2%' OR RPD1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END									IND_OPER,
		0									IND_EMIT,
		ORPD.CardCode						COD_PART,
		ONFM.NfmCode						COD_MOD,
		'00'								COD_SIT,
		ORPD.SeriesStr						SER,
		ORPD.SubStr							SUB,
		ORPD.Serial							NUM_DOC,
		ORPD.U_chaveacesso					CHV_NFE,
		ORPD.TaxDate						DT_EMIS,
		ORPD.DocDate						DT_DOC,
		OUSG.U_CVA_ICMS						COP,
		RPD1.Usage							COD_NAT,
		CASE WHEN ORPD.Installmnt = 1
			THEN 0
			ELSE 1
		END									IND_PGTO,
		RPD1.LineTotal						VL_CONT,
		RPD1.CFOPCode						CFOP,
		ISNULL(ICMS.BaseSum,0.00)			VL_BC_ICMS,
		ISNULL(ICMS.TaxSum,0.00)			VL_ICMS,
		ISNULL(ICMS.TaxRate,0.00)			ALIQ_ICMS,
		ISNULL(ICMS_ST.TaxSum,0.00)			VL_ICMS_ST,	
		ICMS_ST.TaxSum		VL_AT,
		ICMS.Isentas		VL_ISNT_ICMS,
		ICMS.Outras			VL_OUT_ICMS,
		''					COD_INF_OBS
	FROM ORPD WITH(NOLOCK)
		INNER JOIN RPD1 WITH(NOLOCK)
			ON RPD1.DocEntry = ORPD.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = ORPD.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = RPD1.Usage

		LEFT JOIN (
				SELECT RPD4.DocEntry, RPD4.TaxRate, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
				FROM RPD4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RPD4.DocEntry, RPD4.TaxRate
			) ICMS
			ON ICMS.DocEntry = RPD1.DocEntry
		LEFT JOIN (
			SELECT RPD4.DocEntry, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
			FROM RPD4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RPD4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = RPD1.DocEntry
	WHERE ORPD.DocDate BETWEEN @DataInicial AND @DataFinal
	AND ORPD.BPLId = @Filial
	AND ORPD.CANCELED = 'N' AND Model IN (1, 3, 5, 39)
)
