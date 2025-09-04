if object_id('dbo.spc_CVA_Comissoes', 'P') is not null
	drop procedure [spc_CVA_Comissoes]
go
create procedure dbo.spc_CVA_Comissoes(@Status char(1) = 'T', @View char(1) = 'T', @DataInicial datetime, @DataFinal datetime)
as
begin
	set dateformat 'ymd';
	set nocount on;

	begin --CRIAÇÃO DE TABELAS TEMPORÁRIAS
		create table #tmp_table_fat
		(
			RegrCode int,
			Comissionado int,
			Comissao numeric(19,6),
			Prioridade int,
			DocEntry int,
			DocDate datetime,
			Serial int,
			DocTotal numeric(19,6),
			ObjType nvarchar(40),
			ItemCode nvarchar(100),
			LineNum int,
			Quantity numeric(19,6),
			LineTotal numeric(19,6),
			InstlmntID int,
			InsTotal numeric(19,6),
			DueDate datetime,
			ItemName nvarchar(200),
			ItmsGrpCod smallint,
			ItmsGrpNam nvarchar(40),
			CardCode nvarchar(30),
			CardName nvarchar(200),
			PrcCode nvarchar(16),
			PrcName nvarchar(60),
			SlpCode int,
			SlpName nvarchar(310),
			FirmCode smallint,
			FirmName nvarchar(60),
			IndCode int,
			IndName nvarchar(30),
			IndDesc nvarchar(60),
			AbsId int,
			Code nvarchar(14),
			Country nvarchar(6),
			Name nvarchar(200),
			[State] nvarchar(6),
			TaxDate datetime,
			SumApplied numeric(19,6),
			TaxSum numeric(19,6)
		)

		create table #tmp_table_ped
		(
			RegrCode int,
			Comissionado int,
			Comissao numeric(19,6),
			Prioridade int,
			DocEntry int,
			DocDate datetime,
			Serial int,
			DocTotal numeric(19,6),
			ObjType nvarchar(40),
			ItemCode nvarchar(100),
			LineNum int,
			Quantity numeric(19,6),
			LineTotal numeric(19,6),
			InstlmntID int,
			InsTotal numeric(19,6),
			DueDate datetime,
			ItemName nvarchar(200),
			ItmsGrpCod smallint,
			ItmsGrpNam nvarchar(40),
			CardCode nvarchar(30),
			CardName nvarchar(200),
			PrcCode nvarchar(16),
			PrcName nvarchar(60),
			SlpCode int,
			SlpName nvarchar(310),
			FirmCode smallint,
			FirmName nvarchar(60),
			IndCode int,
			IndName nvarchar(30),
			IndDesc nvarchar(60),
			AbsId int,
			Code nvarchar(14),
			Country nvarchar(6),
			Name nvarchar(200),
			[State] nvarchar(6),
			TaxDate datetime,
			SumApplied numeric(19,6),
			TaxSum numeric(19,6)
		)

		create table #tmp_table_rec
		(
			RegrCode int,
			Comissionado int,
			Comissao numeric(19,6),
			Prioridade int,
			DocEntry int,
			DocDate datetime,
			Serial int,
			DocTotal numeric(19,6),
			ObjType nvarchar(40),
			ItemCode nvarchar(100),
			LineNum int,
			Quantity numeric(19,6),
			LineTotal numeric(19,6),
			InstlmntID int,
			InsTotal numeric(19,6),
			DueDate datetime,
			ItemName nvarchar(200),
			ItmsGrpCod smallint,
			ItmsGrpNam nvarchar(40),
			CardCode nvarchar(30),
			CardName nvarchar(200),
			PrcCode nvarchar(16),
			PrcName nvarchar(60),
			SlpCode int,
			SlpName nvarchar(310),
			FirmCode smallint,
			FirmName nvarchar(60),
			IndCode int,
			IndName nvarchar(30),
			IndDesc nvarchar(60),
			AbsId int,
			Code nvarchar(14),
			Country nvarchar(6),
			Name nvarchar(200),
			[State] nvarchar(6),
			TaxDate datetime,
			SumApplied numeric(19,6),
			TaxSum numeric(19,6)
		)

		create table #tmp_table_ret
		(
			U_COMISSIONADO int,
			U_PRIORIDADE int,
			U_CARDCODE nvarchar(50),
			U_CARDNAME nvarchar(200),
			U_REGRA int,
			U_DOCDATE datetime,
			U_DUEDATE datetime,
			U_DOCENTRY int,
			U_OBJTYPE int,
			U_ITEMCODE nvarchar(50),
			U_ITEMNAME nvarchar(200),
			U_LINENUM int,
			U_VALOR numeric(19,6),
			U_PARCELA int,
			U_IMPOSTOS numeric(19,6),
			U_COMISSAO numeric(19,6),
			U_VALORCOMISSAO numeric(19,6),
			U_CENTROCUSTO nvarchar(100),
			U_PAGO char(1),
			U_DATAPAGAMENTO datetime
		)

		create table #tmp_table_where
		(
			RegrCode int,
			Comissionado int,
			Comissao numeric(19,6),
			Momento char(1),
			Prioridade int,
			SqlWhere nvarchar(max)
		)
	end

	begin --DECLARAÇÃO DE VARIÁVEIS
		declare @cur as cursor

		declare @CritCode int,
				@CritName nvarchar(200),
				@CritPos int,
				@CritAtivo char(1),
				@RegrCode int,
				@RegrName nvarchar(200),
				@Tipo int,
				@Comissionado int,
				@Momento char(1),
				@Vendedor int,
				@Item nvarchar(40),
				@Grupo int,
				@CentroCusto nvarchar(16),
				@Fabricante int,
				@Cliente nvarchar(30),
				@Cidade int,
				@Estado nvarchar(6),
				@Setor int,
				@Comissao numeric(19,6),
				@Prioridade int,
				@Query nvarchar(max),
				@Params nvarchar(max),
				@Where nvarchar(max)
	end

	begin --CURSOR PARA BUSCAR CONDIÇÕES DE CRITÉRIOS WHERE
		set @cur = cursor fast_forward for
			select 
				T0.Code as CritCode, T0.Name as CritName, T0.U_POS AS CritPos, T0.U_ATIVO AS CritAtivo,
				T1.Code as RegrCode, T1.Name as RegrName, T1.U_TIPO, T1.U_COMISSIONADO, T1.U_MOMENTO, 
				T1.U_VENDEDOR, T1.U_ITEM, T1.U_GRUPO, T1.U_CENTROCUSTO, T1.U_FABRICANTE, T1.U_CLIENTE, 
				T1.U_CIDADE, T1.U_ESTADO, T1.U_SETOR, T1.U_COMISSAO
			from [@CVA_CRIT_COMISSAO] T0 
			inner join [@CVA_REGR_COMISSAO] T1 on T0.Code = T1.U_PRIORIDADE
			where T0.U_ATIVO = 'Y'
			order by T1.U_TIPO, T1.U_COMISSIONADO, T0.U_POS		

		open @cur
		fetch next from @cur into @CritCode, @CritName, @CritPos, @CritAtivo, @RegrCode, @RegrName, @Tipo, @Comissionado, @Momento, @Vendedor, @Item, @Grupo, @CentroCusto, @Fabricante, @Cliente, @Cidade, @Estado, @Setor, @Comissao

		set @Where = ''

		while @@fetch_status = 0
		begin
			if @Momento = 'F'
			begin
				if isnull(@Vendedor, isnull(@Comissionado, '')) <> '' set @Where = @Where + ' AND T7.SlpCode = ' + cast(isnull(@Vendedor, @Comissionado) as nvarchar(10))
				if isnull(@Item, '') <> '' set @Where = @Where + ' AND T3.ItemCode = ''' + @Item + ''''
				if isnull(@Grupo, '') <> '' set @Where = @Where + ' AND T4.ItmsGrpCod = ' + cast(@Grupo as nvarchar(10))
				if isnull(@CentroCusto, '') <> '' set @Where = @Where + ' AND T6.PrcCode = ''' + @CentroCusto + ''''
				if isnull(@Fabricante, '') <> '' set @Where = @Where + ' AND T8.FirmCode = ' + cast(@Fabricante as nvarchar(10))
				if isnull(@Cliente, '') <> '' set @Where = @Where + ' AND T5.CardCode = ''' + @Cliente + ''''
				if isnull(@Cidade, '') <> '' set @Where = @Where + ' AND T11.AbsId = ' + cast(@Cidade as nvarchar(10))
				if isnull(@Estado, '') <> '' set @Where = @Where + ' AND T11.[State] = ''' + @Estado + ''''
				if isnull(@Setor, '') <> '' set @Where = @Where + ' AND T9.IndCode = ''' + @Setor + ''''
			end

			if @Momento = 'P'
			begin
				if isnull(@Vendedor, isnull(@Comissionado, '')) <> '' set @Where = @Where + ' AND T7.SlpCode = ' + cast(isnull(@Vendedor, @Comissionado) as nvarchar(10))
				if isnull(@Item, '') <> '' set @Where = @Where + ' AND T3.ItemCode = ''' + @Item + ''''
				if isnull(@Grupo, '') <> '' set @Where = @Where + ' AND T4.ItmsGrpCod = ' + cast(@Grupo as nvarchar(10))
				if isnull(@CentroCusto, '') <> '' set @Where = @Where + ' AND T6.PrcCode = ''' + @CentroCusto + ''''
				if isnull(@Fabricante, '') <> '' set @Where = @Where + ' AND T8.FirmCode = ' + cast(@Fabricante as nvarchar(10))
				if isnull(@Cliente, '') <> '' set @Where = @Where + ' AND T5.CardCode = ''' + @Cliente + ''''
				if isnull(@Cidade, '') <> '' set @Where = @Where + ' AND T11.AbsId = ' + cast(@Cidade as nvarchar(10))
				if isnull(@Estado, '') <> '' set @Where = @Where + ' AND T11.[State] = ''' + @Estado + ''''
				if isnull(@Setor, '') <> '' set @Where = @Where + ' AND T9.IndCode = ''' + @Setor + ''''
			end

			if @Momento = 'R'
			begin
				if isnull(@Vendedor, isnull(@Comissionado, '')) <> '' set @Where = @Where + ' AND T14.SlpCode = ' + cast(isnull(@Vendedor, @Comissionado) as nvarchar(10))
				if isnull(@Item, '') <> '' set @Where = @Where + ' AND T11.ItemCode = ''' + @Item + ''''
				if isnull(@Grupo, '') <> '' set @Where = @Where + ' AND T12.ItmsGrpCod = ' + cast(@Grupo as nvarchar(10))
				if isnull(@CentroCusto, '') <> '' set @Where = @Where + ' AND T13.PrcCode = ''' + @CentroCusto + ''''
				if isnull(@Fabricante, '') <> '' set @Where = @Where + ' AND T15.FirmCode = ' + cast(@Fabricante as nvarchar(10))
				if isnull(@Cliente, '') <> '' set @Where = @Where + ' AND T1.CardCode = ''' + @Cliente + ''''
				if isnull(@Cidade, '') <> '' set @Where = @Where + ' AND T18.AbsId = ' + cast(@Cidade as nvarchar(10))
				if isnull(@Estado, '') <> '' set @Where = @Where + ' AND T18.[State] = ''' + @Estado + ''''
				if isnull(@Setor, '') <> '' set @Where = @Where + ' AND T6.IndCode = ''' + @Setor + ''''
			end

			insert into #tmp_table_where values(@RegrCode, @Comissionado, @Comissao, @Momento, @CritPos, @Where)

			set @Where = ''
			fetch next from @cur into @CritCode, @CritName, @CritPos, @CritAtivo, @RegrCode, @RegrName, @Tipo, @Comissionado, @Momento, @Vendedor, @Item, @Grupo, @CentroCusto, @Fabricante, @Cliente, @Cidade, @Estado, @Setor, @Comissao
		end

		close @cur
		deallocate @cur
	end

	begin --CURSOR PARA BUSCAR DOCUMENTOS
		set @cur = cursor fast_forward for
			select RegrCode, Comissionado, Comissao/100, Momento, Prioridade, SqlWhere from #tmp_table_where order by Momento, Comissionado

		open @cur
		fetch next from @cur into @RegrCode, @Comissionado, @Comissao, @Momento, @Prioridade, @Where

		while @@fetch_status = 0
		begin
			if @Momento = 'F'
			begin
				set @Query = N'
				select distinct @P1, @P2, @P3, @P4,
					T0.DocEntry,
					T0.DocDate,
					T0.Serial,
					T0.DocTotal,
					T0.ObjType,
					T1.ItemCode,
					T1.LineNum,
					T1.Quantity,
					T1.LineTotal,
					T2.InstlmntID,
					case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN T2.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt)-(OA_ADIC.TaxSum/OA_MAX.Countt)
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''N'' THEN T2.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt)
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN T2.InsTotal-(OA_ADIC.TaxSum/OA_MAX.Countt)
						else T2.InsTotal end end end,
					T2.DueDate,
					T3.ItemName,
					T4.ItmsGrpCod,
					T4.ItmsGrpNam,
					T5.CardCode,
					T5.CardName,
					T6.PrcCode,
					T6.PrcName,
					T7.SlpCode,
					T7.SlpName,
					T8.FirmCode,
					T8.FirmName,
					T9.IndCode,
					T9.IndName,
					T9.IndDesc,
					T11.AbsId,
					T11.Code,
					T11.Country,
					T11.Name,
					T11.[State],
					NULL AS TaxDate,
					NULL AS SumApplied,
					case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN (OA_INCL.TaxSum/OA_MAX.Countt)+(OA_ADIC.TaxSum/OA_MAX.Countt)
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''N'' THEN (OA_INCL.TaxSum/OA_MAX.Countt)
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN (OA_ADIC.TaxSum/OA_MAX.Countt)
						else 0.00 end end end
				from OINV T0
					inner join INV1 T1 on T0.DocEntry = T1.DocEntry and T1.TargetType <> N''14''
					inner join INV6 T2 on T0.DocEntry = T2.DocEntry
					inner join OITM T3 on T1.ItemCode = T3.ItemCode
					inner join OITB T4 on T3.ItmsGrpCod = T4.ItmsGrpCod
					inner join OCRD T5 on T0.CardCode = T5.CardCode
					left  join OPRC T6 on T1.OcrCode = T6.PrcCode
					left  join OSLP T7 on (T0.SlpCode = T7.SlpCode or T5.SlpCode = T7.SlpCode)
					left  join OMRC T8 on T3.FirmCode = T8.FirmCode
					left  join OOND T9 on T5.IndustryC = T9.IndCode
					left  join INV12 T10 on T0.DocEntry = T10.DocEntry
					left  join OCNT T11 on T10.CountyB = T11.AbsId
				outer apply (
					select Countt = max(INV6.InstlmntID) from INV6 where INV6.DocEntry = T0.DocEntry
				) as OA_MAX
				outer apply (
					select TaxSum = isnull(sum(distinct INV4.TaxSum), 0.00)
					from INV4
						inner join OSTA on INV4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where INV4.TaxInPrice = ''Y'' and INV4.DocEntry = T1.DocEntry and INV4.LineNum = T1.LineNum
				) as OA_INCL				
				outer apply (
					select TaxSum = isnull(sum(distinct INV4.TaxSum), 0.00)
					from INV4
						inner join OSTA on INV4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where INV4.TaxInPrice = ''Y'' and INV4.DocEntry = T1.DocEntry and INV4.LineNum = T1.LineNum
				) as OA_ADIC
				where T0.CANCELED = ''N'' AND T0.DocTotal <> 0.00 ' + @Where + '
				order by 5, 1, 7, 10
				'

				set @Params = N'@P1 int, @P2 int, @P3 numeric(19,6), @P4 int'
				insert into #tmp_table_fat
					exec sp_executesql @Query, @Params, @P1 = @Regrcode, @P2 = @Comissionado, @P3 = @Comissao, @P4 = @Prioridade

				set @Query = N'
				select distinct @P1, @P2, @P3, @P4,
					T0.DocEntry,
					T0.DocDate,
					T0.Serial,
					T0.DocTotal,
					T0.ObjType,
					T1.ItemCode,
					T1.LineNum,
					T1.Quantity,
					T1.LineTotal,
					T2.InstlmntID,
					case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN T2.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt)-(OA_ADIC.TaxSum/OA_MAX.Countt)
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''N'' THEN T2.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt)
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN T2.InsTotal-(OA_ADIC.TaxSum/OA_MAX.Countt)
						else T2.InsTotal end end end,
					T2.DueDate,
					T3.ItemName,
					T4.ItmsGrpCod,
					T4.ItmsGrpNam,
					T5.CardCode,
					T5.CardName,
					T6.PrcCode,
					T6.PrcName,
					T7.SlpCode,
					T7.SlpName,
					T8.FirmCode,
					T8.FirmName,
					T9.IndCode,
					T9.IndName,
					T9.IndDesc,
					T11.AbsId,
					T11.Code,
					T11.Country,
					T11.Name,
					T11.[State],
					NULL AS TaxDate,
					NULL AS SumApplied,
					case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN (OA_INCL.TaxSum/OA_MAX.Countt)+(OA_ADIC.TaxSum/OA_MAX.Countt)
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''N'' THEN (OA_INCL.TaxSum/OA_MAX.Countt)
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN (OA_ADIC.TaxSum/OA_MAX.Countt)
						else 0.00 end end end
				from ODPI T0
					inner join DPI1 T1 on T0.DocEntry = T1.DocEntry and T1.TargetType <> N''14''
					inner join DPI6 T2 on T0.DocEntry = T2.DocEntry
					inner join OITM T3 on T1.ItemCode = T3.ItemCode
					inner join OITB T4 on T3.ItmsGrpCod = T4.ItmsGrpCod
					inner join OCRD T5 on T0.CardCode = T5.CardCode
					left  join OPRC T6 on T1.OcrCode = T6.PrcCode
					left  join OSLP T7 on (T0.SlpCode = T7.SlpCode or T5.SlpCode = T7.SlpCode)
					left  join OMRC T8 on T3.FirmCode = T8.FirmCode
					left  join OOND T9 on T5.IndustryC = T9.IndCode
					left  join DPI12 T10 on T0.DocEntry = T10.DocEntry
					left  join OCNT T11 on T10.CountyB = T11.AbsId
				outer apply (
					select Countt = max(DPI6.InstlmntID) from DPI6 where DPI6.DocEntry = T0.DocEntry
				) as OA_MAX
				outer apply (
					select TaxSum = isnull(sum(distinct DPI4.TaxSum), 0.00)
					from DPI4
						inner join OSTA on DPI4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where DPI4.TaxInPrice = ''Y'' and DPI4.DocEntry = T1.DocEntry and DPI4.LineNum = T1.LineNum
				) as OA_INCL				
				outer apply (
					select TaxSum = isnull(sum(distinct DPI4.TaxSum), 0.00)
					from DPI4
						inner join OSTA on DPI4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where DPI4.TaxInPrice = ''Y'' and DPI4.DocEntry = T1.DocEntry and DPI4.LineNum = T1.LineNum
				) as OA_ADIC
				where T0.CANCELED = ''N'' AND T0.DocTotal <> 0.00 ' + @Where + '
				order by 5, 1, 7, 10
				'

				set @Params = N'@P1 int, @P2 int, @P3 numeric(19,6), @P4 int'
				insert into #tmp_table_fat
					exec sp_executesql @Query, @Params, @P1 = @Regrcode, @P2 = @Comissionado, @P3 = @Comissao, @P4 = @Prioridade

				set @Query = N'
				select distinct @P1, @P2, @P3, @P4,
					T0.DocEntry,
					T0.DocDate,
					T0.Serial,
					-T0.DocTotal,
					T0.ObjType,
					T1.ItemCode,
					T1.LineNum,
					-T1.Quantity,
					-T1.LineTotal,
					T2.InstlmntID,
					case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN -(T2.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt)-(OA_ADIC.TaxSum/OA_MAX.Countt))
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''N'' THEN -(T2.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt))
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN -(T2.InsTotal-(OA_ADIC.TaxSum/OA_MAX.Countt))
						else -T2.InsTotal end end end,
					T2.DueDate,
					T3.ItemName,
					T4.ItmsGrpCod,
					T4.ItmsGrpNam,
					T5.CardCode,
					T5.CardName,
					T6.PrcCode,
					T6.PrcName,
					T7.SlpCode,
					T7.SlpName,
					T8.FirmCode,
					T8.FirmName,
					T9.IndCode,
					T9.IndName,
					T9.IndDesc,
					T11.AbsId,
					T11.Code,
					T11.Country,
					T11.Name,
					T11.[State],
					NULL AS TaxDate,
					NULL AS SumApplied,
					case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN -((OA_INCL.TaxSum/OA_MAX.Countt)+(OA_ADIC.TaxSum/OA_MAX.Countt))
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''N'' THEN -((OA_INCL.TaxSum/OA_MAX.Countt))
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN -((OA_ADIC.TaxSum/OA_MAX.Countt))
						else 0.00 end end end
				from ORIN T0
					inner join RIN1 T1 on T0.DocEntry = T1.DocEntry
					inner join RIN6 T2 on T0.DocEntry = T2.DocEntry
					inner join OITM T3 on T1.ItemCode = T3.ItemCode
					inner join OITB T4 on T3.ItmsGrpCod = T4.ItmsGrpCod
					inner join OCRD T5 on T0.CardCode = T5.CardCode
					left  join OPRC T6 on T1.OcrCode = T6.PrcCode
					left  join OSLP T7 on (T0.SlpCode = T7.SlpCode or T5.SlpCode = T7.SlpCode)
					left  join OMRC T8 on T3.FirmCode = T8.FirmCode
					left  join OOND T9 on T5.IndustryC = T9.IndCode
					left  join RIN12 T10 on T0.DocEntry = T10.DocEntry
					left  join OCNT T11 on T10.CountyB = T11.AbsId
				outer apply (
					select Countt = max(RIN6.InstlmntID) from RIN6 where RIN6.DocEntry = T0.DocEntry
				) as OA_MAX
				outer apply (
					select TaxSum = isnull(sum(distinct RIN4.TaxSum), 0.00)
					from RIN4
						inner join OSTA on RIN4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where RIN4.TaxInPrice = ''Y'' and RIN4.DocEntry = T1.DocEntry and RIN4.LineNum = T1.LineNum
				) as OA_INCL				
				outer apply (
					select TaxSum = isnull(sum(distinct RIN4.TaxSum), 0.00)
					from RIN4
						inner join OSTA on RIN4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where RIN4.TaxInPrice = ''Y'' and RIN4.DocEntry = T1.DocEntry and RIN4.LineNum = T1.LineNum
				) as OA_ADIC
				where T0.CANCELED = ''N'' AND T0.DocTotal <> 0.00 ' + @Where + '
				order by 5, 1, 7, 10		
				'

				set @Params = N'@P1 int, @P2 int, @P3 numeric(19,6), @P4 int'
				insert into #tmp_table_fat
					exec sp_executesql @Query, @Params, @P1 = @Regrcode, @P2 = @Comissionado, @P3 = @Comissao, @P4 = @Prioridade
			end

			if @Momento = 'P'
			begin
				set @Query = N'
				select distinct @P1, @P2, @P3, @P4,
					T0.DocEntry,
					T0.DocDate,
					T0.Serial,
					T0.DocTotal,
					T0.ObjType,
					T1.ItemCode,
					T1.LineNum,
					T1.Quantity,
					T1.LineTotal,
					T2.InstlmntID,
					case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN T2.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt)-(OA_ADIC.TaxSum/OA_MAX.Countt)
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''N'' THEN T2.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt)
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN T2.InsTotal-(OA_ADIC.TaxSum/OA_MAX.Countt)
						else T2.InsTotal end end end,
					T2.DueDate,
					T3.ItemName,
					T4.ItmsGrpCod,
					T4.ItmsGrpNam,
					T5.CardCode,
					T5.CardName,
					T6.PrcCode,
					T6.PrcName,
					T7.SlpCode,
					T7.SlpName,
					T8.FirmCode,
					T8.FirmName,
					T9.IndCode,
					T9.IndName,
					T9.IndDesc,
					T11.AbsId,
					T11.Code,
					T11.Country,
					T11.Name,
					T11.[State],
					NULL AS TaxDate,
					NULL AS SumApplied,
					case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN (OA_INCL.TaxSum/OA_MAX.Countt)+(OA_ADIC.TaxSum/OA_MAX.Countt)
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''N'' THEN (OA_INCL.TaxSum/OA_MAX.Countt)
						else case when isnull(T7.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T7.U_CVA_IMPADC, ''N'') = ''Y'' THEN (OA_ADIC.TaxSum/OA_MAX.Countt)
						else 0.00 end end end
				from ORDR T0
					inner join RDR1 T1 on T0.DocEntry = T1.DocEntry and T1.TargetType not in (N''14'', N''13'')
					inner join RDR6 T2 on T0.DocEntry = T2.DocEntry
					inner join OITM T3 on T1.ItemCode = T3.ItemCode
					inner join OITB T4 on T3.ItmsGrpCod = T4.ItmsGrpCod
					inner join OCRD T5 on T0.CardCode = T5.CardCode
					left  join OPRC T6 on T1.OcrCode = T6.PrcCode
					left  join OSLP T7 on (T0.SlpCode = T7.SlpCode or T5.SlpCode = T7.SlpCode)
					left  join OMRC T8 on T3.FirmCode = T8.FirmCode
					left  join OOND T9 on T5.IndustryC = T9.IndCode
					left  join RDR12 T10 on T0.DocEntry = T10.DocEntry
					left  join OCNT T11 on T10.CountyB = T11.AbsId
				outer apply (
					select Countt = max(RDR6.InstlmntID) from RDR6 where RDR6.DocEntry = T0.DocEntry
				) as OA_MAX
				outer apply (
					select TaxSum = isnull(sum(distinct RDR4.TaxSum), 0.00)
					from RDR4
						inner join OSTA on RDR4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where RDR4.TaxInPrice = ''Y'' and RDR4.DocEntry = T1.DocEntry and RDR4.LineNum = T1.LineNum
				) as OA_INCL				
				outer apply (
					select TaxSum = isnull(sum(distinct RDR4.TaxSum), 0.00)
					from RDR4
						inner join OSTA on RDR4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where RDR4.TaxInPrice = ''Y'' and RDR4.DocEntry = T1.DocEntry and RDR4.LineNum = T1.LineNum
				) as OA_ADIC
				where T0.CANCELED = ''N'' AND T0.DocTotal <> 0.00 ' + @Where + '
				order by 5, 1, 7, 10
				'

				set @Params = N'@P1 int, @P2 int, @P3 numeric(19,6), @P4 int'
				insert into #tmp_table_ped
					exec sp_executesql @Query, @Params, @P1 = @Regrcode, @P2 = @Comissionado, @P3 = @Comissao, @P4 = @Prioridade
			end

			if @Momento = 'R'
			begin
				set @Query = N'
				select distinct @P1, @P2, @P3, @P4,
					T9.DocEntry, T9.DocDate, T9.Serial, T9.DocTotal, T9.ObjType,
					T10.ItemCode, T10.LineNum, T10.Quantity, T10.LineTotal,
					T8.InstlmntID, 
					case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN T8.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt)-(OA_ADIC.TaxSum/OA_MAX.Countt)
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''N'' THEN T8.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt)
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN T8.InsTotal-(OA_ADIC.TaxSum/OA_MAX.Countt)
						else T8.InsTotal end end end,
					T8.DueDate,
					T11.ItemName,
					T12.ItmsGrpCod, T12.ItmsGrpNam,
					T1.CardCode, T1.CardName,
					T13.PrcCode, T13.PrcName,
					T14.SlpCode, T14.SlpName,
					T15.FirmCode, T15.FirmName,
					T16.IndCode, T16.IndName, T16.IndDesc,
					T18.AbsId, T18.Code, T18.Country, T18.Name, T18.[State],
					OA_PGTO.DataPagamento,
					T2.SumApplied, 
					case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN (OA_INCL.TaxSum/OA_MAX.Countt)+(OA_ADIC.TaxSum/OA_MAX.Countt)
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''N'' THEN (OA_INCL.TaxSum/OA_MAX.Countt)
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN (OA_ADIC.TaxSum/OA_MAX.Countt)
						else 0.00 end end end
				from ORCT T0
					inner join OCRD T1 on T0.CardCode = T1.CardCode
					inner join RCT2 T2 on T0.DocEntry = T2.DocNum
					left  join RCT1 T3 on T0.DocEntry = T3.DocNum
					left  join OBOE T4 on T0.BoeAbs = T4.BoeKey
					left  join OBOT T5 on T5.AbsEntry = 
					(
						select top 1 BOT1.AbsEntry from BOT1
						where BOT1.BOENumber = T4.BoeNum and BOT1.BoeType = T4.BoeType
						order by BOT1.AbsEntry desc
					) and T5.StatusTo = ''P''
					left  join OJDT T6 on T5.TransId = T6.Number
					left  join OCHO T7 on T3.CheckAbs = T7.CheckKey
					inner join INV6 T8 on T2.DocEntry = T8.DocEntry and T2.InstId = T8.InstlmntID and T2.InvType = T8.ObjType
					inner join OINV T9 on T8.DocEntry = T9.DocEntry and T9.CANCELED = ''N'' and T9.DocTotal <> 0.00
					inner join INV1 T10 on T9.DocEntry = T10.DocEntry and T10.TargetType <> N''14''
					inner join OITM T11 on T10.ItemCode = T11.ItemCode
					inner join OITB T12 on T11.ItmsGrpCod = T12.ItmsGrpCod
					left  join OPRC T13 on T10.OcrCode = T13.PrcCode
					left  join OSLP T14 on (T9.SlpCode = T14.SlpCode or T1.SlpCode = T14.SlpCode)
					left  join OMRC T15 on T11.FirmCode = T15.FirmCode
					left  join OOND T16 on T1.IndustryC = T16.IndCode
					left  join INV12 T17 on T9.DocEntry = T17.DocEntry
					left  join OCNT T18 on T17.CountyB = T18.AbsId
				outer apply (
					select Countt = COUNT(RCT2.InvoiceId) FROM RCT2 WHERE RCT2.DocNum = T0.DocEntry
				) as OA_MAX
				outer apply (
					select DataPagamento = 
						case when T5.StatusTo = ''P'' then T6.RefDate
						else case when T7.CheckKey is not null then T7.CheckDate
						else case when T0.BoeAbs is null and T0.DocEntry is not null then T0.TaxDate
						else null end end end
				) as OA_PGTO
				outer apply (
					select Countt = max(INV6.InstlmntID) from INV6 where INV6.DocEntry = T9.DocEntry
				) as OA_MAXX
				outer apply (
					select TaxSum = isnull(sum(distinct INV4.TaxSum), 0.00)
					from INV4
						inner join OSTA on INV4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where INV4.TaxInPrice = ''Y'' and INV4.DocEntry = T10.DocEntry and INV4.LineNum = T10.LineNum
				) as OA_INCL				
				outer apply (
					select TaxSum = isnull(sum(distinct INV4.TaxSum), 0.00)
					from INV4
						inner join OSTA on INV4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where INV4.TaxInPrice = ''Y'' and INV4.DocEntry = T10.DocEntry and INV4.LineNum = T10.LineNum
				) as OA_ADIC
				where T0.Canceled = ''N'' and T2.InvType = N''13'' and OA_PGTO.DataPagamento is not null ' + @Where + '
				order by 5, 1, 7, 10
				'

				set @Params = N'@P1 int, @P2 int, @P3 numeric(19,6), @P4 int'
				insert into #tmp_table_rec
					exec sp_executesql @Query, @Params, @P1 = @RegrCode, @P2 = @Comissionado, @P3 = @Comissao, @P4 = @Prioridade

				set @Query = N'
				select distinct @P1, @P2, @P3, @P4,
					T9.DocEntry, T9.DocDate, T9.Serial, T9.DocTotal, T9.ObjType,
					T10.ItemCode, T10.LineNum, T10.Quantity, T10.LineTotal,
					T8.InstlmntID, 
					case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN T8.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt)-(OA_ADIC.TaxSum/OA_MAX.Countt)
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''N'' THEN T8.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt)
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN T8.InsTotal-(OA_ADIC.TaxSum/OA_MAX.Countt)
						else T8.InsTotal end end end,
					T8.DueDate,
					T11.ItemName,
					T12.ItmsGrpCod, T12.ItmsGrpNam,
					T1.CardCode, T1.CardName,
					T13.PrcCode, T13.PrcName,
					T14.SlpCode, T14.SlpName,
					T15.FirmCode, T15.FirmName,
					T16.IndCode, T16.IndName, T16.IndDesc,
					T18.AbsId, T18.Code, T18.Country, T18.Name, T18.[State],
					OA_PGTO.DataPagamento,
					T2.SumApplied, 
					case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN (OA_INCL.TaxSum/OA_MAX.Countt)+(OA_ADIC.TaxSum/OA_MAX.Countt)
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''N'' THEN (OA_INCL.TaxSum/OA_MAX.Countt)
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN (OA_ADIC.TaxSum/OA_MAX.Countt)
						else 0.00 end end end
				from ORCT T0
					inner join OCRD T1 on T0.CardCode = T1.CardCode
					inner join RCT2 T2 on T0.DocEntry = T2.DocNum
					left  join RCT1 T3 on T0.DocEntry = T3.DocNum
					left  join OBOE T4 on T0.BoeAbs = T4.BoeKey
					left  join OBOT T5 on T5.AbsEntry = 
					(
						select top 1 BOT1.AbsEntry from BOT1
						where BOT1.BOENumber = T4.BoeNum and BOT1.BoeType = T4.BoeType
						order by BOT1.AbsEntry desc
					) and T5.StatusTo = ''P''
					left  join OJDT T6 on T5.TransId = T6.Number
					left  join OCHO T7 on T3.CheckAbs = T7.CheckKey
					inner join DPI6 T8 on T2.DocEntry = T8.DocEntry and T2.InstId = T8.InstlmntID and T2.InvType = T8.ObjType
					inner join ODPI T9 on T8.DocEntry = T9.DocEntry and T9.CANCELED = ''N'' and T9.DocTotal <> 0.00
					inner join DPI1 T10 on T9.DocEntry = T10.DocEntry and T10.TargetType <> N''14''
					inner join OITM T11 on T10.ItemCode = T11.ItemCode
					inner join OITB T12 on T11.ItmsGrpCod = T12.ItmsGrpCod
					left  join OPRC T13 on T10.OcrCode = T13.PrcCode
					left  join OSLP T14 on (T9.SlpCode = T14.SlpCode or T1.SlpCode = T14.SlpCode)
					left  join OMRC T15 on T11.FirmCode = T15.FirmCode
					left  join OOND T16 on T1.IndustryC = T16.IndCode
					left  join DPI12 T17 on T9.DocEntry = T17.DocEntry
					left  join OCNT T18 on T17.CountyB = T18.AbsId
				outer apply (
					select Countt = COUNT(RCT2.InvoiceId) FROM RCT2 WHERE RCT2.DocNum = T0.DocEntry
				) as OA_MAX
				outer apply (
					select DataPagamento = 
						case when T5.StatusTo = ''P'' then T6.RefDate
						else case when T7.CheckKey is not null then T7.CheckDate
						else case when T0.BoeAbs is null and T0.DocEntry is not null then T0.TaxDate
						else null end end end
				) as OA_PGTO
				outer apply (
					select Countt = max(DPI6.InstlmntID) from DPI6 where DPI6.DocEntry = T9.DocEntry
				) as OA_MAXX
				outer apply (
					select TaxSum = isnull(sum(distinct DPI4.TaxSum), 0.00)
					from DPI4
						inner join OSTA on DPI4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where DPI4.TaxInPrice = ''Y'' and DPI4.DocEntry = T10.DocEntry and DPI4.LineNum = T10.LineNum
				) as OA_INCL				
				outer apply (
					select TaxSum = isnull(sum(distinct DPI4.TaxSum), 0.00)
					from DPI4
						inner join OSTA on DPI4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where DPI4.TaxInPrice = ''Y'' and DPI4.DocEntry = T10.DocEntry and DPI4.LineNum = T10.LineNum
				) as OA_ADIC
				where T0.Canceled = ''N'' and T2.InvType = N''203'' and OA_PGTO.DataPagamento is not null ' + @Where + '
				order by 5, 1, 7, 10
				'

				set @Params = N'@P1 int, @P2 int, @P3 numeric(19,6), @P4 int'
				insert into #tmp_table_rec
					exec sp_executesql @Query, @Params, @P1 = @RegrCode, @P2 = @Comissionado, @P3 = @Comissao, @P4 = @Prioridade

				set @Query = N'
				select distinct @P1, @P2, @P3, @P4,
					T9.DocEntry, T9.DocDate, T9.Serial, -T9.DocTotal, T9.ObjType,
					T10.ItemCode, T10.LineNum, -T10.Quantity, -T10.LineTotal,
					T8.InstlmntID, 
					case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN -(T8.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt)-(OA_ADIC.TaxSum/OA_MAX.Countt))
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''N'' THEN -(T8.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt))
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN -(T8.InsTotal-(OA_ADIC.TaxSum/OA_MAX.Countt))
						else -T8.InsTotal end end end,
					T8.DueDate,
					T11.ItemName,
					T12.ItmsGrpCod, T12.ItmsGrpNam,
					T1.CardCode, T1.CardName,
					T13.PrcCode, T13.PrcName,
					T14.SlpCode, T14.SlpName,
					T15.FirmCode, T15.FirmName,
					T16.IndCode, T16.IndName, T16.IndDesc,
					T18.AbsId, T18.Code, T18.Country, T18.Name, T18.[State],
					OA_PGTO.DataPagamento,
					-T2.SumApplied, 
					case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN -((OA_INCL.TaxSum/OA_MAX.Countt)+(OA_ADIC.TaxSum/OA_MAX.Countt))
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''N'' THEN -((OA_INCL.TaxSum/OA_MAX.Countt))
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN -((OA_ADIC.TaxSum/OA_MAX.Countt))
						else 0.00 end end end
				from ORCT T0
					inner join OCRD T1 on T0.CardCode = T1.CardCode
					inner join RCT2 T2 on T0.DocEntry = T2.DocNum
					left  join RCT1 T3 on T0.DocEntry = T3.DocNum
					left  join OBOE T4 on T0.BoeAbs = T4.BoeKey
					left  join OBOT T5 on T5.AbsEntry = 
					(
						select top 1 BOT1.AbsEntry from BOT1
						where BOT1.BOENumber = T4.BoeNum and BOT1.BoeType = T4.BoeType
						order by BOT1.AbsEntry desc
					) and T5.StatusTo = ''P''
					left  join OJDT T6 on T5.TransId = T6.Number
					left  join OCHO T7 on T3.CheckAbs = T7.CheckKey
					inner join RIN6 T8 on T2.DocEntry = T8.DocEntry and T2.InstId = T8.InstlmntID and T2.InvType = T8.ObjType
					inner join ORIN T9 on T8.DocEntry = T9.DocEntry and T9.CANCELED = ''N'' and T9.DocTotal <> 0.00
					inner join RIN1 T10 on T9.DocEntry = T10.DocEntry
					inner join OITM T11 on T10.ItemCode = T11.ItemCode
					inner join OITB T12 on T11.ItmsGrpCod = T12.ItmsGrpCod
					left  join OPRC T13 on T10.OcrCode = T13.PrcCode
					left  join OSLP T14 on (T9.SlpCode = T14.SlpCode or T1.SlpCode = T14.SlpCode)
					left  join OMRC T15 on T11.FirmCode = T15.FirmCode
					left  join OOND T16 on T1.IndustryC = T16.IndCode
					left  join RIN12 T17 on T9.DocEntry = T17.DocEntry
					left  join OCNT T18 on T17.CountyB = T18.AbsId
				outer apply (
					select Countt = COUNT(RCT2.InvoiceId) FROM RCT2 WHERE RCT2.DocNum = T0.DocEntry
				) as OA_MAX
				outer apply (
					select DataPagamento = 
						case when T5.StatusTo = ''P'' then T6.RefDate
						else case when T7.CheckKey is not null then T7.CheckDate
						else case when T0.BoeAbs is null and T0.DocEntry is not null then T0.TaxDate
						else null end end end
				) as OA_PGTO
				outer apply (
					select Countt = max(RIN6.InstlmntID) from RIN6 where RIN6.DocEntry = T9.DocEntry
				) as OA_MAXX
				outer apply (
					select TaxSum = isnull(sum(distinct RIN4.TaxSum), 0.00)
					from RIN4
						inner join OSTA on RIN4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where RIN4.TaxInPrice = ''Y'' and RIN4.DocEntry = T10.DocEntry and RIN4.LineNum = T10.LineNum
				) as OA_INCL				
				outer apply (
					select TaxSum = isnull(sum(distinct RIN4.TaxSum), 0.00)
					from RIN4
						inner join OSTA on RIN4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where RIN4.TaxInPrice = ''Y'' and RIN4.DocEntry = T10.DocEntry and RIN4.LineNum = T10.LineNum
				) as OA_ADIC
				where T0.Canceled = ''N'' and T2.InvType = N''14'' and OA_PGTO.DataPagamento is not null ' + @Where + '
				order by 5, 1, 7, 10
				'

				set @Params = N'@P1 int, @P2 int, @P3 numeric(19,6), @P4 int'
				insert into #tmp_table_rec
					exec sp_executesql @Query, @Params, @P1 = @RegrCode, @P2 = @Comissionado, @P3 = @Comissao, @P4 = @Prioridade

				set @Query = N'
				select distinct @P1, @P2, @P3, @P4,
					T9.DocEntry, T9.DocDate, T9.Serial, T9.DocTotal, T9.ObjType,
					T10.ItemCode, T10.LineNum, T10.Quantity, T10.LineTotal,
					T8.InstlmntID, 
					case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN T8.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt)-(OA_ADIC.TaxSum/OA_MAX.Countt)
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''N'' THEN T8.InsTotal-(OA_INCL.TaxSum/OA_MAX.Countt)
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN T8.InsTotal-(OA_ADIC.TaxSum/OA_MAX.Countt)
						else T8.InsTotal end end end,
					T8.DueDate,
					T11.ItemName,
					T12.ItmsGrpCod, T12.ItmsGrpNam,
					T1.CardCode, T1.CardName,
					T13.PrcCode, T13.PrcName,
					T14.SlpCode, T14.SlpName,
					T15.FirmCode, T15.FirmName,
					T16.IndCode, T16.IndName, T16.IndDesc,
					T18.AbsId, T18.Code, T18.Country, T18.Name, T18.[State],
					OA_PGTO.DataPagamento,
					T2.SumApplied, 
					case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN (OA_INCL.TaxSum/OA_MAX.Countt)+(OA_ADIC.TaxSum/OA_MAX.Countt)
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''Y'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''N'' THEN (OA_INCL.TaxSum/OA_MAX.Countt)
						else case when isnull(T14.U_CVA_IMPINCL, ''N'') = ''N'' and isnull(T14.U_CVA_IMPADC, ''N'') = ''Y'' THEN (OA_ADIC.TaxSum/OA_MAX.Countt)
						else 0.00 end end end
				from ORCT T0
					inner join OCRD T1 on T0.CardCode = T1.CardCode
					inner join RCT2 T2 on T0.DocEntry = T2.DocNum
					left  join RCT1 T3 on T0.DocEntry = T3.DocNum
					left  join OBOE T4 on T0.BoeAbs = T4.BoeKey
					left  join OBOT T5 on T5.AbsEntry = 
					(
						select top 1 BOT1.AbsEntry from BOT1
						where BOT1.BOENumber = T4.BoeNum and BOT1.BoeType = T4.BoeType
						order by BOT1.AbsEntry desc
					) and T5.StatusTo = ''P''
					left  join OJDT T6 on T5.TransId = T6.Number
					left  join OCHO T7 on T3.CheckAbs = T7.CheckKey
					inner join RDR6 T8 on T2.DocEntry = T8.DocEntry and T2.InstId = T8.InstlmntID and T2.InvType = T8.ObjType
					inner join ORDR T9 on T8.DocEntry = T9.DocEntry and T9.CANCELED = ''N'' and T9.DocTotal <> 0.00
					inner join RDR1 T10 on T9.DocEntry = T10.DocEntry
					inner join OITM T11 on T10.ItemCode = T11.ItemCode
					inner join OITB T12 on T11.ItmsGrpCod = T12.ItmsGrpCod
					left  join OPRC T13 on T10.OcrCode = T13.PrcCode
					left  join OSLP T14 on (T9.SlpCode = T14.SlpCode or T1.SlpCode = T14.SlpCode)
					left  join OMRC T15 on T11.FirmCode = T15.FirmCode
					left  join OOND T16 on T1.IndustryC = T16.IndCode
					left  join RDR12 T17 on T9.DocEntry = T17.DocEntry
					left  join OCNT T18 on T17.CountyB = T18.AbsId
				outer apply (
					select Countt = COUNT(RCT2.InvoiceId) FROM RCT2 WHERE RCT2.DocNum = T0.DocEntry
				) as OA_MAX
				outer apply (
					select DataPagamento = 
						case when T5.StatusTo = ''P'' then T6.RefDate
						else case when T7.CheckKey is not null then T7.CheckDate
						else case when T0.BoeAbs is null and T0.DocEntry is not null then T0.TaxDate
						else null end end end
				) as OA_PGTO
				outer apply (
					select Countt = max(RDR6.InstlmntID) from RDR6 where RDR6.DocEntry = T9.DocEntry
				) as OA_MAXX
				outer apply (
					select TaxSum = isnull(sum(distinct RDR4.TaxSum), 0.00)
					from RDR4
						inner join OSTA on RDR4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where RDR4.TaxInPrice = ''Y'' and RDR4.DocEntry = T10.DocEntry and RDR4.LineNum = T10.LineNum
				) as OA_INCL				
				outer apply (
					select TaxSum = isnull(sum(distinct RDR4.TaxSum), 0.00)
					from RDR4
						inner join OSTA on RDR4.StaCode = OSTA.Code
						inner join OSTT on OSTA.[Type] = OSTT.AbsId
						inner join ONFT on OSTT.NfTaxId = ONFT.AbsId
					where RDR4.TaxInPrice = ''Y'' and RDR4.DocEntry = T10.DocEntry and RDR4.LineNum = T10.LineNum
				) as OA_ADIC
				where T0.Canceled = ''N'' and T2.InvType = N''17'' and OA_PGTO.DataPagamento is not null ' + @Where + '
				order by 5, 1, 7, 10
				'

				set @Params = N'@P1 int, @P2 int, @P3 numeric(19,6), @P4 int'
				insert into #tmp_table_rec
					exec sp_executesql @Query, @Params, @P1 = @RegrCode, @P2 = @Comissionado, @P3 = @Comissao, @P4 = @Prioridade
			end

			fetch next from @cur into @RegrCode, @Comissionado, @Comissao, @Momento, @Prioridade, @Where
		end

		close @cur
		deallocate @cur
	end

	if @Status = 'P'
	begin
		insert into #tmp_table_ret
		select 
			T0.U_COMISSIONADO,
			T2.U_POS AS U_PRIORIDADE,
			T0.U_CARDCODE,
			T0.U_CARDNAME,
			T0.U_REGRA,
			T0.U_DOCDATE,
			T0.U_DUEDATE,
			T0.U_DOCENTRY,
			T0.U_OBJTYPE,
			T0.U_ITEMCODE,
			T0.U_ITEMNAME,
			T0.U_LINENUM,
			T0.U_VALOR,
			T0.U_PARCELA,
			T0.U_IMPOSTOS,
			T0.U_COMISSAO,
			T0.U_VALORCOMISSAO,
			T0.U_CENTROCUSTO,
			T0.U_PAGO,
			T0.U_DATAPAGAMENTO
		from [@CVA_CALC_COMISSAO] T0
			inner join [@CVA_REGR_COMISSAO] T1 on T0.U_REGRA = T1.Code
			inner join [@CVA_CRIT_COMISSAO] T2 on T1.U_PRIORIDADE = T2.Code
		where T0.U_PAGO = 'Y'
			and T0.U_DATAPAGAMENTO >= @DataInicial
			and T0.U_DATAPAGAMENTO <= @DataFinal
	end
	else if @Status = 'N'
	begin
		insert into #tmp_table_ret
		select distinct
			isnull(T1.U_COMISSIONADO, T0.Comissionado) as U_COMISSIONADO,
			isnull(T3.U_POS, T0.Prioridade) as U_PRIORIDADE,
			isnull(T1.U_CARDCODE, T0.CardCode) as U_CARDCODE,
			isnull(T1.U_CARDNAME, T0.CardName) as U_CARDNAME,
			isnull(T1.U_REGRA, T0.RegrCode) as U_REGRA,
			isnull(T1.U_DOCDATE, T0.DocDate) as U_DOCDATE,
			isnull(T1.U_DUEDATE, T0.DueDate) as U_DUEDATE,
			isnull(T1.U_DOCENTRY, T0.DocEntry) as U_DOCENTRY,
			isnull(T1.U_OBJTYPE, T0.ObjType) as U_OBJTYPE,
			isnull(T1.U_ITEMCODE, T0.ItemCode) as U_ITEMCODE,
			isnull(T1.U_ITEMNAME, T0.ItemName) as U_ITEMNAME,
			isnull(T1.U_LINENUM, T0.LineNum) as U_LINENUM,
			isnull(T1.U_VALOR, T0.InsTotal) as U_VALOR,
			isnull(T1.U_PARCELA, T0.InstlmntID) as U_PARCELA,
			isnull(T1.U_IMPOSTOS, T0.TaxSum) as U_IMPOSTOS,
			isnull(T1.U_COMISSAO, T0.Comissao*100) as U_COMISSAO,
			isnull(T1.U_VALORCOMISSAO, T0.InsTotal*T0.Comissao) as U_VALORCOMISSAO,
			isnull(T1.U_CENTROCUSTO, T0.PrcCode) as U_CENTROCUSTO,
			isnull(T1.U_PAGO, 'N') as U_PAGO,
			T1.U_DATAPAGAMENTO as U_DATAPAGAMENTO
		from #tmp_table_fat T0
			left join [@CVA_CALC_COMISSAO] T1 on T0.DocEntry = T1.U_DOCENTRY and T0.ObjType = T1.U_OBJTYPE and T1.U_PAGO = 'N' and T0.LineNum = T1.U_LINENUM
			left join [@CVA_REGR_COMISSAO] T2 on T1.U_REGRA = T2.Code
			left join [@CVA_CRIT_COMISSAO] T3 on T2.U_PRIORIDADE = T3.Code
		where isnull(T1.U_DOCDATE, T0.DocDate) >= @DataInicial
			and isnull(T1.U_DOCDATE, T0.DocDate) <= @DataFinal
		union all
		select distinct
			isnull(T1.U_COMISSIONADO, T0.Comissionado) as U_COMISSIONADO,
			isnull(T3.U_POS, T0.Prioridade) as U_PRIORIDADE,
			isnull(T1.U_CARDCODE, T0.CardCode) as U_CARDCODE,
			isnull(T1.U_CARDNAME, T0.CardName) as U_CARDNAME,
			isnull(T1.U_REGRA, T0.RegrCode) as U_REGRA,
			isnull(T1.U_DOCDATE, T0.DocDate) as U_DOCDATE,
			isnull(T1.U_DUEDATE, T0.DueDate) as U_DUEDATE,
			isnull(T1.U_DOCENTRY, T0.DocEntry) as U_DOCENTRY,
			isnull(T1.U_OBJTYPE, T0.ObjType) as U_OBJTYPE,
			isnull(T1.U_ITEMCODE, T0.ItemCode) as U_ITEMCODE,
			isnull(T1.U_ITEMNAME, T0.ItemName) as U_ITEMNAME,
			isnull(T1.U_LINENUM, T0.LineNum) as U_LINENUM,
			isnull(T1.U_VALOR, T0.InsTotal) as U_VALOR,
			isnull(T1.U_PARCELA, T0.InstlmntID) as U_PARCELA,
			isnull(T1.U_IMPOSTOS, T0.TaxSum) as U_IMPOSTOS,
			isnull(T1.U_COMISSAO, T0.Comissao*100) as U_COMISSAO,
			isnull(T1.U_VALORCOMISSAO, T0.InsTotal*T0.Comissao) as U_VALORCOMISSAO,
			isnull(T1.U_CENTROCUSTO, T0.PrcCode) as U_CENTROCUSTO,
			isnull(T1.U_PAGO, 'N') as U_PAGO,
			T1.U_DATAPAGAMENTO as U_DATAPAGAMENTO
		from #tmp_table_ped T0
			left join [@CVA_CALC_COMISSAO] T1 on T0.DocEntry = T1.U_DOCENTRY and T0.ObjType = T1.U_OBJTYPE and T1.U_PAGO = 'N'
			left join [@CVA_REGR_COMISSAO] T2 on T1.U_REGRA = T2.Code
			left join [@CVA_CRIT_COMISSAO] T3 on T2.U_PRIORIDADE = T3.Code
		where isnull(T1.U_DOCDATE, T0.DocDate) >= @DataInicial
			and isnull(T1.U_DOCDATE, T0.DocDate) <= @DataFinal
		union all
		select distinct
			isnull(T1.U_COMISSIONADO, T0.Comissionado) as U_COMISSIONADO,
			isnull(T3.U_POS, T0.Prioridade) as U_PRIORIDADE,
			isnull(T1.U_CARDCODE, T0.CardCode) as U_CARDCODE,
			isnull(T1.U_CARDNAME, T0.CardName) as U_CARDNAME,
			isnull(T1.U_REGRA, T0.RegrCode) as U_REGRA,
			isnull(T1.U_DOCDATE, T0.DocDate) as U_DOCDATE,
			isnull(T1.U_DUEDATE, T0.DueDate) as U_DUEDATE,
			isnull(T1.U_DOCENTRY, T0.DocEntry) as U_DOCENTRY,
			isnull(T1.U_OBJTYPE, T0.ObjType) as U_OBJTYPE,
			isnull(T1.U_ITEMCODE, T0.ItemCode) as U_ITEMCODE,
			isnull(T1.U_ITEMNAME, T0.ItemName) as U_ITEMNAME,
			isnull(T1.U_LINENUM, T0.LineNum) as U_LINENUM,
			isnull(T1.U_VALOR, T0.InsTotal) as U_VALOR,
			isnull(T1.U_PARCELA, T0.InstlmntID) as U_PARCELA,
			isnull(T1.U_IMPOSTOS, T0.TaxSum) as U_IMPOSTOS,
			isnull(T1.U_COMISSAO, T0.Comissao*100) as U_COMISSAO,
			isnull(T1.U_VALORCOMISSAO, T0.InsTotal*T0.Comissao) as U_VALORCOMISSAO,
			isnull(T1.U_CENTROCUSTO, T0.PrcCode) as U_CENTROCUSTO,
			isnull(T1.U_PAGO, 'N') as U_PAGO,
			T1.U_DATAPAGAMENTO as U_DATAPAGAMENTO
		from #tmp_table_rec T0
			left join [@CVA_CALC_COMISSAO] T1 on T0.DocEntry = T1.U_DOCENTRY and T0.ObjType = T1.U_OBJTYPE and T1.U_PAGO = 'N'
			left join [@CVA_REGR_COMISSAO] T2 on T1.U_REGRA = T2.Code
			left join [@CVA_CRIT_COMISSAO] T3 on T2.U_PRIORIDADE = T3.Code
		where isnull(T1.U_DOCDATE, T0.TaxDate) >= @DataInicial
			and isnull(T1.U_DOCDATE, T0.TaxDate) <= @DataFinal
	end
	else
	begin
		insert into #tmp_table_ret
		select 
			T0.U_COMISSIONADO,
			T2.U_POS AS U_PRIORIDADE,
			T0.U_CARDCODE,
			T0.U_CARDNAME,
			T0.U_REGRA,
			T0.U_DOCDATE,
			T0.U_DUEDATE,
			T0.U_DOCENTRY,
			T0.U_OBJTYPE,
			T0.U_ITEMCODE,
			T0.U_ITEMNAME,
			T0.U_LINENUM,
			T0.U_VALOR,
			T0.U_PARCELA,
			T0.U_IMPOSTOS,
			T0.U_COMISSAO,
			T0.U_VALORCOMISSAO,
			T0.U_CENTROCUSTO,
			T0.U_PAGO,
			T0.U_DATAPAGAMENTO
		from [@CVA_CALC_COMISSAO] T0
			inner join [@CVA_REGR_COMISSAO] T1 on T0.U_REGRA = T1.Code
			inner join [@CVA_CRIT_COMISSAO] T2 on T1.U_PRIORIDADE = T2.Code
		where T0.U_DATAPAGAMENTO >= @DataInicial
			and T0.U_DATAPAGAMENTO <= @DataFinal
		union
		select distinct
			isnull(T1.U_COMISSIONADO, T0.Comissionado) as U_COMISSIONADO,
			isnull(T3.U_POS, T0.Prioridade) as U_PRIORIDADE,
			isnull(T1.U_CARDCODE, T0.CardCode) as U_CARDCODE,
			isnull(T1.U_CARDNAME, T0.CardName) as U_CARDNAME,
			isnull(T1.U_REGRA, T0.RegrCode) as U_REGRA,
			isnull(T1.U_DOCDATE, T0.DocDate) as U_DOCDATE,
			isnull(T1.U_DUEDATE, T0.DueDate) as U_DUEDATE,
			isnull(T1.U_DOCENTRY, T0.DocEntry) as U_DOCENTRY,
			isnull(T1.U_OBJTYPE, T0.ObjType) as U_OBJTYPE,
			isnull(T1.U_ITEMCODE, T0.ItemCode) as U_ITEMCODE,
			isnull(T1.U_ITEMNAME, T0.ItemName) as U_ITEMNAME,
			isnull(T1.U_LINENUM, T0.LineNum) as U_LINENUM,
			isnull(T1.U_VALOR, T0.InsTotal) as U_VALOR,
			isnull(T1.U_PARCELA, T0.InstlmntID) as U_PARCELA,
			isnull(T1.U_IMPOSTOS, T0.TaxSum) as U_IMPOSTOS,
			isnull(T1.U_COMISSAO, T0.Comissao*100) as U_COMISSAO,
			isnull(T1.U_VALORCOMISSAO, T0.InsTotal*T0.Comissao) as U_VALORCOMISSAO,
			isnull(T1.U_CENTROCUSTO, T0.PrcCode) as U_CENTROCUSTO,
			isnull(T1.U_PAGO, 'N') as U_PAGO,
			T1.U_DATAPAGAMENTO as U_DATAPAGAMENTO
		from #tmp_table_fat T0
			left join [@CVA_CALC_COMISSAO] T1 on T0.DocEntry = T1.U_DOCENTRY and T0.ObjType = T1.U_OBJTYPE and T0.LineNum = T1.U_LINENUM
			left join [@CVA_REGR_COMISSAO] T2 on T1.U_REGRA = T2.Code
			left join [@CVA_CRIT_COMISSAO] T3 on T2.U_PRIORIDADE = T3.Code
		where isnull(T1.U_DOCDATE, T0.DocDate) >= @DataInicial
			and isnull(T1.U_DOCDATE, T0.DocDate) <= @DataFinal
		union
		select distinct
			isnull(T1.U_COMISSIONADO, T0.Comissionado) as U_COMISSIONADO,
			isnull(T3.U_POS, T0.Prioridade) as U_PRIORIDADE,
			isnull(T1.U_CARDCODE, T0.CardCode) as U_CARDCODE,
			isnull(T1.U_CARDNAME, T0.CardName) as U_CARDNAME,
			isnull(T1.U_REGRA, T0.RegrCode) as U_REGRA,
			isnull(T1.U_DOCDATE, T0.DocDate) as U_DOCDATE,
			isnull(T1.U_DUEDATE, T0.DueDate) as U_DUEDATE,
			isnull(T1.U_DOCENTRY, T0.DocEntry) as U_DOCENTRY,
			isnull(T1.U_OBJTYPE, T0.ObjType) as U_OBJTYPE,
			isnull(T1.U_ITEMCODE, T0.ItemCode) as U_ITEMCODE,
			isnull(T1.U_ITEMNAME, T0.ItemName) as U_ITEMNAME,
			isnull(T1.U_LINENUM, T0.LineNum) as U_LINENUM,
			isnull(T1.U_VALOR, T0.InsTotal) as U_VALOR,
			isnull(T1.U_PARCELA, T0.InstlmntID) as U_PARCELA,
			isnull(T1.U_IMPOSTOS, T0.TaxSum) as U_IMPOSTOS,
			isnull(T1.U_COMISSAO, T0.Comissao*100) as U_COMISSAO,
			isnull(T1.U_VALORCOMISSAO, T0.InsTotal*T0.Comissao) as U_VALORCOMISSAO,
			isnull(T1.U_CENTROCUSTO, T0.PrcCode) as U_CENTROCUSTO,
			isnull(T1.U_PAGO, 'N') as U_PAGO,
			T1.U_DATAPAGAMENTO as U_DATAPAGAMENTO
		from #tmp_table_ped T0
			left join [@CVA_CALC_COMISSAO] T1 on T0.DocEntry = T1.U_DOCENTRY and T0.ObjType = T1.U_OBJTYPE and T0.LineNum = T1.U_LINENUM
			left join [@CVA_REGR_COMISSAO] T2 on T1.U_REGRA = T2.Code
			left join [@CVA_CRIT_COMISSAO] T3 on T2.U_PRIORIDADE = T3.Code
		where isnull(T1.U_DOCDATE, T0.DocDate) >= @DataInicial
			and isnull(T1.U_DOCDATE, T0.DocDate) <= @DataFinal
		union
		select distinct
			isnull(T1.U_COMISSIONADO, T0.Comissionado) as U_COMISSIONADO,
			isnull(T3.U_POS, T0.Prioridade) as U_PRIORIDADE,
			isnull(T1.U_CARDCODE, T0.CardCode) as U_CARDCODE,
			isnull(T1.U_CARDNAME, T0.CardName) as U_CARDNAME,
			isnull(T1.U_REGRA, T0.RegrCode) as U_REGRA,
			isnull(T1.U_DOCDATE, T0.DocDate) as U_DOCDATE,
			isnull(T1.U_DUEDATE, T0.DueDate) as U_DUEDATE,
			isnull(T1.U_DOCENTRY, T0.DocEntry) as U_DOCENTRY,
			isnull(T1.U_OBJTYPE, T0.ObjType) as U_OBJTYPE,
			isnull(T1.U_ITEMCODE, T0.ItemCode) as U_ITEMCODE,
			isnull(T1.U_ITEMNAME, T0.ItemName) as U_ITEMNAME,
			isnull(T1.U_LINENUM, T0.LineNum) as U_LINENUM,
			isnull(T1.U_VALOR, T0.InsTotal) as U_VALOR,
			isnull(T1.U_PARCELA, T0.InstlmntID) as U_PARCELA,
			isnull(T1.U_IMPOSTOS, T0.TaxSum) as U_IMPOSTOS,
			isnull(T1.U_COMISSAO, T0.Comissao*100) as U_COMISSAO,
			isnull(T1.U_VALORCOMISSAO, T0.InsTotal*T0.Comissao) as U_VALORCOMISSAO,
			isnull(T1.U_CENTROCUSTO, T0.PrcCode) as U_CENTROCUSTO,
			isnull(T1.U_PAGO, 'N') as U_PAGO,
			T1.U_DATAPAGAMENTO as U_DATAPAGAMENTO
		from #tmp_table_rec T0
			left join [@CVA_CALC_COMISSAO] T1 on T0.DocEntry = T1.U_DOCENTRY and T0.ObjType = T1.U_OBJTYPE and T0.LineNum = T1.U_LINENUM
			left join [@CVA_REGR_COMISSAO] T2 on T1.U_REGRA = T2.Code
			left join [@CVA_CRIT_COMISSAO] T3 on T2.U_PRIORIDADE = T3.Code
		where isnull(T1.U_DOCDATE, T0.TaxDate) >= @DataInicial
			and isnull(T1.U_DOCDATE, T0.TaxDate) <= @DataFinal
	end

	if @View = 'T'
	begin
		select 
			U_COMISSIONADO, 
			U_PRIORIDADE,
			U_CARDCODE,
			U_CARDNAME,
			U_REGRA,
			U_DOCDATE,
			U_DUEDATE,
			U_DOCENTRY,
			U_OBJTYPE,
			U_ITEMCODE,
			U_ITEMNAME,
			U_LINENUM,
			U_VALOR,
			U_PARCELA,
			U_IMPOSTOS,
			U_COMISSAO,
			U_VALORCOMISSAO,
			U_CENTROCUSTO,
			U_PAGO,
			isnull(U_DATAPAGAMENTO, '1900-01-01') as U_DATAPAGAMENTO
		from #tmp_table_ret
	end
	else if @View = 'N'
	begin
		select distinct 
			U_COMISSIONADO, 
			U_PRIORIDADE,
			U_CARDCODE,
			U_CARDNAME,
			U_REGRA,
			U_DOCDATE,
			U_DUEDATE,
			U_DOCENTRY,
			U_OBJTYPE,
			null as U_ITEMCODE,
			null as U_ITEMNAME,
			null as U_LINENUM,
			U_VALOR,
			U_PARCELA,
			U_IMPOSTOS,
			U_COMISSAO,
			U_VALORCOMISSAO,
			NULL AS U_CENTROCUSTO,
			U_PAGO,
			isnull(U_DATAPAGAMENTO, '1900-01-01') as U_DATAPAGAMENTO
		from #tmp_table_ret
	end
	else if @View = 'I'
	begin
		select 
			U_COMISSIONADO, 
			U_PRIORIDADE,
			U_CARDCODE,
			U_CARDNAME,
			U_REGRA,
			U_DOCDATE,
			U_DUEDATE,
			U_DOCENTRY,
			U_OBJTYPE,
			U_ITEMCODE,
			U_ITEMNAME,
			U_LINENUM,
			U_VALOR,
			U_PARCELA,
			U_IMPOSTOS,
			U_COMISSAO,
			U_VALORCOMISSAO,
			U_CENTROCUSTO,
			U_PAGO,
			isnull(U_DATAPAGAMENTO, '1900-01-01') as U_DATAPAGAMENTO
		from #tmp_table_ret
	end

	begin --EXCLUSÃO DE TABELAS TEMPORÁRIAS
	drop table #tmp_table_fat
	drop table #tmp_table_ped
	drop table #tmp_table_rec
	drop table #tmp_table_ret
	drop table #tmp_table_where
	end
end
go
if object_id('dbo.spc_CVA_PagarComissao', 'P') is not null
	drop procedure [spc_CVA_PagarComissao]
go
create procedure dbo.spc_CVA_PagarComissao(@detalhado char(1) = 'Y', @vendedor int = 0, @dataInicial datetime, @dataFinal datetime)
as 
begin

	if @detalhado = 'Y'
		select 
			T0.U_COMISSIONADO,
			T0.U_CARDCODE,
			T0.U_CARDNAME,
			T0.U_REGRA,
			T0.U_DOCDATE,
			T0.U_DUEDATE,
			T0.U_OBJTYPE,
			T0.U_DOCENTRY,
			T0.U_LINENUM,
			T0.U_ITEMCODE,
			T0.U_ITEMNAME,
			T0.U_CENTROCUSTO,
			T0.U_VALOR,
			T0.U_PARCELA,
			T0.U_IMPOSTOS,
			T0.U_COMISSAO,
			T0.U_VALORCOMISSAO
		from [@CVA_CALC_COMISSAO] T0
		where T0.U_PAGO = 'N'
			and T0.U_DOCDATE >= @dataInicial 
			and T0.U_DOCDATE <= @dataFinal
			and (T0.U_COMISSAO = @vendedor or @vendedor = 0)
	else
		select
			T0.U_COMISSIONADO,
			T1.SlpName,
			sum(distinct T0.U_VALORCOMISSAO) as U_VALORCOMISSAO
		from [@CVA_CALC_COMISSAO] T0
			inner join OSLP T1 on T0.U_COMISSIONADO = T1.SlpCode
		where T0.U_PAGO = 'N'
			and T0.U_DOCDATE >= @dataInicial 
			and T0.U_DOCDATE <= @dataFinal
			and (T0.U_COMISSAO = @vendedor or @vendedor = 0)
		group by T0.U_COMISSIONADO, T1.SlpName
end