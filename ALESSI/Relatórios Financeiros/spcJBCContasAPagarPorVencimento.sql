USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[spcJBCContasAPagarPorVencimento]    Script Date: 06/09/2015 09:17:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCContasAPagarPorVencimento]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCContasAPagarPorVencimento]
GO

USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[spcJBCContasAPagarPorVencimento]    Script Date: 06/09/2015 09:17:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--select  'V' 'Tipo','Dt. Vencimento' 'Desc' union all select  'LC','Dt. Lançamento'


CREATE PROC [dbo].[spcJBCContasAPagarPorVencimento] (
	@CarCode nvarchar(15),
	@dateini date,
	@datefim date,
	@tpdata  varchar(2)
	, @Previsoes char(1)
	,@CVA_GRUPOECON  nvarchar(60)
)
with encryption
as 
begin


if @CarCode='*' begin
	set @CarCode=null
end
		set @datefim = dateadd(day,1,@datefim)
		  create table #TransacoesRecorrentes (
			TipoDoc  nvarchar(40) COLLATE SQL_Latin1_General_CP850_CI_AS  null
		  , Data     smalldatetime  null
		  , Lancamento     smalldatetime  null
		  , Tipo     nvarchar (30) COLLATE SQL_Latin1_General_CP850_CI_AS  null
		  , Tp       nvarchar (30) COLLATE SQL_Latin1_General_CP850_CI_AS  null
		  , DocEntry int  		    null
		  , CardCode varchar(30)    COLLATE SQL_Latin1_General_CP850_CI_AS null
		  , CardName varchar(200)  COLLATE SQL_Latin1_General_CP850_CI_AS null
		  , Parcela  int		    null
		  , ParcelaTotal  int		    null
		  , ValorParcela    decimal(19, 2) null
		  , Saldo    decimal(19, 2)
		  , slpcode int
		  , comments nvarchar(max) COLLATE SQL_Latin1_General_CP850_CI_AS
		  ,peyMethod nvarchar(max) COLLATE SQL_Latin1_General_CP850_CI_AS
		  )

		create table #LancamentosRecorrentes
		(
			TipoDoc		 nvarchar(40)	COLLATE SQL_Latin1_General_CP850_CI_AS null
		  , Data		 smalldatetime	null
		  , Tipo		 nvarchar (30)	COLLATE SQL_Latin1_General_CP850_CI_AS null
		  , Tp			 nvarchar (30)	COLLATE SQL_Latin1_General_CP850_CI_AS null
		  , DocEntry	 int  			null
		  , CardCode	 varchar(30)	COLLATE SQL_Latin1_General_CP850_CI_AS null 
		  , CardName	 varchar(200)	COLLATE SQL_Latin1_General_CP850_CI_AS null
		  , Parcela		 int			null
		  , ValorParcela decimal(19, 2) null
		  , Saldo		 decimal(19, 2) 

		)

