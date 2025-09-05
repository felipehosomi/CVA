ALTER PROCEDURE SBO_SP_TransactionNotification
(
	in object_type nvarchar(30), 				-- SBO Object Type
	in transaction_type nchar(1),			-- [A]dd, [U]pdate, [D]elete, [C]ancel, C[L]ose
	in num_of_cols_in_key int,
	in list_of_key_cols_tab_del nvarchar(255),
	in list_of_cols_val_tab_del nvarchar(255)
)
LANGUAGE SQLSCRIPT
AS
-- Return values
error  int;				-- Result (0 for no error)
error_message nvarchar (200); 		-- Error string to be displayed
--

-- Variaveis
--
textTxt varchar(200) := N'';
cont  int := 0;
cont1 int :=0;
groupPN int :=0;
typePN  varchar(10):= N'';
countryB varchar(3):= N'';
countryS varchar(3):= N'';
valor nvarchar (100):= N'';
item nvarchar (20) :=N'';
tipofrete int;
_count int ;         
-- Variaveis para Validação de Chave de Acesso          
        Estrutura int := 1;           
        CNPJ int := 1;      
        MesAno int := 1;           
        NumNf int := 1;     
		Modelo int;
-- Variaveis para validação de produto
        ItemClass varchar(1) :=N'';
        NCM_Code int := 1;
 

begin

error := 0;
error_message := N'Ok';


--------------------------------------------------------------------------------------------------------------------------------

--	ADD	YOUR	CODE	HERE

--------------------------------------------------------------------------------------------------------------------------------

