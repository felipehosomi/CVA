IF EXISTS (SELECT * FROM sys.objects WHERE type = 'IF' AND name = 'FN_CVA_SEF_0150')
	DROP FUNCTION FN_CVA_SEF_0150
GO
CREATE FUNCTION FN_CVA_SEF_0150
(
	@Filial			INT,
	@DataInicial	DATETIME,
	@DataFinal		DATETIME
) RETURNS TABLE
AS
RETURN
(
	WITH PN AS
	(
		SELECT CardCode FROM OPDN WITH(NOLOCK) WHERE CANCELED = 'N' AND BPLId = @Filial AND DocDate BETWEEN @DataInicial AND @DataFinal AND Model IN (1, 3, 5, 39) GROUP BY CardCode

		UNION

		SELECT CardCode FROM OPCH WITH(NOLOCK) WHERE CANCELED = 'N' AND BPLId = @Filial AND DocDate BETWEEN @DataInicial AND @DataFinal AND Model IN (1, 3, 5, 39) GROUP BY CardCode

		UNION

		SELECT CardCode FROM ORDN WITH(NOLOCK) WHERE CANCELED = 'N' AND BPLId = @Filial AND DocDate BETWEEN @DataInicial AND @DataFinal AND Model IN (1, 3, 5, 39) GROUP BY CardCode

		UNION

		SELECT CardCode FROM ORIN WITH(NOLOCK) WHERE CANCELED = 'N' AND BPLId = @Filial AND DocDate BETWEEN @DataInicial AND @DataFinal AND Model IN (1, 3, 5, 39) GROUP BY CardCode

		UNION

		SELECT CardCode FROM ODLN WITH(NOLOCK) WHERE CANCELED = 'N' AND BPLId = @Filial AND DocDate BETWEEN @DataInicial AND @DataFinal AND Model IN (1, 3, 5, 39) GROUP BY CardCode

		UNION

		SELECT CardCode FROM OINV WITH(NOLOCK) WHERE CANCELED = 'N' AND BPLId = @Filial AND DocDate BETWEEN @DataInicial AND @DataFinal AND Model IN (1, 3, 5, 39) GROUP BY CardCode

		UNION

		SELECT CardCode FROM ORPD WITH(NOLOCK) WHERE CANCELED = 'N' AND BPLId = @Filial AND DocDate BETWEEN @DataInicial AND @DataFinal AND Model IN (1, 3, 5, 39) GROUP BY CardCode

		UNION

		SELECT CardCode FROM ORPC WITH(NOLOCK) WHERE CANCELED = 'N' AND BPLId = @Filial AND DocDate BETWEEN @DataInicial AND @DataFinal AND Model IN (1, 3, 5, 39) GROUP BY CardCode
	)

	SELECT 
		'0150'						LIN,
		OCRD.CardCode				COD_PART, 
		OCRD.CardName				NOME,
		'01058'						COD_PAIS,
		ISNULL(TaxId0,'')			CNPJ, 
		''							CPF,
		''							VAZIO,
		ISNULL(CRD1.[State],'')		UF, 
		ISNULL(TaxId1,'')			IE,
		''							IE_ST,
		ISNULL(OCNT.IbgeCode,'')	COD_MUN,
		''							IM,
		ISNULL(TaxId8,'')			SUFRAMA
	FROM PN
		INNER JOIN OCRD WITH(NOLOCK)
			ON OCRD.CardCode = PN.CardCode
		OUTER APPLY
		(
			SELECT TOP 1 * FROM CRD1 WITH(NOLOCK)
			WHERE CRD1.CardCode = OCRD.CardCode
			AND CRD1.AdresType = 'B'
		) CRD1
		OUTER APPLY
		(
			SELECT TOP 1 * FROM CRD7 WITH(NOLOCK)
			WHERE CRD7.CardCode = OCRD.CardCode
			AND ISNULL(CRD7.TaxId0, '') <> ''
		) CRD7
		INNER JOIN OCNT WITH(NOLOCK)
			ON OCNT.AbsId = CRD1.County
)
