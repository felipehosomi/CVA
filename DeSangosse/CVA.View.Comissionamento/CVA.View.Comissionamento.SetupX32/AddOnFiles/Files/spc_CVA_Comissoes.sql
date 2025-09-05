if object_id('dbo.spc_CVA_Comissoes', 'P') is not null
	drop procedure [spc_CVA_Comissoes]
go
 create procedure [dbo].[spc_CVA_Comissoes](@Status char(1) = 'T', @View char(1) = 'T', @DataInicial datetime, @DataFinal datetime)  
as  
begin  
 set dateformat 'ymd';  
 set nocount on;  
  
BEGIN -- CRIAÇÃO DE TABELAS  
CREATE TABLE #tbl_docs  
(  
 Momento CHAR(1) Collate Database_Default,  
 DocEntry INT,  
 DocDate DATETIME,  
 Serial INT,  
 DocTotal NUMERIC(19,6),  
 ObjType NVARCHAR(40) Collate Database_Default,  
 ItemCode NVARCHAR(100) Collate Database_Default,  
 LineNum INT,  
 Quantity NUMERIC(19,6),  
 LineTotal NUMERIC(19,6),  
 InstlmntID INT,  
 InsTotal NUMERIC(19,6),  
 DueDate DATETIME,  
 ItemName NVARCHAR(200) Collate Database_Default,  
 ItmsGrpCod SMALLINT,  
 ItmsGrpNam NVARCHAR(40) Collate Database_Default,  
 CardCode NVARCHAR(30) Collate Database_Default,  
 CardName NVARCHAR(200) Collate Database_Default,  
 PrcCode NVARCHAR(16) Collate Database_Default,  
 PrcName NVARCHAR(60) Collate Database_Default,  
 SlpCode INT,  
 SlpName NVARCHAR(310) Collate Database_Default,  
 U_CVA_IMPADIC CHAR(1) Collate Database_Default,  
 U_CVA_IMPINCL CHAR(1) Collate Database_Default,  
 FirmCode SMALLINT,  
 FirmName NVARCHAR(60) Collate Database_Default,  
 IndCode INT,  
 IndName NVARCHAR(30) Collate Database_Default,  
 IndDesc NVARCHAR(60) Collate Database_Default,  
 AbsId INT,  
 Name NVARCHAR(200) Collate Database_Default,  
 [State] NVARCHAR(6) Collate Database_Default,  
 TaxDate DATETIME,  
 SumApplied NUMERIC(19,6),  
 Tot_Invoices INT,  
 TaxSum_IMPADIC NUMERIC(19,6),  
 TaxSum_IMPINCL NUMERIC(19,6),  
 Tot_Parcelas INT,  
 Tot_Itens INT,  
 DocStatus CHAR(1) Collate Database_Default,  
 TotalItens NUMERIC(19,6),  
 LineTotal_Min_IMPINCL NUMERIC(19,6),  
 LineTotal_Min_IMPADIC NUMERIC(19,6),  
 LineTotal_Min_IMPADIC_Min_IMPINCL NUMERIC(19,6),  
 InsTotal_Min_IMPINCL NUMERIC(19,6),  
 InsTotal_Min_IMPADIC NUMERIC(19,6),  
 InsTotal_Min_IMPINCL_Min_IMPADIC NUMERIC(19,6)  ,
 IPI NUMERIC(19,6) 
)  
  
CREATE TABLE #tbl_docs_aux  
(  
 Momento CHAR(1) Collate Database_Default,  
 DocEntry INT,  
 DocDate DATETIME,  
 Serial INT,  
 DocTotal NUMERIC(19,6),  
 ObjType NVARCHAR(40) Collate Database_Default,  
 ItemCode NVARCHAR(100) Collate Database_Default,  
 LineNum INT,  
 Quantity NUMERIC(19,6),  
 LineTotal NUMERIC(19,6),  
 InstlmntID INT,  
 InsTotal NUMERIC(19,6),  
 DueDate DATETIME,  
 ItemName NVARCHAR(200) Collate Database_Default,  
 ItmsGrpCod SMALLINT,  
 ItmsGrpNam NVARCHAR(40) Collate Database_Default,  
 CardCode NVARCHAR(30) Collate Database_Default,  
 CardName NVARCHAR(200) Collate Database_Default,  
 PrcCode NVARCHAR(16) Collate Database_Default,  
 PrcName NVARCHAR(60) Collate Database_Default,  
 SlpCode INT,  
 SlpName NVARCHAR(310) Collate Database_Default,  
 U_CVA_IMPADIC CHAR(1) Collate Database_Default,  
 U_CVA_IMPINCL CHAR(1) Collate Database_Default,  
 FirmCode SMALLINT,  
 FirmName NVARCHAR(60) Collate Database_Default,  
 IndCode INT,  
 IndName NVARCHAR(30) Collate Database_Default,  
 IndDesc NVARCHAR(60) Collate Database_Default,  
 AbsId INT,  
 Name NVARCHAR(200) Collate Database_Default,  
 [State] NVARCHAR(6) Collate Database_Default,  
 TaxDate DATETIME,  
 SumApplied NUMERIC(19,6),  
 Tot_Invoices INT,  
 TaxSum_IMPADIC NUMERIC(19,6),  
 TaxSum_IMPINCL NUMERIC(19,6),  
 Tot_Parcelas INT,  
 Tot_Itens INT,  
 DocStatus CHAR(1) Collate Database_Default,  
 TotalItens NUMERIC(19,6),  
 LineTotal_Min_IMPINCL NUMERIC(19,6),  
 LineTotal_Min_IMPADIC NUMERIC(19,6),  
 LineTotal_Min_IMPADIC_Min_IMPINCL NUMERIC(19,6),  
 InsTotal_Min_IMPINCL NUMERIC(19,6),  
 InsTotal_Min_IMPADIC NUMERIC(19,6),  
 InsTotal_Min_IMPINCL_Min_IMPADIC NUMERIC(19,6)  
 ,IPI NUMERIC(19,6) 
)  
  
CREATE TABLE #tbl_docs_fat  
(  
 RegrCode INT,  
 Comissionado INT,  
 Comissao NUMERIC(19,6),  
 Prioridade INT,  
 IsItem CHAR(1),  
 Momento CHAR(1) Collate Database_Default,  
 DocEntry INT,  
 DocDate DATETIME,  
 Serial INT,  
 DocTotal NUMERIC(19,6),  
 ObjType NVARCHAR(40) Collate Database_Default,  
 ItemCode NVARCHAR(100) Collate Database_Default,  
 LineNum INT,  
 Quantity NUMERIC(19,6),  
 LineTotal NUMERIC(19,6),  
 InstlmntID INT,  
 InsTotal NUMERIC(19,6),  
 DueDate DATETIME,  
 ItemName NVARCHAR(200) Collate Database_Default,  
 ItmsGrpCod SMALLINT,  
 ItmsGrpNam NVARCHAR(40) Collate Database_Default,  
 CardCode NVARCHAR(30) Collate Database_Default,  
 CardName NVARCHAR(200) Collate Database_Default,  
 PrcCode NVARCHAR(16) Collate Database_Default,  
 PrcName NVARCHAR(60) Collate Database_Default,  
 SlpCode INT,  
 SlpName NVARCHAR(310) Collate Database_Default,  
 U_CVA_IMPADIC CHAR(1) Collate Database_Default,  
 U_CVA_IMPINCL CHAR(1) Collate Database_Default,  
 FirmCode SMALLINT,  
 FirmName NVARCHAR(60) Collate Database_Default,  
 IndCode INT,  
 IndName NVARCHAR(30) Collate Database_Default,  
 IndDesc NVARCHAR(60) Collate Database_Default,  
 AbsId INT,  
 Name NVARCHAR(200) Collate Database_Default,  
 [State] NVARCHAR(6) Collate Database_Default,  
 TaxDate DATETIME,  
 SumApplied NUMERIC(19,6),  
 Tot_Invoices INT,  
 TaxSum_IMPADIC NUMERIC(19,6),  
 TaxSum_IMPINCL NUMERIC(19,6),  
 Tot_Parcelas INT,  
 Tot_Itens INT,  
 DocStatus CHAR(1) Collate Database_Default,  
 TotalItens NUMERIC(19,6),  
 LineTotal_Min_IMPINCL NUMERIC(19,6),  
 LineTotal_Min_IMPADIC NUMERIC(19,6),  
 LineTotal_Min_IMPADIC_Min_IMPINCL NUMERIC(19,6),  
 InsTotal_Min_IMPINCL NUMERIC(19,6),  
 InsTotal_Min_IMPADIC NUMERIC(19,6),  
 InsTotal_Min_IMPINCL_Min_IMPADIC NUMERIC(19,6)  
 ,IPI NUMERIC(19,6) 
)  
  
CREATE TABLE #tmp_table_where  
(  
 RegrCode INT,  
 Comissionado INT,  
 Comissao NUMERIC(19,6),  
 Momento CHAR(1) Collate Database_Default,  
 Prioridade INT,  
 SqlWhere NVARCHAR(MAX) Collate Database_Default  
)  
  