if @Previsoes='S' begin
	--transações recorrentes



		declare @ModeloTrr nvarchar(30) ,@dataInicio date ,@PlanDate date ,@DataFinal date ,@Frequency char(10) /*,@objectType int */,@DraftEntry int , @DocObjType nvarchar(30)

		Declare @dtAtual date


		declare cpl cursor read_only for

		select 
			 --ORCP.AbsEntry
			 ORCP.Code 
			  ,ORCP.StartDate
			  --,ORCL.PlanDate
			  ,case	  
					when ORCP.Frequency = 'D' then --'Diário' 
						DATEADD(day,1,(select max(plandate)    from ORCL where  RcpEntry = ORCP.AbsEntry AND  Status = 'E'))
					when ORCP.Frequency = 'W' then --'Semanal' 
						DATEADD(WEEK,1,(select max(plandate)   from ORCL where  RcpEntry = ORCP.AbsEntry AND  Status = 'E'))
					when ORCP.Frequency = 'M' then --'Mensal' 
						DATEADD(MONTH,1,(select max(plandate)  from ORCL where  RcpEntry = ORCP.AbsEntry AND  Status = 'E'))
					when ORCP.Frequency = 'Q' then --'Trimestral'
						DATEADD(MONTH ,3,(select max(plandate) from ORCL where  RcpEntry = ORCP.AbsEntry AND  Status = 'E'))
					when ORCP.Frequency = 'S' then --'Semestral' 
						DATEADD(MONTH ,6,(select max(plandate) from ORCL where  RcpEntry = ORCP.AbsEntry AND  Status = 'E'))
					when ORCP.Frequency = 'A' then --'Anual' 
						DATEADD(YEAR ,1,(select max(plandate)  from ORCL where  RcpEntry = ORCP.AbsEntry AND  Status = 'E'))
					when ORCP.Frequency = 'O' then --'Uma Vez' 
						dateadd(day,1, (select max(plandate)   from ORCL where  RcpEntry = ORCP.AbsEntry AND  Status = 'E'))
			  end 'PlanDate'
			  --,(select max(plandate) from ORCL where  RcpEntry = ORCP.AbsEntry  AND  Status = 'E') 'PlanDate'
			  --,ORCP.EndDate
			  ,isnull(ORCP.EndDate, dateadd(year,1,@datefim)) 'EndDate'
			  ,ORCP.Frequency
			  --,DocEntry
			  ,ORCP.DraftEntry
			  ,ORCP.DocObjType
		from 
			ORCP

		where
			ORCP.IsRemoved<>'Y'
			and isnull(ORCP.EndDate, @datefim)>=getdate()
			--and isnull(ORCP.EndDate, @DtFinal)>=ORCP.StartDate
			--and ORCP.Code <>'joao'
		order by 2

		declare @contDraf as integer


		open cpl
 
		 fetch from cpl into @ModeloTrr ,@DataInicio ,@PlanDate ,@DataFinal ,@Frequency /*,@ObjectType*/ ,@DraftEntry,@DocObjType

		while @@fetch_status = 0 
 
		begin 
				set @contDraf=0		
				set @dtAtual=@PlanDate

				--print @dtAtual
				--print @DataFinal

				if @DataFinal>@datefim begin set @DataFinal=@datefim end
				while (@dtAtual<@datefim) begin
					set @contDraf=@contDraf+1
					--print '123'
					--print @DraftEntry

					--select * from ODRF where docentry=1
					insert into #TransacoesRecorrentes(tipoDoc,Data,Lancamento, Tipo, Tp,DocEntry,CardCode,CardName,Parcela,ParcelaTotal,ValorParcela,slpcode,comments,peyMethod) 
			
					select 

						'DRFP - ' +cast( @DraftEntry as nvarchar(10)) +' -> ' +@ModeloTrr  +' - ' +cast(@contDraf as nvarchar(50)) as 'TipoDoc'
						,
							case
								when ctg1.InstDays is null then 
									DATEADD(DAY,OCTG.ExtraDays, @dtAtual) 
								else
									DATEADD(DAY,ctg1.InstDays, @dtAtual) 
							end 
							'Vencimento'
							,ODRF.DocDate
							,case 
							when ODRF.ObjType = '13' then 'OINV' --select distinct objtype from OINV
							when ODRF.ObjType = '17' then 'ORDR' --select distinct objtype from ORDR
							when ODRF.ObjType = '18' then 'OPCH' --select distinct objtype from OPCH
							when ODRF.ObjType = '22' then 'OPOR' --select distinct objtype from OPOR
							else 'N/A '
						 end as 'Tipo'
						,case when ODRF.ObjType  = '13' then 'Nota Fiscal de Saída'
							  when ODRF.ObjType  = '18' then 'Nota Fiscal de Entrada'
							  when ODRF.ObjType  = '22' then 'Pedido de Compra'
							  when ODRF.ObjType  = '17' then 'Pedido de Venda'
						end as 'Tp'
						,odrf.docentry   as 'N° Doc'
						,OCRD.CardCode   as 'Cliente' 
						,OCRD.CardName   as 'Razão'
						,case when ctg1.IntsNo is null then 1 else ctg1.IntsNo end
						as 'Parcela'
						,case when ctg1.IntsNo is null then 1 else OCTG.InstNum end
						as 'Parcela'
						, case when ODRF.ObjType in ('22','18') then
							 case when ctg1.InstPrcnt is null then DocTotal*-1 else ((DocTotal/100)*ctg1.InstPrcnt)*-1 end 
						  else
							 case when ctg1.InstPrcnt is null then DocTotal else (DocTotal/100)*ctg1.InstPrcnt end 
						  end as 'ValorParcela'
						,ODRF.slpcode
						,ODRF.comments
						,ODRF.peyMethod
					from 
						ODRF
						inner join OCTG on OCTG.GroupNum=ODRF.GroupNum
						left JOIN  ctg1 ON ODRF.GroupNum=ctg1.CTGCode
						inner join OCRD on OCRD.CardCode=ODRF.CardCode 
						
					where 
						ODRF.DocEntry=@DraftEntry

						--and T0.U_CVA_GRUPOECON='002'
						--and DocTotal>0
				
					if @Frequency = 'D' begin --'Diário' 
						set @dtAtual= DATEADD(day,1,@dtAtual)
					end
					else if @Frequency = 'W' begin --'Semanal' 
						set @dtAtual= DATEADD(WEEK,1,@dtAtual)
					end
					else if @Frequency = 'M' begin --'Mensal' 
						set @dtAtual= DATEADD(MONTH,1,@dtAtual)
					end
					else if @Frequency = 'Q' begin --'Trimestral'
						set @dtAtual= DATEADD(MONTH ,3,@dtAtual)
					end
					else if @Frequency = 'S' begin --'Semestral' 
						set @dtAtual= DATEADD(MONTH ,6,@dtAtual)
					end
					else if @Frequency = 'A' begin --'Anual' 
						set @dtAtual= DATEADD(YEAR ,1,@dtAtual)
					end
					else if @Frequency = 'O' begin --'Uma Vez' 
						set @dtAtual=dateadd(day,1, @DataFinal)
					end
				end
	
				fetch next from cpl
				into @ModeloTrr ,@dataInicio ,@PlanDate ,@DataFinal ,@Frequency /*,@ObjectType*/ ,@DraftEntry,@DocObjType

		end 

		close cpl
		deallocate cpl 

		--insert into #NF_Fiscal([TransType],RefDate,ShortName,SourceLine,Saldo,BaseRef,[CardCode],[CardName],[BalDueDeb],[BalDueCred],slpcode)
		--insert into #ContasAPagarPorVencimento ( Origem, OrigemNr, Parcela, ShortName, CardName, Lancamento , Vencimento  )

		--select 
		--	T1.CardCode
		--	,T1.CardNAme
		--	,T0.Lancamento
		--	,T0.Data as Vencimento
		--	,T0.TipoDoc  +' - '+ T0.TP + ' - ' + cast(T0.DocEntry as nvarchar(30))
		--	,T0.DocEntry
		--	,T0.Parcela
		--	,T0.ParcelaTotal
		--	,null
		--	,comments
		--	,0
		--	,T0.ValorParcela 
		--	,T0.ValorParcela 
		--	,T0.Data
		--	,T0.peyMethod
		--from 
		--	#TransacoesRecorrentes T0
		--inner join OCRD T1 on T1.CardCode=T0.CardCode 
		--where data>=@dateini and data<=@datefim
		--and (T1.CardType='S') 
		--group by 
		--	T0.tipoDoc,T0.Data
		--	,T0.Tipo, T0.Tp,T0.DocEntry
		--	,T0.CardCode,T0.CardName,T0.Parcela,T0.ValorParcela
		--	,T0.Saldo,T1.CardCode,T1.CardNAme
		--	,T0.ValorParcela, T0.ParcelaTotal ,t0.slpcode
		--	,T0.Lancamento,T0.comments,T0.peyMethod

		--order by 2



	--update #NF_Fiscal set [DueDate]=[RefDate]
	--where [TransType] like 'DRF%'


	--lançamentos recorrentes

	--insert into #NF_Fiscal([TransType],RefDate,ShortName,SourceLine,Saldo,BaseRef,[CardCode],[CardName],[BalDueDeb],[BalDueCred])
	--exec SpcJBCFluxoCaixaLancamentosRecorrentes @dateini,@datefim,'C',@CVA_GRUPOECON,@slpCode



 
	 declare @RcurCode varchar(50) ,@RcurDesc varchar(72) ,@FrequencyLCTO varchar(30) ,@ProximaExecucao date ,@DataLimite date ,@Total decimal(19,4),@CardCodeLCTO nvarchar(30),@CardName nvarchar(200)

	 declare @dtAtualLCTO date

	 declare LRR cursor read_only for

 
	  select orcr.RcurCode 
			,orcr.RcurDesc 
			,orcr.Frequency
			,orcr.NextDeu   as 'ProximaExecução'
			,isnull(orcr.LimitDate,@datefim) as 'Data Limite'
			,case when CardType='S' then abs((rcr1.Debit-rcr1.Credit))*-1 else abs((rcr1.Debit-rcr1.Credit)) end as 'Total'
			,OCRD.CardCode
			,OCRD.CardName

		from
		   orcr
		   inner join rcr1 on rcr1.RcurCode=orcr.RcurCode
		   inner join OCRD on OCRD.CardCode=rcr1.AcctCode 
		   
		   where 
			orcr.Instance=0
	open LRR
 
	 fetch from LRR into @RcurCode,@RcurDesc,@FrequencyLCTO,@ProximaExecucao,@DataLimite,@Total,@CardCodeLCTO,@CardName
	 while @@fetch_status = 0 
	 begin
			--print @RcurCode

			set @dtAtualLCTO=@ProximaExecucao
			if @DataLimite>@datefim begin set @DataLimite=@datefim end


			--if (@RcurCode='Teste')begin
			--	print @dtAtualLCTO
			--	print @DataLimite
			--end

			while (@dtAtualLCTO<=@DataLimite) begin

	

		insert into #LancamentosRecorrentes
		select
			@RcurCode
			,@dtAtualLCTO
			,'LC'
			,'LCM'
			,0
			,@CardCodeLCTO
			,@CardName
			,1
			,@Total
			,null

				if @FrequencyLCTO = 'D' begin --'Diário' 
					set @dtAtualLCTO= DATEADD(day,1,@dtAtualLCTO)
				end
				else if @FrequencyLCTO = 'W' begin --'Semanal' 
					set @dtAtualLCTO= DATEADD(WEEK,1,@dtAtualLCTO)
				end
				else if @FrequencyLCTO = 'M' begin --'Mensal' 
					set @dtAtualLCTO= DATEADD(MONTH,1,@dtAtualLCTO)
				end
				else if @FrequencyLCTO = 'Q' begin --'Trimestral'
					set @dtAtualLCTO= DATEADD(MONTH ,3,@dtAtualLCTO)
				end
				else if @FrequencyLCTO = 'S' begin --'Semestral' 
					set @dtAtualLCTO= DATEADD(MONTH ,6,@dtAtualLCTO)
				end
				else if @FrequencyLCTO = 'A' begin --'Anual' 
					set @dtAtualLCTO= DATEADD(YEAR ,1,@dtAtualLCTO)
				end
				else if @FrequencyLCTO = 'O' begin --'Uma Vez' 
					set @dtAtualLCTO=dateadd(day,1, @DataLimite)
				end
			end
	fetch next from LRR
	into @RcurCode,@RcurDesc,@FrequencyLCTO,@ProximaExecucao,@DataLimite,@Total ,@CardCodeLCTO,@CardName

	end 

	close LRR
	deallocate LRR
