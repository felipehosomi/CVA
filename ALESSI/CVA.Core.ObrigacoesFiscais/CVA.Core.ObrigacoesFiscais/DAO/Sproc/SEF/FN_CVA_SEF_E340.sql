IF EXISTS (SELECT * FROM sys.objects WHERE type = 'TF' AND name = 'FN_CVA_SEF_E340')
	DROP FUNCTION FN_CVA_SEF_E340
GO

--- SELECT * FROM FN_CVA_SEF_E340 (9,'2018-01-01','2018-01-31')
CREATE FUNCTION FN_CVA_SEF_E340
(
	@Filial			INT,
	@DataInicial	DATETIME,
	@DataFinal		DATETIME
) RETURNS @DOCS TABLE

	(


	VL_01				DECIMAL(19,2),
	VL_02				DECIMAL(19,2),
	VL_03				DECIMAL(19,2),
	VL_04				DECIMAL(19,2),
	VL_05				DECIMAL(19,2),
	VL_06				DECIMAL(19,2),
	VL_07				DECIMAL(19,2),
	VL_08				DECIMAL(19,2),
	VL_09				DECIMAL(19,2),
	VL_10				DECIMAL(19,2),
	VL_11				DECIMAL(19,2),
	VL_12				DECIMAL(19,2),
	VL_13				DECIMAL(19,2),
	VL_14				DECIMAL(19,2),
	VL_15				DECIMAL(19,2),
	VL_16				DECIMAL(19,2),
	VL_17				DECIMAL(19,2),
	VL_18				DECIMAL(19,2),
	VL_19				DECIMAL(19,2),
	VL_20				DECIMAL(19,2),
	VL_21				DECIMAL(19,2),
	VL_22				DECIMAL(19,2),
	VL_99				DECIMAL(19,2)

	)


AS
BEGIN

DECLARE @DOCS2 TABLE

	(

	VL_01				DECIMAL(19,2),
	VL_02				DECIMAL(19,2),
	VL_03				DECIMAL(19,2),
	VL_04				DECIMAL(19,2),
	VL_05				DECIMAL(19,2),
	VL_06				DECIMAL(19,2),
	VL_07				DECIMAL(19,2),
	VL_08				DECIMAL(19,2),
	VL_09				DECIMAL(19,2),
	VL_10				DECIMAL(19,2),
	VL_11				DECIMAL(19,2),
	VL_12				DECIMAL(19,2),
	VL_13				DECIMAL(19,2),
	VL_14				DECIMAL(19,2),
	VL_15				DECIMAL(19,2),
	VL_16				DECIMAL(19,2),
	VL_17				DECIMAL(19,2),
	VL_18				DECIMAL(19,2),
	VL_19				DECIMAL(19,2),
	VL_20				DECIMAL(19,2),
	VL_21				DECIMAL(19,2),
	VL_22				DECIMAL(19,2),
	VL_99				DECIMAL(19,2)

	)


INSERT INTO @DOCS2
	SELECT 
	/*VL_01*/ (SELECT VL_ICMS FROM FN_CVA_SEF_E330 (@Filial,@DataInicial,@DataFinal) WHERE IND_TOT = '4'),
	/*VL_02*/ 0.00,
	/*VL_03*/ 0.00,
	/*VL_04*/ 0.00,
	/*VL_05*/ (SELECT ISNULL(SUM(VL_AJ),0.0) FROM FN_CVA_SEF_E350 (@Filial,@DataInicial,@DataFinal) WHERE COD_AJ = 199),
	/*VL_06*/ 0.00,
	/*VL_07*/ ISNULL(ICMS_ANT.U_Saldo,0),
	/*VL_08*/ 0.00, /*VALOR CALCULADO*/
	/*VL_09*/ (SELECT VL_ICMS FROM FN_CVA_SEF_E330 (@Filial,@DataInicial,@DataFinal) WHERE IND_TOT = '8'),
	/*VL_10*/ 0.00,
	/*VL_11*/ 0.00,
	/*VL_12*/ 0.00, /*VALOR CALCULADO*/
	/*VL_13*/ 0.00, /*VALOR CALCULADO*/
	/*VL_14*/ 0.00, /*VALOR CALCULADO*/
	/*VL_15*/ 0.00, 
	/*VL_16*/ 0.00, /*VALOR CALCULADO*/
	/*VL_17*/ (SELECT VL_ICMS_ST FROM FN_CVA_SEF_E330 (@Filial,@DataInicial,@DataFinal) WHERE IND_TOT = '4'),
	/*VL_18*/ 0.00,
	/*VL_19*/ (SELECT VL_ST_UF FROM FN_CVA_SEF_E330 (@Filial,@DataInicial,@DataFinal) WHERE IND_TOT = '8'),
	/*VL_20*/ 0.00,
	/*VL_21*/ 0.00,
	/*VL_22*/ 0.00, /*VALOR CALCULADO*/
	/*VL_99*/ (SELECT VL_ST_OE FROM FN_CVA_SEF_E330 (@Filial,@DataInicial,@DataFinal) WHERE IND_TOT = '8')

	FROM OBPL WITH(NOLOCK)
		LEFT JOIN [@SKILL_005_APR1] ICMS_ANT WITH(NOLOCK)
					ON ICMS_ANT.U_TipoImposto = 'ICMS' 
					AND ICMS_ANT.U_DtInicio = DATEADD(MONTH, -1, @DataInicial)
					AND ICMS_ANT.U_BPLId = OBPL.BPLId
		WHERE OBPL.BPLId = @Filial

