create view "ODOC_View" as 
		
	select OPCH."DocEntry", 'AP' as "TransType", OPCH."DocNum", OPCH."ObjType", OPCH."Model", OPCH."Serial", OPCH."TransId",
		   OPCH."CardCode", case OCRD."CardType" when 'C' then '1' when 'S' then '3' else '6' end "CardType",		   
		   OPCH."DocTotal", OPCH."SeriesStr", OPCH."SubStr", OPCH."TaxDate", OPCH."DocDate", OPCH."DocCur",
		   OPCH."DiscPrcnt", OPCH."DpmAmnt", OPCH."TotalExpns", OPCH."VatSum", OPCH."Installmnt", 
		   OPCH."BPLId", OPCH."JrnlMemo", OPCH."UpdateDate", OPCH."UpdateTS", 'S' as "EventType", OPCH."CANCELED"
	  from OPCH  
	 inner join OCRD on OCRD."CardCode" = OPCH."CardCode"
	 where OPCH."CANCELED" <> 'C'
	 
	union all 

	select ORPC."DocEntry", 'AR' as "TransType", ORPC."DocNum", ORPC."ObjType", ORPC."Model", ORPC."Serial", ORPC."TransId",
		   ORPC."CardCode", case OCRD."CardType" when 'C' then '1' when 'S' then '3' else '6' end "CardType",		
		   ORPC."DocTotal", ORPC."SeriesStr", ORPC."SubStr", ORPC."TaxDate", ORPC."DocDate", ORPC."DocCur",
		   ORPC."DiscPrcnt", ORPC."DpmAmnt", ORPC."TotalExpns", ORPC."VatSum", ORPC."Installmnt", 
		   ORPC."BPLId", ORPC."JrnlMemo", ORPC."UpdateDate", ORPC."UpdateTS", 'E' as "EventType", ORPC."CANCELED"
	  from ORPC  
	 inner join OCRD on OCRD."CardCode" = ORPC."CardCode"
	 where ORPC."CANCELED" <> 'C'
	 
	union all 

	select ODPO."DocEntry", 'AP' as "TransType", ODPO."DocNum", ODPO."ObjType", ODPO."Model", ODPO."Serial", ODPO."TransId",
		   ODPO."CardCode", case OCRD."CardType" when 'C' then '1' when 'S' then '3' else '6' end "CardType",		
		   ODPO."DocTotal", ODPO."SeriesStr", ODPO."SubStr", ODPO."TaxDate", ODPO."DocDate", ODPO."DocCur",
		   ODPO."DiscPrcnt", ODPO."DpmAmnt", ODPO."TotalExpns", ODPO."VatSum", ODPO."Installmnt", 
		   ODPO."BPLId", ODPO."JrnlMemo", ODPO."UpdateDate", ODPO."UpdateTS", 'S' as "EventType", 
		   coalesce((select 'Y' from RPC1 where RPC1."BaseEntry" = ODPO."DocEntry" and RPC1."BaseType" = ODPO."ObjType"), 'N') as "CANCELED"
	  from ODPO  
	 inner join OCRD on OCRD."CardCode" = ODPO."CardCode"
	 
	union all 

	select OINV."DocEntry", 'AR' as "TransType", OINV."DocNum", OINV."ObjType", OINV."Model", OINV."Serial", OINV."TransId",
		   OINV."CardCode", case OCRD."CardType" when 'C' then '1' when 'S' then '3' else '6' end "CardType",		
		   OINV."DocTotal", OINV."SeriesStr", OINV."SubStr", OINV."TaxDate", OINV."DocDate", OINV."DocCur",
		   OINV."DiscPrcnt", OINV."DpmAmnt", OINV."TotalExpns", OINV."VatSum", OINV."Installmnt", 
		   OINV."BPLId", OINV."JrnlMemo", OINV."UpdateDate", OINV."UpdateTS", 'E' as "EventType", OINV."CANCELED"
	  from OINV  
	 inner join OCRD on OCRD."CardCode" = OINV."CardCode"
	 where OINV."CANCELED" <> 'C'
	 
	union all 

	select ORIN."DocEntry", 'AP' as "TransType", ORIN."DocNum", ORIN."ObjType", ORIN."Model", ORIN."Serial", ORIN."TransId",
		   ORIN."CardCode", case OCRD."CardType" when 'C' then '1' when 'S' then '3' else '6' end "CardType",		
		   ORIN."DocTotal", ORIN."SeriesStr", ORIN."SubStr", ORIN."TaxDate", ORIN."DocDate", ORIN."DocCur",
		   ORIN."DiscPrcnt", ORIN."DpmAmnt", ORIN."TotalExpns", ORIN."VatSum", ORIN."Installmnt", 
		   ORIN."BPLId", ORIN."JrnlMemo", ORIN."UpdateDate", ORIN."UpdateTS", 'S' as "EventType", ORIN."CANCELED"
	  from ORIN  
	 inner join OCRD on OCRD."CardCode" = ORIN."CardCode"
	 where ORIN."CANCELED" <> 'C'
	 
	union all 
	
	select ODPI."DocEntry", 'AR' as "TransType", ODPI."DocNum", ODPI."ObjType", ODPI."Model", ODPI."Serial", ODPI."TransId",
		   ODPI."CardCode", case OCRD."CardType" when 'C' then '1' when 'S' then '3' else '6' end "CardType",		
		   ODPI."DocTotal", ODPI."SeriesStr", ODPI."SubStr", ODPI."TaxDate", ODPI."DocDate", ODPI."DocCur",
		   ODPI."DiscPrcnt", ODPI."DpmAmnt", ODPI."TotalExpns", ODPI."VatSum", ODPI."Installmnt", 
		   ODPI."BPLId", ODPI."JrnlMemo", ODPI."UpdateDate", ODPI."UpdateTS", 'E' as "EventType", 
		   coalesce((select 'Y' from RIN1 where RIN1."BaseEntry" = ODPI."DocEntry" and RIN1."BaseType" = ODPI."ObjType"), 'N') as "CANCELED"
	  from ODPI 
	 inner join OCRD on OCRD."CardCode" = ODPI."CardCode"

 	 union all 
	
	select OJDT."TransId" as "DocEntry", 
		   case (select distinct JDT1."DebCred"
				   from JDT1
				  inner join OCRD on OCRD."CardCode" = JDT1."ShortName"
   			      where JDT1."TransId" = OJDT."TransId") when 'C' then 'AP' else 'AR' end as "TransType", 
		   OJDT."Number" as "DocNum", '30' as "ObjType", null as "Model", null as "Serial", OJDT."TransId",
		   (select max(OCRD."CardCode")
				   from JDT1
				  inner join OCRD on OCRD."CardCode" = JDT1."ShortName"
   			      where JDT1."TransId" = OJDT."TransId") as "CardCode",
		   case (select distinct OCRD."CardType"
				   from JDT1
				  inner join OCRD on OCRD."CardCode" = JDT1."ShortName"
   			      where JDT1."TransId" = OJDT."TransId") when 'C' then '1' when 'S' then '3' else '6' end "CardType",
		   OJDT."LocTotal", null as "SeriesStr", null as "SubStr", OJDT."TaxDate", OJDT."RefDate" as "DocDate", OJDT."TransCurr" as "DocCur",
		   0.0 as "DiscPrcnt", 0.0 as "DpmAmnt", 0.0 as "TotalExpns", 0.0 as "VatSum", 
		   (select count(JDT1."DueDate") as "Qty" 
			  from JDT1 
			 inner join OCRD on OCRD."CardCode" = JDT1."ShortName"
			 where JDT1."TransId" = OJDT."TransId") as "Installmnt",
		   (select distinct JDT1."BPLId"
			  from JDT1 
			 where JDT1."TransId" = OJDT."TransId") as "BPLId", 
		   OJDT."Memo" as "JrnlMemo", OJDT."CreateDate", OJDT."U_UpdateTS" as "UpdateTS", 
		   case (select distinct OCRD."CardType"
				   from JDT1
				  inner join OCRD on OCRD."CardCode" = JDT1."ShortName"
   			      where JDT1."TransId" = OJDT."TransId") when 'C' then 'E' else 'S' end as "EventType", 
		   coalesce((select distinct 'Y' from OJDT as "A0" where "A0"."StornoToTr" = OJDT."TransId"), 'N') as "CANCELED"
	  from OJDT
     where OJDT."TransType" = '30'	  
       and OJDT."StornoToTr" is null