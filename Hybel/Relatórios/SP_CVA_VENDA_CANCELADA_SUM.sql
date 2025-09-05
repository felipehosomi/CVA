IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_VENDA_CANCELADA_SUM')
	DROP PROCEDURE SP_CVA_VENDA_CANCELADA_SUM
GO
CREATE PROCEDURE SP_CVA_VENDA_CANCELADA_SUM
(
	@Motivo		NVARCHAR(100),
	@DataDe		DATETIME,
	@DataAte	DATETIME,
	@Filial		INT
)
AS
BEGIN
	DECLARE @columns NVARCHAR(MAX), @columnsIsNull NVARCHAR(MAX), @sql NVARCHAR(MAX);
	SET @columns = N''
	SET @columnsIsNull = N''

	SELECT @columns += N', p.' + QUOTENAME(Name)  FROM [@CVA_MOTIVO_CANC] 
	SELECT @columnsIsNull += N', ISNULL(p.' + QUOTENAME(Name) + ', 0)' FROM [@CVA_MOTIVO_CANC] 

	SET @sql = N'
	SELECT BPLName, ' + STUFF(@columns, 1, 2, '') + ', Total = ' + STUFF(REPLACE(@columnsIsNull, ', ISNULL(p.', ' + ISNULL(p.'), 1, 3, '') + '
	FROM
	(
		SELECT OBPL.BPLName, MOT.Name, ORDR.DocTotal
		FROM [@CVA_MOTIVO_CANC] MOT
			INNER JOIN ORDR
				ON ORDR.U_CVA_Motivo_Canc = MOT.Code
			INNER JOIN OBPL
				ON OBPL.BPLId = ORDR.BPLId
		WHERE ORDR.Canceled = ''Y''

		UNION ALL

		SELECT OBPL.BPLName, MOT.Name, OQUT.DocTotal
		FROM [@CVA_MOTIVO_CANC] MOT
			INNER JOIN OQUT
				ON OQUT.U_CVA_Motivo_Canc = MOT.Code
			INNER JOIN OBPL
				ON OBPL.BPLId = OQUT.BPLId
		WHERE OQUT.Canceled = ''Y''
	) AS j
	PIVOT
	(
	  SUM(DocTotal) FOR Name IN ('
	  + STUFF(REPLACE(@columns, ', p.[', ',['), 1, 1, '') + ')
	) AS p'

	PRINT @sql

	EXEC sp_executesql @sql
	
END