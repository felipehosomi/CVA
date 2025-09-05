--use SBO_IMEPEL_0804

alter proc spcCVAEtiquetaLogistica
	       
  @layout varchar(72)
, @objtype varchar(72)
, @key varchar(72)
, @quantity int
, @campolivre1 varchar(250)
, @campolivre2 varchar(250)
, @campolivre3 varchar(250)
, @campolivre4 varchar(250)
, @campolivre5 varchar(250)
, @campolivre6 varchar(250)
, @campolivre7 varchar(250)
, @campolivre8 varchar(250)

as
Begin


 declare @script			nvarchar(max)  
 declare @Buffer			nvarchar(max)  

 declare @cliente			varchar(250)
 declare @cidade_cliente	varchar(250)
 declare @cliente_1			varchar(250)
 declare @cliente_2			varchar(250)
 declare @endereco		    varchar(250)
 declare @endereco_1	    varchar(250)
 declare @endereco_2		varchar(250)
 declare @horario		    varchar(250)
 declare @obs_1				varchar(250)
 declare @obs_2				varchar(250)
 declare @obs_3				varchar(250) 
 declare @fornecedor        varchar(250)
 declare @cidade_fornecedor varchar(250)

 declare @vol				int
 declare @vol_1				int
 declare @n_nf		        int
 declare @contador			int
 declare @controle			int
 declare @volumeAte			int
 declare @volumeDe			int

 create table #result 
 (
    id int not null identity
  , tag varchar(8000) null
 )

  create table #volume
 (
    id int not null identity
  , volumeDe int null
  ,volumeAte int
 )

 insert into #volume (volumeDe,volumeAte)
 select @campolivre1,@campolivre2


  select @script  = U_script
    from [@CVA_ETIQUETA]
   where code = @layout		

   --select @script

    create table #Cliente (cliente varchar(50) null, ordem int not null identity)
	create table #Endereco (endereco varchar(50) null, ordem int not null identity)
	create table #obs (obs varchar(50) null, ordem int not null identity)
	
	select @cliente = T0.cardName
	  ,@cidade_cliente = T2.City + ' / '+T2.State 
	  ,@endereco = T2.AddrType + T2.Street +' , '+ T2.StreetNo +' , '+ T2.Block + ', CEP: '+ t2.zipcode
	  ,@n_nf = T0.serial
	  ,@obs_1 = T1.U_akbobsexp 
	  ,@horario = T1.U_akbhorario 
	  ,@fornecedor = (select CompnyNAme from OADM)
	  ,@cidade_fornecedor = (select City + ' / '+ State from ADM1)
  from OINV T0
 inner join OCRD T1 on T0.CardCode = T1.CardCode
 inner join CRD1 T2 on T1.cardcode = T2.Cardcode 
 where t0.serial = '14387'
   and T0.objtype = '13'

----------------------------------------------------------------------------------------
--> Dividindo o campo Ciente em duas linhas
----------------------------------------------------------------------------------------
    insert into #Cliente (Cliente)
    select splitdata  from dbo.fncArkabSplitString(@cliente, '')

	select @Cliente_1 = cliente from #Cliente where ordem = 1
	select @Cliente_1 = @Cliente_1 + ' ' + cliente from #Cliente where ordem = 2
	select @Cliente_1 = @Cliente_1 + ' ' + cliente from #Cliente where ordem = 3
	select @Cliente_1 = @Cliente_1 + ' ' + cliente from #Cliente where ordem = 4
	select @Cliente_1 = @Cliente_1 + ' ' + cliente from #Cliente where ordem = 5
	select @Cliente_1 = @Cliente_1 + ' ' + cliente from #Cliente where ordem = 6

	select @Cliente_2 = cliente from #cliente where ordem = 7
	select @Cliente_2 = @Cliente_2 + ' ' + cliente from #cliente where ordem = 8
	select @Cliente_2 = @Cliente_2 + ' ' + cliente from #cliente where ordem = 9
	select @Cliente_2 = @Cliente_2 + ' ' + cliente from #cliente where ordem = 10
	
----------------------------------------------------------------------------------------
--> Dividindo o campo endereço em duas linhas
----------------------------------------------------------------------------------------
    insert into #endereco (endereco)
    select splitdata  from dbo.fncArkabSplitString(@endereco, '')

	select @endereco_1 = endereco from #endereco where ordem = 1
	select @endereco_1 = @endereco_1 + ' ' + endereco from #endereco where ordem = 2
	select @endereco_1 = @endereco_1 + ' ' + endereco from #endereco where ordem = 3
	select @endereco_1 = @endereco_1 + ' ' + endereco from #endereco where ordem = 4
	select @endereco_1 = @endereco_1 + ' ' + endereco from #endereco where ordem = 5
	select @endereco_1 = @endereco_1 + ' ' + endereco from #endereco where ordem = 6

	select @endereco_2 = endereco from #endereco where ordem = 7
	select @endereco_2 = @endereco_2 + ' ' + endereco from #endereco where ordem = 8
	select @endereco_2 = @endereco_2 + ' ' + endereco from #endereco where ordem = 9
	select @endereco_2 = @endereco_2 + ' ' + endereco from #endereco where ordem = 10
