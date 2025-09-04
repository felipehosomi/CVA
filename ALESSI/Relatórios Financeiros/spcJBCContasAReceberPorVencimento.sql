USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[spcJBCContasAReceberPorVencimento]    Script Date: 06/09/2015 09:17:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCContasAReceberPorVencimento]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCContasAReceberPorVencimento]
GO

USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[spcJBCContasAReceberPorVencimento]    Script Date: 06/09/2015 09:17:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[spcJBCContasAReceberPorVencimento] (
	@CardCode varchar(60),
	@dateini smalldatetime,
	@datefim smalldatetime,
	@tpData varchar(2)
	,@BankCode	nvarchar(60)
	,@CVA_GRUPOECON  nvarchar(60)
	,@slpCode int
	, @Previsoes char(1)
	, @Resumo char(1)=N
)
with encryption
as 

Begin


if rtrim(ltrim(isnull(@CardCode, ''))) in ('', '*', '*Todos')
set @CardCode = null 

Create table #NF_Fiscal( 
     [TransId]      int
	,[Line_ID]		int
	,[Account]      nvarchar(300)
	,[ShortName]    nvarchar(300)
	,[TransType]    nvarchar(400)
	,[CreatedBy]    int
	,[BaseRef]      nvarchar(220)
	,[SourceLine]   smallint
	,[RefDate]      datetime
	,[DueDate]      datetime
	,[BalDueCred]	numeric (16, 9) 
	,[BalDueDeb]	numeric (16, 9) 
	,[BalDueCred_BalDueDeb] numeric (16, 9) 
    ,[Saldo]		numeric (16, 9) 
	,[LineMemo]	    nvarchar(1000)
	,[CardName]	    nvarchar(300)
	,[CardCode]	    nvarchar(300)
	,[Balance]	    numeric (19, 6)
	,[SlpCode]	    int
	,[DebitMAthCredit]numeric (19, 6) 
	,[IsSales]		nvarchar(20)
	,[Currency]		nvarchar(60)
	,[BPLName]	    nvarchar(200)
	)

	--transações recorrentes
