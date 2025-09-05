IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_NOTA_SAIDA')
	DROP PROCEDURE SP_CVA_NOTA_SAIDA
GO
CREATE PROC SP_CVA_NOTA_SAIDA
(
	@NrNF	INT
)
AS
BEGIN

SELECT
	OINV.Serial,
	OINV.DocNum,
	OINV.CardCode,
	OINV.DocDate,
	OINV.DocDueDate,
	OINV.TaxDate,
	OINV.DiscPrcnt,
	OINV.VatSum,
	OINV.DocTotal,
	OINV.Address2,
	OINV.Comments,
	OINV.DocDueDate		AS Valid,
	OINV.PeyMethod,
	INV1.VisOrder + 1 VisOrder,
	INV1.ShipDate,
	INV1.ItemCode, 
	INV1.Dscription,
	CASE WHEN ISNULL(OITL.AllocQty, 0) = 0 
		THEN INV1.Quantity
		ELSE OITL.AllocQty
	END Quantity,
	INV1.Usage,
	INV1.CFOPCode,
	OCFP.Descrip	CFOPDesc,
	INV1.unitMsr,
	INV1.Currency,
	INV1.Price, 
	INV1.Price + (INV1.VatSum / INV1.Quantity) PriceVat,
	INV1.PriceBefDi,
	CASE WHEN ISNULL(OITL.AllocQty, 0) = 0 
		THEN INV1.LineTotal
		ELSE INV1.Price * OITL.AllocQty
	END LineTotal,
	CASE WHEN ISNULL(OITL.AllocQty, 0) = 0 
		THEN INV1.LineTotal + INV1.VatSum
		ELSE ((INV1.LineTotal + INV1.VatSum) /  INV1.Quantity) * OITL.AllocQty
	END LineTotalVat,
	(INV1.DiscPrcnt / 100) * INV1.LineTotal [Discount],
	INV1.AcctCode,
	INV3.LineTotal	FreightValue,
	INV12.AddrTypeS,
	INV12.StreetS,
	INV12.StreetNoS,
	INV12.BuildingS,
	INV12.ZipCodeS,
	INV12.BlockS,
	INV12.CityS,
	INV12.StateS,
	INV12.TaxId0,
	INV12.TaxId1,
	OITL.DistNumber,
	OCRD.CardName, 
	OCRD.CntctPrsn, 
	OCRD.City, 
	OCRD.State1, 
	OCRD.Address, 
	OCRD.StreetNo, 
	OCRD.Phone1, 
	OCRD.Phone2, 
	OCRD.Fax,
	OCTG.PymntGroup, 
	OITM.NCMCode,
	OITM.FrgnName,
	CASE WHEN OMRC.FirmName = '- Nenhum fabricante -' THEN '' ELSE OMRC.FirmName END	FirmName,
	OADP.LogoImage,
	OBPL.BPLName,
	OBPL.AddrType	BPAddrType,
	OBPL.Street		BPStreet,
	OBPL.StreetNo	BPStreetNo,
	OBPL.Block		BPBlock,
	OBPL.City		BPCity,
	OBPL.State		BPState,
	OBPL.ZipCode	BPZipCode,
	OBPL.TaxIdNum	BPTaxIdNum,
	OBPL.TaxIdNum2	TaxIdNum2,
	CASE WHEN VENDOR.SlpName = '-Nenhum vendedor-' THEN '' ELSE VENDOR.SlpName END	VendorName,
	VENDOR.Email	VendorEmail,
	CARRIER.CardName			CarrierName,
	CARRIER_ADDRESS.AddrType	CarrierAddrType,
	CARRIER_ADDRESS.Street		CarrierStreet,
	CARRIER_ADDRESS.StreetNo	CarrierStreetNo,
	CARRIER_ADDRESS.Block		CarrierBlock,
	CARRIER_ADDRESS.City		CarrierCity,
	CARRIER_ADDRESS.State		CarrierState,
	CARRIER_ADDRESS.ZipCode		CarrierZipCode,
	CONTACT.E_MailL				ContactEmail,
	CONTACT.Name				ContactName,
	OSHP.TrnspName,
	CAST(OITM.U_quantmed1 AS NVARCHAR) +'x'+CAST(OITM.U_quantmed2 AS NVARCHAR) AS QtdEmb,
	--'10x20' AS QtdEmb,
	OSHP.WebSite				TrnspWebSite
	,OPYM.[Descript]
	, CASE WHEN OINV.[U_TipoAutorizacao] = 0	THEN 'Selecionar' ELSE 
	CASE WHEN OINV.[U_TipoAutorizacao] = 1	THEN 'Autorização de Compra' ELSE 
	CASE WHEN OINV.[U_TipoAutorizacao] = 2	THEN 'Autorização de Despesa' ELSE 
	CASE WHEN OINV.[U_TipoAutorizacao] = 3	THEN 'Autorização de Fornecimento' ELSE 
	CASE WHEN OINV.[U_TipoAutorizacao] = 4	THEN 'Empenho' ELSE 
	CASE WHEN OINV.[U_TipoAutorizacao] = 5	THEN 'Ordem de Compra' ELSE 
	CASE WHEN OINV.[U_TipoAutorizacao] = 6	THEN 'Ordem de Despesa' ELSE 
	CASE WHEN OINV.[U_TipoAutorizacao] = 7	THEN 'Ordem de Fornecimento' ELSE 
	CASE WHEN OINV.[U_TipoAutorizacao] = 8	THEN 'Pedido' ELSE 
	CASE WHEN OINV.[U_TipoAutorizacao] = 9	THEN 'Requisição' ELSE 'Solicitação'
	--CASE WHEN OINV.[U_TipoAutorizacao] = 11 THEN 'Não possui - Base Nº Licitação'
		END END END END END END END END END END  AS TipoAutorizacao
	,OINV.[U_Numerodocumento]
	,OINV.[U_TipoLicitacao]
	,OINV.[U_NumeroLicitacao]
	,OINV.[U_TipoContrato]
	,OINV.[U_NumeroContrato]
	,OINV.[DiscSum] + OINV.[DocTotal] AS TotalAntes
	,OINV.[DiscSum]	AS Desconto
	, OINV.[DocTotal] AS TotalDepois

	FROM OINV WITH(NOLOCK)
	INNER JOIN OCRD WITH(NOLOCK)
		ON OCRD.CardCode = OINV.CardCode
	INNER JOIN INV1 WITH(NOLOCK)
		ON INV1.DocEntry = OINV.DocEntry
	INNER JOIN OITM WITH(NOLOCK)
		ON INV1.ItemCode = OITM.ItemCode
	INNER JOIN INV12 WITH(NOLOCK)
		ON INV12.DocEntry = OINV.DocEntry
	LEFT JOIN OMRC WITH(NOLOCK)
		ON OMRC.FirmCode = OITM.FirmCode
	LEFT JOIN INV3 WITH(NOLOCK)
		ON INV3.DocEntry = OINV.DocEntry
	LEFT JOIN OCFP WITH(NOLOCK)
		ON INV1.CFOPCode = OCFP.Code
	LEFT JOIN OSLP VENDOR WITH(NOLOCK)
		ON VENDOR.SlpCode = OINV.SlpCode
	LEFT JOIN OSHP WITH(NOLOCK)
		ON OSHP.TrnspCode = OINV.TrnspCode
	LEFT JOIN OCPR CONTACT WITH(NOLOCK)
		ON CONTACT.CardCode = OCRD.CardCode
		AND CONTACT.Name = OCRD.CntctPrsn
	LEFT JOIN OCRD CARRIER WITH(NOLOCK)
		ON CARRIER.CardCode = INV12.Carrier
	LEFT JOIN CRD1 CARRIER_ADDRESS WITH(NOLOCK)
		ON CARRIER_ADDRESS.CardCode = INV12.Carrier
		AND CARRIER_ADDRESS.LineNum = 0
	LEFT JOIN OBPL WITH(NOLOCK)
		ON OBPL.BPLId = OINV.BPLId
	LEFT JOIN OCTG WITH(NOLOCK)
		ON OCTG.GroupNUm = OINV.GroupNum
	LEFT JOIN 
	(
		SELECT OITL.ApplyType, OITL.ApplyEntry, OITL.ApplyLine, OBTN.DistNumber, 
		CASE WHEN SUM(ITL1.AllocQty) > 0
			THEN SUM(ITL1.AllocQty)
			ELSE SUM(ITL1.Quantity) * (-1)
		END AllocQty  
		FROM OITL
			LEFT JOIN ITL1
				ON ITL1.LogEntry = OITL.LogEntry
			LEFT JOIN OBTN WITH(NOLOCK)
				ON OBTN.ItemCode = ITL1.ItemCode
				AND OBTN.AbsEntry = ITL1.MdAbsEntry
		GROUP BY  OITL.ApplyType, OITL.ApplyEntry, OITL.ApplyLine, OBTN.DistNumber
		HAVING SUM(ITL1.AllocQty) > 0 OR  SUM(ITL1.Quantity) * (-1) > 0
	) OITL
		ON OITL.ApplyType = OINV.ObjType
		AND OITL.ApplyEntry = OINV.DocEntry
		AND OITL.ApplyLine = INV1.LineNum
	LEFT JOIN OADP WITH(NOLOCK)
		ON 1 = 1

	LEFT JOIN OPYM ON OPYM.[PayMethCod] = OINV.[PeyMethod]

	WHERE OINV.Serial = @NrNF

