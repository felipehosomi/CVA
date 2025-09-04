USE SBOAlessi
GO

/****** Object:  StoredProcedure SpcJBCLancamentosRecorrentes    Script Date: 07/21/2015 14:10:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SpcJBCFluxoCaixaLancamentosRecorrentes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SpcJBCFluxoCaixaLancamentosRecorrentes]
GO

USE SBOAlessi
GO

/****** Object:  StoredProcedure [dbo].[SpcJBCFluxoCaixaLancamentosRecorrentes]    Script Date: 07/21/2015 14:10:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
  
-- exec SpcJBCFluxoCaixaLancamentosRecorrentes '2015-01-01', '2015-12-31'  
Create proc [dbo].[SpcJBCFluxoCaixaLancamentosRecorrentes] @dtInicial  date,@dtFinal    date, @CardType Nvarchar(1),@CVA_GRUPOECON  nvarchar(60),@slpCode int
 WITH ENCRYPTION as
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
		,isnull(orcr.LimitDate,@DtFinal) as 'Data Limite'
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
		if @DataLimite>@dtFinal begin set @DataLimite=@dtFinal end


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


select	
	'LCMRecorrente' + T0.TipoDoc
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
	data>=@Dtinicial and data<=@DtFinal --and 1=2
	and (T1.CardType=@CardType or @CardType='*')
order by 2


GO
exec SpcJBCFluxoCaixaLancamentosRecorrentes '2000-08-18', '2020-08-21','C','*',0