--insert into #NF_Fiscal([TransType],RefDate,ShortName,SourceLine,Saldo,BaseRef,[CardCode],[CardName],[BalDueDeb],[BalDueCred],slpcode)
--exec SpcJBCFluxoCaixaTransacoesRecorrentes @dateini,@datefim,'C',@CVA_GRUPOECON,@slpCode
if @Previsoes='S' begin

		set @datefim = dateadd(day,1,@datefim)
		  create table #TransacoesRecorrentes (
			TipoDoc  nvarchar(40)   null
		  , Data     smalldatetime  null
		  , Tipo     nvarchar (30)  null
		  , Tp       nvarchar (30)  null
		  , DocEntry int  		    null
		  , CardCode varchar(30)    COLLATE SQL_Latin1_General_CP850_CI_AS null
		  , CardName varchar(200)   null
		  , Parcela  int		    null
		  , ParcelaTotal  int		    null
		  , ValorParcela    decimal(19, 2) null
		  , Saldo    decimal(19, 2)
		  , slpcode int
		  )


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
			and isnull(ORCP.EndDate, @datefim)>=cast(getdate() as date)
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
					insert into #TransacoesRecorrentes(tipoDoc,Data,Tipo, Tp,DocEntry,CardCode,CardName,Parcela,ParcelaTotal,ValorParcela,slpcode) 
			
					select 

						'DRFV - ' +cast( @DraftEntry as nvarchar(10)) +' -> ' +@ModeloTrr  +' - ' +cast(@contDraf as nvarchar(50)) as 'TipoDoc'
						,
							case
								when ctg1.InstDays is null then 
									DATEADD(DAY,OCTG.ExtraDays, @dtAtual) 
								else
									DATEADD(DAY,ctg1.InstDays, @dtAtual) 
							end 
							'Vencimento'

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
					from 
						ODRF
						inner join OCTG on OCTG.GroupNum=ODRF.GroupNum
						left JOIN  ctg1 ON ODRF.GroupNum=ctg1.CTGCode
						inner join OCRD on OCRD.CardCode=ODRF.CardCode and (OCRD.U_CVA_GRUPOECON=@CVA_GRUPOECON or @CVA_GRUPOECON='*')
					where 
						ODRF.DocEntry=@DraftEntry
						and (ODRF.SlpCode=@slpCode or @slpCode =0)
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

		insert into #NF_Fiscal([TransType],RefDate,ShortName,SourceLine,Saldo,BaseRef,[CardCode],[CardName],[BalDueDeb],[BalDueCred],slpcode)
		select 
			T0.TipoDoc  +' - '+ T0.TP + ' - ' + cast(T0.DocEntry as nvarchar(30))
			,T0.Data
			,T0.CardCode
			--,cast(T0.Parcela as nvarchar(30))+ '/' + cast(T0.ParcelaTotal as nvarchar(30))
			,T0.Parcela
			,T0.ValorParcela 
			,T0.DocEntry
			,T1.CardCode
			,T1.CardNAme
			,T0.ValorParcela 
			,T0.ValorParcela *-1
			,T0.slpcode
			--,T0.ParcelaTotal
		from 
			#TransacoesRecorrentes T0
		inner join OCRD T1 on T1.CardCode=T0.CardCode 
		where data>=@dateini and data<=@datefim
		and (T1.CardType='C') and (T1.U_CVA_GRUPOECON=@CVA_GRUPOECON or @CVA_GRUPOECON='*')
		group by T0.tipoDoc,T0.Data,T0.Tipo, T0.Tp,T0.DocEntry,T0.CardCode,T0.CardName,T0.Parcela,T0.ValorParcela,T0.Saldo,T1.CardCode,T1.CardNAme,T0.ValorParcela, T0.ParcelaTotal ,t0.slpcode

		--order by 2
		drop table #TransacoesRecorrentes


	update #NF_Fiscal set [DueDate]=[RefDate]
	where [TransType] like 'DRF%'

		--lançamentos recorrentes

	--insert into #NF_Fiscal([TransType],RefDate,ShortName,SourceLine,Saldo,BaseRef,[CardCode],[CardName],[BalDueDeb],[BalDueCred])
	--exec SpcJBCFluxoCaixaLancamentosRecorrentes @dateini,@datefim,'C',@CVA_GRUPOECON,@slpCode

		create table #LancamentosRecorrentes
		(
			TipoDoc		 nvarchar(40)	null
		  , Data		 smalldatetime	null
		  , Tipo		 nvarchar (30)	null
		  , Tp			 nvarchar (30)	null
		  , DocEntry	 int  			null
		  , CardCode	 varchar(30)	COLLATE SQL_Latin1_General_CP850_CI_AS null 
		  , CardName	 varchar(200)	null
		  , Parcela		 int			null
		  , ValorParcela decimal(19, 2) null
		  , Saldo		 decimal(19, 2) 

		)

 
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
		   inner join OCRD on OCRD.CardCode=rcr1.AcctCode  and (OCRD.U_CVA_GRUPOECON=@CVA_GRUPOECON or @CVA_GRUPOECON='*') and (OCRD.SlpCode=@slpCode or @slpCode =0)
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

	insert into #NF_Fiscal([TransType],RefDate,ShortName,SourceLine,Saldo,BaseRef,[CardCode],[CardName],[BalDueDeb],[BalDueCred])
	select	
		'LCMRecorrenteV' + T0.TipoDoc
		,T0.Data
		,T0.CardCode
		,T0.Parcela
		,T0.ValorParcela
		,0
		--,T0.*
		,T0.CardCode
		,T0.CardName
		,T0.ValorParcela
		,T0.ValorParcela
	from 
		#LancamentosRecorrentes T0
		inner join OCRD T1 on T1.CardCode=T0.CardCode  and (T1.U_CVA_GRUPOECON=@CVA_GRUPOECON or @CVA_GRUPOECON='*')
	where 
		data>=@dateini and data<=@datefim --and 1=2
		and (T1.CardType='C' )
	order by 2


	update #NF_Fiscal set [BalDueCred]=[BalDueCred]*-1,[DueDate]=[RefDate]
	where [TransType] like 'LCMRecorrente%'
	set @datefim = dateadd(day,-1,@datefim)
end


--CHEQUES
insert into #NF_Fiscal([TransType],RefDate        ,ShortName    ,SourceLine,Saldo        ,BaseRef  ,[CardCode]   ,[CardName]   ,[BalDueDeb],[BalDueCred],DueDate)
select  distinct       'Cheque'   ,OCHH.CheckDate ,OCRD.CardCode,0         ,OCHH.CheckSum,checkkey ,OCRD.CardCode,OCRD.CardName,OCHH.CheckSum*-1,OCHH.CheckSum*-1,OJDT.DueDate
	--,OCHH.*  
from 
	OCHH 
	inner join RCT1 on OCHH.CheckKey = RCT1.CheckAbs
	INNER JOIN ORCT ON RCT1.DocNum = ORCT.DocEntry
	INNER JOIN RCT2 ON RCT2.DocNum = ORCT.DocEntry 
	Inner Join OCRD on OCRD.CardCode=OCHH.CardCode and (OCRD.U_CVA_GRUPOECON=@CVA_GRUPOECON or @CVA_GRUPOECON='*')
	inner join OJDT on  OJDT.TransType=24 and OJDT.baseref=ORCT.DocEntry
