IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'CVA_WAYBILL')
	DROP PROCEDURE CVA_WAYBILL
GO

CREATE PROCEDURE CVA_WAYBILL
( 
  @DocKey NVARCHAR(30)  
) 
AS   
BEGIN
	SELECT
		OINV.DocEntry,
		OUSR.USER_CODE		[UserCode],
		OUSR.U_NAME			[UserName],
		WAYBILL.Code		[Waybill],
		WAYBILL.U_Comments	[Comments],
		OCRD.CardCode		[CarrierCode],
		OCRD.CardName		[CarrierName],
		VEH_TYPE.Name		[Vehicle_Type],
		VEH.U_Plate			[Plate],
		VEH.U_Color			[Color],
		VEH.U_Driver		[Driver],
		OINV.DocNum,
		OINV.Serial,
		OSLP.SlpName,
		INV12.AddrTypeS + ' ' + 
		INV12.StreetS + ' ' + 
		INV12.StreetNoS	[Address],
		OINV.CardName	[Customer],
		INV12.BlockS	[Neighborhood],
		INV12.QoP		[Volume],
		INV12.ZipCodeS	[ZipCode],
		INV12.GrsWeight	[GrossWeigh],
		INV12.CityS		[City],
		INV12.StateS	[State],
		OINV.DocTotalSy,
		OINV.DocTotal
	FROM [@CVA_WAYBILL] WAYBILL
		INNER JOIN [@CVA_WAYBILL_INV] INV
			ON INV.Code = WAYBILL.Code
		INNER JOIN OUSR
			ON OUSR.USERID = WAYBILL.UserSign
		INNER JOIN OCRD
			ON OCRD.CardCode = WAYBILL.U_Carrier
		INNER JOIN [@CVA_VEHICLE] VEH
			ON VEH.Code = WAYBILL.U_Vehicle
		INNER JOIN [@CVA_VEH_TYPE] VEH_TYPE
			ON VEH_TYPE.Code = WAYBILL.U_VehType
		INNER JOIN OINV
			ON OINV.DocNum = INV.U_DocNum
		INNER JOIN OSLP
			ON OSLP.SlpCode = OINV.SlpCode
		INNER JOIN INV12
			ON INV12.DocEntry = OINV.DocEntry
	WHERE WAYBILL.Code = @DocKey
END