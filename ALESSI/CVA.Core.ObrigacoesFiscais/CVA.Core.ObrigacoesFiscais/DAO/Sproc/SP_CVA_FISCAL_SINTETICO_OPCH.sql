IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_FISCAL_SINTETICO_OPCH')
	DROP PROCEDURE SP_CVA_FISCAL_SINTETICO_OPCH
GO
CREATE PROCEDURE SP_CVA_FISCAL_SINTETICO_OPCH
(
	@DateFrom DATETIME = NULL,
	@DateTo DATETIME = NULL,
	@BPlId INT = NULL,
	@Period NVARCHAR(3)
)
AS
BEGIN
	;WITH Impostos AS
	(
		SELECT DISTINCT
			OPCH.DocEntry			[ID],
			OPCH.DocNum				[Nº Doc.],
			OPCH.DocType			[Tipo Doc.],
			OPCH.CANCELED			[Cancelado],
			OPCH.ObjType			[Tipo Objeto],
			MONTH(OPCH.DocDate)		[Mês Apuração],
			YEAR(OPCH.DocDate)		[Ano Apuração],
			@Period					[Período Apuração],
			OPCH.DocDate			[Data Lançamento],
			OPCH.DocDueDate			[Data Vencimento],
			OPCH.CardCode			[Cód. PN],
			OPCH.CardName			[Nome PN],
			CASE WHEN ISNULL(PCH12.TaxId0, '') <> ''
				THEN PCH12.TaxId0
				ELSE PCH12.TaxId4
			END						[CNPJ/CPF],
			PCH12.TaxId1			[IE PN],
			PCH12.AddrTypeS			[Logradouro],
			PCH12.StreetS			[Endereço],
			PCH12.StreetNoS			[Nº],
			PCH12.BlockS			[Bairro],
			PCH12.CityS				[Cidade],
			PCH12.ZipCodeS			[CEP],
			PCH12.StateS			[UF],
			PCH12.CountyS			[ID Cidade],
			PCH12.CountryS			[País],
			OPCH.VatSum				[Imposto Total],
			OPCH.DiscPrcnt			[% Desconto Doc.],
			OPCH.DiscSum			[Desconto Total],
			OPCH.DiscSumFC			[Desconto Total (ME)],
			OPCH.DocTotal			[Total Doc.],
			OPCH.Comments			[Observações],
			OPCH.JrnlMemo			[Obs. Diário],
			OPCH.TransId			[Nº Transação],
			OPCH.TotalExpns			[Despesas Adicionais],
			OPCH.WTSum				[Valor IRF],
			OPCH.BPLId				[Filial],
			OPCH.BPLName			[Nome Filial],
			OBPL.TaxIdNum			[CNPJ Filial],
			OBPL.TaxIdNum2			[IE Filial],
			CASE WHEN ISNULL(@BPlId, 0) = 0
				THEN ''
				ELSE OBPL.TaxIdNum
			END						[Filial Selecionada],
			OBPLMain.TaxIdNum		[CNPJ Matriz],
			OBPLMain.TaxIdNum2		[IE Matriz],
			OPCH.SeqCode			[Cód. Sequência],
			OPCH.Serial				[Nº Série],
			OPCH.SeriesStr			[Cadeia Séries],
			OPCH.SubStr				[Cadeia Subséries],
			OPCH.Model				[Modelo NF],
			OPCH.TaxOnExp			[Imposto Desp. Adicionais],
			PCH1.CFOPCode			[CFOP],
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
		FROM OPCH WITH(NOLOCK)
			INNER JOIN PCH1 WITH(NOLOCK)
				ON PCH1.DocEntry = OPCH.DocEntry
				AND PCH1.VisOrder = 0
			INNER JOIN PCH12 WITH(NOLOCK)
				ON PCH12.DocEntry = OPCH.DocEntry
			INNER JOIN OBPL WITH(NOLOCK)
				ON OBPL.BPLId = OPCH.BPLId
			INNER JOIN OBPL OBPLMain WITH(NOLOCK)
				ON OBPLMain.MainBPL = 'Y'
			LEFT JOIN (
					SELECT PCH4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM PCH4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
					GROUP BY PCH4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) ICMS
				ON ICMS.DocEntry = OPCH.DocEntry
			LEFT JOIN (
					SELECT PCH4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM PCH4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
					GROUP BY PCH4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) ICMS_ST
				ON ICMS_ST.DocEntry = OPCH.DocEntry
			LEFT JOIN (
					SELECT PCH4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM PCH4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'
					GROUP BY PCH4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) IPI
				ON IPI.DocEntry = OPCH.DocEntry
			LEFT JOIN (
					SELECT PCH4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM PCH4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'PIS'
					GROUP BY PCH4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) PIS
				ON PIS.DocEntry = OPCH.DocEntry
			LEFT JOIN (
					SELECT PCH4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM PCH4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'COFINS'
					GROUP BY PCH4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) COFINS
				ON COFINS.DocEntry = OPCH.DocEntry
			LEFT JOIN (
					SELECT PCH4.DocEntry, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, NonDdctPrc, U_Isento, U_Outros FROM PCH4 WITH(NOLOCK)
						INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
						INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code IN ('ISS', 'ISSQN')
					GROUP BY PCH4.DocEntry, NonDdctPrc, U_Isento, U_Outros
				) ISS
				ON ISS.DocEntry = OPCH.DocEntry
		WHERE OPCH.DocDate BETWEEN @DateFrom AND @DateTo
		AND  (ISNULL(@BPlId, 0) = 0 OR OPCH.BPLId = @BPlId)
	)
	, ImpostosRetidos AS
	(
		SELECT * FROM
		(
			SELECT 
			Impostos.*,
			PCH5.WTAmnt,
			PCH5.WTType
			FROM Impostos
				LEFT JOIN
				(
					SELECT PCH5.AbsEntry, SUM(PCH5.WTAmnt) AS WTAmnt, OWTT.WTType
					FROM PCH5 WITH(NOLOCK)
						INNER JOIN OWHT WITH(NOLOCK) ON OWHT.WTCode = PCH5.WtCode
						INNER JOIN OWTT WITH(NOLOCK) ON OWTT.WTTypeId = OWHT.WTTypeId
					GROUP BY PCH5.AbsEntry, OWTT.WTType
				) PCH5
				ON PCH5.AbsEntry = Impostos.ID
		) AS P
		PIVOT
		(
			SUM(P.WTAmnt)
			FOR P.WTType IN ([IRRF], [PIS], [COFINS], [ISS], [CSLL], [CRFS])
		) AS PVT
	)
	SELECT * FROM ImpostosRetidos
	ORDER BY ID
END