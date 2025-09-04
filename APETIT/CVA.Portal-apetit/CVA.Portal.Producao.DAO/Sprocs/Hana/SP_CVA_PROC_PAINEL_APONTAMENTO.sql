CREATE PROCEDURE "SP_CVA_PROC_PAINEL_APONTAMENTO"(
    vBplId varchar(50),
	vDtDe TIMESTAMP,
	vDtAte TIMESTAMP
)  
LANGUAGE SQLSCRIPT AS
BEGIN  

/***************************************/	
declare v_COUNT int;

/***************************************/

SELECT COUNT (*) INTO v_COUNT FROM M_TEMPORARY_TABLES WHERE SCHEMA_NAME=CURRENT_SCHEMA 
AND UPPER(TABLE_NAME)='#TBT_DATES';
IF v_COUNT>=1 THEN
	DROP TABLE #TBT_DATES;
END IF;

SELECT COUNT (*) INTO v_COUNT FROM M_TEMPORARY_TABLES WHERE SCHEMA_NAME=CURRENT_SCHEMA 
AND UPPER(TABLE_NAME)='#TBT_DATES_DESC';
IF v_COUNT>=1 THEN
	DROP TABLE #TBT_DATES_DESC;
END IF;

SELECT COUNT (*) INTO v_COUNT FROM M_TEMPORARY_TABLES WHERE SCHEMA_NAME=CURRENT_SCHEMA 
AND UPPER(TABLE_NAME)='#TBT_APONTAMENTO_INIT';
IF v_COUNT>=1 THEN
	DROP TABLE #TBT_APONTAMENTO_INIT;
END IF;

SELECT COUNT (*) INTO v_COUNT FROM M_TEMPORARY_TABLES WHERE SCHEMA_NAME=CURRENT_SCHEMA 
AND UPPER(TABLE_NAME)='#TBT_APONTAMENTO_END';
IF v_COUNT>=1 THEN
	DROP TABLE #TBT_APONTAMENTO_END;
END IF;

/***************************************/
create local temporary 
table #TBT_DATES
(   	
	Dt Date
);

create local temporary 
table #TBT_DATES_DESC
(   	
	DayOfWeek int,
	DayOfWeekDesc NVARCHAR(100)
);

create local temporary 
table #TBT_APONTAMENTO_INIT
(   	
	Dt Date,
	DayOfWeek INT,
	DayOfWeekDesc NVARCHAR(100),
	ServicoCode NVARCHAR(100),
	ServicoName NVARCHAR(100),
	TurnoCode NVARCHAR(100),
	TurnoName NVARCHAR(100),
	QtyPL DECIMAL(19,6),
	QtyAP DECIMAL(19,6)
);


create local temporary 
table #TBT_APONTAMENTO_END
(   	
	Dt Date,
	DayOfWeek INT,
	DayOfWeekDesc NVARCHAR(100),
	ServicoCode NVARCHAR(100),
	ServicoName NVARCHAR(100),
	TurnoCode NVARCHAR(100),
	TurnoName NVARCHAR(100),
	QtyPL DECIMAL(19,6),
	QtyAP DECIMAL(19,6),
	Diff DECIMAL(19,6)
);



/***************************************/
--Valida a qtd de datas
insert into #TBT_DATES values (vDtDe);
	WHILE vDtDe < vDtAte DO
		vDtDe := ADD_DAYS(vDtDe, 1);
		insert into #TBT_DATES values (vDtDe);
	END WHILE;


--Add Descrição nos dias da semana
insert into #TBT_DATES_DESC values (1,'Domingo');
insert into #TBT_DATES_DESC values (2,'Segunda');
insert into #TBT_DATES_DESC values (3,'Terça');
insert into #TBT_DATES_DESC values (4,'Quarta');
insert into #TBT_DATES_DESC values (5,'Quinta');
insert into #TBT_DATES_DESC values (6,'Sexta');
insert into #TBT_DATES_DESC values (7,'Sabado');


--Popula os dt em registros na tb
insert into #TBT_APONTAMENTO_INIT
	select DISTINCT "DT", dayofweek("DT") "DayOfWeek", "DAYOFWEEKDESC", C."U_CVA_ID_SERVICO", C."U_CVA_DES_SERVICO" "U_CVA_D_SERVICO", G."Code" "TurnoCode", G."Name" "TurnoName"
	    ,IFNULL(
				F."U_Quantity"
		,0) as "QtyPL"
		,IFNULL(
			(select sum("U_QTYREF") 
				from "@CVA_APTO_TERCEIROS" w0
				where w0."U_FILIAL" in(vBplId)
				and TO_DATE(w0."CreateDate") = TO_DATE(A."DT")
				and w0."U_TURNO" = G."Name"
				and w0."U_SERVICO" = C."U_CVA_ID_SERVICO")
		,0) as "QtyAP"
	from #TBT_DATES A
	inner join "@CVA_LN_PLANEJAMENTO" B ON EXTRACT (DAY FROM TO_DATE (A."DT", 'YYYY-MM-DD')) = B."U_Day"
	INNER JOIN "@CVA_PLANEJAMENTO" C ON C."DocEntry" = B."DocEntry" AND C."U_CVA_ID_CONTRATO" in (select "Number" from "OOAT" where "U_CVA_FILIAL" in (vBplId))
	INNER JOIN "@CVA_GRPSERVICOS" D ON D."U_ServiceId" = C."U_CVA_ID_SERVICO"
	INNER JOIN "@CVA_LIN_GRPSERVICOS" E ON E."Code" = D."Code"
	INNER JOIN "@CVA_MNP2" F ON E."U_CVA_TURNO" = F."U_Shift" AND B."U_Day" = F."U_Day" and F."DocEntry" in (select "DocEntry" from "@CVA_PLANEJAMENTO" where "U_CVA_ID_CONTRATO" in (select "Number" from "OOAT" where "U_CVA_FILIAL" in (vBplId)))
	INNER JOIN "@CVA_TURNO" G on F."U_Shift" = G."Name"
	INNER JOIN #TBT_DATES_DESC H ON H."DAYOFWEEK" = dayofweek("DT");


--Cacula sum() por serviço/contrato

insert into #TBT_APONTAMENTO_END
	select t0.*
	,(IFNULL(t0.QtyPL,0) - IFNULL(t0.QtyAP,0))
	from #TBT_APONTAMENTO_INIT t0;

 	
--select final	
select * from #TBT_APONTAMENTO_END
order by DT, SERVICONAME, TURNOCODE
;

/***************************************/		

DROP TABLE #TBT_DATES;
DROP TABLE #TBT_DATES_DESC;
DROP TABLE #TBT_APONTAMENTO_INIT;
DROP TABLE #TBT_APONTAMENTO_END;

/***************************************/	
	
END;