where 
	Deposited='N' AND
	(
		(OCHH.CheckDate>=@dateini and OCHH.CheckDate<=@datefim and @tpData='V')
		or (OJDT.DueDate>=@dateini and OJDT.DueDate<=@datefim and @tpData='LC')
	)
	and (OCRD.CardCode=@CardCode or (ISNULL(@CardCode,'0')='0'))
	--inner join OCRD T1 on T1.CardCode=T0.CardCode  and (T1.U_CVA_GRUPOECON=@CVA_GRUPOECON or @CVA_GRUPOECON='*')
	--select @CardCode

insert into #NF_Fiscal

SELECT 
	T0.[TransId], 
	T0.[Line_ID], 
	MAX(T0.[Account]) as 'Account' ,
	MAX(T0.[ShortName])  as 'ShortName', 
	MAX(T0.[TransType]) as 'TransType', 
	MAX(T0.[CreatedBy]) as 'CreatedBy',
	MAX(T0.[BaseRef]) as 'BaseRef',
	MAX(T0.[SourceLine]) as 'SourceLine', 
	MAX(T0.[RefDate]) as 'RefDate', 
	MAX(T0.[DueDate]) as 'DueDate', 
	
    MAX(T0.[BalDueCred]) AS'BalDueCred',
    SUM(T1.[ReconSum]) AS 'BalDueDeb',
    MAX(T0.[BalDueCred]) + SUM(T1.[ReconSum]) AS 'BalDueCred - BalDueDeb',
    ( MAX(T0.[BalDueCred]) + SUM(T1.[ReconSum]) ) * (-1) AS 'Saldo',    

	MAX(T0.[LineMemo])as 'LineMemo', 
	MAX(T4.[CardName]) as 'CardName', 
	MAX(T5.[CardCode])as 'CardCode', 

	MAX(T4.[Balance])as 'Balance', 
	MAX(T5.[SlpCode])as 'SlpCode', 

	MAX(T0.[Debit]) + MAX(T0.[Credit]) as 'Debit + Credit', 
	MAX(T5.[IsSales]) as 'IsSales', 
	MAX(T4.[Currency]) as 'Currency', 
	T0.[BPLName] as 'BPLName'
	
FROM  
	[dbo].[JDT1] T0  
	INNER  JOIN [dbo].[ITR1] T1  ON  T1.[TransId] = T0.[TransId]  
		AND  T1.[TransRowId] = T0.[Line_ID]   
	INNER  JOIN [dbo].[OITR] T2  ON  T2.[ReconNum] = T1.[ReconNum]   
	INNER  JOIN [dbo].[OJDT] T3  ON  T3.[TransId] = T0.[TransId]   
	INNER  JOIN [dbo].[OCRD] T4  ON  T4.[CardCode] = T0.[ShortName] and (T4.U_CVA_GRUPOECON=@CVA_GRUPOECON or @CVA_GRUPOECON='*')
	LEFT OUTER  JOIN [dbo].[B1_JournalTransSourceView] T5  ON  T5.[ObjType] = T0.[TransType]  
		AND  T5.[DocEntry] = T0.[CreatedBy]  
		AND  (T5.[TransType] <> 'I'  OR  (T5.[TransType] = 'I'  
		AND  T5.[InstlmntID] = T0.[SourceLine] ))  
WHERE 
	(
		((T0.[RefDate] <= (@DateFim)  AND  T0.[RefDate] >= (@DateIni)  AND  T0.[RefDate] <= (@DateFim)  
		AND  T2.[ReconDate] > (@DateFim)  )  and @tpData='LC')

		or (
				((T0.[DueDate] >= (@DateIni)  AND  T0.[DueDate] <= (@DateFim)  )  and @tpData='V' )
				and
				(
					((T0.[RefDate] <= (SELECT CAST(CAST(GETDATE() AS DATE) AS DATETIME))  /*AND  T0.[RefDate] >= (@DateIni) */ AND  T0.[RefDate] <= (SELECT CAST(CAST(GETDATE() AS DATE) AS DATETIME))  
					AND  T2.[ReconDate] > (SELECT CAST(CAST(GETDATE() AS DATE) AS DATETIME))  )  and @tpData='V')--JBC
				)
			)
	)
	
	AND  T4.[CardType] = ('C')  AND  T4.[Balance] <> (0)  
	
	

	AND  T1.[IsCredit] = ('C')   

	AND  ((T4.[CardCode] = (@CardCode)  ) OR (ISNULL(@CardCode,'0')='0')  ) 
	



GROUP BY 
	T0.[TransId], T0.[Line_ID], T0.[BPLName] 
