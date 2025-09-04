CREATE VIEW  CVA_LIVRODESAIDA_ITENS
        
AS         
        
SELECT       
 "OINV"."BPLId"					 "BPLId",        
 "INV1"."DocEntry"               "DocEntry",        
 "INV1"."ObjType"                "ObjType",      
 "OINV"."DocDate"                "DocDate",
 "OINV"."VATRegNum"				 "VATRegNum",
 "OINV"."CardCode"				 "CardCode",  
 "INV12"."TaxId0"				 "TaxId0",      
 "OINV"."Serial"                 "Serial",
 "OINV"."SeriesStr"				 "SeriesStr",
 "OINV"."SubStr"				 "SubStr",
 "OINV"."Model"				     "Model",
 "INV1"."LineNum"				 "LineNum",        
 "INV1"."ItemCode"               "ItemCode",       
 "INV1"."Dscription"             "Dscription",      
 "ONCM"."NcmCode"				 "NCMCode",       
 "INV1"."Quantity"               "Quantity",         
 "INV1"."Price"                  "Price",         
 "INV1"."LineTotal"              "LineTotal",        
 "INV1"."DistribSum"   "DistribSum",        
 "INV1"."CSTCode"                "CSTCode",         
 "INV1"."CFOPCode"               "CFOPCode",        
 "ICMS"."BaseSum"      "ICMS_BaseSum",        
 "ICMS"."TaxSum"       "ICMS_TaxSum",        
 "ICMS"."U_ExcAmtL"    "ICMS_U_ExcAmtL",        
 "ICMS"."U_OthAmtL"    "ICMS_U_OthAmtL",         
 "ICMSST"."BaseSum"    "ICMSST_BaseSum",        
 "ICMSST"."TaxSum"     "ICMSST_TaxSum",        
 "INV1"."CSTfIPI"                "CSTfIPI",        
 "IPI"."NonDdctPrc"              "IPI_NonDdctPrc",        
 "IPI"."BaseSum"       "IPI_BaseSum",        
 "IPI"."TaxSum"        "IPI_TaxSum",        
 "IPI"."U_ExcAmtL"     "IPI_U_ExcAmtL",        
 "IPI"."U_OthAmtL"     "IPI_U_OthAmtL",        
 "INV1"."CSTfPIS"                "CSTfPIS",        
 "PIS"."BaseSum"       "PIS_BaseSum",        
 "PIS"."TaxSum"        "PIS_TaxSum",        
 "PIS"."U_ExcAmtL"     "PIS_U_ExcAmtL",        
 "PIS"."U_OthAmtL"     "PIS_U_OthAmtL",        
 "INV1"."CSTfCOFINS"             "CSTfCOFINS",        
 "COFINS"."BaseSum"    "COFINS_BaseSum",        
 "COFINS"."TaxSum"     "COFINS_TaxSum",        
 "COFINS"."U_ExcAmtL"  "COFINS_U_ExcAmtL",        
 "COFINS"."U_OthAmtL"  "COFINS_U_OthAmtL",
  (SELECT "S1"."U_cdErro"
	FROM "@SKL25NFE" "S1" 
		WHERE "S1"."U_DocEntry"="OINV"."DocEntry" 
		AND "U_cdErro" = '100'
		AND (CASE "OINV"."ObjType" 
				WHEN '13' THEN 'NS' 
				WHEN '14' THEN 'DN' 
				WHEN '15' THEN 'EM' 
				WHEN '16' THEN 'DT' 
				WHEN '18' THEN 'NE' 
				WHEN '19' THEN 'DE' 
				WHEN '20' THEN 'RM' 
				WHEN '21' THEN 'DM' 
				ELSE "OINV"."ObjType"  END)="S1"."U_tipoDocumento"
	) AS "StatusNfe"
        
        
 FROM "INV1"         
     INNER JOIN "OINV" ON "INV1"."DocEntry" = "OINV"."DocEntry"        
  LEFT JOIN         
   (        
   SELECT "INV4"."DocEntry", "INV4"."LineNum", "INV4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "INV4"                  
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "INV4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'ICMS'           
      INNER JOIN "INV1" ON "INV4"."DocEntry" = "INV1"."DocEntry" AND "INV4"."LineNum" = "INV1"."LineNum"             
     GROUP BY "INV4"."DocEntry", "INV4"."LineNum", "INV4"."NonDdctPrc"        
   ) "ICMS" ON "ICMS"."DocEntry" = "INV1"."DocEntry" AND "ICMS"."LineNum" = "INV1"."LineNum"         
  LEFT JOIN         
   (        
   SELECT "INV4"."DocEntry", "INV4"."LineNum", "INV4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "INV4"                   
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "INV4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'IPI'           
      INNER JOIN "INV1" ON "INV4"."DocEntry" = "INV1"."DocEntry" AND "INV4"."LineNum" = "INV1"."LineNum"             
     GROUP BY "INV4"."DocEntry", "INV4"."LineNum", "INV4"."NonDdctPrc"        
   ) "IPI" ON "IPI"."DocEntry" = "INV1"."DocEntry" AND "IPI"."LineNum" = "INV1"."LineNum"           
  LEFT JOIN         
   (        
   SELECT "INV4"."DocEntry", "INV4"."LineNum", "INV4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "INV4"          
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "INV4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'ICMS-ST'           
      INNER JOIN "INV1" ON "INV4"."DocEntry" = "INV1"."DocEntry" AND "INV4"."LineNum" = "INV1"."LineNum"             
     GROUP BY "INV4"."DocEntry", "INV4"."LineNum", "INV4"."NonDdctPrc"        
   ) "ICMSST" ON "ICMSST"."DocEntry" = "INV1"."DocEntry" AND "ICMSST"."LineNum" = "INV1"."LineNum"           
  LEFT JOIN         
   (        
   SELECT "INV4"."DocEntry", "INV4"."LineNum", "INV4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "INV4"          
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "INV4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'PIS'           
      INNER JOIN "INV1" ON "INV4"."DocEntry" = "INV1"."DocEntry" AND "INV4"."LineNum" = "INV1"."LineNum"             
     GROUP BY "INV4"."DocEntry", "INV4"."LineNum", "INV4"."NonDdctPrc"        
   ) "PIS" ON "PIS"."DocEntry" = "INV1"."DocEntry" AND "PIS"."LineNum" = "INV1"."LineNum"          
  LEFT JOIN         
   (        
   SELECT "INV4"."DocEntry", "INV4"."LineNum", "INV4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "INV4"          
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "INV4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = '"COFINS".'           
      INNER JOIN "INV1" ON "INV4"."DocEntry" = "INV1"."DocEntry" AND "INV4"."LineNum" = "INV1"."LineNum"             
     GROUP BY "INV4"."DocEntry", "INV4"."LineNum", "INV4"."NonDdctPrc"        
   ) "COFINS" ON "COFINS"."DocEntry" = "INV1"."DocEntry" AND "COFINS"."LineNum" = "INV1"."LineNum"     
   LEFT JOIN "OITM" ON "INV1"."ItemCode" = "OITM"."ItemCode"    
   LEFT JOIN "ONCM" ON "OITM"."NCMCode" = "ONCM"."AbsEntry"   
   LEFT JOIN "INV12" ON "INV12"."DocEntry" = "OINV"."DocEntry"  
        
         
 WHERE 0=0
	--AND "OINV".Canceled = 'N' 
	AND "OINV"."Model" <> '37'   
	AND (SELECT max("S1"."U_cdErro")
	FROM "@SKL25NFE" "S1" 
		WHERE "S1"."U_DocEntry"="OINV"."DocEntry" 
		AND "U_cdErro" = '100'
		AND (CASE "OINV"."ObjType" 
				WHEN '13' THEN 'NS' 
				WHEN '14' THEN 'DN' 
				WHEN '15' THEN 'EM' 
				WHEN '16' THEN 'DT' 
				WHEN '18' THEN 'NE' 
				WHEN '19' THEN 'DE' 
				WHEN '20' THEN 'RM' 
				WHEN '21' THEN 'DM' 
				ELSE "OINV"."ObjType"  END)="S1"."U_tipoDocumento") = '100'      
        
