IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_GISS')
	DROP PROCEDURE SP_CVA_GISS
GO
CREATE PROCEDURE SP_CVA_GISS
(
	@Branch		INT,
	@DateFrom	DATETIME,
	@DateTo		DATETIME
)
AS
BEGIN
	SELECT
		
		OPCH.Serial		[Nº NF],
		OPCH.SeriesStr	[Série],
		OPCH.DocDate	[Data Entrada],
		OPCH.DocTotal	[Vlr. Total],
		PCH5.TaxbleAmnt	[Base Cálculo],
		PCH5.Rate		[Alíquota],
		PCH5.WTAmnt		[ISS Retido],
		CRD1.State		[UF],
		CRD1.City		[Cidade],
		CRD7.TaxId0		[CNPJ/CPF],
		CASE WHEN OCRD.U_CVA_SimplesNacional = 'Y'
			THEN 'Sim'
			ELSE 'Não'
		END [Simples Nacional]
	FROM OPCH WITH(NOLOCK)
		INNER JOIN OCRD WITH(NOLOCK)
			ON OCRD.CardCode = OPCH.CardCode
		CROSS APPLY 
		(
			SELECT TOP 1 * FROM CRD7 WITH (NOLOCK)
			WHERE CRD7.CardCode = OPCH.CardCode
			AND ISNULL(CRD7.TaxId0, '') <> ''
			--AND ISNULL(CRD7.Address, '') <> ''
		) AS CRD7
		CROSS APPLY 
		(
			SELECT TOP 1 OCNT.State, OCNT.Name City, OCNT.IbgeCode FROM CRD1 WITH (NOLOCK)
				INNER JOIN OCNT WITH (NOLOCK)
					ON OCNT.AbsId = CRD1.County
			WHERE CRD1.CardCode = OPCH.CardCode
			AND CRD1.AdresType = 'B'
		) AS CRD1
		LEFT JOIN 
		(
				SELECT PCH5.AbsEntry, PCH5.Rate, PCH5.TaxbleAmnt, SUM(PCH5.WTAmnt) WTAmnt
				FROM PCH5 WITH(NOLOCK) 
					INNER JOIN OWHT WITH(NOLOCK) ON OWHT.WTCode = PCH5.WTCode
					INNER JOIN OWTT WITH(NOLOCK) ON OWTT.WTTypeId = OWHT.WTTypeId AND OWTT.WTType = 'ISS'
				GROUP BY PCH5.AbsEntry, PCH5.Rate, PCH5.TaxbleAmnt
		) PCH5
			ON PCH5.AbsEntry = OPCH.DocEntry
	WHERE OPCH.CANCELED = 'N'
	AND OPCH.DocType = 'S'
	AND (OPCH.BPLId = @Branch OR ISNULL(@Branch, 0) = 0)
	AND OPCH.DocDate BETWEEN @DateFrom AND @DateTo
END