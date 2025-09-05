



/*
select * from "@CVA_CONFERENCIA";
select * from "@CVA_CONFERENCIA_LINHA";*/



-- Conferencia
Create Table "@CVA_CONFERENCIA"
(
	 CVA_DocEntry int primary key
	,CVA_DtInicial_1 LONGDATE
	,CVA_DtFinal_1   LONGDATE
	,CVA_DtInicial_2 LONGDATE 
	,CVA_DtFinal_2   LONGDATE
	,CVA_Status_1    char(1)
	,CVA_Status_2    char(1)
	,CVA_DocStatus   char(1)
);

-- Conferencia Linha
Create  table "@CVA_CONFERENCIA_LINHA"
(
	 CVA_DocEntry  int not null
	,CVA_LineNum   int
	,CVA_ItemCode  nvarchar(50)
	,CVA_Descricao nvarchar(254)
	,CVA_Quant_1   int
	,CVA_Quant_2   int
	,CVA_User_1    nvarchar(10)
	,CVA_User_2    nvarchar(10)
	,CVA_Data_1    LONGDATE
	,CVA_Hora_1    time
	,CVA_Data_2    LONGDATE
	,CVA_Hora_2    time
	,PRIMARY KEY (CVA_DocEntry)
	--,FOREIGN KEY (CVA_DocEntry) REFERENCES "@CVA_CONFERENCIA_LINHA"
	
);

ALTER TABLE "@CVA_CONFERENCIA_LINHA" ADD CONSTRAINT FK FOREIGN KEY (CVA_DocEntry) 
 REFERENCES "@CVA_CONFERENCIA"(CVA_DocEntry) ON DELETE CASCADE;
 
-- Volumes de Despacho
Create Table "@CVA_VOLEMB" 
(
	 CVA_DocEntry int Primary Key
	,CVA_DocDate  date
);

-- Volumes de Despacho Linhas
Create Table "@CVA_VOLEMB_LINHA" 
(
	 CVA_DocEntry  int 
	,CVA_LineNum   int
	,CVA_IdVolume  int
	,CVA_Qauntity  decimal(19,2)
	,CVA_Peso	   decimal(19,2) 
);

ALTER TABLE "@CVA_VOLEMB_LINHA" ADD CONSTRAINT FK_EMB_LINHA FOREIGN KEY (CVA_DocEntry) 
 REFERENCES "@CVA_VOLEMB"(CVA_DocEntry) ON DELETE CASCADE;