UNION ALL         
        
--------------------------------------------------------------------------------------------------------        
----------------------------------- DEVOLUÇÃO NOTA DE ENTRADA ------------------------------------------        
--------------------------------------------------------------------------------------------------------        
        
SELECT       
 "ORPC"."BPLId"					 "BPLId",          
 "RPC1"."DocEntry"               "DocEntry",        
 "RPC1"."ObjType"                "ObjType",        
 "ORPC"."DocDate"                "DocDate", 
 "ORPC"."VATRegNum"				 "VATRegNum", 
 "ORPC"."CardCode"				 "CardCode",  
 "RPC12"."TaxId0"				 "TaxId0",            
 "ORPC"."Serial"                 "Serial",
 "ORPC"."SeriesStr"				 "SeriesStr",
 "ORPC"."SubStr"				 "SubStr",  
 "ORPC"."Model"				     "Model",      
 "RPC1"."LineNum"				"LineNum",          
 "RPC1"."ItemCode"               "ItemCode",         
 "RPC1"."Dscription"             "Dscription",    
 "ONCM"."NcmCode"					"NCMCode",         
 "RPC1"."Quantity"               "Quantity",         
 "RPC1"."Price"                  "Price",         
 "RPC1"."LineTotal"              "LineTotal",        
 "RPC1"."DistribSum"   "DistribSum",        
 "RPC1"."CSTCode"                "CSTCode",         
 "RPC1"."CFOPCode"               "CFOPCode",        
 "ICMS"."BaseSum"      "ICMS_BaseSum",        
 "ICMS"."TaxSum"       "ICMS_TaxSum",        
 "ICMS"."U_ExcAmtL"    "ICMS_U_ExcAmtL",        
 "ICMS"."U_OthAmtL"    "ICMS_U_OthAmtL",         
 "ICMSST"."BaseSum"    "ICMSST_BaseSum",        
 "ICMSST"."TaxSum"     "ICMSST_TaxSum",        
 "RPC1"."CSTfIPI"                "CSTfIPI",        
 "IPI"."NonDdctPrc"              "IPI_NonDdctPrc",        
 "IPI"."BaseSum"       "IPI_BaseSum",        
 "IPI"."TaxSum"        "IPI_TaxSum",        
 "IPI"."U_ExcAmtL"     "IPI_U_ExcAmtL",        
 "IPI"."U_OthAmtL"     "IPI_U_OthAmtL",        
 "RPC1"."CSTfPIS"                "CSTfPIS",        
 "PIS"."BaseSum"       "PIS_BaseSum",        
 "PIS"."TaxSum"        "PIS_TaxSum",        
 "PIS"."U_ExcAmtL"     "PIS_U_ExcAmtL",        
 "PIS"."U_OthAmtL"     "PIS_U_OthAmtL",        
 "RPC1"."CSTfCOFINS"             "CSTfCOFINS",        
 "COFINS"."BaseSum"    "COFINS_BaseSum",        
 "COFINS"."TaxSum"     "COFINS_TaxSum",        
 "COFINS"."U_ExcAmtL"  "COFINS_U_ExcAmtL",        
 "COFINS"."U_OthAmtL"  "COFINS_U_OthAmtL",
 (SELECT max("S1"."U_cdErro")
	FROM "@SKL25NFE" "S1"
		WHERE "S1"."U_DocEntry"="ORPC"."DocEntry" 
		AND "U_cdErro" = '100'
		AND (CASE "ORPC"."ObjType" 
				WHEN '13' THEN 'NS' 
				WHEN '14' THEN 'DN' 
				WHEN '15' THEN 'EM' 
				WHEN '16' THEN 'DT' 
				WHEN '18' THEN 'NE' 
				WHEN '19' THEN 'DE' 
				WHEN '20' THEN 'RM' 
				WHEN '21' THEN 'DM' 
				ELSE "ORPC"."ObjType"  END)="S1"."U_tipoDocumento") AS "StatusNfe"     
        
        
 FROM "RPC1"         
     INNER JOIN "ORPC" ON "RPC1"."DocEntry" = "ORPC"."DocEntry"        
  LEFT JOIN         
   (        
   SELECT "RPC4"."DocEntry", "RPC4"."LineNum", "RPC4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "RPC4"                  
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "RPC4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'ICMS'           
      INNER JOIN "RPC1" ON "RPC4"."DocEntry" = "RPC1"."DocEntry" AND "RPC4"."LineNum" = "RPC1"."LineNum"             
     GROUP BY "RPC4"."DocEntry", "RPC4"."LineNum", "RPC4"."NonDdctPrc"        
   ) "ICMS" ON "ICMS"."DocEntry" = "RPC1"."DocEntry" AND "ICMS"."LineNum" = "RPC1"."LineNum"         
  LEFT JOIN         
   (        
   SELECT "RPC4"."DocEntry", "RPC4"."LineNum", "RPC4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "RPC4"                   
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "RPC4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'IPI'           
      INNER JOIN "RPC1" ON "RPC4"."DocEntry" = "RPC1"."DocEntry" AND "RPC4"."LineNum" = "RPC1"."LineNum"             
     GROUP BY "RPC4"."DocEntry", "RPC4"."LineNum", "RPC4"."NonDdctPrc"        
   ) "IPI" ON "IPI"."DocEntry" = "RPC1"."DocEntry" AND "IPI"."LineNum" = "RPC1"."LineNum"           
  LEFT JOIN         
   (        
   SELECT "RPC4"."DocEntry", "RPC4"."LineNum", "RPC4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "RPC4"          
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "RPC4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'ICMS-ST'           
      INNER JOIN "RPC1" ON "RPC4"."DocEntry" = "RPC1"."DocEntry" AND "RPC4"."LineNum" = "RPC1"."LineNum"             
     GROUP BY "RPC4"."DocEntry", "RPC4"."LineNum", "RPC4"."NonDdctPrc"        
   ) "ICMSST" ON "ICMSST"."DocEntry" = "RPC1"."DocEntry" AND "ICMSST"."LineNum" = "RPC1"."LineNum"           
  LEFT JOIN         
   (        
   SELECT "RPC4"."DocEntry", "RPC4"."LineNum", "RPC4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "RPC4"          
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "RPC4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'PIS'           
      INNER JOIN "RPC1" ON "RPC4"."DocEntry" = "RPC1"."DocEntry" AND "RPC4"."LineNum" = "RPC1"."LineNum"             
     GROUP BY "RPC4"."DocEntry", "RPC4"."LineNum", "RPC4"."NonDdctPrc"        
   ) "PIS" ON "PIS"."DocEntry" = "RPC1"."DocEntry" AND "PIS"."LineNum" = "RPC1"."LineNum"          
  LEFT JOIN         
   (        
   SELECT "RPC4"."DocEntry", "RPC4"."LineNum", "RPC4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "RPC4"          
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "RPC4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = '"COFINS".'           
      INNER JOIN "RPC1" ON "RPC4"."DocEntry" = "RPC1"."DocEntry" AND "RPC4"."LineNum" = "RPC1"."LineNum"             
     GROUP BY "RPC4"."DocEntry", "RPC4"."LineNum", "RPC4"."NonDdctPrc"        
   ) "COFINS" ON "COFINS"."DocEntry" = "RPC1"."DocEntry" AND "COFINS"."LineNum" = "RPC1"."LineNum"          
   LEFT JOIN "OITM" ON "RPC1"."ItemCode" = "OITM"."ItemCode"    
   LEFT JOIN "ONCM" ON "OITM"."NCMCode" = "ONCM"."AbsEntry"   
   LEFT JOIN "RPC12" ON "ORPC"."DocEntry" = "RPC12"."DocEntry"   
         
 WHERE 0=0
	--AND "ORPC".Canceled = 'N'  
	AND "ORPC"."Model" <> '37'  
	AND (SELECT max("S1"."U_cdErro")
		FROM "@SKL25NFE" "S1" 
			WHERE "S1"."U_DocEntry"="ORPC"."DocEntry" 
			AND "U_cdErro" = '100'
			AND (CASE "ORPC"."ObjType" 
					WHEN '13' THEN 'NS' 
					WHEN '14' THEN 'DN' 
					WHEN '15' THEN 'EM' 
					WHEN '16' THEN 'DT' 
					WHEN '18' THEN 'NE' 
					WHEN '19' THEN 'DE' 
					WHEN '20' THEN 'RM' 
					WHEN '21' THEN 'DM' 
					ELSE "ORPC"."ObjType"  END)="S1"."U_tipoDocumento") = '100'          
        
        
