USE SBO_PRD_CFI0002
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcCVAOfertaDeCompras]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].spcCVAOfertaDeCompras
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE spcCVAOfertaDeCompras
	@DocNum int
	,@DataIni as DAte
	,@DataFIM as DAte	
	,@DataIniLCTO as DAte
	,@DataFIMLCTO as DAte	
	,@DataIniVAL as DAte
	,@DataFIMVAL as DAte	
	,@BplId int	
	,@DocStatus as nvarchar(1)
	
	,@SolicitanteIni as nvarchar(max)
	,@SolicitanteFim as nvarchar(max)
	
	,@DepartamentoINI as nvarchar(max)
	,@DepartamentoFIM as nvarchar(max)
WITH ENCRYPTION 
AS 
BEGIN
	select 
		T0.DocNum  as 'Nº OFC'
		,upper(T4.Name) as 'Departamento'
		,upper(T2.SlpName) as 'Solicitante'
		,upper(case when T0.DocStatus='O' then 'Aberto' when T0.DocStatus='C' then 'Fechado' else T0.DocStatus end ) as 'Status'
		,cast(T0.DocDate as DAte) as 'Data OFC'
		,cast(T0.TaxDate as DAte) as 'Lançamento'
		,cast(T0.DocDueDate as DAte) as 'Valido até'
		,upper(T1.BPLName) 'Filial de Requisição'
		,T0.DocTotal as 'Valor'
	from 
		OPQT T0
		inner join OBPL T1 on T1.BPLId=T0.BPLId		
		inner join OSLP T2 on T2.SlpCode=T0.SlpCode		
		INNER JOIN OUSR T3 ON T0.[UserSign] = T3.[USERID]
		inner join OUDP T4 on T4.Code=T3.Department
	where 
		T0.CANCELED<>'Y' and T0.CANCELED<>'C'
		and cast(T0.DocDate as DAte) between @DataIni and @DataFIM
		and cast(T0.TaxDate as DAte) between @DataIniLCTO and @DataFIMLCTO
		and cast(T0.DocDueDate as DAte) between @DataIniVAL and @DataFIMVAL
		and (@DocNum=0 or @DocNum=T0.DocNum )
		and (@BplId=0 or @BplId=T1.BPLId)
		and (  @DocStatus='*' or @DocStatus=T0.DocStatus)
		and (
				(  @SolicitanteIni='Todos' and @SolicitanteFim='Todos' )
				or (  T2.SlpName>=@SolicitanteIni and T2.SlpName<=@SolicitanteFim )
			)
		and (
				(  @DepartamentoINI='Todos' and @DepartamentoFIM='Todos'   )
				or (  T4.Name>=@DepartamentoINI and  T4.Name<=@DepartamentoFIM   )
				
			)		
end

go

execute spcCVAOfertaDeCompras 0,'2018-01-01','2018-12-30', '2018-01-01','2018-12-30', '2018-01-01','2018-12-30',0,'*','Todos','Todos','Todos','Todos'



--select * from OPQT
--status
--select 'O' Codigo, 'Aberto' Nome union all select 'C' Codigo, 'Fechado' Nome  union all select '*' Codigo, 'Todos' Nome  order by 1

--Solicitante
--select T0.SlpName Codigo,T0.SlpName Nome from OSLP T0 union all select 'Todos', 'Todos' order by 1


--Departamento
--select T0.Name Codigo,T0.Name Nome from OUDP T0 union all select 'Todos' Codigo, 'Todos' Nome  order by 1