create table #tmp_table_ret  
(  
 U_COMISSIONADO int,  
 U_PRIORIDADE int,  
 U_CARDCODE nvarchar(50) Collate Database_Default,  
 U_CARDNAME nvarchar(200) Collate Database_Default,  
 U_REGRA int,  
 U_DOCDATE datetime,  
 U_DUEDATE datetime,  
 U_DOCENTRY int,  
 U_OBJTYPE int,  
 U_ITEMCODE nvarchar(50) Collate Database_Default,  
 U_ITEMNAME nvarchar(200) Collate Database_Default,  
 U_LINENUM int,  
 U_VALOR numeric(19,6),  
 U_PARCELA int,  
 U_IMPOSTOS numeric(19,6),  
 U_COMISSAO numeric(19,6),  
 U_VALORCOMISSAO numeric(19,6),  
 U_CENTROCUSTO nvarchar(100) Collate Database_Default,  
 U_PAGO char(1) Collate Database_Default,  
 U_DATAPAGAMENTO datetime,  
 U_MOMENTO nvarchar(max)  
 ,IPI NUMERIC(19,6) 
)  
END  
  
begin --DECLARAÇÃO DE VARIÁVEIS  
 declare @cur as cursor  
  
 declare @CritCode int,  
   @CritName nvarchar(200),  
   @CritPos int,  
   @CritAtivo char(1),  
   @RegrCode int,  
   @RegrName nvarchar(200),  
   @Tipo int,  
   @Comissionado int,  
   @Momento char(1),  
   @Vendedor int,  
   @Item nvarchar(40),  
   @Grupo int,  
   @CentroCusto nvarchar(16),  
   @Fabricante int,  
   @Cliente nvarchar(30),  
   @Cidade int,  
   @Estado nvarchar(6),  
   @Setor int,  
   @Comissao numeric(19,6),  
   @IPI nvarchar(1),
   @Prioridade int,  
   @Query nvarchar(max),  
   @Params nvarchar(max),  
   @Where nvarchar(max)  
end  
  --print robson
  --print 'João 1'