end
--insert into #NF_Fiscal([TransType],RefDate,ShortName,SourceLine,Saldo,BaseRef,[CardCode],[CardName],[BalDueDeb],[BalDueCred])
--select	
--	T0.CardCode
--	,T0.CardName
--	,T0.Data Lancamento
--	,T0.Data Vencimento
--	,('LCMRecorrente' + T0.TipoDoc) as Origem
--	,null OrigemNR
--	,1 PArcela
--	,1 ParcelaToal
--	,null Serial
--	--,T0.CardCode
--	,null LineMemo
--	--,T0.Parcela
--	--,T0.ValorParcela
--	,0 Debit
--	--,T0.*

--	,T0.ValorParcela *-1 Credit
--	,T0.ValorParcela *-1 Saldo
--	,T0.Data DueDate
--	,null
--from 
--	#LancamentosRecorrentes T0
--	inner join OCRD T1 on T1.CardCode=T0.CardCode  --and (T1.U_CVA_GRUPOECON=@CVA_GRUPOECON or @CVA_GRUPOECON='*')
--where 
--	data>=@dateini and data<=@datefim --and 1=2
--	and (T1.CardType='S' )
--order by 2


--update #NF_Fiscal set [BalDueCred]=[BalDueCred]*-1,[DueDate]=[RefDate]
--where [TransType] like 'LCMRecorrente%'
set @datefim = dateadd(day,-1,@datefim)

