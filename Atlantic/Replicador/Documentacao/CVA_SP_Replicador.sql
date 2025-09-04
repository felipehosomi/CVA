ALTER PROCEDURE CVA_SP_Replicador(@object_type NVARCHAR(20), @transaction_type NCHAR(1), @list_of_cols_val_tab_del NVARCHAR(255))
AS
BEGIN
	DECLARE 
		@query	NVARCHAR(MAX), 
		@error	INT, 
		@data	DATETIME, 
		@dbname	NVARCHAR(MAX), 
		@db		INT,
		@obj	INT,
		@func	INT

	SET @query = 'INSERT INTO [CVA_ATL_REP].[dbo].[CVA_REG](INS, STU, BAS, CODE, OBJ, FUNC) VALUES(''@data'', 3, @db, ''@code'', @obj, @func)'
	SET @error = 0

	SELECT @data = GETDATE()
	SELECT @dbname = DB_NAME()	
	SELECT @db = ISNULL(T0.ID, 0) FROM [CVA_ATL_REP].[dbo].[CVA_BAS] T0 WHERE T0.COMP = @dbname
	SELECT @func = ISNULL(T0.ID, 0) FROM [CVA_ATL_REP].[dbo].[CVA_FUNC] T0 WHERE T0.FUNC = @transaction_type

	IF @db = 0 RETURN
	IF @func = 0 RETURN

	SELECT @obj =
		CASE @object_type
			WHEN N'1'	THEN 8	-- CVA_Obj_PlanoContas
			WHEN N'2'	THEN 11	-- CVA_Obj_ParceiroNegocio
			WHEN N'3'	THEN 10	-- CVA_Obj_Item
			WHEN N'10'	THEN 1	-- CVA_Obj_GrupoParceiroNegocio
			WHEN N'12'	THEN 3	-- CVA_Obj_Usuario
			WHEN N'40'	THEN 4	-- CVA_Obj_FormaPagamento
			WHEN N'52'	THEN 2	-- CVA_Obj_GrupoItem
			WHEN N'61'	THEN 5	-- CVA_Obj_CentroCusto
			WHEN N'128'	THEN 12	-- CVA_Obj_Imposto
			WHEN N'138'	THEN 6	-- CVA_Obj_Indicador
			WHEN N'260'	THEN 7	-- CVA_Obj_Utilizacao
		END

	IF NOT EXISTS (
		SELECT 1
		FROM [CVA_ATL_REP].[dbo].[CVA_REG] T0
		WHERE T0.CODE = @list_of_cols_val_tab_del AND T0.STU = 5 AND T0.OBJ = @obj AND T0.BAS = @db
	)
	BEGIN
		IF @obj IN (6, 12) AND @transaction_type = 'D'
			SET @error = 1
		ELSE
			SELECT @query = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@query, '@code', @list_of_cols_val_tab_del), '@obj', @obj), '@func', @func), '@data', @data), '@db', @db)
	END
	ELSE
	BEGIN
		SET @error = 1
		UPDATE [CVA_ATL_REP].[dbo].[CVA_REG] SET STU = 3 WHERE STU = 5 AND CODE = @list_of_cols_val_tab_del AND OBJ = @obj AND BAS = @db
		UPDATE [CVA_ATL_REP].[dbo].[CVA_TIM] SET STU = 2 WHERE STU = 5
	END

	IF @error = 0
		EXEC (@query)
END
