USE SBO_PRD_CFI0002
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcCVAPContratoGuardaChuvaCompras]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].spcCVAPContratoGuardaChuvaCompras
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE spcCVAPContratoGuardaChuvaCompras
	@Inicio as DAte
	,@Fim as DAte
	,@InicioAssinatura as DAte
	,@FimAssinatura as DAte	
	,@Status nvarchar(1)
	,@Tipo nvarchar(1)
	,@Metodo nvarchar(1)
	,@CArdCode as nvarchar(max)
	,@CArdRazaoSocial as nvarchar(max)
	,@CArdNomeFantasia as nvarchar(max)		
	,@Owner as int
WITH ENCRYPTION 
AS 
BEGIN 
	select
		T0.Number 'Nº CGC'
		,upper(case when T0.Status='A' then 'Aprovado' when T0.Status='F' then 'Em Espera' when T0.Status='D' then 'Esboço' when T0.Status='T' then 'Terminado' when T0.Status='C' then 'Cancelado'  end) 'Status'
		,upper(T0.BpCode) 'COD'
		,upper(T0.BpName) 'Fornecedor '
		,upper(case when T0.Type='G'  then 'Geral' when T0.Type='S' then 'Específico' end) 'Tipo de Contrato'	
		,upper(CASE when T0.Method='I' then 'Método de Itens' when T0.Method='M' then 'Método Monetário' end) 'Método'
		,cast(T0.StartDate as DAte) 'Data Início'
		,cast(T0.EndDate as date)	'Data Final'
		,cast(T0.SignDate as date) 'Data Assinatura'
		,cast(T0.TermDate as date) 'Data Rescisão'
		,coalesce((select SUM (T1.PlanQty *  T1.UnitPrice  )  from OAT1 T1 where T0.AbsID=T1.AgrNo  ),0) 'Valor'
		,(coalesce((select SUM (T1.PlanQty *  T1.UnitPrice  )  from OAT1 T1 where T0.AbsID=T1.AgrNo  ),0)-coalesce((select SUM (T1.CumQty *  T1.CumAmntLC  )  from OAT1 T1 where T0.AbsID=T1.AgrNo  ),0)) 'Saldo'
	from 
		OOAT T0
		inner join OCRD T2 on T2.Cardcode=T0.BpCode
		left join OHEM T1 on T1.empID=T0.Owner		
	where
		T0.BpType='S'
		and cast(T0.StartDate as DAte) between @Inicio and @Fim
		and cast(T0.SignDate as date) between @InicioAssinatura and @FimAssinatura
		and (@Status='*' or @Status=T0.Status)
		and (@Tipo='*' or @Tipo=T0.Type)
		and (@Metodo='*' or @Metodo=T0.Method)
		and (@CArdCode='*' or ( @CArdCode<>'*' and @CArdCode=T2.CardCode ) )
		and (@CArdRazaoSocial='*' or (@CArdRazaoSocial<>'*' and T2.CardName like '%'+ @CArdRazaoSocial +'%'))
		and (@CArdNomeFantasia='*' or (@CArdNomeFantasia<>'*' and T2.CardFName like '%'+ @CArdRazaoSocial +'%'))
		and (@Owner=0 or @Owner=T0.Owner)		
	order by 1		
end

go

execute spcCVAPContratoGuardaChuvaCompras '2000-01-01','2019-01-01','2000-01-01','2019-01-01','*','*','*','*','*','*',0
--titular
--select empID Codigo,firstName as Nome from OHEM  union all select 0, 'Todos' order by 1

----Status
--select 'A' Codigo,'Aprovado' Nome union all select 'F','Em Espera' union all select 'D','Esboço' union all select 'T','Terminado' union all select 'C','Cancelado' union all select '*', 'Todos' order by 1

----Tipo de Contrato
--select 'G' Codigo ,'Geral' Nome union all select 'S','Específico' union all select '*', 'Todos' order by 1

----método de acordo
--select 'I' Codigo ,'Método de Itens' Nome union all select 'M','Método Monetário' union all select '*', 'Todos' order by 1

--select 
--	T1.PlanQty 'Planijado'
--	,T1.UnitPrice 'PReco'
--	,T1.CumQty 'Comprada'
--	,T1.CumAmntLC 'ValorComprado'
--from 
--	OOAT T0
--	inner join OAT1 T1 on  T0.AbsID=T1.AgrNo
--where 
--	T0.BpType='S'

--select * from OAT4


--select * from 		OOAT T0
--		left join OHEM T1 on T1.empID=T0.Owner
--		inner join OCRD T2 on T2.Cardcode=T0.BpCode