/*IF (:object_type = '2') AND ((:transaction_type = 'U') OR (:transaction_type = 'A')) THEN

SELECT IFNULL("CardType", '') INTO typePN FROM OCRD WHERE "CardCode" = :list_of_cols_val_tab_del;

-- Verifica o grupo do PN
IF (:typePN='C') OR (:typePN='S') THEN
----------- DADOS GERAIS DO PN -----------------------------------------------------------------------------
SELECT IFNULL("CardName", '') INTO textTxt FROM OCRD WHERE "CardCode" = :list_of_cols_val_tab_del;
IF :textTxt = '' THEN
	error := -100;
	error_message := 'SAP: Informe a Razão Social do parceiro de negócio (PN).';
ELSE
    cont := 0;
    SELECT LENGTH("CardName") INTO cont FROM OCRD WHERE "CardCode" = :list_of_cols_val_tab_del;
    IF :cont > 60 THEN
	   error := -100;
	   error_message := 'SAP: O campo Razão Social Permite no máximo 60 caracteres.';
	ELSE
	   textTxt := '';
       SELECT IFNULL("CardFName", '') INTO textTxt FROM OCRD WHERE "CardCode" = :list_of_cols_val_tab_del;
       IF :textTxt = '' OR (:textTxt is NULL) THEN
	      error := -100;
	      error_message := 'SAP: Informe o Nome Fantasia do parceiro de negócio (PN).';
	   ELSE
	      textTxt := '';
          SELECT IFNULL("Phone1", '') INTO textTxt FROM OCRD WHERE "CardCode" = :list_of_cols_val_tab_del;
          IF (:textTxt = '') OR (:textTxt is NULL) THEN
	          error := -100;
	          error_message := 'SAP: Telefone inválido ou não informado.';
	      ELSE          
              textTxt := '';
              SELECT IFNULL("Phone2", '') INTO textTxt FROM OCRD WHERE "CardCode" = :list_of_cols_val_tab_del;
              IF (:textTxt = '') OR (:textTxt is NULL) THEN
	              error := -100;
	              error_message := 'SAP: DDD inválido ou não informado.';
	          ELSE
	              textTxt := '';
                  SELECT IFNULL("E_Mail", '') INTO textTxt FROM OCRD WHERE "CardCode" = :list_of_cols_val_tab_del;
                  IF :textTxt = '' THEN
	                 error := -100;
	                 error_message := 'SAP: Informe o e-mail do parceiro de negócio (PN).';
	              ELSE
	                 cont := 0;
                     SELECT COUNT(*) INTO cont FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
                                               WHERE OCRD."CardCode" = :list_of_cols_val_tab_del AND CRD1."AdresType" = 'B';
                      IF (:cont = 0) THEN
	                     error := -102;
	                     error_message := 'SAP: ENDEREÇOS: Preencher ao menos um campo de ENDEREÇO DE COBRANÇA.';
	                 ELSE
	                     countryB := '';
                         SELECT IFNULL(CRD1."Country", '') INTO countryB 
                                    FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
                                    WHERE OCRD."CardCode" = :list_of_cols_val_tab_del AND CRD1."AdresType" = 'B';
                         
	                     cont := 0;
                         SELECT COUNT(*) INTO cont FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
                                                   WHERE OCRD."CardCode" = :list_of_cols_val_tab_del AND CRD1."AdresType" = 'S';
                         IF (:cont = 0) THEN
	                         error := -102;
	                         error_message := 'SAP: ENDEREÇOS: Preencher ao menos um campo de ENDEREÇO DE ENTREGA.';
	                     ELSE
	                         countryS := '';
                             SELECT IFNULL(CRD1."Country", '') INTO countryS 
                                    FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
                                    WHERE OCRD."CardCode" = :list_of_cols_val_tab_del AND CRD1."AdresType" = 'S';
	                     
	                         cont := 0;
                             SELECT COUNT(*) INTO cont FROM CRD7 
                                                       WHERE (IFNULL(CRD7."TaxId0",'')<>'' OR IFNULL(CRD7."TaxId4",'')<>'') 
                                                              AND CRD7."CardCode" = :list_of_cols_val_tab_del;
                             IF (:cont = 0) THEN
	                             error := -100;
	                             error_message := 'SAP: CNPJ/CPF é obrigatório';
	                         END IF;
                         END IF;
                     END IF;
                  END IF;
              END IF;
          END IF;
       END IF;
    END IF;
END IF;

/*
----------- CONTATOS DO PN ------------------------------------------------------------------------------------
cont := 0;
SELECT COUNT(*) INTO cont 
  FROM OCPR INNER JOIN OCRD ON OCPR."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del;
IF :cont = 0 THEN
	error := -101;
	error_message := 'SAP: Preencher ao menos 1 contato para o PN.';
END IF;
*/
----------- ENDEREÇO DO PN -----------------------------------------------------------------------------------

----------- ENDEREÇO COBRANÇA DO PN -----------------------------------------------------------------------------------

