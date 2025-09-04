CREATE PROCEDURE SP_CVA_ETAPA_PRODUCAO
(
	NrOP		INT,
	CodEtapa	INT
)
AS
BEGIN

SELECT 
	"OWOR"."DocEntry",
	"OWOR"."DocNum",
	"OWOR"."PlannedQty" "QtdeOriginal",
	"OWOR"."PlannedQty" "QtdeOP",
	"OWOR"."StartDate" 	"DocDate",
	TO_VARCHAR("OWOR"."StartDate", 'YYYY-MM-DD') "DocDateStr",
	"ORDR"."DocEntry" 	"DocEntryPedido",
	"ORDR"."DocNum" 	"NrPedido",
	"WOR1"."LineNum",
	"WOR1"."wareHouse"	"Deposito",
	"OITM"."ItemCode",
	"OITM"."ItemName",
	"WOR4"."StageId" 	"CodEtapa",
	"WOR4"."Name"	 	"DescEtapa",
	"MP"."ItemCode"		"CodMP",
	"MP"."ItemName"		"DescMP",
	"MP"."ManBtchNum" 	"ControlePorLote",
	"MP"."ManSerNum"  	"ControlePorSerie",
	"WOR1"."PlannedQty"	"Quantidade",
	"WOR1"."BaseQty"	"QtdeBase",
	"WOR1"."IssuedQty"	"QtdeEmitida",
	CASE WHEN "WOR1"."PlannedQty" - "WOR1"."IssuedQty" > 0
		THEN "WOR1"."PlannedQty" - "WOR1"."IssuedQty"
		ELSE 0.00
	END	"QtdeRealizada"
FROM "OWOR"
	INNER JOIN "OITM"
		ON "OITM"."ItemCode" = "OWOR"."ItemCode"
	INNER JOIN "WOR4" 
		ON "WOR4"."DocEntry" = "OWOR"."DocEntry"
	INNER JOIN "WOR1"
		ON "WOR1"."DocEntry" = "WOR4"."DocEntry"
		AND "WOR1"."StageId" = "WOR4"."StageId"
	INNER JOIN "OITM" "MP"
		ON "MP"."ItemCode" = "WOR1"."ItemCode"
	LEFT JOIN "ORDR"
		ON "ORDR"."DocNum" = "OWOR"."OriginNum"
WHERE "OWOR"."DocNum" = NrOP
AND "WOR1"."StageId" = CodEtapa;

END

