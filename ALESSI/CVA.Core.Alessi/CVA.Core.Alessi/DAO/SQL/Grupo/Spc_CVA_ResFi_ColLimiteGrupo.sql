/****** Object:  StoredProcedure [dbo].[spc_CVA_ResFi_ColLimite_Grupo]    Script Date: 31/08/2018 15:15:43 ******/
DROP PROCEDURE [dbo].[spc_CVA_ResFi_ColLimite_Grupo]
GO

/****** Object:  StoredProcedure [dbo].[spc_CVA_ResFi_ColLimite_Grupo]    Script Date: 31/08/2018 15:15:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[spc_CVA_ResFi_ColLimite_Grupo]
  @Grupo varchar(20) 
as
begin	

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
  --,'002'
  ,@Grupo
  ,''  
  ,'S'   
  	
	select T0.U_Lim_Cred
	      ,isnull((
					 T0.U_Lim_Cred - (select sum(#tmp.Saldo) from #tmp)	
		             --(select U_Lim_Cred from [@CVA_GRUPOECON] T0 inner join OCRD T1 on T0.Code = T1.U_CVA_GRUPOECON where T0.code = @Grupo)
					   
				  ),0) as Disponivel
		  ,T0.U_cva_valcredito
		  ,T0.U_Lim_Cred
		  ,(select sum(#tmp.Saldo) from #tmp)
	 from [@CVA_GRUPOECON] T0
	inner join OCRD T1
	   on T0.Code = T1.U_CVA_GRUPOECON
	where Code =  @Grupo
		
		end
	
end

GO

exec spc_CVA_ResFi_ColLimite_Grupo '002'