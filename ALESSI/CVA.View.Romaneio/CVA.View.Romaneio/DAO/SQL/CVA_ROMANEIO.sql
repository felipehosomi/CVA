IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'CVA_ROMANEIO')
	DROP PROCEDURE CVA_ROMANEIO
GO

CREATE PROCEDURE CVA_ROMANEIO
( 
  @DocKey nvarchar(30)  
) 
AS   
BEGIN
;WITH WAYBILL AS
(
	SELECT
		OINV.DocEntry,
		OUSR.U_NAME		[UserName],
		WAYBILL.Code	[Waybill],
		OCRD.CardName	[Carrier],
		VEH_TYPE.Name	[Vehicle_Type],
		VEH.U_Plate		[Plate],
		VEH.U_Color		[Color],
		VEH.U_Driver	[Driver],
		OINV.SlpCode,
		INV12.AddrTypeS + ' ' + 
		INV12.StreetNoS + ' ' + 
		INV12.StreetNoS	[Address],
		OINV.CardName	[Customer],
		INV12.BlockS	[Neighborhood],
		INV12.QoP		[Volume],
		INV12.ZipCodeS	[ZipCode],
		INV12.GrsWeight	[GrossWeigh],
		INV12.CityS		[City],
		INV12.StateS	[State],
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
		INNER JOIN INV12
			ON INV12.DocEntry = OINV.DocEntry
	WHERE WAYBILL.Code = @DocKey
)
	SELECT 
		WAYBILL.*, 
		OITM.SalPackMsr		Pack,
		SUM(INV1.PackQty)	PackQty
	FROM WAYBILL
		INNER JOIN INV1
			ON INV1.DocEntry = WAYBILL.DocEntry
		INNER JOIN OITM
			ON OITM.ItemCode = INV1.ItemCode
	GROUP BY 
		OITM.SalPackMsr,
		WAYBILL.DocEntry,
		WAYBILL.UserName,
		WAYBILL.Waybill,
		WAYBILL.Carrier,
		WAYBILL.Vehicle_Type,
		WAYBILL.Plate,
		WAYBILL.Color,
		WAYBILL.Driver,
		WAYBILL.SlpCode,
		WAYBILL.[Address],
		WAYBILL.Customer,
		WAYBILL.Neighborhood,
		WAYBILL.Volume,
		WAYBILL.ZipCode,
		WAYBILL.GrossWeigh,
		WAYBILL.City,
		WAYBILL.[State],
		WAYBILL.DocTotal
END