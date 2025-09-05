--select * from vW_CVA_Fluxo_A_Receber where cardcode='C000012'  and docentry =5340

CREATE VIEW VW_CVA_Fluxo_A_Receber
AS
SELECT 
	T0.[BPLId] ,
	T0.[BPLName] ,
	T2.[CardCode] ,
	T2.[CardName],
	T0.[objtype] ,
	T0.[docentry] ,
	T0.[Serial], 
	T1.[InstlmntID],
	[Prestacao] = convert(nvarchar(10), T1.[InstlmntID]) + ' de ' + convert(nvarchar(10), [Installmnt]), 
	[TipoDocumento] = 'NFe',
	T0.[DocDate], 
	T1.[DueDate], 
	[DiasAtraso] = datediff(Day,T1.[DueDate],getdate()),
	T1.[Instotal],
	[ValorRetido] = 0.00,
	[Total] = T1.[Instotal]- T1.[PaidToDate] * (100 - isnull((select sum(Rate) from PCH5 S1 where S1.AbsEntry = T0.DocEntry),0))/100,
	[Porcentagem] = T0.[DiscPrcnt],
	[DescontoTotal] = 0.00,
	[JurosTotal] = 0.00,
	[PagtoTotal] = 0.00
from [OINV] T0  
	INNER  JOIN [INV6] T1  ON  T1.[docentry] = T0.[docentry]
	INNER  JOIN [OCRD] T2  ON  T0.[CardCode] = T2.[CardCode] 
	LEFT  JOIN [OBPL] T3  ON  T0.[BPLId] = T3.[BPLId]    
WHERE (T1.[TotalBlck] <> T1.[InsTotal] ) 
	AND  T1.[Status] = 'O'
	AND T0.DocTotal > 0
	AND T1.InsTotal > 0
	AND T0.[CANCELED] = 'N'
	AND T2.[CardType] = 'C'
			    					
UNION 
			
-- ODPI
SELECT 
	T0.[BPLId] as CódigoFilial,
	T0.[BPLName] as NomeFilial,
	T2.[CardCode] ,
	T2.[CardName],
	T0.[objtype] ,
	T0.[docentry] ,
	[U_docnum] = T0.[DocNum], 
	[Parcela] = T1.[InstlmntID],
	[U_prestac] = convert(nvarchar(10), T1.[InstlmntID]) + ' de ' + convert(nvarchar(10), [Installmnt]), 
	[U_tipo] = 'AD',
	[U_datadoc] = T0.[DocDate], 
	[U_dataven] = T1.[DueDate], 
	[U_atraso] = datediff(Day,T1.[DueDate],getdate()),
	[U_total] = T1.[Instotal],
	[Valor Retido] = 0.00,
	[U_saldo] = T1.[Instotal]- T1.[PaidToDate] * (100 - isnull((select sum(Rate) from DPO5 S1 where S1.AbsEntry = T0.DocEntry),0))/100,
	[Porcentagem] = T0.[DiscPrcnt],
	[U_destot] = 0.00,
	[Juros Total] = 0.00,
	[U_pagtot] = 0.00
from [ODPO] T0  
	INNER  JOIN [DPO6] T1  ON  T1.[docentry] = T0.[docentry] 
	INNER  JOIN [OCRD] T2  ON  T0.[CardCode] = T2.[CardCode]  
	LEFT  JOIN [OBPL] T3  ON  T0.[BPLId] = T3.[BPLId]   
WHERE (T1.[TotalBlck] <> T1.[InsTotal] ) 
	AND  T1.[Status] = 'O'
	AND T0.DocTotal > 0
	AND T1.InsTotal > 0
	AND T0.[CANCELED] = 'N'
	AND T2.[CardType] = 'S'
				
				
UNION 
			
-- ODPO - situação fechado
SELECT 
	T0.[BPLId] as CódigoFilial,
	T0.[BPLName] as NomeFilial,
	T2.[CardCode] ,
	T2.[CardName],
	T0.[objtype] ,
	T0.[docentry] ,
	[U_docnum] = T0.[DocNum], 
	[Parcela] = T1.[InstlmntID],
	[U_prestac] = convert(nvarchar(10), T1.[InstlmntID]) + ' de ' + convert(nvarchar(10), [Installmnt]), 
	[U_tipo] = 'AD',
	[U_datadoc] = T0.[DocDate], 
	[U_dataven] = T1.[DueDate], 
	[U_atraso] = datediff(Day,T1.[DueDate],getdate()),
	[U_total] = T1.[Instotal],
	[Valor Retido] = 0.00,
	[U_saldo] = T1.[Instotal]- T1.[PaidToDate] * (100 - isnull((select sum(Rate) from DPO5 S1 where S1.AbsEntry = T0.DocEntry),0))/100,
	[Porcentagem] = T0.[DiscPrcnt],
	[U_destot] = 0.00,
	[Juros Total] = 0.00,
	[U_pagtot] = 0.00
from [ODPI] T0  
	INNER  JOIN [DPI6] T1  ON  T1.[docentry] = T0.[docentry] 
	INNER  JOIN [OCRD] T2  ON  T0.[CardCode] = T2.[CardCode] 
	LEFT  JOIN [OBPL] T3  ON  T0.[BPLId] = T3.[BPLId]      
