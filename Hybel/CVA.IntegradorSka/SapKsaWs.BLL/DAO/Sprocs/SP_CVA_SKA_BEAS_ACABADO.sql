IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_SKA_BEAS_ACABADO')
	DROP PROCEDURE SP_CVA_SKA_BEAS_ACABADO
GO
CREATE PROCEDURE SP_CVA_SKA_BEAS_ACABADO
(
	@IdMes		INT
)
AS
BEGIN
	SELECT 
		'WORA' AS 'TransactionType'
		,Convert(Char(10),T0.DATAFIM,121) AS 'DocumentValidDate'
		,'' AS 'DocumentRemarks'
		,'' AS 'BusinessPartner'
		,'' AS 'Project'
		,'' AS 'LinkDocumentType'
		,T0.OP AS 'LinkDocument'
		,T1.BELPOS_ID AS 'LinkLine1'
		,0	AS 'LinkLine2'
		,T1.ItemCode AS 'Itemcode'
		,CASE WHEN ISNULL(T2.Quantity, 0) > 0
			THEN T2.Quantity + T0.REJ
			ELSE T0.QUANT + T0.REJ
		END AS 'Quantity'
		,T1.WhsCode AS 'Warehouse'
		,'' AS 'Bincode'
		,'' AS 'RFID'
		,'' AS 'Version'
		,T2.ITEM AS 'Batchnumber'
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
		, 0.00 'BatchQuantity'
	FROM  MES..SSPExportProd T0 WITH(NOLOCK)	
		INNER JOIN BEAS_FTPOS T1  WITH(NOLOCK)
			ON CAST(T1.BELNR_ID AS CHAR (10)) = T0.OP 
			AND CAST(T1.BELPOS_ID AS CHAR (10)) = T0.BELPOS_ID
		LEFT JOIN BEAS_FTPOS_RESERVIERUNG T2 WITH(NOLOCK)
			ON T2.BELNR_ID = T1.BELNR_ID
			AND T2.BELPOS_ID = T1.BELPOS_ID
	WHERE T0.id = @IdMes
END