UNION ALL

SELECT
	ORIN.Serial,
	ORIN.DocNum,
	ORIN.CardCode,
	ORIN.DocDate,
	ORIN.DocDueDate,
	ORIN.TaxDate,
	ORIN.DiscPrcnt,
	ORIN.VatSum,
	ORIN.DocTotal,
	ORIN.Address2,
	ORIN.Comments,
	ORIN.DocDueDate		AS Valid,
	ORIN.PeyMethod,
	RIN1.VisOrder + 1 VisOrder,
	RIN1.ShipDate,
	RIN1.ItemCode, 
	RIN1.Dscription,
	CASE WHEN ISNULL(OITL.AllocQty, 0) = 0 
		THEN RIN1.Quantity
		ELSE OITL.AllocQty
	END Quantity,
	RIN1.Usage,
	RIN1.CFOPCode,
	OCFP.Descrip	CFOPDesc,
	RIN1.unitMsr,
	RIN1.Currency,
	RIN1.Price, 
	RIN1.Price + (RIN1.VatSum / RIN1.Quantity) PriceVat,
	RIN1.PriceBefDi,
	CASE WHEN ISNULL(OITL.AllocQty, 0) = 0 
		THEN RIN1.LineTotal
		ELSE RIN1.Price * OITL.AllocQty
	END LineTotal,
	CASE WHEN ISNULL(OITL.AllocQty, 0) = 0 
		THEN RIN1.LineTotal + RIN1.VatSum
		ELSE ((RIN1.LineTotal + RIN1.VatSum) /  RIN1.Quantity) * OITL.AllocQty
	END LineTotalVat,
	(RIN1.DiscPrcnt / 100) * RIN1.LineTotal [Discount],
	RIN1.AcctCode,
	RIN3.LineTotal	FreightValue,
	RIN12.AddrTypeS,
	RIN12.StreetS,
	RIN12.StreetNoS,
	RIN12.BuildingS,
	RIN12.ZipCodeS,
	RIN12.BlockS,
	RIN12.CityS,
	RIN12.StateS,
	RIN12.TaxId0,
	RIN12.TaxId1,
	OITL.DistNumber,
	OCRD.CardName, 
	OCRD.CntctPrsn, 
	OCRD.City, 
	OCRD.State1, 
	OCRD.Address, 
	OCRD.StreetNo, 
	OCRD.Phone1, 
	OCRD.Phone2, 
	OCRD.Fax,
	OCTG.PymntGroup, 
	OITM.NCMCode,
	OITM.FrgnName,
	CASE WHEN OMRC.FirmName = '- Nenhum fabricante -' THEN '' ELSE OMRC.FirmName END	FirmName,
	OADP.LogoImage,
	OBPL.BPLName,
	OBPL.AddrType	BPAddrType,
	OBPL.Street		BPStreet,
	OBPL.StreetNo	BPStreetNo,
	OBPL.Block		BPBlock,
	OBPL.City		BPCity,
	OBPL.State		BPState,
	OBPL.ZipCode	BPZipCode,
	OBPL.TaxIdNum	BPTaxIdNum,
	OBPL.TaxIdNum2	TaxIdNum2,
	CASE WHEN VENDOR.SlpName = '-Nenhum vendedor-' THEN '' ELSE VENDOR.SlpName END	VendorName,
	VENDOR.Email	VendorEmail,
	CARRIER.CardName			CarrierName,
	CARRIER_ADDRESS.AddrType	CarrierAddrType,
	CARRIER_ADDRESS.Street		CarrierStreet,
	CARRIER_ADDRESS.StreetNo	CarrierStreetNo,
	CARRIER_ADDRESS.Block		CarrierBlock,
	CARRIER_ADDRESS.City		CarrierCity,
	CARRIER_ADDRESS.State		CarrierState,
	CARRIER_ADDRESS.ZipCode		CarrierZipCode,
	CONTACT.E_MailL				ContactEmail,
	CONTACT.Name				ContactName,
	OSHP.TrnspName,
	CAST(OITM.U_quantmed1 AS NVARCHAR) +'x'+CAST(OITM.U_quantmed2 AS NVARCHAR) AS QtdEmb,
	--'10x20' AS QtdEmb,
	OSHP.WebSite				TrnspWebSite
	,OPYM.[Descript]
	, CASE WHEN ORIN.[U_TipoAutorizacao] = 0	THEN 'Selecionar' ELSE 
	CASE WHEN ORIN.[U_TipoAutorizacao] = 1	THEN 'Autorização de Compra' ELSE 
	CASE WHEN ORIN.[U_TipoAutorizacao] = 2	THEN 'Autorização de Despesa' ELSE 
	CASE WHEN ORIN.[U_TipoAutorizacao] = 3	THEN 'Autorização de Fornecimento' ELSE 
	CASE WHEN ORIN.[U_TipoAutorizacao] = 4	THEN 'Empenho' ELSE 
	CASE WHEN ORIN.[U_TipoAutorizacao] = 5	THEN 'Ordem de Compra' ELSE 
	CASE WHEN ORIN.[U_TipoAutorizacao] = 6	THEN 'Ordem de Despesa' ELSE 
	CASE WHEN ORIN.[U_TipoAutorizacao] = 7	THEN 'Ordem de Fornecimento' ELSE 
	CASE WHEN ORIN.[U_TipoAutorizacao] = 8	THEN 'Pedido' ELSE 
	CASE WHEN ORIN.[U_TipoAutorizacao] = 9	THEN 'Requisição' ELSE 'Solicitação'
	--CASE WHEN ORIN.[U_TipoAutorizacao] = 11 THEN 'Não possui - Base Nº Licitação'
		END END END END END END END END END END  AS TipoAutorizacao
	,ORIN.[U_Numerodocumento]
	,ORIN.[U_TipoLicitacao]
	,ORIN.[U_NumeroLicitacao]
	,ORIN.[U_TipoContrato]
	,ORIN.[U_NumeroContrato]
	,ORIN.[DiscSum] + ORIN.[DocTotal] AS TotalAntes
	,ORIN.[DiscSum]	AS Desconto
	, ORIN.[DocTotal] AS TotalDepois

	FROM ORIN WITH(NOLOCK)
	INNER JOIN OCRD WITH(NOLOCK)
		ON OCRD.CardCode = ORIN.CardCode
	INNER JOIN RIN1 WITH(NOLOCK)
		ON RIN1.DocEntry = ORIN.DocEntry
	INNER JOIN OITM WITH(NOLOCK)
		ON RIN1.ItemCode = OITM.ItemCode
	INNER JOIN RIN12 WITH(NOLOCK)
		ON RIN12.DocEntry = ORIN.DocEntry
	LEFT JOIN OMRC WITH(NOLOCK)
		ON OMRC.FirmCode = OITM.FirmCode
	LEFT JOIN RIN3 WITH(NOLOCK)
		ON RIN3.DocEntry = ORIN.DocEntry
	LEFT JOIN OCFP WITH(NOLOCK)
		ON RIN1.CFOPCode = OCFP.Code
	LEFT JOIN OSLP VENDOR WITH(NOLOCK)
		ON VENDOR.SlpCode = ORIN.SlpCode
	LEFT JOIN OSHP WITH(NOLOCK)
		ON OSHP.TrnspCode = ORIN.TrnspCode
	LEFT JOIN OCPR CONTACT WITH(NOLOCK)
		ON CONTACT.CardCode = OCRD.CardCode
		AND CONTACT.Name = OCRD.CntctPrsn
	LEFT JOIN OCRD CARRIER WITH(NOLOCK)
		ON CARRIER.CardCode = RIN12.Carrier
	LEFT JOIN CRD1 CARRIER_ADDRESS WITH(NOLOCK)
		ON CARRIER_ADDRESS.CardCode = RIN12.Carrier
		AND CARRIER_ADDRESS.LineNum = 0
	LEFT JOIN OBPL WITH(NOLOCK)
		ON OBPL.BPLId = ORIN.BPLId
	LEFT JOIN OCTG WITH(NOLOCK)
		ON OCTG.GroupNUm = ORIN.GroupNum
	LEFT JOIN 
	(
		SELECT OITL.ApplyType, OITL.ApplyEntry, OITL.ApplyLine, OBTN.DistNumber, 
		CASE WHEN SUM(ITL1.AllocQty) > 0
			THEN SUM(ITL1.AllocQty)
			ELSE SUM(ITL1.Quantity) * (-1)
		END AllocQty  
		FROM OITL
			LEFT JOIN ITL1
				ON ITL1.LogEntry = OITL.LogEntry
			LEFT JOIN OBTN WITH(NOLOCK)
				ON OBTN.ItemCode = ITL1.ItemCode
				AND OBTN.AbsEntry = ITL1.MdAbsEntry
		GROUP BY  OITL.ApplyType, OITL.ApplyEntry, OITL.ApplyLine, OBTN.DistNumber
		HAVING SUM(ITL1.AllocQty) > 0 OR  SUM(ITL1.Quantity) * (-1) > 0
	) OITL
		ON OITL.ApplyType = ORIN.ObjType
		AND OITL.ApplyEntry = ORIN.DocEntry
		AND OITL.ApplyLine = RIN1.LineNum
	LEFT JOIN OADP WITH(NOLOCK)
		ON 1 = 1

	LEFT JOIN OPYM ON OPYM.[PayMethCod] = ORIN.[PeyMethod]

	WHERE ORIN.Serial = @NrNF

END