UNION ALL         
        
--------------------------------------------------------------------------------------------------------        
----------------------------------- DEVOLUÇÃO ----------------------------------------------------------        
--------------------------------------------------------------------------------------------------------        
        
        
        
SELECT             
 "ORDN"."BPLId"                  "BPLId",            
 "RDN1"."DocEntry"               "DocEntry",    
 "RDN1"."ObjType"                "ObjType",        
 "ORDN"."DocDate"                "DocDate",
 "ORDN"."VATRegNum"				 "VATRegNum",   
 "ORDN"."CardCode"				 "CardCode",  
 "RDN12"."TaxId0"				 "TaxId0",         
 "ORDN"."Serial"                 "Serial",
 "ORDN"."SeriesStr"				 "SeriesStr",
 "ORDN"."SubStr"				 "SubStr",  
 "ORDN"."Model"				     "Model",           
 "RDN1"."LineNum"                "LineNum",          
 "RDN1"."ItemCode"               "ItemCode",         
 "RDN1"."Dscription"             "Dscription",    
 "ONCM"."NcmCode"				 "NCMCode",         
 "RDN1"."Quantity"               "Quantity",         
 "RDN1"."Price"                  "Price",         
 "RDN1"."LineTotal"              "LineTotal",        
 "RDN1"."DistribSum"   "DistribSum",        
 "RDN1"."CSTCode"                "CSTCode",         
 "RDN1"."CFOPCode"               "CFOPCode",        
 "ICMS"."BaseSum"      "ICMS_BaseSum",        
 "ICMS"."TaxSum"       "ICMS_TaxSum",        
 "ICMS"."U_ExcAmtL"    "ICMS_U_ExcAmtL",        
 "ICMS"."U_OthAmtL"    "ICMS_U_OthAmtL",         
 "ICMSST"."BaseSum"    "ICMSST_BaseSum",        
 "ICMSST"."TaxSum"     "ICMSST_TaxSum",        
 "RDN1"."CSTfIPI"                "CSTfIPI",        
 "IPI"."NonDdctPrc"              "IPI_NonDdctPrc",        
 "IPI"."BaseSum"       "IPI_BaseSum",        
 "IPI"."TaxSum"        "IPI_TaxSum",        
 "IPI"."U_ExcAmtL"     "IPI_U_ExcAmtL",        
 "IPI"."U_OthAmtL"     "IPI_U_OthAmtL",        
 "RDN1"."CSTfPIS"                "CSTfPIS",        
 "PIS"."BaseSum"       "PIS_BaseSum",        
 "PIS"."TaxSum"        "PIS_TaxSum",        
 "PIS"."U_ExcAmtL"     "PIS_U_ExcAmtL",        
 "PIS"."U_OthAmtL"     "PIS_U_OthAmtL",        
 "RDN1"."CSTfCOFINS"             "CSTfCOFINS",        
 "COFINS"."BaseSum"    "COFINS_BaseSum",        
 "COFINS"."TaxSum"     "COFINS_TaxSum",        
 "COFINS"."U_ExcAmtL"  "COFINS_U_ExcAmtL",        
 "COFINS"."U_OthAmtL"  "COFINS_U_OthAmtL",
 (SELECT max("S1"."U_cdErro")
	FROM "@SKL25NFE" "S1" 
		WHERE "S1"."U_DocEntry"="ORDN"."DocEntry" 
		AND "U_cdErro" = '100'
		AND (CASE "ORDN"."ObjType" 
				WHEN '13' THEN 'NS' 
				WHEN '14' THEN 'DN' 
				WHEN '15' THEN 'EM' 
				WHEN '16' THEN 'DT' 
				WHEN '18' THEN 'NE' 
				WHEN '19' THEN 'DE' 
				WHEN '20' THEN 'RM' 
				WHEN '21' THEN 'DM' 
				ELSE "ORDN"."ObjType"  END)="S1"."U_tipoDocumento") AS "StatusNfe"             
        
        
 FROM "RDN1"         
     INNER JOIN "ORDN" ON "RDN1"."DocEntry" = "ORDN"."DocEntry"        
  LEFT JOIN         
   (        
   SELECT "RDN4"."DocEntry", "RDN4"."LineNum", "RDN4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "RDN4"                  
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "RDN4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'ICMS'           
      INNER JOIN "RDN1" ON "RDN4"."DocEntry" = "RDN1"."DocEntry" AND "RDN4"."LineNum" = "RDN1"."LineNum"             
     GROUP BY "RDN4"."DocEntry", "RDN4"."LineNum", "RDN4"."NonDdctPrc"        
   ) "ICMS" ON "ICMS"."DocEntry" = "RDN1"."DocEntry" AND "ICMS"."LineNum" = "RDN1"."LineNum"         
  LEFT JOIN         
   (        
   SELECT "RDN4"."DocEntry", "RDN4"."LineNum", "RDN4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "RDN4"                   
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "RDN4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'IPI'           
      INNER JOIN "RDN1" ON "RDN4"."DocEntry" = "RDN1"."DocEntry" AND "RDN4"."LineNum" = "RDN1"."LineNum"             
     GROUP BY "RDN4"."DocEntry", "RDN4"."LineNum", "RDN4"."NonDdctPrc"        
   ) "IPI" ON "IPI"."DocEntry" = "RDN1"."DocEntry" AND "IPI"."LineNum" = "RDN1"."LineNum"           
  LEFT JOIN         
   (        
   SELECT "RDN4"."DocEntry", "RDN4"."LineNum", "RDN4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "RDN4"          
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "RDN4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'ICMS-ST'           
      INNER JOIN "RDN1" ON "RDN4"."DocEntry" = "RDN1"."DocEntry" AND "RDN4"."LineNum" = "RDN1"."LineNum"             
     GROUP BY "RDN4"."DocEntry", "RDN4"."LineNum", "RDN4"."NonDdctPrc"        
   ) "ICMSST" ON "ICMSST"."DocEntry" = "RDN1"."DocEntry" AND "ICMSST"."LineNum" = "RDN1"."LineNum"           
  LEFT JOIN         
   (        
   SELECT "RDN4"."DocEntry", "RDN4"."LineNum", "RDN4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "RDN4"          
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "RDN4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'PIS'           
      INNER JOIN "RDN1" ON "RDN4"."DocEntry" = "RDN1"."DocEntry" AND "RDN4"."LineNum" = "RDN1"."LineNum"             
     GROUP BY "RDN4"."DocEntry", "RDN4"."LineNum", "RDN4"."NonDdctPrc"        
   ) "PIS" ON "PIS"."DocEntry" = "RDN1"."DocEntry" AND "PIS"."LineNum" = "RDN1"."LineNum"          
  LEFT JOIN         
   (        
   SELECT "RDN4"."DocEntry", "RDN4"."LineNum", "RDN4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "RDN4"          
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "RDN4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = '"COFINS".'           
      INNER JOIN "RDN1" ON "RDN4"."DocEntry" = "RDN1"."DocEntry" AND "RDN4"."LineNum" = "RDN1"."LineNum"             
     GROUP BY "RDN4"."DocEntry", "RDN4"."LineNum", "RDN4"."NonDdctPrc"        
   ) "COFINS" ON "COFINS"."DocEntry" = "RDN1"."DocEntry" AND "COFINS"."LineNum" = "RDN1"."LineNum"          
   LEFT JOIN "OITM" ON "RDN1"."ItemCode" = "OITM"."ItemCode"    
   LEFT JOIN "ONCM" ON "OITM"."NCMCode" = "ONCM"."AbsEntry"  
   LEFT JOIN "RDN12" ON "ORDN"."DocEntry" = "RDN12"."DocEntry"   
         
 WHERE 0=0
	--AND "ORDN".Canceled = 'N'   
	AND "ORDN"."Model" <> '37'
	AND (SELECT max("S1"."U_cdErro")
		FROM "@SKL25NFE" "S1" 
			WHERE "S1"."U_DocEntry"="ORDN"."DocEntry" 
			AND "U_cdErro" = '100'
			AND (CASE "ORDN"."ObjType" 
					WHEN '13' THEN 'NS' 
					WHEN '14' THEN 'DN' 
					WHEN '15' THEN 'EM' 
					WHEN '16' THEN 'DT' 
					WHEN '18' THEN 'NE' 
					WHEN '19' THEN 'DE' 
					WHEN '20' THEN 'RM' 
					WHEN '21' THEN 'DM' 
					ELSE "ORDN"."ObjType"  END)="S1"."U_tipoDocumento")  = '100'      

 UNION ALL 

        
