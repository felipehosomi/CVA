USE SBO_PRD_CFI0002
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcCVASolicitacaoDeCompras]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].spcCVASolicitacaoDeCompras
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE spcCVASolicitacaoDeCompras
	@DocNum int
	,@DataIni as DAte
	,@DataFIM as DAte
	,@DocStatus as nvarchar(1)
	,@BplId int
	,@SolicitanteIni as nvarchar(max)
	,@SolicitanteFim as nvarchar(max)
	,@Departamento as int
WITH ENCRYPTION 
AS 
BEGIN
	select 
		T0.DocNum  as 'Nº SOL'
		,upper(T2.Name) as 'Departamento'
		,upper(T0.ReqName) as 'Solicitante'
		--,case when T0.DocStatus='O' then 'Aberto' when T0.DocStatus='C' then 'Fechado' else T0.DocStatus end  as 'Status'
		,upper(case 
			when T0.WddStatus= '-' then 'Sem esquema de Autorização' 
			when T0.WddStatus= 'W' then 'Pendente'
			when T0.WddStatus= 'Y' then 'Aprovado' 
			when T0.WddStatus= 'N' then 'Rejeitado' 
			when T0.WddStatus= 'P' then 'Gerada' 
			when T0.WddStatus= 'A' then 'Gerado pelo Autorizador'
			when T0.WddStatus= 'C' then 'Cancelado' 
		end) 'Status'				
		,cast(T0.DocDate as DAte) as 'Data SOL'
		,cast(T0.TaxDate as DAte) as 'Lançamento'
		,cast(T0.DocDueDate as DAte) as 'Valido até'
		,upper(T1.BPLName) 'Filial de Requisição'
		,T0.DocTotal as 'Valor'
	from 
		OPRQ T0
		inner join OBPL T1 on T1.BPLId=T0.BPLId
		inner join OUDP T2 on T2.Code=T0.Department
	where 
		T0.CANCELED<>'Y' and T0.CANCELED<>'C'
		and cast(T0.DocDate as DAte) between @DataIni and @DataFIM
		and (  @DocStatus='*' or @DocStatus=T0.DocStatus)
		and (
				--(  @Solicitante='*' or T0.ReqName=@Solicitante )
				(  @SolicitanteIni='*' and @SolicitanteFim='*')
				or (T0.ReqName >= @SolicitanteIni and (T0.ReqName <= @SolicitanteFim))
			)
		and (  @Departamento=0 or T2.Code=@Departamento   )
		and (@DocNum=0 or @DocNum=T0.DocNum )
		and (@BplId=0 or @BplId=T1.BPLId)
		
end

go

execute spcCVASolicitacaoDeCompras 0,'2000-01-01','2018-12-30','O',0,'BRUNO','ROBERTA',0

--select 'O' Codigo, 'Aberto' Nome union all select 'C' Codigo, 'Fechado' Nome  union all select '*' Codigo, 'Todos' Nome  order by 1
--select distinct T0.ReqName Codigo ,T0.ReqName Nome from OPRQ T0 union all select '*' Codigo, 'Todos' Nome  order by 1

--select T0.Code Codigo,T0.Name Nome from OUDP T0 union all select 0 Codigo, 'Todos' Nome  order by 1

--select T0.BPLId,t0.BPLName from OBPL T0 union all select 0 Codigo, 'Todos' Nome  order by 1


--select '-' Codigo,'Sem esquema de Autorização'  Nome union all select 'W','Pendente' union all select 'Y','Aprovado'  union all select 'N','Rejeitado' union all select 'P','Gerada' union all select 'A','Gerado pelo Autorizador'union all select 'C','Cancelado'  union all select '*' Codigo, 'Todos' Nome  order by 1


--create table CVAStatusAutorizacao
--(
--	Codigo NVarchar(max)
--	,Nome NVarchar(max)
--)

--insert into CVAStatusAutorizacao

--select '-' Codigo,'Sem esquema de Autorização'  Nome union all select 'W','Pendente' union all select 'Y','Aprovado'  union all select 'N','Rejeitado' union all select 'P','Gerada' union all select 'A','Gerado pelo Autorizador'union all select 'C','Cancelado'  union all select '*' Codigo, 'Todos' Nome  order by 1


--select Codigo,Nome from CVAStatusAutorizacao order by 1