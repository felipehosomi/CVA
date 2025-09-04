USE SBOAlessi
GO

/****** Object:  StoredProcedure SpcJBCFluxoCaixaTransacoesRecorrentes    Script Date: 07/21/2015 14:10:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SpcJBCFluxoCaixaTransacoesRecorrentes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SpcJBCFluxoCaixaTransacoesRecorrentes]
GO

USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[SpcJBCFluxoCaixaTransacoesRecorrentes]    Script Date: 07/21/2015 14:10:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create proc [dbo].[SpcJBCFluxoCaixaTransacoesRecorrentes]

 @Dtinicial smalldatetime 
,@DtFinal   smalldatetime
, @CardType Nvarchar(1)
,@CVA_GRUPOECON  nvarchar(60)
,@slpCode int
WITH ENCRYPTION as 

set @DtFinal = dateadd(day,1,@DtFinal)
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
	  ,isnull(ORCP.EndDate, dateadd(year,1,@DtFinal)) 'EndDate'
	  ,ORCP.Frequency
	  --,DocEntry
      ,ORCP.DraftEntry
	  ,ORCP.DocObjType
from 
	ORCP

where
	ORCP.IsRemoved<>'Y'
	and isnull(ORCP.EndDate, @DtFinal)>=getdate()
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

		if @DataFinal>@DtFinal begin set @DataFinal=@DtFinal end
		while (@dtAtual<@DataFinal) begin
			set @contDraf=@contDraf+1
			--print '123'
			--print @DraftEntry

			--select * from ODRF where docentry=1
			insert into #TransacoesRecorrentes(tipoDoc,Data,Tipo, Tp,DocEntry,CardCode,CardName,Parcela,ParcelaTotal,ValorParcela,slpcode) 
			
			select 

				'DRF - ' +cast( @DraftEntry as nvarchar(10)) +' -> ' +@ModeloTrr  +' - ' +cast(@contDraf as nvarchar(50)) as 'TipoDoc'
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
where data>=@Dtinicial and data<=@DtFinal
and (T1.CardType=@CardType or @CardType='*') and (T1.U_CVA_GRUPOECON=@CVA_GRUPOECON or @CVA_GRUPOECON='*')
group by T0.tipoDoc,T0.Data,T0.Tipo, T0.Tp,T0.DocEntry,T0.CardCode,T0.CardName,T0.Parcela,T0.ValorParcela,T0.Saldo,T1.CardCode,T1.CardNAme,T0.ValorParcela, T0.ParcelaTotal ,t0.slpcode

--order by 2
drop table #TransacoesRecorrentes

-- @Dtinicial smalldatetime 
--,@DtFinal   smalldatetime 
go

--exec SpcJBCFluxoCaixaTransacoesRecorrentes '2015-08-20','2015-08-24'
exec SpcJBCFluxoCaixaTransacoesRecorrentes '2000-08-18','2050-10-25','C','*',0
--proximo é dia 21

--select docnum,DocTotal from ODRF where docentry=52