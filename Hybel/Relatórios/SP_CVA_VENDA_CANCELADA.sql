IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_VENDA_CANCELADA')
	DROP PROCEDURE SP_CVA_VENDA_CANCELADA
GO
CREATE PROCEDURE SP_CVA_VENDA_CANCELADA
(
	@Motivo		NVARCHAR(100),
	@DataDe		DATETIME,
	@DataAte	DATETIME,
	@Filial		INT,
	@CardCode	NVARCHAR(100),
	@ItemCode	NVARCHAR(100),
	@Tipo		INT
)
AS
BEGIN
	SELECT
		OADP.LogoImage,
		OBPL.BPLName,
		OBPL.BPLFrName,
		'Orçamentos' TipoDoc,
		OQUT.DocEntry,
		OQUT.DocNum,
		OQUT.DocDate,
		OCRD.CardCode,
		OCRD.CardName,
		ADOC.CancelDate,
		OCTG.PymntGroup,
		QUT1.VisOrder + 1	Linha,
		QUT1.ItemCode,
		QUT1.Dscription,
		QUT1.LineTotal,
		OCRD.CntctPrsn,
		MOT.Name	Motivo
	FROM OQUT WITH(NOLOCK)
		INNER JOIN OCRD WITH(NOLOCK)
			ON OCRD.CardCode = OQUT.CardCode
		INNER JOIN OCTG WITH(NOLOCK)
			ON OCTG.GroupNum = OQUT.GroupNum
		INNER JOIN QUT1 WITH(NOLOCK)
			ON QUT1.DocEntry = OQUT.DocEntry
		INNER JOIN OBPL WITH(NOLOCK)
			ON OBPL.BPLId = OQUT.BPLId
		INNER JOIN [@CVA_MOTIVO_CANC] MOT WITH(NOLOCK)
			ON MOT.Code = OQUT.U_CVA_Motivo_Canc
		INNER JOIN 
					(SELECT ADOC.DocEntry, MIN(UpdateDate) CancelDate FROM ADOC WITH(NOLOCK)
							WHERE ADOC.ObjType = 23
							AND ISNULL(ADOC.U_CVA_Motivo_Canc, '') <> ''
						GROUP BY ADOC.DocEntry
					) ADOC ON ADOC.DocEntry = OQUT.DocEntry
		INNER JOIN OADP WITH(NOLOCK)
			ON 1 = 1
	WHERE	(MOT.Code = @Motivo OR ISNULL(@Motivo, '*') = '*')
	AND		(OQUT.DocDate BETWEEN @DataDe AND @DataAte)
	AND		(OBPL.BPLId = @Filial OR @Filial = 0)
	AND		(OCRD.CardCode = @CardCode OR ISNULL(@CardCode, '*') = '*')
	AND		(QUT1.ItemCode = @ItemCode OR ISNULL(@ItemCode, '*') = '*')
	AND		(@Tipo = 23 OR ISNULL(@Tipo, 0) = 0)

	UNION ALL

	SELECT
		OADP.LogoImage,
		OBPL.BPLName,
		OBPL.BPLFrName,
		'Pedidos' TipoDoc,
		ORDR.DocEntry,
		ORDR.DocNum,
		ORDR.DocDate,
		OCRD.CardCode,
		OCRD.CardName,
		ADOC.CancelDate,
		OCTG.PymntGroup,
		RDR1.VisOrder + 1	Linha,
		RDR1.ItemCode,
		RDR1.Dscription,
		RDR1.LineTotal,
		OCRD.CntctPrsn,
		MOT.Name	Motivo
	FROM ORDR WITH(NOLOCK)
		INNER JOIN OCRD WITH(NOLOCK)
			ON OCRD.CardCode = ORDR.CardCode
		INNER JOIN OCTG WITH(NOLOCK)
			ON OCTG.GroupNum = ORDR.GroupNum
		INNER JOIN RDR1 WITH(NOLOCK)
			ON RDR1.DocEntry = ORDR.DocEntry
		INNER JOIN OBPL WITH(NOLOCK)
			ON OBPL.BPLId = ORDR.BPLId
		INNER JOIN [@CVA_MOTIVO_CANC] MOT WITH(NOLOCK)
			ON MOT.Code = ORDR.U_CVA_Motivo_Canc
		INNER JOIN 
					(SELECT ADOC.DocEntry, MIN(UpdateDate) CancelDate FROM ADOC WITH(NOLOCK)
							WHERE ADOC.ObjType = 17
							AND ISNULL(ADOC.U_CVA_Motivo_Canc, '') <> ''
						GROUP BY ADOC.DocEntry
					) ADOC ON ADOC.DocEntry = ORDR.DocEntry
		INNER JOIN OADP WITH(NOLOCK)
			ON 1 = 1
	WHERE	(MOT.Code = @Motivo OR ISNULL(@Motivo, '*') = '*')
	AND		(ORDR.DocDate BETWEEN @DataDe AND @DataAte)
	AND		(OBPL.BPLId = @Filial OR @Filial = 0)
	AND		(OCRD.CardCode = @CardCode OR ISNULL(@CardCode, '*') = '*')
	AND		(RDR1.ItemCode = @ItemCode OR ISNULL(@ItemCode, '*') = '*')
	AND		(@Tipo = 17 OR ISNULL(@Tipo, 0) = 0)
	
END