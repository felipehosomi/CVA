DECLARE @Base NVARCHAR(MAX)

DECLARE cursor1 CURSOR FOR
SELECT BASE FROM CVA_BASES

--Abrindo Cursor
OPEN cursor1
 
-- Lendo a próxima linha
FETCH NEXT FROM cursor1 INTO @Base
 
-- Percorrendo linhas do cursor (enquanto houverem)
WHILE @@FETCH_STATUS = 0
BEGIN
 
 SELECT @Base
 BEGIN TRY  
	EXEC('select * from [' + @Base + ']..[@CVA_IMP_PED1] where U_NUMSAP = 422')
 END TRY
 BEGIN CATCH  
     print 'Tabela não existe: ' + @Base
END CATCH  
-- Lendo a próxima linha
FETCH NEXT FROM cursor1 INTO @Base
END
 
-- Fechando Cursor para leitura
CLOSE cursor1
 
-- Finalizado o cursor
DEALLOCATE cursor1