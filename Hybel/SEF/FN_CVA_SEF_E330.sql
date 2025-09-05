IF EXISTS (SELECT * FROM sys.objects WHERE type = 'IF' AND name = 'FN_CVA_SEF_E330')
	DROP FUNCTION FN_CVA_SEF_E330
GO
CREATE FUNCTION FN_CVA_SEF_E330
(
	@Filial			INT,
	@DataInicial	DATETIME,
	@DataFinal		DATETIME
) RETURNS TABLE
AS
RETURN
(
	WITH ICMS AS
	(
		SELECT 
			SUBSTRING(PDN1.CFOPCode, 1, 1)	IND_TOT,
			SUM(PDN1.LineTotal)				VL_CONT,
			SUM(ISS.BaseSum)				VL_OP_ISS,
			SUM(ICMS.BaseSum)				VL_BC_ICMS,
			SUM(ICMS.TaxSum)				VL_ICMS,
			SUM(ICMS_ST.TaxSum)				VL_ICMS_ST,
			SUM(ICMS.Isentas)				VL_ISNT_ICMS,
			SUM(ICMS.Outras)				VL_OUT_ICMS
		FROM OPDN WITH(NOLOCK)
			INNER JOIN PDN1 WITH(NOLOCK)
				ON PDN1.DocEntry = OPDN.DocEntry
			LEFT JOIN (
						SELECT PDN4.DocEntry, PDN4.LineNum, SUM(PDN4.TaxSum) TaxSum, SUM(PDN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
						FROM PDN4 WITH(NOLOCK) 
							INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType
							INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
						GROUP BY PDN4.DocEntry, PDN4.LineNum
					) ISS
				ON ISS.DocEntry = PDN1.DocEntry
				AND ISS.LineNum = PDN1.LineNum
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
	
		WHERE OPDN.DocDate BETWEEN @DataInicial AND @DataFinal
		AND OPDN.BPLId = @Filial
		AND ISNULL(PDN1.TargetType, 0) <> 18
		AND OPDN.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

		GROUP BY
			SUBSTRING(PDN1.CFOPCode, 1, 1)

		UNION ALL

		SELECT 
			SUBSTRING(PCH1.CFOPCode, 1, 1)	IND_TOT,
			SUM(PCH1.LineTotal)				VL_CONT,
			SUM(ISS.BaseSum)				VL_OP_ISS,
			SUM(ICMS.BaseSum)				VL_BC_ICMS,
			SUM(ICMS.TaxSum)				VL_ICMS,
			SUM(ICMS_ST.TaxSum)				VL_ICMS_ST,
			SUM(ICMS.Isentas)				VL_ISNT_ICMS,
			SUM(ICMS.Outras)				VL_OUT_ICMS
		FROM OPCH WITH(NOLOCK)
			INNER JOIN PCH1 WITH(NOLOCK)
				ON PCH1.DocEntry = OPCH.DocEntry
			LEFT JOIN (
						SELECT PCH4.DocEntry, PCH4.LineNum, SUM(PCH4.TaxSum) TaxSum, SUM(PCH4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
						FROM PCH4 WITH(NOLOCK) 
							INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
							INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
						GROUP BY PCH4.DocEntry, PCH4.LineNum
					) ISS
				ON ISS.DocEntry = PCH1.DocEntry
				AND ISS.LineNum = PCH1.LineNum
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
	
		WHERE OPCH.DocDate BETWEEN @DataInicial AND @DataFinal
		AND OPCH.BPLId = @Filial
		AND OPCH.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

		GROUP BY
			SUBSTRING(PCH1.CFOPCode, 1, 1)

		UNION ALL

		SELECT 
			SUBSTRING(RDN1.CFOPCode, 1, 1)	IND_TOT,
			SUM(RDN1.LineTotal)				VL_CONT,
			SUM(ISS.BaseSum)				VL_OP_ISS,
			SUM(ICMS.BaseSum)				VL_BC_ICMS,
			SUM(ICMS.TaxSum)				VL_ICMS,
			SUM(ICMS_ST.TaxSum)				VL_ICMS_ST,
			SUM(ICMS.Isentas)				VL_ISNT_ICMS,
			SUM(ICMS.Outras)				VL_OUT_ICMS
		FROM ORDN WITH(NOLOCK)
			INNER JOIN RDN1 WITH(NOLOCK)
				ON RDN1.DocEntry = ORDN.DocEntry
			LEFT JOIN (
						SELECT RDN4.DocEntry, RDN4.LineNum, SUM(RDN4.TaxSum) TaxSum, SUM(RDN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
						FROM RDN4 WITH(NOLOCK) 
							INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType
							INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
						GROUP BY RDN4.DocEntry, RDN4.LineNum
					) ISS
				ON ISS.DocEntry = RDN1.DocEntry
				AND ISS.LineNum = RDN1.LineNum
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
	
		WHERE ORDN.DocDate BETWEEN @DataInicial AND @DataFinal
		AND ORDN.BPLId = @Filial
		AND ORDN.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

		GROUP BY
			SUBSTRING(RDN1.CFOPCode, 1, 1)
	
		UNION ALL

		SELECT 
			SUBSTRING(RIN1.CFOPCode, 1, 1)	IND_TOT,
			SUM(RIN1.LineTotal)				VL_CONT,
			SUM(ISS.BaseSum)				VL_OP_ISS,
			SUM(ICMS.BaseSum)				VL_BC_ICMS,
			SUM(ICMS.TaxSum)				VL_ICMS,
			SUM(ICMS_ST.TaxSum)				VL_ICMS_ST,
			SUM(ICMS.Isentas)				VL_ISNT_ICMS,
			SUM(ICMS.Outras)				VL_OUT_ICMS
		FROM ORIN WITH(NOLOCK)
			INNER JOIN RIN1 WITH(NOLOCK)
				ON RIN1.DocEntry = ORIN.DocEntry
			LEFT JOIN (
						SELECT RIN4.DocEntry, RIN4.LineNum, SUM(RIN4.TaxSum) TaxSum, SUM(RIN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
						FROM RIN4 WITH(NOLOCK) 
							INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType
							INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
						GROUP BY RIN4.DocEntry, RIN4.LineNum
					) ISS
				ON ISS.DocEntry = RIN1.DocEntry
				AND ISS.LineNum = RIN1.LineNum
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
	
		WHERE ORIN.DocDate BETWEEN @DataInicial AND @DataFinal
		AND ORIN.BPLId = @Filial
		AND ORIN.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

		GROUP BY
			SUBSTRING(RIN1.CFOPCode, 1, 1)

		UNION ALL

		SELECT 
			SUBSTRING(DLN1.CFOPCode, 1, 1)	IND_TOT,
			SUM(DLN1.LineTotal)				VL_CONT,
			SUM(ISS.BaseSum)				VL_OP_ISS,
			SUM(ICMS.BaseSum)				VL_BC_ICMS,
			SUM(ICMS.TaxSum)				VL_ICMS,
			SUM(ICMS_ST.TaxSum)				VL_ICMS_ST,
			SUM(ICMS.Isentas)				VL_ISNT_ICMS,
			SUM(ICMS.Outras)				VL_OUT_ICMS
		FROM ODLN WITH(NOLOCK)
			INNER JOIN DLN1 WITH(NOLOCK)
				ON DLN1.DocEntry = ODLN.DocEntry
			LEFT JOIN (
						SELECT DLN4.DocEntry, DLN4.LineNum, SUM(DLN4.TaxSum) TaxSum, SUM(DLN4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
						FROM DLN4 WITH(NOLOCK) 
							INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = DLN4.StaType
							INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
						GROUP BY DLN4.DocEntry, DLN4.LineNum
					) ISS
				ON ISS.DocEntry = DLN1.DocEntry
				AND ISS.LineNum = DLN1.LineNum
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
				ON ICMS_ST.DocEntry = DLN1.DocEntry
				AND ICMS_ST.LineNum = DLN1.LineNum
	
		WHERE ODLN.DocDate BETWEEN @DataInicial AND @DataFinal
		AND ODLN.BPLId = @Filial
		AND ISNULL(DLN1.TargetType, 0) <> 13
		AND ODLN.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

		GROUP BY
			SUBSTRING(DLN1.CFOPCode, 1, 1)

		UNION ALL

		SELECT 
			SUBSTRING(INV1.CFOPCode, 1, 1)	IND_TOT,
			SUM(INV1.LineTotal)				VL_CONT,
			SUM(ISS.BaseSum)				VL_OP_ISS,
			SUM(ICMS.BaseSum)				VL_BC_ICMS,
			SUM(ICMS.TaxSum)				VL_ICMS,
			SUM(ICMS_ST.TaxSum)				VL_ICMS_ST,
			SUM(ICMS.Isentas)				VL_ISNT_ICMS,
			SUM(ICMS.Outras)				VL_OUT_ICMS
		FROM OINV WITH(NOLOCK)
			INNER JOIN INV1 WITH(NOLOCK)
				ON INV1.DocEntry = OINV.DocEntry
			LEFT JOIN (
						SELECT INV4.DocEntry, INV4.LineNum, SUM(INV4.TaxSum) TaxSum, SUM(INV4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
						FROM INV4 WITH(NOLOCK) 
							INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
							INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
						GROUP BY INV4.DocEntry, INV4.LineNum
					) ISS
				ON ISS.DocEntry = INV1.DocEntry
				AND ISS.LineNum = INV1.LineNum
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
	
		WHERE OINV.DocDate BETWEEN @DataInicial AND @DataFinal
		AND OINV.BPLId = @Filial
		AND OINV.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

		GROUP BY
			SUBSTRING(INV1.CFOPCode, 1, 1)

		UNION ALL
		
		SELECT 
			SUBSTRING(RPD1.CFOPCode, 1, 1)	IND_TOT,
			SUM(RPD1.LineTotal)				VL_CONT,
			SUM(ISS.BaseSum)				VL_OP_ISS,
			SUM(ICMS.BaseSum)				VL_BC_ICMS,
			SUM(ICMS.TaxSum)				VL_ICMS,
			SUM(ICMS_ST.TaxSum)				VL_ICMS_ST,
			SUM(ICMS.Isentas)				VL_ISNT_ICMS,
			SUM(ICMS.Outras)				VL_OUT_ICMS
		FROM ORPD WITH(NOLOCK)
			INNER JOIN RPD1 WITH(NOLOCK)
				ON RPD1.DocEntry = ORPD.DocEntry
			LEFT JOIN (
						SELECT RPD4.DocEntry, RPD4.LineNum, SUM(RPD4.TaxSum) TaxSum, SUM(RPD4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
						FROM RPD4 WITH(NOLOCK) 
							INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPD4.StaType
							INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
						GROUP BY RPD4.DocEntry, RPD4.LineNum
					) ISS
				ON ISS.DocEntry = RPD1.DocEntry
				AND ISS.LineNum = RPD1.LineNum
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
	
		WHERE ORPD.DocDate BETWEEN @DataInicial AND @DataFinal
		AND ORPD.BPLId = @Filial
		AND ORPD.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

		GROUP BY
			SUBSTRING(RPD1.CFOPCode, 1, 1)

		UNION ALL
		
		SELECT 
			SUBSTRING(RPC1.CFOPCode, 1, 1)	IND_TOT,
			SUM(RPC1.LineTotal)				VL_CONT,
			SUM(ISS.BaseSum)				VL_OP_ISS,
			SUM(ICMS.BaseSum)				VL_BC_ICMS,
			SUM(ICMS.TaxSum)				VL_ICMS,
			SUM(ICMS_ST.TaxSum)				VL_ICMS_ST,
			SUM(ICMS.Isentas)				VL_ISNT_ICMS,
			SUM(ICMS.Outras)				VL_OUT_ICMS
		FROM ORPC WITH(NOLOCK)
			INNER JOIN RPC1 WITH(NOLOCK)
				ON RPC1.DocEntry = ORPC.DocEntry
			LEFT JOIN (
						SELECT RPC4.DocEntry, RPC4.LineNum, SUM(RPC4.TaxSum) TaxSum, SUM(RPC4.BaseSum) BaseSum, SUM(U_ExcAmtS) Isentas, SUM(U_OthAmtS) Outras
						FROM RPC4 WITH(NOLOCK) 
							INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RPC4.StaType
							INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
						GROUP BY RPC4.DocEntry, RPC4.LineNum
					) ISS
				ON ISS.DocEntry = RPC1.DocEntry
				AND ISS.LineNum = RPC1.LineNum
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
	
		WHERE ORPC.DocDate BETWEEN @DataInicial AND @DataFinal
		AND ORPC.BPLId = @Filial
		AND ORPC.CANCELED = 'N' AND Model IN (1, 3, 5, 39)

		GROUP BY
			SUBSTRING(RPC1.CFOPCode, 1, 1)
	)
	, Grouped AS
	(
		SELECT 
			IND_TOT,
			SUM(VL_CONT)		VL_CONT,
			SUM(VL_OP_ISS)		VL_OP_ISS,
			SUM(VL_BC_ICMS)		VL_BC_ICMS,
			SUM(VL_ICMS)		VL_ICMS,
			SUM(VL_ICMS_ST)		VL_ICMS_ST,
			SUM(VL_ISNT_ICMS)	VL_ISNT_ICMS,
			SUM(VL_OUT_ICMS)	VL_OUT_ICMS
		FROM ICMS
		WHERE IND_TOT IN (1, 2, 3)
		GROUP BY IND_TOT

		UNION ALL

		SELECT 
			4,
			SUM(VL_CONT)		VL_CONT,
			SUM(VL_OP_ISS)		VL_OP_ISS,
			SUM(VL_BC_ICMS)		VL_BC_ICMS,
			SUM(VL_ICMS)		VL_ICMS,
			SUM(VL_ICMS_ST)		VL_ICMS_ST,
			SUM(VL_ISNT_ICMS)	VL_ISNT_ICMS,
			SUM(VL_OUT_ICMS)	VL_OUT_ICMS
		FROM ICMS
		WHERE IND_TOT IN (1, 2, 3)

		UNION ALL

		SELECT 
			IND_TOT,
			SUM(VL_CONT)		VL_CONT,
			SUM(VL_OP_ISS)		VL_OP_ISS,
			SUM(VL_BC_ICMS)		VL_BC_ICMS,
			SUM(VL_ICMS)		VL_ICMS,
			SUM(VL_ICMS_ST)		VL_ICMS_ST,
			SUM(VL_ISNT_ICMS)	VL_ISNT_ICMS,
			SUM(VL_OUT_ICMS)	VL_OUT_ICMS
		FROM ICMS
		WHERE IND_TOT IN (5, 6, 7)
		GROUP BY IND_TOT

		UNION ALL

		SELECT 
			8,
			SUM(VL_CONT)		VL_CONT,
			SUM(VL_OP_ISS)		VL_OP_ISS,
			SUM(VL_BC_ICMS)		VL_BC_ICMS,
			SUM(VL_ICMS)		VL_ICMS,
			SUM(VL_ICMS_ST)		VL_ICMS_ST,
			SUM(VL_ISNT_ICMS)	VL_ISNT_ICMS,
			SUM(VL_OUT_ICMS)	VL_OUT_ICMS
		FROM ICMS
		WHERE IND_TOT IN (5, 6, 7)
	)
	SELECT *, 
		0.00	VL_ST_ENT,
		0.00	VL_ST_FNT,
		0.00	VL_ST_UF,
		0.00	VL_ST_OE,
		0.00	VL_AT
	FROM Grouped 
	WHERE ISNULL(VL_CONT, 0) > 0
)
