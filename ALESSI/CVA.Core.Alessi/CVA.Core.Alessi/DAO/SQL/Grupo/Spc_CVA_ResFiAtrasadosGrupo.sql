
/****** Object:  StoredProcedure [dbo].[scp_CVA_ResFiAtrsadosGrupo]    Script Date: 04/09/2018 11:24:23 ******/
DROP PROCEDURE [dbo].[scp_CVA_ResFiAtrsadosGrupo]
GO

/****** Object:  StoredProcedure [dbo].[scp_CVA_ResFiAtrsadosGrupo]    Script Date: 04/09/2018 11:24:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create proc scp_CVA_ResFiAtrsadosGrupo
  @grupo varchar(20)
 
as
 declare @dt varchar(30) = Getdate() -1

begin 
	Create table #tmp
	( 
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
	,[Serial]       int
	,[FormaPagamento] varchar(200)
	,[PayMethodNF]   varchar(200)
	,[BancoNF]      int
	,[Installmnt]   int
	,[OrctComments] varchar(max)
	,[BankName]     varchar(200)
	,[DocEntryNFS]  int
	,[LateDays]     int
	)

insert into #tmp
  exec spc_CVA_ContasAReceberPorVencimento     
  '*'
  --,'2000-01-01'
   --@CardCode
   ,'2000-01-01'
  ,@dt
  --,'2018-09-04'
  ,'V'
  ,'*'
  ,'*'
  ,@grupo
  ,''
  ,'S'   
	

select distinct isnull((select sum(DATEDIFF(Day,#tmp.DueDate,@dt)) /
    (select case when count(#tmp.Installmnt) = 0 then 1 else count(#tmp.Installmnt) end from #tmp where @dt > #tmp.DueDate)),0) as Média
				
			   
	,(select count(DATEDIFF(Day,#tmp.DueDate,@dt)) from #tmp where @dt > #tmp.DueDate  ) as Atual
			   
	,isnull((select max(DATEDIFF(Day,#tmp.DueDate,@dt)) from #tmp where @dt > #tmp.DueDate ),0) as Maior
			   
	,isnull((
		(select SUM(Saldo) from #tmp where @dt > #tmp.DueDate) / (select SUM(Saldo) from #tmp) 
	),0) as Atraso
			   
			   
	  
  from #tmp
   

drop table #tmp
end

go

exec  scp_CVA_ResFiAtrsadosGrupo '002'

