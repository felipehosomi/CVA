

alter proc spcCVAPesquisaEtiquetaQualidade
  
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
		 , T0.DistNumber as 'Key'
         , T0.ItemCode +' - ' + T0.itemName as 'Item'
	from OBTN T0
  left join BEAS_QSFTHAUPT T1 on T0.DistNumber = T1.BatchNum and T0.ItemCode = T1.ItemCode
   where T0.itemcode = @docini 
     and T0.Distnumber = @terms


 
	
   order by 2


	go

exec  spcCVAPesquisaEtiquetaQualidade '1311', '', null, null, '096/16'




