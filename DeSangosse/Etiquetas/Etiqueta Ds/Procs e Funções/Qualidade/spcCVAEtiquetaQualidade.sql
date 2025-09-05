--use SBO_IMEPEL_0804

alter proc spcCVAEtiquetaQualidade
	       
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

 declare @p					varchar(250)
 declare @produto			varchar(250)
 declare @p_1				varchar(250)
 declare @responsavel		varchar(250)
 declare @codigo			varchar(250)
 declare @lote				varchar(250)
 declare @item				varchar(250)

 declare @dt_validade		varchar(250)
 declare @dt_fabri			varchar(250)
 declare @dt_analise		varchar(250)

 declare @contador			int

 create table #result 
 (
    id int not null identity
  , tag varchar(8000) null
 )



  select @script  = U_script
    from [@CVA_ETIQUETA]
   where code = @layout		

  create table #obj (obj varchar(50) null, ordem int not null identity)
  create table #Item (item varchar(50) null, ordem int not null identity)
	 
  insert into #obj(obj)
   select splitdata  from dbo.fncArkabSplitString(@objtype, '|')

  select @item = obj from #obj where ordem = 2


 


select @codigo = T0.ItemCode
	  ,@produto=  T0.itemName
      ,@lote = T0.DistNumber
	  ,@dt_analise = Convert(varchar(10),T0.InDate ,103)
	  ,@dt_fabri = Convert(varchar(10),T0.MnfDate ,103)
	  ,@dt_validade =  Convert(varchar(10),T0.ExpDate ,103)
	  ,@responsavel = null
 from OBTN T0
 left join BEAS_QSFTHAUPT T1 on T0.DistNumber = T1.BatchNum and T0.ItemCode = T1.ItemCode
	--inner join OCRD on oinv.cardcode = ocrd.cardcode
   where T0.itemcode = @item
     and T0.Distnumber = @key



	/* Dividir linhas do Produto */

insert into #Item(item)
    select splitdata  from dbo.fncArkabSplitString(@produto, '')	

	select @produto = item from #Item where ordem = 1
	select @produto = @produto + ' ' + item from #Item where ordem = 2
	select @produto = @produto + ' ' + item from #Item where ordem = 3
	select @produto = @produto + ' ' + item from #Item where ordem = 4
	select @produto = @produto + ' ' + item from #Item where ordem = 5

	select @p = item from #Item where ordem = 6
	select @p = @p + ' ' + item from #Item where ordem = 7
	select @p = @p + ' ' + item from #Item where ordem = 8
	select @p = @p + ' ' + item from #Item where ordem = 9
	select @p = @p + ' ' + item from #Item where ordem = 10
	
	
	 

	set @script = replace(@script, '@codigo',isnull(@codigo,''))
	set @script = replace(@script, '@produto',isnull(@produto,''))
	set @script = replace(@script, '@p_1',isnull(@p,''))
	set @script = replace(@script, '@lote',isnull(@lote,''))
	set @script = replace(@script, '@dt_analise',isnull(@dt_analise,''))
	set @script = replace(@script, '@dt_fabri',isnull(@dt_fabri,''))
	set @script = replace(@script, '@dt_validade',isnull(@dt_validade,''))
	set @script = replace(@script, '@responsavel',isnull(@responsavel,''))
	set @script = replace(@script ,'@quantity', isnull(@quantity,1))

	set @contador = 1

	while @contador <= @quantity
	 begin	 
			insert into #result (tag)			
			select @script
			

			set @contador += 1
	 end

	  select Id, Tag from #result order by id

	  drop table #obj	  
	  drop table #result
end

go
 exec spcCVAEtiquetaQualidade '191','10000044|1311','096/16',3,null,null,null,null,null,null,null,null