HAVING 
	MAX(T0.[BalFcCred]) <>- SUM(T1.[ReconSumFC])  OR  MAX(T0.[BalDueCred]) <>- SUM(T1.[ReconSum])   
UNION ALL 
SELECT 
	T0.[TransId], 
	T0.[Line_ID], 
	MAX(T0.[Account]), 
	MAX(T0.[ShortName]), 
	MAX(T0.[TransType]), 
	MAX(T0.[CreatedBy]), 
	MAX(T0.[BaseRef]), 
	MAX(T0.[SourceLine]), 
	MAX(T0.[RefDate]), 
	MAX(T0.[DueDate]), 
	
    MAX(T0.[BalDueCred]) AS'BalDueCred',
    SUM(T1.[ReconSum]) AS 'BalDueDeb',
    MAX(T0.[BalDueCred]) + SUM(T1.[ReconSum]) AS 'BalDueCred - BalDueDeb',
    ( MAX(T0.[BalDueCred]) + SUM(T1.[ReconSum]) ) * (-1) AS 'Saldo',
    	
	MAX(T0.[LineMemo]), 
	MAX(T4.[CardName]), 
	MAX(T5.[CardCode]), 

	MAX(T4.[Balance]), 
	MAX(T5.[SlpCode]), 
	MAX(T0.[Debit]) + MAX(T0.[Credit]), 
	MAX(T5.[IsSales]), 
	MAX(T4.[Currency]), 
	T0.[BPLName] 
FROM  
	[dbo].[JDT1] T0  
	INNER  JOIN [dbo].[ITR1] T1  ON  T1.[TransId] = T0.[TransId]  AND  T1.[TransRowId] = T0.[Line_ID]   
	INNER  JOIN [dbo].[OITR] T2  ON  T2.[ReconNum] = T1.[ReconNum]   
	INNER  JOIN [dbo].[OJDT] T3  ON  T3.[TransId] = T0.[TransId]   
	INNER  JOIN [dbo].[OCRD] T4  ON  T4.[CardCode] = T0.[ShortName]  and (T4.U_CVA_GRUPOECON=@CVA_GRUPOECON or @CVA_GRUPOECON='*')
	LEFT OUTER  JOIN [dbo].[B1_JournalTransSourceView] T5  ON  T5.[ObjType] = T0.[TransType]  
		AND  T5.[DocEntry] = T0.[CreatedBy]  
		AND  (T5.[TransType] <> 'I'  OR  (T5.[TransType] = 'I'  AND  T5.[InstlmntID] = T0.[SourceLine] ))  
WHERE 
	(
		((T0.[RefDate] <= (@DateFim)  AND  T0.[RefDate] >= (@DateIni)  AND  T0.[RefDate] <= (@DateFim)  
		AND  T2.[ReconDate] > (@DateFim)  ) and @tpData='LC' )

		or (
			((T0.[DueDate] >= (@DateIni)  AND  T0.[DueDate] <= (@DateFim)  ) and @tpData='V') 
			and
			(
				((T0.[RefDate] <= (CAST(CAST(GETDATE() AS DATE) AS DATETIME))  /*AND  T0.[RefDate] >= (@DateIni)*/  AND  T0.[RefDate] <= (CAST(CAST(GETDATE() AS DATE) AS DATETIME))  
				AND  T2.[ReconDate] > (CAST(CAST(GETDATE() AS DATE) AS DATETIME))  ) and @tpData='V' )
			)
		)
	)

	AND  T4.[CardType] = ('C')  AND  T4.[Balance] <> (0)  
	AND  ( (T4.[CardCode] = (@CardCode)  ) OR (ISNULL(@CardCode,'0')='0') )
	AND  T1.[IsCredit] = ('D')   
GROUP BY
	T0.[TransId], T0.[Line_ID], T0.[BPLName] 
HAVING 
	MAX(T0.[BalFcDeb]) <>- SUM(T1.[ReconSumFC])  OR  MAX(T0.[BalDueDeb]) <>- SUM(T1.[ReconSum])   
UNION ALL 
SELECT --1 as Query
	T0.[TransId], 
	T0.[Line_ID], 
	MAX(T0.[Account]), 
	MAX(T0.[ShortName]), 
	MAX(T0.[TransType]), 
	MAX(T0.[CreatedBy]), 
	MAX(T0.[BaseRef]), 
	MAX(T0.[SourceLine]), 
	MAX(T0.[RefDate]), 
	MAX(T0.[DueDate]), 
	
    MAX(t0.balduecred) as 'BalDueCred',
    MAX(t0.BalDueDeb)  as 'BalDueDeb',
    MAX(T0.[BalDueCred]) - MAX(T0.[BalDueDeb]) as 'BalDueCred - BalDueDeb',
   ( MAX(T0.[BalDueCred]) - MAX(T0.[BalDueDeb]) ) * -1 as 'Saldo'	,
	
	MAX(T0.[LineMemo]), 
	MAX(T2.[CardName]), 
	MAX(T2.[CardCode]), 
	MAX(T2.[Balance]), 
	MAX(T3.[SlpCode]), 
	MAX(T0.[Debit]) + MAX(T0.[Credit]), 
	MAX(T3.[IsSales]), 
	MAX(T2.[Currency]), 
	T0.[BPLName] 
