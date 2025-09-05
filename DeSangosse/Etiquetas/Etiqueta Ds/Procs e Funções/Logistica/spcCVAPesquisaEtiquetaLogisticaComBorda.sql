

Create proc spcCVAPesquisaEtiquetaLogisticaComBorda
  
  @docini varchar(72)
, @docfim varchar(72)
, @dtini date
, @dtfim date
, @terms varchar(250)
as

  if rtrim(ltrim(isnull(@docini, ''))) = '' set @docini = NULL
  if rtrim(ltrim(isnull(@docfim, ''))) = '' set @docfim = NULL



  select convert(bit,0) as 'Sel'
		 , oinv.objtype as 'ObjType'
		 , oinv.Serial as 'Key'
         , oinv.cardcode +' - ' + oinv.cardname as 'cliente'
    from OINV 
	inner join OCRD on oinv.cardcode = ocrd.cardcode
   where oinv.serial between @docini and @docfim 


 
	
   order by 2


	go

exec  spcCVAPesquisaEtiquetaLogisticaComBorda '14387', '14387', null, null, ''




