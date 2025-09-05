

/****** Object:  UserDefinedFunction [dbo].[fncArkabSplitString]    Script Date: 11/04/2019 15:42:35 ******/
DROP FUNCTION [dbo].[fncArkabSplitString]
GO

/****** Object:  UserDefinedFunction [dbo].[fncArkabSplitString]    Script Date: 11/04/2019 15:42:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE function [dbo].[fncArkabSplitString] 
( 
    @string nvarchar(max), 
    @delimiter char(1) 
) 
returns @output table(splitdata nvarchar(max) 
) 
begin 
    declare @start int, @end int 
    select @start = 1, @end = charindex(@delimiter, @string) 
    while @start < len(@string) + 1 begin 
        if @end = 0  
            set @end = len(@string) + 1
       
        insert into @output (splitdata) values(rtrim(ltrim(substring(@string, @start, @end - @start)))) 
        set @start = @end + 1 
        set @end = charindex(@delimiter, @string, @start)
        
    end 
    return 
end







GO


