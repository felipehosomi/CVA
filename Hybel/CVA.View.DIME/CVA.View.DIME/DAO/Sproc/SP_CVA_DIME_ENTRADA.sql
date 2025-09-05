IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_DIME_ENTRADA')
	DROP PROCEDURE SP_CVA_DIME_ENTRADA
GO
CREATE PROCEDURE [dbo].[SP_CVA_DIME_ENTRADA] 
(
	@Code		NVARCHAR(50)
)
AS
BEGIN
	SELECT 
		PDN1.CFOPCode	CFOP,
		PDN12.CityS		Cidade,
		ISNULL(PDN12.StateS, OCRD.State1)	UF,
		PDN1.LineTotal + PDN1.VatSum + ISNULL(PDN2.LineTotal,0) + ISNULL(FRETE.VatSum,0) + CASE WHEN PDN1.VatSum > 0  THEN  ISNULL(PDN1.DistribSum, 0) ELSE 0 END ValorContabil,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ISNULL(ICMS.BaseSum,0)	
		END BaseCalculo,
		CASE WHEN (OCRD.U_h_simples_nacional = 1 AND OUSG.ID IN (76, 83)) OR OUSG.Usage LIKE 'E-Ativo%' OR OUSG.ID IN (42,45,95) OR CFOPCode = '3101' -- E-Desp Energia PR
			THEN 0
			ELSE ISNULL(ICMS.TaxSum,0)	+ ISNULL(FRETE.VatSum,0) 
		END ImpostoCreditado,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ISNULL(ICMS.Isentas,0)
		END Isentas,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN ISNULL(ICMS.BaseSum,0) + ISNULL(ICMS.Outras,0)
			ELSE ISNULL(ICMS.Outras,0)
		END Outras,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ISNULL(ICMS_ST.TaxSum,0)		
		END ImpostoRetido,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ISNULL(ICMS_ST.BaseSum,0)	
		END BaseCalculoImpostoRetido
	FROM [@CVA_DIME] DIME WITH(NOLOCK)
		INNER JOIN OPDN WITH(NOLOCK)
			ON OPDN.BPLId = DIME.U_Filial OR DIME.U_Filial = 0
		INNER JOIN OCRD WITH(NOLOCK)
			ON OCRD.CardCode = OPDN.CardCode
		INNER JOIN PDN1 WITH(NOLOCK)
			ON PDN1.DocEntry = OPDN.DocEntry
		INNER JOIN OITM WITH(NOLOCK)
			ON OITM.ItemCode = PDN1.ItemCode
		LEFT JOIN OUSG WITH(NOLOCK)
			ON OUSG.ID = PDN1.Usage
		INNER JOIN PDN12 WITH(NOLOCK)
			ON PDN12.DocEntry = OPDN.DocEntry
		LEFT JOIN PDN2 WITH(NOLOCK)
			ON PDN2.DocEntry = PDN1.DocEntry
			AND PDN2.LineNum = PDN1.LineNum
		LEFT JOIN (
					SELECT PDN4.DocEntry, PDN4.LineNum, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM PDN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
					GROUP BY PDN4.DocEntry, PDN4.LineNum
				) ICMS
			ON ICMS.DocEntry = PDN1.DocEntry
			AND ICMS.LineNum = PDN1.LineNum
		LEFT JOIN (
					SELECT PDN4.DocEntry, PDN4.LineNum, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM PDN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
					GROUP BY PDN4.DocEntry, PDN4.LineNum
				) ICMS_ST
			ON ICMS_ST.DocEntry = PDN1.DocEntry
			AND ICMS_ST.LineNum = PDN1.LineNum
		LEFT JOIN (
					SELECT PDN3.DocEntry, SUM(PDN3.LineTotal) / (SELECT COUNT(*) FROM PDN1 WHERE PDN1.DocEntry = PDN3.DocEntry) as VatSum
					FROM PDN3 WITH(NOLOCK) 
					GROUP BY PDN3.DocEntry
				) FRETE
			ON FRETE.DocEntry = OPDN.DocEntry
	WHERE DIME.Code = @Code
	AND OPDN.DocDate BETWEEN DIME.U_DtDe AND DIME.U_DtAte
	AND OPDN.CANCELED = 'N'
	AND OPDN.Model NOT IN (0, 28, 37, 3, 57)
	AND ISNULL(PDN1.TargetType, 0) <> 18

	UNION ALL

	SELECT 
		PCH1.CFOPCode	CFOP,
		PCH12.CityS		Cidade,
		ISNULL(PCH12.StateS, OCRD.State1)	UF,
		PCH1.LineTotal + PCH1.VatSum + ISNULL(PCH2.LineTotal,0) + ISNULL(FRETE.VatSum,0) + CASE WHEN PCH1.VatSum > 0  THEN  ISNULL(PCH1.DistribSum, 0) ELSE 0 END ValorContabil,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ISNULL(ICMS.BaseSum,0)	
		END BaseCalculo,
		CASE WHEN (OCRD.U_h_simples_nacional = 1 AND OUSG.ID IN (76, 83)) OR OUSG.Usage LIKE 'E-Ativo%' OR OUSG.ID IN (42,45,95) OR CFOPCode = '3101'-- E-Desp Energia PR
			THEN 0
			ELSE ISNULL(ICMS.TaxSum,0)	+ ISNULL(FRETE.VatSum,0) 
		END ImpostoCreditado,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ISNULL(ICMS.Isentas,0)
		END Isentas,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN ISNULL(ICMS.BaseSum,0) + ISNULL(ICMS.Outras,0)
			ELSE ISNULL(ICMS.Outras,0)
		END Outras,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ISNULL(ICMS_ST.TaxSum,0)		
		END ImpostoRetido,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ISNULL(ICMS_ST.BaseSum,0)	
		END BaseCalculoImpostoRetido
	FROM [@CVA_DIME] DIME WITH(NOLOCK)
		INNER JOIN OPCH WITH(NOLOCK)
			ON OPCH.BPLId = DIME.U_Filial OR DIME.U_Filial = 0
		INNER JOIN OCRD WITH(NOLOCK)
			ON OCRD.CardCode = OPCH.CardCode
		INNER JOIN PCH1 WITH(NOLOCK)
			ON PCH1.DocEntry = OPCH.DocEntry
		INNER JOIN OITM WITH(NOLOCK)
			ON OITM.ItemCode = PCH1.ItemCode
		LEFT JOIN OUSG WITH(NOLOCK)
			ON OUSG.ID = PCH1.Usage
		INNER JOIN PCH12 WITH(NOLOCK)
			ON PCH12.DocEntry = OPCH.DocEntry
		LEFT JOIN PCH2 WITH(NOLOCK)
			ON PCH2.DocEntry = PCH1.DocEntry
			AND PCH2.LineNum = PCH1.LineNum
		LEFT JOIN (
					SELECT PCH4.DocEntry, PCH4.LineNum, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM PCH4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'		
					GROUP BY PCH4.DocEntry, PCH4.LineNum
				) ICMS
			ON ICMS.DocEntry = PCH1.DocEntry
			AND ICMS.LineNum = PCH1.LineNum
		LEFT JOIN (
					SELECT PCH4.DocEntry, PCH4.LineNum, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM PCH4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
					GROUP BY PCH4.DocEntry, PCH4.LineNum
				) ICMS_ST
			ON ICMS_ST.DocEntry = PCH1.DocEntry
			AND ICMS_ST.LineNum = PCH1.LineNum
		LEFT JOIN (
					SELECT PCH3.DocEntry, SUM(PCH3.LineTotal) / (SELECT COUNT(*) FROM PCH1 WHERE PCH1.DocEntry = PCH3.DocEntry) as VatSum
					FROM PCH3 WITH(NOLOCK) 
					GROUP BY PCH3.DocEntry
				) FRETE
			ON FRETE.DocEntry = OPCH.DocEntry
	WHERE DIME.Code = @Code
	AND OPCH.DocDate BETWEEN DIME.U_DtDe AND DIME.U_DtAte
	AND OPCH.CANCELED = 'N'
	AND OPCH.Model NOT IN (0, 28, 37, 3, 57)

	UNION ALL

	SELECT 
		RIN1.CFOPCode	CFOP,
		RIN12.CityS		Cidade,
		ISNULL(RIN12.StateS, OCRD.State1)	UF,
		RIN1.LineTotal + RIN1.VatSum + ISNULL(RIN2.LineTotal, 0) + ISNULL(FRETE.VatSum,0) + CASE WHEN RIN1.VatSum > 0  THEN  ISNULL(RIN1.DistribSum, 0) ELSE 0 END ValorContabil,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ISNULL(ICMS.BaseSum,0)	
		END BaseCalculo,
		CASE WHEN (OCRD.U_h_simples_nacional = 1 AND OUSG.ID IN (76, 83)) OR OUSG.Usage LIKE 'E-Ativo%' OR OUSG.ID IN (42,45,95) OR CFOPCode = '3101'-- E-Desp Energia PR
			THEN 0
			ELSE ISNULL(ICMS.TaxSum,0)	+ ISNULL(FRETE.VatSum,0) 
		END ImpostoCreditado,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ISNULL(ICMS.Isentas,0)
		END Isentas,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN ISNULL(ICMS.BaseSum,0) + ISNULL(ICMS.Outras,0)
			ELSE ISNULL(ICMS.Outras,0)
		END Outras,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ISNULL(ICMS_ST.TaxSum,0)	
		END ImpostoRetido,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ICMS_ST.BaseSum	
		END BaseCalculoImpostoRetido
	FROM [@CVA_DIME] DIME WITH(NOLOCK)
		INNER JOIN ORIN WITH(NOLOCK)
			ON ORIN.BPLId = DIME.U_Filial OR DIME.U_Filial = 0
		INNER JOIN OCRD WITH(NOLOCK)
			ON OCRD.CardCode = ORIN.CardCode
		INNER JOIN RIN1 WITH(NOLOCK)
			ON RIN1.DocEntry = ORIN.DocEntry
		LEFT JOIN OUSG WITH(NOLOCK)
			ON OUSG.ID = RIN1.Usage
		INNER JOIN RIN12 WITH(NOLOCK)
			ON RIN12.DocEntry = ORIN.DocEntry
		LEFT JOIN RIN2 WITH(NOLOCK)
			ON RIN2.DocEntry = RIN1.DocEntry
			AND RIN2.LineNum = RIN1.LineNum
		LEFT JOIN (
					SELECT RIN4.DocEntry, RIN4.LineNum, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM RIN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
					GROUP BY RIN4.DocEntry, RIN4.LineNum
				) ICMS
			ON ICMS.DocEntry = RIN1.DocEntry
			AND ICMS.LineNum = RIN1.LineNum
		LEFT JOIN (
					SELECT RIN4.DocEntry, RIN4.LineNum, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM RIN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
					GROUP BY RIN4.DocEntry, RIN4.LineNum
				) ICMS_ST
			ON ICMS_ST.DocEntry = RIN1.DocEntry
			AND ICMS_ST.LineNum = RIN1.LineNum
		LEFT JOIN (
					SELECT RIN3.DocEntry, SUM(RIN3.LineTotal) / (SELECT COUNT(*) FROM RIN1 WHERE RIN1.DocEntry = RIN3.DocEntry) as VatSum
					FROM RIN3 WITH(NOLOCK) 
					GROUP BY RIN3.DocEntry
				) FRETE
			ON FRETE.DocEntry = ORIN.DocEntry
	WHERE DIME.Code = @Code
	AND ORIN.DocDate BETWEEN DIME.U_DtDe AND DIME.U_DtAte
	AND ORIN.CANCELED = 'N'
	AND ORIN.Model NOT IN (0, 28, 37, 3, 57)

	UNION ALL

	SELECT 
		RDN1.CFOPCode	CFOP,
		RDN12.CityS		Cidade,
		ISNULL(RDN12.StateS, OCRD.State1)	UF,
		RDN1.LineTotal + RDN1.VatSum + ISNULL(RDN2.LineTotal,0) + ISNULL(FRETE.VatSum,0) + CASE WHEN RDN1.VatSum > 0  THEN  ISNULL(RDN1.DistribSum, 0) ELSE 0 END ValorContabil,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ISNULL(ICMS.BaseSum,0)	
		END BaseCalculo,
		CASE WHEN (OCRD.U_h_simples_nacional = 1 AND OUSG.ID IN (76, 83)) OR OUSG.Usage LIKE 'E-Ativo%' OR OUSG.ID IN (42,45,95) OR CFOPCode = '3101'-- E-Desp Energia PR
			THEN 0
			ELSE ISNULL(ICMS.TaxSum,0)	+ ISNULL(FRETE.VatSum,0) 
		END ImpostoCreditado,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ISNULL(ICMS.Isentas,0)
		END Isentas,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN ISNULL(ICMS.BaseSum,0) + ISNULL(ICMS.Outras,0)
			ELSE ISNULL(ICMS.Outras,0)
		END Outras,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ISNULL(ICMS_ST.TaxSum,0)	
		END ImpostoRetido,
		CASE WHEN OUSG.Usage LIKE 'E-Ativo%'
			THEN 0
			ELSE ICMS_ST.BaseSum	
		END BaseCalculoImpostoRetido
	FROM [@CVA_DIME] DIME WITH(NOLOCK)
		INNER JOIN ORDN WITH(NOLOCK)
			ON ORDN.BPLId = DIME.U_Filial OR DIME.U_Filial = 0
		INNER JOIN OCRD WITH(NOLOCK)
			ON OCRD.CardCode = ORDN.CardCode
		INNER JOIN RDN1 WITH(NOLOCK)
			ON RDN1.DocEntry = ORDN.DocEntry
		LEFT JOIN OUSG WITH(NOLOCK)
			ON OUSG.ID = RDN1.Usage
		INNER JOIN RDN12 WITH(NOLOCK)
			ON RDN12.DocEntry = ORDN.DocEntry
		LEFT JOIN RDN2 WITH(NOLOCK)
			ON RDN2.DocEntry = RDN1.DocEntry
			AND RDN2.LineNum = RDN1.LineNum
		LEFT JOIN (
					SELECT RDN4.DocEntry, RDN4.LineNum, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM RDN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
					GROUP BY RDN4.DocEntry, RDN4.LineNum
				) ICMS
			ON ICMS.DocEntry = RDN1.DocEntry
			AND ICMS.LineNum = RDN1.LineNum
		LEFT JOIN (
					SELECT RDN4.DocEntry, RDN4.LineNum, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
					FROM RDN4 WITH(NOLOCK) 
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
					GROUP BY RDN4.DocEntry, RDN4.LineNum
				) ICMS_ST
			ON ICMS_ST.DocEntry = RDN1.DocEntry
			AND ICMS_ST.LineNum = RDN1.LineNum
		LEFT JOIN (
					SELECT RDN3.DocEntry, SUM(RDN3.LineTotal) / (SELECT COUNT(*) FROM RDN1 WHERE RDN1.DocEntry = RDN3.DocEntry) as VatSum
					FROM RDN3 WITH(NOLOCK) 
					GROUP BY RDN3.DocEntry
				) FRETE
			ON FRETE.DocEntry = ORDN.DocEntry
	WHERE DIME.Code = @Code
	AND ORDN.DocDate BETWEEN DIME.U_DtDe AND DIME.U_DtAte
	AND ORDN.CANCELED = 'N'
	AND ORDN.Model NOT IN (0, 28, 37, 3, 57)

END
