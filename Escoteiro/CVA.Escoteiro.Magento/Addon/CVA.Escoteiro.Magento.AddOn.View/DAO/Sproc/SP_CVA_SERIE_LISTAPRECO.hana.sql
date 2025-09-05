CREATE PROCEDURE SP_CVA_SERIE_LISTAPRECO
(
	WhsCode 	VARCHAR(100),
	ItemCode 	VARCHAR(100),
	PriceList 	INT,
	Discount 	NUMERIC(19, 6),
	Searching 	VARCHAR(1)
)
AS
BEGIN
SELECT 
		'Y'					"#",
		"OSRN"."ItemCode" 	"Cód. Item",
		"OSRN"."itemName" 	"Descrição",
		"OSRN"."DistNumber"	"Item (Nr. Série)",
		"ITM1"."Price"		"Preço Original",
		CASE WHEN Searching = 'Y' AND "ITM1"."Price" > 0
			THEN (("ITM1"."Price" - "OSRN"."U_maxPreco") / "ITM1"."Price") * 100 
			ELSE Discount		
		END "Desconto",
		CASE WHEN Searching = 'Y' OR "OSRN"."U_maxPreco" <> 0
			THEN "OSRN"."U_maxPreco"
			ELSE "ITM1"."Price" - ("ITM1"."Price" * (Discount / 100)) 
		END "Preço Novo",
		"OSRN"."SysNumber"
	FROM "ITL1"
		INNER JOIN	"OITL" ON "OITL"."LogEntry" = "ITL1"."LogEntry"
		INNER JOIN	"OSRN" ON "ITL1"."ItemCode" = "OSRN"."ItemCode" and "ITL1"."SysNumber" = "OSRN"."SysNumber"
		LEFT JOIN "ITM1" ON "ITM1"."ItemCode" = "OSRN"."ItemCode" AND "ITM1"."PriceList" = PriceList
	WHERE ("ITL1"."ItemCode" = ItemCode OR ItemCode IS NULL)
	AND "OITL"."LocCode" = WhsCode
	GROUP BY 
		"OSRN"."ItemCode",
		"OSRN"."itemName", 
		"OSRN"."SysNumber",
		"OSRN"."DistNumber",
		"ITM1"."Price",
		"OSRN"."U_maxPreco"
	HAVING SUM("ITL1"."Quantity") > 0
	ORDER BY "OSRN"."ItemCode", "OSRN"."DistNumber";
END