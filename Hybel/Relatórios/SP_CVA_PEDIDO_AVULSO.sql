IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_PEDIDO_AVULSO')
	DROP PROCEDURE SP_CVA_PEDIDO_AVULSO
GO

CREATE PROCEDURE SP_CVA_PEDIDO_AVULSO
(
	@DataDe		DATETIME,
	@DataAte	DATETIME,
	@DocEntry	INT
)
AS
BEGIN
	SELECT
				T1.DOCDATE,
				T1.CARDNAME,
				T1.BPLNAME, 
				T1.Comments,
				T0.ITEMCODE, 
	T0.Dscription, 
	T0.DocEntry AS 'Item Ped. de Compra', 
	T0.LineNum AS  'It. Ped.', 
				(CASE WHEN ISNULL (T2.Quantity ,0) = 0 THEN T0.Quantity ELSE T0.Quantity - T2.Quantity END)   AS 'Quantidade', 
	T0.Quantity AS 'Quantidade Pedido', 
	ISNULL (T2.Quantity, 0) AS 'Quantidade Transferida',
	OBPL.BPLName,
	OBPL.BPLFrName,
	OADP.LogoImage
	FROM RDR1 T0 
		INNER JOIN ORDR T1 ON T0.DOCENTRY = T1.DOCENTRY
		INNER JOIN OBPL ON OBPL.BPlId = T1.BPlId
		LEFT JOIN  (SELECT WTR1.ItemCode AS Item, 
					SUM(WTR1.Quantity) AS Quantity, 
					WTR1.u_ItemPedCompra AS Pedido,
					ISNULL(WTR1.u_ItemPed,0) AS LinhaPedido

					FROM WTR1 WHERE ISNULL(u_ItemPedCompra,0) <> 0 and ISNULL(u_ItemPed,0) >= 0 
					GROUP BY WTR1.ItemCode,WTR1.u_ItemPedCompra,WTR1.u_ItemPed) T2 ON T0.Docentry = T2.Pedido 
																									AND T0.linenum = T2.LinhaPedido 																			                AND T0.Itemcode = T2.Item 
					LEFT JOIN OITM T3 ON T3.ITEMCODE = T0.ITEMCODE
					LEFT JOIN	OADP WITH(NOLOCK) ON 1 = 1
	WHERE T3.ITMSGRPCOD <> 103 AND T3.INVNTITEM = 'Y'
	AND  (T0.Quantity <> ISNULL (T2.Quantity, 0) OR T0.Quantity < ISNULL (T2.Quantity, 0))
	AND T0.SHIPDATE  BETWEEN @DataDe AND @DataAte AND (T1.DocEntry = @DocEntry OR @DocEntry = 0)
END