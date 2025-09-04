ALTER PROCEDURE "SPC_CVA_WMS_ENTRADAMAT_TRANS"(
	DocEntry int,
	ObjType varchar(50),
	Transacao varchar(1)
)  
	LANGUAGE SQLSCRIPT AS
BEGIN  
-- OPCH = 18
-- OPDN = 20

	--	IF((ObjType = '18' OR ObjType = '20') AND Transacao = 'A')
	IF((ObjType = '18' OR ObjType = '20') AND Transacao = 'A')
	THEN		
		INSERT INTO "@CVA_WMS_INTEGRACAO"	
			VALUES ((SELECT COALESCE(MAX("Code"), 0) FROM "@CVA_WMS_INTEGRACAO") + 1, TO_VARCHAR((SELECT COALESCE(MAX("Code"), 0) FROM "@CVA_WMS_INTEGRACAO"))+ 1, TO_VARCHAR(DocEntry), ObjType, 0, null,null );
	END IF;
--	select * from "@CVA_WMS_INTEGRACAO";
END;


--select * from "@CVA_WMS_INTEGRACAO"


--SELECT * FROM OACT
/* Atualizar no final da TRANSACTIONNOTIFICATION */

--------------------------------------------------------------------------------------------------------------------------------
--	Adicionar Integração ENTRADA DE MATERIAIS NO CD
--------------------------------------------------------------------------------------------------------------------------------
--IF (:object_type = '4' AND (:transaction_type = 'U' OR :transaction_type = 'A' ))  THEN    
--
--   Select "InvntItem" INTO InvntItem FROM OITM  WHERE OITM."ItemCode" = :list_of_cols_val_tab_del;
--   SELECT LENGTH(T0."DfltWH") INTO Deposito FROM OITM T0 WHERE T0."ItemCode" = :list_of_cols_val_tab_del;   
       
 
--  IF ((:Deposito = 1 or :Deposito is NULL ) and :InvntItem = 'Y' )THEN             
--       error := -199;           
--       error_message := 'Selecionar um depósito padrão!';         
--   END IF;
--END IF;
--call "SPC_CVA_WMS_ENTRADAMAT_TRANS" (1, '1', '', (SELECT COALESCE(MAX("Code"), 1) FROM "@CVA_WMS_INTEGRACAO"));
--------------------------------------------------------------------------------------------------------------------------------
--	Adicionar Integração ENTRADA DE MATERIAIS NO CD
--------------------------------------------------------------------------------------------------------------------------------

/* Atualizar no final da TRANSACTIONNOTIFICATION */