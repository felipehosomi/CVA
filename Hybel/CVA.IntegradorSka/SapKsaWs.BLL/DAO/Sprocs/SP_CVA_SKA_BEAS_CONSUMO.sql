IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_SKA_BEAS_CONSUMO')
	DROP PROCEDURE SP_CVA_SKA_BEAS_CONSUMO
GO
CREATE PROCEDURE SP_CVA_SKA_BEAS_CONSUMO
(
	@IdMes		INT,
	@BelPosId	INT,
	@PosId		INT
)
AS
BEGIN
	SELECT 
	'WOIM' AS 'TransactionType'
	,Convert(Char(10),MES.DATAFIM,121) AS 'DocumentValidDate'
	,'' AS 'DocumentRemarks'
	,'' AS 'BusinessPartner'
	,'' AS 'Project'
	,'' AS 'LinkDocumentType'
	,MES.OP AS 'LinkDocument'
	,STL.BELPOS_ID AS 'LinkLine1'
	,STL.POS_ID AS 'LinkLine2'
	,STL.ART1_ID AS 'Itemcode'
	, CASE WHEN ISNULL(RES.Quantity, 0) <> 0
		THEN RES.Quantity
		ELSE(STL.MENGE_VERBRAUCH/STL.MENGE_JE) * MES.QUANT 
	END AS 'Quantity'
	,STL.WhsCode AS 'Warehouse'
	,'' AS 'Bincode'
	,'' AS 'RFID'
	,'' AS 'Version',
	CASE WHEN ISNULL(RES.BatchNum, '') <> '' AND RES.BatchNum <> '<>'
		THEN RES.BatchNum
		ELSE Batch.BatchNum
	END AS 'Batchnumber'
	,'' AS 'SerialNumberInternal'
	,'' AS 'BatchAttribute1'
	,'' AS 'BatchAttribute2'
	,'' AS 'DateOfEntry'
	,'' AS 'ManufacturingDate'
	,'' AS 'ExpirDate'
	,'' AS 'PersonnelNumber'
	,'' AS 'UserWEBUser'
	,'' AS 'Price'
	,'' AS 'Currency'
	,'' AS 'WorkStation'
	,'' AS 'Userfield1'
	,'' AS 'Userfield2'
	,'' AS 'Userfield3'
	,'' AS 'Userfield4'
	,'' AS 'Freetext'
	, Batch.Quantity 'BatchQuantity'
	FROM BEAS_FTSTL STL WITH(NOLOCK)
		INNER JOIN MES..SSPExportProd MES WITH(NOLOCK)
			ON CAST(STL.BELNR_ID AS VARCHAR (10)) = MES.OP 
			AND CAST(STL.BELPOS_ID AS VARCHAR (10)) = MES.BELPOS_ID
			AND CAST(STL.APLANPOS_ID AS VARCHAR (10)) = MES.POS_ID
		LEFT JOIN BEAS_RESERVATION_LINE RES
			ON RES.BASE_DOCENTRY = STL.BELNR_ID
			AND RES.BASE_LINENUM = STL.BELPOS_ID
			AND RES.BASE_LINENUM2 = STL.POS_ID
			AND RES.LINK_TYPE = 'WO'
		LEFT JOIN 
		(
			SELECT OITL.ItemCode, OITL.LocCode WhsCode, OBTN.DistNumber BatchNum, SUM(ITL1.Quantity) Quantity
			FROM		OITL WITH(NOLOCK)
					INNER JOIN	ITL1 WITH(NOLOCK) ON ITL1.LogEntry		= OITL.LogEntry 
					INNER JOIN	OBTN WITH(NOLOCK) ON OBTN.AbsEntry		= ITL1.MdAbsEntry
			GROUP BY OITL.ItemCode, OITL.LocCode, OBTN.DistNumber
			HAVING SUM(ITL1.Quantity) > 0
		) Batch ON Batch.ItemCode = STL.ART1_ID AND Batch.WhsCode = STL.WhsCode AND ISNULL(RES.BatchNum, '') = ''
	WHERE MES.id = @IdMes
	AND 
	(
		STL.APLANPOS_ID = @PosId
		OR -- Se for a última posição, pega os que estão nulo
		(
			@BelPosId =
			(
				SELECT MAX(BEAS.BELPOS_ID) FROM BEAS_FTAPL BEAS
				WHERE CAST(BEAS.BELNR_ID AS VARCHAR (10)) = MES.OP 
			) 
			AND STL.APLANPOS_ID IS NULL
		)
	)
	ORDER BY Batch.BatchNum
END