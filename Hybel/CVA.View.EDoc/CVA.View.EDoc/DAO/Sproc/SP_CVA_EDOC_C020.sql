IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_EDOC_C020')
	DROP PROCEDURE SP_CVA_EDOC_C020
GO
CREATE PROCEDURE [dbo].[SP_CVA_EDOC_C020]  
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
		CASE WHEN SeqCode IN (-1,-2) THEN 1 ELSE 0 END IndicadorEmitente,
		OPDN.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		OPDN.SeriesStr		Serie,
		OPDN.Serial			NrNF,
		OPDN.U_chaveacesso	ChaveAcesso,
		OPDN.TaxDate		DataEmissao,
		OPDN.DocDate		DataDocumento,
		CODNAT.COD			UsageId,		
		CASE WHEN OPDN.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		CASE WHEN DocTotal = 0 THEN MAX1099	ELSE DocTotal END	ValorDocumento,
		0.00				ValorJuros,
		--(PDN1.DiscPrcnt / 100) * PDN1.LineTotal	ValorDesconto,
		0.00				ValorDesconto,
		CASE WHEN DocTotal = 0 
			THEN MAX1099	- OPDN.VatSum 
			ELSE DocTotal - OPDN.VatSum 
		END	TotalLinha,
		ISNULL(FRETE.ValorFrete,0) ValorFrete,
		ISNULL(SEGURO.ValorSeguro,0)  ValorSeguro,
		ISNULL(OUTROS.ValorOutrasDespesas,0)  ValorOutrasDespesas,
		ISNULL(ISS.BaseSum,0)			BaseISS,
		ISNULL(ICMS.BaseSum,0)			BaseICMS,
		ISNULL(ICMS.TaxSum,0)			ValorICMS,
		ISNULL(ICMS_ST.BaseSum,0)		BaseICMS_ST,
		ISNULL(ICMS_ST.TaxSum,0)		ValorICMS_ST,
		ISNULL(IPI.TaxSum,0)			ValorIPI
	INTO #Docs
	FROM OPDN WITH(NOLOCK)
		OUTER APPLY 
		(
			SELECT TOP 1 * FROM PDN1 WITH(NOLOCK)
			WHERE PDN1.DocEntry = OPDN.DocEntry
		) PDN1
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
					SELECT PDN4.DocEntry, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum
					FROM PDN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY PDN4.DocEntry
				) ISS
			ON ISS.DocEntry = PDN1.DocEntry
		LEFT JOIN (
				SELECT PDN4.DocEntry, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum
				FROM PDN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY PDN4.DocEntry
			) ICMS
			ON ICMS.DocEntry = PDN1.DocEntry
		LEFT JOIN (
			SELECT PDN4.DocEntry, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum
			FROM PDN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY PDN4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = PDN1.DocEntry
		LEFT JOIN (
			SELECT PDN4.DocEntry, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum
			FROM PDN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY PDN4.DocEntry
		) IPI
			ON IPI.DocEntry = PDN1.DocEntry
		LEFT JOIN CODNAT WITH(NOLOCK) 
			ON CODNAT.CFOP = PDN1.CFOPCode
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
		CASE WHEN SeqCode IN (-1,-2) THEN 1 ELSE 0 END IndicadorEmitente,
		OPCH.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		OPCH.SeriesStr		Serie,
		OPCH.Serial			NrNF,
		OPCH.U_chaveacesso	ChaveAcesso,
		OPCH.TaxDate		DataEmissao,
		OPCH.DocDate		DataDocumento,
		CODNAT.COD			UsageId,		
		CASE WHEN OPCH.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		CASE WHEN DocTotal = 0 THEN MAX1099	ELSE DocTotal END	ValorDocumento,
		0.00				ValorJuros,
		--(PCH1.DiscPrcnt / 100) * PCH1.LineTotal	ValorDesconto,
		0.00				ValorDesconto,
		CASE WHEN DocTotal = 0 
			THEN MAX1099	- OPCH.VatSum 
			ELSE DocTotal - OPCH.VatSum 
		END	TotalLinha,
		ISNULL(FRETE.ValorFrete,0) ValorFrete,
		ISNULL(SEGURO.ValorSeguro,0)  ValorSeguro,
		ISNULL(OUTROS.ValorOutrasDespesas,0)  ValorOutrasDespesas,
		ISNULL(ISS.BaseSum,0)			BaseISS,
		ISNULL(ICMS.BaseSum,0)			BaseICMS,
		ISNULL(ICMS.TaxSum,0)			ValorICMS,
		ISNULL(ICMS_ST.BaseSum,0)		BaseICMS_ST,
		ISNULL(ICMS_ST.TaxSum,0)		ValorICMS_ST,
		ISNULL(IPI.TaxSum,0)			ValorIPI
	FROM OPCH WITH(NOLOCK)
		OUTER APPLY 
		(
			SELECT TOP 1 * FROM PCH1 WITH(NOLOCK)
			WHERE PCH1.DocEntry = OPCH.DocEntry
		) PCH1
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
					SELECT PCH4.DocEntry, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum
					FROM PCH4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY PCH4.DocEntry
				) ISS
			ON ISS.DocEntry = PCH1.DocEntry
		LEFT JOIN (
				SELECT PCH4.DocEntry, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum
				FROM PCH4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY PCH4.DocEntry
			) ICMS
			ON ICMS.DocEntry = PCH1.DocEntry
		LEFT JOIN (
			SELECT PCH4.DocEntry, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum
			FROM PCH4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY PCH4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = PCH1.DocEntry
		LEFT JOIN (
			SELECT PCH4.DocEntry, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum
			FROM PCH4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY PCH4.DocEntry
		) IPI
			ON IPI.DocEntry = PCH1.DocEntry
		LEFT JOIN CODNAT WITH(NOLOCK) 
			ON CODNAT.CFOP = PCH1.CFOPCode
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
		CASE WHEN SeqCode IN (-1,-2) THEN 1 ELSE 0 END IndicadorEmitente,
		ORDN.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		ORDN.SeriesStr		Serie,
		ORDN.Serial			NrNF,
		ORDN.U_chaveacesso	ChaveAcesso,
		ORDN.TaxDate		DataEmissao,
		ORDN.DocDate		DataDocumento,
		CODNAT.COD			UsageId,		
		CASE WHEN ORDN.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		CASE WHEN DocTotal = 0 THEN MAX1099	ELSE DocTotal END	ValorDocumento,
		0.00				ValorJuros,
		--(RDN1.DiscPrcnt / 100) * RDN1.LineTotal	ValorDesconto,
		0.00				ValorDesconto,
		CASE WHEN DocTotal = 0 
			THEN MAX1099	- ORDN.VatSum 
			ELSE DocTotal - ORDN.VatSum 
		END	TotalLinha,
		ISNULL(FRETE.ValorFrete,0) ValorFrete,
		ISNULL(SEGURO.ValorSeguro,0)  ValorSeguro,
		ISNULL(OUTROS.ValorOutrasDespesas,0)  ValorOutrasDespesas,
		ISNULL(ISS.BaseSum,0)			BaseISS,
		ISNULL(ICMS.BaseSum,0)			BaseICMS,
		ISNULL(ICMS.TaxSum,0)			ValorICMS,
		ISNULL(ICMS_ST.BaseSum,0)		BaseICMS_ST,
		ISNULL(ICMS_ST.TaxSum,0)		ValorICMS_ST,
		ISNULL(IPI.TaxSum,0)			ValorIPI
	FROM ORDN WITH(NOLOCK)
		OUTER APPLY 
		(
			SELECT TOP 1 * FROM RDN1 WITH(NOLOCK)
			WHERE RDN1.DocEntry = ORDN.DocEntry
		) RDN1
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
					SELECT RDN4.DocEntry, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum
					FROM RDN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY RDN4.DocEntry
				) ISS
			ON ISS.DocEntry = RDN1.DocEntry
		LEFT JOIN (
				SELECT RDN4.DocEntry, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum
				FROM RDN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RDN4.DocEntry
			) ICMS
			ON ICMS.DocEntry = RDN1.DocEntry
		LEFT JOIN (
			SELECT RDN4.DocEntry, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum
			FROM RDN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RDN4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = RDN1.DocEntry
		LEFT JOIN (
			SELECT RDN4.DocEntry, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum
			FROM RDN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY RDN4.DocEntry
		) IPI
			ON IPI.DocEntry = RDN1.DocEntry
		LEFT JOIN CODNAT WITH(NOLOCK) 
			ON CODNAT.CFOP = RDN1.CFOPCode
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
		CASE WHEN SeqCode IN (-1,-2) THEN 1 ELSE 0 END IndicadorEmitente,
		ORIN.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		ORIN.SeriesStr		Serie,
		ORIN.Serial			NrNF,
		ORIN.U_chaveacesso	ChaveAcesso,
		ORIN.TaxDate		DataEmissao,
		ORIN.DocDate		DataDocumento,
		CODNAT.COD			UsageId,		
		CASE WHEN ORIN.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		CASE WHEN DocTotal = 0 THEN MAX1099	ELSE DocTotal END	ValorDocumento,
		0.00				ValorJuros,
		--(RIN1.DiscPrcnt / 100) * RIN1.LineTotal	ValorDesconto,
		0.00				ValorDesconto,
		CASE WHEN DocTotal = 0 
			THEN MAX1099	- ORIN.VatSum 
			ELSE DocTotal - ORIN.VatSum 
		END	TotalLinha,
		ISNULL(FRETE.ValorFrete,0) ValorFrete,
		ISNULL(SEGURO.ValorSeguro,0)  ValorSeguro,
		ISNULL(OUTROS.ValorOutrasDespesas,0)  ValorOutrasDespesas,
		ISNULL(ISS.BaseSum,0)			BaseISS,
		ISNULL(ICMS.BaseSum,0)			BaseICMS,
		ISNULL(ICMS.TaxSum,0)			ValorICMS,
		ISNULL(ICMS_ST.BaseSum,0)		BaseICMS_ST,
		ISNULL(ICMS_ST.TaxSum,0)		ValorICMS_ST,
		ISNULL(IPI.TaxSum,0)			ValorIPI
	FROM ORIN WITH(NOLOCK)
		OUTER APPLY 
		(
			SELECT TOP 1 * FROM RIN1 WITH(NOLOCK)
			WHERE RIN1.DocEntry = ORIN.DocEntry
		) RIN1
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
					SELECT RIN4.DocEntry, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum
					FROM RIN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY RIN4.DocEntry
				) ISS
			ON ISS.DocEntry = RIN1.DocEntry
		LEFT JOIN (
				SELECT RIN4.DocEntry, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum
				FROM RIN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RIN4.DocEntry
			) ICMS
			ON ICMS.DocEntry = RIN1.DocEntry
		LEFT JOIN (
			SELECT RIN4.DocEntry, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum
			FROM RIN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RIN4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = RIN1.DocEntry
		LEFT JOIN (
			SELECT RIN4.DocEntry, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum
			FROM RIN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY RIN4.DocEntry
		) IPI
			ON IPI.DocEntry = RIN1.DocEntry
		LEFT JOIN CODNAT WITH(NOLOCK) 
			ON CODNAT.CFOP = RIN1.CFOPCode
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
		CASE WHEN SeqCode IN (-1,-2) THEN 1 ELSE 0 END IndicadorEmitente,
		ODLN.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		ODLN.SeriesStr		Serie,
		ODLN.Serial			NrNF,
		ODLN.U_chaveacesso	ChaveAcesso,
		ODLN.TaxDate		DataEmissao,
		ODLN.DocDate		DataDocumento,
		CODNAT.COD			UsageId,		
		CASE WHEN ODLN.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		CASE WHEN DocTotal = 0 THEN MAX1099	ELSE DocTotal END	ValorDocumento,
		0.00				ValorJuros,
		--(DLN1.DiscPrcnt / 100) * DLN1.LineTotal	ValorDesconto,
		0.00				ValorDesconto,
		CASE WHEN DocTotal = 0 
			THEN MAX1099	- ODLN.VatSum 
			ELSE DocTotal - ODLN.VatSum 
		END	TotalLinha,
		ISNULL(FRETE.ValorFrete,0) ValorFrete,
		ISNULL(SEGURO.ValorSeguro,0)  ValorSeguro,
		ISNULL(OUTROS.ValorOutrasDespesas,0)  ValorOutrasDespesas,
		ISNULL(ISS.BaseSum,0)			BaseISS,
		ISNULL(ICMS.BaseSum,0)			BaseICMS,
		ISNULL(ICMS.TaxSum,0)			ValorICMS,
		ISNULL(ICMS_ST.BaseSum,0)		BaseICMS_ST,
		ISNULL(ICMS_ST.TaxSum,0)		ValorICMS_ST,
		ISNULL(IPI.TaxSum,0)			ValorIPI
	FROM ODLN WITH(NOLOCK)
		OUTER APPLY 
		(
			SELECT TOP 1 * FROM DLN1 WITH(NOLOCK)
			WHERE DLN1.DocEntry = ODLN.DocEntry
		) DLN1
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
					SELECT DLN4.DocEntry, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum
					FROM DLN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY DLN4.DocEntry
				) ISS
			ON ISS.DocEntry = DLN1.DocEntry
		LEFT JOIN (
				SELECT DLN4.DocEntry, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum
				FROM DLN4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY DLN4.DocEntry
			) ICMS
			ON ICMS.DocEntry = DLN1.DocEntry
		LEFT JOIN (
			SELECT DLN4.DocEntry, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum
			FROM DLN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY DLN4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = DLN1.DocEntry
		LEFT JOIN (
			SELECT DLN4.DocEntry, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum
			FROM DLN4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY DLN4.DocEntry
		) IPI
			ON IPI.DocEntry = DLN1.DocEntry
		LEFT JOIN CODNAT WITH(NOLOCK) 
			ON CODNAT.CFOP = DLN1.CFOPCode
	WHERE ODLN.DocDate BETWEEN @DataDe AND @DataAte
	AND ODLN.BPLId = @Filial
	AND ISNULL(DLN1.TargetType, 0) <> 13
	AND ODLN.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	INSERT INTO #Docs
	SELECT 
		OINV.DocEntry,
		OINV.ObjType,
		CASE WHEN INV1.CFOPCode LIKE '1%' OR INV1.CFOPCode LIKE '2%' OR INV1.CFOPCode LIKE '3%' 
			THEN 0
			ELSE 1
		END					IndicadorOperacao,
		CASE WHEN SeqCode IN (-1,-2) THEN 1 ELSE 0 END IndicadorEmitente,
		OINV.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		OINV.SeriesStr		Serie,
		OINV.Serial			NrNF,
		OINV.U_chaveacesso	ChaveAcesso,
		OINV.TaxDate		DataEmissao,
		OINV.DocDate		DataDocumento,
		CODNAT.COD			UsageId,		
		CASE WHEN OINV.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		CASE WHEN DocTotal = 0 THEN MAX1099	ELSE DocTotal END	ValorDocumento,
		0.00				ValorJuros,
		--(INV1.DiscPrcnt / 100) * INV1.LineTotal	ValorDesconto,
		0.00				ValorDesconto,
		CASE WHEN DocTotal = 0 
			THEN MAX1099	- OINV.VatSum 
			ELSE DocTotal - OINV.VatSum 
		END	TotalLinha,
		ISNULL(FRETE.ValorFrete,0) ValorFrete,
		ISNULL(SEGURO.ValorSeguro,0)  ValorSeguro,
		ISNULL(OUTROS.ValorOutrasDespesas,0)  ValorOutrasDespesas,
		ISNULL(ISS.BaseSum,0)			BaseISS,
		ISNULL(ICMS.BaseSum,0)			BaseICMS,
		ISNULL(ICMS.TaxSum,0)			ValorICMS,
		ISNULL(ICMS_ST.BaseSum,0)		BaseICMS_ST,
		ISNULL(ICMS_ST.TaxSum,0)		ValorICMS_ST,
		ISNULL(IPI.TaxSum,0)			ValorIPI
	FROM OINV WITH(NOLOCK)
		OUTER APPLY 
		(
			SELECT TOP 1 * FROM INV1 WITH(NOLOCK)
			WHERE INV1.DocEntry = OINV.DocEntry
		) INV1
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
					SELECT INV4.DocEntry, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum
					FROM INV4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY INV4.DocEntry
				) ISS
			ON ISS.DocEntry = INV1.DocEntry
		LEFT JOIN (
				SELECT INV4.DocEntry, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum
				FROM INV4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY INV4.DocEntry
			) ICMS
			ON ICMS.DocEntry = INV1.DocEntry
		LEFT JOIN (
			SELECT INV4.DocEntry, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum
			FROM INV4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY INV4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = INV1.DocEntry
		LEFT JOIN (
			SELECT INV4.DocEntry, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum
			FROM INV4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY INV4.DocEntry
		) IPI
			ON IPI.DocEntry = INV1.DocEntry
		LEFT JOIN CODNAT WITH(NOLOCK) 
			ON CODNAT.CFOP = INV1.CFOPCode
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
		CASE WHEN SeqCode IN (-1,-2) THEN 1 ELSE 0 END IndicadorEmitente,
		ORPD.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		ORPD.SeriesStr		Serie,
		ORPD.Serial			NrNF,
		ORPD.U_chaveacesso	ChaveAcesso,
		ORPD.TaxDate		DataEmissao,
		ORPD.DocDate		DataDocumento,
		CODNAT.COD			UsageId,		
		CASE WHEN ORPD.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		CASE WHEN DocTotal = 0 THEN MAX1099	ELSE DocTotal END	ValorDocumento,
		0.00				ValorJuros,
		--(RPD1.DiscPrcnt / 100) * RPD1.LineTotal	ValorDesconto,
		0.00				ValorDesconto,
		CASE WHEN DocTotal = 0 
			THEN MAX1099	- ORPD.VatSum 
			ELSE DocTotal - ORPD.VatSum 
		END	TotalLinha,
		ISNULL(FRETE.ValorFrete,0) ValorFrete,
		ISNULL(SEGURO.ValorSeguro,0)  ValorSeguro,
		ISNULL(OUTROS.ValorOutrasDespesas,0)  ValorOutrasDespesas,
		ISNULL(ISS.BaseSum,0)			BaseISS,
		ISNULL(ICMS.BaseSum,0)			BaseICMS,
		ISNULL(ICMS.TaxSum,0)			ValorICMS,
		ISNULL(ICMS_ST.BaseSum,0)		BaseICMS_ST,
		ISNULL(ICMS_ST.TaxSum,0)		ValorICMS_ST,
		ISNULL(IPI.TaxSum,0)			ValorIPI
	FROM ORPD WITH(NOLOCK)
		OUTER APPLY 
		(
			SELECT TOP 1 * FROM RPD1 WITH(NOLOCK)
			WHERE RPD1.DocEntry = ORPD.DocEntry
		) RPD1
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
					SELECT RPD4.DocEntry, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum
					FROM RPD4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY RPD4.DocEntry
				) ISS
			ON ISS.DocEntry = RPD1.DocEntry
		LEFT JOIN (
				SELECT RPD4.DocEntry, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum
				FROM RPD4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RPD4.DocEntry
			) ICMS
			ON ICMS.DocEntry = RPD1.DocEntry
		LEFT JOIN (
			SELECT RPD4.DocEntry, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum
			FROM RPD4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RPD4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = RPD1.DocEntry
		LEFT JOIN (
			SELECT RPD4.DocEntry, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum
			FROM RPD4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY RPD4.DocEntry
		) IPI
			ON IPI.DocEntry = RPD1.DocEntry
		LEFT JOIN CODNAT WITH(NOLOCK) 
			ON CODNAT.CFOP = RPD1.CFOPCode
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
		CASE WHEN SeqCode IN (-1,-2) THEN 1 ELSE 0 END IndicadorEmitente,
		ORPC.CardCode,
		ONFM.NfmCode		ModeloNF,
		OUSG.U_CVA_ICMS		TabelaICMS,
		ORPC.SeriesStr		Serie,
		ORPC.Serial			NrNF,
		ORPC.U_chaveacesso	ChaveAcesso,
		ORPC.TaxDate		DataEmissao,
		ORPC.DocDate		DataDocumento,
		CODNAT.COD			UsageId,		
		CASE WHEN ORPC.Installmnt = 1
			THEN 0
			ELSE 1
		END					IndicadorPagamento,
		CASE WHEN DocTotal = 0 THEN MAX1099	ELSE DocTotal END	ValorDocumento,
		0.00				ValorJuros,
		--(RPC1.DiscPrcnt / 100) * RPC1.LineTotal	ValorDesconto,
		0.00				ValorDesconto,
		CASE WHEN DocTotal = 0 
			THEN MAX1099	- ORPC.VatSum 
			ELSE DocTotal - ORPC.VatSum 
		END	TotalLinha,
		ISNULL(FRETE.ValorFrete,0) ValorFrete,
		ISNULL(SEGURO.ValorSeguro,0)  ValorSeguro,
		ISNULL(OUTROS.ValorOutrasDespesas,0)  ValorOutrasDespesas,
		ISNULL(ISS.BaseSum,0)			BaseISS,
		ISNULL(ICMS.BaseSum,0)			BaseICMS,
		ISNULL(ICMS.TaxSum,0)			ValorICMS,
		ISNULL(ICMS_ST.BaseSum,0)		BaseICMS_ST,
		ISNULL(ICMS_ST.TaxSum,0)		ValorICMS_ST,
		ISNULL(IPI.TaxSum,0)			ValorIPI
	FROM ORPC WITH(NOLOCK)
		OUTER APPLY 
		(
			SELECT TOP 1 * FROM RPC1 WITH(NOLOCK)
			WHERE RPC1.DocEntry = ORPC.DocEntry
		) RPC1
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
					SELECT RPC4.DocEntry, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum
					FROM RPC4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY RPC4.DocEntry
				) ISS
			ON ISS.DocEntry = RPC1.DocEntry
		LEFT JOIN (
				SELECT RPC4.DocEntry, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum
				FROM RPC4 WITH(NOLOCK) 
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY RPC4.DocEntry
			) ICMS
			ON ICMS.DocEntry = RPC1.DocEntry
		LEFT JOIN (
			SELECT RPC4.DocEntry, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum
			FROM RPC4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
			GROUP BY RPC4.DocEntry
		) ICMS_ST
			ON ICMS_ST.DocEntry = RPC1.DocEntry
		LEFT JOIN (
			SELECT RPC4.DocEntry, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum
			FROM RPC4 WITH(NOLOCK) 
				INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
				INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
			GROUP BY RPC4.DocEntry
		) IPI
			ON IPI.DocEntry = RPC1.DocEntry
		LEFT JOIN CODNAT WITH(NOLOCK) 
			ON CODNAT.CFOP = RPC1.CFOPCode
	WHERE ORPC.DocDate BETWEEN @DataDe AND @DataAte
	AND ORPC.BPLId = @Filial
	AND ORPC.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

	SELECT
		Docs.DocEntry,
		Docs.ObjType,
		'C020'				Linha,
		IndicadorOperacao,
		IndicadorEmitente,			
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
		Docs.IndicadorEmitente,
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

