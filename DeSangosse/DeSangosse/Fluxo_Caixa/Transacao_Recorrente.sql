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

/****** Object:  StoredProcedure [dbo].[SpcCVAFluxoCaixaTransacoesRecorrentes]    Script Date: 22/08/2018 16:21:42 ******/
DROP PROCEDURE [dbo].[SpcCVAFluxoCaixaTransacoesRecorrentes]
GO

/****** Object:  StoredProcedure [dbo].[SpcCVAFluxoCaixaTransacoesRecorrentes]    Script Date: 22/08/2018 16:21:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[SpcCVAFluxoCaixaTransacoesRecorrentes]

 @Dtinicial  date
,@DtFinal    date

as
set @DtFinal = dateadd(day,1,@DtFinal)
  create table #TransacoesRecorrentes 
  (
	  TipoDoc  nvarchar(40)   null
	, Data     smalldatetime  null
	, Tipo     nvarchar (30)  null
	, Tp       nvarchar (30)  null
	, DocEntry int            null
	, CardCode varchar(30)    null
	, CardName varchar(200)   null
	, Parcela  int            null
	, ValorParcela    decimal(19, 2) null
	, Saldo    decimal(19, 2)

  ) 

declare @ModeloTrr nvarchar(30) ,@dataInicio date ,@PlanDate date ,@DataFinal date ,@Frequency char(10) /*,@objectType int */,@DraftEntry int , @DocObjType nvarchar(30)

Declare @dtAtual date

declare cpl cursor read_only for

select
       ORCP.Code
      ,ORCP.StartDate
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
      ,isnull(ORCP.EndDate, dateadd(year,1,@DtFinal)) 'EndDate'
      ,ORCP.Frequency
      ,ORCP.DraftEntry
      ,ORCP.DocObjType
from  ORCP
where ORCP.IsRemoved<>'Y'
  and isnull(ORCP.EndDate, @DtFinal)>=getdate()
order by 2
declare @contDraf as integer
open cpl
 fetch from cpl into @ModeloTrr ,@DataInicio ,@PlanDate ,@DataFinal ,@Frequency /*,@ObjectType*/ ,@DraftEntry,@DocObjType

while @@fetch_status = 0
Begin
        set @contDraf=0     
        set @dtAtual=@PlanDate

        if @DataFinal>@DtFinal begin set @DataFinal=@DtFinal end

        while (@dtAtual<@DataFinal) begin

            set @contDraf=@contDraf+1

            insert into #TransacoesRecorrentes(tipoDoc,Data,Tipo, Tp,DocEntry,CardCode,CardName,Parcela,ValorParcela)           

            select
                'DRF - ' +cast( @DraftEntry as nvarchar(10)) +' -> ' +@ModeloTrr  +' - ' +cast(@contDraf as nvarchar(50)) as 'TipoDoc'
                ,case when ctg1.InstDays is null 
					     then DATEADD(DAY,OCTG.ExtraDays, @dtAtual)
                      else
                          DATEADD(DAY,ctg1.InstDays, @dtAtual)
					  end 'Vencimento' 

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
                ,case when ctg1.IntsNo is null then 0 else ctg1.IntsNo end as 'Parcela'
                ,case when ODRF.ObjType in ('22','18') 
				      then case when ctg1.InstPrcnt is null 
					            then DocTotal*-1 else ((DocTotal/100)*ctg1.InstPrcnt)*-1 end
			      else
                     case when ctg1.InstPrcnt is null then DocTotal else (DocTotal/100)*ctg1.InstPrcnt end end as 'ValorParcela'
            from ODRF
           inner join OCTG on OCTG.GroupNum=ODRF.GroupNum
            left JOIN  ctg1 ON ODRF.GroupNum=ctg1.CTGCode
           inner join OCRD on OCRD.CardCode=ODRF.CardCode
           where ODRF.DocEntry=@DraftEntry
		     and DocTotal > 0             

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

 

select * from #TransacoesRecorrentes

where data>=@Dtinicial and data<=@DtFinal

group by tipoDoc,Data,Tipo, Tp,DocEntry,CardCode,CardName,Parcela,ValorParcela,Saldo

 

order by 2

drop table #TransacoesRecorrentes

GO

