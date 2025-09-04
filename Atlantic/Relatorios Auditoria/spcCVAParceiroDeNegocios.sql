USE SBO_PRD_CFI0002
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcCVAParceiroDeNegocios]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].spcCVAParceiroDeNegocios
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE spcCVAParceiroDeNegocios
	@CardType nvarchar(1)
	,@UF as nvarchar(max)
	,@Grupo int
	,@Status as nvarchar(1)
	,@CArdCode as nvarchar(max)
	,@CArdRazaoSocial as nvarchar(max)
	,@CArdNomeFantasia as nvarchar(max)	
	,@Country as nvarchar(max)
	--,@state as nvarchar(max)
	,@County int
WITH ENCRYPTION 
AS 
BEGIN 
	select 
		ROW_NUMBER() OVER(ORDER BY TB.CardCode ASC) AS Item
		,*
	 from (
	select distinct --top 100

		upper(case when T0.validFor ='Y' then 'Ativo' when T0.validFor ='N'  then 'Inativo' end) 'Status'
		,upper(T1.GroupName) 'Grupo'
		,upper(T0.CardCode) 'Cod'
		,upper(T0.CardName) 'Razão Social'
		,upper(coalesce(cast(T0.AliasName as nvarchar(max)),'') )'Nome Fantasia'
		,coalesce(T2.TaxId0,'') 'CNPJ'
		,coalesce(T2.TaxId4,'') 'CPF'
		,upper(coalesce(T2.TaxId1,'')) 'Insc. Estadual'
		,upper(coalesce(T2.TaxId3,'')) 'Insc. Municipal'
		,upper(T3.City) 'Cidade'
		,upper(T3.State) 'UF'
		,(coalesce(T0.Phone1,'') + ' ' + coalesce(T0.Phone2,'')) Telefone
		,upper(CASE when T0.CardType='C' then 'Cliente' when T0.CardType='S' then 'Fornecedor' when T0.CardType='L' then 'Lead' end) Tipo
		,upper(T0.CardCode) CardCode
	from 
		OCRD T0
		inner join OCRG T1 on T1.GroupCode=T0.GroupCode
		inner join CRD7 T2 on  T2.CardCode=T0.CardCode and (not T2.TaxId0 is null or not T2.TaxId4 is null) and coalesce(T2.Address,'')=''
		inner join CRD1 T3 on T3.CardCode=T0.CardCode and T3.Address=T0.ShipToDef
	where
		(T0.CardType=@CardType or @CardType='*')
		and (@UF='*' or @UF=T3.State)
		and (@Grupo=0 or @Grupo=T1.GroupCode  )
		and (@Status='*' or @Status=T0.validFor)
		and (@CArdCode='*' or ( @CArdCode<>'*' and @CArdCode=T0.CardCode ) )
		and (@CArdRazaoSocial='*' or (@CArdRazaoSocial<>'*' and T0.CardName like '%'+ @CArdRazaoSocial +'%'))
		and (@CArdNomeFantasia='*' or (@CArdNomeFantasia<>'*' and T0.CardFName like '%'+ @CArdRazaoSocial +'%'))		
		and (@Country='*' or T3.Country=@Country)
		and (@County=0 or @County=T3.County)
		--and (@state='*' or T3.state=@state)
	) as TB
	order by 
		CardCode
		--select * from CRD7
	--where T0.CardCode='F00002144734739'
		
end

go

execute spcCVAParceiroDeNegocios '*','*',0,'*','*','*','*','*',0
--cardcode
--select Cardcode Codigo,CardName Nome from OCRD  union all select '*','Todos' order by 1

--Pais
--select top 10 Country from CRD1
--select Code Codigo,Name Nome from OCRY union all select '*','Todos' order by 1

--UF
--select top 10 state from CRD1
--select Code Codigo,Name Nome from OCST union all select '*','Todos' order by 1

--cidade
--select top 10 County from CRD1
--select AbsId Codigo,Name Nome from OCNT  union all select 0,'Todos' order by 1


--select 'C' Codigo,'Cliente' Nome union all select 'S' Codigo,'Fornecedor' Nome union all select 'L' Codigo,'Lead' Nome union all select '*' Codigo,'Todos' Nome order by 1
--select Code Codigo,Name Nome from OCST union all select '*' Codigo,'Todos' Nome order by 1 
--select T0.GroupCode Codigo,T0.GroupName from OCRG T0 union all select 0 Codigo,'Todos' Nome order by 1
--select 'Y' Codigo, 'Ativo' Nome union all select 'N' Codigo, 'Inativo' union all select '*' Codigo,'Todos' Nome order by 1 

--select distinct T0.validFor from OCRD T0


--SELECT * FROM OCRD where cardcode='02513254902'

--select * from CRD7 where cardcode='C00000000430285'