FROM  
	[dbo].[JDT1] T0  
	INNER  JOIN [dbo].[OJDT] T1  ON  T1.[TransId] = T0.[TransId]   
	INNER  JOIN [dbo].[OCRD] T2  ON  T2.[CardCode] = T0.[ShortName]    and (T2.U_CVA_GRUPOECON=@CVA_GRUPOECON or @CVA_GRUPOECON='*')
	LEFT OUTER  JOIN [dbo].[B1_JournalTransSourceView] T3  ON  T3.[ObjType] = T0.[TransType]  
		AND  T3.[DocEntry] = T0.[CreatedBy]  AND  (T3.[TransType] <> 'I'  
		OR  (T3.[TransType] = 'I'  AND  T3.[InstlmntID] = T0.[SourceLine] ))  
WHERE
	( 
		((T0.[RefDate] <= (@DateFim)  AND  T0.[RefDate] >= (@DateIni)  AND  T0.[RefDate] <= (@DateFim)  

		AND   
			NOT EXISTS (
				SELECT U0.[TransId], U0.[TransRowId] 
				FROM  [dbo].[ITR1] U0  
					INNER  JOIN [dbo].[OITR] U1  ON  U1.[ReconNum] = U0.[ReconNum]   
				WHERE 
					T0.[TransId] = U0.[TransId]  AND  T0.[Line_ID] = U0.[TransRowId]  AND  U1.[ReconDate] > (@DateFim)   
				GROUP BY 
					U0.[TransId], U0.[TransRowId])

		) and @tpData='LC')


		or 
			(
				(T0.[DueDate] >= (@DateIni)  AND  T0.[DueDate] <= (@DateFim)  and @tpData='V' 	)
				and
				(
					((T0.[RefDate] <= (CAST(CAST(GETDATE() AS DATE) AS DATETIME))  /*AND  T0.[RefDate] >= (@DateIni)*/  AND  T0.[RefDate] <= (CAST(CAST(GETDATE() AS DATE) AS DATETIME))  

					AND   
						NOT EXISTS (
							SELECT U0.[TransId], U0.[TransRowId] 
							FROM  [dbo].[ITR1] U0  
								INNER  JOIN [dbo].[OITR] U1  ON  U1.[ReconNum] = U0.[ReconNum]   
							WHERE 
								T0.[TransId] = U0.[TransId]  AND  T0.[Line_ID] = U0.[TransRowId]  AND  U1.[ReconDate] > (CAST(CAST(GETDATE() AS DATE) AS DATETIME))   
							GROUP BY 
								U0.[TransId], U0.[TransRowId])

					) and @tpData='V')
				)
			)


			--CAST(CAST(GETDATE() AS DATE) AS DATETIME)
	)
	AND  T2.[CardType] = ('C')  AND  T2.[Balance] <> (0)  
	AND  ((T2.[CardCode] = (@CardCode)  ) OR (ISNULL(@CardCode,'0')='0'))
	AND  (T0.[BalDueCred] <> T0.[BalDueDeb]  
	OR  T0.[BalFcCred] <> T0.[BalFcDeb] ) 
GROUP BY 
	T0.[TransId], T0.[Line_ID], T0.[BPLName]
	
end 