select 
	tb.ShortName
	,tb.CardName
	,tb.Lancamento
	,tb.Vencimento
	,tb.Origem
	,tb.OrigemNr
	,tb.Parcela
	,tb.ParcelaTotal
	,tb.Serial
	,tb.LineMemo
	,tb.Debit
	,tb.Credit
	,tb.Saldo
	,tb.DueDate

	,case 
			when Origem='18'  then  OPCH.PeyMethod
			when Origem='204'  then  ODPO.PeyMethod	  
			when Origem='19' then  ORPC.PeyMethod
			--else 'N/A'
	end 'PeyMethodNF' 
 from (
			SELECT 	
				T0.[ShortName],
				--ocrd.CardFName,
				ocrd.CardName,
				T0.[RefDate] as Lancamento, 
				T0.[DueDate] as Vencimento,
				T0.[TransType] as Origem,
				T0.[CreatedBy] as OrigemNr,
				T0.[SourceLine] as Parcela,
				OPCH.Installmnt as ParcelaTotal,
				OPCH.Serial,
				T0.[LineMemo],
				T0.[Debit], T0.[Credit],
				(T0.[Debit] - T0.[Credit])*-1 as Saldo
				,T0.DueDate
			FROM  
				[dbo].[JDT1] T0   LEFT OUTER  JOIN [dbo].[OUSR] T1  ON  T0.[UserSign] = T1.[USERID]    
				LEFT OUTER  JOIN (
						SELECT 
							T0.[TransId] AS 'TransId', T0.[TransRowId] AS 'TransRowId', MAX(T0.[ReconNum]) AS 'MaxReconNum' 
						FROM  
							[dbo].[ITR1] T0  
						GROUP BY 
							T0.[TransId], T0.[TransRowId]
				) T2  ON  T0.[TransId] = T2.[TransId]  AND  T0.[Line_ID] = T2.[TransRowId]   
				inner join ocrd on ocrd.CardCode=T0.[ShortName] and ocrd.CardType='S'
				left join OPCH on opch.docentry=T0.[CreatedBy] and T0.[TransType]='18'
			WHERE 
				(
				(
					((T0.[DueDate] >= (@dateini) and T0.[DueDate] <=(@datefim))  and @tpData='V')
					and (
						((/*T0.[RefDate] >= (@dateIni) and */T0.[RefDate] <= ((CAST(CAST(GETDATE() AS DATE) AS DATETIME))   )) and @tpData='V' )
					)
				)

				or
				((T0.[RefDate] >= (@dateIni) and T0.[RefDate] <= (@datefim)) and @tpData='LC' )

				)

				AND (T0.[ShortName] = @CarCode or @CarCode is null)--ISNULL(@CarCode,T0.[ShortName])

				AND  (T0.[BalDueDeb] <> 0  OR  T0.[BalDueCred] <> 0 OR  T0.[BalFcDeb] <> 0  OR  T0.[BalFcCred] <> 0 ) 
	
	
				--AND  (T0.[RefDate] >= @LancamentoIni  AND  T0.[RefDate] <= @LancamentoFim)
				--and (T0.[DueDate] >=@VencimentoIni and T0.[DueDate] <=@VencimentoFim )	
) as tb	
    inner join OCRD on OCRD.CardCode=tb.ShortName
	left join OPCH on OPCH.DocEntry=OrigemNr and Origem='18'
	left join ODPO on ODPO.DocEntry=OrigemNr and Origem='204'
	left join ORPC on ODPO.DocEntry=OrigemNr and Origem='19'
