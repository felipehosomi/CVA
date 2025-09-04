IF EXISTS (SELECT * FROM sys.objects WHERE type = 'TF' AND name = 'fnc_CVA_ProvisoesFluxoCaixa')
	DROP FUNCTION fnc_CVA_ProvisoesFluxoCaixa
GO
CREATE FUNCTION [dbo].[fnc_CVA_ProvisoesFluxoCaixa](@DtIni DATETIME, @DtFin DATETIME, @TpData VARCHAR(2))
RETURNS @rtnTable TABLE
(
	ShortName NVARCHAR(50),
	CardName NVARCHAR(254),
	Lancamento DATETIME,
	Vencimento DATETIME,
	Origem INT,
	OrigemNr NVARCHAR(200),
	Parcela INT,
	ParcelaTotal INT,
	Serial INT,
	LineMemo NVARCHAR(MAX),
	Debit NUMERIC(19,6),
	Credit NUMERIC(19,6),
	Saldo NUMERIC(19,6),
	DueDate DATETIME,
	PeyMethodNF NVARCHAR(MAX)
)
AS
BEGIN	
	BEGIN -- DECLARAÇÃO DE VARIÁVEIS
		DECLARE @DtAux DATETIME

		SET @DtAux = @DtIni
	END

	BEGIN -- DECLARAÇÃO DE TABELAS
		DECLARE @tb_datas TABLE
		(
			Data DATETIME,
			Dia INT,
			Mes INT,
			Ano INT,
			Semana INT,
			Trimestre INT,
			Semestre INT
		)

		DECLARE @tb_previstos TABLE
		(
			DateId DATETIME,
			Code INT,
			Dcription NVARCHAR(100),
			CredCur NVARCHAR(10),
			Credit NUMERIC(19,6),
			DebCur NVARCHAR(10),
			Debit NUMERIC(19,6),
			Frequency CHAR(1),
			Remind INT,
			EndDate DATETIME
		)
	END

	BEGIN -- POPULANDO TABELA DE DATAS
		WHILE @DtAux <= @DtFin
		BEGIN
			DECLARE @_Dia INT, @_Mes INT, @_Ano INT, @_Semana INT, @_Trimestre INT, @_Semestre INT
		
			SELECT
				@_Dia = DAY(@DtAux),
				@_Mes = MONTH(@DtAux),
				@_Ano = YEAR(@DtAux),
				@_Semana = DATEPART(WEEKDAY, @DtAux),
				@_Trimestre = DATEPART(QUARTER, @DtAux),
				@_Semestre = (DATEPART(QUARTER, @DtAux)+1)/2

			INSERT INTO @tb_datas(Data, Dia, Mes, Ano, Semana, Trimestre, Semestre) VALUES(@DtAux, @_Dia, @_Mes, @_Ano, @_Semana, @_Trimestre, @_Semestre)

			SELECT @DtAux = DATEADD(DAY, 1, @DtAux)
		END
	END

	BEGIN -- LANÇAMENTOS PREVISTOS NO FLUXO DE CAIXA
		DECLARE @dtCursor AS CURSOR, @Data DATETIME, @Dia INT, @Mes INT, @Ano INT, @Semana INT, @Trimestre INT, @Semestre INT

		SET @dtCursor = CURSOR FAST_FORWARD FOR
			SELECT Data, Dia, Mes, Ano, Semana, Trimestre, Semestre FROM @tb_datas ORDER BY Data ASC

		OPEN @dtCursor
		FETCH NEXT FROM @dtCursor INTO @Data, @Dia, @Mes, @Ano, @Semana, @Trimestre, @Semestre

		WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN -- FREQUÊNCIA: DIÁRIA
				INSERT INTO @tb_previstos
					SELECT DISTINCT DATEADD(DAY, OA_REMIND.Remind, @Data), T0.Code, T0.Dscription, T0.CredCur, T0.Credit, T0.DebCur, -T0.Debit, T0.Frequency, OA_REMIND.Remind, ISNULL(T0.EndDate, @DtFin)
					FROM OCFL T0
					OUTER APPLY (
						SELECT Remind = CAST(SUBSTRING(CAST(T0.Remind AS NVARCHAR(10)), 2, 3) AS INT)
					) AS OA_REMIND
					WHERE T0.Debit <> 0 
						AND T0.Frequency = 'D'
						AND DATEADD(DAY, OA_REMIND.Remind, @Data) > T0.DateID
						AND DATEADD(DAY, OA_REMIND.Remind, @Data) <= ISNULL(T0.EndDate, @DtFin)
						AND DATEPART(DAY, DATEADD(DAY, OA_REMIND.Remind, @Data))%OA_REMIND.Remind = 0
			END		

			BEGIN -- FREQUÊNCIA: SEMANAL
				INSERT INTO @tb_previstos
					SELECT DISTINCT @Data, T0.Code, T0.Dscription, T0.CredCur, T0.Credit, T0.DebCur, -T0.Debit, T0.Frequency, OA_REMIND.Remind, ISNULL(T0.EndDate, @DtFin)
					FROM OCFL T0
					OUTER APPLY (
						SELECT Remind = CAST(T0.Remind AS INT)
					) AS OA_REMIND
					WHERE T0.Debit <> 0 
						AND T0.Frequency = 'W'
						AND @Data > T0.DateID
						AND @Data <= ISNULL(T0.EndDate, @DtFin)
						AND DATEPART(WEEKDAY, @Data) = OA_REMIND.Remind
			END

			BEGIN -- FREQUÊNCIA: MENSAL
				INSERT INTO @tb_previstos
					SELECT DISTINCT DATEADD(MONTH, @Mes, CONVERT(DATETIME, CAST(YEAR(T0.DateId) AS NVARCHAR(10)) + '-' + CAST(MONTH(T0.DateId) AS NVARCHAR(10)) + '-' + CAST(OA_REMIND.Remind AS NVARCHAR(10)))), T0.Code, T0.Dscription, T0.CredCur, T0.Credit, T0.DebCur, -T0.Debit, T0.Frequency, OA_REMIND.Remind, ISNULL(T0.EndDate, @DtFin)
					FROM OCFL T0
					OUTER APPLY (
						SELECT Remind = CAST(SUBSTRING(CAST(T0.Remind AS NVARCHAR(10)), 2, 3) AS INT)
					) AS OA_REMIND
					WHERE T0.Debit <> 0 
						AND T0.Frequency = 'M'
						AND DATEADD(MONTH, @Mes, CONVERT(DATETIME, CAST(YEAR(T0.DateId) AS NVARCHAR(10)) + '-' + CAST(MONTH(T0.DateId) AS NVARCHAR(10)) + '-' + CAST(OA_REMIND.Remind AS NVARCHAR(10)))) > T0.DateID
						AND DATEADD(MONTH, @Mes, CONVERT(DATETIME, CAST(YEAR(T0.DateId) AS NVARCHAR(10)) + '-' + CAST(MONTH(T0.DateId) AS NVARCHAR(10)) + '-' + CAST(OA_REMIND.Remind AS NVARCHAR(10)))) <= ISNULL(T0.EndDate, @DtFin)			
			END

			FETCH NEXT FROM @dtCursor INTO @Data, @Dia, @Mes, @Ano, @Semana, @Trimestre, @Semestre
		END

		CLOSE @dtCursor
		DEALLOCATE @dtCursor

		BEGIN -- FREQUÊNCIA: TRIMESTRAL
			DECLARE @QuarterDiff INT, @QuarterAux INT
			SET @QuarterAux = 1
			SET @QuarterDiff = 12

			WHILE @QuarterAux <= @QuarterDiff
			BEGIN
				INSERT INTO @tb_previstos
					SELECT DISTINCT DATEADD(QUARTER, @QuarterAux, T0.DateID), T0.Code, T0.Dscription, T0.CredCur, T0.Credit, T0.DebCur, -T0.Debit, T0.Frequency, T0.Remind, ISNULL(T0.EndDate, @DtFin)
					FROM OCFL T0
					WHERE T0.Debit <> 0
						AND T0.Frequency = 'Q'
						AND DATEADD(QUARTER, @QuarterAux, T0.DateID) > T0.DateID
						AND DATEADD(QUARTER, @QuarterAux, T0.DateID) <= ISNULL(T0.EndDate, @DtFin)

				SET @QuarterAux = @QuarterAux + 1
			END
		END

		BEGIN -- FREQUÊNCIA: SEMESTRAL
			INSERT INTO @tb_previstos
				SELECT DISTINCT DATEADD(MONTH, 6, T0.DateID), T0.Code, T0.Dscription, T0.CredCur, T0.Credit, T0.DebCur, -T0.Debit, T0.Frequency, T0.Remind, ISNULL(T0.EndDate, @DtFin)
				FROM OCFL T0
				WHERE T0.Debit <> 0
					AND T0.Frequency = 'S'
					AND DATEADD(MONTH, 6, T0.DateID) > T0.DateID
					AND DATEADD(MONTH, 6, T0.DateID) <= ISNULL(T0.EndDate, @DtFin)
		END

		BEGIN -- FREQUÊNCIA: ANUAL
			INSERT INTO @tb_previstos
				SELECT DISTINCT DATEADD(YEAR, 1, T0.DateID), T0.Code, T0.Dscription, T0.CredCur, T0.Credit, T0.DebCur, -T0.Debit, T0.Frequency, T0.Remind, ISNULL(T0.EndDate, @DtFin)
				FROM OCFL T0
				WHERE T0.Debit <> 0
					AND T0.Frequency = 'A'
					AND DATEADD(YEAR, 1, T0.DateID) > T0.DateID
					AND DATEADD(YEAR, 1, T0.DateID) <= ISNULL(T0.EndDate, @DtFin)
		END

		BEGIN -- FREQUÊNCIA: UMA VEZ
			INSERT INTO @tb_previstos
				SELECT DISTINCT T0.DateID, T0.Code, T0.Dscription, T0.CredCur, T0.Credit, T0.DebCur, -T0.Debit, T0.Frequency, T0.Remind, ISNULL(T0.EndDate, @DtFin)
				FROM OCFL T0
				WHERE T0.Debit <> 0
					AND T0.Frequency = 'O'
					AND T0.DateID >= @DtIni
					AND T0.DateID <= ISNULL(T0.EndDate, @DtFin)
		END

		INSERT INTO @rtnTable
			SELECT DISTINCT	
				'LP' + REPLICATE('0', 4 - LEN(T0.Code)) + CAST(T0.Code AS VARCHAR),
				'Lançamento previsto',
				T0.DateId,
				T0.DateId,
				-1,
				T0.Code,
				1,
				1,
				-1,
				T0.Dcription,
				T0.Credit,
				T0.Debit*-1,
				T0.Debit*-1,
				T0.DateId,
				'N/A'
			FROM @tb_previstos T0
			WHERE T0.DateId >= @DtIni AND T0.DateId <= @DtFin
	END

	BEGIN -- TRANSAÇÕES RECORRENTES
		INSERT INTO @rtnTable
			SELECT DISTINCT 
				T6.CardCode,
				T6.CardName,
				T0.PlanDate,
				DATEADD(DAY, ISNULL(T5.InstDays, 0), T0.PlanDate),
				-1,
				T0.AbsEntry,
				ISNULL(T5.IntsNo, 1),
				ISNULL(T4.InstNum, 1),
				-1,
				'Transação recorrente',
				0.00,
				(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
				(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
				DATEADD(DAY, ISNULL(T5.InstDays, 0), T0.PlanDate),
				T1.PeyMethod
			FROM ORCL T0
				INNER JOIN ODRF T1 On T0.DocEntry = T1.DocEntry AND T0.DocObjType = T1.ObjType
				INNER JOIN DRF1 T2 ON T1.DocEntry = T2.DocEntry
				INNER JOIN OACT T3 ON T2.AcctCode = T3.AcctCode
				INNER JOIN OCTG T4 ON T1.GroupNum = T4.GroupNum
				LEFT  JOIN CTG1 T5 ON T4.GroupNum = T5.CTGCode
				INNER JOIN OCRD T6 ON T1.CardCode = T6.CardCode AND T6.CardType = 'S'
				LEFT  JOIN CRD7 T7 ON T6.CardCode = T7.CardCode ANd T7.[Address] = '' AND T7.AddrType = 'S'
			WHERE T0.[Status] = 'N'
				AND T1.DocStatus = 'O'
				ANd T1.CANCELED = 'N'
				AND T2.LineStatus = 'O'
				AND (
					(
						(DATEADD(DAY, ISNULL(T5.InstDays, 0), T0.PlanDate) >= @DtIni AND @TpData = 'V') AND (DATEADD(DAY, ISNULL(T5.InstDays, 0), T0.PlanDate) <= @DtFin AND @TpData = 'V')
					)
					OR (
						(T0.PlanDate >= @DtIni AND @TpData = 'LC') AND (T0.PlanDate <= @DtFin AND @TpData = 'LC')
					)
				)

		DECLARE @MonthDiff INT, @MonthAux INT
		SET @MonthAux = 0
		SET @QuarterAux = 0
		SELECT @QuarterDiff = DATEDIFF(QUARTER, @DtIni, @DtFin)
		SELECT @MonthDiff = DATEDIFF(MONTH, @DtIni, @DtFin)

		IF EXISTS (
			SELECT 1 FROM ORCP WHERE IsRemoved = 'N' AND Frequency = 'Q'
		)
		BEGIN
			WHILE @QuarterAux <= @QuarterDiff
			BEGIN
				INSERT INTO @rtnTable
					SELECT DISTINCT 
						T6.CardCode,
						T6.CardName,
						DATEADD(QUARTER, @QuarterAux, T0.StartDate),
						DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(QUARTER, @QuarterAux, T0.StartDate)),
						-1,
						T0.AbsEntry,
						ISNULL(T5.IntsNo, 1),
						ISNULL(T4.InstNum, 1),
						-1,
						'Transação recorrente',
						0.00,
						(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
						(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
						DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(QUARTER, @QuarterAux, T0.StartDate)),
						T1.PeyMethod
					FROM ORCP T0
						INNER JOIN ODRF T1 On T0.DraftEntry = T1.DocEntry AND T0.DocObjType = T1.ObjType
						INNER JOIN DRF1 T2 ON T1.DocEntry = T2.DocEntry
						INNER JOIN OACT T3 ON T2.AcctCode = T3.AcctCode
						INNER JOIN OCTG T4 ON T1.GroupNum = T4.GroupNum
						LEFT  JOIN CTG1 T5 ON T4.GroupNum = T5.CTGCode
						INNER JOIN OCRD T6 ON T1.CardCode = T6.CardCode AND T6.CardType = 'S'
						LEFT  JOIN CRD7 T7 ON T6.CardCode = T7.CardCode ANd T7.[Address] = '' AND T7.AddrType = 'S'
					WHERE T0.IsRemoved = 'N'
						AND T1.DocStatus = 'O'
						ANd T1.CANCELED = 'N'
						AND T2.LineStatus = 'O'
						AND T0.Frequency = 'Q'
						AND (
							(
								(DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(QUARTER, @QuarterAux, T0.StartDate)) >= @DtIni AND @TpData = 'V')
								AND (DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(QUARTER, @QuarterAux, T0.StartDate)) <= @DtFin AND @TpData = 'V')
							)
							OR	(
								(DATEADD(QUARTER, @QuarterAux, T0.StartDate) >= @DtIni AND @TpData = 'LC')
								AND (DATEADD(QUARTER, @QuarterAux, T0.StartDate) <= @DtFin AND @TpData = 'LC')
							)
						)

				SET @QuarterAux = @QuarterAux + 1
			END
		END

		IF EXISTS (
			SELECT 1 FROM ORCP WHERE IsRemoved = 'N' AND Frequency = 'M'
		)
		BEGIN
			WHILE @MonthAux <= @MonthDiff
			BEGIN
				INSERT INTO @rtnTable
					SELECT DISTINCT 
						T6.CardCode,
						T6.CardName,
						DATEADD(MONTH, @MonthAux, OA_REMIND.Remind),
						DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(MONTH, @MonthAux, OA_REMIND.Remind)),
						-1,
						T0.AbsEntry,
						ISNULL(T5.IntsNo, 1),
						ISNULL(T4.InstNum, 1),
						-1,
						'Transação recorrente',
						0.00,
						(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
						(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
						DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(MONTH, @MonthAux, OA_REMIND.Remind)),
						T1.PeyMethod
					FROM ORCP T0
						INNER JOIN ODRF T1 On T0.DraftEntry = T1.DocEntry AND T0.DocObjType = T1.ObjType
						INNER JOIN DRF1 T2 ON T1.DocEntry = T2.DocEntry
						INNER JOIN OACT T3 ON T2.AcctCode = T3.AcctCode
						INNER JOIN OCTG T4 ON T1.GroupNum = T4.GroupNum
						LEFT  JOIN CTG1 T5 ON T4.GroupNum = T5.CTGCode
						INNER JOIN OCRD T6 ON T1.CardCode = T6.CardCode AND T6.CardType = 'S'
						LEFT  JOIN CRD7 T7 ON T6.CardCode = T7.CardCode ANd T7.[Address] = '' AND T7.AddrType = 'S'
					OUTER APPLY (
						SELECT Remind = CONVERT(DATETIME, CAST(YEAR(T0.StartDate) AS NVARCHAR(10)) + '-' + CAST(MONTH(T0.StartDate) AS NVARCHAR(10)) + '-' + SUBSTRING(CAST(T0.Remind AS NVARCHAR(10)), 3, 2))
					) AS OA_REMIND
					WHERE T0.IsRemoved = 'N'
						AND T1.DocStatus = 'O'
						ANd T1.CANCELED = 'N'
						AND T2.LineStatus = 'O'
						AND T0.Frequency = 'M'
						AND (
							(
								(DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(MONTH, @MonthAux, OA_REMIND.Remind)) >= @DtIni AND @TpData = 'V')
								AND (DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(MONTH, @MonthAux, OA_REMIND.Remind)) <= @DtFin AND @TpData = 'V')
							)
							OR 
							(
								(DATEADD(MONTH, @MonthAux, OA_REMIND.Remind) >= @DtIni AND @TpData = 'LC')
								AND (DATEADD(MONTH, @MonthAux, OA_REMIND.Remind) <= @DtFin AND @TpData = 'LC')
							)
						)

				SET @MonthAux = @MonthAux + 1
			END
		END

		INSERT INTO @rtnTable
			SELECT DISTINCT 
				T6.CardCode,
				T6.CardName,
				DATEADD(MONTH, 6, OA_REMIND.Remind),
				DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(MONTH, 6, OA_REMIND.Remind)),
				-1,
				T0.AbsEntry,
				ISNULL(T5.IntsNo, 1),
				ISNULL(T4.InstNum, 1),
				-1,
				'Transação recorrente',
				0.00,
				(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
				(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
				DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(MONTH, 6, OA_REMIND.Remind)),
				T1.PeyMethod
			FROM ORCP T0
				INNER JOIN ODRF T1 On T0.DraftEntry = T1.DocEntry AND T0.DocObjType = T1.ObjType
				INNER JOIN DRF1 T2 ON T1.DocEntry = T2.DocEntry
				INNER JOIN OACT T3 ON T2.AcctCode = T3.AcctCode
				INNER JOIN OCTG T4 ON T1.GroupNum = T4.GroupNum
				LEFT  JOIN CTG1 T5 ON T4.GroupNum = T5.CTGCode
				INNER JOIN OCRD T6 ON T1.CardCode = T6.CardCode AND T6.CardType = 'S'
				LEFT  JOIN CRD7 T7 ON T6.CardCode = T7.CardCode ANd T7.[Address] = '' AND T7.AddrType = 'S'
			OUTER APPLY (
				SELECT Remind = CONVERT(DATETIME, CAST(YEAR(T0.StartDate) AS NVARCHAR(10)) + '-' + CAST(MONTH(T0.StartDate) AS NVARCHAR(10)) + '-' + SUBSTRING(CAST(T0.Remind AS NVARCHAR(10)), 3, 2))
			) AS OA_REMIND
			WHERE T0.IsRemoved = 'N'
				AND T1.DocStatus = 'O'
				ANd T1.CANCELED = 'N'
				AND T2.LineStatus = 'O'
				AND T0.Frequency = 'S'
				AND (
					(
						(DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(MONTH, 6, OA_REMIND.Remind)) >= @DtIni AND @TpData = 'V')
						AND (DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(MONTH, 6, OA_REMIND.Remind)) <= @DtFin AND @TpData = 'V')
					)
					OR
					(
						(DATEADD(MONTH, 6, OA_REMIND.Remind) >= @DtIni AND @TpData = 'LC')
						AND (DATEADD(MONTH, 6, OA_REMIND.Remind) <= @DtFin AND @TpData = 'LC')
					)
				)

		INSERT INTO @rtnTable
			SELECT DISTINCT 
				T6.CardCode,
				T6.CardName,
				DATEADD(YEAR, 1, OA_REMIND.Remind),
				DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(YEAR, 1, OA_REMIND.Remind)),
				-1,
				T0.AbsEntry,
				ISNULL(T5.IntsNo, 1),
				ISNULL(T4.InstNum, 1),
				-1,
				'Transação recorrente',
				0.00,
				(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
				(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
				DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(YEAR, 1, OA_REMIND.Remind)),
				T1.PeyMethod
			FROM ORCP T0
				INNER JOIN ODRF T1 On T0.DraftEntry = T1.DocEntry AND T0.DocObjType = T1.ObjType
				INNER JOIN DRF1 T2 ON T1.DocEntry = T2.DocEntry
				INNER JOIN OACT T3 ON T2.AcctCode = T3.AcctCode
				INNER JOIN OCTG T4 ON T1.GroupNum = T4.GroupNum
				LEFT  JOIN CTG1 T5 ON T4.GroupNum = T5.CTGCode
				INNER JOIN OCRD T6 ON T1.CardCode = T6.CardCode AND T6.CardType = 'S'
				LEFT  JOIN CRD7 T7 ON T6.CardCode = T7.CardCode ANd T7.[Address] = '' AND T7.AddrType = 'S'
			OUTER APPLY (
				SELECT Remind = CONVERT(DATETIME, CAST(YEAR(T0.StartDate) AS NVARCHAR(10)) + '-' + CAST(MONTH(T0.StartDate) AS NVARCHAR(10)) + '-' + SUBSTRING(CAST(T0.Remind AS NVARCHAR(10)), 3, 2))
			) AS OA_REMIND
			WHERE T0.IsRemoved = 'N'
				AND T1.DocStatus = 'O'
				ANd T1.CANCELED = 'N'
				AND T2.LineStatus = 'O'
				AND T0.Frequency = 'A'
				AND (
					(
						(DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(YEAR, 1, OA_REMIND.Remind)) >= @DtIni AND @TpData = 'V')
						AND (DATEADD(DAY, ISNULL(T5.InstDays, 0), DATEADD(YEAR, 1, OA_REMIND.Remind)) <= @DtFin AND @TpData = 'V')
					)
					OR
					(
						(DATEADD(YEAR, 1, OA_REMIND.Remind) >= @DtIni AND @TpData = 'LC')
						AND (DATEADD(YEAR, 1, OA_REMIND.Remind) >= @DtFin AND @TpData = 'LC')
					)
				)

		INSERT INTO @rtnTable
			SELECT DISTINCT 
				T6.CardCode,
				T6.CardName,
				OA_REMIND.Remind,
				DATEADD(DAY, ISNULL(T5.InstDays, 0), OA_REMIND.Remind),
				-1,
				T0.AbsEntry,
				ISNULL(T5.IntsNo, 1),
				ISNULL(T4.InstNum, 1),
				-1,
				'Transação recorrente',
				0.00,
				(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
				(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
				DATEADD(DAY, ISNULL(T5.InstDays, 0), OA_REMIND.Remind),
				T1.PeyMethod
			FROM ORCP T0
				INNER JOIN ODRF T1 On T0.DraftEntry = T1.DocEntry AND T0.DocObjType = T1.ObjType
				INNER JOIN DRF1 T2 ON T1.DocEntry = T2.DocEntry
				INNER JOIN OACT T3 ON T2.AcctCode = T3.AcctCode
				INNER JOIN OCTG T4 ON T1.GroupNum = T4.GroupNum
				LEFT  JOIN CTG1 T5 ON T4.GroupNum = T5.CTGCode
				INNER JOIN OCRD T6 ON T1.CardCode = T6.CardCode AND T6.CardType = 'S'
				LEFT  JOIN CRD7 T7 ON T6.CardCode = T7.CardCode ANd T7.[Address] = '' AND T7.AddrType = 'S'
			OUTER APPLY (
				SELECT Remind = CONVERT(DATETIME, CAST(YEAR(T0.StartDate) AS NVARCHAR(10)) + '-' + CAST(MONTH(T0.StartDate) AS NVARCHAR(10)) + '-' + SUBSTRING(CAST(T0.Remind AS NVARCHAR(10)), 3, 2))
			) AS OA_REMIND
			WHERE T0.IsRemoved = 'N'
				AND T1.DocStatus = 'O'
				ANd T1.CANCELED = 'N'
				AND T2.LineStatus = 'O'
				AND T0.Frequency = 'O'
				AND (
					(
						(DATEADD(DAY, ISNULL(T5.InstDays, 0), OA_REMIND.Remind) >= @DtIni AND @TpData = 'V')
						AND (DATEADD(DAY, ISNULL(T5.InstDays, 0), OA_REMIND.Remind) <= @DtFin AND @TpData = 'V')
					)
					OR 
					(
						(DATEADD(DAY, ISNULL(T5.InstDays, 0), OA_REMIND.Remind) >= @DtIni AND @TpData = 'LC')
						AND (DATEADD(DAY, ISNULL(T5.InstDays, 0), OA_REMIND.Remind) <= @DtFin AND @TpData = 'LC')
					)
				)

		SET @dtCursor = CURSOR FAST_FORWARD FOR
			SELECT Data, Dia, Mes, Ano, Semana, Trimestre, Semestre FROM @tb_datas ORDER BY Data ASC

		OPEN @dtCursor
		FETCH NEXT FROM @dtCursor INTO @Data, @Dia, @Mes, @Ano, @Semana, @Trimestre, @Semestre

		WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT INTO @rtnTable
				SELECT DISTINCT 
					T6.CardCode,
					T6.CardName,
					OA_REMIND.Remind,
					DATEADD(DAY, ISNULL(T5.InstDays, 0), OA_REMIND.Remind),
					-1,
					T0.AbsEntry,
					ISNULL(T5.IntsNo, 1),
					ISNULL(T4.InstNum, 1),
					-1,
					'Transação recorrente',
					0.00,
					(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
					(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
					DATEADD(DAY, ISNULL(T5.InstDays, 0), OA_REMIND.Remind),
					T1.PeyMethod
				FROM ORCP T0
					INNER JOIN ODRF T1 On T0.DraftEntry = T1.DocEntry AND T0.DocObjType = T1.ObjType
					INNER JOIN DRF1 T2 ON T1.DocEntry = T2.DocEntry
					INNER JOIN OACT T3 ON T2.AcctCode = T3.AcctCode
					INNER JOIN OCTG T4 ON T1.GroupNum = T4.GroupNum
					LEFT  JOIN CTG1 T5 ON T4.GroupNum = T5.CTGCode
					INNER JOIN OCRD T6 ON T1.CardCode = T6.CardCode AND T6.CardType = 'S'
					LEFT  JOIN CRD7 T7 ON T6.CardCode = T7.CardCode ANd T7.[Address] = '' AND T7.AddrType = 'S'
				OUTER APPLY (
					SELECT Remind = DATEADD(DAY, CAST(SUBSTRING(CAST(T0.Remind AS NVARCHAR(10)), 2, 3) AS INT), @Data)
				) AS OA_REMIND
				WHERE T0.IsRemoved = 'N'
					AND T1.DocStatus = 'O'
					ANd T1.CANCELED = 'N'
					AND T2.LineStatus = 'O'
					AND T0.Frequency = 'D'
					AND (
						(
							(DATEADD(DAY, ISNULL(T5.InstDays, 0), OA_REMIND.Remind) >= @DtIni AND @TpData = 'V')
							AND (DATEADD(DAY, ISNULL(T5.InstDays, 0), OA_REMIND.Remind) <= @DtFin AND @TpData = 'V')
							AND DATEPART(DAY, DATEADD(DAY, ISNULL(T5.InstDays, 0), OA_REMIND.Remind))%CAST(T0.Remind AS NVARCHAR(10)) = 0
						)
						OR	(
							(OA_REMIND.Remind >= @DtIni AND @TpData = 'LC')
							AND (OA_REMIND.Remind <= @DtFin AND @TpData = 'LC')
							AND DATEPART(DAY, OA_REMIND.Remind)%CAST(T0.Remind AS NVARCHAR(10)) = 0
						)
					)

			INSERT INTO @rtnTable
				SELECT DISTINCT 
					T6.CardCode,
					T6.CardName,
					@Data,
					DATEADD(DAY, ISNULL(T5.InstDays, 0), @Data),
					-1,
					T0.AbsEntry,
					ISNULL(T5.IntsNo, 1),
					ISNULL(T4.InstNum, 1),
					-1,
					'Transação recorrente',
					0.00,
					(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
					(T2.LineTotal * ISNULL(T5.InstPrcnt, 100))/100,
					DATEADD(DAY, ISNULL(T5.InstDays, 0), @Data),
					T1.PeyMethod
				FROM ORCP T0
					INNER JOIN ODRF T1 On T0.DraftEntry = T1.DocEntry AND T0.DocObjType = T1.ObjType
					INNER JOIN DRF1 T2 ON T1.DocEntry = T2.DocEntry
					INNER JOIN OACT T3 ON T2.AcctCode = T3.AcctCode
					INNER JOIN OCTG T4 ON T1.GroupNum = T4.GroupNum
					LEFT  JOIN CTG1 T5 ON T4.GroupNum = T5.CTGCode
					INNER JOIN OCRD T6 ON T1.CardCode = T6.CardCode AND T6.CardType = 'S'
					LEFT  JOIN CRD7 T7 ON T6.CardCode = T7.CardCode ANd T7.[Address] = '' AND T7.AddrType = 'S'
				WHERE T0.IsRemoved = 'N'
					AND T1.DocStatus = 'O'
					ANd T1.CANCELED = 'N'
					AND T2.LineStatus = 'O'
					AND T0.Frequency = 'W'
					AND (
						(
							(DATEADD(DAY, ISNULL(T5.InstDays, 0), @Data) >= @DtIni AND @TpData = 'V')
							AND (DATEADD(DAY, ISNULL(T5.InstDays, 0), @Data) <= @DtFin AND @TpData = 'V')
							AND DATEPART(WEEKDAY, DATEADD(DAY, ISNULL(T5.InstDays, 0), @Data)) = T0.Remind
						)
						OR	(
							(@Data >= @DtIni AND @TpData = 'LC')
							AND (@Data <= @DtFin AND @TpData = 'LC')
							AND DATEPART(WEEKDAY, @Data) = T0.Remind
						)
					)

			FETCH NEXT FROM @dtCursor INTO @Data, @Dia, @Mes, @Ano, @Semana, @Trimestre, @Semestre
		END

		CLOSE @dtCursor
		DEALLOCATE @dtCursor
	END

	BEGIN -- LANÇAMENTOS PERIÓDICOS
		SET @dtCursor = CURSOR FAST_FORWARD FOR
			SELECT Data, Dia, Mes, Ano, Semana, Trimestre, Semestre FROM @tb_datas ORDER BY Data ASC

		OPEN @dtCursor
		FETCH NEXT FROM @dtCursor INTO @Data, @Dia, @Mes, @Ano, @Semana, @Trimestre, @Semestre

		WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT INTO @rtnTable
				SELECT DISTINCT
					ISNULL(T3.CardCode, T2.AcctCode),
					ISNULL(T3.CardName, T2.AcctName),
					DATEADD(DAY, OA_REMIND.Remind, @Data),
					DATEADD(DAY, OA_REMIND.Remind, @Data),
					-1,
					T0.RcurCode,
					1,
					1,
					-1,
					CAST(T0.Memo AS NVARCHAR(MAX)),
					T1.Debit,
					T1.Credit,
					T1.Debit - T1.Credit,
					DATEADD(DAY, OA_REMIND.Remind, @Data),
					'N/A'
				FROM ORCR T0
					INNER JOIN RCR1 T1 ON T0.RcurCode = T1.RcurCode AND T1.Debit > 0
					LEFT  JOIN OACT T2 ON T1.AcctCode = T2.AcctCode
					LEFT  JOIN OCRD T3 ON T1.AcctCode = T3.CardCode AND T3.CardType = 'S'
					LEFT  JOIN CRD7 T4 ON T3.CardCode = T4.CardCode AND T4.[Address] = '' AND T4.AddrType = 'S'
					LEFT  JOIN OSLP T8 ON T3.SlpCode = T8.SlpCode
				OUTER APPLY (
					SELECT Remind = CAST(T0.Remind AS INT)
				) AS OA_REMIND
				WHERE T0.Frequency = 'D' 
					AND DATEADD(DAY, OA_REMIND.Remind, @Data) > T0.NextDeu
					AND DATEADD(DAY, OA_REMIND.Remind, @Data) <= ISNULL(T0.LimitDate, @DtFin)
					AND DATEPART(DAY, DATEADD(DAY, OA_REMIND.Remind, @Data))%OA_REMIND.Remind = 0

			INSERT INTO @rtnTable
				SELECT DISTINCT
					ISNULL(T3.CardCode, T2.AcctCode),
					ISNULL(T3.CardName, T2.AcctName),
					@Data,
					@Data,
					-1,
					T0.RcurCode,
					1,
					1,
					-1,
					CAST(T0.Memo AS NVARCHAR(MAX)),
					T1.Debit,
					T1.Credit,
					T1.Debit - T1.Credit,
					@Data,
					'N/A'
				FROM ORCR T0
					INNER JOIN RCR1 T1 ON T0.RcurCode = T1.RcurCode AND T1.Debit > 0
					LEFT  JOIN OACT T2 ON T1.AcctCode = T2.AcctCode
					LEFT  JOIN OCRD T3 ON T1.AcctCode = T3.CardCode AND T3.CardType = 'S'
					LEFT  JOIN CRD7 T4 ON T3.CardCode = T4.CardCode AND T4.[Address] = '' AND T4.AddrType = 'S'
					LEFT  JOIN OSLP T8 ON T3.SlpCode = T8.SlpCode
				OUTER APPLY (
					SELECT Remind = CAST(T0.Remind AS INT)
				) AS OA_REMIND
				WHERE T0.Frequency = 'W' 
					AND @Data > T0.NextDeu
					AND @Data <= ISNULL(T0.LimitDate, @DtFin)
					AND DATEPART(WEEKDAY, @Data) = OA_REMIND.Remind

			INSERT INTO @rtnTable
				SELECT DISTINCT
					ISNULL(T3.CardCode, T2.AcctCode),
					ISNULL(T3.CardName, T2.AcctName),
					DATEADD(MONTH, @Mes, CONVERT(DATETIME, CAST(YEAR(T0.NextDeu) AS NVARCHAR(10)) + '-' + CAST(MONTH(T0.NextDeu) AS NVARCHAR(10)) + '-' + CAST(OA_REMIND.Remind AS NVARCHAR(10)))),
					DATEADD(MONTH, @Mes, CONVERT(DATETIME, CAST(YEAR(T0.NextDeu) AS NVARCHAR(10)) + '-' + CAST(MONTH(T0.NextDeu) AS NVARCHAR(10)) + '-' + CAST(OA_REMIND.Remind AS NVARCHAR(10)))),
					-1,
					T0.RcurCode,
					1,
					1,
					-1,
					CAST(T0.Memo AS NVARCHAR(MAX)),
					T1.Debit,
					T1.Credit,
					T1.Debit - T1.Credit,
					DATEADD(MONTH, @Mes, CONVERT(DATETIME, CAST(YEAR(T0.NextDeu) AS NVARCHAR(10)) + '-' + CAST(MONTH(T0.NextDeu) AS NVARCHAR(10)) + '-' + CAST(OA_REMIND.Remind AS NVARCHAR(10)))),
					'N/A'
				FROM ORCR T0
					INNER JOIN RCR1 T1 ON T0.RcurCode = T1.RcurCode AND T1.Debit > 0
					LEFT  JOIN OACT T2 ON T1.AcctCode = T2.AcctCode
					LEFT  JOIN OCRD T3 ON T1.AcctCode = T3.CardCode AND T3.CardType = 'S'
					LEFT  JOIN CRD7 T4 ON T3.CardCode = T4.CardCode AND T4.[Address] = '' AND T4.AddrType = 'S'
					LEFT  JOIN OSLP T8 ON T3.SlpCode = T8.SlpCode
				OUTER APPLY (
					SELECT Remind = CASE WHEN CAST(T0.Remind AS INT) > DAY(DATEADD(s, -1, DATEADD(mm, DATEDIFF(m, 0, T0.NextDeu)+1, 0))) THEN CAST(T0.Remind AS INT)-1 ELSE CAST(T0.Remind AS INT) END
				) AS OA_REMIND
				WHERE T0.Frequency = 'M' 
					AND DATEADD(MONTH, @Mes, CONVERT(DATETIME, CAST(YEAR(T0.NextDeu) AS NVARCHAR(10)) + '-' + CAST(MONTH(T0.NextDeu) AS NVARCHAR(10)) + '-' + CAST(OA_REMIND.Remind AS NVARCHAR(10)))) > T0.NextDeu
					AND DATEADD(MONTH, @Mes, CONVERT(DATETIME, CAST(YEAR(T0.NextDeu) AS NVARCHAR(10)) + '-' + CAST(MONTH(T0.NextDeu) AS NVARCHAR(10)) + '-' + CAST(OA_REMIND.Remind AS NVARCHAR(10)))) <= ISNULL(T0.LimitDate, @DtFin)
					
			FETCH NEXT FROM @dtCursor INTO @Data, @Dia, @Mes, @Ano, @Semana, @Trimestre, @Semestre
		END

		CLOSE @dtCursor
		DEALLOCATE @dtCursor

		SET @QuarterAux = 1
		SET @QuarterDiff = 12

		WHILE @QuarterAux <= @QuarterDiff
		BEGIN
			INSERT INTO @rtnTable
				SELECT DISTINCT
					ISNULL(T3.CardCode, T2.AcctCode),
					ISNULL(T3.CardName, T2.AcctName),
					DATEADD(QUARTER, @QuarterAux, T0.NextDeu),
					DATEADD(QUARTER, @QuarterAux, T0.NextDeu),
					-1,
					T0.RcurCode,
					1,
					1,
					-1,
					CAST(T0.Memo AS NVARCHAR(MAX)),
					T1.Debit,
					T1.Credit,
					T1.Debit - T1.Credit,
					DATEADD(QUARTER, @QuarterAux, T0.NextDeu),
					'N/A'
				FROM ORCR T0
					INNER JOIN RCR1 T1 ON T0.RcurCode = T1.RcurCode AND T1.Debit > 0
					LEFT  JOIN OACT T2 ON T1.AcctCode = T2.AcctCode
					LEFT  JOIN OCRD T3 ON T1.AcctCode = T3.CardCode AND T3.CardType = 'S'
					LEFT  JOIN CRD7 T4 ON T3.CardCode = T4.CardCode AND T4.[Address] = '' AND T4.AddrType = 'S'
					LEFT  JOIN OSLP T8 ON T3.SlpCode = T8.SlpCode
				WHERE T0.Frequency = 'Q' 
					AND DATEADD(QUARTER, @QuarterAux, T0.NextDeu) > T0.NextDeu
					AND DATEADD(QUARTER, @QuarterAux, T0.NextDeu) <= ISNULL(T0.LimitDate, @DtFin)
					
			SET @QuarterAux = @QuarterAux + 1
		END

		INSERT INTO @rtnTable
			SELECT DISTINCT
				ISNULL(T3.CardCode, T2.AcctCode),
				ISNULL(T3.CardName, T2.AcctName),
				DATEADD(MONTH, 6, T0.NextDeu),
				DATEADD(MONTH, 6, T0.NextDeu),
				-1,
				T0.RcurCode,
				1,
				1,
				-1,
				CAST(T0.Memo AS NVARCHAR(MAX)),
				T1.Debit,
				T1.Credit,
				T1.Debit - T1.Credit,
				DATEADD(MONTH, 6, T0.NextDeu),
				'N/A'
			FROM ORCR T0
				INNER JOIN RCR1 T1 ON T0.RcurCode = T1.RcurCode AND T1.Debit > 0
				LEFT  JOIN OACT T2 ON T1.AcctCode = T2.AcctCode
				LEFT  JOIN OCRD T3 ON T1.AcctCode = T3.CardCode AND T3.CardType = 'S'
				LEFT  JOIN CRD7 T4 ON T3.CardCode = T4.CardCode AND T4.[Address] = '' AND T4.AddrType = 'S'
				LEFT  JOIN OSLP T8 ON T3.SlpCode = T8.SlpCode
			WHERE T0.Frequency = 'S' 
				AND DATEADD(MONTH, 6, T0.NextDeu) > T0.NextDeu
				AND DATEADD(MONTH, 6, T0.NextDeu) <= ISNULL(T0.LimitDate, @DtFin)

		INSERT INTO @rtnTable
			SELECT DISTINCT
				ISNULL(T3.CardCode, T2.AcctCode),
				ISNULL(T3.CardName, T2.AcctName),
				DATEADD(YEAR, 1, T0.NextDeu),
				DATEADD(YEAR, 1, T0.NextDeu),
				-1,
				T0.RcurCode,
				1,
				1,
				-1,
				CAST(T0.Memo AS NVARCHAR(MAX)),
				T1.Debit,
				T1.Credit,
				T1.Debit - T1.Credit,
				DATEADD(YEAR, 1, T0.NextDeu),
				'N/A'
			FROM ORCR T0
				INNER JOIN RCR1 T1 ON T0.RcurCode = T1.RcurCode AND T1.Debit > 0
				LEFT  JOIN OACT T2 ON T1.AcctCode = T2.AcctCode
				LEFT  JOIN OCRD T3 ON T1.AcctCode = T3.CardCode AND T3.CardType = 'S'
				LEFT  JOIN CRD7 T4 ON T3.CardCode = T4.CardCode AND T4.[Address] = '' AND T4.AddrType = 'S'
				LEFT  JOIN OSLP T8 ON T3.SlpCode = T8.SlpCode
			WHERE T0.Frequency = 'A' 
				AND DATEADD(YEAR, 1, T0.NextDeu) > T0.NextDeu
				AND DATEADD(YEAR, 1, T0.NextDeu) <= ISNULL(T0.LimitDate, @DtFin)

		INSERT INTO @rtnTable
			SELECT DISTINCT
				ISNULL(T3.CardCode, T2.AcctCode),
				ISNULL(T3.CardName, T2.AcctName),
				T0.NextDeu,
				T0.NextDeu,
				-1,
				T0.RcurCode,
				1,
				1,
				-1,
				CAST(T0.Memo AS NVARCHAR(MAX)),
				T1.Debit,
				T1.Credit,
				T1.Debit - T1.Credit,
				T0.NextDeu,
				'N/A'
			FROM ORCR T0
				INNER JOIN RCR1 T1 ON T0.RcurCode = T1.RcurCode AND T1.Debit > 0
				LEFT  JOIN OACT T2 ON T1.AcctCode = T2.AcctCode
				LEFT  JOIN OCRD T3 ON T1.AcctCode = T3.CardCode AND T3.CardType = 'S'
				LEFT  JOIN CRD7 T4 ON T3.CardCode = T4.CardCode AND T4.[Address] = '' AND T4.AddrType = 'S'
				LEFT  JOIN OSLP T8 ON T3.SlpCode = T8.SlpCode
			WHERE T0.Frequency = 'O' 
				AND T0.NextDeu >= @DtIni
				AND T0.NextDeu <= ISNULL(T0.LimitDate, @DtFin)
	END
	
	RETURN
END