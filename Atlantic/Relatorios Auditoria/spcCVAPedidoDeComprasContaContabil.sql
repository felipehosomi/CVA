USE SBO_PRD_CFI0002
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcCVAPedidoDeComprasContaContabil]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].spcCVAPedidoDeComprasContaContabil
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE spcCVAPedidoDeComprasContaContabil
	@DocNum int
	,@CArdCode as nvarchar(max)
	,@CArdRazaoSocial as nvarchar(max)
	,@CArdNomeFantasia as nvarchar(max)		
	,@CNPJ as nvarchar(max)
	,@Situacao as nvarchar(max)	
	,@SituacaoEntrega as nvarchar(max)	
	,@BplId int	
	,@ContaContabil as nvarchar(max)
	,@DataIni as DAte
	,@DataFIM as DAte	
	,@SolicitanteIni as nvarchar(max)
	,@SolicitanteFim as nvarchar(max)
	,@AprovadorIni as nvarchar(max)
	,@AprovadorFim as nvarchar(max)	
	
WITH ENCRYPTION 
AS 
BEGIN
	select * from (
		select distinct
			  --T0.DocEntry
			  T0.DocNum as 'Nº PC'
			  ,T0.CardCode as 'COD'
			  ,UPPER(T0.CardName) as 'FOERNECEDOR'
			  ,UPPER(case when T0.WddStatus = '-' then 'Sem' 
					when T0.WddStatus = 'W' then 'Pendente'
					when T0.WddStatus = 'Y' then 'Aprovado'
					when T0.WddStatus = 'N' then 'Rejeitado'
					when T0.WddStatus = 'P' then 'Gerado'
					when T0.WddStatus = 'A' then 'Gerado pelo Autorizador' 
					when T0.WddStatus = 'C' then 'Cancelado'
					else '' end) as 'SITUAÇÃO' 
						  
			  ,UPPER(case when (select count(TargetType) from por1 where TargetType = 20 and DocEntry = T0.DocEntry) = 0 then 'Aguardando Entrega' 
					when (select count(TargetType) from por1 where TargetType = 20 and DocEntry = T0.DocEntry) > 0 and T0.DocStatus = 'O'  then 'Entregue Parcialmente'
					when (select count(TargetType) from por1 where TargetType = 20 and DocEntry = T0.DocEntry) > 0 and T0.DocStatus = 'C'  then 'Entregue Totalmente'    
			  else 'Não encaixa na regra da especificação' end) as 'SITUAÇÃO(ENTREGA)'
			  	  
			  ,T0.CreateDate  as 'DATA OPERAÇÃO'
			  ,T0.DocDate as 'DATA PC'
			  ,T0.DocTotal as 'VALOR TOTAL'
			  ,UPPER(T2.BPLName) as 'FILIAL REQUISIÇÃO'
			  ,UPPER(T1.SlpName) as 'SOLICITANTE'
			  ,UPPER((select dbo.fncCVAListaDeAprovadores (T0.ObjType,T0.DocEntry))) APROVADOR
			  ,coalesce(T4.TaxId0,'') 'CNPJ'
			  ,UPPER((SELECT TOP 1 POR1.AcctCode +' - ' + Acctname  FROM POR1  inner join  OACT on POR1.AcctCode  =OACT.acctcode WHERE POR1.DOCENTRY=T0.DocEntry)) [CONTA CONTÁBIL]
		from 
		
		
		
			OPOR T0
			inner join OSLP T1 on T0.SlpCode = T1.SlpCode 
			inner join OBPL T2 on T0.BPLId=T2.BPLId
			inner join OCRD T3 on T3.CardCode=T0.CardCode
			left join CRD7 T4 on  T4.CardCode=T0.CardCode and (not T4.TaxId0 is null or not T4.TaxId4 is null) --and coalesce(T4.Address,'')=''
		where
			T0.CANCELED<>'Y' and T0.CANCELED<>'C'
			and (@DocNum=0 or @DocNum=T0.DocNum )
			and (@CArdCode='*' or ( @CArdCode<>'*' and @CArdCode=T3.CardCode ) )
			and (@CArdRazaoSocial='*' or (@CArdRazaoSocial<>'*' and T3.CardName like '%'+ @CArdRazaoSocial +'%'))
			and (@CArdNomeFantasia='*' or (@CArdNomeFantasia<>'*' and T3.CardFName like '%'+ @CArdRazaoSocial +'%'))
			
			and ( T0.WddStatus=@Situacao or @Situacao='*' )			
			
			and cast(T0.CreateDate as DAte) between @DataIni and @DataFIM	
			and (@BplId=0 or @BplId=T0.BPLId)		
			and (
					(  @SolicitanteIni='Todos' and @SolicitanteFim='Todos' )
					or (  T1.SlpName>=@SolicitanteIni and T1.SlpName<=@SolicitanteFim )
				)
			and (@CNPJ='*' or coalesce(T4.TaxId0,'')='*')
	) as TB
	where
		(UPPER(@SituacaoEntrega)='*' or UPPER(@SituacaoEntrega)=TB.[SITUAÇÃO(ENTREGA)])
		and (
				(  @AprovadorIni='Todos' and @AprovadorFim='Todos' )
				or (  TB.Aprovador>=@AprovadorIni and TB.Aprovador<=@AprovadorFim)
			)
		and (UPPER(@ContaContabil)='*' or upper(@ContaContabil)=[CONTA CONTÁBIL])