--------------------------------------------------------------------------------------------------------        
----------------------------------- ENTREGA ----------------------------------------------------------        
--------------------------------------------------------------------------------------------------------   
-- APENAS NOTAS COM O "Model"O 55

SELECT       
 "ODLN"."BPLId"					 "BPLId",        
 "DLN1"."DocEntry"               "DocEntry",        
 "DLN1"."ObjType"                "ObjType",      
 "ODLN"."DocDate"                "DocDate",
 "ODLN"."VATRegNum"				 "VATRegNum",    
 "ODLN"."CardCode"				 "CardCode",  
 "DLN12"."TaxId0"				 "TaxId0",      
 "ODLN"."Serial"                 "Serial",
 "ODLN"."SeriesStr"				 "SeriesStr",
 "ODLN"."SubStr"				 "SubStr",
 "ODLN"."Model"				     "Model",
 "DLN1"."LineNum"				 "LineNum",        
 "DLN1"."ItemCode"               "ItemCode",       
 "DLN1"."Dscription"             "Dscription",      
 "ONCM"."NcmCode"				 "NCMCode",       
 "DLN1"."Quantity"               "Quantity",         
 "DLN1"."Price"                  "Price",         
 "DLN1"."LineTotal"              "LineTotal",        
 "DLN1"."DistribSum"   "DistribSum",        
 "DLN1"."CSTCode"                "CSTCode",         
 "DLN1"."CFOPCode"               "CFOPCode",        
 "ICMS"."BaseSum"      "ICMS_BaseSum",        
 "ICMS"."TaxSum"       "ICMS_TaxSum",        
 "ICMS"."U_ExcAmtL"    "ICMS_U_ExcAmtL",        
 "ICMS"."U_OthAmtL"    "ICMS_U_OthAmtL",         
 "ICMSST"."BaseSum"    "ICMSST_BaseSum",        
 "ICMSST"."TaxSum"     "ICMSST_TaxSum",        
 "DLN1"."CSTfIPI"                "CSTfIPI",        
 "IPI"."NonDdctPrc"              "IPI_NonDdctPrc",        
 "IPI"."BaseSum"       "IPI_BaseSum",        
 "IPI"."TaxSum"        "IPI_TaxSum",        
 "IPI"."U_ExcAmtL"     "IPI_U_ExcAmtL",        
 "IPI"."U_OthAmtL"     "IPI_U_OthAmtL",        
 "DLN1"."CSTfPIS"                "CSTfPIS",        
 "PIS"."BaseSum"       "PIS_BaseSum",        
 "PIS"."TaxSum"        "PIS_TaxSum",        
 "PIS"."U_ExcAmtL"     "PIS_U_ExcAmtL",        
 "PIS"."U_OthAmtL"     "PIS_U_OthAmtL",        
 "DLN1"."CSTfCOFINS"             "CSTfCOFINS",        
 "COFINS"."BaseSum"    "COFINS_BaseSum",        
 "COFINS"."TaxSum"     "COFINS_TaxSum",        
 "COFINS"."U_ExcAmtL"  "COFINS_U_ExcAmtL",        
 "COFINS"."U_OthAmtL"  "COFINS_U_OthAmtL",
 (SELECT max("S1"."U_cdErro")
	FROM "@SKL25NFE" "S1" 
		WHERE "S1"."U_DocEntry"="ODLN"."DocEntry" 
		AND "U_cdErro" = '100'
		AND (CASE "ODLN"."ObjType" 
				WHEN '13' THEN 'NS' 
				WHEN '14' THEN 'DN' 
				WHEN '15' THEN 'EM' 
				WHEN '16' THEN 'DT' 
				WHEN '18' THEN 'NE' 
				WHEN '19' THEN 'DE' 
				WHEN '20' THEN 'RM' 
				WHEN '21' THEN 'DM' 
				ELSE "ODLN"."ObjType"  END)="S1"."U_tipoDocumento") AS "StatusNfe"         
        
        
 FROM "DLN1"         
     INNER JOIN "ODLN" ON "DLN1"."DocEntry" = "ODLN"."DocEntry"        
  LEFT JOIN         
   (        
   SELECT "DLN4"."DocEntry", "DLN4"."LineNum", "DLN4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "DLN4"                  
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "DLN4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'ICMS'           
      INNER JOIN "DLN1" ON "DLN4"."DocEntry" = "DLN1"."DocEntry" AND "DLN4"."LineNum" = "DLN1"."LineNum"             
     GROUP BY "DLN4"."DocEntry", "DLN4"."LineNum", "DLN4"."NonDdctPrc"        
   ) "ICMS" ON "ICMS"."DocEntry" = "DLN1"."DocEntry" AND "ICMS"."LineNum" = "DLN1"."LineNum"         
  LEFT JOIN         
   (        
   SELECT "DLN4"."DocEntry", "DLN4"."LineNum", "DLN4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "DLN4"                   
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "DLN4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'IPI'           
      INNER JOIN "DLN1" ON "DLN4"."DocEntry" = "DLN1"."DocEntry" AND "DLN4"."LineNum" = "DLN1"."LineNum"             
     GROUP BY "DLN4"."DocEntry", "DLN4"."LineNum", "DLN4"."NonDdctPrc"        
   ) "IPI" ON "IPI"."DocEntry" = "DLN1"."DocEntry" AND "IPI"."LineNum" = "DLN1"."LineNum"           
  LEFT JOIN         
   (        
   SELECT "DLN4"."DocEntry", "DLN4"."LineNum", "DLN4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "DLN4"          
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "DLN4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'ICMS-ST'           
      INNER JOIN "DLN1" ON "DLN4"."DocEntry" = "DLN1"."DocEntry" AND "DLN4"."LineNum" = "DLN1"."LineNum"             
     GROUP BY "DLN4"."DocEntry", "DLN4"."LineNum", "DLN4"."NonDdctPrc"        
   ) "ICMSST" ON "ICMSST"."DocEntry" = "DLN1"."DocEntry" AND "ICMSST"."LineNum" = "DLN1"."LineNum"           
  LEFT JOIN         
   (        
   SELECT "DLN4"."DocEntry", "DLN4"."LineNum", "DLN4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "DLN4"          
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "DLN4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'PIS'           
      INNER JOIN "DLN1" ON "DLN4"."DocEntry" = "DLN1"."DocEntry" AND "DLN4"."LineNum" = "DLN1"."LineNum"             
     GROUP BY "DLN4"."DocEntry", "DLN4"."LineNum", "DLN4"."NonDdctPrc"        
   ) "PIS" ON "PIS"."DocEntry" = "DLN1"."DocEntry" AND "PIS"."LineNum" = "DLN1"."LineNum"          
  LEFT JOIN         
   (        
   SELECT "DLN4"."DocEntry", "DLN4"."LineNum", "DLN4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "DLN4"          
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "DLN4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = '"COFINS".'           
      INNER JOIN "DLN1" ON "DLN4"."DocEntry" = "DLN1"."DocEntry" AND "DLN4"."LineNum" = "DLN1"."LineNum"             
     GROUP BY "DLN4"."DocEntry", "DLN4"."LineNum", "DLN4"."NonDdctPrc"        
   ) "COFINS" ON "COFINS"."DocEntry" = "DLN1"."DocEntry" AND "COFINS"."LineNum" = "DLN1"."LineNum"     
   LEFT JOIN "OITM" ON "DLN1"."ItemCode" = "OITM"."ItemCode"    
   LEFT JOIN "ONCM" ON "OITM"."NCMCode" = "ONCM"."AbsEntry"   
   LEFT JOIN "DLN12" ON "DLN12"."DocEntry" = "ODLN"."DocEntry"  
        
         
 WHERE 0=0
	--AND "ODLN".Canceled = 'N'
	AND "ODLN"."Model" = '39'
	AND (SELECT max("S1"."U_cdErro")
		FROM "@SKL25NFE" S1 
			WHERE "S1"."U_DocEntry"="ODLN"."DocEntry" 
			AND "U_cdErro" = '100'
			AND (CASE "ODLN"."ObjType" 
					WHEN '13' THEN 'NS' 
					WHEN '14' THEN 'DN' 
					WHEN '15' THEN 'EM' 
					WHEN '16' THEN 'DT' 
					WHEN '18' THEN 'NE' 
					WHEN '19' THEN 'DE' 
					WHEN '20' THEN 'RM' 
					WHEN '21' THEN 'DM' 
					ELSE "ODLN"."ObjType"  END)="S1"."U_tipoDocumento") = '100'  
	


