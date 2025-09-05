IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_ETIQUETA_VERDE')
	DROP PROCEDURE SP_CVA_ETIQUETA_VERDE
GO
CREATE PROCEDURE SP_CVA_ETIQUETA_VERDE
(
	@NrNota		INT
)
AS
BEGIN
	SELECT
		OINV.DocDate	DataEmissao,
		OINV.Serial		NrNF,
		INV1.VisOrder	Linha,
		INV1.ItemCode,
		INV1.Dscription	ItemName,
		INV1.unitMsr	UN,
		ORDR.DocNum		NrPedido,
		OINV.[Address]	Endereco,
		CAST(INV1.Quantity	AS NUMERIC(19, 2)) Quantidade,
		CAST(INV1.Quantity AS INT) QtdeEtiq,
		Cliente.CardCode + ' - ' + Cliente.CardName	Cliente,
		Transportadora.CardCode + ' - ' + Transportadora.CardName	Transportadora
	FROM OINV WITH(NOLOCK)
		INNER JOIN INV1 WITH(NOLOCK)
			ON INV1.DocEntry = OINV.DocEntry
		INNER JOIN INV12 WITH(NOLOCK)
			ON INV12.DocEntry = OINV.DocEntry
		INNER JOIN OCRD Cliente WITH(NOLOCK)
			ON Cliente.CardCode = OINV.CardCode
		LEFT JOIN OCRD Transportadora WITH(NOLOCK)
			ON Transportadora.CardCode = INV12.Carrier
		LEFT JOIN RDR1 WITH(NOLOCK)
			ON RDR1.DocEntry = INV1.BaseEntry
			AND RDR1.LineNum = INV1.BaseLine
			AND INV1.BaseType = 17
		LEFT JOIN ORDR WITH(NOLOCK)
			ON ORDR.DocEntry = RDR1.DocEntry
	WHERE OINV.Serial = @NrNota
END