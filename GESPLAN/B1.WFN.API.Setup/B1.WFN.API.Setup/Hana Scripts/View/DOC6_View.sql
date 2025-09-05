create view "DOC6_View" as

	select OPCH."DocEntry", OPCH."ObjType", 'OPCH' as "Table", OPCH."CardCode", 
		   OPCH."Model", OPCH."Serial", OPCH."SeriesStr", OPCH."SubStr", 
		   OPCH."TaxDate", OPCH."DocDate", OPCH."CANCELED", OPCH."Installmnt", 
		   OPCH."Comments", PCH6."InstlmntID", PCH6."DueDate", PCH6."InsTotal", 
		   PCH6."Status", null as "DebCred"
	  from OPCH  
	 inner join PCH6 on PCH6."DocEntry" = OPCH."DocEntry"
	 
	 union all
	 
	select ORPC."DocEntry", ORPC."ObjType", 'ORPC' as "Table", ORPC."CardCode", 
		   ORPC."Model", ORPC."Serial", ORPC."SeriesStr", ORPC."SubStr", 
		   ORPC."TaxDate", ORPC."DocDate", ORPC."CANCELED", ORPC."Installmnt", 
		   ORPC."Comments", RPC6."InstlmntID", RPC6."DueDate", RPC6."InsTotal", 
		   RPC6."Status", null as "DebCred"
	  from ORPC  
	 inner join RPC6 on RPC6."DocEntry" = ORPC."DocEntry"
	 
	 union all
	 
	select ODPO."DocEntry", ODPO."ObjType", 'ODPO' as "Table", ODPO."CardCode", 
		   ODPO."Model", ODPO."Serial", ODPO."SeriesStr", ODPO."SubStr", 
		   ODPO."TaxDate", ODPO."DocDate", ODPO."CANCELED", ODPO."Installmnt", 
		   ODPO."Comments", DPO6."InstlmntID", DPO6."DueDate", DPO6."InsTotal", 
		   DPO6."Status", null as "DebCred"
	  from ODPO  
	 inner join DPO6 on DPO6."DocEntry" = ODPO."DocEntry"

	 union all

	select OINV."DocEntry", OINV."ObjType", 'OINV' as "Table", OINV."CardCode", 
		   OINV."Model", OINV."Serial", OINV."SeriesStr", OINV."SubStr", 
		   OINV."TaxDate", OINV."DocDate", OINV."CANCELED", OINV."Installmnt", 
		   OINV."Comments", INV6."InstlmntID", INV6."DueDate", INV6."InsTotal", 
		   INV6."Status", null as "DebCred"
	  from OINV  
	 inner join INV6 on INV6."DocEntry" = OINV."DocEntry"
	 
	 union all

	select ORIN."DocEntry", ORIN."ObjType", 'ORIN' as "Table", ORIN."CardCode", 
		   ORIN."Model", ORIN."Serial", ORIN."SeriesStr", ORIN."SubStr", 
		   ORIN."TaxDate", ORIN."DocDate", ORIN."CANCELED", ORIN."Installmnt", 
		   ORIN."Comments", RIN6."InstlmntID", RIN6."DueDate", RIN6."InsTotal", 
		   RIN6."Status", null as "DebCred"
	  from ORIN  
	 inner join RIN6 on RIN6."DocEntry" = ORIN."DocEntry"

	 union all
	 
	select ODPI."DocEntry", ODPI."ObjType", 'ODPI' as "Table", ODPI."CardCode", 
		   ODPI."Model", ODPI."Serial", ODPI."SeriesStr", ODPI."SubStr", 
		   ODPI."TaxDate", ODPI."DocDate", ODPI."CANCELED", ODPI."Installmnt", 
		   ODPI."Comments", DPI6."InstlmntID", DPI6."DueDate", DPI6."InsTotal", 
		   DPI6."Status", null as "DebCred"
	  from ODPI  
	 inner join DPI6 on DPI6."DocEntry" = ODPI."DocEntry"
	 
	 union all
	 
	select OJDT."TransId" as "DocEntry", OJDT."ObjType", 'OJDT' as "Table", OCRD."CardCode",
		   null as "Model", null as "Serial", null as "SeriesStr", null as "SubStr", 
		   OJDT."TaxDate", OJDT."RefDate" as "DocDate", 
		   coalesce((select distinct 'Y' from OJDT as "A0" where "A0"."StornoToTr" = OJDT."TransId"), 'N') as "CANCELED",
		   (select count(A0."DueDate") as "Qty" 
			  from JDT1 as "A0"
			 inner join OCRD as "A1" on A1."CardCode" = A0."ShortName"
			 where A0."TransId" = OJDT."TransId") as "Installmnt",
		   OJDT."Memo" as "Comments", 
		   (select "InstlmntID"
			  from (select A0."Line_ID", row_number()over(order by A0."DueDate") as "InstlmntID"
			  		  from JDT1 as "A0"
				     inner join OCRD as "A1" on A1."CardCode" = A0."ShortName"
				     where A0."TransId" = OJDT."TransId") as "Instmnts"
			  where "Instmnts"."Line_ID" = JDT1."Line_ID") as "InstlmntID", JDT1."DueDate", JDT1."Debit" + JDT1."Credit" as "InsTotal", 
		   null as "Status", JDT1."DebCred"
	  from OJDT 
	 inner join JDT1 on JDT1."TransId" = OJDT."TransId"
	 inner join OCRD on OCRD."CardCode" = JDT1."ShortName"
	 where OJDT."TransType" = '30'	  
       and OJDT."StornoToTr" is null