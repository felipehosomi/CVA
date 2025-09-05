IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_DCIP_SIMPLES')
	DROP PROCEDURE SP_CVA_DCIP_SIMPLES
GO
CREATE PROCEDURE SP_CVA_DCIP_SIMPLES
(
	@Filial		INT,
	@DataDe		DATETIME,
	@DataAte	DATETIME
)
AS
BEGIN
	CREATE TABLE #Aliquota
	(
		Aliq		NUMERIC(10, 2),
		ValorDe		NUMERIC(19, 2),
		ValorAte	NUMERIC(19, 2)
	)

	INSERT INTO #Aliquota
	VALUES	(1.25, 0, 180000),
			(1.86, 180000.01, 360000),
			(2.33, 360000.01, 540000),
			(2.56, 540000.01, 720000),
			(2.58, 720000.01, 900000),
			(2.82, 900000.01, 1080000),
			(2.84, 1080000.01, 1260000),
			(2.87, 1260000.01, 1440000),
			(3.07, 1440000.01, 1620000),
			(3.10, 1620000.01, 1800000),
			(3.38, 1800000.01, 1980000),
			(3.41, 1980000.01, 2160000),
			(3.45, 2160000.01, 2340000),
			(3.48, 2340000.01, 2520000),
			(3.51, 2520000.01, 2700000),
			(3.82, 2700000.01, 2880000),
			(3.85, 2880000.01, 3060000),
			(3.88, 3060000.01, 3240000),
			(3.91, 3240000.01, 3420000),
			(3.95, 3420000.01, 3600000),
			(7.00, 3600000, 99999999999999)

	SELECT
		OBPL.TaxIdNum2		IEFilial,
		PCH12.TaxId1		IE,
		@DataDe				Periodo,
		'120'				Tipo,
		PCH12.TaxId0		CNPJ,
		PCH12.StateS		UF,
		OPCH.SeriesStr		Serie,
		OPCH.Serial			NrNota,
		OPCH.DocDate		Data,
		PCH1.CFOPCode		CFOP,
		OPCH.DocTotal		ValorTotal,
		OPCH.DocTotal		BaseCalculo,
		0.00				AliqICMS,
		CASE ONFM.NfmCode
			WHEN '1' THEN 1
			WHEN '1A' THEN 2
			WHEN '1AF' THEN 34
			WHEN '1F' THEN 35
			WHEN '55' THEN 55
		END					ModeloNF
	INTO #NotasFiscais
	FROM OPCH WITH(NOLOCK)
		INNER JOIN OCRD WITH(NOLOCK)
			ON OCRD.CardCode = OPCH.CardCode
			AND OCRD.u_h_simples_nacional = 1
		INNER JOIN PCH12 WITH(NOLOCK)
			ON PCH12.DocEntry = OPCH.DocEntry
		INNER JOIN OBPL WITH(NOLOCK)
			ON OBPL.BPLId = OPCH.BPLId
		INNER JOIN PCH1 WITH(NOLOCK)
			ON PCH1.DocEntry = OPCH.DocEntry
			--AND PCH1.CFOPCode IN (	'5101', '5102', '6101', '6102',
			--						'5103', '5104', '6103', '6104',
			--						'5105', '5106', '6105', '6106',
			--						'5109', '5110', '6107', '6108',
			--						'5111', '5112', '6109', '6110',
			--						'5113', '5114', '6111', '6112',
			--						'5116', '5115', '6113', '6114',
			--						'5118', '5117', '6116', '6115',
			--						'5122', '5119', '6118', '6117',
			--						'5124', '5120', '6122', '6119',
			--						'5125', '5123', '6124', '6120',
			--						'5401', '5403', '6125', '6123',
			--						'5402', '5405', '6401', '6403',
			--						'6402', '6404')
		INNER JOIN ONFM WITH(NOLOCK)
			ON ONFM.AbsEntry = OPCH.Model
	WHERE 
	NOT EXISTS
	(
		SELECT * FROM ORPC WITH(NOLOCK)
			INNER JOIN RPC1 WITH(NOLOCK)
				ON RPC1.DocEntry = ORPC.DocEntry
		WHERE ORPC.CANCELED <> 'C' 
		AND RPC1.BaseType = 18
		AND RPC1.BaseEntry = PCH1.DocEntry
		AND RPC1.BaseLine = PCH1.LineNum
	)
	AND OPCH.BPLId = @Filial
	AND OPCH.DocDate BETWEEN @DataDe AND @DataAte

	SELECT * FROM #NotasFiscais

DROP TABLE #Aliquota
DROP TABLE #NotasFiscais

END