where (OCRD.U_CVA_GRUPOECON=@CVA_GRUPOECON or @CVA_GRUPOECON='*')
union all
	select 
		T1.CardCode
		,T1.CardNAme
		,T0.Lancamento
		,T0.Data as Vencimento
		,T0.TipoDoc  +' - '+ T0.TP + ' - ' + cast(T0.DocEntry as nvarchar(30))
		,T0.DocEntry
		,T0.Parcela
		,T0.ParcelaTotal
		,null
		,comments
		,0
		--,T0.ValorParcela 
		,0
		,T0.ValorParcela 
		,T0.Data
		,T0.peyMethod
	from 
		#TransacoesRecorrentes T0
	inner join OCRD T1 on T1.CardCode=T0.CardCode 
	left join [@CVA_GRUPOECON] on [@CVA_GRUPOECON].code=T1.U_CVA_GRUPOECON
	where 
		data>=@dateini and data<=@datefim
		and (T1.CardType='S') 
		AND (T1.CardCode = @CarCode or @CarCode is null)
		and (T1.U_CVA_GRUPOECON=@CVA_GRUPOECON or @CVA_GRUPOECON='*')
	group by 
		T0.tipoDoc,T0.Data
		,T0.Tipo, T0.Tp,T0.DocEntry
		,T0.CardCode,T0.CardName,T0.Parcela,T0.ValorParcela
		,T0.Saldo,T1.CardCode,T1.CardNAme
		,T0.ValorParcela, T0.ParcelaTotal ,t0.slpcode
		,T0.Lancamento,T0.comments,T0.peyMethod