if @Resumo='N' begin
	--select distinct TransType from #NF_Fiscal
	--203 ODPI
	--182 OBOE
	--13  OINV
	--24  ORCT
	--14  IRIN
	--30  OJDT
	select 	
		TransId     
		,Line_ID     
		,Account                                                                                                                                                                                                                                                          
		,ShortName                                                                                                                                                                                                                                                        
		,TransType                                                                                                                                                                                                                                                        
		,CreatedBy   
		,BaseRef                                                                                                                                                                                                                      
		,SourceLine 
		,RefDate                 
		,DueDate                 
		,BalDueCred                              
		,BalDueDeb                               
		,BalDueCred_BalDueDeb                    
		,Saldo                                   
		,LineMemo
		,CardName                                                                                                                                                                                                                                                         
		,CardCode                                                                                                                                                                                                                                                         
		,Balance                                 
		,SlpCode     
		,DebitMAthCredit                         
		,IsSales              
		,Currency                                                     
		,BPLName                                                                                                                                                                                                  
		,Serial
		,case when FormaPagamento='' then PeyMethodNF else FormaPagamento end 'FormaPagamento'
		,PeyMethodNF   		

		,case 
			when SUBSTRING ( TransType ,0 , 4) <> 'DRF'  then null
			when coalesce(ODSC.BankName,'')<>'' then ODSC.BankCode --ODSC.BankName
			else case  when TransType='24' and  FormaPagamento='Boleto'  then TB.BankCode else (select top 1 BnkDflt  from OPYM where PayMethCod=PeyMethodNF )  end 
		
		end	'BancoNF'
		,Installmnt	
		,OrctComments
		--,ODSC.BankName
		,case 
			when SUBSTRING ( TransType ,0 , 4) <> 'DRF'  then null
			when coalesce(ODSC.BankCode,'')<>'' then cast(ODSC.BankCode AS nvarchar(30))
			--ODSC.BankCode
			else case  when TransType='24' and  FormaPagamento='Boleto'  then TB.BankCode else (select top 1 BnkDflt  from OPYM where PayMethCod=PeyMethodNF )  end 
		
		end	'BankName'
		,DocEntryNFS

		,DATEDIFF ( day , DueDAte , cast(getdate()as date) ) as Dias
	from (
		 select 
			#NF_Fiscal.[TransId]
			,#NF_Fiscal.[Line_ID]
			,#NF_Fiscal.[Account]
			,#NF_Fiscal.[ShortName]
			,#NF_Fiscal.[TransType]
			,#NF_Fiscal.[CreatedBy]
			,#NF_Fiscal.[BaseRef]
			, case when #NF_Fiscal.[SourceLine]<0 then 0 else #NF_Fiscal.[SourceLine] end SourceLine
			,#NF_Fiscal.[RefDate]
			,#NF_Fiscal.[DueDate]
			,#NF_Fiscal.[BalDueCred]
			,#NF_Fiscal.[BalDueDeb]
			,#NF_Fiscal.[BalDueCred_BalDueDeb]
			,#NF_Fiscal.[Saldo]
			,#NF_Fiscal.[LineMemo]
			,#NF_Fiscal.[CardName]
			,#NF_Fiscal.[CardCode]
			,#NF_Fiscal.[Balance]
			,#NF_Fiscal.[SlpCode]
			,#NF_Fiscal.[DebitMAthCredit]
			,#NF_Fiscal.[IsSales]
			,#NF_Fiscal.[Currency]
			,#NF_Fiscal.[BPLName]
	
			,case when #NF_Fiscal.TransType = '13'  then  OINV.Serial
				  when #NF_Fiscal.TransType = '30'  then  OJDT.Serial
				  when #NF_Fiscal.TransType = '14'  then  ORIN.Serial
				  --when #NF_Fiscal.TransType = '24'  then  ORCT.Serial
				  when #NF_Fiscal.TransType = '203' then  ODPI.Serial
				  when #NF_Fiscal.TransType = '24'  then  (select top 1 serial from OINV where OINV.docentry in (select top 1 RCT2.baseAbs  from RCT2 where RCT2.invtype='13' and RCT2.DocNum=#NF_Fiscal.[BaseRef]))

				  --select top RCT2.baseAbs  from RCT2 where RCT2.invtype='13' and RCT2.DocNum=#NF_Fiscal.[BaseRef]
				  --else 321
				  --else 'N/A'
			end 'Serial' 
			, case when #NF_Fiscal.TransType = '24'  then  
					case 
						when orct.CashSum   > 0 then 'dinheiro'
						when orct.BoeSum    > 0 then 'Boleto'
						when orct.CheckSum  > 0 then 'Cheque'
						when orct.CreditSum > 0 then 'Cartão de Crédito'
						when orct.TrsfrSum  > 0 then 'Tranferência Bancária'
						else ''
					end
				else ''
			end 'FormaPagamento'
	
			,case 
				  when #NF_Fiscal.TransType = '13'  then  OINV.PeyMethod
				  when #NF_Fiscal.TransType = '14'  then  ORIN.PeyMethod	  
				  when #NF_Fiscal.TransType = '203' then  ODPI.PeyMethod
				  when SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) = 'DRF' then ODRF.PeyMethod
			  
				  else ''
			end 'PeyMethodNF' 
			,case 
				  when #NF_Fiscal.TransType = '13'  then  OINV.Installmnt
				  when #NF_Fiscal.TransType = '14'  then  ORIN.Installmnt
				  when #NF_Fiscal.TransType = '203' then  ODPI.Installmnt
				  when SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) = 'DRF' then ODRF.Installmnt
				  else null
			end 'Installmnt '
			,coalesce(orct.Comments,ODRF.comments) 'OrctComments'
			,ODSC.BankName
			,ODSC.BankCode
			,case
				when #NF_Fiscal.TransType = '24'  then  (select top 1 RCT2.baseAbs  from RCT2 where RCT2.invtype='13' and RCT2.DocNum=#NF_Fiscal.[BaseRef])
				else 0
			end 'DocEntryNFS'
		  from 
			#NF_Fiscal
			left join OINV on OINV.DocEntry = #NF_Fiscal.BaseRef and #NF_Fiscal.TransType='13'  and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) <> 'DRF' 
			left join OJDT on OJDT.TransId  = #NF_Fiscal.BaseRef and #NF_Fiscal.TransType='30'  and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) <> 'DRF' 
			left join ORIN on ORIN.Docentry = #NF_Fiscal.BaseRef and #NF_Fiscal.TransType='14' 	 and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) <> 'DRF' 
		
			left join ORCT on ORCT.DocEntry = #NF_Fiscal.BaseRef and #NF_Fiscal.TransType='24' and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) <> 'DRF' 
			left join OBOE on OBOE.boenum =ORCT.boenum and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) <> 'DRF' 
			left join ODSC on ODSC.BankCode=OBOE.BPBankCod and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) <> 'DRF' 

			left join ODPI on ODPI.DocEntry = #NF_Fiscal.BaseRef and #NF_Fiscal.TransType='203' and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) <> 'DRF' 
			left join ODRF on ODRF.DocEntry = #NF_Fiscal.BaseRef and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) = 'DRF' 

	) TB	
		left join ODSC on ODSC.BankCode=(select top 1 BnkDflt  from OPYM where PayMethCod=PeyMethodNF )  and SUBSTRING ( TransType ,0 , 4) <> 'DRF' 
		where (@BankCode=(select top 1 BnkDflt  from OPYM where PayMethCod=PeyMethodNF )) or isnull(@BankCode, '*')='*'
		and (@slpCode=0 or TB.SlpCode=@slpCode)

		and
		(
			(TB.DueDate>=@dateini and TB.DueDate<=@datefim and @tpData='V')
			or (TB.RefDate>=@dateini and TB.RefDate<=@datefim and @tpData='LC')
		)
	order by 9	  
