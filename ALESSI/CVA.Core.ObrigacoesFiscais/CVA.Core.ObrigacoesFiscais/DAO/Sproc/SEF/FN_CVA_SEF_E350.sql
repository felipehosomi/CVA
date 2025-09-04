IF EXISTS (SELECT * FROM sys.objects WHERE type = 'TF' AND name = 'FN_CVA_SEF_E350')
	DROP FUNCTION FN_CVA_SEF_E350
GO

--- select * from FN_CVA_SEF_E350 (9, '2018-01-01', '2018-01-31')
CREATE FUNCTION FN_CVA_SEF_E350
(
	@Filial			INT,
	@DataInicial	DATETIME,
	@DataFinal		DATETIME
) RETURNS @DOCS TABLE

	(

	UF_AJ				VARCHAR(2),
	COD_AJ				VARCHAR(3),
	VL_AJ				DECIMAL(19,2),
	NUM_DA				VARCHAR(100),
	NUM_PROC			VARCHAR(100),
	IND_PROC			VARCHAR(100),
	DESCR_PROC			VARCHAR(100),
	COD_INF_OBS			VARCHAR(100),
	IND_AP				VARCHAR(100)

	)


AS
BEGIN

DECLARE @DOCS2 TABLE

	(

	UF_AJ				VARCHAR(2),
	COD_AJ				VARCHAR(3),
	VL_AJ				DECIMAL(19,2),
	NUM_DA				VARCHAR(100),
	NUM_PROC			VARCHAR(100),
	IND_PROC			VARCHAR(100),
	DESCR_PROC			VARCHAR(100),
	COD_INF_OBS			VARCHAR(100),
	IND_AP				VARCHAR(100)

	)

DECLARE @DOCS3 TABLE

	(

	DOCENTRY			VARCHAR(50),
	OBJTYPE				INT,
	UF					VARCHAR(50),
	VL_ICMS				DECIMAL(19,2),
	CFOP				VARCHAR(4)

	)


INSERT INTO @DOCS3
	SELECT DOCENTRY, OBJTYPE, UF, VL_ICMS, CFOP  FROM FN_CVA_SEF_E020 (@Filial,@DataInicial, @DataFinal) 
	UNION ALL 
	SELECT DOCENTRY, OBJTYPE, UF, VL_ICMS, CFOP  FROM FN_CVA_SEF_E100 (@Filial,@DataInicial, @DataFinal) 
	UNION ALL 
	SELECT DOCENTRY, OBJTYPE, UF, VL_ICMS, CFOP FROM FN_CVA_SEF_E120  (@Filial,@DataInicial, @DataFinal) 			



INSERT INTO @DOCS2

		SELECT
		(SELECT STATE FROM OBPL WHERE BPLID = @Filial)	UF_AJ,
		'000'											COD_AJ,
		SUM(VL_ICMS)									VL_AJ,
		''	NUM_DA,
		''	NUM_PROC,
		''	IND_PROC,
		''	DESCR_PROC,
		''	COD_INF_OBS,
		''	IND_AP
	FROM @DOCS3 
	WHERE 0=0
		AND CFOP LIKE '1%'  

INSERT INTO @DOCS2

		SELECT
		(SELECT STATE FROM OBPL WHERE BPLID = @Filial)	UF_AJ,
		'001'											COD_AJ,
		SUM(VL_ICMS)									VL_AJ,
		''	NUM_DA,
		''	NUM_PROC,
		''	IND_PROC,
		''	DESCR_PROC,
		''	COD_INF_OBS,
		''	IND_AP
	FROM @DOCS3 
	WHERE 0=0
		AND CFOP LIKE '2%'  

INSERT INTO @DOCS2

		SELECT
		(SELECT STATE FROM OBPL WHERE BPLID = @Filial)	UF_AJ,
		'050'											COD_AJ,
		SUM(VL_ICMS)									VL_AJ,
		''	NUM_DA,
		''	NUM_PROC,
		''	IND_PROC,
		''	DESCR_PROC,
		''	COD_INF_OBS,
		''	IND_AP
	FROM @DOCS3 
	WHERE 0=0
		AND CFOP LIKE '5%'  

INSERT INTO @DOCS2

		SELECT
		(SELECT STATE FROM OBPL WHERE BPLID = @Filial)	UF_AJ,
		'051'											COD_AJ,
		SUM(VL_ICMS)									VL_AJ,
		''	NUM_DA,
		''	NUM_PROC,
		''	IND_PROC,
		''	DESCR_PROC,
		''	COD_INF_OBS,
		''	IND_AP
	FROM @DOCS3 
	WHERE 0=0
		AND CFOP LIKE '6%'  

