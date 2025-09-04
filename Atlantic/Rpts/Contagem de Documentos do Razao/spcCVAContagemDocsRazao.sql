USE SBO_PRD_ATL0001
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcCVAContagemDocsRazao]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcCVAContagemDocsRazao]
GO

CREATE PROC [dbo].[spcCVAContagemDocsRazao] (
	@dateini DATETIME,
	@datefim DATETIME                          ,
	@pdatabase_name nvarchar(50)
)
with encryption
as
Begin
	declare @database_name nvarchar(50)
	declare @QUERY as nvarchar(max)	

	set @QUERY=''

	DECLARE database_cursor CURSOR FOR   
	SELECT 
		distinct
		[BASE]
	FROM 
		[CVA_ATL_CON].[dbo].[CVA_BASES]
	order by 1

	OPEN database_cursor  

	FETCH NEXT FROM database_cursor   
	INTO @database_name

	WHILE @@FETCH_STATUS = 0  
	BEGIN 
			if len(@QUERY)>0 
			begin
				set @QUERY=@QUERY+' Union all '
			end

			set @QUERY=@QUERY + N'
			select 
				count(*) Contador,''Lançamento Contábil Manual'' TransType ,''' + @database_name + '''  as Base
			from 
				[' + @database_name + '].[dbo].OJDT T0 
			where 
				Cast(T0.RefDate as DAte) Between cast(@dateini as Date) and cast(@datefim as date) and TransType=30
			union all
			select 
				count (*) ,''Adiantamento para Fornecedor'',''' + @database_name + '''  as Base
			from 
				[' + @database_name + '].[dbo].ODPO T0 
			where Cast(T0.DocDate as Date) Between cast(@dateini as date) and cast(@datefim as date)
			union all
			select 
				count (*) ,''Adiantamento para Cliente'',''' + @database_name + '''  as Base
			from 
				[' + @database_name + '].[dbo].ODPI T0 
			where Cast(T0.DocDate as Date) Between cast(@dateini as date) and cast(@datefim as date)
			union all
			select 
				count (*) ,''Contas à Pagar'',''' + @database_name + '''  as Base
			from 
				[' + @database_name + '].[dbo].OVPM T0 
			where Cast(T0.DocDate as Date) Between cast(@dateini as date) and cast(@datefim as date)
			union all
			select 
				count (*) ,''Contas à Receber'',''' + @database_name + '''  as Base
			from 
				[' + @database_name + '].[dbo].ORCT T0 
			where Cast(T0.DocDate as Date) Between cast(@dateini as date) and cast(@datefim as date)
			union all
			select 
				count (*) ,''Devolução de Nota Fiscal de Entrada'',''' + @database_name + '''  as Base
			from 
				[' + @database_name + '].[dbo].ORPC T0 
			where Cast(T0.DocDate as Date) Between cast(@dateini as date) and cast(@datefim as date)
			union all
			select 
				count (*) ,''Devolução de Nota Fiscal de Saída'',''' + @database_name + '''  as Base
			from 
				[' + @database_name + '].[dbo].ORIN T0 
			where Cast(T0.DocDate as Date) Between cast(@dateini as date) and cast(@datefim as date)
			union all
			select 
				count (*) ,''Nota Fiscal de Entrada'',''' + @database_name + '''  as Base
			from 
				[' + @database_name + '].[dbo].OPCH T0 
			where Cast(T0.DocDate as Date) Between cast(@dateini as date) and cast(@datefim as date)
			union all
			select 
				count (*) ,''Nota Fiscal de Saída'',''' + @database_name + '''  as Base
			from 
				[' + @database_name + '].[dbo].OINV T0 
			where Cast(T0.DocDate as Date) Between cast(@dateini as date) and cast(@datefim as date)
			union all
			select 
				count (*) ,''Entrega de Saída'',''' + @database_name + '''  as Base
			from 
				[' + @database_name + '].[dbo].ODLN T0 
			where Cast(T0.DocDate as Date) Between cast(@dateini as date) and cast(@datefim as date)
			union all
			select 
				count (*) ,''Recebimento de Mercadoria (Compras)'',''' + @database_name + '''  as Base
			from 
				[' + @database_name + '].[dbo].OPDN T0 
			where Cast(T0.DocDate as Date) Between cast(@dateini as date) and cast(@datefim as date)
			union all
			select 
				count (*) ,''Devolução de Mercadoria (Compras)'',''' + @database_name + '''  as Base
			from 
				[' + @database_name + '].[dbo].ORPD T0 
			where Cast(T0.DocDate as Date) Between cast(@dateini as date) and cast(@datefim as date)
			union all
			select 
				count (*) ,''Devolução de Venda'',''' + @database_name + '''  as Base
			from 
				[' + @database_name + '].[dbo].ORDN T0 
			where Cast(T0.DocDate as Date) Between cast(@dateini as date) and cast(@datefim as date)
			union all
			select count(*),''Boleto A Pagar'',''' + @database_name + '''  as Base from OBOE T0 where  Cast(T0.DueDate as Date) Between cast(@dateini as date) and cast(@datefim as date)
			'

		
			--N'@CardCode NVARCHAR(150), @DataInicial DATETIME, @DataFinal DATETIME ,@cenario int ', @CardCode , @DataInicial ,@DataFinal ,@cenario	
			--@dateini DATETIME,@datefim DATETIME
			--print @database_name
		FETCH NEXT FROM database_cursor   
		INTO @database_name
	END   
	CLOSE database_cursor;  
	DEALLOCATE database_cursor;

	set @QUERY='select sum(Contador) Total,Transtype ''Tipo de Documento'' from (' + @QUERY+ ' ) as TB where Base='''+@pdatabase_name+''' or '''+@pdatabase_name+''' = ''*'' group by Transtype order by 1 desc'
	print @QUERY
	--@QUERY
	--print '123'
	EXEC sp_executesql @QUERY, N'@dateini DATETIME,@datefim DATETIME ', @dateini , @datefim
end

GO

execute spcCVAContagemDocsRazao '2017-01-01','2017-12-31','*'--'SBO_PRD_MOR0001'

SELECT  distinct [BASE],[BASE] FROM [CVA_ATL_CON].[dbo].[CVA_BASES] union all select '*','Todos' order by 1

	--SELECT 
	--	distinct
	--	*
	--FROM 
	--	[CVA_ATL_CON].[dbo].[CVA_BASES]
	--order by 1
		--SELECT @DSCR = DSCR FROM [CVA_ATL_PORTAL].[dbo].[CVA_SCENARIO] WHERE ID = @cenario


--select distinct
--	T1.BASE
--	,T0.DSCR
--from 
--	[CVA_ATL_PORTAL].[dbo].[CVA_SCENARIO] T0
--	inner join [CVA_ATL_PORTAL].[dbo].[CVA_SCENARIO_ITEM] T1 on T1.SCN_ID=T0.ID

		--select * from [CVA_ATL_PORTAL].[dbo].[CVA_SCENARIO_ITEM]

--select * from [CVA_ATL_CON].[dbo].[CVA_SCENARIO_ITEM]
--select * from OBOE

--select * from [CVA_ATL_CON].[dbo].[CVA_BASES]