WHERE (T1.[TotalBlck] <> T1.[InsTotal] ) 
	AND  T1.[Status] = 'C'
	AND T0.DocTotal > 0
	AND T1.InsTotal > 0
	AND T0.[CANCELED] = 'N'
	AND T0.[Posted] = 'N'
	AND T0.[PaidSum] <> 0
	AND T0.[DpmStatus] = 'O'
	AND T2.[CardType] = 'S'
				
UNION
				
-- OCPI
SELECT 
	T0.[BPLId] as CódigoFilial,
	T0.[BPLName] as NomeFilial,
	T2.[CardCode] ,
	T2.[CardName],
	T0.[objtype] ,
	T0.[docentry] ,
	[U_docnum] = T0.[DocNum], 
	[Parcela] = T1.[InstlmntID],
	[U_prestac] = convert(nvarchar(10), T1.[InstlmntID]) + ' de ' + convert(nvarchar(10), [Installmnt]), 
	[U_tipo] = 'CorNF',
	[U_datadoc] = T0.[DocDate], 
	[U_dataven] = T1.[DueDate], 
	[U_atraso] = datediff(Day,T1.[DueDate],getdate()),
	[U_total] = T1.[Instotal],
	[Valor Retido] = 0.00,
	[U_saldo] = T1.[Instotal]- T1.[PaidToDate] * (100 - isnull((select sum(Rate) from CPI5 S1 where S1.AbsEntry = T0.DocEntry),0))/100,
	[Porcentagem] = T0.[DiscPrcnt],
	[U_destot] = 0.00,
	[Juros Total] = 0.00,
	[U_pagtot] = 0.00
					
from [OCSI] T0  
	INNER  JOIN [CSI6] T1  ON  T1.[docentry] = T0.[docentry]  
	INNER  JOIN [OCRD] T2  ON  T0.[CardCode] = T2.[CardCode] 
	LEFT  JOIN [OBPL] T3  ON  T0.[BPLId] = T3.[BPLId]     
WHERE (T1.[TotalBlck] <> T1.[InsTotal] ) 
	AND  T1.[Status] = 'O'
	AND T0.DocTotal > 0
	AND T1.InsTotal > 0
	AND T0.[CANCELED] = 'N'
	AND T2.[CardType] = 'S'
					
					
UNION
				
-- ORIN
SELECT
	T0.[BPLId] as CódigoFilial,
	T0.[BPLName] as NomeFilial,
	T2.[CardCode] ,
	T2.[CardName],
	T0.[objtype] ,
	T0.[docentry] ,
	[U_docnum] = T0.[DocNum], 
	[Parcela] = T1.[InstlmntID],
	[U_prestac] = convert(nvarchar(10), T1.[InstlmntID]) + ' de ' + convert(nvarchar(10), [Installmnt]), 
	[U_tipo] = 'DevNFE',
	[U_datadoc] = T0.[DocDate], 
	[U_dataven] = T1.[DueDate], 
	[U_atraso] = datediff(Day,T1.[DueDate],getdate()),
	[U_total] = -T1.[Instotal],
	[Valor Retido] = 0.00,
	[U_saldo] = -(T1.[Instotal]- T1.[PaidToDate] * (100 - isnull((select sum(Rate) from RPC5 S1 where S1.AbsEntry = T0.DocEntry),0))/100),
	[Porcentagem] = T0.[DiscPrcnt],
	[U_destot] = 0.00,
	[Juros Total] = 0.00,
	[U_pagtot] = 0.00
					
from [ORIN] T0  
	INNER  JOIN [RIN6] T1  ON  T1.[docentry] = T0.[docentry]   
	INNER  JOIN [OCRD] T2  ON  T0.[CardCode] = T2.[CardCode] 
	LEFT  JOIN [OBPL] T3  ON  T0.[BPLId] = T3.[BPLId]     
WHERE (T1.[TotalBlck] <> T1.[InsTotal] ) 
	AND  T1.[Status] = 'O'
	AND T0.DocTotal > 0
	AND T1.InsTotal > 0
	AND T0.[CANCELED] = 'N'
	AND T2.[CardType] = 'S'
										
					
UNION
					
-- JDT1
SELECT
	T0.[BPLId] as CódigoFilial,
	T0.[BPLName] as NomeFilial,
	T2.[CardCode] ,
	T2.[CardName],
	T0.[objtype] ,
	T0.[TransId] ,
	[U_docnum] = T0.[TransId],
	[Parcela] = T0.Line_ID,
	[U_prestac] =  '1 de 1', 
	[U_tipo] = 'LC',
	[U_datadoc] = T0.[TaxDate], 
	[U_dataven] = T1.[DueDate], 
	[U_atraso] = datediff(Day,T1.[DueDate],getdate()),
	[U_total] = T0.[Debit],
	[Valor Retido] = 0,
	[U_saldo] = T0.[BalDueDeb],
	[Porcentagem] = 0.00,
	[U_destot] = 0.00,
	[Juros Total] = 0.00,
	[U_pagtot] = 0.00
					
from [JDT1] T0  
	INNER  JOIN [OJDT] T1  ON  T0.[TransId] = T1.[TransId]   
	INNER  JOIN [OCRD] T2  ON  T0.[ShortName] = T2.[CardCode] 
	LEFT  JOIN [OBPL] T3  ON  T0.[BPLId] = T3.[BPLId]    
WHERE T0.[Closed] = 'N'
	AND  T0.[TransType] = 30