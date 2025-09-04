
Create PROCEDURE "SPC_CVA_VALUES_ESTOCAGEM"
(

)
	LANGUAGE SQLSCRIPT AS
BEGIN  

SELECT '1' AS CODE
	  , 'Seco' AS DESCRICPTION
FROM dummy

union all

SELECT '2' AS CODE
	  , 'Resfriado' AS DESCRICPTION
FROM dummy	  

union all	 

SELECT '3' AS CODE
	  , 'Consgelado' AS DESCRICPTION
FROM dummy;

END;
        