-- Se o PN for Brasil, validar endereçõs
/*IF (:countryB = 'BR') THEN

textTxt := '';
SELECT IFNULL(CRD1."AddrType", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'B';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo LOGRADOURO no ENDEREÇO de COBRANÇA.';
END IF;

textTxt := '';
SELECT IFNULL(CRD1."Street", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'B';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo RUA no ENDEREÇO de COBRANÇA.';
END IF;

textTxt := '';
SELECT IFNULL(CRD1."StreetNo", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'B';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo NÚMERO DA RUA no ENDEREÇO de COBRANÇA.';
END IF;

textTxt := '';
SELECT IFNULL(CRD1."ZipCode", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'B';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo CEP no ENDEREÇO de COBRANÇA.';
END IF;

textTxt := '';
SELECT IFNULL(CRD1."Block", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'B';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo BAIRRO no ENDEREÇO de COBRANÇA.';
END IF;

textTxt := '';
SELECT IFNULL(CRD1."City", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'B';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo CIDADE no ENDEREÇO de COBRANÇA.';
END IF;

textTxt := '';
SELECT IFNULL(CRD1."State", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'B';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo ESTADO no ENDEREÇO de COBRANÇA.';
END IF;

textTxt := '';
SELECT IFNULL(CRD1."County", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'B';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo MUNICÍPIO no ENDEREÇO de COBRANÇA.';
END IF;
END IF; 
--------------------------------------------------------------------------------------------------------------------
--Fim do Endereço de Cobrança


----------- ENDEREÇO ENTREGA DO PN -----------------------------------------------------------------------------------
IF (:countryS = 'BR')  THEN

textTxt := '';
SELECT IFNULL(CRD1."AddrType", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'S';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo LOGRADOURO no ENDEREÇO de DESTINATARIO.';
END IF;

textTxt := '';
SELECT IFNULL(CRD1."Street", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'S';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo RUA no ENDEREÇO de DESTINATARIO.';
END IF;

textTxt := '';
SELECT IFNULL(CRD1."StreetNo", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'S';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo NÚMERO DA RUA no ENDEREÇO de DESTINATARIO.';
END IF;

textTxt := '';
SELECT IFNULL(CRD1."ZipCode", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'S';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo CEP no ENDEREÇO de DESTINATARIO.';
END IF;

textTxt := '';
SELECT IFNULL(CRD1."Block", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'S';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo BAIRRO no ENDEREÇO de DESTINATARIO.';
END IF;

textTxt := '';
SELECT IFNULL(CRD1."City", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'S';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo CIDADE no ENDEREÇO de DESTINATARIO.';
END IF;

textTxt := '';
SELECT IFNULL(CRD1."State", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'S';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo ESTADO no ENDEREÇO de DESTINATARIO.';
END IF;

textTxt := '';
SELECT IFNULL(CRD1."County", '') INTO textTxt 
  FROM CRD1 INNER JOIN OCRD ON CRD1."CardCode" = OCRD."CardCode"  
 WHERE OCRD."CardCode" = :list_of_cols_val_tab_del
   AND CRD1."AdresType" = 'S';

IF (:textTxt = '') THEN
	error := -102;
	error_message := 'SAP: Preencher o campo MUNICÍPIO no ENDEREÇO de DESTINATARIO.';
END IF;


END IF;
-- Fim do valida endereço entrega

-- Fim do valida grupo
END IF;
-- Fim do Valida PN  
END IF;*/

---------------------------------------------------------------------------------------------------------------------------------            
------------------------------------------------ OITM - CADASTRO DE PRODUTOS ----------------------------------------------------            
---------------------------------------------------------------------------------------------------------------------------------
/*
IF (:object_type = '4' AND (:transaction_type = 'U' OR :transaction_type ='A' ))  THEN       

   Select "ItemClass" INTO ItemClass FROM OITM WHERE OITM."ItemCode" = :list_of_cols_val_tab_del;
       
   IF (:ItemClass = '2') THEN
      Select IFNULL(OITM."NCMCode",0) INTO NCM_Code FROM OITM WHERE OITM."ItemCode" = :list_of_cols_val_tab_del;
      IF (:NCM_Code = 0) THEN             
          error := -172;           
          error_message := 'Codigo de NCM inválido !';         
     END IF;
   END IF;  
END IF;
*/

-------------------------------------------------INICIO - VALIDAÇÃO CHAVE DE ACESSO ---------------------------------------------- 
----- OPCH - FISCAL DE ENTRADA ---------------------------------------------------------------------------------------------------            
----------------------------------------------------------------------------------------------------------------------------------           
          
