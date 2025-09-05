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

/****** Object:  StoredProcedure [dbo].[SpcCVAFluxoCaixaLancamentosRecorrentes]    Script Date: 22/08/2018 16:21:25 ******/
DROP PROCEDURE [dbo].[SpcCVAFluxoCaixaLancamentosRecorrentes]
GO

/****** Object:  StoredProcedure [dbo].[SpcCVAFluxoCaixaLancamentosRecorrentes]    Script Date: 22/08/2018 16:21:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



create proc [dbo].[SpcCVAFluxoCaixaLancamentosRecorrentes] 

  @dtInicial    date = null
 ,@dtFinal      date = null
 ,@dtLanInicial date = null
 ,@dtLanFinal   date = null
as

    create table #LancamentosRecorrentes
    (
		 ChaveDoc INT
		,ChaveDocString NVARCHAR(10) Collate Database_Default
		,Numero			NVARCHAR(50) Collate Database_Default
		,Parcela		INT
		,ParcelaString NVARCHAR(10) Collate Database_Default
		,CardCode		NVARCHAR(50) Collate Database_Default
		,CardName		NVARCHAR(200) Collate Database_Default
		,CardFName		NVARCHAR(200) Collate Database_Default
		,CardCodeName  NVARCHAR(300) Collate Database_Default
		,CNPJ			NVARCHAR(20) Collate Database_Default
		,TipoObj		NVARCHAR(40) Collate Database_Default
		,TipoDoc		NVARCHAR(2) Collate Database_Default
		,TabelaDoc		NVARCHAR(4) Collate Database_Default
		,Emissao		DATETIME
		,Vencimento			DATETIME
		,ValorDoc			NUMERIC(19,6)
		,DocCur				NVARCHAR(10) Collate Database_Default
		,DocRate	NUMERIC(19,6)
		,ValorDocME NUMERIC(19,6)
		,ChavePgto INT
		,ChavePgtoString NVARCHAR(10) Collate Database_Default
		,Pagamento DATETIME
		,TipoObjPgto NVARCHAR(40) Collate Database_Default
		,TipoPgto NVARCHAR(2) Collate Database_Default
		,TabelaPgto NVARCHAR(4) Collate Database_Default
		,ValorPago NUMERIC(19,6)
		,JurosDesconto NUMERIC(19,6)
		,ValorSaldo NUMERIC(19,6)
		,PgtoCur NVARCHAR(10) Collate Database_Default
		,PgtoRate NUMERIC(19,6)
		,ValorPagoME NUMERIC(19,6)
		,FormaPgto NVARCHAR(MAX) Collate Database_Default
		,Comments NVARCHAR(MAX) Collate Database_Default
		,AcctCode NVARCHAR(50) Collate Database_Default
		,AcctName NVARCHAR(200) Collate Database_Default
		,DocStatus CHAR(1) Collate Database_Default
    )

 declare @RcurCode varchar(50) ,@RcurDesc varchar(72) ,@Frequency varchar(30) ,@ProximaExecucao date ,@DataLimite date ,@Total decimal(19,4),@CardCode nvarchar(30),@CardName nvarchar(200)

declare @dtAtual date

declare LRR cursor read_only for

  select orcr.RcurCode
        ,orcr.RcurDesc
		,orcr.Frequency
        ,orcr.NextDeu   as 'ProximaExecução'
        ,isnull(orcr.LimitDate,@DtFinal) as 'Data Limite'
        ,case when CardType='S' then abs((rcr1.Debit-rcr1.Credit))*-1 else abs((rcr1.Debit-rcr1.Credit)) end as 'Total'
        ,OCRD.CardCode
        ,OCRD.CardName
    from orcr
   inner join rcr1 on rcr1.RcurCode=orcr.RcurCode
   inner join OCRD on OCRD.CardCode=rcr1.AcctCode
   where orcr.Instance=0

open LRR

 fetch from LRR into @RcurCode,@RcurDesc,@Frequency,@ProximaExecucao,@DataLimite,@Total,@CardCode,@CardName

while @@fetch_status = 0

 begin

        set @dtAtual=@ProximaExecucao

        if @DataLimite>@dtFinal 
			begin 
				 set @DataLimite=@dtFinal 
			end

        while (@dtAtual<=@DataLimite) begin 

    insert into #LancamentosRecorrentes
		select
		
			 0								AS 'ChaveDoc'
			,@RcurCode						AS 'ChaveDocString'
			,'0'							AS 'Numero'
			,1								AS 'Parcela'
			,'1/1'							AS 'ParcelaString'
			,@CardCode						AS 'CardCode'
		    ,@CardName						AS 'CardName'
			,null							AS 'CardFName'
			,@CardCode +' - ' +@CardName 	AS 'CardCodeName'
			,null							AS 'CNPJ'
			,'-3'							AS 'TipoObj'
			,'LR'							AS 'TipoDoc'
			,'ORCR'							AS 'TabelaDoc'
			,@dtAtual						AS 'Emissao'
			,@DataLimite					AS 'Vencimento'
			,@Total							AS 'ValorDoc'
			,'R$'							AS 'DocCur'
			,1								AS 'DocRate'
			,0								AS 'ValorDocME'
			,0								AS 'ChavePgto'
			,null 							AS 'ChavePgtoString'
			,null							AS 'Pagamento'
			,null							AS 'TipoObjPgto'
			,null							AS 'TipoPgto'
			,null							AS 'TabelaPgto'
			,0								AS 'ValorPago'
			,0								AS 'JurosDesconto'
			,@Total							AS 'ValorSaldo'
			,null							AS 'PgtoCur'
			,0								AS 'PgtoRate'
			,0								AS 'ValorPagoME'
			,null							AS 'FormaPgto'
			,'Lancamentos Periódicos'		AS 'Comments'
			,null							AS 'AcctCode'
			,null							AS 'AcctName'
			,'A'							AS 'DocStatus'
			
			

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
                set @dtAtual=dateadd(day,1, @DataLimite)
            end
        end

fetch next from LRR

into @RcurCode,@RcurDesc,@Frequency,@ProximaExecucao,@DataLimite,@Total ,@CardCode,@CardName

end

close LRR

deallocate LRR

select distinct * from #LancamentosRecorrentes
where (Emissao >= @Dtinicial or @dtInicial = null)
  and (Emissao <= @DtFinal   or @dtFinal   = null)
  and (Vencimento >= @dtLanInicial or @dtLanInicial = null)
  and (Vencimento <= @dtLanFinal or @dtLanFinal = null)
order by 2

 
GO