INSERT INTO @DOCS
	SELECT 
		/*VL_01*/ VL_01,
		/*VL_02*/ VL_02,
		/*VL_03*/ VL_03,
		/*VL_04*/ VL_04,
		/*VL_05*/ VL_05,
		/*VL_06*/ VL_06,
		/*VL_07*/ VL_07,
		/*VL_08*/ VL_01 + VL_02 + VL_03 + VL_04 + VL_05 + VL_06 + VL_07,
		/*VL_09*/ VL_09,
		/*VL_10*/ VL_10,
		/*VL_11*/ VL_11,
		/*VL_12*/ VL_09 + VL_10 + VL_11,
		/*VL_13*/ CASE WHEN 
					(VL_01 + VL_02 + VL_03 + VL_04 + VL_05 + VL_06 + VL_07) > --- VL_08
					( VL_09 + VL_10 + VL_11) -- Vl_12
					THEN (VL_01 + VL_02 + VL_03 + VL_04 + VL_05 + VL_06 + VL_07) - ( VL_09 + VL_10 + VL_11) -- VL_08 - VL_12
					ELSE 0.00 END ,
		/*VL_14*/ CASE WHEN 
					( VL_09 + VL_10 + VL_11) > -- Vl_12
					(VL_01 + VL_02 + VL_03 + VL_04 + VL_05 + VL_06 + VL_07) --- VL_08
					THEN ( VL_09 + VL_10 + VL_11) - (VL_01 + VL_02 + VL_03 + VL_04 + VL_05 + VL_06 + VL_07)  -- VL_12 - VL_08
					ELSE 0.00 END,
		/*VL_15*/ VL_06,
		/*VL_16*/ CASE WHEN (
							(CASE WHEN 
								( VL_09 + VL_10 + VL_11) > -- Vl_12
								(VL_01 + VL_02 + VL_03 + VL_04 + VL_05 + VL_06 + VL_07) --- VL_08
								THEN ( VL_09 + VL_10 + VL_11) - (VL_01 + VL_02 + VL_03 + VL_04 + VL_05 + VL_06 + VL_07)  -- VL_12 - VL_08
								ELSE 0.00 END))
						> VL_15 THEN 
							(CASE WHEN 
								( VL_09 + VL_10 + VL_11) > -- Vl_12
								(VL_01 + VL_02 + VL_03 + VL_04 + VL_05 + VL_06 + VL_07) --- VL_08
								THEN ( VL_09 + VL_10 + VL_11) - (VL_01 + VL_02 + VL_03 + VL_04 + VL_05 + VL_06 + VL_07)  -- VL_12 - VL_08
								ELSE 0.00 END) - VL_15 
							ELSE  0.00 END,
		/*VL_17*/ VL_17,
		/*VL_18*/ VL_18,
		/*VL_19*/ VL_19,
		/*VL_20*/ VL_20,
		/*VL_21*/ VL_21,
		/*VL_22*/ (CASE WHEN (
							(CASE WHEN 
								( VL_09 + VL_10 + VL_11) > -- Vl_12
								(VL_01 + VL_02 + VL_03 + VL_04 + VL_05 + VL_06 + VL_07) --- VL_08
								THEN ( VL_09 + VL_10 + VL_11) - (VL_01 + VL_02 + VL_03 + VL_04 + VL_05 + VL_06 + VL_07)  -- VL_12 - VL_08
								ELSE 0.00 END))
						> VL_15 THEN 
							(CASE WHEN 
								( VL_09 + VL_10 + VL_11) > -- Vl_12
								(VL_01 + VL_02 + VL_03 + VL_04 + VL_05 + VL_06 + VL_07) --- VL_08
								THEN ( VL_09 + VL_10 + VL_11) - (VL_01 + VL_02 + VL_03 + VL_04 + VL_05 + VL_06 + VL_07)  -- VL_12 - VL_08
								ELSE 0.00 END) - VL_15 
							ELSE  0.00 END)
					+ VL_17 + VL_18 + VL_19 + VL_20 + VL_21,
		/*VL_99*/ VL_99
		
		FROM @DOCS2

RETURN 

END