IF (:object_type = '18' AND (:transaction_type ='U' OR :transaction_type ='A') ) THEN          
    SELECT "Model" INTO Modelo from OPCH T0 Where T0."DocEntry" = :list_of_cols_val_tab_del;
    
    IF Modelo <> 28 and Modelo <> 0 and Modelo <> 37 and Modelo <> 46 and Modelo <> 2 THEN
    	SELECT CASE WHEN (CASE WHEN "Model"=44 then LENGTH(T0."U_AGL_CHV_CTE") ELSE LENGTH(T0."U_nfe_ChaveAcesso") END) = 44 THEN 1 ELSE 0 END INTO Estrutura
		FROM OPCH T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';

    	SELECT CASE WHEN substring((CASE WHEN "Model"=44 then T0."U_AGL_CHV_CTE" ELSE T0."U_nfe_ChaveAcesso" END), 7, 14) = replace(replace(replace((SELECT "TaxId0" FROM PCH12 WHERE "DocEntry" = T0."DocEntry"), '.', ''), '/', ''), '-', '') THEN 1 ELSE 0 END INTO CNPJ 
		FROM OPCH T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';

    	SELECT CASE WHEN substring((CASE WHEN "Model"=44 then T0."U_AGL_CHV_CTE" ELSE T0."U_nfe_ChaveAcesso" END), 26, 9) = T0."Serial" THEN 1 ELSE 0 END INTO NumNf
		FROM OPCH T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';

    	SELECT CASE WHEN substring((CASE WHEN "Model"=44 then T0."U_AGL_CHV_CTE" ELSE T0."U_nfe_ChaveAcesso" END), 3, 4) = substring(CAST(YEAR(T0."TaxDate") AS nvarchar(4)), 3, 2) || RIGHT(replicate('0', 2) || CAST(MONTH(T0."TaxDate") AS varchar), 2) THEN 1 ELSE 0 END INTO MesAno
		FROM OPCH T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';
	END IF;
	
END IF;

----------------------------------------------------------------------------------------------------------------------------------            
---- OPDN - RECEBIMENTO DE MERCADORIAS -------------------------------------------------------------------------------------------            
----------------------------------------------------------------------------------------------------------------------------------           
       
IF (:object_type = '20' AND (:transaction_type ='U' OR :transaction_type ='A') )  THEN
   	SELECT "Model" INTO Modelo from OPDN T0 Where T0."DocEntry" = :list_of_cols_val_tab_del;
    
    IF Modelo <> 28 and Modelo <> 0 and Modelo <> 37 and Modelo <> 46 and Modelo <> 2 THEN
    	SELECT CASE WHEN (CASE WHEN "Model"=44 then LENGTH(T0."U_AGL_CHV_CTE") ELSE LENGTH(T0."U_nfe_ChaveAcesso") END) = 44 THEN 1 ELSE 0 END INTO Estrutura
		FROM OPDN T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';

    	SELECT CASE WHEN substring((CASE WHEN "Model"=44 then T0."U_AGL_CHV_CTE" ELSE T0."U_nfe_ChaveAcesso" END), 7, 14) = replace(replace(replace((SELECT "TaxId0" FROM PCH12 WHERE "DocEntry" = T0."DocEntry"), '.', ''), '/', ''), '-', '') THEN 1 ELSE 0 END INTO CNPJ 
		FROM OPDN T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';

    	SELECT CASE WHEN substring((CASE WHEN "Model"=44 then T0."U_AGL_CHV_CTE" ELSE T0."U_nfe_ChaveAcesso" END), 26, 9) = T0."Serial" THEN 1 ELSE 0 END INTO NumNf
		FROM OPDN T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';

    	SELECT CASE WHEN substring((CASE WHEN "Model"=44 then T0."U_AGL_CHV_CTE" ELSE T0."U_nfe_ChaveAcesso" END), 3, 4) = substring(CAST(YEAR(T0."TaxDate") AS nvarchar(4)), 3, 2) || RIGHT(replicate('0', 2) || CAST(MONTH(T0."TaxDate") AS varchar), 2) THEN 1 ELSE 0 END INTO MesAno
		FROM OPDN T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';
	END IF;
END IF;


----------------------------------------------------------------------------------------------------------------------------------            
----- ORDN - DEVOLUÇÕES ----------------------------------------------------------------------------------------------------------            
----------------------------------------------------------------------------------------------------------------------------------           
          
