USE SBO_PRD_CFI0002
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcCVAControleFinanceiroCompras]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].spcCVAControleFinanceiroCompras
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE spcCVAControleFinanceiroCompras
	@DocNumPC int
	,@DataIni as DAte
	,@DataFIM as DAte
	,@CArdCode as nvarchar(max)
	,@CArdRazaoSocial as nvarchar(max)
	,@CArdNomeFantasia as nvarchar(max)
	,@DocStatus as nvarchar(1)
	,@Ordenacao as nvarchar(max)
	,@NotaFiscal as nvarchar(max)
	,@SituacaoEntrega as nvarchar(max)
	,@BplId int
	--,@DocStatus as nvarchar(1)
	--,@Solicitante as int
	--,@Departamento as int
WITH ENCRYPTION 
AS 
BEGIN
	create table #tempTable(
		Fornecedor nvarchar(max)
		,Situação nvarchar(max)
		,[Nº PC] int
		,Data DateTime
		,Vencimento DateTime
		,[Valor Total] double precision
		,[Filial de Cobrança] nvarchar(max)
		,DocEntry int
		,DocEntryNF int
		,Serial int 
		,SituacaoEntrega nvarchar(max)
	)
	insert into #tempTable
	select distinct
		upper((T4.CArdCode + ' - ' +T4.CardName)) 'Fornecedor'
		,upper(case 
			when T3.WddStatus= '-' then 'Sem Processo de Autorização' 
			when T3.WddStatus= 'W' then 'Pendente'
			when T3.WddStatus= 'Y' then 'Aprovado' 
			when T3.WddStatus= 'N' then 'Rejeitado' 
			when T3.WddStatus= 'P' then 'Gerada' 
			when T3.WddStatus= 'A' then 'Gerado pelo Autorizador'
			when T3.WddStatus= 'C' then 'Cancelado' 
		end) 'Situação'		
		,T3.DocNum 'Nº PC'
		,T3.DocDate 'Data'
		,T3.DocDueDate 'Vencimento'
		,T3.DocTotal 'Valor Total'
		,upper(T5.BPLName) 'Filial de Cobrança'
		,T3.DocEntry
		,null --coalesce(T0.DocEntry,0) DocEntryNF
		,null --T0.Serial		
		--,null
		,upper(case 
			when (select sum(OpenQty) from POR1 where POR1.docentry=T3.docentry)=0 then  'Entregue Totalmente' 
			when (select sum(OpenQty) from POR1 where POR1.docentry=T3.docentry)=(select sum(quantity) from POR1 where POR1.docentry=T3.docentry) then  'Aguardando Entrega' 
			when (select sum(OpenQty) from POR1 where POR1.docentry=T3.docentry)<(select sum(quantity) from POR1 where POR1.docentry=T3.docentry) then  'Entregue Parcialmente' 
			
		end) as 'SITUAÇÃO(ENTREGA)'	
		  --,UPPER(case when (select count(TargetType) from por1 where TargetType = 20 /*and DocEntry = T0.DocEntry*/) = 0 then 'Aguardando Entrega' 
				--when (select count(TargetType) from por1 where TargetType = 20 and DocEntry = T0.DocEntry) > 0 and T0.DocStatus = 'O'  then 'Entregue Parcialmente'
				--when (select count(TargetType) from por1 where TargetType = 20 and DocEntry = T0.DocEntry) > 0 and T0.DocStatus = 'C'  then 'Entregue Totalmente'    
		  --else 'Não encaixa na regra da especificação' end) as 'SITUAÇÃO(ENTREGA)'		
	from 
		OPOR T3		
		--inner join POR1 T2 on  T3.DocEntry= T2.DocEntry
		inner join OCRD T4 on T4.CardCode=T3.CardCode
		inner join OBPL T5 on T5.BPLId=T3.BPLId
				
		--left join PCH1 T1 on T1.BaseType=T3.ObjType and T1.BaseLine=T2.LineNum and T2.DocEntry=T1.BaseEntry --and not exists ( select 1 from OPCH where OPCH.Docentry=T1.DocEntry and OPCH.CANCELED='N' /*and OPCH.CANCELED<>'C'*/)
		--left join OPCH T0 on T1.DocEntry=T0.DocEntry --and T0.CANCELED='N' --and T0.CANCELED<>'C'
	where
		T3.CANCELED<>'Y' and T3.CANCELED<>'C'
		and T3.DocDate between @DataIni and @DataFIM
		and (@DocNumPC=0 or T3.DocNum=@DocNumPC)
		
		and (@CArdCode='*' or ( @CArdCode<>'*' and @CArdCode=T4.CardCode ) )
		and (@CArdRazaoSocial='*' or (@CArdRazaoSocial<>'*' and T4.CardName like '%'+ @CArdRazaoSocial +'%'))
		and (@CArdNomeFantasia='*' or (@CArdNomeFantasia<>'*' and T4.CardFName like '%'+ @CArdRazaoSocial +'%'))
		
		and (  @DocStatus='*' or @DocStatus=T3.DocStatus)
		--and (
		--		@NotaFiscal='Todos' 
		--		or (@NotaFiscal='Com Nota Fiscal' and Not T0.DocEntry  is null )
		--		or (@NotaFiscal='Sem Nota Fiscal' and T0.DocEntry  is null )
		--	)
		and (@BplId=0 or @BplId=T3.BPLId)
		--and T0.CANCELED='N'
	if @Ordenacao='Data do PC' begin
		select * from #tempTable where @SituacaoEntrega=SituacaoEntrega or @SituacaoEntrega='*' order by Data
	end
	else if @Ordenacao='Fornecedor' begin
		select * from #tempTable where @SituacaoEntrega=SituacaoEntrega or @SituacaoEntrega='*' order by Fornecedor
	end	
	else if @Ordenacao='Valor' begin
		select * from #tempTable where @SituacaoEntrega=SituacaoEntrega or @SituacaoEntrega='*' order by [Valor Total]
	end		
	drop table #tempTable 
end

go

execute spcCVAControleFinanceiroCompras 746,'2000-01-01','2018-12-30','*','*','*','*','Data do PC','Todos','*',0

--@DocNumPC int
--,@DataIni as DAte
--,@DataFIM as DAte
--,@CArdCode as nvarchar(max)
--,@CArdRazaoSocial as nvarchar(max)
--,@CArdNomeFantasia as nvarchar(max)
--,@DocStatus as nvarchar(1)
--,@Ordenacao as nvarchar(max)
--,@NotaFiscal as nvarchar(max)
--,@SituacaoEntrega as nvarchar(max)
--,@BplId int

--execute spcCVAControleFinanceiroCompras 0,'2000-01-01','2018-12-30','*','*','*','*','Data do PC','Todos','Todos',0
--ordenacao
--select 'Data do PC' Nome,'Data do PC' Codigo  union all select 'Fornecedor','Fornecedor' union all select 'Valor','Valor'
--nota fiscal
--select 'Com Nota Fiscal' Codigo,'Com Nota Fiscal' Nome union all select 'Sem Nota Fiscal','Sem Nota Fiscal' union all select 'Todos','Todos'

--select * from INV6
--select * from OPOR T0 where T0.DocNum=3

--CardCode
--select T0.CardCode Codigo,T0.CardName Nome from OCRD T0 union all select '*','Todos' order by 1




--select * from OPCH where DocEntry=1819




--select * from POR1 where DocEntry=746