UNION ALL         
        
--------------------------------------------------------------------------------------------------------        
----------------------------------- DEV MERCADORIA -----------------------------------------------------        
--------------------------------------------------------------------------------------------------------   
-- APENAS NOTAS COM O "Model"O 55


SELECT       
 "ORPD"."BPLId"					 "BPLId",        
 "RPD1"."DocEntry"               "DocEntry",        
 "RPD1"."ObjType"                "ObjType",      
 "ORPD"."DocDate"                "DocDate",
 "ORPD"."VATRegNum"				 "VATRegNum",    
 "ORPD"."CardCode"				 "CardCode",  
 "RPD12"."TaxId0"				 "TaxId0",      
 "ORPD"."Serial"                 "Serial",
 "ORPD"."SeriesStr"				 "SeriesStr",
 "ORPD"."SubStr"				 "SubStr",
 "ORPD"."Model"				     "Model",
 "RPD1"."LineNum"				 "LineNum",        
 "RPD1"."ItemCode"               "ItemCode",       
 "RPD1"."Dscription"             "Dscription",      
 "ONCM"."NcmCode"				 "NCMCode",       
 "RPD1"."Quantity"               "Quantity",         
 "RPD1"."Price"                  "Price",         
 "RPD1"."LineTotal"              "LineTotal",        
 "RPD1"."DistribSum"   "DistribSum",        
 "RPD1"."CSTCode"                "CSTCode",         
 "RPD1"."CFOPCode"               "CFOPCode",        
 "ICMS"."BaseSum"      "ICMS_BaseSum",        
 "ICMS"."TaxSum"       "ICMS_TaxSum",        
 "ICMS"."U_ExcAmtL"    "ICMS_U_ExcAmtL",        
 "ICMS"."U_OthAmtL"    "ICMS_U_OthAmtL",         
 "ICMSST"."BaseSum"    "ICMSST_BaseSum",        
 "ICMSST"."TaxSum"     "ICMSST_TaxSum",        
 "RPD1"."CSTfIPI"                "CSTfIPI",        
 "IPI"."NonDdctPrc"              "IPI_NonDdctPrc",        
 "IPI"."BaseSum"       "IPI_BaseSum",        
 "IPI"."TaxSum"        "IPI_TaxSum",        
 "IPI"."U_ExcAmtL"     "IPI_U_ExcAmtL",        
 "IPI"."U_OthAmtL"     "IPI_U_OthAmtL",        
 "RPD1"."CSTfPIS"                "CSTfPIS",        
 "PIS"."BaseSum"       "PIS_BaseSum",        
 "PIS"."TaxSum"        "PIS_TaxSum",        
 "PIS"."U_ExcAmtL"     "PIS_U_ExcAmtL",        
 "PIS"."U_OthAmtL"     "PIS_U_OthAmtL",        
 "RPD1"."CSTfCOFINS"             "CSTfCOFINS",        
 "COFINS"."BaseSum"    "COFINS_BaseSum",        
 "COFINS"."TaxSum"     "COFINS_TaxSum",        
 "COFINS"."U_ExcAmtL"  "COFINS_U_ExcAmtL",        
 "COFINS"."U_OthAmtL"  "COFINS_U_OthAmtL",
 (SELECT max("S1"."U_cdErro")
	FROM "@SKL25NFE" "S1" 
		WHERE "S1"."U_DocEntry"="ORPD"."DocEntry" 
		AND "U_cdErro" = '100'
		AND (CASE "ORPD"."ObjType" 
				WHEN '13' THEN 'NS' 
				WHEN '14' THEN 'DN' 
				WHEN '15' THEN 'EM' 
				WHEN '16' THEN 'DT' 
				WHEN '18' THEN 'NE' 
				WHEN '19' THEN 'DE' 
				WHEN '20' THEN 'RM' 
				WHEN '21' THEN 'DM' 
				ELSE "ORPD"."ObjType"  END)="S1"."U_tipoDocumento") AS "StatusNfe"          
        
        
 FROM "RPD1"         
     INNER JOIN "ORPD" ON "RPD1"."DocEntry" = "ORPD"."DocEntry"        
  LEFT JOIN         
   (        
   SELECT "RPD4"."DocEntry", "RPD4"."LineNum", "RPD4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "RPD4"                  
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "RPD4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'ICMS'           
      INNER JOIN "RPD1" ON "RPD4"."DocEntry" = "RPD1"."DocEntry" AND "RPD4"."LineNum" = "RPD1"."LineNum"             
     GROUP BY "RPD4"."DocEntry", "RPD4"."LineNum", "RPD4"."NonDdctPrc"        
   ) "ICMS" ON "ICMS"."DocEntry" = "RPD1"."DocEntry" AND "ICMS"."LineNum" = "RPD1"."LineNum"         
  LEFT JOIN         
   (        
   SELECT "RPD4"."DocEntry", "RPD4"."LineNum", "RPD4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "RPD4"                   
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "RPD4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'IPI'           
      INNER JOIN "RPD1" ON "RPD4"."DocEntry" = "RPD1"."DocEntry" AND "RPD4"."LineNum" = "RPD1"."LineNum"             
     GROUP BY "RPD4"."DocEntry", "RPD4"."LineNum", "RPD4"."NonDdctPrc"        
   ) "IPI" ON "IPI"."DocEntry" = "RPD1"."DocEntry" AND "IPI"."LineNum" = "RPD1"."LineNum"           
  LEFT JOIN         
   (        
   SELECT "RPD4"."DocEntry", "RPD4"."LineNum", "RPD4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "RPD4"          
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "RPD4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'ICMS-ST'           
      INNER JOIN "RPD1" ON "RPD4"."DocEntry" = "RPD1"."DocEntry" AND "RPD4"."LineNum" = "RPD1"."LineNum"             
     GROUP BY "RPD4"."DocEntry", "RPD4"."LineNum", "RPD4"."NonDdctPrc"        
   ) "ICMSST" ON "ICMSST"."DocEntry" = "RPD1"."DocEntry" AND "ICMSST"."LineNum" = "RPD1"."LineNum"           
  LEFT JOIN         
   (        
   SELECT "RPD4"."DocEntry", "RPD4"."LineNum", "RPD4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "RPD4"          
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "RPD4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = 'PIS'           
      INNER JOIN "RPD1" ON "RPD4"."DocEntry" = "RPD1"."DocEntry" AND "RPD4"."LineNum" = "RPD1"."LineNum"             
     GROUP BY "RPD4"."DocEntry", "RPD4"."LineNum", "RPD4"."NonDdctPrc"        
   ) "PIS" ON "PIS"."DocEntry" = "RPD1"."DocEntry" AND "PIS"."LineNum" = "RPD1"."LineNum"          
  LEFT JOIN         
   (        
   SELECT "RPD4"."DocEntry", "RPD4"."LineNum", "RPD4"."NonDdctPrc", SUM("TaxSum") "TaxSum", SUM("BaseSum") "BaseSum", SUM("U_ExcAmtL") "U_ExcAmtL", SUM("U_OthAmtL") "U_OthAmtL" FROM  "RPD4"          
      INNER JOIN "OSTT" ON "OSTT"."AbsId" = "RPD4"."staType"                  
      INNER JOIN "ONFT" ON "ONFT"."AbsId" = "OSTT"."NfTaxId" AND "ONFT"."Code" = '"COFINS".'           
      INNER JOIN "RPD1" ON "RPD4"."DocEntry" = "RPD1"."DocEntry" AND "RPD4"."LineNum" = "RPD1"."LineNum"             
     GROUP BY "RPD4"."DocEntry", "RPD4"."LineNum", "RPD4"."NonDdctPrc"        
   ) "COFINS" ON "COFINS"."DocEntry" = "RPD1"."DocEntry" AND "COFINS"."LineNum" = "RPD1"."LineNum"     
   LEFT JOIN "OITM" ON "RPD1"."ItemCode" = "OITM"."ItemCode"    
   LEFT JOIN "ONCM" ON "OITM"."NCMCode" = "ONCM"."AbsEntry"   
   LEFT JOIN "RPD12" ON "RPD12"."DocEntry" = "ORPD"."DocEntry"  
        
         
 WHERE 0=0
	AND "ORPD"."CANCELED" = 'N' 
	AND "ORPD"."Model" = '39'  
	AND  (SELECT max("S1"."U_cdErro")
			FROM "@SKL25NFE" "S1" 
				WHERE "S1"."U_DocEntry"="ORPD"."DocEntry" 
				AND "U_cdErro" = '100'
				AND (CASE "ORPD"."ObjType" 
						WHEN '13' THEN 'NS' 
						WHEN '14' THEN 'DN' 
						WHEN '15' THEN 'EM' 
						WHEN '16' THEN 'DT' 
						WHEN '18' THEN 'NE' 
						WHEN '19' THEN 'DE' 
						WHEN '20' THEN 'RM' 
						WHEN '21' THEN 'DM' 
						ELSE "ORPD"."ObjType"  END)="S1"."U_tipoDocumento") = '100'
						


