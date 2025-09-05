/*    ==Parâmetros de Script==

    Versão do Servidor de Origem : SQL Server 2014 (12.0.2000)
    Edição do Mecanismo de Banco de Dados de Origem : Microsoft SQL Server Standard Edition
    Tipo do Mecanismo de Banco de Dados de Origem : SQL Server Autônomo

    Versão do Servidor de Destino : SQL Server 2014
    Edição de Mecanismo de Banco de Dados de Destino : Microsoft SQL Server Standard Edition
    Tipo de Mecanismo de Banco de Dados de Destino : SQL Server Autônomo
*/

USE [SBOHybel]
GO

/****** Object:  StoredProcedure [dbo].[spcCVAFluxo_Caixa_Carga]    Script Date: 22/08/2018 16:20:49 ******/
DROP PROCEDURE [dbo].[spcCVAFluxo_Caixa_Carga]
GO

/****** Object:  StoredProcedure [dbo].[spcCVAFluxo_Caixa_Carga]    Script Date: 22/08/2018 16:20:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		Marco Kurpel
-- Create date: 17/05/2018
-- Description:	Fluxo de Caixa Carga
-- =============================================
CREATE PROCEDURE [dbo].[spcCVAFluxo_Caixa_Carga] 

	 @DataInicial date
	 ,@DataFinal date
	-- @DtLancamentoIni DATE
	--,@DtLancamentoFin DATE
	--,@DtVencimentoIni DATE
	--,@DtVencimentoFin DATE
	,@Provisoes CHAR(1) = 'N'
	,@SaldoInciaal int

AS
BEGIN

declare @DtVencimentoIni DATE
declare @DtVencimentoFin DATE

SELECT @DtVencimentoIni = @DataInicial
      ,@DtVencimentoFin = @DataFinal

declare @PrevisoesParam char(1)


  declare @vGroupNum int
  declare @vShipDate smalldatetime
  declare @vLineTotal money
  declare @vTipo varchar(3)
  declare @vAdiantamento  money
  declare @vRestante money

	  declare cp1 cursor local fast_forward read_only for

	  select distinct 'PRE' as tipo			 
			 , ordr.groupnum as forma
			 , rdr1.ShipDate as data_base
			 , SUM(rdr1.linetotal) as valor
			 ,null
			 ,null
		  from RDR1
		 inner join ORDR
			on ORDR.DocEntry = rdr1.DocEntry 
		 where rdr1.InvntSttus = 'O'
		   and ordr.DocStatus = 'O'
		   and (@Provisoes='P' or @Provisoes='T') 
		   and  @Provisoes<>'N'
		   and ORDR.CANCELED <> 'Y'
		 group by  ordr.groupnum, rdr1.ShipDate 
		 union all
		select distinct  'PRO' as tipo			 
			 , oPOR.groupnum as forma
			 , POR1.ShipDate as data_base 
			 , sum(POR1.linetotal) as valor
			 , ODPO.DpmAmnt as adiantamento		
			 ,null	 
        from opor 
	   inner join por1 
		  on opor.docentry = por1.docentry
	    left join dpo1 
		  on por1.docentry = dpo1.baseEntry
	     and por1.linenum = dpo1.baseline 
	     and por1.objtype = dpo1.basetype
	    left join odpo
		  on dpo1.docentry = odpo.docentry   
	   where POR1.InvntSttus = 'O'
	     and oPOR.DocStatus = 'O'  
		 --and por1.ShipDate = '2018-06-18'
		 and (@Provisoes='C' or @Provisoes='T') 
		 and  @Provisoes<>'N'		 
		 and ODPO.DocEntry not in (select BaseEntry  from rpc1 where rpc1.BaseEntry = odpo.DocEntry and rpc1.BaseType = 204 )
	   group by  oPOR.groupnum, POR1.ShipDate, ODPO.DpmAmnt
	   order by 1, 3, 2
	 open cp1  
 while 1 = 1
	  begin
		fetch next from cp1 into @vTipo, @vGroupNum, @vShipDate, @vLineTotal,@vAdiantamento,@vRestante
		if @@FETCH_STATUS <> 0 break
    

	insert into #lancamentos (tipo,dt_vencimento, valor,valornota,valorADT,restante)
		  SELECT @vTipo
			   , DATEADD(day, InstDays, @vShipDate)			   
			   , (case when @vTipo = 'PC' then ((((@vLineTotal)* InstPrcnt) / 100)) else ((@vLineTotal * InstPrcnt) / 100) end)- isnull(@vAdiantamento,0)
			   ,@vlinetotal
			   ,isnull(@vAdiantamento,0) 
			   , (case when @vTipo = 'PC' then ((((@vLineTotal)* InstPrcnt) / 100)) else ((@vLineTotal * InstPrcnt) / 100) end)- isnull(@vAdiantamento,0)
			from CTG1
		   where CTGCode = @vGroupNum 	
       
		if @@ROWCOUNT = 0
		  insert into #lancamentos (tipo,dt_vencimento, valor    ,valornota,valorADT,restante)
						    select @vTipo,@vShipDate, @vLineTotal,@vlinetotal,@vAdiantamento ,@vRestante  
	  end
	  close cp1
	  deallocate cp1


		

	  --select * from #lancamentos where dt_vencimento < '2018-06-19' order by dt_vencimento
	


Create Table #ContasReceber
(
	ChaveDoc INT,		
	ChaveDocString NVARCHAR(10) Collate Database_Default,
	Numero NVARCHAR(50) Collate Database_Default,
	Parcela INT,
	ParcelaString NVARCHAR(10) Collate Database_Default,
	CardCode NVARCHAR(50) Collate Database_Default,
	CardName NVARCHAR(200) Collate Database_Default,
	CardFName NVARCHAR(200) Collate Database_Default,
	CardCodeName NVARCHAR(300) Collate Database_Default,
	CNPJ NVARCHAR(20) Collate Database_Default,
	TipoObj NVARCHAR(40) Collate Database_Default,
	TipoDoc NVARCHAR(2) Collate Database_Default,
	TabelaDoc NVARCHAR(4) Collate Database_Default,
	Emissao DATETIME,
	Vencimento DATETIME,
	ValorDoc NUMERIC(19,6),
	DocCur NVARCHAR(10) Collate Database_Default,
	DocRate NUMERIC(19,6),
	ValorDocME NUMERIC(19,6),
	ChavePgto INT,
	ChavePgtoString NVARCHAR(10) Collate Database_Default,
	Pagamento DATETIME,
	TipoObjPgto NVARCHAR(40) Collate Database_Default,
	TipoPgto NVARCHAR(2) Collate Database_Default,
	TabelaPgto NVARCHAR(4) Collate Database_Default,
	ValorPago NUMERIC(19,6),
	JurosDesconto NUMERIC(19,6),
	ValorSaldo NUMERIC(19,6),
	PgtoCur NVARCHAR(10) Collate Database_Default,
	PgtoRate NUMERIC(19,6),
	ValorPagoME NUMERIC(19,6),
	FormaPgto NVARCHAR(MAX) Collate Database_Default,
	Comments NVARCHAR(MAX) Collate Database_Default,
	AcctCode NVARCHAR(50) Collate Database_Default,
	AcctName NVARCHAR(200) Collate Database_Default,
	DocStatus CHAR(1) Collate Database_Default,	
	DiffDias  int,
	SlpCode INT,
	SlpName NVARCHAR(100) Collate Database_Default,
	GrupoEconCode NVARCHAR(50) Collate Database_Default,
	GrupoEconDesc NVARCHAR(200) Collate Database_Default
														
)

select @PrevisoesParam=case when (@Provisoes='P' or @Provisoes='T') then 'Y' else 'N' end 
insert into #contasReceber
exec spc_CVA_RelatorioCR_Nova 
 'T'
 ,null
 ,null
 ,@dataInicial
 ,@dataFinaL 
 --,@DtVencimentoIni 
 --,@DtVencimentoFin 
 ,@PrevisoesParam 


--select * from #ContasReceber



CREATE TABLE #ContasPagar
(
	ChaveDoc INT,
	ChaveDocString NVARCHAR(10) Collate Database_Default,
	Numero NVARCHAR(50) Collate Database_Default,
	Parcela INT,
	ParcelaString NVARCHAR(10) Collate Database_Default,
	CardCode NVARCHAR(50) Collate Database_Default,
	CardName NVARCHAR(200) Collate Database_Default,
	CardFName NVARCHAR(200) Collate Database_Default,
	CardCodeName NVARCHAR(300) Collate Database_Default,
	CNPJ NVARCHAR(20) Collate Database_Default,
	TipoObj NVARCHAR(40) Collate Database_Default,
	TipoDoc NVARCHAR(2) Collate Database_Default,
	TabelaDoc NVARCHAR(4) Collate Database_Default,
	Emissao DATETIME,
	Vencimento DATETIME,
	ValorDoc NUMERIC(19,6),
	DocCur NVARCHAR(10) Collate Database_Default,
	DocRate NUMERIC(19,6),
	ValorDocME NUMERIC(19,6),
	ChavePgto INT,
	ChavePgtoString NVARCHAR(10) Collate Database_Default,
	Pagamento DATETIME,
	TipoObjPgto NVARCHAR(40) Collate Database_Default,
	TipoPgto NVARCHAR(2) Collate Database_Default,
	TabelaPgto NVARCHAR(4) Collate Database_Default,
	ValorPago NUMERIC(19,6),
	JurosDesconto NUMERIC(19,6),
	ValorSaldo NUMERIC(19,6),
	PgtoCur NVARCHAR(10) Collate Database_Default,
	PgtoRate NUMERIC(19,6),
	ValorPagoME NUMERIC(19,6),
	FormaPgto NVARCHAR(MAX) Collate Database_Default,
	Comments NVARCHAR(MAX) Collate Database_Default,
	AcctCode NVARCHAR(50) Collate Database_Default,
	AcctName NVARCHAR(200) Collate Database_Default,
	DocStatus CHAR(1) Collate Database_Default,
	DiffDias  int
)
select @PrevisoesParam=case when (@Provisoes='C' or @Provisoes='T') then 'Y' else 'N' end 
insert into #ContasPagar
exec spc_CVA_RelatorioCP_Nova 
 'T'
 ,null
 ,null
 ,@dataInicial
 ,@dataFinaL 
 --,@DtVencimentoIni 
 --,@DtVencimentoFin 
 ,@PrevisoesParam 


--select * from #ContasPagar


 insert into #lancamentos (tipo, dt_vencimento, valor)
    SELECT distinct 'CAP'
          , dt_vencimento, 
           sum(vl_saldo)
    FROM
    (
		select 
			ValorSaldo as 'vl_saldo' ,
			Vencimento as 'dt_vencimento'
		from 	
		#ContasPagar
	

    ) as completo
    where dt_vencimento <= @DataFinal 
    group by dt_vencimento 

	--select * from #lancamentos where Tipo = 'PRO' and  dt_vencimento between @DataInicial and @DataFinal

 insert into #lancamentos (tipo, dt_vencimento, valor)
    SELECT distinct 'CAR'
          , dt_vencimento, 
           sum(vl_saldo)
    FROM 
	(
		select 
			ValorSaldo as 'vl_saldo' ,
			Vencimento as 'dt_vencimento'
		from 
			#ContasReceber   
     ) as completo
    where dt_vencimento between @DataInicial and @DataFinal 
    group by dt_vencimento 

	--select * from #lancamentos where tipo = 'PRE'
	
  delete from #lancamentos where dt_vencimento > @DataFinal


  

drop table #ContasReceber
drop table #ContasPagar
end



GO