IF (:object_type = '16' AND (:transaction_type ='U' OR :transaction_type ='A') )  THEN        
    SELECT "Model" INTO Modelo from ORDN T0 Where T0."DocEntry" = :list_of_cols_val_tab_del;
    
    IF Modelo <> 28 and Modelo <> 0 and Modelo <> 37 and Modelo <> 46 and Modelo <> 2 THEN
    	SELECT CASE WHEN (CASE WHEN "Model"=44 then LENGTH(T0."U_AGL_CHV_CTE") ELSE LENGTH(T0."U_nfe_ChaveAcesso") END) = 44 THEN 1 ELSE 0 END INTO Estrutura
		FROM ORDN T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';

    	SELECT CASE WHEN substring((CASE WHEN "Model"=44 then T0."U_AGL_CHV_CTE" ELSE T0."U_nfe_ChaveAcesso" END), 7, 14) = replace(replace(replace((SELECT "TaxId0" FROM PCH12 WHERE "DocEntry" = T0."DocEntry"), '.', ''), '/', ''), '-', '') THEN 1 ELSE 0 END INTO CNPJ 
		FROM ORDN T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';

    	SELECT CASE WHEN substring((CASE WHEN "Model"=44 then T0."U_AGL_CHV_CTE" ELSE T0."U_nfe_ChaveAcesso" END), 26, 9) = T0."Serial" THEN 1 ELSE 0 END INTO NumNf
		FROM ORDN T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';

    	SELECT CASE WHEN substring((CASE WHEN "Model"=44 then T0."U_AGL_CHV_CTE" ELSE T0."U_nfe_ChaveAcesso" END), 3, 4) = substring(CAST(YEAR(T0."TaxDate") AS nvarchar(4)), 3, 2) || RIGHT(replicate('0', 2) || CAST(MONTH(T0."TaxDate") AS varchar), 2) THEN 1 ELSE 0 END INTO MesAno
		FROM ORDN T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';
	END IF;
END IF;

----------------------------------------------------------------------------------------------------------------------------------            
----- ORIN - DEVOLUÇÃO NOTA FISCAL SAIDA -----------------------------------------------------------------------------------------            
----------------------------------------------------------------------------------------------------------------------------------           
/*          
IF (:object_type = '14' AND (:transaction_type ='U' OR :transaction_type ='A') )  THEN      
    SELECT "Model" INTO Modelo from ORIN T0 Where T0."DocEntry" = :list_of_cols_val_tab_del;
    
    IF Modelo <> 28 and Modelo <> 0 and Modelo <> 37 and Modelo <> 46 THEN
    	SELECT CASE WHEN (CASE WHEN "Model"=44 then LENGTH(T0."U_AGL_CHV_CTE") ELSE LENGTH(T0."U_nfe_ChaveAcesso") END) = 44 THEN 1 ELSE 0 END INTO Estrutura
		FROM ORIN T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';

    	SELECT CASE WHEN substring((CASE WHEN "Model"=44 then T0."U_AGL_CHV_CTE" ELSE T0."U_nfe_ChaveAcesso" END), 7, 14) = replace(replace(replace((SELECT "TaxId0" FROM PCH12 WHERE "DocEntry" = T0."DocEntry"), '.', ''), '/', ''), '-', '') THEN 1 ELSE 0 END INTO CNPJ 
		FROM ORIN T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';

    	SELECT CASE WHEN substring((CASE WHEN "Model"=44 then T0."U_AGL_CHV_CTE" ELSE T0."U_nfe_ChaveAcesso" END), 26, 9) = T0."Serial" THEN 1 ELSE 0 END INTO NumNf
		FROM ORIN T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';

    	SELECT CASE WHEN substring((CASE WHEN "Model"=44 then T0."U_AGL_CHV_CTE" ELSE T0."U_nfe_ChaveAcesso" END), 3, 4) = substring(CAST(YEAR(T0."TaxDate") AS nvarchar(4)), 3, 2) || RIGHT(replicate('0', 2) || CAST(MONTH(T0."TaxDate") AS varchar), 2) THEN 1 ELSE 0 END INTO MesAno
		FROM ORIN T0 WHERE T0."DocEntry" = :list_of_cols_val_tab_del AND ("Model"=39 OR "Model"=45 OR "Model"=44) AND T0."SeqCode" = '-2';
	END IF;
END IF;*/
-----------------------------------------------------------------------------------------------------------------------------------