end else begin
	select descricao,sum(Valor) as Valor,ORdem from (
						select 
							case 
								when BalDueCred <> 0 then BalDueCred *-1
								when BalDueDeb <> 0 then BalDueDeb
							end Valor
							,case
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) <0 then 'Em Atraso'
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) <=30 and ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) >=0 then '0 a 30'
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) >=31 and ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) <=60 then '31 a 60'
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) >=61 and ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) <=90 then '61 a 90'
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) >=91  and ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) <=180 then '91 a 180'
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) >=181   and ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) <=360 then '181 a 360'
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) >=361   and ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) <=720 then '361 a 720'
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) >=721  then '721'
							end as descricao
							,case
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) <0 then 0
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) <=30 and ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) >=0 then 1
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) >=31 and ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) <=60 then 2
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) >=61 and ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) <=90 then 3
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) >=91  and ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) <=180 then 4
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) >=181   and ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) <=360 then 5
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) >=361   and ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) <=720 then 6
								when ( DATEDIFF ( day , DueDAte , cast(getdate()as date) ) *-1) >=721  then '721'
							end as ORdem
							--,( DATEDIFF ( day , RefDate , cast(getdate()as date) ) *-1)as Dias
						from (
							 select 
								#NF_Fiscal.[TransId]
								,#NF_Fiscal.[Line_ID]
								,#NF_Fiscal.[Account]
								,#NF_Fiscal.[ShortName]
								,#NF_Fiscal.[TransType]
								,#NF_Fiscal.[CreatedBy]
								,#NF_Fiscal.[BaseRef]
								, case when #NF_Fiscal.[SourceLine]<0 then 0 else #NF_Fiscal.[SourceLine] end SourceLine
								,#NF_Fiscal.[RefDate]
								,#NF_Fiscal.[DueDate]
								,#NF_Fiscal.[BalDueCred]
								,#NF_Fiscal.[BalDueDeb]
								,#NF_Fiscal.[BalDueCred_BalDueDeb]
								,#NF_Fiscal.[Saldo]
								,#NF_Fiscal.[LineMemo]
								,#NF_Fiscal.[CardName]
								,#NF_Fiscal.[CardCode]
								,#NF_Fiscal.[Balance]
								,#NF_Fiscal.[SlpCode]
								,#NF_Fiscal.[DebitMAthCredit]
								,#NF_Fiscal.[IsSales]
								,#NF_Fiscal.[Currency]
								,#NF_Fiscal.[BPLName]
	
								,case when #NF_Fiscal.TransType = '13'  then  OINV.Serial
									  when #NF_Fiscal.TransType = '30'  then  OJDT.Serial
									  when #NF_Fiscal.TransType = '14'  then  ORIN.Serial
									  --when #NF_Fiscal.TransType = '24'  then  ORCT.Serial
									  when #NF_Fiscal.TransType = '203' then  ODPI.Serial
									  when #NF_Fiscal.TransType = '24'  then  (select top 1 serial from OINV where OINV.docentry in (select top 1 RCT2.baseAbs  from RCT2 where RCT2.invtype='13' and RCT2.DocNum=#NF_Fiscal.[BaseRef]))
									  --select top RCT2.baseAbs  from RCT2 where RCT2.invtype='13' and RCT2.DocNum=#NF_Fiscal.[BaseRef]
									  --else 321
									  --else 'N/A'
								end 'Serial' 
								, case when #NF_Fiscal.TransType = '24'  then  
										case 
											when orct.CashSum   > 0 then 'dinheiro'
											when orct.BoeSum    > 0 then 'Boleto'
											when orct.CheckSum  > 0 then 'Cheque'
											when orct.CreditSum > 0 then 'Cartão de Crédito'
											when orct.TrsfrSum  > 0 then 'Tranferência Bancária'
											else ''
										end
									else ''
								end 'FormaPagamento'
	
								,case 
									  when #NF_Fiscal.TransType = '13'  then  OINV.PeyMethod
									  when #NF_Fiscal.TransType = '14'  then  ORIN.PeyMethod	  
									  when #NF_Fiscal.TransType = '203' then  ODPI.PeyMethod
									  when SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) = 'DRF' then ODRF.PeyMethod
			  
									  else ''
								end 'PeyMethodNF' 
								,case 
									  when #NF_Fiscal.TransType = '13'  then  OINV.Installmnt
									  when #NF_Fiscal.TransType = '14'  then  ORIN.Installmnt
									  when #NF_Fiscal.TransType = '203' then  ODPI.Installmnt
									  when SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) = 'DRF' then ODRF.Installmnt
									  else null
								end 'Installmnt '
								,coalesce(orct.Comments,ODRF.comments) 'OrctComments'
								,ODSC.BankName
								,ODSC.BankCode
								,case
									when #NF_Fiscal.TransType = '24'  then  (select top 1 RCT2.baseAbs  from RCT2 where RCT2.invtype='13' and RCT2.DocNum=#NF_Fiscal.[BaseRef])
									else 0
								end 'DocEntryNFS'
							  from 
								#NF_Fiscal
								left join OINV on OINV.DocEntry = #NF_Fiscal.BaseRef and #NF_Fiscal.TransType='13'  and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) <> 'DRF' 
								left join OJDT on OJDT.TransId  = #NF_Fiscal.BaseRef and #NF_Fiscal.TransType='30'  and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) <> 'DRF' 
								left join ORIN on ORIN.Docentry = #NF_Fiscal.BaseRef and #NF_Fiscal.TransType='14' 	 and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) <> 'DRF' 
		
								left join ORCT on ORCT.DocEntry = #NF_Fiscal.BaseRef and #NF_Fiscal.TransType='24' and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) <> 'DRF' 
								left join OBOE on OBOE.boenum =ORCT.boenum and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) <> 'DRF' 
								left join ODSC on ODSC.BankCode=OBOE.BPBankCod and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) <> 'DRF' 

								left join ODPI on ODPI.DocEntry = #NF_Fiscal.BaseRef and #NF_Fiscal.TransType='203' and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) <> 'DRF' 
								left join ODRF on ODRF.DocEntry = #NF_Fiscal.BaseRef and SUBSTRING ( #NF_Fiscal.TransType ,0 , 4) = 'DRF' 

						) TB	
							left join ODSC on ODSC.BankCode=(select top 1 BnkDflt  from OPYM where PayMethCod=PeyMethodNF )  and SUBSTRING ( TransType ,0 , 4) <> 'DRF' 
							where (@BankCode=(select top 1 BnkDflt  from OPYM where PayMethCod=PeyMethodNF )) or isnull(@BankCode, '*')='*'
							and (@slpCode=0 or TB.SlpCode=@slpCode)
							and
							(
								(TB.DueDate>=@dateini and TB.DueDate<=@datefim and @tpData='V')
								or (TB.RefDate>=@dateini and TB.RefDate<=@datefim and @tpData='LC')
							)
						--order by 9	
	) as TB1  


	group by
		descricao,ORdem
	Order By Ordem

end
drop table #NF_Fiscal


GO


execute spcJBCContasAReceberPorVencimento '*','2018-04-09','2018-04-19','V','*','*',0,'N','N'