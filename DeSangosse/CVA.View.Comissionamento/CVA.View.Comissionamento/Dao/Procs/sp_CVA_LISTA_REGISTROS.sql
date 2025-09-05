Create Proc SP_CVA_LISTA_REGISTROS
(
	 @dtInicial Date
	,@dtFinal   Date
	,@NfInicial int
	,@NfFinal	int
	,@DocInicial int
	,@DocFinal   int
)
 as
 begin

select  convert(bit,0) as '#'
       ,U_CARDCODE as 'Cód Cliente'
	   ,U_CARDNAME as 'Cliente'
	   ,U_REGRA	   as 'Regra Comissão'
	   ,U_DOCDATE  as 'Data Documento'
	   ,U_DUEDATE  as 'Data Vencimento'
	   ,U_TAXDATE  as 'Data Pagamento'
	   ,U_DOCENTRY as 'N° Documento'
	   ,U_ITEMCODE as 'Cód Item'
	   ,U_ITEMNAME as 'Item'
	   ,U_VALOR	   as 'Valor'
	   ,U_PARCELA  as 'Parcela'
	   ,U_IMPOSTOS as 'Imposto'
	   ,U_COMISSAO as ' Comissão (%)'
	   ,U_VALORCOMISSAO as ' Valor Comissão'
	   ,U_PAGO as 'Pago'
	   
  from [@CVA_CALC_COMISSAO]
  where( 
		 (U_DOCDATE between @dtInicial and @dtFinal)
			or 
		 (U_DOCENTRY between @NfInicial and @NfFinal)
			or
		 ( U_DOCENTRY between @DocInicial and @DocFinal)
		)

  end

  go

  exec sp_CVA_LISTA_REGISTROS '19000101','19000101',3448,3448,0,0



  