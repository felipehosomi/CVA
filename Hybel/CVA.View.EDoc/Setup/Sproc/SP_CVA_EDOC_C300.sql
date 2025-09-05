IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_EDOC_C300')
	DROP PROCEDURE SP_CVA_EDOC_C300
GO
CREATE PROCEDURE SP_CVA_EDOC_C300
(
	@Filial			INT,
	@DataDe			DATETIME,
	@DataAte		DATETIME
)
AS
BEGIN
	SELECT
		OPDN.DocEntry,
		OPDN.ObjType,
		PDN1.Usage,
		PDN1.VisOrder	NumeroLinha,
		PDN1.ItemCode,
		PDN1.Price		ValorUnitario,
		PDN1.Quantity	Quantidade,
		(PDN1.DiscPrcnt / 100) * PDN1.LineTotal	ValorDesconto,
		0.00			ValorJuros,
		PDN1.Price * PDN1.Quantity	ValorLiquido,
		PDN1.CSTCode	CST,
		PDN1.CFOPCode	CFOP,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxRate		AliquotaICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxRate		AliquotaICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.BaseSum			BaseIPI,
		IPI.TaxRate			AliquotaIPI,
		IPI.TaxSum			ValorIPI
	INTO #Docs
	FROM OPDN WITH(NOLOCK)
		INNER JOIN PDN1 WITH(NOLOCK)
			ON PDN1.DocEntry = OPDN.DocEntry
		LEFT JOIN (
				SELECT PDN4.DocEntry, PDN4.LineNum, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum, PDN4.TaxRate
				FROM PDN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY PDN4.DocEntry, PDN4.LineNum, PDN4.TaxRate
			) ICMS
			ON ICMS.DocEntry = PDN1.DocEntry
			AND ICMS.LineNum = PDN1.LineNum
		LEFT JOIN (
			SELECT PDN4.DocEntry, PDN4.LineNum, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum, PDN4.TaxRate
			FROM PDN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY PDN4.DocEntry, PDN4.LineNum, PDN4.TaxRate
		) ICMS_ST
			ON ICMS_ST.DocEntry = PDN1.DocEntry
			AND ICMS_ST.LineNum = PDN1.LineNum
		LEFT JOIN (
			SELECT PDN4.DocEntry, PDN4.LineNum, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum, PDN4.TaxRate
			FROM PDN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY PDN4.DocEntry, PDN4.LineNum, PDN4.TaxRate
		) IPI
			ON IPI.DocEntry = PDN1.DocEntry
			AND IPI.LineNum = PDN1.LineNum
	WHERE OPDN.DocDate BETWEEN @DataDe AND @DataAte
	AND OPDN.BPLId = @Filial
	AND ISNULL(PDN1.TargetType, 0) <> 18
	AND OPDN.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	INSERT INTO #Docs
	SELECT 
		OPCH.DocEntry,
		OPCH.ObjType,
		PCH1.Usage,
		PCH1.VisOrder	NumeroLinha,
		PCH1.ItemCode,
		PCH1.Price		ValorUnitario,
		PCH1.Quantity	Quantidade,
		(PCH1.DiscPrcnt / 100) * PCH1.LineTotal	ValorDesconto,
		0.00			ValorJuros,
		PCH1.Price * PCH1.Quantity	ValorLiquido,
		PCH1.CSTCode	CST,
		PCH1.CFOPCode	CFOP,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxRate		AliquotaICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxRate		AliquotaICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.BaseSum			BaseIPI,
		IPI.TaxRate			AliquotaIPI,
		IPI.TaxSum			ValorIPI
	FROM OPCH WITH(NOLOCK)
		INNER JOIN PCH1 WITH(NOLOCK)
			ON PCH1.DocEntry = OPCH.DocEntry
		LEFT JOIN (
				SELECT PCH4.DocEntry, PCH4.LineNum, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum, PCH4.TaxRate
				FROM PCH4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY PCH4.DocEntry, PCH4.LineNum, PCH4.TaxRate
			) ICMS
			ON ICMS.DocEntry = PCH1.DocEntry
			AND ICMS.LineNum = PCH1.LineNum
		LEFT JOIN (
			SELECT PCH4.DocEntry, PCH4.LineNum, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum, PCH4.TaxRate
			FROM PCH4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY PCH4.DocEntry, PCH4.LineNum, PCH4.TaxRate
		) ICMS_ST
			ON ICMS_ST.DocEntry = PCH1.DocEntry
			AND ICMS_ST.LineNum = PCH1.LineNum
		LEFT JOIN (
			SELECT PCH4.DocEntry, PCH4.LineNum, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum, PCH4.TaxRate
			FROM PCH4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY PCH4.DocEntry, PCH4.LineNum, PCH4.TaxRate
		) IPI
			ON IPI.DocEntry = PCH1.DocEntry
			AND IPI.LineNum = PCH1.LineNum
	WHERE OPCH.DocDate BETWEEN @DataDe AND @DataAte
	AND OPCH.BPLId = @Filial
	AND OPCH.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	INSERT INTO #Docs
	SELECT 
		ORDN.DocEntry,
		ORDN.ObjType,
		RDN1.Usage,
		RDN1.VisOrder	NumeroLinha,
		RDN1.ItemCode,
		RDN1.Price		ValorUnitario,
		RDN1.Quantity	Quantidade,
		(RDN1.DiscPrcnt / 100) * RDN1.LineTotal	ValorDesconto,
		0.00			ValorJuros,
		RDN1.Price * RDN1.Quantity	ValorLiquido,
		RDN1.CSTCode	CST,
		RDN1.CFOPCode	CFOP,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxRate		AliquotaICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxRate		AliquotaICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.BaseSum			BaseIPI,
		IPI.TaxRate			AliquotaIPI,
		IPI.TaxSum			ValorIPI
	FROM ORDN WITH(NOLOCK)
		INNER JOIN RDN1 WITH(NOLOCK)
			ON RDN1.DocEntry = ORDN.DocEntry
		LEFT JOIN (
				SELECT RDN4.DocEntry, RDN4.LineNum, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum, RDN4.TaxRate
				FROM RDN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RDN4.DocEntry, RDN4.LineNum, RDN4.TaxRate
			) ICMS
			ON ICMS.DocEntry = RDN1.DocEntry
			AND ICMS.LineNum = RDN1.LineNum
		LEFT JOIN (
			SELECT RDN4.DocEntry, RDN4.LineNum, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum, RDN4.TaxRate
			FROM RDN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RDN4.DocEntry, RDN4.LineNum, RDN4.TaxRate
		) ICMS_ST
			ON ICMS_ST.DocEntry = RDN1.DocEntry
			AND ICMS_ST.LineNum = RDN1.LineNum
		LEFT JOIN (
			SELECT RDN4.DocEntry, RDN4.LineNum, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum, RDN4.TaxRate
			FROM RDN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY RDN4.DocEntry, RDN4.LineNum, RDN4.TaxRate
		) IPI
			ON IPI.DocEntry = RDN1.DocEntry
			AND IPI.LineNum = RDN1.LineNum
	WHERE ORDN.DocDate BETWEEN @DataDe AND @DataAte
	AND ORDN.BPLId = @Filial
	AND ORDN.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	INSERT INTO #Docs
	SELECT 
		ORIN.DocEntry,
		ORIN.ObjType,
		RIN1.Usage,
		RIN1.VisOrder	NumeroLinha,
		RIN1.ItemCode,
		RIN1.Price		ValorUnitario,
		RIN1.Quantity	Quantidade,
		(RIN1.DiscPrcnt / 100) * RIN1.LineTotal	ValorDesconto,
		0.00			ValorJuros,
		RIN1.Price * RIN1.Quantity	ValorLiquido,
		RIN1.CSTCode	CST,
		RIN1.CFOPCode	CFOP,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxRate		AliquotaICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxRate		AliquotaICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.BaseSum			BaseIPI,
		IPI.TaxRate			AliquotaIPI,
		IPI.TaxSum			ValorIPI
	FROM ORIN WITH(NOLOCK)
		INNER JOIN RIN1 WITH(NOLOCK)
			ON RIN1.DocEntry = ORIN.DocEntry
		LEFT JOIN (
				SELECT RIN4.DocEntry, RIN4.LineNum, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum, RIN4.TaxRate
				FROM RIN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RIN4.DocEntry, RIN4.LineNum, RIN4.TaxRate
			) ICMS
			ON ICMS.DocEntry = RIN1.DocEntry
			AND ICMS.LineNum = RIN1.LineNum
		LEFT JOIN (
			SELECT RIN4.DocEntry, RIN4.LineNum, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum, RIN4.TaxRate
			FROM RIN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RIN4.DocEntry, RIN4.LineNum, RIN4.TaxRate
		) ICMS_ST
			ON ICMS_ST.DocEntry = RIN1.DocEntry
			AND ICMS_ST.LineNum = RIN1.LineNum
		LEFT JOIN (
			SELECT RIN4.DocEntry, RIN4.LineNum, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum, RIN4.TaxRate
			FROM RIN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY RIN4.DocEntry, RIN4.LineNum, RIN4.TaxRate
		) IPI
			ON IPI.DocEntry = RIN1.DocEntry
			AND IPI.LineNum = RIN1.LineNum
	WHERE ORIN.DocDate BETWEEN @DataDe AND @DataAte
	AND ORIN.BPLId = @Filial
	AND ORIN.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	INSERT INTO #Docs
	SELECT 
		ODLN.DocEntry,
		ODLN.ObjType,
		DLN1.Usage,
		DLN1.VisOrder	NumeroLinha,
		DLN1.ItemCode,
		DLN1.Price		ValorUnitario,
		DLN1.Quantity	Quantidade,
		(DLN1.DiscPrcnt / 100) * DLN1.LineTotal	ValorDesconto,
		0.00			ValorJuros,
		DLN1.Price * DLN1.Quantity	ValorLiquido,
		DLN1.CSTCode	CST,
		DLN1.CFOPCode	CFOP,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxRate		AliquotaICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxRate		AliquotaICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.BaseSum			BaseIPI,
		IPI.TaxRate			AliquotaIPI,
		IPI.TaxSum			ValorIPI
	FROM ODLN WITH(NOLOCK)
		INNER JOIN DLN1 WITH(NOLOCK)
			ON DLN1.DocEntry = ODLN.DocEntry
		LEFT JOIN (
				SELECT DLN4.DocEntry, DLN4.LineNum, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum, DLN4.TaxRate
				FROM DLN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY DLN4.DocEntry, DLN4.LineNum, DLN4.TaxRate
			) ICMS
			ON ICMS.DocEntry = DLN1.DocEntry
			AND ICMS.LineNum = DLN1.LineNum
		LEFT JOIN (
			SELECT DLN4.DocEntry, DLN4.LineNum, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum, DLN4.TaxRate
			FROM DLN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY DLN4.DocEntry, DLN4.LineNum, DLN4.TaxRate
		) ICMS_ST
			ON ICMS_ST.DocEntry = DLN1.DocEntry
			AND ICMS_ST.LineNum = DLN1.LineNum
		LEFT JOIN (
			SELECT DLN4.DocEntry, DLN4.LineNum, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum, DLN4.TaxRate
			FROM DLN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY DLN4.DocEntry, DLN4.LineNum, DLN4.TaxRate
		) IPI
			ON IPI.DocEntry = DLN1.DocEntry
			AND IPI.LineNum = DLN1.LineNum
	WHERE ODLN.DocDate BETWEEN @DataDe AND @DataAte
	AND ODLN.BPLId = @Filial
	AND ODLN.CANCELED = 'N' AND Model IN (1, 3, 5, 39)
	AND ISNULL(DLN1.TargetType, 0) <> 13

	INSERT INTO #Docs
	SELECT 
		OINV.DocEntry,
		OINV.ObjType,
		INV1.Usage,
		INV1.VisOrder	NumeroLinha,
		INV1.ItemCode,
		INV1.Price		ValorUnitario,
		INV1.Quantity	Quantidade,
		(INV1.DiscPrcnt / 100) * INV1.LineTotal	ValorDesconto,
		0.00			ValorJuros,
		INV1.Price * INV1.Quantity	ValorLiquido,
		INV1.CSTCode	CST,
		INV1.CFOPCode	CFOP,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxRate		AliquotaICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxRate		AliquotaICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.BaseSum			BaseIPI,
		IPI.TaxRate			AliquotaIPI,
		IPI.TaxSum			ValorIPI
	FROM OINV WITH(NOLOCK)
		INNER JOIN INV1 WITH(NOLOCK)
			ON INV1.DocEntry = OINV.DocEntry
		LEFT JOIN (
				SELECT INV4.DocEntry, INV4.LineNum, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum, INV4.TaxRate
				FROM INV4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY INV4.DocEntry, INV4.LineNum, INV4.TaxRate
			) ICMS
			ON ICMS.DocEntry = INV1.DocEntry
			AND ICMS.LineNum = INV1.LineNum
		LEFT JOIN (
			SELECT INV4.DocEntry, INV4.LineNum, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum, INV4.TaxRate
			FROM INV4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY INV4.DocEntry, INV4.LineNum, INV4.TaxRate
		) ICMS_ST
			ON ICMS_ST.DocEntry = INV1.DocEntry
			AND ICMS_ST.LineNum = INV1.LineNum
		LEFT JOIN (
			SELECT INV4.DocEntry, INV4.LineNum, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum, INV4.TaxRate
			FROM INV4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY INV4.DocEntry, INV4.LineNum, INV4.TaxRate
		) IPI
			ON IPI.DocEntry = INV1.DocEntry
			AND IPI.LineNum = INV1.LineNum
	WHERE OINV.DocDate BETWEEN @DataDe AND @DataAte
	AND OINV.BPLId = @Filial
	AND OINV.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	INSERT INTO #Docs
	SELECT 
		ORPD.DocEntry,
		ORPD.ObjType,
		RPD1.Usage,
		RPD1.VisOrder	NumeroLinha,
		RPD1.ItemCode,
		RPD1.Price		ValorUnitario,
		RPD1.Quantity	Quantidade,
		(RPD1.DiscPrcnt / 100) * RPD1.LineTotal	ValorDesconto,
		0.00			ValorJuros,
		RPD1.Price * RPD1.Quantity	ValorLiquido,
		RPD1.CSTCode	CST,
		RPD1.CFOPCode	CFOP,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxRate		AliquotaICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxRate		AliquotaICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.BaseSum			BaseIPI,
		IPI.TaxRate			AliquotaIPI,
		IPI.TaxSum			ValorIPI
	FROM ORPD WITH(NOLOCK)
		INNER JOIN RPD1 WITH(NOLOCK)
			ON RPD1.DocEntry = ORPD.DocEntry
		LEFT JOIN (
				SELECT RPD4.DocEntry, RPD4.LineNum, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum, RPD4.TaxRate
				FROM RPD4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RPD4.DocEntry, RPD4.LineNum, RPD4.TaxRate
			) ICMS
			ON ICMS.DocEntry = RPD1.DocEntry
			AND ICMS.LineNum = RPD1.LineNum
		LEFT JOIN (
			SELECT RPD4.DocEntry, RPD4.LineNum, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum, RPD4.TaxRate
			FROM RPD4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RPD4.DocEntry, RPD4.LineNum, RPD4.TaxRate
		) ICMS_ST
			ON ICMS_ST.DocEntry = RPD1.DocEntry
			AND ICMS_ST.LineNum = RPD1.LineNum
		LEFT JOIN (
			SELECT RPD4.DocEntry, RPD4.LineNum, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum, RPD4.TaxRate
			FROM RPD4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY RPD4.DocEntry, RPD4.LineNum, RPD4.TaxRate
		) IPI
			ON IPI.DocEntry = RPD1.DocEntry
			AND IPI.LineNum = RPD1.LineNum
	WHERE ORPD.DocDate BETWEEN @DataDe AND @DataAte
	AND ORPD.BPLId = @Filial
	AND ORPD.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	INSERT INTO #Docs
	SELECT 
		ORPC.DocEntry,
		ORPC.ObjType,
		RPC1.Usage,
		RPC1.VisOrder	NumeroLinha,
		RPC1.ItemCode,
		RPC1.Price		ValorUnitario,
		RPC1.Quantity	Quantidade,
		(RPC1.DiscPrcnt / 100) * RPC1.LineTotal	ValorDesconto,
		0.00			ValorJuros,
		RPC1.Price * RPC1.Quantity	ValorLiquido,
		RPC1.CSTCode	CST,
		RPC1.CFOPCode	CFOP,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxRate		AliquotaICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxRate		AliquotaICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.BaseSum			BaseIPI,
		IPI.TaxRate			AliquotaIPI,
		IPI.TaxSum			ValorIPI
	FROM ORPC WITH(NOLOCK)
		INNER JOIN RPC1 WITH(NOLOCK)
			ON RPC1.DocEntry = ORPC.DocEntry
		LEFT JOIN (
				SELECT RPC4.DocEntry, RPC4.LineNum, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum, RPC4.TaxRate
				FROM RPC4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RPC4.DocEntry, RPC4.LineNum, RPC4.TaxRate
			) ICMS
			ON ICMS.DocEntry = RPC1.DocEntry
			AND ICMS.LineNum = RPC1.LineNum
		LEFT JOIN (
			SELECT RPC4.DocEntry, RPC4.LineNum, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum, RPC4.TaxRate
			FROM RPC4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RPC4.DocEntry, RPC4.LineNum, RPC4.TaxRate
		) ICMS_ST
			ON ICMS_ST.DocEntry = RPC1.DocEntry
			AND ICMS_ST.LineNum = RPC1.LineNum
		LEFT JOIN (
			SELECT RPC4.DocEntry, RPC4.LineNum, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum, RPC4.TaxRate
			FROM RPC4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY RPC4.DocEntry, RPC4.LineNum, RPC4.TaxRate
		) IPI
			ON IPI.DocEntry = RPC1.DocEntry
			AND IPI.LineNum = RPC1.LineNum
	WHERE ORPC.DocDate BETWEEN @DataDe AND @DataAte
	AND ORPC.BPLId = @Filial
	AND ORPC.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	SELECT
		Docs.DocEntry,
		Docs.ObjType,
		Docs.Usage UsageId,
		'C300'	Linha,
		Docs.NumeroLinha,
		Docs.ItemCode,
		OITM.SalUnitMsr	UN,
		Docs.ValorUnitario,
		Docs.Quantidade,
		Docs.ValorDesconto,
		Docs.ValorJuros,
		Docs.ValorLiquido,
		ONCM.NcmCode	NCM,
		Docs.CST,
		Docs.CFOP,
		Docs.BaseICMS,
		DOcs.AliquotaICMS,
		Docs.ValorICMS,
		Docs.BaseICMS_ST,
		Docs.AliquotaICMS_ST,
		Docs.ValorICMS_ST,
		Docs.BaseIPI,
		Docs.AliquotaIPI,
		Docs.ValorIPI
	FROM #Docs Docs
		INNER JOIN OITM WITH(NOLOCK)
			ON OITM.ItemCode = Docs.ItemCode
		INNER JOIN ONCM WITH(NOLOCK)
			ON ONCM.AbsEntry = OITM.NCMCode
	
	DROP TABLE #Docs
END