  
alter proc scp_CVA_ResFiAberto  
  @CardCode varchar(20)  
 ,@CardType varchar(20)  
as  
 declare @dt varchar(30) = Getdate() -1  
  
begin   
 Create table #tmp  
 (   
     [TransId]      int  
 ,[Line_ID]  int  
 ,[Account]      nvarchar(300)  
 ,[ShortName]    nvarchar(300)  
 ,[TransType]    nvarchar(400)  
 ,[CreatedBy]    int  
 ,[BaseRef]      nvarchar(220)  
 ,[SourceLine]   smallint  
 ,[RefDate]      datetime  
 ,[DueDate]      datetime  
 ,[BalDueCred] numeric (16, 9)   
 ,[BalDueDeb] numeric (16, 9)   
 ,[BalDueCred_BalDueDeb] numeric (16, 9)   
    ,[Saldo]  numeric (16, 9)   
 ,[LineMemo]     nvarchar(1000)  
 ,[CardName]     nvarchar(300)  
 ,[CardCode]     nvarchar(300)  
 ,[Balance]     numeric (19, 6)  
 ,[SlpCode]     int  
 ,[DebitMAthCredit]numeric (19, 6)   
 ,[IsSales]  nvarchar(20)  
 ,[Currency]  nvarchar(60)  
 ,[BPLName]     nvarchar(200)  
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
  --'C00001163'  
  --,'2000-01-01'  
   @CardCode  
   ,'2000-01-01'  
  ,@dt  
  --,'2018-09-04'  
  ,'V'  
  ,'*'  
  ,'*'  
  ,'*'  
  ,''  
  ,'S'  
      
 print @dt  
  
select distinct (select count(*) from #tmp ) as qtdeAberto  
      ,(select isnull(sum(Saldo),0) from #tmp ) as ValorAberto  
   ,(select count(*) from #tmp where PayMethodNF in('RecebTransf','')) as qtdeEmCarteira  
      ,(select isnull(sum(Saldo),0) from #tmp where PayMethodNF in('RecebTransf','')) ValorCarteira  
   ,(select count(*) from #tmp where FormaPagamento like 'bol%') as qtdeBoleto  
      ,(select isnull(sum(Saldo),0) from #tmp where FormaPagamento like 'Bol%') ValorBoleto  
   ,(select count(*) from #tmp where FormaPagamento like '%Cheque%') as qtdeCheque  
      ,(select isnull(sum(Saldo),0) from #tmp where PayMethodNF like '%Cheque%') ValorCheque  
   ,(select count(*) from #tmp where #tmp.DueDate < @dt) as qtdeAtrasos  
      ,(select isnull(sum(Saldo),0) from #tmp where #tmp.DueDate < @dt) as ValorAtrasos  
   ,(  
     (select count(*) from #tmp )  
    +(select count(*) from #tmp where #tmp.DueDate < @dt)  
    +(select count(*) from #tmp where PayMethodNF in('RecebTransf',''))   
    +(select count(*) from #tmp where FormaPagamento like 'bol%')   
    +(select count(*) from #tmp where PayMethodNF like '%Cheque%')  
      
  ) as qtdeTotal  
   ,(  
         (select isnull(sum(Saldo),0) from #tmp )  
     +(select isnull(sum(Saldo),0) from #tmp where #tmp.DueDate < @dt)  
     +(select isnull(sum(Saldo),0) from #tmp where PayMethodNF in('RecebTransf',''))   
     +(select isnull(sum(Saldo),0) from #tmp where FormaPagamento like 'bol%')   
     +(select isnull(sum(Saldo),0) from #tmp where PayMethodNF like '%Cheque%')   
    ) as ValorTotal  
  
     
  from #tmp  
drop table #tmp  
end  
  
  go
exec  scp_CVA_ResFiAberto 'C40000','C'