IF ((:object_type = '18' OR :object_type = '16' OR :object_type = '20' OR :object_type = '14') AND 
    (:transaction_type ='U' OR :transaction_type ='A') ) THEN  
    
    IF (:Estrutura = 0) THEN             
        error := -171;           
        error_message := 'Estrutura da chave de acesso inválida - Número de dígitos incorreto !';         
    ELSE          
       IF (:CNPJ = 0) THEN            
             error := -171;            
             error_message := 'Chave de acesso inválida - CNPJ diferente do informado no documento !';          
        ELSE          
           
       	     IF (:NumNf = 0) THEN      
                error := -171;            
                error_message := 'Chave de acesso inválida - NF diferente do informado no documento !';          
             ELSE          
          
                IF (:MesAno = 0) THEN         
		           error := -171;            
     		       error_message := 'Chave de acesso inválida - Mes/Ano diferente do informado na Data do Documento !';          
     		    ELSE
     		     	IF (:Modelo = 0) THEN
     		     		error := -171;
     		     		error_message := 'Modelo da nota fiscal deve ser preenchido !';
     		     	END IF;
     		     END IF;
             END IF;
        END IF;
   END IF;          
END IF;

----------------------------------------------------------------------------------------------------------------------------
--------------------------------------------  FINAL - VALIDAÇÃO CHAVE DE ACESSO --------------------------------------------
----------------------------------------------------------------------------------------------------------------------------

----------------------------------------------------------------------------------------------------------------------------------            
----- ORDR - PEDIDO DE VENDA -----------------------------------------------------------------------------------------------------            
----------------------------------------------------------------------------------------------------------------------------------    
IF (:object_type = '17' AND :transaction_type= 'U') then
 	_count := 0;
 
	 select count(*)
	   into _count
	   from ORDR
	  inner join ADOC on ADOC."DocEntry" = ORDR."DocEntry"
	    and ADOC."ObjType" = ORDR."ObjType"
	    and ADOC."LogInstanc" = (select max(A0."LogInstanc") 
	   							   from ADOC as A0
	   							  where A0."DocEntry" = ORDR."DocEntry"
	   							    and A0."ObjType" = ORDR."ObjType")
	    and (ADOC."U_CVA_Increment_id" <> coalesce(ORDR."U_CVA_Increment_id", 0) 
	     or ADOC."U_CVA_EntityId" <> coalesce(ORDR."U_CVA_EntityId", 0)) 			   
	  where ORDR."DocEntry" = :list_of_cols_val_tab_del;
 
	 IF (:_count > 0) then
			error := -171;
     		error_message := 'Os campos ID Pedido Magento e Nº Pedido Site Magento não podem ser alterados.';			  
	 END IF;		
END IF;  

----------------------------------------------------------------------------------------------------------------------------------  
----------------------------------------------------------------------------------------------------------------------------------  
----------------------------------------------------------------------------------------------------------------------------------  

----------------------------------------------------------------------------------------------------------------------------------            
----- OINV - NOTA FISCAL DE SAÍDA ------------------------------------------------------------------------------------------------            
----------------------------------------------------------------------------------------------------------------------------------    
IF (:object_type = '13' AND :transaction_type= 'U') then
 	_count := 0;
 
	 select count(*)
	   into _count
	   from OINV
	  inner join ADOC on ADOC."DocEntry" = OINV."DocEntry"
	    and ADOC."ObjType" = OINV."ObjType"
	    and ADOC."LogInstanc" = (select max(A0."LogInstanc") 
	   							   from ADOC as A0
	   							  where A0."DocEntry" = OINV."DocEntry"
	   							    and A0."ObjType" = OINV."ObjType")
	    and (ADOC."U_CVA_Increment_id" <> coalesce(OINV."U_CVA_Increment_id", 0) 
	     or ADOC."U_CVA_EntityId" <> coalesce(OINV."U_CVA_EntityId", 0)) 			   
	  where OINV."DocEntry" = :list_of_cols_val_tab_del;
 
	 IF (:_count > 0) then
			error := -131;
     		error_message := 'Os campos ID Pedido Magento e Nº Pedido Site Magento não podem ser alterados.';			  
	 END IF;		
