IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_EDOC_C020')
	DROP PROCEDURE SP_CVA_EDOC_C020
GO
CREATE PROCEDURE SP_CVA_EDOC_C020
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
		CASE WHEN PDN1.CFOPCode LIKE '1%' OR PDN1.CFOPCode LIKE '2%' OR PDN1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END					IndicadorOperacao,
		OPDN.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		OPDN.SeriesStr		Serie,
		OPDN.Serial			NrNF,
		OPDN.U_chaveacesso	ChaveAcesso,
		OPDN.TaxDate		DataEmissao,
		OPDN.DocDate		DataDocumento,
		PDN1.Usage			UsageId,
		CASE WHEN OPDN.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		OPDN.DocTotal		ValorDocumento,
		0.00				ValorJuros,
		(PDN1.DiscPrcnt / 100) * PDN1.LineTotal	ValorDesconto,
		PDN1.LineTotal		TotalLinha,
		FRETE.ValorFrete,
		SEGURO.ValorSeguro,
		OUTROS.ValorOutrasDespesas,
		ISS.BaseSum			BaseISS,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.TaxSum			ValorIPI
	INTO #Docs
	FROM OPDN WITH(NOLOCK)
		INNER JOIN PDN1 WITH(NOLOCK)
			ON PDN1.DocEntry = OPDN.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = OPDN.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = PDN1.Usage
		LEFT JOIN (SELECT FRETE.DocEntry, SUM(FRETE.LineTotal) ValorFrete FROM PDN3 FRETE WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON FRETE.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%FRETE%' GROUP BY FRETE.DocEntry) FRETE
			ON FRETE.DocEntry = OPDN.DocEntry
		LEFT JOIN (SELECT SEGURO.DocEntry, SUM(SEGURO.LineTotal) ValorSeguro FROM PDN3 SEGURO WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON SEGURO.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%SEGURO%' GROUP BY SEGURO.DocEntry) SEGURO
			ON SEGURO.DocEntry = OPDN.DocEntry
		LEFT JOIN (SELECT OUTROS.DocEntry, SUM(OUTROS.LineTotal) ValorOutrasDespesas FROM PDN3 OUTROS WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON OUTROS.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName NOT LIKE '%FRETE%' AND OEXD.ExpnsName NOT LIKE '%SEGURO%' GROUP BY OUTROS.DocEntry) OUTROS
			ON OUTROS.DocEntry = OPDN.DocEntry
		LEFT JOIN (
					SELECT PDN4.DocEntry, PDN4.LineNum, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum
					FROM PDN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY PDN4.DocEntry, PDN4.LineNum
				) ISS
			ON ISS.DocEntry = PDN1.DocEntry
			AND ISS.LineNum = PDN1.LineNum
		LEFT JOIN (
				SELECT PDN4.DocEntry, PDN4.LineNum, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum
				FROM PDN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY PDN4.DocEntry, PDN4.LineNum
			) ICMS
			ON ICMS.DocEntry = PDN1.DocEntry
			AND ICMS.LineNum = PDN1.LineNum
		LEFT JOIN (
			SELECT PDN4.DocEntry, PDN4.LineNum, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum
			FROM PDN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY PDN4.DocEntry, PDN4.LineNum
		) ICMS_ST
			ON ICMS_ST.DocEntry = PDN1.DocEntry
			AND ICMS_ST.LineNum = PDN1.LineNum
		LEFT JOIN (
			SELECT PDN4.DocEntry, PDN4.LineNum, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum
			FROM PDN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY PDN4.DocEntry, PDN4.LineNum
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
		CASE WHEN PCH1.CFOPCode LIKE '1%' OR PCH1.CFOPCode LIKE '2%' OR PCH1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END					IndicadorOperacao,	
		OPCH.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		OPCH.SeriesStr		Serie,
		OPCH.Serial			NrNF,
		OPCH.U_chaveacesso	ChaveAcesso,
		OPCH.TaxDate		DataEmissao,
		OPCH.DocDate		DataDocumento,
		PCH1.Usage			UsageId,
		CASE WHEN OPCH.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		OPCH.DocTotal		ValorDocumento,
		0.00				ValorJuros,
		(PCH1.DiscPrcnt / 100) * PCH1.LineTotal	ValorDesconto,
		PCH1.LineTotal		TotalLinha,
		FRETE.ValorFrete,
		SEGURO.ValorSeguro,
		OUTROS.ValorOutrasDespesas,
		ISS.BaseSum			BaseISS,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.TaxSum			ValorIPI
	FROM OPCH WITH(NOLOCK)
		INNER JOIN PCH1 WITH(NOLOCK)
			ON PCH1.DocEntry = OPCH.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = OPCH.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = PCH1.Usage
		LEFT JOIN (SELECT FRETE.DocEntry, SUM(FRETE.LineTotal) ValorFrete FROM PCH3 FRETE WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON FRETE.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%FRETE%' GROUP BY FRETE.DocEntry) FRETE
			ON FRETE.DocEntry = OPCH.DocEntry
		LEFT JOIN (SELECT SEGURO.DocEntry, SUM(SEGURO.LineTotal) ValorSeguro FROM PCH3 SEGURO WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON SEGURO.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%SEGURO%' GROUP BY SEGURO.DocEntry) SEGURO
			ON SEGURO.DocEntry = OPCH.DocEntry
		LEFT JOIN (SELECT OUTROS.DocEntry, SUM(OUTROS.LineTotal) ValorOutrasDespesas FROM PCH3 OUTROS WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON OUTROS.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName NOT LIKE '%FRETE%' AND OEXD.ExpnsName NOT LIKE '%SEGURO%' GROUP BY OUTROS.DocEntry) OUTROS
			ON OUTROS.DocEntry = OPCH.DocEntry
		LEFT JOIN (
					SELECT PCH4.DocEntry, PCH4.LineNum, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum
					FROM PCH4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY PCH4.DocEntry, PCH4.LineNum
				) ISS
			ON ISS.DocEntry = PCH1.DocEntry
			AND ISS.LineNum = PCH1.LineNum
		LEFT JOIN (
				SELECT PCH4.DocEntry, PCH4.LineNum, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum
				FROM PCH4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY PCH4.DocEntry, PCH4.LineNum
			) ICMS
			ON ICMS.DocEntry = PCH1.DocEntry
			AND ICMS.LineNum = PCH1.LineNum
		LEFT JOIN (
			SELECT PCH4.DocEntry, PCH4.LineNum, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum
			FROM PCH4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY PCH4.DocEntry, PCH4.LineNum
		) ICMS_ST
			ON ICMS_ST.DocEntry = PCH1.DocEntry
			AND ICMS_ST.LineNum = PCH1.LineNum
		LEFT JOIN (
			SELECT PCH4.DocEntry, PCH4.LineNum, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum
			FROM PCH4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY PCH4.DocEntry, PCH4.LineNum
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
		CASE WHEN RDN1.CFOPCode LIKE '1%' OR RDN1.CFOPCode LIKE '2%' OR RDN1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END					IndicadorOperacao,
		ORDN.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		ORDN.SeriesStr		Serie,
		ORDN.Serial			NrNF,
		ORDN.U_chaveacesso	ChaveAcesso,
		ORDN.TaxDate		DataEmissao,
		ORDN.DocDate		DataDocumento,
		RDN1.Usage			UsageId,
		CASE WHEN ORDN.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		ORDN.DocTotal		ValorDocumento,
		0.00				ValorJuros,
		(RDN1.DiscPrcnt / 100) * RDN1.LineTotal	ValorDesconto,
		RDN1.LineTotal		TotalLinha,
		FRETE.ValorFrete,
		SEGURO.ValorSeguro,
		OUTROS.ValorOutrasDespesas,
		ISS.BaseSum			BaseISS,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.TaxSum			ValorIPI
	FROM ORDN WITH(NOLOCK)
		INNER JOIN RDN1 WITH(NOLOCK)
			ON RDN1.DocEntry = ORDN.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = ORDN.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = RDN1.Usage
		LEFT JOIN (SELECT FRETE.DocEntry, SUM(FRETE.LineTotal) ValorFrete FROM RDN3 FRETE WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON FRETE.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%FRETE%' GROUP BY FRETE.DocEntry) FRETE
			ON FRETE.DocEntry = ORDN.DocEntry
		LEFT JOIN (SELECT SEGURO.DocEntry, SUM(SEGURO.LineTotal) ValorSeguro FROM RDN3 SEGURO WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON SEGURO.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%SEGURO%' GROUP BY SEGURO.DocEntry) SEGURO
			ON SEGURO.DocEntry = ORDN.DocEntry
		LEFT JOIN (SELECT OUTROS.DocEntry, SUM(OUTROS.LineTotal) ValorOutrasDespesas FROM RDN3 OUTROS WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON OUTROS.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName NOT LIKE '%FRETE%' AND OEXD.ExpnsName NOT LIKE '%SEGURO%' GROUP BY OUTROS.DocEntry) OUTROS
			ON OUTROS.DocEntry = ORDN.DocEntry
		LEFT JOIN (
					SELECT RDN4.DocEntry, RDN4.LineNum, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum
					FROM RDN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY RDN4.DocEntry, RDN4.LineNum
				) ISS
			ON ISS.DocEntry = RDN1.DocEntry
			AND ISS.LineNum = RDN1.LineNum
		LEFT JOIN (
				SELECT RDN4.DocEntry, RDN4.LineNum, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum
				FROM RDN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RDN4.DocEntry, RDN4.LineNum
			) ICMS
			ON ICMS.DocEntry = RDN1.DocEntry
			AND ICMS.LineNum = RDN1.LineNum
		LEFT JOIN (
			SELECT RDN4.DocEntry, RDN4.LineNum, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum
			FROM RDN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RDN4.DocEntry, RDN4.LineNum
		) ICMS_ST
			ON ICMS_ST.DocEntry = RDN1.DocEntry
			AND ICMS_ST.LineNum = RDN1.LineNum
		LEFT JOIN (
			SELECT RDN4.DocEntry, RDN4.LineNum, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum
			FROM RDN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY RDN4.DocEntry, RDN4.LineNum
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
		CASE WHEN RIN1.CFOPCode LIKE '1%' OR RIN1.CFOPCode LIKE '2%' OR RIN1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END					IndicadorOperacao,
		ORIN.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		ORIN.SeriesStr		Serie,
		ORIN.Serial			NrNF,
		ORIN.U_chaveacesso	ChaveAcesso,
		ORIN.TaxDate		DataEmissao,
		ORIN.DocDate		DataDocumento,
		RIN1.Usage			UsageId,
		CASE WHEN ORIN.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		ORIN.DocTotal		ValorDocumento,
		0.00				ValorJuros,
		(RIN1.DiscPrcnt / 100) * RIN1.LineTotal	ValorDesconto,
		RIN1.LineTotal		TotalLinha,
		FRETE.ValorFrete,
		SEGURO.ValorSeguro,
		OUTROS.ValorOutrasDespesas,
		ISS.BaseSum			BaseISS,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.TaxSum			ValorIPI
	FROM ORIN WITH(NOLOCK)
		INNER JOIN RIN1 WITH(NOLOCK)
			ON RIN1.DocEntry = ORIN.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = ORIN.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = RIN1.Usage
		LEFT JOIN (SELECT FRETE.DocEntry, SUM(FRETE.LineTotal) ValorFrete FROM RIN3 FRETE WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON FRETE.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%FRETE%' GROUP BY FRETE.DocEntry) FRETE
			ON FRETE.DocEntry = ORIN.DocEntry
		LEFT JOIN (SELECT SEGURO.DocEntry, SUM(SEGURO.LineTotal) ValorSeguro FROM RIN3 SEGURO WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON SEGURO.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%SEGURO%' GROUP BY SEGURO.DocEntry) SEGURO
			ON SEGURO.DocEntry = ORIN.DocEntry
		LEFT JOIN (SELECT OUTROS.DocEntry, SUM(OUTROS.LineTotal) ValorOutrasDespesas FROM RIN3 OUTROS WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON OUTROS.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName NOT LIKE '%FRETE%' AND OEXD.ExpnsName NOT LIKE '%SEGURO%' GROUP BY OUTROS.DocEntry) OUTROS
			ON OUTROS.DocEntry = ORIN.DocEntry
		LEFT JOIN (
					SELECT RIN4.DocEntry, RIN4.LineNum, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum
					FROM RIN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY RIN4.DocEntry, RIN4.LineNum
				) ISS
			ON ISS.DocEntry = RIN1.DocEntry
			AND ISS.LineNum = RIN1.LineNum
		LEFT JOIN (
				SELECT RIN4.DocEntry, RIN4.LineNum, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum
				FROM RIN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RIN4.DocEntry, RIN4.LineNum
			) ICMS
			ON ICMS.DocEntry = RIN1.DocEntry
			AND ICMS.LineNum = RIN1.LineNum
		LEFT JOIN (
			SELECT RIN4.DocEntry, RIN4.LineNum, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum
			FROM RIN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RIN4.DocEntry, RIN4.LineNum
		) ICMS_ST
			ON ICMS_ST.DocEntry = RIN1.DocEntry
			AND ICMS_ST.LineNum = RIN1.LineNum
		LEFT JOIN (
			SELECT RIN4.DocEntry, RIN4.LineNum, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum
			FROM RIN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY RIN4.DocEntry, RIN4.LineNum
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
		CASE WHEN DLN1.CFOPCode LIKE '1%' OR DLN1.CFOPCode LIKE '2%' OR DLN1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END					IndicadorOperacao,		
		ODLN.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		ODLN.SeriesStr		Serie,
		ODLN.Serial			NrNF,
		ODLN.U_chaveacesso	ChaveAcesso,
		NULL				DataEmissao,
		ODLN.DocDate		DataDocumento,
		DLN1.Usage			UsageId,
		CASE WHEN ODLN.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		ODLN.DocTotal		ValorDocumento,
		0.00				ValorJuros,
		(DLN1.DiscPrcnt / 100) * DLN1.LineTotal	ValorDesconto,
		DLN1.LineTotal		TotalLinha,
		FRETE.ValorFrete,
		SEGURO.ValorSeguro,
		OUTROS.ValorOutrasDespesas,
		ISS.BaseSum			BaseISS,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.TaxSum			ValorIPI
	FROM ODLN WITH(NOLOCK)
		INNER JOIN DLN1 WITH(NOLOCK)
			ON DLN1.DocEntry = ODLN.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = ODLN.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = DLN1.Usage
		LEFT JOIN (SELECT FRETE.DocEntry, SUM(FRETE.LineTotal) ValorFrete FROM DLN3 FRETE WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON FRETE.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%FRETE%' GROUP BY FRETE.DocEntry) FRETE
			ON FRETE.DocEntry = ODLN.DocEntry
		LEFT JOIN (SELECT SEGURO.DocEntry, SUM(SEGURO.LineTotal) ValorSeguro FROM DLN3 SEGURO WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON SEGURO.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%SEGURO%' GROUP BY SEGURO.DocEntry) SEGURO
			ON SEGURO.DocEntry = ODLN.DocEntry
		LEFT JOIN (SELECT OUTROS.DocEntry, SUM(OUTROS.LineTotal) ValorOutrasDespesas FROM DLN3 OUTROS WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON OUTROS.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName NOT LIKE '%FRETE%' AND OEXD.ExpnsName NOT LIKE '%SEGURO%' GROUP BY OUTROS.DocEntry) OUTROS
			ON OUTROS.DocEntry = ODLN.DocEntry
		LEFT JOIN (
					SELECT DLN4.DocEntry, DLN4.LineNum, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum
					FROM DLN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY DLN4.DocEntry, DLN4.LineNum
				) ISS
			ON ISS.DocEntry = DLN1.DocEntry
			AND ISS.LineNum = DLN1.LineNum
		LEFT JOIN (
				SELECT DLN4.DocEntry, DLN4.LineNum, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum
				FROM DLN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY DLN4.DocEntry, DLN4.LineNum
			) ICMS
			ON ICMS.DocEntry = DLN1.DocEntry
			AND ICMS.LineNum = DLN1.LineNum
		LEFT JOIN (
			SELECT DLN4.DocEntry, DLN4.LineNum, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum
			FROM DLN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY DLN4.DocEntry, DLN4.LineNum
		) ICMS_ST
			ON ICMS_ST.DocEntry = DLN1.DocEntry
			AND ICMS_ST.LineNum = DLN1.LineNum
		LEFT JOIN (
			SELECT DLN4.DocEntry, DLN4.LineNum, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum
			FROM DLN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY DLN4.DocEntry, DLN4.LineNum
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
		CASE WHEN INV1.CFOPCode LIKE '1%' OR INV1.CFOPCode LIKE '2%' OR INV1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END					IndicadorOperacao,
		OINV.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		OINV.SeriesStr		Serie,
		OINV.Serial			NrNF,
		OINV.U_chaveacesso	ChaveAcesso,
		NULL				DataEmissao,
		OINV.DocDate		DataDocumento,
		INV1.Usage			UsageId,
		CASE WHEN OINV.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		OINV.DocTotal		ValorDocumento,
		0.00				ValorJuros,
		(INV1.DiscPrcnt / 100) * INV1.LineTotal	ValorDesconto,
		INV1.LineTotal		TotalLinha,
		FRETE.ValorFrete,
		SEGURO.ValorSeguro,
		OUTROS.ValorOutrasDespesas,
		ISS.BaseSum			BaseISS,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.TaxSum			ValorIPI
	FROM OINV WITH(NOLOCK)
		INNER JOIN INV1 WITH(NOLOCK)
			ON INV1.DocEntry = OINV.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = OINV.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = INV1.Usage
		LEFT JOIN (SELECT FRETE.DocEntry, SUM(FRETE.LineTotal) ValorFrete FROM INV3 FRETE WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON FRETE.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%FRETE%' GROUP BY FRETE.DocEntry) FRETE
			ON FRETE.DocEntry = OINV.DocEntry
		LEFT JOIN (SELECT SEGURO.DocEntry, SUM(SEGURO.LineTotal) ValorSeguro FROM INV3 SEGURO WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON SEGURO.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%SEGURO%' GROUP BY SEGURO.DocEntry) SEGURO
			ON SEGURO.DocEntry = OINV.DocEntry
		LEFT JOIN (SELECT OUTROS.DocEntry, SUM(OUTROS.LineTotal) ValorOutrasDespesas FROM INV3 OUTROS WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON OUTROS.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName NOT LIKE '%FRETE%' AND OEXD.ExpnsName NOT LIKE '%SEGURO%' GROUP BY OUTROS.DocEntry) OUTROS
			ON OUTROS.DocEntry = OINV.DocEntry
		LEFT JOIN (
					SELECT INV4.DocEntry, INV4.LineNum, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum
					FROM INV4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY INV4.DocEntry, INV4.LineNum
				) ISS
			ON ISS.DocEntry = INV1.DocEntry
			AND ISS.LineNum = INV1.LineNum
		LEFT JOIN (
				SELECT INV4.DocEntry, INV4.LineNum, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum
				FROM INV4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY INV4.DocEntry, INV4.LineNum
			) ICMS
			ON ICMS.DocEntry = INV1.DocEntry
			AND ICMS.LineNum = INV1.LineNum
		LEFT JOIN (
			SELECT INV4.DocEntry, INV4.LineNum, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum
			FROM INV4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY INV4.DocEntry, INV4.LineNum
		) ICMS_ST
			ON ICMS_ST.DocEntry = INV1.DocEntry
			AND ICMS_ST.LineNum = INV1.LineNum
		LEFT JOIN (
			SELECT INV4.DocEntry, INV4.LineNum, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum
			FROM INV4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY INV4.DocEntry, INV4.LineNum
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
		CASE WHEN RPD1.CFOPCode LIKE '1%' OR RPD1.CFOPCode LIKE '2%' OR RPD1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END					IndicadorOperacao,
		ORPD.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		ORPD.SeriesStr		Serie,
		ORPD.Serial			NrNF,
		ORPD.U_chaveacesso	ChaveAcesso,
		NULL				DataEmissao,
		ORPD.DocDate		DataDocumento,
		RPD1.Usage			UsageId,
		CASE WHEN ORPD.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		ORPD.DocTotal		ValorDocumento,
		0.00				ValorJuros,
		(RPD1.DiscPrcnt / 100) * RPD1.LineTotal	ValorDesconto,
		RPD1.LineTotal		TotalLinha,
		FRETE.ValorFrete,
		SEGURO.ValorSeguro,
		OUTROS.ValorOutrasDespesas,
		ISS.BaseSum			BaseISS,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.TaxSum			ValorIPI
	FROM ORPD WITH(NOLOCK)
		INNER JOIN RPD1 WITH(NOLOCK)
			ON RPD1.DocEntry = ORPD.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = ORPD.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = RPD1.Usage
		LEFT JOIN (SELECT FRETE.DocEntry, SUM(FRETE.LineTotal) ValorFrete FROM RPD3 FRETE WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON FRETE.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%FRETE%' GROUP BY FRETE.DocEntry) FRETE
			ON FRETE.DocEntry = ORPD.DocEntry
		LEFT JOIN (SELECT SEGURO.DocEntry, SUM(SEGURO.LineTotal) ValorSeguro FROM RPD3 SEGURO WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON SEGURO.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%SEGURO%' GROUP BY SEGURO.DocEntry) SEGURO
			ON SEGURO.DocEntry = ORPD.DocEntry
		LEFT JOIN (SELECT OUTROS.DocEntry, SUM(OUTROS.LineTotal) ValorOutrasDespesas FROM RPD3 OUTROS WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON OUTROS.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName NOT LIKE '%FRETE%' AND OEXD.ExpnsName NOT LIKE '%SEGURO%' GROUP BY OUTROS.DocEntry) OUTROS
			ON OUTROS.DocEntry = ORPD.DocEntry
		LEFT JOIN (
					SELECT RPD4.DocEntry, RPD4.LineNum, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum
					FROM RPD4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY RPD4.DocEntry, RPD4.LineNum
				) ISS
			ON ISS.DocEntry = RPD1.DocEntry
			AND ISS.LineNum = RPD1.LineNum
		LEFT JOIN (
				SELECT RPD4.DocEntry, RPD4.LineNum, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum
				FROM RPD4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RPD4.DocEntry, RPD4.LineNum
			) ICMS
			ON ICMS.DocEntry = RPD1.DocEntry
			AND ICMS.LineNum = RPD1.LineNum
		LEFT JOIN (
			SELECT RPD4.DocEntry, RPD4.LineNum, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum
			FROM RPD4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RPD4.DocEntry, RPD4.LineNum
		) ICMS_ST
			ON ICMS_ST.DocEntry = RPD1.DocEntry
			AND ICMS_ST.LineNum = RPD1.LineNum
		LEFT JOIN (
			SELECT RPD4.DocEntry, RPD4.LineNum, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum
			FROM RPD4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY RPD4.DocEntry, RPD4.LineNum
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
		CASE WHEN RPC1.CFOPCode LIKE '1%' OR RPC1.CFOPCode LIKE '2%' OR RPC1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END					IndicadorOperacao,
		ORPC.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		ORPC.SeriesStr		Serie,
		ORPC.Serial			NrNF,
		ORPC.U_chaveacesso	ChaveAcesso,
		NULL				DataEmissao,
		ORPC.DocDate		DataDocumento,
		RPC1.Usage			UsageId,
		CASE WHEN ORPC.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		ORPC.DocTotal		ValorDocumento,
		0.00				ValorJuros,
		(RPC1.DiscPrcnt / 100) * RPC1.LineTotal	ValorDesconto,
		RPC1.LineTotal		TotalLinha,
		FRETE.ValorFrete,
		SEGURO.ValorSeguro,
		OUTROS.ValorOutrasDespesas,
		ISS.BaseSum			BaseISS,
		ICMS.BaseSum		BaseICMS,
		ICMS.TaxSum			ValorICMS,
		ICMS_ST.BaseSum		BaseICMS_ST,
		ICMS_ST.TaxSum		ValorICMS_ST,
		IPI.TaxSum			ValorIPI
	FROM ORPC WITH(NOLOCK)
		INNER JOIN RPC1 WITH(NOLOCK)
			ON RPC1.DocEntry = ORPC.DocEntry
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = ORPC.Model
		INNER JOIN OUSG WITH(NOLOCK)
			ON OUSG.Id = RPC1.Usage
		LEFT JOIN (SELECT FRETE.DocEntry, SUM(FRETE.LineTotal) ValorFrete FROM RPC3 FRETE WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON FRETE.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%FRETE%' GROUP BY FRETE.DocEntry) FRETE
			ON FRETE.DocEntry = ORPC.DocEntry
		LEFT JOIN (SELECT SEGURO.DocEntry, SUM(SEGURO.LineTotal) ValorSeguro FROM RPC3 SEGURO WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON SEGURO.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName LIKE '%SEGURO%' GROUP BY SEGURO.DocEntry) SEGURO
			ON SEGURO.DocEntry = ORPC.DocEntry
		LEFT JOIN (SELECT OUTROS.DocEntry, SUM(OUTROS.LineTotal) ValorOutrasDespesas FROM RPC3 OUTROS WITH(NOLOCK) INNER JOIN OEXD WITH(NOLOCK) ON OUTROS.ExpnsCode = OEXD.ExpnsCode AND OEXD.ExpnsName NOT LIKE '%FRETE%' AND OEXD.ExpnsName NOT LIKE '%SEGURO%' GROUP BY OUTROS.DocEntry) OUTROS
			ON OUTROS.DocEntry = ORPC.DocEntry
		LEFT JOIN (
					SELECT RPC4.DocEntry, RPC4.LineNum, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum
					FROM RPC4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY RPC4.DocEntry, RPC4.LineNum
				) ISS
			ON ISS.DocEntry = RPC1.DocEntry
			AND ISS.LineNum = RPC1.LineNum
		LEFT JOIN (
				SELECT RPC4.DocEntry, RPC4.LineNum, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum
				FROM RPC4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RPC4.DocEntry, RPC4.LineNum
			) ICMS
			ON ICMS.DocEntry = RPC1.DocEntry
			AND ICMS.LineNum = RPC1.LineNum
		LEFT JOIN (
			SELECT RPC4.DocEntry, RPC4.LineNum, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum
			FROM RPC4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RPC4.DocEntry, RPC4.LineNum
		) ICMS_ST
			ON ICMS_ST.DocEntry = RPC1.DocEntry
			AND ICMS_ST.LineNum = RPC1.LineNum
		LEFT JOIN (
			SELECT RPC4.DocEntry, RPC4.LineNum, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum
			FROM RPC4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY RPC4.DocEntry, RPC4.LineNum
		) IPI
			ON IPI.DocEntry = RPC1.DocEntry
			AND IPI.LineNum = RPC1.LineNum
	WHERE ORPC.DocDate BETWEEN @DataDe AND @DataAte
	AND ORPC.BPLId = @Filial
	AND ORPC.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	SELECT
		Docs.DocEntry,
		Docs.ObjType,
		'C020'				Linha,
		IndicadorOperacao,
		0					IndicadorEmitente,			
		Docs.CardCode,
		Docs.ModeloNF,
		'00'				SituacaoNF,
		Docs.Serie,
		Docs.NrNF,
		Docs.ChaveAcesso,
		Docs.DataEmissao,
		Docs.DataDocumento,
		Docs.UsageId,
		Docs.IndicadorPagamento,
		Docs.ValorDocumento,
		Docs.ValorJuros,
		SUM(Docs.ValorDesconto) ValorDesconto,
		SUM(TotalLinha) TotalLinha,
		Docs.ValorFrete,
		Docs.ValorSeguro,
		Docs.ValorOutrasDespesas,
		SUM(BaseISS) BaseISS,
		SUM(BaseICMS) BaseICMS,
		SUM(ValorICMS) ValorICMS,
		SUM(ValorICMS_ST) ValorICMS_ST,
		SUM(ValorIPI) ValorIPI
	FROM #Docs Docs
	GROUP BY
		Docs.DocEntry,
		Docs.ObjType,
		Docs.IndicadorOperacao,
		Docs.CardCode,
		Docs.ModeloNF,
		Docs.TabelaICMS,
		Docs.Serie,
		Docs.NrNF,
		Docs.ChaveAcesso,
		Docs.DataEmissao,
		Docs.DataDocumento,
		Docs.UsageId,
		Docs.IndicadorPagamento,
		Docs.ValorDocumento,
		Docs.ValorJuros,
		Docs.ValorFrete,
		Docs.ValorSeguro,
		Docs.ValorOutrasDespesas

	DROP TABLE #Docs
END