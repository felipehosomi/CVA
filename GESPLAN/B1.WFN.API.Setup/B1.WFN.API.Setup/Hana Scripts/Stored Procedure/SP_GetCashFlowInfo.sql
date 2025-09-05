create procedure SP_GetCashFlowInfo
(
	fromDate datetime,
	toDate datetime,
	partType char
)
as
begin
	-- Header
	if :partType = 'H' then
		select ODOC."DocEntry", ODOC."DocNum", ODOC."BPLId", ODOC."TransType", ODOC."ObjType", 
			   ODOC."EventType", ODOC."CANCELED" as "DocCancel", ODOC."DocDate", ODOC."Serial", 
			   ODOC."DocCur", ODOC."Installmnt", ODOC."JrnlMemo", ODOC."DocTotal",
			   (select OPCH."Comments" from OPCH  inner join VPM1 as "VPM1" on VPM1."DocNum" = OPCH."DocNum" and VPM1."ObjType" = 46) as "Comments",
			   (select OCRD."U_GrupoDespesa" from OCRD  where OCRD."CardCode" = ODOC."CardCode") as "U_GrupoDespesa",
			   (select OCRD."U_SubGrupoDespesa" from OCRD  where OCRD."CardCode" = ODOC."CardCode") as "U_GrupoDespesa"
		  from "ODOC_View" as "ODOC"
		 inner join "DOC6_View" as "DOC6" on DOC6."DocEntry" = ODOC."DocEntry" 
		   and DOC6."ObjType" = ODOC."ObjType"
		 where to_timestamp(cast(ODOC."UpdateDate" as varchar(10)) || right('000000' || cast("UpdateTS" as varchar(6)), 6), 'YYYY-MM-DDHH24MISS') >= :fromDate
		   and to_timestamp(cast(ODOC."UpdateDate" as varchar(10)) || right('000000' || cast("UpdateTS" as varchar(6)), 6), 'YYYY-MM-DDHH24MISS') <= :toDate;
		-- Abertura Financeira
	elseif :partType = 'F' then
		select ODOC."DocEntry", ODOC."CardCode", ODOC."CardType", OPAY."PaymentType", ODOC."EventType", 
			   DOC6."InstlmntID", DOC6."DueDate", DOC6."InsTotal", OPAY."PaymentDate" as "PayDate", 
			   OPAY."PmntAcct", OPAY."BoeNum", OPAY."SumApplied",		  
			   case ODOC."CANCELED" when 'Y' then ODOC."CANCELED" else OPAY."Canceled" end as "PayCancel"			   
		  from "ODOC_View" as "ODOC"
		 inner join "DOC6_View" as "DOC6" on DOC6."DocEntry" = ODOC."DocEntry"  
		   and DOC6."ObjType" = ODOC."ObjType"
		  left join "OPAY_View" as "OPAY" on OPAY."DocEntry" = DOC6."DocEntry"
		   and OPAY."InvType" = ODOC."ObjType" 	
		   and OPAY."InstId" = DOC6."InstlmntID"
		 where to_timestamp(cast(ODOC."UpdateDate" as varchar(10)) || right('000000' || cast("UpdateTS" as varchar(6)), 6), 'YYYY-MM-DDHH24MISS') >= :fromDate
		   and to_timestamp(cast(ODOC."UpdateDate" as varchar(10)) || right('000000' || cast("UpdateTS" as varchar(6)), 6), 'YYYY-MM-DDHH24MISS') <= :toDate;
	-- Abertura Contábil
	elseif :partType = 'C' then
		select 
	ODOC."DocEntry", 
	ODOC."EventType", 
	JDT1."Line_ID", 
	JDT1."Account", 
	ODOC."CANCELED" as "JrnlCancel", 
	INV1."ItemCode" as "ItemCode",
	JDT1."Credit" + JDT1."Debit" as "JrnlLineValue", 
	JDT1."RefDate", JDT1."DueDate",
	JDT1."ProfitCode" || ' ' || JDT1."OcrCode2" || ' ' || JDT1."OcrCode3" || ' ' || JDT1."OcrCode4" || ' ' || JDT1."OcrCode5" as "ProfitCode"			  
from "ODOC_View" as "ODOC"
	inner join "DOC6_View" as "DOC6" on DOC6."DocEntry" = ODOC."DocEntry"  
		 and DOC6."ObjType" = ODOC."ObjType"
	inner join OJDT on OJDT."TransId" = ODOC."TransId"
	inner join JDT1 on JDT1."TransId" = OJDT."TransId"
		   and case ODOC."EventType" when 'S' then JDT1."Credit" when 'E' then JDT1."Debit" end > 0.0
	inner join INV1 on INV1."DocEntry" = ODOC."DocEntry"
			
		 where to_timestamp(cast(ODOC."UpdateDate" as varchar(10)) || right('000000' || cast("UpdateTS" as varchar(6)), 6), 'YYYY-MM-DDHH24MISS') >= :fromDate
		   and to_timestamp(cast(ODOC."UpdateDate" as varchar(10)) || right('000000' || cast("UpdateTS" as varchar(6)), 6), 'YYYY-MM-DDHH24MISS') <= :toDate;
		end if;
end