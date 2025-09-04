drop FUNCTION fncCVAListaDeAprovadores
go
Create FUNCTION fncCVAListaDeAprovadores
(
    @ObjType as nvarchar(max)
    ,@DocEntry as int
)
RETURNS varchar(max) 
AS
BEGIN
    Declare @ListadeAprovadores varchar(max);
    --SELECT @logid = E.LoginId from HumanResources.Employee As E
    --where E.BusinessEntityID = @eid
    
declare @USER_CODE as nvarchar(max)
DECLARE db_cursor CURSOR FOR 
select	
	T2.U_NAME
from 
	OWDD T0
	inner join WDD1 T1 on T0.WddCode=T1.WddCode
	inner join OUSR T2 on T2.USERID=T1.userid
where	
	T0.ObjType=@ObjType
	and T0.DocEntry=@DocEntry

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @USER_CODE  

WHILE @@FETCH_STATUS = 0  
BEGIN  
	--print '123'
	if LEN(coalesce(@ListadeAprovadores,''))=0 begin
		set @ListadeAprovadores=@USER_CODE
	end
	else begin
		set @ListadeAprovadores=@ListadeAprovadores+','+@USER_CODE
	end


      FETCH NEXT FROM db_cursor INTO @USER_CODE 
END 

CLOSE db_cursor  
DEALLOCATE db_cursor     

    RETURN  coalesce(@ListadeAprovadores,'N/A')

END
GO




--select DocEntry,Docnum,draftKey,ObjType,* from OPOR where DocNum=10972


--select DocEntry,Docnum,AuthCode,* from ODRF where DocEntry=14757


--select	
--	T2.USER_CODE
--from 
--	OWDD T0
--	inner join WDD1 T1 on T0.WddCode=T1.WddCode
--	inner join OUSR T2 on T2.USERID=T1.userid
--where	
--	T0.ObjType=22
--	and T0.DocEntry=10972
	
	
--	--select * from OUSR
	
--select	
--	T2.USER_CODE
--from 
--	OWDD T0
--	inner join WDD1 T1 on T0.WddCode=T1.WddCode
--	inner join OUSR T2 on T2.USERID=T1.userid
--where	
--	T0.ObjType=22
--	and T0.DocEntry=10972

--FOR XML RAW, ELEMENTS	


--select	
--	T2.USER_CODE
--from 
--	OWDD T0
--	inner join WDD1 T1 on T0.WddCode=T1.WddCode
--	inner join OUSR T2 on T2.USERID=T1.userid
--where	
--	T0.ObjType=22
--	and T0.DocEntry=10972
--FOR XML RAW('USER_CODE'), ROOT('USER_CODE')