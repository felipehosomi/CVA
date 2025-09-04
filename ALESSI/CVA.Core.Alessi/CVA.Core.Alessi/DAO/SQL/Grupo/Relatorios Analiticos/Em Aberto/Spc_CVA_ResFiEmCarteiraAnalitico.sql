 
/****** Object:  StoredProcedure [dbo].[scp_CVA_ResFiEmCarteiraAnaliticoGrupo]    Script Date: 04/09/2018 11:24:23 ******/
DROP PROCEDURE [dbo].[scp_CVA_ResFiEmCarteiraAnaliticoGrupo]
GO

/****** Object:  StoredProcedure [dbo].[scp_CVA_ResFiEmCarteiraAnaliticoGrupo]    Script Date: 04/09/2018 11:24:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO 
Create proc scp_CVA_ResFiEmCarteiraAnaliticoGrupo
  @grupo varchar(20)  
as  
 declare @dt varchar(30) = Getdate() -1  
  
begin   
 Create table #tmp  
 (   
  [TransId]              int  
 ,[Line_ID]              int  
 ,[Account]              nvarchar(300)  
 ,[ShortName]            nvarchar(300)  
 ,[TransType]            nvarchar(400)  
 ,[CreatedBy]            int  
 ,[BaseRef]              nvarchar(220)  
 ,[SourceLine]           smallint  
 ,[RefDate]              datetime  
 ,[DueDate]              datetime  
 ,[BalDueCred]           numeric (16, 9)   
 ,[BalDueDeb]            numeric (16, 9)   
 ,[BalDueCred_BalDueDeb] numeric (16, 9)   
 ,[Saldo]				 numeric (16, 9)   
 ,[LineMemo]			 nvarchar(1000)  
 ,[CardName]			 nvarchar(300)  
 ,[CardCode]			 nvarchar(300)  
 ,[Balance]				 numeric (19, 6)  
 ,[SlpCode]				 int  
 ,[DebitMAthCredit]		 numeric (19, 6)   
 ,[IsSales]				 nvarchar(20)  
 ,[Currency]			 nvarchar(60)  
 ,[BPLName]				 nvarchar(200)  
 ,[Serial]				 int  
 ,[FormaPagamento]		 varchar(200)  
 ,[PayMethodNF]			 varchar(200)  
 ,[BancoNF]				 int  
 ,[Installmnt]			 int  
 ,[OrctComments]		 varchar(max)  
 ,[BankName]			 varchar(200)  
 ,[DocEntryNFS]			 int  
 ,[LateDays]			 int  
 )  
  
insert into #tmp  
  exec spc_CVA_ContasAReceberPorVencimento       
  --'C00001163'  
  --,'2000-01-01'  
   '*'  
   ,'2000-01-01'  
  ,@dt  
  --,'2018-09-04'  
  ,'V'  
  ,'*'  
  ,'*'
  ,@grupo  
  --,'*'  
  ,''  
  ,'S'  
select CardCode +' - ' + CardName as 'Cliente'
	    ,Serial as 'NF'
		,RefDate as 'Dta Doc'		
		,DueDate as 'Venc'
		,Convert(decimal(19,2),Saldo)  as 'Valor'
		,(select U_Juros from [@CVA_Juros]) as 'Juros'
		--Saldo
		--,(select Convert(decimal(19,2),U_Juros) from [@CVA_Juros])/100
		--,DATEDIFF(DAY,RefDate,DueDate)
		--,(((select Convert(decimal(19,2),U_Juros) from [@CVA_Juros])/100)/30)*100
		--,round(DATEDIFF(DAY,RefDate,DueDate)*(((select Convert(decimal(19,2),U_Juros) from [@CVA_Juros])/100)/30)*100,2)
		--,((round(DATEDIFF(DAY,RefDate,DueDate)*(((select Convert(decimal(19,2),U_Juros) from [@CVA_Juros])/100)/30)*100,2)/100)*Saldo)
		,Convert(Decimal(19,2),Saldo + ((round(DATEDIFF(DAY,RefDate,DueDate)*(((select Convert(decimal(19,2),U_Juros) from [@CVA_Juros])/100)/30)*100,2)/100)*Saldo)) as Total    
    
  from #tmp where PayMethodNF in('RecebTransf','')
  order by 1
drop table #tmp  
end  
  
  go
exec  scp_CVA_ResFiEmCarteiraAnaliticoGrupo '002'