INSERT INTO #tbl_docs  
SELECT DISTINCT  
 'F' AS Momento,  
 T0.DocEntry,  
 T0.DocDate,  
 T0.Serial,  
 T0.DocTotal,  
 T0.ObjType,  
 T2.ItemCode,  
 T2.LineNum,  
 T2.Quantity,  
 T2.LineTotal/T0.Installmnt AS LineTotal,  
 T3.InstlmntID,  
 T3.InsTotal/OA_MAX_ITEM.TotItem AS InsTotal,  
 T3.DueDate,  
 T4.ItemName,  
 T5.ItmsGrpCod,  
 T5.ItmsGrpNam,  
 T1.CardCode,  
 T1.CardName,  
 T7.PrcCode,  
 T7.PrcName,  
 T6.SlpCode,  
 T6.SlpName,  
 ISNULL(T6.U_CVA_IMPADC, 'N') AS U_CVA_IMPADIC,  
 ISNULL(T6.U_CVA_IMPINCL, 'N') AS U_CVA_IMPINCL,  
 T8.FirmCode,  
 T8.FirmName,  
 T9.IndCode,  
 T9.IndName,  
 T9.IndDesc,  
 T11.AbsId,  
 T11.Name,  
 T11.[State],  
 NULL AS TaxDate,  
 NULL AS SumApplied,  
 NULL AS Tot_Invoices,  
 T2.VatSum/T0.Installmnt AS TaxSum_IMPADIC,  
 OA_IMPINCL.TaxSum/T0.Installmnt AS TaxSum_IMPINCL,  
 T0.Installmnt AS Tot_Parcelas,  
 OA_MAX_ITEM.TotItem AS Tot_Itens,  
 NULL AS DocStatus,  
 OA_MAX_ITEM.TotalItens,  
 CASE WHEN OA_INC.Incluir = 3 THEN (T2.LineTotal/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPINCL,  
 CASE WHEN OA_INC.Incluir = 2 THEN (T2.LineTotal/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 1 THEN (T2.LineTotal/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPINCL_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 3 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem)-(OA_IMPINCL.TaxSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPINCL,  
 CASE WHEN OA_INC.Incluir = 2 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 1 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPINCL_Min_IMPADIC  
 ,OA_IMPIPI.TaxSum/T0.Installmnt
FROM OINV T0  
 INNER JOIN OCRD T1 ON T0.CardCode = T1.CardCode AND T1.CardType = 'C'  
 INNER JOIN INV1 T2 ON T0.DocEntry = T2.DocEntry AND ISNULL(T2.FreeChrgBP, 'N') = 'N'  
 INNER JOIN INV6 T3 ON T0.DocEntry = T3.DocEntry  
 INNER JOIN OITM T4 ON T2.ItemCode = T4.ItemCode  
 INNER JOIN OITB T5 ON T4.ItmsGrpCod = T5.ItmsGrpCod  
 LEFT  JOIN OSLP T6 ON ISNULL(T0.SlpCode, T1.SlpCode) = T6.SlpCode  
 LEFT  JOIN OPRC T7 ON T2.OcrCode = T7.PrcCode  
 LEFT  JOIN OMRC T8 ON T4.FirmCode = T8.FirmCode  
 LEFT  JOIN OOND T9 ON T1.IndustryC = T9.IndCode  
 LEFT  JOIN INV12 T10 ON T0.DocEntry = T10.DocEntry  
 LEFT  JOIN OCNT T11 ON T10.CountyB = T11.AbsId  
OUTER APPLY (  
 SELECT TotItem = COUNT(INV1.LineNum), TotalItens = SUM(INV1.LineTotal) FROM INV1 WHERE INV1.DocEntry = T0.DocEntry  
) AS OA_MAX_ITEM  
OUTER APPLY (  
 SELECT TaxSum = ISNULL(SUM(DISTINCT INV4.TaxSum), 0.00)  
 FROM INV4  
  INNER JOIN OSTA ON INV4.StaCode = OSTA.Code  
  INNER JOIN OSTT ON OSTA.[Type] = OSTT.AbsId  
  INNER JOIN ONFT ON OSTT.NfTaxId = ONFT.AbsId  
 WHERE INV4.TaxInPrice = 'Y' AND INV4.DocEntry = T2.DocEntry AND INV4.LineNum = T2.LineNum  
) AS OA_IMPINCL  
OUTER APPLY (  
 SELECT TaxSum = ISNULL(SUM(DISTINCT INV4.TaxSum), 0.00)  
 FROM INV4  
  --INNER JOIN OSTA ON INV4.StaCode = OSTA.Code  
  --INNER JOIN OSTT ON OSTA.[Type] = OSTT.AbsId  
  --INNER JOIN ONFT ON OSTT.NfTaxId = ONFT.AbsId  
 WHERE INV4.StaCode like 'IP%' AND INV4.DocEntry = T2.DocEntry AND INV4.LineNum = T2.LineNum  
) AS OA_IMPIPI
OUTER APPLY (  
 SELECT Incluir = CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN 1  
  ELSE CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN 2  
  ELSE CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN 3  
  ELSE 0 END END END  
) AS OA_INC  
WHERE T0.CANCELED = 'N' AND T3.InsTotal <> 0 AND T0.DocDate >= @DataInicial AND T0.DocDate <= @DataFinal  
  --print 'João 2'
INSERT INTO #tbl_docs  
SELECT DISTINCT  
 'P' AS Momento,  
 T0.DocEntry,  
 T0.DocDate,  
 T0.DocEntry,  
 T0.DocTotal,  
 T0.ObjType,  
 T2.ItemCode,  
 T2.LineNum,  
 T2.Quantity,  
 T2.LineTotal/T0.Installmnt AS LineTotal,  
 T3.InstlmntID,  
 T3.InsTotal/OA_MAX_ITEM.TotItem AS InsTotal,  
 T3.DueDate,  
 T4.ItemName,  
 T5.ItmsGrpCod,  
 T5.ItmsGrpNam,  
 T1.CardCode,  
 T1.CardName,  
 T7.PrcCode,  
 T7.PrcName,  
 T6.SlpCode,  
 T6.SlpName,  
 ISNULL(T6.U_CVA_IMPADC, 'N') AS U_CVA_IMPADIC,  
 ISNULL(T6.U_CVA_IMPINCL, 'N') AS U_CVA_IMPINCL,  
 T8.FirmCode,  
 T8.FirmName,  
 T9.IndCode,  
 T9.IndName,  
 T9.IndDesc,  
 T11.AbsId,  
 T11.Name,  
 T11.[State],  
 NULL AS TaxDate,  
 NULL AS SumApplied,  
 NULL AS Tot_Invoices,  
 T2.VatSum/T0.Installmnt AS TaxSum_IMPADIC,  
 OA_IMPINCL.TaxSum/T0.Installmnt AS TaxSum_IMPINCL,  
 T0.Installmnt AS Tot_Parcelas,  
 OA_MAX_ITEM.TotItem AS Tot_Itens,  
 NULL AS DocStatus,  
 OA_MAX_ITEM.TotalItens,  
 CASE WHEN OA_INC.Incluir = 3 THEN (T2.LineTotal/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPINCL,  
 CASE WHEN OA_INC.Incluir = 2 THEN (T2.LineTotal/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 1 THEN (T2.LineTotal/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPINCL_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 3 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem)-(OA_IMPINCL.TaxSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPINCL,  
 CASE WHEN OA_INC.Incluir = 2 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 1 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPINCL_Min_IMPADIC  
 ,OA_IMPIPI.TaxSum/T0.Installmnt
FROM ORDR T0  
 INNER JOIN OCRD T1 ON T0.CardCode = T1.CardCode AND T1.CardType = 'C'  
 INNER JOIN RDR1 T2 ON T0.DocEntry = T2.DocEntry AND ISNULL(T2.FreeChrgBP, 'N') = 'N'  
 INNER JOIN RDR6 T3 ON T0.DocEntry = T3.DocEntry  
 INNER JOIN OITM T4 ON T2.ItemCode = T4.ItemCode  
 INNER JOIN OITB T5 ON T4.ItmsGrpCod = T5.ItmsGrpCod  
 LEFT  JOIN OSLP T6 ON ISNULL(T0.SlpCode, T1.SlpCode) = T6.SlpCode  
 LEFT  JOIN OPRC T7 ON T2.OcrCode = T7.PrcCode  
 LEFT  JOIN OMRC T8 ON T4.FirmCode = T8.FirmCode  
 LEFT  JOIN OOND T9 ON T1.IndustryC = T9.IndCode  
 LEFT  JOIN RDR12 T10 ON T0.DocEntry = T10.DocEntry  
 LEFT  JOIN OCNT T11 ON T10.CountyB = T11.AbsId  
OUTER APPLY (  
 SELECT TotItem = COUNT(RDR1.LineNum), TotalItens = SUM(RDR1.LineTotal) FROM RDR1 WHERE RDR1.DocEntry = T0.DocEntry  
) AS OA_MAX_ITEM  
OUTER APPLY (  
 SELECT TaxSum = ISNULL(SUM(DISTINCT RDR4.TaxSum), 0.00)  
 FROM RDR4  
  INNER JOIN OSTA ON RDR4.StaCode = OSTA.Code  
  INNER JOIN OSTT ON OSTA.[Type] = OSTT.AbsId  
  INNER JOIN ONFT ON OSTT.NfTaxId = ONFT.AbsId  
 WHERE RDR4.TaxInPrice = 'Y' AND RDR4.DocEntry = T2.DocEntry AND RDR4.LineNum = T2.LineNum  
) AS OA_IMPINCL  
OUTER APPLY (  
 SELECT TaxSum = ISNULL(SUM(DISTINCT RDR4.TaxSum), 0.00)  
 FROM RDR4  
  --INNER JOIN OSTA ON RDR4.StaCode = OSTA.Code  
  --INNER JOIN OSTT ON OSTA.[Type] = OSTT.AbsId  
  --INNER JOIN ONFT ON OSTT.NfTaxId = ONFT.AbsId  
 WHERE RDR4.StaCode like 'IP%' AND RDR4.DocEntry = T2.DocEntry AND RDR4.LineNum = T2.LineNum  
) AS OA_IMPIPI
OUTER APPLY (  
 SELECT Incluir = CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN 1  
  ELSE CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN 2  
  ELSE CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN 3  
  ELSE 0 END END END  
) AS OA_INC  
WHERE T0.CANCELED = 'N' AND T3.InsTotal <> 0 AND T0.DocDate >= @DataInicial AND T0.DocDate <= @DataFinal  
  --print 'João 3'
INSERT INTO #tbl_docs  
SELECT DISTINCT  
 'F' AS Momento,  
 T0.DocEntry,  
 T0.DocDate,  
 T0.DocEntry,  
 T0.DocTotal,  
 T0.ObjType,  
 T2.ItemCode,  
 T2.LineNum,  
 T2.Quantity,  
 T2.LineTotal/T0.Installmnt AS LineTotal,  
 T3.InstlmntID,  
 T3.InsTotal/OA_MAX_ITEM.TotItem AS InsTotal,  
 T3.DueDate,  
 T4.ItemName,  
 T5.ItmsGrpCod,  
 T5.ItmsGrpNam,  
 T1.CardCode,  
 T1.CardName,  
 T7.PrcCode,  
 T7.PrcName,  
 T6.SlpCode,  
 T6.SlpName,  
 ISNULL(T6.U_CVA_IMPADC, 'N') AS U_CVA_IMPADIC,  
 ISNULL(T6.U_CVA_IMPINCL, 'N') AS U_CVA_IMPINCL,  
 T8.FirmCode,  
 T8.FirmName,  
 T9.IndCode,  
 T9.IndName,  
 T9.IndDesc,  
 T11.AbsId,  
 T11.Name,  
 T11.[State],  
 NULL AS TaxDate,  
 NULL AS SumApplied,  
 NULL AS Tot_Invoices,  
 T2.VatSum/T0.Installmnt AS TaxSum_IMPADIC,  
 OA_IMPINCL.TaxSum/T0.Installmnt AS TaxSum_IMPINCL,  
 T0.Installmnt AS Tot_Parcelas,  
 OA_MAX_ITEM.TotItem AS Tot_Itens,  
 NULL AS DocStatus,  
 OA_MAX_ITEM.TotalItens,  
 CASE WHEN OA_INC.Incluir = 3 THEN (T2.LineTotal/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPINCL,  
 CASE WHEN OA_INC.Incluir = 2 THEN (T2.LineTotal/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 1 THEN (T2.LineTotal/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPINCL_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 3 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem)-(OA_IMPINCL.TaxSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPINCL,  
 CASE WHEN OA_INC.Incluir = 2 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 1 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPINCL_Min_IMPADIC  
 ,OA_IMPIPI.TaxSum/T0.Installmnt
FROM ODPI T0  
 INNER JOIN OCRD T1 ON T0.CardCode = T1.CardCode AND T1.CardType = 'C'  
 INNER JOIN DPI1 T2 ON T0.DocEntry = T2.DocEntry AND ISNULL(T2.FreeChrgBP, 'N') = 'N'  
 INNER JOIN DPI6 T3 ON T0.DocEntry = T3.DocEntry  
 INNER JOIN OITM T4 ON T2.ItemCode = T4.ItemCode  
 INNER JOIN OITB T5 ON T4.ItmsGrpCod = T5.ItmsGrpCod  
 LEFT  JOIN OSLP T6 ON ISNULL(T0.SlpCode, T1.SlpCode) = T6.SlpCode  
 LEFT  JOIN OPRC T7 ON T2.OcrCode = T7.PrcCode  
 LEFT  JOIN OMRC T8 ON T4.FirmCode = T8.FirmCode  
 LEFT  JOIN OOND T9 ON T1.IndustryC = T9.IndCode  
 LEFT  JOIN DPI12 T10 ON T0.DocEntry = T10.DocEntry  
 LEFT  JOIN OCNT T11 ON T10.CountyB = T11.AbsId  
OUTER APPLY (  
 SELECT TotItem = COUNT(DPI1.LineNum), TotalItens = SUM(DPI1.LineTotal) FROM DPI1 WHERE DPI1.DocEntry = T0.DocEntry  
) AS OA_MAX_ITEM  
OUTER APPLY (  
 SELECT TaxSum = ISNULL(SUM(DISTINCT DPI4.TaxSum), 0.00)  
 FROM DPI4  
  INNER JOIN OSTA ON DPI4.StaCode = OSTA.Code  
  INNER JOIN OSTT ON OSTA.[Type] = OSTT.AbsId  
  INNER JOIN ONFT ON OSTT.NfTaxId = ONFT.AbsId  
 WHERE DPI4.TaxInPrice = 'Y' AND DPI4.DocEntry = T2.DocEntry AND DPI4.LineNum = T2.LineNum  
) AS OA_IMPINCL  
OUTER APPLY (  
 SELECT TaxSum = ISNULL(SUM(DISTINCT DPI4.TaxSum), 0.00)  
 FROM DPI4  
  --INNER JOIN OSTA ON RDR4.StaCode = OSTA.Code  
  --INNER JOIN OSTT ON OSTA.[Type] = OSTT.AbsId  
  --INNER JOIN ONFT ON OSTT.NfTaxId = ONFT.AbsId  
 WHERE DPI4.StaCode like 'IP%' AND DPI4.DocEntry = T2.DocEntry AND DPI4.LineNum = T2.LineNum  
) AS OA_IMPIPI
OUTER APPLY (  
 SELECT Incluir = CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN 1  
  ELSE CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN 2  
  ELSE CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN 3  
  ELSE 0 END END END  
) AS OA_INC  
WHERE T0.CANCELED = 'N' AND T3.InsTotal <> 0 AND T0.DocDate >= @DataInicial AND T0.DocDate <= @DataFinal    
--print 'João 4'

INSERT INTO #tbl_docs  
SELECT DISTINCT  
 'F' AS Momento,  
 T0.DocEntry,  
 T0.DocDate,  
 T0.Serial,  
 -T0.DocTotal,  
 T0.ObjType,  
 T2.ItemCode,  
 T2.LineNum,  
 -T2.Quantity,  
 -(T2.LineTotal/T0.Installmnt) AS LineTotal,  
 T3.InstlmntID,  
 -(T3.InsTotal/OA_MAX_ITEM.TotItem) AS InsTotal,  
 T3.DueDate,  
 T4.ItemName,  
 T5.ItmsGrpCod,  
 T5.ItmsGrpNam,  
 T1.CardCode,  
 T1.CardName,  
 T7.PrcCode,  
 T7.PrcName,  
 T6.SlpCode,  
 T6.SlpName,  
 ISNULL(T6.U_CVA_IMPADC, 'N') AS U_CVA_IMPADIC,  
 ISNULL(T6.U_CVA_IMPINCL, 'N') AS U_CVA_IMPINCL,  
 T8.FirmCode,  
 T8.FirmName,  
 T9.IndCode,  
 T9.IndName,  
 T9.IndDesc,  
 T11.AbsId,  
 T11.Name,  
 T11.[State],  
 NULL AS TaxDate,  
 NULL AS SumApplied,  
 NULL AS Tot_Invoices,  
 -(T2.VatSum/T0.Installmnt) AS TaxSum_IMPADIC,  
 -(OA_IMPINCL.TaxSum/T0.Installmnt) AS TaxSum_IMPINCL,  
 T0.Installmnt AS Tot_Parcelas,  
 OA_MAX_ITEM.TotItem AS Tot_Itens,  
 'C' AS DocStatus,  
 -OA_MAX_ITEM.TotalItens,  
 CASE WHEN OA_INC.Incluir = 3 THEN -((T2.LineTotal/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt)) ELSE 0 END AS LineTotal_Min_IMPINCL,  
 CASE WHEN OA_INC.Incluir = 2 THEN -((T2.LineTotal/T0.Installmnt)-(T2.VatSum/T0.Installmnt)) ELSE 0 END AS LineTotal_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 1 THEN -((T2.LineTotal/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt)-(T2.VatSum/T0.Installmnt)) ELSE 0 END AS LineTotal_Min_IMPINCL_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 3 THEN -((T3.InsTotal/OA_MAX_ITEM.TotItem)-(OA_IMPINCL.TaxSum/T0.Installmnt)) ELSE 0 END AS InsTotal_Min_IMPINCL,  
 CASE WHEN OA_INC.Incluir = 2 THEN -((T3.InsTotal/OA_MAX_ITEM.TotItem)-(T2.VatSum/T0.Installmnt)) ELSE 0 END AS InsTotal_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 1 THEN -((T3.InsTotal/OA_MAX_ITEM.TotItem/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt)-(T2.VatSum/T0.Installmnt)) ELSE 0 END AS InsTotal_Min_IMPINCL_Min_IMPADIC  
 ,OA_IMPIPI.TaxSum/T0.Installmnt
FROM ORIN T0  
 INNER JOIN OCRD T1 ON T0.CardCode = T1.CardCode AND T1.CardType = 'C'  
 INNER JOIN RIN1 T2 ON T0.DocEntry = T2.DocEntry AND ISNULL(T2.FreeChrgBP, 'N') = 'N'  
 INNER JOIN RIN6 T3 ON T0.DocEntry = T3.DocEntry  
 INNER JOIN OITM T4 ON T2.ItemCode = T4.ItemCode  
 INNER JOIN OITB T5 ON T4.ItmsGrpCod = T5.ItmsGrpCod  
 LEFT  JOIN OSLP T6 ON ISNULL(T0.SlpCode, T1.SlpCode) = T6.SlpCode  
 LEFT  JOIN OPRC T7 ON T2.OcrCode = T7.PrcCode  
 LEFT  JOIN OMRC T8 ON T4.FirmCode = T8.FirmCode  
 LEFT  JOIN OOND T9 ON T1.IndustryC = T9.IndCode  
 LEFT  JOIN RIN12 T10 ON T0.DocEntry = T10.DocEntry  
 LEFT  JOIN OCNT T11 ON T10.CountyB = T11.AbsId  
OUTER APPLY (  
 SELECT TotItem = COUNT(RIN1.LineNum), TotalItens = SUM(RIN1.LineTotal) FROM RIN1 WHERE RIN1.DocEntry = T0.DocEntry  
) AS OA_MAX_ITEM  
OUTER APPLY (  
 SELECT TaxSum = ISNULL(SUM(DISTINCT RIN4.TaxSum), 0.00)  
 FROM RIN4  
  INNER JOIN OSTA ON RIN4.StaCode = OSTA.Code  
  INNER JOIN OSTT ON OSTA.[Type] = OSTT.AbsId  
  INNER JOIN ONFT ON OSTT.NfTaxId = ONFT.AbsId  
 WHERE RIN4.TaxInPrice = 'Y' AND RIN4.DocEntry = T2.DocEntry AND RIN4.LineNum = T2.LineNum  
) AS OA_IMPINCL  
OUTER APPLY (  
 SELECT TaxSum = ISNULL(SUM(DISTINCT RIN4.TaxSum), 0.00)  
 FROM RIN4  
  --INNER JOIN OSTA ON RDR4.StaCode = OSTA.Code  
  --INNER JOIN OSTT ON OSTA.[Type] = OSTT.AbsId  
  --INNER JOIN ONFT ON OSTT.NfTaxId = ONFT.AbsId  
 WHERE RIN4.StaCode like 'IP%' AND RIN4.DocEntry = T2.DocEntry AND RIN4.LineNum = T2.LineNum  
) AS OA_IMPIPI
OUTER APPLY (  
 SELECT Incluir = CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN 1  
  ELSE CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN 2  
  ELSE CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN 3  
  ELSE 0 END END END  
) AS OA_INC  
WHERE T0.CANCELED = 'N' AND T3.InsTotal <> 0 AND T0.DocDate >= @DataInicial AND T0.DocDate <= @DataFinal  
  
  --print 'João 4'

INSERT INTO #tbl_docs  
SELECT DISTINCT  
 'R' AS Momento,  
 T0.DocEntry,  
 T0.DocDate,  
 T0.Serial,  
 T0.DocTotal,  
 T0.ObjType,  
 T2.ItemCode,  
 T2.LineNum,  
 T2.Quantity,  
 T2.LineTotal/T0.Installmnt AS LineTotal,  
 T3.InstlmntID,  
 T3.InsTotal/OA_MAX_ITEM.TotItem AS InsTotal,  
 T3.DueDate,  
 T4.ItemName,  
 T5.ItmsGrpCod,  
 T5.ItmsGrpNam,  
 T1.CardCode,  
 T1.CardName,  
 T7.PrcCode,  
 T7.PrcName,  
 T6.SlpCode,  
 T6.SlpName,  
 ISNULL(T6.U_CVA_IMPADC, 'N') AS U_CVA_IMPADIC,  
 ISNULL(T6.U_CVA_IMPINCL, 'N') AS U_CVA_IMPINCL,  
 T8.FirmCode,  
 T8.FirmName,  
 T9.IndCode,  
 T9.IndName,  
 T9.IndDesc,  
 T11.AbsId,  
 T11.Name,  
 T11.[State],  
 OA_DATA.DataPagamento AS TaxDate,  
 T12.SumApplied AS SumApplied,  
 OA_MAX.Countt AS Tot_Invoices,  
 T2.VatSum/T0.Installmnt AS TaxSum_IMPADIC,  
 OA_IMPINCL.TaxSum/T0.Installmnt AS TaxSum_IMPINCL,  
 T0.Installmnt AS Tot_Parcelas,  
 OA_MAX_ITEM.TotItem AS Tot_Itens,  
 CASE WHEN OA_DATA.DataPagamento IS NOT NULL THEN 'P' ELSE NULL END AS DocStatus,  
 OA_MAX_ITEM.TotalItens,  
 CASE WHEN OA_INC.Incluir = 3 THEN (T2.LineTotal/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPINCL,  
 CASE WHEN OA_INC.Incluir = 2 THEN (T2.LineTotal/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 1 THEN (T2.LineTotal/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPINCL_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 3 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem)-(OA_IMPINCL.TaxSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPINCL,  
 CASE WHEN OA_INC.Incluir = 2 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 1 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPINCL_Min_IMPADIC  
 ,OA_IMPIPI.TaxSum/T0.Installmnt
FROM OINV T0  
 INNER JOIN OCRD T1 ON T0.CardCode = T1.CardCode AND T1.CardType = 'C'  
 INNER JOIN INV1 T2 ON T0.DocEntry = T2.DocEntry AND ISNULL(T2.FreeChrgBP, 'N') = 'N'  
 INNER JOIN INV6 T3 ON T0.DocEntry = T3.DocEntry  
 INNER JOIN OITM T4 ON T2.ItemCode = T4.ItemCode  
 INNER JOIN OITB T5 ON T4.ItmsGrpCod = T5.ItmsGrpCod  
 LEFT  JOIN OSLP T6 ON ISNULL(T0.SlpCode, T1.SlpCode) = T6.SlpCode  
 LEFT  JOIN OPRC T7 ON T2.OcrCode = T7.PrcCode  
 LEFT  JOIN OMRC T8 ON T4.FirmCode = T8.FirmCode  
 LEFT  JOIN OOND T9 ON T1.IndustryC = T9.IndCode  
 LEFT  JOIN INV12 T10 ON T0.DocEntry = T10.DocEntry  
 LEFT  JOIN OCNT T11 ON T10.CountyB = T11.AbsId  
 INNER JOIN RCT2 T12 ON T3.DocEntry = T12.DocEntry AND T3.InstlmntID = T12.InstId AND T3.ObjType = T12.InvType  
 INNER JOIN ORCT T13 ON T12.DocEntry = T13.DocEntry AND T13.Canceled = 'N'  
 LEFT  JOIN OBOE T14 ON T13.BoeAbs = T14.BoeKey  
 LEFT  JOIN OBOT T15 ON T15.AbsEntry =   
 (  
  SELECT TOP 1 BOT1.AbsEntry FROM BOT1  
  WHERE BOT1.BOENumber = T14.BoeNum AND BOT1.BoeType = T14.BoeType  
  ORDER BY BOT1.AbsEntry DESC  
 ) AND T15.StatusTo = 'P'  
 LEFT  JOIN OJDT T16 ON T15.TransId = T16.Number  
 LEFT  JOIN RCT1 T17 ON T13.DocEntry = T17.DocNum  
 LEFT  JOIN OCHO T18 ON T17.CheckAbs = T18.CheckKey  
OUTER APPLY (  
 SELECT Countt = COUNT(RCT2.InvoiceId) FROM RCT2 WHERE RCT2.DocNum = T13.DocEntry  
) AS OA_MAX  
OUTER APPLY (  
 SELECT DataPagamento =   
  CASE WHEN T15.StatusTo = 'P' THEN T16.RefDate  
  ELSE CASE WHEN T18.CheckKey IS NOT NULL THEN T18.CheckDate  
  ELSE CASE WHEN T13.BoeAbs IS NULL AND T13.DocEntry IS NOT NULL THEN T13.TaxDate  
  ELSE NULL END END END  
) AS OA_DATA  
OUTER APPLY (  
 SELECT TotItem = COUNT(INV1.LineNum), TotalItens = SUM(INV1.LineTotal) FROM INV1 WHERE INV1.DocEntry = T0.DocEntry  
) AS OA_MAX_ITEM  
OUTER APPLY (  
 SELECT TaxSum = ISNULL(SUM(DISTINCT INV4.TaxSum), 0.00)  
 FROM INV4  
  INNER JOIN OSTA ON INV4.StaCode = OSTA.Code  
  INNER JOIN OSTT ON OSTA.[Type] = OSTT.AbsId  
  INNER JOIN ONFT ON OSTT.NfTaxId = ONFT.AbsId  
 WHERE INV4.TaxInPrice = 'Y' AND INV4.DocEntry = T2.DocEntry AND INV4.LineNum = T2.LineNum  
) AS OA_IMPINCL  
OUTER APPLY (  
 SELECT TaxSum = ISNULL(SUM(DISTINCT INV4.TaxSum), 0.00)  
 FROM INV4  
  --INNER JOIN OSTA ON RDR4.StaCode = OSTA.Code  
  --INNER JOIN OSTT ON OSTA.[Type] = OSTT.AbsId  
  --INNER JOIN ONFT ON OSTT.NfTaxId = ONFT.AbsId  
 WHERE INV4.StaCode like 'IP%' AND INV4.DocEntry = T2.DocEntry AND INV4.LineNum = T2.LineNum  
) AS OA_IMPIPI
OUTER APPLY (  
 SELECT Incluir = CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN 1  
  ELSE CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN 2  
  ELSE CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN 3  
  ELSE 0 END END END  
) AS OA_INC  
INNER  JOIN OACT T19 ON T19.AcctCode = (CASE WHEN T13.CashSum > 0 THEN 
												T13.CASHACCT    
											 WHEN T13.TrsfrSUM > 0 THEN 
												T13.TrsfrAcct
											 WHEN T13.Boesum > 0 THEN 
												T13.BoeAcc
											WHEN T13.[CheckSum] > 0 THEN 
												T13.CheckAcct
											END) AND T19.Finanse <> 'Y'
WHERE T0.CANCELED = 'N' AND T3.InsTotal <> 0 AND OA_DATA.DataPagamento >= @DataInicial AND OA_DATA.DataPagamento <= @DataFinal  
  
  --print 'João 5'
INSERT INTO #tbl_docs  
SELECT DISTINCT  
 'R' AS Momento,  
 T0.DocEntry,  
 T0.DocDate,  
 T0.Serial,  
 T0.DocTotal,  
 T0.ObjType,  
 T2.ItemCode,  
 T2.LineNum,  
 T2.Quantity,  
 T2.LineTotal/T0.Installmnt AS LineTotal,  
 T3.InstlmntID,  
 T3.InsTotal/OA_MAX_ITEM.TotItem AS InsTotal,  
 T3.DueDate,  
 T4.ItemName,  
 T5.ItmsGrpCod,  
 T5.ItmsGrpNam,  
 T1.CardCode,  
 T1.CardName,  
 T7.PrcCode,  
 T7.PrcName,  
 T6.SlpCode,  
 T6.SlpName,  
 ISNULL(T6.U_CVA_IMPADC, 'N') AS U_CVA_IMPADIC,  
 ISNULL(T6.U_CVA_IMPINCL, 'N') AS U_CVA_IMPINCL,  
 T8.FirmCode,  
 T8.FirmName,  
 T9.IndCode,  
 T9.IndName,  
 T9.IndDesc,  
 T11.AbsId,  
 T11.Name,  
 T11.[State],  
 OA_DATA.DataPagamento AS TaxDate,  
 T12.SumApplied AS SumApplied,  
 OA_MAX.Countt AS Tot_Invoices,  
 T2.VatSum/T0.Installmnt AS TaxSum_IMPADIC,  
 OA_IMPINCL.TaxSum/T0.Installmnt AS TaxSum_IMPINCL,  
 T0.Installmnt AS Tot_Parcelas,  
 OA_MAX_ITEM.TotItem AS Tot_Itens,  
 CASE WHEN OA_DATA.DataPagamento IS NOT NULL THEN 'P' ELSE NULL END AS DocStatus,  
 OA_MAX_ITEM.TotalItens,  
 CASE WHEN OA_INC.Incluir = 3 THEN (T2.LineTotal/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPINCL,  
 CASE WHEN OA_INC.Incluir = 2 THEN (T2.LineTotal/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 1 THEN (T2.LineTotal/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPINCL_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 3 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem)-(OA_IMPINCL.TaxSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPINCL,  
 CASE WHEN OA_INC.Incluir = 2 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 1 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPINCL_Min_IMPADIC  
 ,OA_IMPIPI.TaxSum/T0.Installmnt
FROM ODPI T0  
 INNER JOIN OCRD T1 ON T0.CardCode = T1.CardCode AND T1.CardType = 'C'  
 INNER JOIN DPI1 T2 ON T0.DocEntry = T2.DocEntry AND ISNULL(T2.FreeChrgBP, 'N') = 'N'  
 INNER JOIN DPI6 T3 ON T0.DocEntry = T3.DocEntry  
 INNER JOIN OITM T4 ON T2.ItemCode = T4.ItemCode  
 INNER JOIN OITB T5 ON T4.ItmsGrpCod = T5.ItmsGrpCod  
 LEFT  JOIN OSLP T6 ON ISNULL(T0.SlpCode, T1.SlpCode) = T6.SlpCode  
 LEFT  JOIN OPRC T7 ON T2.OcrCode = T7.PrcCode  
 LEFT  JOIN OMRC T8 ON T4.FirmCode = T8.FirmCode  
 LEFT  JOIN OOND T9 ON T1.IndustryC = T9.IndCode  
 LEFT  JOIN DPI12 T10 ON T0.DocEntry = T10.DocEntry  
 LEFT  JOIN OCNT T11 ON T10.CountyB = T11.AbsId  
 INNER JOIN RCT2 T12 ON T3.DocEntry = T12.DocEntry AND T3.InstlmntID = T12.InstId AND T3.ObjType = T12.InvType  
 INNER JOIN ORCT T13 ON T12.DocEntry = T13.DocEntry AND T13.Canceled = 'N'  
 LEFT  JOIN OBOE T14 ON T13.BoeAbs = T14.BoeKey  
 LEFT  JOIN OBOT T15 ON T15.AbsEntry =   
 (  
  SELECT TOP 1 BOT1.AbsEntry FROM BOT1  
  WHERE BOT1.BOENumber = T14.BoeNum AND BOT1.BoeType = T14.BoeType  
  ORDER BY BOT1.AbsEntry DESC  
 ) AND T15.StatusTo = 'P'  
 LEFT  JOIN OJDT T16 ON T15.TransId = T16.Number  
 LEFT  JOIN RCT1 T17 ON T13.DocEntry = T17.DocNum  
 LEFT  JOIN OCHO T18 ON T17.CheckAbs = T18.CheckKey  
OUTER APPLY (  
 SELECT Countt = COUNT(RCT2.InvoiceId) FROM RCT2 WHERE RCT2.DocNum = T13.DocEntry  
) AS OA_MAX  
OUTER APPLY (  
 SELECT DataPagamento =   
  CASE WHEN T15.StatusTo = 'P' THEN T16.RefDate  
  ELSE CASE WHEN T18.CheckKey IS NOT NULL THEN T18.CheckDate  
  ELSE CASE WHEN T13.BoeAbs IS NULL AND T13.DocEntry IS NOT NULL THEN T13.TaxDate  
  ELSE NULL END END END  
) AS OA_DATA  
OUTER APPLY (  
 SELECT TotItem = COUNT(DPI1.LineNum), TotalItens = SUM(DPI1.LineTotal) FROM DPI1 WHERE DPI1.DocEntry = T0.DocEntry  
) AS OA_MAX_ITEM  
OUTER APPLY (  
 SELECT TaxSum = ISNULL(SUM(DISTINCT DPI4.TaxSum), 0.00)  
 FROM DPI4  
  INNER JOIN OSTA ON DPI4.StaCode = OSTA.Code  
  INNER JOIN OSTT ON OSTA.[Type] = OSTT.AbsId  
  INNER JOIN ONFT ON OSTT.NfTaxId = ONFT.AbsId  
 WHERE DPI4.TaxInPrice = 'Y' AND DPI4.DocEntry = T2.DocEntry AND DPI4.LineNum = T2.LineNum  ) AS OA_IMPINCL  
OUTER APPLY (  
 SELECT TaxSum = ISNULL(SUM(DISTINCT DPI4.TaxSum), 0.00)  
 FROM DPI4  
  --INNER JOIN OSTA ON RDR4.StaCode = OSTA.Code  
  --INNER JOIN OSTT ON OSTA.[Type] = OSTT.AbsId  
  --INNER JOIN ONFT ON OSTT.NfTaxId = ONFT.AbsId  
 WHERE DPI4.StaCode like 'IP%' AND DPI4.DocEntry = T2.DocEntry AND DPI4.LineNum = T2.LineNum  
) AS OA_IMPIPI

OUTER APPLY (  
 SELECT Incluir = CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN 1  
  ELSE CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN 2  
  ELSE CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN 3  
  ELSE 0 END END END  
) AS OA_INC  
INNER  JOIN OACT T19 ON T19.AcctCode = (CASE WHEN T13.CashSum > 0 THEN 
												T13.CASHACCT    
											 WHEN T13.TrsfrSUM > 0 THEN 
												T13.TrsfrAcct
											 WHEN T13.Boesum > 0 THEN 
												T13.BoeAcc
											WHEN T13.[CheckSum] > 0 THEN 
												T13.CheckAcct
											END) AND T19.Finanse <> 'Y'
WHERE T0.CANCELED = 'N' AND T3.InsTotal <> 0 AND OA_DATA.DataPagamento >= @DataInicial AND OA_DATA.DataPagamento <= @DataFinal  
  --print 'João 7'

INSERT INTO #tbl_docs  
SELECT DISTINCT  
 'R' AS Momento,  
 T0.DocEntry,  
 T0.DocDate,  
 T0.Serial,  
 T0.DocTotal,  
 T0.ObjType,  
 T2.ItemCode,  
 T2.LineNum,  
 T2.Quantity,  
 T2.LineTotal/T0.Installmnt AS LineTotal,  
 T3.InstlmntID,  
 T3.InsTotal/OA_MAX_ITEM.TotItem AS InsTotal,  
 T3.DueDate,  
 T4.ItemName,  
 T5.ItmsGrpCod,  
 T5.ItmsGrpNam,  
 T1.CardCode,  
 T1.CardName,  
 T7.PrcCode,  
 T7.PrcName,  
 T6.SlpCode,  
 T6.SlpName,  
 ISNULL(T6.U_CVA_IMPADC, 'N') AS U_CVA_IMPADIC,  
 ISNULL(T6.U_CVA_IMPINCL, 'N') AS U_CVA_IMPINCL,  
 T8.FirmCode,  
 T8.FirmName,  
 T9.IndCode,  
 T9.IndName,  
 T9.IndDesc,  
 T11.AbsId,  
 T11.Name,  
 T11.[State],  
 OA_DATA.DataPagamento AS TaxDate,  
 T12.SumApplied AS SumApplied,  
 OA_MAX.Countt AS Tot_Invoices,  
 T2.VatSum/T0.Installmnt AS TaxSum_IMPADIC,  
 OA_IMPINCL.TaxSum/T0.Installmnt AS TaxSum_IMPINCL,  
 T0.Installmnt AS Tot_Parcelas,  
 OA_MAX_ITEM.TotItem AS Tot_Itens,  
 CASE WHEN OA_DATA.DataPagamento IS NOT NULL THEN 'P' ELSE NULL END AS DocStatus,  
 OA_MAX_ITEM.TotalItens,  
 CASE WHEN OA_INC.Incluir = 3 THEN (T2.LineTotal/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPINCL,  
 CASE WHEN OA_INC.Incluir = 2 THEN (T2.LineTotal/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 1 THEN (T2.LineTotal/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS LineTotal_Min_IMPINCL_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 3 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem)-(OA_IMPINCL.TaxSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPINCL,  
 CASE WHEN OA_INC.Incluir = 2 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPADIC,  
 CASE WHEN OA_INC.Incluir = 1 THEN (T3.InsTotal/OA_MAX_ITEM.TotItem/T0.Installmnt)-(OA_IMPINCL.TaxSum/T0.Installmnt)-(T2.VatSum/T0.Installmnt) ELSE 0 END AS InsTotal_Min_IMPINCL_Min_IMPADIC  
 ,OA_IMPIPI.TaxSum/T0.Installmnt
FROM ORIN T0  
 INNER JOIN OCRD T1 ON T0.CardCode = T1.CardCode AND T1.CardType = 'C'  
 INNER JOIN RIN1 T2 ON T0.DocEntry = T2.DocEntry AND ISNULL(T2.FreeChrgBP, 'N') = 'N'  
 INNER JOIN RIN6 T3 ON T0.DocEntry = T3.DocEntry  
 INNER JOIN OITM T4 ON T2.ItemCode = T4.ItemCode  
 INNER JOIN OITB T5 ON T4.ItmsGrpCod = T5.ItmsGrpCod  
 LEFT  JOIN OSLP T6 ON ISNULL(T0.SlpCode, T1.SlpCode) = T6.SlpCode  
 LEFT  JOIN OPRC T7 ON T2.OcrCode = T7.PrcCode  
 LEFT  JOIN OMRC T8 ON T4.FirmCode = T8.FirmCode  
 LEFT  JOIN OOND T9 ON T1.IndustryC = T9.IndCode  
 LEFT  JOIN RIN12 T10 ON T0.DocEntry = T10.DocEntry  
 LEFT  JOIN OCNT T11 ON T10.CountyB = T11.AbsId  
 INNER JOIN RCT2 T12 ON T3.DocEntry = T12.DocEntry AND T3.InstlmntID = T12.InstId AND T3.ObjType = T12.InvType  
 INNER JOIN ORCT T13 ON T12.DocEntry = T13.DocEntry AND T13.Canceled = 'N'  
 LEFT  JOIN OBOE T14 ON T13.BoeAbs = T14.BoeKey  
 LEFT  JOIN OBOT T15 ON T15.AbsEntry =   
 (  
  SELECT TOP 1 BOT1.AbsEntry FROM BOT1  
  WHERE BOT1.BOENumber = T14.BoeNum AND BOT1.BoeType = T14.BoeType  
  ORDER BY BOT1.AbsEntry DESC  
 ) AND T15.StatusTo = 'P'  
 LEFT  JOIN OJDT T16 ON T15.TransId = T16.Number  
 LEFT  JOIN RCT1 T17 ON T13.DocEntry = T17.DocNum  
 LEFT  JOIN OCHO T18 ON T17.CheckAbs = T18.CheckKey  
OUTER APPLY (  
 SELECT Countt = COUNT(RCT2.InvoiceId) FROM RCT2 WHERE RCT2.DocNum = T13.DocEntry  
) AS OA_MAX  
OUTER APPLY (  
 SELECT DataPagamento =   
  CASE WHEN T15.StatusTo = 'P' THEN T16.RefDate  
  ELSE CASE WHEN T18.CheckKey IS NOT NULL THEN T18.CheckDate  
  ELSE CASE WHEN T13.BoeAbs IS NULL AND T13.DocEntry IS NOT NULL THEN T13.TaxDate  
  ELSE NULL END END END  
) AS OA_DATA  
OUTER APPLY (  
 SELECT TotItem = COUNT(RIN1.LineNum), TotalItens = SUM(RIN1.LineTotal) FROM RIN1 WHERE RIN1.DocEntry = T0.DocEntry  
) AS OA_MAX_ITEM  
OUTER APPLY (  
 SELECT TaxSum = ISNULL(SUM(DISTINCT RIN4.TaxSum), 0.00)  
 FROM RIN4  
  INNER JOIN OSTA ON RIN4.StaCode = OSTA.Code  
  INNER JOIN OSTT ON OSTA.[Type] = OSTT.AbsId  
  INNER JOIN ONFT ON OSTT.NfTaxId = ONFT.AbsId  
 WHERE RIN4.TaxInPrice = 'Y' AND RIN4.DocEntry = T2.DocEntry AND RIN4.LineNum = T2.LineNum  
) AS OA_IMPINCL  
OUTER APPLY (  
 SELECT TaxSum = ISNULL(SUM(DISTINCT RIN4.TaxSum), 0.00)  
 FROM RIN4  
  --INNER JOIN OSTA ON RDR4.StaCode = OSTA.Code  
  --INNER JOIN OSTT ON OSTA.[Type] = OSTT.AbsId  
  --INNER JOIN ONFT ON OSTT.NfTaxId = ONFT.AbsId  
 WHERE RIN4.StaCode like 'IP%' AND RIN4.DocEntry = T2.DocEntry AND RIN4.LineNum = T2.LineNum  
) AS OA_IMPIPI

OUTER APPLY (  
 SELECT Incluir = CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN 1  
  ELSE CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'Y' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'N' THEN 2  
  ELSE CASE WHEN ISNULL(T6.U_CVA_IMPADC, 'N') = 'N' AND ISNULL(T6.U_CVA_IMPINCL, 'N') = 'Y' THEN 3  
  ELSE 0 END END END  
) AS OA_INC  
INNER  JOIN OACT T19 ON T19.AcctCode = (CASE WHEN T13.CashSum > 0 THEN 
												T13.CASHACCT    
											 WHEN T13.TrsfrSUM > 0 THEN 
												T13.TrsfrAcct
											 WHEN T13.Boesum > 0 THEN 
												T13.BoeAcc
											WHEN T13.[CheckSum] > 0 THEN 
												T13.CheckAcct
											END) AND T19.Finanse <> 'Y'
WHERE T0.CANCELED = 'N' AND T3.InsTotal <> 0 AND OA_DATA.DataPagamento >= @DataInicial AND OA_DATA.DataPagamento <= @DataFinal  
  --print 'João 8'
INSERT INTO #tbl_docs_aux SELECT DISTINCT * FROM #tbl_docs T0 ORDER BY 1, 6, 2, 11, 8  
   --print 'João 9'
BEGIN --CURSOR PARA BUSCAR CONDIÇÕES DE CRITÉRIOS WHERE  
SET @cur = CURSOR FAST_FORWARD FOR  
 SELECT DISTINCT  
  T0.Code AS CritCode, T0.Name AS CritName, T0.U_POS AS CritPos, T0.U_ATIVO AS CritAtivo,  
  T1.Code AS RegrCode, T1.Name AS RegrName, T1.U_TIPO, T1.U_COMISSIONADO, T1.U_MOMENTO,   
  T1.U_VENDEDOR, T1.U_ITEM, T1.U_GRUPO, T1.U_CENTROCUSTO, T1.U_FABRICANTE, T1.U_CLIENTE,   
  T1.U_CIDADE, T1.U_ESTADO, T1.U_SETOR, T1.U_COMISSAO ,T1.U_IPI
 FROM [@CVA_CRIT_COMISSAO] T0   
 INNER JOIN [@CVA_REGR_COMISSAO] T1 ON T0.Code = T1.U_PRIORIDADE  
 WHERE T0.U_ATIVO = 'Y' AND T1.U_ATIVO = 'Y'  
 ORDER BY T1.U_TIPO, T1.U_COMISSIONADO, T0.U_POS    
  
OPEN @cur  
FETCH NEXT FROM @cur INTO @CritCode, @CritName, @CritPos, @CritAtivo, @RegrCode, @RegrName, @Tipo, @Comissionado, @Momento, @Vendedor, @Item, @Grupo, @CentroCusto, @Fabricante, @Cliente, @Cidade, @Estado, @Setor, @Comissao ,@IPI  
  
SET @Where = ''  
  
WHILE @@FETCH_STATUS = 0  
BEGIN  
 SET @WHERE = 'Momento = ''' + @Momento + ''''  
  
 if isnull(@Vendedor, isnull(@Comissionado, '')) <> '' set @Where = @Where + ' AND SlpCode = ' + cast(isnull(@Vendedor, @Comissionado) as nvarchar(10))  
 if isnull(@Item, '') <> '' set @Where = @Where + ' AND ItemCode = ''' + @Item + ''''  
 if isnull(@Grupo, '') <> '' set @Where = @Where + ' AND ItmsGrpCod = ' + cast(@Grupo as nvarchar(10))  
 if isnull(@CentroCusto, '') <> '' set @Where = @Where + ' AND PrcCode = ''' + @CentroCusto + ''''  
 if isnull(@Fabricante, '') <> '' set @Where = @Where + ' AND FirmCode = ' + cast(@Fabricante as nvarchar(10))  
 if isnull(@Cliente, '') <> '' set @Where = @Where + ' AND CardCode = ''' + @Cliente + ''''  
 if isnull(@Cidade, '') <> '' set @Where = @Where + ' AND AbsId = ' + cast(@Cidade as nvarchar(10))  
 if isnull(@Estado, '') <> '' set @Where = @Where + ' AND [State] = ''' + @Estado + ''''  
 if isnull(@Setor, '') <> '' set @Where = @Where + ' AND IndCode = ''' + @Setor + ''''  
  
 insert into #tmp_table_where values(@RegrCode, @Comissionado, @Comissao, @Momento, @CritPos, @Where)  
  
 set @Where = ''  
 fetch next from @cur into @CritCode, @CritName, @CritPos, @CritAtivo, @RegrCode, @RegrName, @Tipo, @Comissionado, @Momento, @Vendedor, @Item, @Grupo, @CentroCusto, @Fabricante, @Cliente, @Cidade, @Estado, @Setor, @Comissao  ,@IPI
end  
  
close @cur  
deallocate @cur  
end  
  
set @cur = cursor fast_forward for  
 select RegrCode, Comissionado, Comissao/100, Momento, Prioridade, SqlWhere from #tmp_table_where order by Momento, Comissionado  
  
open @cur  
fetch next from @cur into @RegrCode, @Comissionado, @Comissao, @Momento, @Prioridade, @Where  
  
while @@fetch_status = 0  
begin  
 declare @isItem char(1) = 'N'  
  
 if @Where like '%ItemCode%' begin set @isItem = 'Y' end  
 else if @Where like '%ItmsGrpCod%' begin set @isItem = 'Y' end  
 else begin set @isItem = 'N' end  
  
 SET @Query = N'SELECT DISTINCT @P1, @P2, @P3, @P4, @P5, * FROM #tbl_docs_aux WHERE ' + @Where  
 SET @Params = N'@P1 INT, @P2 INT, @P3 NUMERIC(19,6), @P4 INT, @P5 CHAR(1)'  
  
 INSERT INTO #tbl_docs_fat  
  EXEC sp_executesql @Query, @Params, @P1 = @Regrcode, @P2 = @Comissionado, @P3 = @Comissao, @P4 = @Prioridade, @P5 = @isItem  
    
 fetch next from @cur into @RegrCode, @Comissionado, @Comissao, @Momento, @Prioridade, @Where  
END  
  
CLOSE @cur  
DEALLOCATE @cur  
  


insert into #tmp_table_ret  
 select   
  T0.U_COMISSIONADO,  
  T2.U_POS AS U_PRIORIDADE,  
  T0.U_CARDCODE,  
  T0.U_CARDNAME,  
  T0.U_REGRA,  
  T0.U_DOCDATE,  
  T0.U_DUEDATE,  
  T0.U_DOCENTRY,  
  T0.U_OBJTYPE,  
  T0.U_ITEMCODE,  
  T0.U_ITEMNAME,  
  T0.U_LINENUM,  
  T0.U_VALOR,  
  T0.U_PARCELA,  
  T0.U_IMPOSTOS,  
  T0.U_COMISSAO,  
  T0.U_VALORCOMISSAO,  
  T0.U_CENTROCUSTO,  
  T0.U_PAGO,  
  T0.U_DATAPAGAMENTO,  
  CASE T1.U_MOMENTO WHEN 'P' THEN 'Pedido' WHEN 'F' THEN 'Faturamento' WHEN 'R' THEN 'Recebimento' END  
  ,0
 from [@CVA_CALC_COMISSAO] T0  
  inner join [@CVA_REGR_COMISSAO] T1 on T0.U_REGRA = T1.Code AND T1.U_ATIVO = 'Y'  
  inner join [@CVA_CRIT_COMISSAO] T2 on T1.U_PRIORIDADE = T2.Code  
 where T0.U_PAGO = 'Y'  
  and T0.U_DATAPAGAMENTO >= @DataInicial  
  and T0.U_DATAPAGAMENTO <= @DataFinal  
  and @Status IN ('P', 'T')  
  
insert into #tmp_table_ret  
 select   
  T0.U_COMISSIONADO,  
  T2.U_POS AS U_PRIORIDADE,  
  T0.U_CARDCODE,  
  T0.U_CARDNAME,  
  T0.U_REGRA,  
  T0.U_DOCDATE,  
  T0.U_DUEDATE,  
  T0.U_DOCENTRY,  
  T0.U_OBJTYPE,  
  T0.U_ITEMCODE,  
  T0.U_ITEMNAME,  
  T0.U_LINENUM,  
  T0.U_VALOR,  
  T0.U_PARCELA,  
  T0.U_IMPOSTOS,  
  T0.U_COMISSAO,  
  T0.U_VALORCOMISSAO,  
  T0.U_CENTROCUSTO,  
  T0.U_PAGO,  
  T0.U_DATAPAGAMENTO,  
  CASE T1.U_MOMENTO WHEN 'P' THEN 'Pedido' WHEN 'F' THEN 'Faturamento' WHEN 'R' THEN 'Recebimento' END  
  ,0
 from [@CVA_CALC_COMISSAO] T0  
  inner join [@CVA_REGR_COMISSAO] T1 on T0.U_REGRA = T1.Code AND T1.U_ATIVO = 'Y'  
  inner join [@CVA_CRIT_COMISSAO] T2 on T1.U_PRIORIDADE = T2.Code  
 where T0.U_PAGO = 'N'  
  and T0.U_DOCDATE >= @DataInicial  
  and T0.U_DOCDATE <= @DataFinal  
  and @Status IN ('N', 'T')  
  
  --select * from #tbl_docs_fat where IPI<>0

  --update #tbl_docs_fat
  --set
INSERT INTO #tmp_table_ret  
 SELECT DISTINCT  
  T0.Comissionado,  
  T0.Prioridade,  
  T0.CardCode,  
  T0.CardName,  
  T0.RegrCode,  
  T0.DocDate,  
  T0.DueDate,  
  T0.DocEntry,  
  T0.ObjType,  
  T0.ItemCode,  
  T0.ItemName,  
  T0.LineNum,  
  CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'Y' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'Y' THEN   
   CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal_Min_IMPADIC_Min_IMPINCL ELSE T0.InsTotal_Min_IMPINCL_Min_IMPADIC END  
  ELSE CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'N' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'Y' THEN  
   CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal_Min_IMPINCL ELSE T0.InsTotal_Min_IMPINCL END  
  ELSE CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'Y' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'N' THEN  
   CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal_Min_IMPADIC ELSE T0.InsTotal_Min_IMPADIC END  
  ELSE   
   CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal ELSE T0.InsTotal END  
  END END END AS Valor,  
  T0.InstlmntID,  
  CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'Y' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'Y' THEN   
   T0.TaxSum_IMPADIC + T0.TaxSum_IMPINCL  
  ELSE CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'N' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'Y' THEN  
   T0.TaxSum_IMPINCL  
  ELSE CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'Y' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'N' THEN  
   T0.TaxSum_IMPADIC  
  ELSE 0.00  
  END END END AS Impostos,  
  T0.Comissao*100 AS Comissao,  
  (  
  CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'Y' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'Y' THEN   
   CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal_Min_IMPADIC_Min_IMPINCL ELSE T0.InsTotal_Min_IMPINCL_Min_IMPADIC END  
  ELSE CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'N' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'Y' THEN  
   CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal_Min_IMPINCL ELSE T0.InsTotal_Min_IMPINCL END  
  ELSE CASE WHEN ISNULL(T0.U_CVA_IMPADIC, 'N') = 'Y' AND ISNULL(T0.U_CVA_IMPINCL, 'N') = 'N' THEN  
   CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal_Min_IMPADIC ELSE T0.InsTotal_Min_IMPADIC END  
  ELSE   
   CASE WHEN T0.IsItem = 'Y' THEN T0.LineTotal ELSE T0.InsTotal END  
  END END END  
  )*T0.Comissao AS ValorComissao,  
  T0.PrcCode,  
  'N' AS Pago,  
  NULL AS DataPagamento,  
  CASE T0.Momento WHEN 'P' THEN 'Pedido' WHEN 'F' THEN 'Faturamento' WHEN 'R' THEN 'Recebimento' END  
  ,T0.IPI
 FROM #tbl_docs_fat T0  
  LEFT JOIN #tmp_table_ret T1 ON T0.DocEntry = T1.U_DOCENTRY AND T0.ObjType = T1.U_OBJTYPE AND T0.LineNum = T1.U_LINENUM AND T0.InstlmntID = T1.U_PARCELA AND T0.RegrCode = T1.U_REGRA  
 WHERE @Status IN ('N', 'T') AND T1.U_DOCENTRY IS NULL  
  
  update Q
  set U_Valor=U_Valor+IPI
  from #tmp_table_ret Q
  where IPI>0 and exists (select 1 from [@CVA_REGR_COMISSAO] where code=Q.U_REGRA and U_IPI='Y')
  

  update Q
  set U_VALORCOMISSAO=(U_Valor/100)*U_COMISSAO
  from #tmp_table_ret Q
  where IPI>0 and exists (select 1 from [@CVA_REGR_COMISSAO] where code=Q.U_REGRA and U_IPI='Y')

if @View = 'T'  
 begin  
  select distinct  
   U_COMISSIONADO,   
   U_PRIORIDADE,  
   U_CARDCODE,  
   U_CARDNAME,  
   U_REGRA,  
   U_DOCDATE,  
   U_DUEDATE,  
   U_DOCENTRY,  
   U_OBJTYPE,  
   U_ITEMCODE,  
   U_ITEMNAME,  
   U_LINENUM,  
   U_VALOR,  
   U_PARCELA,  
   U_IMPOSTOS,  
   U_COMISSAO,  
   U_VALORCOMISSAO,  
   isnull(U_CENTROCUSTO, '') AS U_CENTROCUSTO,  
   MAX(U_PAGO) AS U_PAGO,  
   MAX(isnull(U_DATAPAGAMENTO, '1900-01-01')) as U_DATAPAGAMENTO,  
   U_MOMENTO  

  from #tmp_table_ret  
  group by   
   U_COMISSIONADO,   
   U_PRIORIDADE,  
   U_CARDCODE,  
   U_CARDNAME,  
   U_REGRA,  
   U_DOCDATE,  
   U_DUEDATE,  
   U_DOCENTRY,  
   U_OBJTYPE,  
   U_ITEMCODE,  
   U_ITEMNAME,  
   U_LINENUM,  
   U_VALOR,  
   U_PARCELA,  
   U_IMPOSTOS,  
   U_COMISSAO,  
   U_VALORCOMISSAO,  
   isnull(U_CENTROCUSTO, ''),  
   U_MOMENTO  
 end  
 else if @View = 'N'  
 begin  
  select distinct   
   U_COMISSIONADO,   
   U_PRIORIDADE,  
   U_CARDCODE,  
   U_CARDNAME,  
   U_REGRA,  
   U_DOCDATE,  
   U_DUEDATE,  
   U_DOCENTRY,  
   U_OBJTYPE,  
   null as U_ITEMCODE,  
   null as U_ITEMNAME,  
   null as U_LINENUM,  
   SUM(DISTINCT U_VALOR) AS U_VALOR,  
   U_PARCELA,  
   SUM(DISTINCT U_IMPOSTOS) AS U_IMPOSTOS,  
   SUM(DISTINCT U_COMISSAO) AS U_COMISSAO,  
   SUM(DISTINCT U_VALORCOMISSAO) AS U_VALORCOMISSAO,  
   NULL AS U_CENTROCUSTO,  
   MAX(U_PAGO) AS U_PAGO,  
   MAX(isnull(U_DATAPAGAMENTO, '1900-01-01')) as U_DATAPAGAMENTO,  
   U_MOMENTO  
   --,0
  from #tmp_table_ret  
  group by U_COMISSIONADO,   
   U_PRIORIDADE, U_CARDCODE, U_CARDNAME, U_REGRA, U_DOCDATE, U_DUEDATE, U_DOCENTRY, U_OBJTYPE, U_PARCELA, U_MOMENTO  
 end  
 else if @View = 'I'  
 begin  
  select distinct  
   U_COMISSIONADO,   
   U_PRIORIDADE,  
   U_CARDCODE,  
   U_CARDNAME,  
   U_REGRA,  
   U_DOCDATE,  
   U_DUEDATE,  
   U_DOCENTRY,  
   U_OBJTYPE,  
   U_ITEMCODE,  
   U_ITEMNAME,  
   U_LINENUM,  
   U_VALOR,  
   U_PARCELA,  
   U_IMPOSTOS,  
   U_COMISSAO,  
   U_VALORCOMISSAO,  
   isnull(U_CENTROCUSTO, '') AS U_CENTROCUSTO,  
   MAX(U_PAGO) AS U_PAGO,  
   MAX(isnull(U_DATAPAGAMENTO, '1900-01-01')) as U_DATAPAGAMENTO,  
   U_MOMENTO  
  from #tmp_table_ret  
  group by   
   U_COMISSIONADO,   
   U_PRIORIDADE,  
   U_CARDCODE,  
   U_CARDNAME,  
   U_REGRA,  
   U_DOCDATE,  
   U_DUEDATE,  
   U_DOCENTRY,  
   U_OBJTYPE,  
   U_ITEMCODE,  
   U_ITEMNAME,  
   U_LINENUM,  
   U_VALOR,  
   U_PARCELA,  
   U_IMPOSTOS,  
   U_COMISSAO,  
   U_VALORCOMISSAO,  
   isnull(U_CENTROCUSTO, ''),  
   U_MOMENTO  
 end  
  
DROP TABLE #tbl_docs_aux  
DROP TABLE #tbl_docs  
DROP TABLE #tmp_table_where  
DROP TABLE #tbl_docs_fat  
DROP TABLE #tmp_table_ret  
end  