union all
select	
	T0.CardCode
	,T0.CardName
	,T0.Data Lancamento
	,T0.Data Vencimento
	,('LCMRecorrenteP' + T0.TipoDoc) as Origem
	,null OrigemNR
	,1 PArcela
	,1 ParcelaToal
	,null Serial
	--,T0.CardCode
	,null LineMemo
	--,T0.Parcela
	--,T0.ValorParcela
	,0 Debit
	--,T0.*

	,T0.ValorParcela *-1 Credit
	,T0.ValorParcela *-1 Saldo
	,T0.Data DueDate
	,null
from 
	#LancamentosRecorrentes T0
	inner join OCRD T1 on T1.CardCode=T0.CardCode  --and (T1.U_CVA_GRUPOECON=@CVA_GRUPOECON or @CVA_GRUPOECON='*')
	left join [@CVA_GRUPOECON] on [@CVA_GRUPOECON].code=T1.U_CVA_GRUPOECON
where 
	data>=@dateini and data<=@datefim --and 1=2
	and (T1.CardType='S' )
	AND (T1.CardCode = @CarCode or @CarCode is null)
	and (T1.U_CVA_GRUPOECON=@CVA_GRUPOECON or @CVA_GRUPOECON='*')

order by 2
--ORDER BY 
--	ShortName,DueDate asc

drop table #TransacoesRecorrentes
END

GO


execute spcJBCContasAPagarPorVencimento '*','2018-04-01','2018-04-15','V','S','*'