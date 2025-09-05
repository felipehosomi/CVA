ALTER VIEW [dbo].[DOC1_View] AS 
SELECT T1.[DocEntry], 132 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[OCIN] T0  
INNER  JOIN [dbo].[CIN1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 163 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[OCPI] T0  
INNER  JOIN [dbo].[CPI1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 164 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[OCPV] T0  
INNER  JOIN [dbo].[CPV1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 165 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[OCSI] T0  
INNER  JOIN [dbo].[CSI1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 166 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[OCSV] T0  
INNER  JOIN [dbo].[CSV1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 15 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[ODLN] T0  
INNER  JOIN [dbo].[DLN1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 203 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[ODPI] T0  
INNER  JOIN [dbo].[DPI1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 204 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[ODPO] T0  
INNER  JOIN [dbo].[DPO1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 140000010 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[OIEI] T0  
INNER  JOIN [dbo].[IEI1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 60 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[OIGE] T0  
INNER  JOIN [dbo].[IGE1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 59 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[OIGN] T0  
INNER  JOIN [dbo].[IGN1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 13 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[OINV] T0  
INNER  JOIN [dbo].[INV1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 140000009 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[OOEI] T0  
INNER  JOIN [dbo].[OEI1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 18 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[OPCH] T0  
INNER  JOIN [dbo].[PCH1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 20 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[OPDN] T0  
INNER  JOIN [dbo].[PDN1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 22 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[OPOR] T0  
INNER  JOIN [dbo].[POR1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 23 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[OQUT] T0  
INNER  JOIN [dbo].[QUT1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 16 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[ORDN] T0  
INNER  JOIN [dbo].[RDN1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 17 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[ORDR] T0  
INNER  JOIN [dbo].[RDR1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 14 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[ORIN] T0  
INNER  JOIN [dbo].[RIN1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 19 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[ORPC] T0  
INNER  JOIN [dbo].[RPC1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 21 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[ORPD] T0  
INNER  JOIN [dbo].[RPD1] T1  ON  T0.[DocEntry] = T1.[DocEntry]   

UNION ALL

SELECT T1.[DocEntry], 67 AS 'DocType', T1.[LineNum], T0.[Model], T0.[Serial], T0.[SeriesStr], T0.[SubStr], T0.[TaxDate], T1.[CFOPCode], T1.[AcctCode]  
FROM  [dbo].[OWTR] T0  
INNER  JOIN [dbo].[WTR1] T1  ON  T0.[DocEntry] = T1.[DocEntry]  