END IF;  

----------------------------------------------------------------------------------------------------------------------------------  
----------------------------------------------------------------------------------------------------------------------------------  
----------------------------------------------------------------------------------------------------------------------------------  

----------------------------------------------------------------------------------------------------
--> Sincroniza Produto PDV
----------------------------------------------------------------------------------------------------

IF (:object_type = '4' AND (:transaction_type = 'A' or :transaction_type= 'U')) then
 _count := 0;
 
 SELECT COUNT(*) into _count FROM OITM WHERE "ItemCode" = :list_of_cols_val_tab_del AND "InvntItem" = 'Y';
	IF (:_count > 0) then
				
			INSERT INTO "CUPOMFISCAL_SAP"."SincronizaProduto"
			SELECT (SELECT COUNT(*)+1 FROM "CUPOMFISCAL_SAP"."SincronizaProduto" ) as "id" 
			     , T0."ItemCode"
				 , T0."ItemName"
 				 , T3."NcmCode"
 				 , T0."CodeBars"
 				 , :transaction_type
 				 , :error_message
				 , T1."Price"
				 , Current_date
			 from OITM  T0 
			 inner join ITM1 T1  on T0."ItemCode" = T1."ItemCode"
			 inner join OPLN T2  on T1."PriceList" = T2."ListNum" and T2."U_CVA_ListPricePDV" = 'S'
			 inner join ONCM T3  on T0."NCMCode" = T3."AbsEntry"
			WHERE T0."ItemCode" = :list_of_cols_val_tab_del;
			  
		END IF;		
END IF;

-----------------------------------------------------------------------------------------------------------------------------
--> Atualizar o campo Tipo de envio padrão NFe - AGL de acorco com a opção selecionada no campo Frete por Conta
--> Eloir
--> 13/03/2020
-----------------------------------------------------------------------------------------------------------------------------

IF (:object_type = '17' AND (:transaction_type ='U' OR :transaction_type ='A') ) THEN

	SELECT COALESCE("Incoterms",-1) INTO tipofrete from RDR12 T0 Where T0."DocEntry" = :list_of_cols_val_tab_del;
	update ORDR set "U_nfe_tipoEnv" = :tipofrete where "DocEntry" = :list_of_cols_val_tab_del;

	 IF (:tipofrete = -1) then
			error := -101;
     		error_message := 'Preencha o campo Frete por Conta!';	
     ELSE IF(:tipofrete <> 0 and :tipofrete <> 1 and :tipofrete <> 2 and :tipofrete <> 3 and :tipofrete <> 4 and :tipofrete <> 9) then
     		error := -101;
     		error_message := 'Apenas os números 0,1,2,3,4,9 são válidos!!';			  
	 END IF;																								
end if;