----------------------------------------------------------------------------------------
--> Dividindo o campo obs em tres linhas
----------------------------------------------------------------------------------------
    insert into #obs (obs)
    select splitdata  from dbo.fncArkabSplitString(@obs_1, '')

	select @obs_1 = obs from #obs where ordem = 1
	select @obs_1 = @obs_1 + ' ' + obs from #obs where ordem = 2
	select @obs_1 = @obs_1 + ' ' + obs from #obs where ordem = 3
	select @obs_1 = @obs_1 + ' ' + obs from #obs where ordem = 4
	select @obs_1 = @obs_1 + ' ' + obs from #obs where ordem = 5
	select @obs_1 = @obs_1 + ' ' + obs from #obs where ordem = 6
	select @obs_1 = @obs_1 + ' ' + obs from #obs where ordem = 7
	select @obs_1 = @obs_1 + ' ' + obs from #obs where ordem = 8
	select @obs_1 = @obs_1 + ' ' + obs from #obs where ordem = 9
	select @obs_1 = @obs_1 + ' ' + obs from #obs where ordem = 10

	select @obs_2 = obs from #obs where ordem = 11
	select @obs_2 = @obs_2 + ' ' + obs from #obs where ordem = 12
	select @obs_2 = @obs_2 + ' ' + obs from #obs where ordem = 13
	select @obs_2 = @obs_2 + ' ' + obs from #obs where ordem = 14
	select @obs_2 = @obs_2 + ' ' + obs from #obs where ordem = 15
	select @obs_2 = @obs_2 + ' ' + obs from #obs where ordem = 16
	select @obs_2 = @obs_2 + ' ' + obs from #obs where ordem = 17
	select @obs_2 = @obs_2 + ' ' + obs from #obs where ordem = 18
	select @obs_2 = @obs_2 + ' ' + obs from #obs where ordem = 19

	select @obs_3 = obs from #obs where ordem = 20
	select @obs_3 = @obs_3 + ' ' + obs from #obs where ordem = 21
	select @obs_3 = @obs_3 + ' ' + obs from #obs where ordem = 22
	select @obs_3 = @obs_3 + ' ' + obs from #obs where ordem = 23
	select @obs_3 = @obs_3 + ' ' + obs from #obs where ordem = 24
	select @obs_3 = @obs_3 + ' ' + obs from #obs where ordem = 25
	select @obs_3 = @obs_3 + ' ' + obs from #obs where ordem = 26
	select @obs_3 = @obs_3 + ' ' + obs from #obs where ordem = 27
	select @obs_3 = @obs_3 + ' ' + obs from #obs where ordem = 28

	set @script = replace(@script, '@cliente_1',isnull(@cliente_1,''))
	set @script = replace(@script, '@cliente_2',isnull(@cliente_2,''))
	set @script = replace(@script, '@cidade_cliente',isnull(@cidade_cliente,''))
	set @script = replace(@script, '@endereco_1',isnull(@endereco_1,''))	
	set @script = replace(@script, '@endereco_2',isnull(@endereco_2,''))
	set @script = replace(@script, '@n_nf',isnull(@n_nf,''))
	set @script = replace(@script, '@horario',isnull(@horario,''))
	set @script = replace(@script, '@obs_1',isnull(@obs_1,''))
	set @script = replace(@script, '@obs_2',isnull(@obs_2,''))
	set @script = replace(@script, '@obs_3',isnull (@obs_3,''))
	set @script = replace(@script, '@fornecedor',isnull (@fornecedor,''))
	set @script = replace(@script, '@cidade_fornecedor',isnull(@cidade_fornecedor,''))
	set @script = replace(@script, '@vol_1',isnull(@campoLivre2,''))

	/*set @script = replace(@script, 'Å','A')
	set @script = replace(@script, 'Ä','A')
	set @script = replace(@script, 'Ã','A')
	set @script = replace(@script, 'Â','A')
	set @script = replace(@script, 'Á','A')

	set @script = replace(@script, 'ä','a')
	set @script = replace(@script, 'ã','a')
	set @script = replace(@script, 'â','a')
	set @script = replace(@script, 'á','a')
	set @script = replace(@script, 'à','a')

	set @script = replace(@script, 'Ç','C')
	
	set @script = replace(@script, 'ç','c')

	set @script = replace(@script, 'Ë','E')
	set @script = replace(@script, 'Ê','E')
	set @script = replace(@script, 'É','E')
	set @script = replace(@script, 'È','E')

	set @script = replace(@script, 'Ï','I')
	set @script = replace(@script, 'Î','I')
	set @script = replace(@script, 'Í','I')
	set @script = replace(@script, 'Ì','I')

	set @script = replace(@script, 'Ö','O')
	set @script = replace(@script, 'Õ','O')
	set @script = replace(@script, 'Ô','O')
	set @script = replace(@script, 'Ó','O')
	set @script = replace(@script, 'Ò','O')

	set @script = replace(@script, 'Ü','U')
	set @script = replace(@script, 'Û','U')
	set @script = replace(@script, 'Ú','U')
	set @script = replace(@script, 'Ù','U')
	
	
	set @script = replace(@script, 'é','e')
	set @script = replace(@script, 'è','e')
	set @script = replace(@script, 'ì','i')
	set @script = replace(@script, 'ò','o')
			*/
	

	select @volumeAte = volumeAte from #volume
	select @volumeDe  = volumeDe from #volume

	while @volumeDe <= @volumeAte
	 begin
	 --select @contador
			set @Buffer =  replace(@script, '@vol',isnull(convert(varchar(10),@volumeDe),''))

			insert into #result (tag)
			--select [dbo].[FN_FORMATAR_TEXTO](@Buffer)

			
			select @Buffer
			

			set @volumeDe += 1
	 end

	  select Id, Tag from #result order by id

	  drop table #Cliente
	  drop table #Endereco
	  drop table #obs
	  drop table #result


end

go
 exec spcCVAEtiquetaLogistica '001','13','14387',1,'1','2',null,null,null,null,null,null

