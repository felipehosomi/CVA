IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_PERDCOMP')
	DROP PROCEDURE SP_CVA_PERDCOMP
GO
CREATE PROCEDURE SP_CVA_PERDCOMP
(
	@Table NVARCHAR(3),
	@DateFrom DATETIME = NULL,
	@DateTo DATETIME = NULL,
	@BPlId INT = NULL,
	@Period NVARCHAR(3)
)
AS
BEGIN
	DECLARE @Query AS NVARCHAR(MAX)
	DECLARE @Params AS NVARCHAR(MAX)

	SET @Query = 
	';WITH Impostos AS
	(
		SELECT DISTINCT
			O' + @Table + '.DocEntry,
			O' + @Table + '.ObjType,
			' + @Table + '1.LineNum,
			' + @Table + '1.ItemCode,
			' + @Table + '1.Dscription,
			' + @Table + '1.Quantity,
			' + @Table + '1.Price,
			' + @Table + '1.DiscPrcnt,
			' + @Table + '1.LineTotal,
			' + @Table + '1.WhsCode,
			' + @Table + '1.AcctCode,
			' + @Table + '1.TotalSumSy,
			' + @Table + '1.VatSum,
			' + @Table + '1.FinncPriod,
			' + @Table + '1.DistribSum,
			' + @Table + '1.TaxCode,
			' + @Table + '1.TaxType,
			' + @Table + '1.CFOPCode,
			' + @Table + '1.CSTCode,
			' + @Table + '1.Usage,
			' + @Table + '1.TaxOnly,
			' + @Table + '1.CSTfIPI,
			' + @Table + '1.CSTfPIS,
			' + @Table + '1.CSTfCOFINS,
			ICMS.TaxSum			ICMS_NR,
			ICMS.BaseSum		ICMS_BC,
			ICMS.U_Isento		ICMS_Isento,
			ICMS.U_Outros		ICMS_Outros,
			ICMS_ST.TaxSum		ICMS_ST,
			ICMS_ST.BaseSum		ICMS_ST_BC,
			ICMS_ST.U_Isento	ICMS_ST_Isento,
			ICMS_ST.U_Outros	ICMS_ST_Outros,
			IPI.TaxSum			IPI_NR,
			IPI.BaseSum			IPI_BC,
			IPI.NonDdctPrc		IPI_NaoDedutivel,
			IPI.U_Isento		IPI_Isento,
			IPI.U_Outros		IPI_Outros,
			PIS.TaxSum			PIS_NR,
			PIS.BaseSum			PIS_BC,
			PIS.U_Isento		PIS_Isento,
			PIS.U_Outros		PIS_Outros,
			COFINS.TaxSum		COFINS_NR,
			COFINS.BaseSum		COFINS_BC,
			COFINS.U_Isento		COFINS_Isento,
			COFINS.U_Outros		COFINS_Outros,
			ISS.TaxSum			ISS_NR,
			ISS.BaseSum			ISS_BC,
			ISS.U_Isento		ISS_Isento,
			ISS.U_Outros		ISS_Outros
		FROM O' + @Table + ' WITH(NOLOCK)
			INNER JOIN ' + @Table + '1 WITH(NOLOCK)
				ON ' + @Table + '1.DocEntry = O' + @Table + '.DocEntry
			LEFT JOIN (
				SELECT ' + @Table + '4.* FROM ' + @Table + '4 WITH(NOLOCK)
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = ' + @Table + '4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = ''ICMS''
				) ICMS
				ON ICMS.DocEntry = ' + @Table + '1.DocEntry
				AND ICMS.LineNum = ' + @Table + '1.LineNum
			LEFT JOIN (
				SELECT ' + @Table + '4.* FROM ' + @Table + '4 WITH(NOLOCK)
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = ' + @Table + '4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = ''ICMS-ST''
				) ICMS_ST
				ON ICMS_ST.DocEntry = ' + @Table + '1.DocEntry
				AND ICMS_ST.LineNum = ' + @Table + '1.LineNum
			LEFT JOIN (
				SELECT ' + @Table + '4.* FROM ' + @Table + '4 WITH(NOLOCK)
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = ' + @Table + '4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = ''IPI''
				) IPI
				ON IPI.DocEntry = ' + @Table + '1.DocEntry
				AND IPI.LineNum = ' + @Table + '1.LineNum
			LEFT JOIN (
				SELECT ' + @Table + '4.* FROM ' + @Table + '4 WITH(NOLOCK)
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = ' + @Table + '4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = ''PIS''
				) PIS
				ON PIS.DocEntry = ' + @Table + '1.DocEntry
				AND PIS.LineNum = ' + @Table + '1.LineNum
			LEFT JOIN (
				SELECT ' + @Table + '4.* FROM ' + @Table + '4 WITH(NOLOCK)
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = ' + @Table + '4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = ''COFINS''
				) COFINS
				ON COFINS.DocEntry = ' + @Table + '1.DocEntry
				AND COFINS.LineNum = ' + @Table + '1.LineNum
			LEFT JOIN (
				SELECT ' + @Table + '4.* FROM ' + @Table + '4 WITH(NOLOCK)
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = ' + @Table + '4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN (''ISS'', ''ISSQN'')
				) ISS
				ON ISS.DocEntry = ' + @Table + '1.DocEntry
				AND ISS.LineNum = ' + @Table + '1.LineNum
		WHERE O' + @Table + '.DocDate BETWEEN @P1 AND @P2
		AND  O' + @Table + '.BPLId = @P3
	)
	, ImpostosRetidos AS
	(
		SELECT * FROM
		(
			SELECT 
			Impostos.*,
			' + @Table + '5.WTAmnt,
			OWTT.WTType
			FROM Impostos
				LEFT JOIN ' + @Table + '5 WITH(NOLOCK)
					ON ' + @Table + '5.AbsEntry = Impostos.DocEntry
					AND ' + @Table + '5.LineNum = Impostos.LineNum		
				LEFT JOIN OWHT WITH(NOLOCK)
					ON OWHT.WTCode = ' + @Table + '5.WtCode
				LEFT JOIN OWTT WITH(NOLOCK)
					ON OWTT.WTTypeId = OWHT.WTTypeId
		) AS P
		PIVOT
		(
			SUM(P.WTAmnt)
			FOR P.WTType IN ([IRRF], [PIS], [COFINS], [ISS], [CSLL], [CRFS])
		) AS PVT
	)
	SELECT * FROM ImpostosRetidos
	ORDER BY DocEntry'

	SET @Params = N'@P1 DATETIME, @P2 DATETIME, @P3 INT'

	EXEC sp_executesql @Query, @Params, @P1 = @DateFrom, @P2 = @DateTo, @P3 = @BPlId
END