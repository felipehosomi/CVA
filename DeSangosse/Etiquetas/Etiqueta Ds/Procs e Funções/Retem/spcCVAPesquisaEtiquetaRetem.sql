

Create proc spcCVAPesquisaEtiquetaRetem
  
  @docini varchar(72)
, @docfim varchar(72)
, @dtini date
, @dtfim date
, @terms varchar(250)
as

  if rtrim(ltrim(isnull(@docini, ''))) = '' set @docini = NULL
  if rtrim(ltrim(isnull(@docfim, ''))) = '' set @docfim = NULL



  select convert(bit,0) as 'Sel'
		 , T0.objtype+'|'+ T0.ItemCode as 'ObjType'
		 , T1.DocEntry as 'Key'
         , T0.ItemCode +' - ' + T0.itemName as 'Item'
	from OBTN T0
  inner join BEAS_QSFTHAUPT T1 on T0.DistNumber = T1.BatchNum and T0.ItemCode = T1.ItemCode
   where T1.DocEntry between @docini and @docfim

   order by 2

	go

exec  spcCVAPesquisaEtiquetaRetem '13', '20', null, null, null




