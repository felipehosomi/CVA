CREATE PROCEDURE SP_CVA_PERCENTUAL_PRODUCAO
(
	DataDe DATE,
	DataAte DATE,
	NrOP INT,
	NrPedido INT
)
AS
BEGIN

	SELECT (IFNULL(SUM("APON"."U_QtdeApontada"), 0.00) / IFNULL(SUM("OWOR"."PlannedQty"), 1.00)) * 100
	FROM "OWOR"
		INNER JOIN "WOR4"
			ON "WOR4"."DocEntry" = "OWOR"."DocEntry"
		LEFT JOIN "@CVA_APONTAMENTO" "APON"
			ON "APON"."U_NrOP" = "OWOR"."DocEntry"
			AND "APON"."U_CodEtapa" = "WOR4"."Name"
	WHERE 
	"OWOR"."DueDate" BETWEEN DataDe AND DataAte
	AND "OWOR"."Status" NOT IN ('P', 'C')
	AND ("OWOR"."DocNum" = NrOP OR NrOP IS NULL)
	AND ("OWOR"."OriginNum" = NrPedido OR NrPedido IS NULL);

END

