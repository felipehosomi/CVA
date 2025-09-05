
Create proc spcCVAEtiquetaRetem
	       
  @layout		varchar(72)
, @objtype		varchar(72)
, @key			varchar(72)
, @quantity		int
, @campolivre1	varchar(250)
, @campolivre2	varchar(250)
, @campolivre3	varchar(250)
, @campolivre4	varchar(250)
, @campolivre5	varchar(250)
, @campolivre6	varchar(250)
, @campolivre7	varchar(250)
, @campolivre8	varchar(250)

as
Begin


 declare @script			nvarchar(max)  

 declare @produto			varchar(250)
 declare @responsavel		varchar(250)
 declare @codigo			varchar(250)
 declare @lote				varchar(250)
 declare @item				varchar(250)

 declare @dt_validade		date
 declare @dt_fabri			date
 declare @dt_analise		date

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
	 
	 insert into #obj(obj)
    select splitdata  from dbo.fncArkabSplitString(@objtype, '|')

	select @item = obj from #obj where ordem = 2


select @codigo  = T0.ItemCode
	  ,@produto = T0.itemName
      ,@lote    = T0.DistNumber
	  ,@dt_analise = T0.InDate 
	  ,@dt_fabri   = T0. MnfDate 
	  ,@dt_validade = T0.ExpDate
 from OBTN T0
 inner join BEAS_QSFTHAUPT T1 on T0.DistNumber = T1.BatchNum and T0.ItemCode = T1.ItemCode
	--inner join OCRD on oinv.cardcode = ocrd.cardcode
   where T1.DocEntry = @key

	 

	set @script = replace(@script, '@codigo',isnull(@codigo,''))
	set @script = replace(@script, '@produto',isnull(@produto,''))
	set @script = replace(@script, '@lote',isnull(@lote,''))
	set @script = replace(@script, '@dt_analise',isnull(@dt_analise,''))
	set @script = replace(@script, '@dt_fabri',isnull(@dt_fabri,''))
	set @script = replace(@script, '@dt_validade',isnull(@dt_validade,''))	
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
 exec spcCVAEtiquetaRetem '003','13','13',1,null,null,null,null,null,null,null,null