----------------------------------------------------------------------------------------------------
--> Sincroniza Cliente PDV
----------------------------------------------------------------------------------------------------
/*
if(:object_type = '2' AND (:transaction_type = 'A' or :transaction_type  = 'U')) then
--Begin

INSERT INTO "CUPOMFISCAL_SAP"."SincronizaCliente" 
	Select 	 (SELECT COUNT(*)+1 FROM "CUPOMFISCAL_SAP"."SincronizaCliente" ) as "id" 	 		
			,T0."CardName" as "cli_nome"
			,0 as "cli_idade"
			,TO_VARCHAR(T0."CreateDate") as "cli_data_nascimento"
			,null as "cli_rg"
			,(select distinct replace(replace("TaxId4",'-',''),'.','') from CRD7 where "AddrType" = 'S' and "CardCode" = :list_of_cols_val_tab_del ) as "cli_cpf"
			,T1."Street" as "cli_logradouro"		 	
			,T1."StreetNo" as "cli_numero"
			,T1."Building" as "cli_complemento"
			,T1."Block" as "cli_bairro"
            ,ifnull(T1."County",0) as "cid_codigo"
            ,T0."CardName" as "cli_razao_social"
			,(select distinct "TaxId0" from CRD7 where "AddrType" = 'S' and "CardCode" = :list_of_cols_val_tab_del ) as "cli_cnpj"
			,null as "cli_ie"
			,TO_VARCHAR(T0."CreateDate") as "cli_data_fundacao"
			,'Pessoa Fisíca' as "cli_tipo_pessoa"
			,TO_NCLOB(T0."Phone1") as "cli_telefone"
			,T0."Cellular" as "cli_celular"
			,null as "cli_calJuros"
			,0 "cli_limiteCredito"
			,0 as "cli_limiteMensal"
			,T1."ZipCode" as "cli_cep"
			,null as "cli_cota"
			,null as "cli_Grupo"
			,0 as "emp_codigo"
			,T0."E_Mail" as "cli_email1"
			,T0."E_Mail" as "cli_email2"
			,T0."Free_Text" as "observacao"
			,T0."CardCode" as "CardCode" 
			,T1."AddrType" as "cli_TipoLogradouro"
			,case when T1."State" = 'AC' then 1 
			      when T1."State" = 'AL' then 2 
			      when T1."State" = 'AM' then 3 
			      when T1."State" = 'AP' then 4 
			      when T1."State" = 'BA' then 5 
			      when T1."State" = 'CE' then 6 
			      when T1."State" = 'DF' then 7 
			      when T1."State" = 'ES' then 8 
			      when T1."State" = 'GO' then 9 
			      when T1."State" = 'MA' then 10 
			      when T1."State" = 'MG' then 11
			      when T1."State" = 'MS' then 12 
			      when T1."State" = 'MT' then 13 
			      when T1."State" = 'PA' then 14 
			      when T1."State" = 'PB' then 15 
			      when T1."State" = 'PE' then 16 
			      when T1."State" = 'PI' then 17 
			      when T1."State" = 'PR' then 18
			      when T1."State" = 'RJ' then 19
			      when T1."State" = 'RN' then 20
			      when T1."State" = 'RO' then 21
			      when T1."State" = 'RR' then 22
			      when T1."State" = 'RS' then 23
			      when T1."State" = 'SC' then 24
			      when T1."State" = 'SE' then 25
			      when T1."State" = 'SP' then 26
			      when T1."State" = 'TO' then 27			      			      
			  end  as "cli_uf"
			,T0."U_CampoLivre1" as "campoLivre1"
			,T0."U_CampoLivre2" as "campoLivre2"
			,T0."U_CampoLivre3" as "campoLivre3"
			,T0."U_CampoLivre4" as "campoLivre4"
			,Current_date as "SINCRONIZADOEM"
	   from OCRD T0
	   Left Join CRD1 T1 on T1."CardCode" = T0."CardCode" and T1."AdresType" = 'S' 
      where  T0."CardType" = 'C'
        and Ifnull(T0."CardName",'')<> ''
         and T0."CardCode" =  :list_of_cols_val_tab_del;
End IF;
*/

----------------------------------------------------------------------------------------------------
-->Valida se os itens do pedido tem utilização
----------------------------------------------------------------------------------------------------
IF (:object_type = '17' AND (:transaction_type = 'A' or :transaction_type= 'U')) then
 	_count := 0;
 
	 select count(*)
	   into _count
	   from ORDR
	   inner join RDR1 on ORDR."DocEntry" = RDR1."DocEntry"			   
	  where RDR1."Usage" Is Null And ORDR."DocEntry" = :list_of_cols_val_tab_del;
 
	 IF (:_count > 0) then
			error := -171;
     		error_message := 'Preencha a utilização dos itens do pedido.';			  
	 END IF;		
END IF; 


-- Select the return values
select :error, :error_message FROM dummy;

end;
