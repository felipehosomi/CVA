if object_id('dbo.spc_CVA_PagarComissao', 'P') is not null
	drop procedure [spc_CVA_PagarComissao]
go
create procedure dbo.spc_CVA_PagarComissao(@detalhado char(1) = 'Y', @vendedor int = 0, @dataInicial datetime, @dataFinal datetime)
as 
begin

	if @detalhado = 'Y'
		select 
			T0.U_COMISSIONADO,
			T0.U_CARDCODE,
			T0.U_CARDNAME,
			T0.U_REGRA,
			T0.U_DOCDATE,
			T0.U_DUEDATE,
			T0.U_OBJTYPE,
			T0.U_DOCENTRY,
			T0.U_LINENUM,
			T0.U_ITEMCODE,
			T0.U_ITEMNAME,
			T0.U_CENTROCUSTO,
			T0.U_VALOR,
			T0.U_PARCELA,
			T0.U_IMPOSTOS,
			T0.U_COMISSAO,
			T0.U_VALORCOMISSAO
		from [@CVA_CALC_COMISSAO] T0
		where T0.U_PAGO = 'N'
			and T0.U_DOCDATE >= @dataInicial 
			and T0.U_DOCDATE <= @dataFinal
			and (T0.U_COMISSAO = @vendedor or @vendedor = 0)
	else
		select
			T0.U_COMISSIONADO,
			T1.SlpName,
			sum(distinct T0.U_VALORCOMISSAO) as U_VALORCOMISSAO
		from [@CVA_CALC_COMISSAO] T0
			inner join OSLP T1 on T0.U_COMISSIONADO = T1.SlpCode
		where T0.U_PAGO = 'N'
			and T0.U_DOCDATE >= @dataInicial 
			and T0.U_DOCDATE <= @dataFinal
			and (T0.U_COMISSAO = @vendedor or @vendedor = 0)
		group by T0.U_COMISSIONADO, T1.SlpName
end