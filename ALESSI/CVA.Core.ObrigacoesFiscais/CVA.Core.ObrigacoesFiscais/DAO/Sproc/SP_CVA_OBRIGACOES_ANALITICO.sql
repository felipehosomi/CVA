IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_OBRIGACOES_ANALITICO')
	DROP PROCEDURE SP_CVA_OBRIGACOES_ANALITICO
GO
CREATE PROCEDURE SP_CVA_OBRIGACOES_ANALITICO
(
	@Table NVARCHAR(MAX),
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
			O' + @Table + '.DocEntry	[ID],
			O' + @Table + '.ObjType		[Tipo Doc.],
			' + @Table + '1.LineNum		[N� Linha],
			' + @Table + '1.ItemCode	[N� Item],
			' + @Table + '1.Dscription	[Descri��o],
			' + @Table + '1.Quantity	[Quantidade],
			' + @Table + '1.Price		[Pre�o],
			' + @Table + '1.DiscPrcnt	[% Desconto],
			' + @Table + '1.LineTotal	[Total Linha],
			' + @Table + '1.WhsCode		[Dep�sito],
			' + @Table + '1.AcctCode	[C�d. Conta],
			' + @Table + '1.TotalSumSy	[Total Linha (MS)],
			' + @Table + '1.VatSum		[Imposto Total],
			' + @Table + '1.FinncPriod	[Per�odo Cont�bil],
			' + @Table + '1.DistribSum	[Valor Distribu�do],
			' + @Table + '1.TaxCode		[C�d. Imposto],
			' + @Table + '1.TaxType		[Tipo Imposto],
			' + @Table + '1.CFOPCode	[CFOP],
			' + @Table + '1.CSTCode		[CST ICMS],
			' + @Table + '1.Usage		[Utiliza��o],
			' + @Table + '1.TaxOnly		[S� Imposto],
			' + @Table + '1.CSTfIPI		[CST IPI],
			' + @Table + '1.CSTfPIS		[CST PIS],
			' + @Table + '1.CSTfCOFINS	[CST COFINS],
			ICMS.TaxSum				[Total ICMS],
			ICMS.BaseSum			[BC ICMS],
			ICMS.U_Outros			[Outras ICMS],
			ICMS.U_Isento			[Isentas ICMS],
			ICMS_ST.TaxSum			[Total ICMS-ST],
			ICMS_ST.BaseSum			[Total BC ICMS-ST],
			ICMS_ST.U_Outros		[Outras ICMS-ST],
			ICMS_ST.U_Isento		[Isentas ICMS-ST],
			IPI.TaxSum				[Total IPI],
			IPI.BaseSum				[BC IPI],
			IPI.NonDdctPrc			[% N�o Dedut�vel IPI],
			IPI.U_Outros			[Outras IPI],
			IPI.U_Isento			[Isentas IPI],
			PIS.TaxSum				[Total PIS],
			PIS.BaseSum				[BC PIS],
			PIS.U_Outros			[Outras PIS],
			PIS.U_Isento			[Isentas PIS],
			COFINS.TaxSum			[Total COFINS],
			COFINS.BaseSum			[BC COFINS],
			COFINS.U_Outros			[Outras COFINS],
			COFINS.U_Isento			[Isentas COFINS],
			ISS.TaxSum				[Total ISS],
			ISS.BaseSum				[BC ISS],
			ISS.U_Outros			[Outras ISS],
			ISS.U_Isento			[Isentas ISS]
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
					ON ' + @Table + '5.AbsEntry = Impostos.ID
					AND ' + @Table + '5.LineNum = Impostos.[N� Linha]	
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
	ORDER BY ID'

	SET @Params = N'@P1 DATETIME, @P2 DATETIME, @P3 INT'

	EXEC sp_executesql @Query, @Params, @P1 = @DateFrom, @P2 = @DateTo, @P3 = @BPlId
END