INSERT INTO @DOCS2

		SELECT
		(SELECT STATE FROM OBPL WHERE BPLID = @Filial)	UF_AJ,
		'199'											COD_AJ,
		U_VlOperacao									VL_AJ,
		''	NUM_DA,
		''	NUM_PROC,
		''	IND_PROC,
		''	DESCR_PROC,
		''	COD_INF_OBS,
		''	IND_AP
	FROM [@SKILL_005_APR3] WITH(NOLOCK) 
	WHERE 0=0
		AND U_DtOperacao BETWEEN @DataInicial AND @DataFinal	
		--AND U_Periodo = (Select SUBSTRING((Convert(Char(10), @DataInicial,112)),0,5) + '-' + SUBSTRING((Convert(Char(10), @DataInicial ,112)),5,2))
		AND U_TipoImposto = 'ICMS'
		AND U_BPLId = @Filial	

INSERT INTO @DOCS2

		SELECT 
			U_UF												UF_AJ, 
			CASE WHEN [@SKILL_005_APR6].U_UF = OBPL.State 
					THEN '060' ELSE '061' END					COD_AJ,
			U_Imposto											VL_AJ,
			''	NUM_DA,
			''	NUM_PROC,
			''	IND_PROC,
			''	DESCR_PROC,
			''	COD_INF_OBS,
			''	IND_AP   

			FROM [@SKILL_005_APR6]
				LEFT JOIN  OBPL ON [@SKILL_005_APR6].U_BPLId = OBPL.BPLId
					WHERE U_BPLId = @Filial 
					AND U_DtInicio BETWEEN @DataInicial AND @DataFinal
					and U_Imposto <> 0.00

INSERT INTO @DOCS2

			SELECT 
			(SELECT STATE FROM OBPL WHERE BPLID = @Filial)		UF_AJ, 
			'760'												COD_AJ,
			U_Imposto											VL_AJ,
			''	NUM_DA,
			''	NUM_PROC,
			''	IND_PROC,
			''	DESCR_PROC,
			''	COD_INF_OBS,
			''	IND_AP   

			FROM [@SKILL_005_APR1]
				LEFT JOIN  OBPL ON [@SKILL_005_APR1].U_BPLId = OBPL.BPLId
					WHERE U_BPLId = @Filial 
					AND U_DtInicio BETWEEN @DataInicial AND @DataFinal AND U_TipoImposto = 'ICMS'

INSERT INTO @DOCS2

			SELECT 
			(SELECT STATE FROM OBPL WHERE BPLID = @Filial)		UF_AJ, 
			'765'												COD_AJ,
			U_Imposto											VL_AJ,
			''	NUM_DA,
			''	NUM_PROC,
			''	IND_PROC,
			''	DESCR_PROC,
			''	COD_INF_OBS,
			''	IND_AP   

			FROM [@SKILL_005_APR1]
				LEFT JOIN  OBPL ON [@SKILL_005_APR1].U_BPLId = OBPL.BPLId
					WHERE U_BPLId = @Filial 
					AND U_DtInicio BETWEEN @DataInicial AND @DataFinal AND U_TipoImposto = 'ICMS'




INSERT INTO @DOCS2

			SELECT 
			U_UF												UF_AJ, 
			'795'												COD_AJ,
			U_Imposto											VL_AJ,
			''	NUM_DA,
			''	NUM_PROC,
			''	IND_PROC,
			''	DESCR_PROC,
			''	COD_INF_OBS,
			''	IND_AP   

			FROM [@SKILL_005_APR6]
				LEFT JOIN  OBPL ON [@SKILL_005_APR6].U_BPLId = OBPL.BPLId
					WHERE U_BPLId = @Filial 
					AND U_DtInicio BETWEEN @DataInicial AND @DataFinal
					AND U_Imposto <> 0.00
					AND [@SKILL_005_APR6].U_UF = OBPL.State


INSERT INTO @DOCS2

			SELECT 
			U_UF												UF_AJ, 
			'799'												COD_AJ,
			U_Imposto											VL_AJ,
			''	NUM_DA,
			''	NUM_PROC,
			''	IND_PROC,
			''	DESCR_PROC,
			''	COD_INF_OBS,
			''	IND_AP   

			FROM [@SKILL_005_APR6]
				LEFT JOIN  OBPL ON [@SKILL_005_APR6].U_BPLId = OBPL.BPLId
					WHERE U_BPLId = @Filial 
					AND U_DtInicio BETWEEN @DataInicial AND @DataFinal
					AND U_Imposto <> 0.00
					AND [@SKILL_005_APR6].U_UF <> OBPL.State



INSERT INTO @DOCS
		SELECT * FROM @DOCS2

RETURN 

END