end

go

execute spcCVAPedidoDeComprasContaContabil 0,'*','*','*','*','*','*',0 ,'*','2018-01-01','2018-12-30','Todos','Todos','Todos','Todos'

--select Codigo,Nome from CVAStatusAutorizacao order by 1
--select Codigo,Nome from CVASSituacaoEntrega order by 1
	--@CArdCode as nvarchar(max)
	--,@CArdRazaoSocial as nvarchar(max)
	--,@CArdNomeFantasia as nvarchar(max)		
	--,@CNPJ as nvarchar(max)
	--,@Situacao as nvarchar(max)	
	--,@SituacaoEntrega as nvarchar(max)	
	--,@BplId int	
	
	--,@ContaContabil as nvarchar(max)
	--,@DataIni as DAte
	--,@DataFIM as DAte	
	--,@SolicitanteIni as nvarchar(max)
	--,@SolicitanteFim as nvarchar(max)
	--,@AprovadorIni as nvarchar(max)
	--,@AprovadorFim as nvarchar(max)	
	


--create table CVASSituacaoEntrega
--(
--	Codigo NVarchar(max)
--	,Nome NVarchar(max)
--)

--insert into CVASSituacaoEntrega
--select 'Aguardando Entrega' as Codigo, 'Aguardando Entrega' as Nome union all select 'Entregue Parcialmente'  as Codigo,'Entregue Parcialmente' as Nome union all select 'Entregue Totalmente'  as Codigo,'Entregue Totalmente' as Nome union all select '*', 'Todos' order by 1 

--select Codigo,Nome from CVASSituacaoEntrega order by 1
--select * from OPQT
--status
--select 'O' Codigo, 'Aberto' Nome union all select 'C' Codigo, 'Fechado' Nome  union all select '*' Codigo, 'Todos' Nome  order by 1

--Solicitante
--select T0.SlpName Codigo,T0.SlpName Nome from OSLP T0 union all select 'Todos', 'Todos' order by 1


--Departamento
--select T0.Name Codigo,T0.Name Nome from OUDP T0 union all select 'Todos' Codigo, 'Todos' Nome  order by 1


----aprovadores
--select USER_CODE Codigo,USER_CODE Nome from OUSR  union all select 'Todos' Codigo, 'Todos' Nome  order by 1

--select Codigo,Nome from CVAStatusAutorizacao order by 1

--Fornecedores
--select T0.CardCode,T0.CardName from OCRD T0 where T0.CardType='S' union all select '*' Codigo, 'Todos' Nome  order by 1

--select Codigo,Nome from (select '*' Codigo, 'Todos' Nome  union all select T0.AcctCode Codigo,T0.AcctName Nome from OACT T0 ) as TB order by 1