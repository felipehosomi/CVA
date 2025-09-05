IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_TRANSF_FILIAIS')
	DROP PROCEDURE SP_CVA_TRANSF_FILIAIS
GO
CREATE PROCEDURE SP_CVA_TRANSF_FILIAIS
(
	@WhsCode	NVARCHAR(60)
)
AS
BEGIN
	SELECT
		'Y'								[Gerar]
		,dadosEstoque.ItemCode			[Cód. Item]
		,dadosEstoque.ItemName			[Descrição]
		,dadosEstoque.WhsCode			[Depósito]
		,dadosEstoque.EmEstoque			[Em Estoque]
		,dadosEstoque.EstoqueMinimo		[Est. Mínimo]
		,CASE WHEN dadosEstoque.Pedido > Necessario
			THEN Necessario
			ELSE Pedido
		END			[Pedido]
		,dadosEstoque.EmTransito		[Em Trânsito]
		,dadosEstoque.PedidosAbertos	[Pedidos Abertos]
		,case when (dadosEstoque.EmEstoque + dadosEstoque.EmTransito + PedidosAbertos ) < dadosEstoque.EstoqueMinimo then dadosEstoque.EstoqueMinimo - (dadosEstoque.EmEstoque + dadosEstoque.EmTransito + PedidosAbertos) else 0.00 end as [Quantidade]
	FROM (
		SELECT 
			A.ItemCode
			,A.ItemName
			,B.WhsCode
			,C.WhsName
			,B.OnHand as 'EmEstoque'
			,B.MinStock as 'EstoqueMinimo'
			,CASE WHEN B.OnHand < B.MinStock 
				THEN B.MinStock - B.OnHand
				ELSE 0
			END as 'Necessario'
			,isnull(
			(
				SELECT CASE WHEN OITW.OnHand - OITW.IsCommited + OITW.OnOrder > 0
					THEN OITW.OnHand - OITW.IsCommited + OITW.OnOrder
					ELSE 0 END
				FROM OITM
					INNER JOIN OITW
						ON OITM.ItemCode = OITW.ItemCode
				WHERE OITW.ItemCode = A.ItemCode
				AND OITW.WhsCode = OITM.DfltWH
			), 0.00) as 'Pedido'
			--,isnull(
			--(
			--	SELECT SUM(RDR1.OpenQty)
			--	FROM RDR1
			--		INNER JOIN OBPL ON OBPL.DflWhs = RDR1.WhsCode
			--		INNER JOIN ORDR ON ORDR.DocEntry = RDR1.DocEntry AND ORDR.BPLId = OBPL.BPLId
			--		INNER JOIN BEAS_FTHAUPT AUPT ON AUPT.DocEntry = RDR1.DocEntry AND AUPT.AUFTRAGPOS = RDR1.LineNum AND AUPT.ABGKZ = 'N'
			--	WHERE RDR1.ItemCode = A.ItemCode
			--	AND RDR1.WhsCode = B.WhsCode
			--	AND RDR1.LineStatus = 'O'
			--), 0.00) as 'Pedido'
			,isnull(
			(
				SELECT SUM(NFItens.Quantity)
				FROM INV1 NFItens 
					INNER JOIN OINV NFCabe on NFCabe.DocEntry = NFItens.DocEntry
					INNER JOIN CRD7 ON CRD7.CardCode = NFCabe.CardCode AND CRD7.[Address] = ''
					INNER JOIN OBPL ON OBPL.TaxIdNum = CRD7.TaxId0 AND OBPL.DflWhs = @WhsCode
				WHERE NFItens.Usage = '130' 
					and NFCabe.CANCELED = 'N'
					and NFItens.WhsCode = '12'
					and NFItens.ItemCode = B.ItemCode
					and 0 = (
								select COUNT(DevCabe.DocEntry) 
									from RIN1 DevItens inner join ORIN DevCabe on DevCabe.docentry = DevItens.docentry
								where  devitens.Usage = '5' 
								and devitens.basetype = -1 
								and DevCabe.CardCode = 'C000004684'
								and DevItens.ItemCode = NFItens.ItemCode
								and DevCabe.Serial = NFCabe.Serial
							)
			), 0.00) as 'EmTransito'
			,isnull(
			(
				select sum(BB.quantity)
				FROM ordr AA inner join rdr1 BB on AA.docentry = BB.docentry
					inner join rdr12 CC on CC.DocEntry = AA.DocEntry
				where AA.docstatus = 'O' 
				and BB.Usage = '130' 
				and (select DfltResWhs from obpl where TaxIdNum = CC.TaxId0) = B.WhsCode and BB.ItemCode = B.ItemCode
			), 0.00) as 'PedidosAbertos'
		FROM oitm A 
			INNER JOIN OITW B on A.itemcode = B.itemcode 
			INNER JOIN OWHS C on C.WhsCode = B.WhsCode
		WHERE B.WhsCode = @WhsCode and B.MinStock > B.OnHand
	) AS dadosEstoque
END