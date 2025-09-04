IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_OBRIGACOES_SINTETICO')
	DROP PROCEDURE SP_CVA_OBRIGACOES_SINTETICO
GO
CREATE PROCEDURE SP_CVA_OBRIGACOES_SINTETICO
(
	@Table NVARCHAR(MAX),
	@DateFrom DATETIME = NULL,
	@DateTo DATETIME = NULL,
	@BPlId INT = NULL,
	@Period INT
)
AS
BEGIN
	DECLARE @Query AS NVARCHAR(MAX)
	DECLARE @Params AS NVARCHAR(MAX)

	SET @Query = 
	';WITH Impostos AS
	(
		SELECT DISTINCT
			O' + @Table + '.DocEntry			[ID],
			O' + @Table + '.DocNum				[Nº Doc.],
			O' + @Table + '.DocType				[Tipo Doc.],
			O' + @Table + '.CANCELED			[Cancelado],
			O' + @Table + '.ObjType				[Tipo Objeto],
			MONTH(O' + @Table + '.DocDate)		[Mês Apuração],
			YEAR(O' + @Table + '.DocDate)		[Ano Apuração],
			@P4									[Período Apuração],
			O' + @Table + '.DocDate				[Data Lançamento],
			O' + @Table + '.DocDueDate			[Data Vencimento],
			O' + @Table + '.CardCode			[Cód. PN],
			O' + @Table + '.CardName			[Nome PN],
			CASE WHEN ISNULL(' + @Table + '12.TaxId0, '''') <> ''''
				THEN ' + @Table + '12.TaxId0
				ELSE ' + @Table + '12.TaxId4
			END								[CNPJ/CPF],
			' + @Table + '12.TaxId1			[IE PN],
			' + @Table + '12.AddrTypeS		[Logradouro],
			' + @Table + '12.StreetS		[Endereço],
			' + @Table + '12.StreetNoS		[Nº],
			' + @Table + '12.BlockS			[Bairro],
			' + @Table + '12.CityS			[Cidade],
			' + @Table + '12.ZipCodeS		[CEP],
			' + @Table + '12.StateS			[UF],
			' + @Table + '12.CountyS		[ID Cidade],
			' + @Table + '12.CountryS		[País],
			O' + @Table + '.VatSum			[Imposto Total],
			O' + @Table + '.DiscPrcnt		[% Desconto Doc.],
			O' + @Table + '.DiscSum			[Desconto Total],
			O' + @Table + '.DiscSumFC		[Desconto Total (ME)],
			O' + @Table + '.DocTotal		[Total Doc.],
			O' + @Table + '.Comments		[Observações],
			O' + @Table + '.JrnlMemo		[Obs. Diário],
			O' + @Table + '.TransId			[Nº Transação],
			O' + @Table + '.TotalExpns		[Despesas Adicionais],
			O' + @Table + '.WTSum			[Valor IRF],
			O' + @Table + '.BPLId			[Filial],
			O' + @Table + '.BPLName			[Nome Filial],
			OBPL.TaxIdNum					[CNPJ Filial],
			OBPL.TaxIdNum2					[IE Filial],
			CASE WHEN ISNULL(@P3, 0) = 0
				THEN ''''
				ELSE OBPL.TaxIdNum
			END								[Filial Selecionada],
			OBPLMain.TaxIdNum				[CNPJ Matriz],
			OBPLMain.TaxIdNum2				[IE Matriz],
			O' + @Table + '.SeqCode			[Cód. Sequência],
			O' + @Table + '.Serial			[Nº Série],
			O' + @Table + '.SeriesStr		[Cadeia Séries],
			O' + @Table + '.SubStr			[Cadeia Subséries],
			O' + @Table + '.Model			[Modelo NF],
			O' + @Table + '.TaxOnExp		[Imposto Desp. Adicionais],
			' + @Table + '1.CFOPCode		[CFOP],
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
			IPI.NonDdctPrc			[% Não Dedutível IPI],
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
				AND ' + @Table + '1.VisOrder = 0
			INNER JOIN ' + @Table + '12 WITH(NOLOCK)
				ON ' + @Table + '12.DocEntry = O' + @Table + '.DocEntry
			INNER JOIN OBPL WITH(NOLOCK)
				ON OBPL.BPLId = O' + @Table + '.BPLId
			INNER JOIN OBPL OBPLMain WITH(NOLOCK)
				ON OBPLMain.MainBPL = ''Y''
			LEFT JOIN (
					SELECT ' + @Table + '4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM ' + @Table + '4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = ' + @Table + '4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = ''ICMS''
					GROUP BY ' + @Table + '4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) ICMS
				ON ICMS.DocEntry = O' + @Table + '.DocEntry
			LEFT JOIN (
					SELECT ' + @Table + '4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM ' + @Table + '4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = ' + @Table + '4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = ''ICMS-ST''
					GROUP BY ' + @Table + '4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) ICMS_ST
				ON ICMS_ST.DocEntry = O' + @Table + '.DocEntry
			LEFT JOIN (
					SELECT ' + @Table + '4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM ' + @Table + '4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = ' + @Table + '4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = ''IPI''
					GROUP BY ' + @Table + '4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) IPI
				ON IPI.DocEntry = O' + @Table + '.DocEntry
			LEFT JOIN (
					SELECT ' + @Table + '4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM ' + @Table + '4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = ' + @Table + '4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = ''PIS''
					GROUP BY ' + @Table + '4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) PIS
				ON PIS.DocEntry = O' + @Table + '.DocEntry
			LEFT JOIN (
					SELECT ' + @Table + '4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM ' + @Table + '4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = ' + @Table + '4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = ''COFINS''
					GROUP BY ' + @Table + '4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) COFINS
				ON COFINS.DocEntry = O' + @Table + '.DocEntry
			LEFT JOIN (
					SELECT ' + @Table + '4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM ' + @Table + '4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = ' + @Table + '4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN (''ISS'', ''ISSQN'')
					GROUP BY ' + @Table + '4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) ISS
				ON ISS.DocEntry = O' + @Table + '.DocEntry
		WHERE O' + @Table + '.DocDate BETWEEN @P1 AND @P2
		AND  (ISNULL(@P3, 0) = 0 OR O' + @Table + '.BPLId = @P3)
	)
	, ImpostosRetidos AS
	(
		SELECT * FROM
		(
			SELECT 
			Impostos.*,
			' + @Table + '5.WTAmnt,
			' + @Table + '5.WTType
			FROM Impostos
				LEFT JOIN
				(
					SELECT ' + @Table + '5.AbsEntry, SUM(' + @Table + '5.WTAmnt) AS WTAmnt, OWTT.WTType
					FROM ' + @Table + '5 WITH(NOLOCK)
						INNER JOIN OWHT WITH(NOLOCK) ON OWHT.WTCode = ' + @Table + '5.WtCode
						INNER JOIN OWTT WITH(NOLOCK) ON OWTT.WTTypeId = OWHT.WTTypeId
					GROUP BY ' + @Table + '5.AbsEntry, OWTT.WTType
				) ' + @Table + '5
				ON ' + @Table + '5.AbsEntry = Impostos.ID
		) AS P
		PIVOT
		(
			SUM(P.WTAmnt)
			FOR P.WTType IN ([IRRF], [PIS], [COFINS], [ISS], [CSLL], [CRFS])
		) AS PVT
	)
	SELECT * FROM ImpostosRetidos
	ORDER BY ID'

	SET @Params = N'@P1 DATETIME, @P2 DATETIME, @P3 INT, @P4 INT'

	EXEC sp_executesql @Query, @Params, @P1 = @DateFrom, @P2 = @DateTo, @P3 = @BPlId, @P4 = @Period
END