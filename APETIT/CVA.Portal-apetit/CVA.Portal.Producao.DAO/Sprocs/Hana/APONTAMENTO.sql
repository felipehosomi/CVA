
CREATE FUNCTION CVA_TURNO (
 turnoIn int
)
returns turnoOut nvarchar(10)
 LANGUAGE SQLSCRIPT
 SQL SECURITY INVOKER 
AS
BEGIN

	turnoOut :=
	(
		case 	when turnoIn = 01 then 
					'Almoço'
				when turnoIn = 02 then 
	  	  			'Jantar'
  	  			when turnoIn = 03 then 
	  	  			'Ceia'
	  	  		else 
	  	  			''
		end
 	
	);

END