IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spc_CVA_ContasAReceberPorVencimento')
	DROP PROCEDURE spc_CVA_ContasAReceberPorVencimento
GO
CREATE PROC [dbo].[spc_CVA_ContasAReceberPorVencimento] (
	@CardCode			varchar(15)
	,@dateini			datetime
	,@datefim			datetime
	,@tpData			varchar(2)
	,@BankCode			nvarchar(60)
	,@formaPagamento	nvarchar(200)
	,@grupoEconomico	nvarchar(100)
	,@agrupamento		varchar(2)
	,@validaDiaUtil		varchar(1)
)
as 

Begin

if rtrim(ltrim(isnull(@CardCode, ''))) in ('', '*', '*Todos')
BEGIN
	set @CardCode = null 
END

Create table #NF_Fiscal( 
     [TransId]      int
	,[Line_ID]		int
	,[Account]      nvarchar(300)
	,[ShortName]    nvarchar(300)
	,[TransType]    nvarchar(400)
	,[CreatedBy]    int
	,[BaseRef]      nvarchar(220)
	,[SourceLine]   smallint
	,[RefDate]      datetime
	,[DueDate]      datetime
	,[BalDueCred]	numeric (16, 9) 
	,[BalDueDeb]	numeric (16, 9) 
	,[BalDueCred_BalDueDeb] numeric (16, 9) 
    ,[Saldo]		numeric (16, 9) 
	,[LineMemo]	    nvarchar(1000)
	,[CardName]	    nvarchar(300)
	,[CardCode]	    nvarchar(300)
	,[Balance]	    numeric (19, 6)
	,[SlpCode]	    int
	,[DebitMAthCredit]numeric (19, 6) 
	,[IsSales]		nvarchar(20)
	,[Currency]		nvarchar(60)
	,[BPLName]	    nvarchar(200)
	)

insert into #NF_Fiscal

SELECT 
	T0.[TransId], 
	T0.[Line_ID], 
	MAX(T0.[Account]) as 'Account' ,
	MAX(T0.[ShortName])  as 'ShortName', 
	MAX(T0.[TransType]) as 'TransType', 
	MAX(T0.[CreatedBy]) as 'CreatedBy',
	MAX(T0.[BaseRef]) as 'BaseRef',
	MAX(T0.[SourceLine]) as 'SourceLine', 
	CASE WHEN @tpData = 'V' THEN MAX(OA_REF.RefDate) ELSE MAX(T0.[RefDate]) END as 'RefDate', 
	CASE WHEN @tpData = 'V' THEN MAX(OA_REF.DueDate) ELSE MAX(T0.[DueDate]) END as 'DueDate', 
	
    MAX(T0.[BalDueCred]) AS'BalDueCred',
    SUM(T1.[ReconSum]) AS 'BalDueDeb',
    MAX(T0.[BalDueCred]) + SUM(T1.[ReconSum]) AS 'BalDueCred - BalDueDeb',
    ( MAX(T0.[BalDueCred]) + SUM(T1.[ReconSum]) )  AS 'Saldo',    


	--MAX(T0.[BalDueCred]) + SUM(T1.[ReconSum]), 
	--MAX(T0.[BalFcCred]) + SUM(T1.[ReconSumFC]), 
	--MAX(T0.[BalScCred]) + SUM(T1.[ReconSumSC]), 
	MAX(T0.[LineMemo])as 'LineMemo', 
	MAX(T4.[CardName]) as 'CardName', 
	MAX(T5.[CardCode])as 'CardCode', 

	MAX(T4.[Balance])as 'Balance', 
	MAX(T5.[SlpCode])as 'SlpCode', 

	MAX(T0.[Debit]) + MAX(T0.[Credit]) as 'Debit + Credit', 
	MAX(T5.[IsSales]) as 'IsSales', 
	MAX(T4.[Currency]) as 'Currency', 
	T0.[BPLName] as 'BPLName'
	
FROM  
	[dbo].[JDT1] T0  
	INNER  JOIN [dbo].[ITR1] T1  ON  T1.[TransId] = T0.[TransId] AND  T1.[TransRowId] = T0.[Line_ID]   
	INNER  JOIN [dbo].[OITR] T2  ON  T2.[ReconNum] = T1.[ReconNum]   
	INNER  JOIN [dbo].[OJDT] T3  ON  T3.[TransId] = T0.[TransId]   
	INNER  JOIN [dbo].[OCRD] T4  ON  T4.[CardCode] = T0.[ShortName]    
	LEFT OUTER  JOIN [dbo].[B1_JournalTransSourceView] T5  ON  T5.[ObjType] = T0.[TransType]  
		AND  T5.[DocEntry] = T0.[CreatedBy]  
		AND  (T5.[TransType] <> 'I'  OR  (T5.[TransType] = 'I'  
		AND  T5.[InstlmntID] = T0.[SourceLine] ))  
OUTER APPLY (
	SELECT DueDate = CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](T0.[DueDate])) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](T0.[DueDate])) ELSE T0.[DueDate] END,
		RefDate = CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](T0.[RefDate])) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](T0.[RefDate])) ELSE T0.[RefDate] END,
		ReconDate = CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](T2.[ReconDate])) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](T2.[ReconDate])) ELSE T2.[ReconDate] END
) AS OA_REF
WHERE 
	(
		((T0.[RefDate] <= (@DateFim)  AND  T0.[RefDate] >= (@DateIni)  AND  T0.[RefDate] <= (@DateFim)  
		AND  T2.[ReconDate] > (@DateFim)  )  and @tpData='LC')

		or (
				((OA_REF.DueDate >= (@DateIni)  AND  OA_REF.DueDate <= (@DateFim)  )  and @tpData='V' )
				and
				(
					(   (OA_REF.RefDate <= (SELECT CAST(CAST(GETDATE() AS DATE) AS DATETIME))  
					OR OA_REF.ReconDate >= (SELECT CAST(CAST(GETDATE() AS DATE) AS DATETIME))  )  and @tpData='V')--JBC
				)
			)
	)
	
	AND  T4.[CardType] = ('C')  
	--AND  T4.[Balance] <> (0)  
	
	AND  T1.[IsCredit] = ('C')   

	AND  ((T4.[CardCode] = (@CardCode)  ) OR (ISNULL(@CardCode,'0')='0')  ) 
	AND (ISNULL(@grupoEconomico, '*') = '*' OR  T4.U_CVA_GRUPOECON = @grupoEconomico)

GROUP BY 
	T0.[TransId], T0.[Line_ID], T0.[BPLName] 
HAVING 
	MAX(T0.[BalFcCred]) <>- SUM(T1.[ReconSumFC])  OR  MAX(T0.[BalDueCred]) <>- SUM(T1.[ReconSum])   
UNION ALL 
SELECT 
	T0.[TransId], 
	T0.[Line_ID], 
	MAX(T0.[Account]), 
	MAX(T0.[ShortName]), 
	MAX(T0.[TransType]), 
	MAX(T0.[CreatedBy]), 
	MAX(T0.[BaseRef]), 
	MAX(T0.[SourceLine]), 
	CASE WHEN @tpData = 'V' THEN MAX(OA_REF.RefDate) ELSE MAX(T0.[RefDate]) END, 
	CASE WHEN @tpData = 'V' THEN MAX(OA_REF.DueDate) ELSE MAX(T0.[DueDate]) END, 
	
    MAX(T0.[BalDueCred]) AS'BalDueCred',
    SUM(T1.[ReconSum]) AS 'BalDueDeb',
    MAX(T0.[BalDueCred]) + SUM(T1.[ReconSum]) AS 'BalDueCred - BalDueDeb',
    ( MAX(T0.[BalDueCred]) + SUM(T1.[ReconSum]) ) * (-1) AS 'Saldo',
    	
	--- MAX(T0.[BalDueDeb]) - SUM(T1.[ReconSum]),  
	--- MAX(T0.[BalFcDeb]) - SUM(T1.[ReconSumFC]),  
	--- MAX(T0.[BalScDeb]) - SUM(T1.[ReconSumSC]), 
	MAX(T0.[LineMemo]), 
	MAX(T4.[CardName]), 
	MAX(T5.[CardCode]), 

	MAX(T4.[Balance]), 
	MAX(T5.[SlpCode]), 
	MAX(T0.[Debit]) + MAX(T0.[Credit]), 
	MAX(T5.[IsSales]), 
	MAX(T4.[Currency]), 
	T0.[BPLName] 
FROM  
	[dbo].[JDT1] T0  
	INNER  JOIN [dbo].[ITR1] T1  ON  T1.[TransId] = T0.[TransId]  AND  T1.[TransRowId] = T0.[Line_ID]   
	INNER  JOIN [dbo].[OITR] T2  ON  T2.[ReconNum] = T1.[ReconNum]   
	INNER  JOIN [dbo].[OJDT] T3  ON  T3.[TransId] = T0.[TransId]   
	INNER  JOIN [dbo].[OCRD] T4  ON  T4.[CardCode] = T0.[ShortName]    
	LEFT OUTER  JOIN [dbo].[B1_JournalTransSourceView] T5  ON  T5.[ObjType] = T0.[TransType]  
		AND  T5.[DocEntry] = T0.[CreatedBy]  
		AND  (T5.[TransType] <> 'I'  OR  (T5.[TransType] = 'I'  AND  T5.[InstlmntID] = T0.[SourceLine] ))  
OUTER APPLY (
	SELECT DueDate = CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](T0.[DueDate])) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](T0.[DueDate])) ELSE T0.[DueDate] END,
		RefDate = CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](T0.[RefDate])) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](T0.[RefDate])) ELSE T0.[RefDate] END,
		ReconDate = CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](T2.[ReconDate])) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](T2.[ReconDate])) ELSE T2.[ReconDate] END
) AS OA_REF
WHERE 
	(
		((T0.[RefDate] <= (@DateFim)  AND  T0.[RefDate] >= (@DateIni)  AND  T0.[RefDate] <= (@DateFim)  
		AND  T2.[ReconDate] > (@DateFim)  ) and @tpData='LC' )

		or (
			((OA_REF.DueDate >= (@DateIni)  AND  OA_REF.DueDate <= (@DateFim)  ) and @tpData='V') 
			and
			(
				((OA_REF.RefDate <= (CAST(CAST(GETDATE() AS DATE) AS DATETIME))  /*AND  T0.[RefDate] >= (@DateIni)*/  AND  OA_REF.ReconDate <= (CAST(CAST(GETDATE() AS DATE) AS DATETIME))  
				AND  OA_REF.ReconDate >= (CAST(CAST(GETDATE() AS DATE) AS DATETIME))  ) and @tpData='V' )
			)
		)
	)

	AND  T4.[CardType] = ('C')  
	--AND  T4.[Balance] <> (0)  
	AND  ( (T4.[CardCode] = (@CardCode)  ) OR (ISNULL(@CardCode,'0')='0') )
	AND  T1.[IsCredit] = ('D')   
	AND (ISNULL(@grupoEconomico, '*') = '*' OR  T4.U_CVA_GRUPOECON = @grupoEconomico)

GROUP BY
	T0.[TransId], T0.[Line_ID], T0.[BPLName] 
HAVING 
	MAX(T0.[BalFcDeb]) <>- SUM(T1.[ReconSumFC])  OR  MAX(T0.[BalDueDeb]) <>- SUM(T1.[ReconSum])   
UNION ALL 
SELECT --1 as Query
	T0.[TransId], 
	T0.[Line_ID], 
	MAX(T0.[Account]), 
	MAX(T0.[ShortName]), 
	MAX(T0.[TransType]), 
	MAX(T0.[CreatedBy]), 
	MAX(T0.[BaseRef]), 
	MAX(T0.[SourceLine]), 
	CASE WHEN @tpData = 'V' THEN MAX(OA_REF.RefDate) ELSE MAX(T0.[RefDate]) END, 
	CASE WHEN @tpData = 'V' THEN MAX(OA_REF.DueDate) ELSE MAX(T0.[DueDate]) END, 
	
    MAX(t0.balduecred) as 'BalDueCred',
    MAX(t0.BalDueDeb)  as 'BalDueDeb',
    MAX(T0.[BalDueCred]) - MAX(T0.[BalDueDeb]) as 'BalDueCred - BalDueDeb',
   ( MAX(T0.[BalDueCred]) - MAX(T0.[BalDueDeb]) ) * -1 as 'Saldo'	,
	
	--MAX(T0.[BalDueCred]) - MAX(T0.[BalDueDeb]), 
	--MAX(T0.[BalFcCred]) - MAX(T0.[BalFcDeb]), 
	--MAX(T0.[BalScCred]) - MAX(T0.[BalScDeb]), 
	
	MAX(T0.[LineMemo]), 
	MAX(T2.[CardName]), 
	MAX(T2.[CardCode]), 
	MAX(T2.[Balance]), 
	MAX(T3.[SlpCode]), 
	MAX(T0.[Debit]) + MAX(T0.[Credit]), 
	MAX(T3.[IsSales]), 
	MAX(T2.[Currency]), 
	T0.[BPLName] 
FROM  
	[dbo].[JDT1] T0  
	INNER  JOIN [dbo].[OJDT] T1  ON  T1.[TransId] = T0.[TransId]   
	INNER  JOIN [dbo].[OCRD] T2  ON  T2.[CardCode] = T0.[ShortName]    
	LEFT OUTER  JOIN [dbo].[B1_JournalTransSourceView] T3  ON  T3.[ObjType] = T0.[TransType]  
		AND  T3.[DocEntry] = T0.[CreatedBy]  AND  (T3.[TransType] <> 'I'  
		OR  (T3.[TransType] = 'I'  AND  T3.[InstlmntID] = T0.[SourceLine] ))  
OUTER APPLY (
	SELECT DueDate = CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](T0.[DueDate])) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](T0.[DueDate])) ELSE T0.[DueDate] END,
		RefDate = CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](T0.[RefDate])) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](T0.[RefDate])) ELSE T0.[RefDate] END
) AS OA_REF
WHERE
	( 
		((T0.[RefDate] <= (@DateFim)  AND  T0.[RefDate] >= (@DateIni)  AND  T0.[RefDate] <= (@DateFim)  

		AND   
			NOT EXISTS (
				SELECT U0.[TransId], U0.[TransRowId] 
				FROM  [dbo].[ITR1] U0  
					INNER  JOIN [dbo].[OITR] U1  ON  U1.[ReconNum] = U0.[ReconNum]   
				WHERE 
					T0.[TransId] = U0.[TransId]  AND  T0.[Line_ID] = U0.[TransRowId]  AND  U1.[ReconDate] > (@DateFim)   
				GROUP BY 
					U0.[TransId], U0.[TransRowId])

		) and @tpData='LC')


		or 
			(
				(OA_REF.DueDate >= (@DateIni)  AND  OA_REF.DueDate <= (@DateFim)  and @tpData='V' 	)
				and
				(
					((OA_REF.RefDate <= (CAST(CAST(GETDATE() AS DATE) AS DATETIME))  /*AND  T0.[RefDate] >= (@DateIni)*/  AND  OA_REF.RefDate <= (CAST(CAST(GETDATE() AS DATE) AS DATETIME))  

					AND   
						NOT EXISTS (
							SELECT U0.[TransId], U0.[TransRowId] 
							FROM  [dbo].[ITR1] U0  
								INNER  JOIN [dbo].[OITR] U1  ON  U1.[ReconNum] = U0.[ReconNum]   
							WHERE 
								T0.[TransId] = U0.[TransId]  AND  T0.[Line_ID] = U0.[TransRowId]  AND  (CASE  WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](U1.[ReconDate])) = 0 THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](U1.[ReconDate])) ELSE U1.[ReconDate] END) > (CAST(CAST(GETDATE() AS DATE) AS DATETIME))   
							GROUP BY 
								U0.[TransId], U0.[TransRowId])

					) and @tpData='V')
				)
			)


			--CAST(CAST(GETDATE() AS DATE) AS DATETIME)
	)
	AND  T2.[CardType] = ('C')  AND  T2.[Balance] <> (0)  
	AND  ((T2.[CardCode] = (@CardCode)  ) OR (ISNULL(@CardCode,'0')='0'))
	AND  (T0.[BalDueCred] <> T0.[BalDueDeb]  
	OR  T0.[BalFcCred] <> T0.[BalFcDeb] ) 
	AND (ISNULL(@grupoEconomico, '*') = '*' OR  T2.U_CVA_GRUPOECON = @grupoEconomico)

GROUP BY 
	T0.[TransId], T0.[Line_ID], T0.[BPLName]
	
end 

--select * from #NF_Fiscal
--select distinct TransType from #NF_Fiscal
--203 ODPI
--182 OBOE
--13  OINV
--24  ORCT
--14  IRIN
--30  OJDT
select 
	TransId     
	,Line_ID     
	,Account                                                                                                                                                                                                                                                          
	,ShortName                                                                                                                                                                                                                                                        
	,TransType                                                                                                                                                                                                                                                        
	,CreatedBy   
	,BaseRef                                                                                                                                                                                                                      
	,SourceLine 
	,RefDate                 
	,DueDate                 
	,BalDueCred                              
	,BalDueDeb                               
	,BalDueCred_BalDueDeb                    
	,Saldo                                   
	,LineMemo                                                                                                                                                                                                                                                         
	,CardName                                                                                                                                                                                                                                                         
	,CardCode                                                                                                                                                                                                                                                         
	,Balance                                 
	,SlpCode     
	,DebitMAthCredit                         
	,IsSales              
	,Currency                                                     
	,BPLName                                                                                                                                                                                                  
	,Serial      

	,case when FormaPagamento='' then PeyMethodNF else FormaPagamento end 'FormaPagamento'
	,PeyMethodNF   		

	,case 
		when coalesce(ODSC.BankName,'')<>'' then ODSC.BankCode --ODSC.BankName
		else case  when TransType='24' and  FormaPagamento = 'Boleto'  then TB.BankCode else (select top 1 BnkDflt  from OPYM where PayMethCod=PeyMethodNF )  end 
		
	end	'BancoNF'


	--select * from OPYM
	,Installmnt	
	,OrctComments
	--,ODSC.BankName
	,case 
		when coalesce(ODSC.BankName,'') <> '' then ODSC.BankName
		--ODSC.BankCode
		else case  when TransType='24' and  FormaPagamento = 'Boleto'  then TB.BankName else (select top 1 BnkDflt  from OPYM where PayMethCod=PeyMethodNF )  end 
		
	end	'BankName'
	,DocEntryNFS
	,LateDays
	--,TB.bank
	--,CntrlBnkNF
from (
	 select distinct
		#NF_Fiscal.[TransId]
		,#NF_Fiscal.[Line_ID]
		,#NF_Fiscal.[Account]
		,#NF_Fiscal.[ShortName]
		,#NF_Fiscal.[TransType]
		,#NF_Fiscal.[CreatedBy]
		,#NF_Fiscal.[BaseRef]
		, case when #NF_Fiscal.[SourceLine]<0 then 0 else #NF_Fiscal.[SourceLine] end SourceLine
		,#NF_Fiscal.[RefDate]
		, case when ISNULL(ORCT_NF.BoeSum, 0) > 0 
				then 
					CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](dateadd(day, 1, #NF_Fiscal.[DueDate]))) = 0 
						THEN (SELECT [CVA].[dbo].[fncProximo_Dia_Util](dateadd(day, 1, #NF_Fiscal.[DueDate]))) 
						ELSE dateadd(day, 1, #NF_Fiscal.[DueDate]) 
					END
				else #NF_Fiscal.[DueDate]
		end 'DueDate'
		,#NF_Fiscal.[BalDueCred]
		,#NF_Fiscal.[BalDueDeb]
		,#NF_Fiscal.[BalDueCred_BalDueDeb]
		,#NF_Fiscal.[Saldo]
		,#NF_Fiscal.[LineMemo]
		,#NF_Fiscal.[CardName]
		,#NF_Fiscal.[CardCode]
		,#NF_Fiscal.[Balance]
		,#NF_Fiscal.[SlpCode]
		,#NF_Fiscal.[DebitMAthCredit]
		,#NF_Fiscal.[IsSales]
		,#NF_Fiscal.[Currency]
		,#NF_Fiscal.[BPLName]
	
		,case when #NF_Fiscal.TransType = '13'  then ISNULL(CAST(OINV.Serial AS VARCHAR(100)), '')
			  when #NF_Fiscal.TransType = '30'  then  ISNULL(JDT1.Ref1, '')
			  when #NF_Fiscal.TransType = '14'  then  ISNULL(CAST(ORIN.Serial AS VARCHAR(100)), '')
			  --when #NF_Fiscal.TransType = '24'  then  ORCT.Serial
			  when #NF_Fiscal.TransType = '203' then  ISNULL(CAST(ODPI.Serial AS VARCHAR(100)), '')
			  when #NF_Fiscal.TransType = '24'  then  (select top 1 ISNULL(CAST(OINV.Serial AS VARCHAR(100)), '') from OINV where OINV.docentry in (select top 1 RCT2.baseAbs  from RCT2 where RCT2.invtype='13' and RCT2.DocNum=#NF_Fiscal.[BaseRef]))

		end 'Serial' 
		, case when #NF_Fiscal.TransType = '24'  then  
				case 
					when orct.CashSum   > 0 then 'Dinheiro'
					when orct.BoeSum    > 0 then 'Boleto'
					when orct.CheckSum  > 0 then 'Cheque'
					when orct.CreditSum > 0 then 'Cartão de Crédito'
					when orct.TrsfrSum  > 0 then 'Tranferência Bancária'
					else ''
				end
			else ''
		end 'FormaPagamento'
	
		,case 
			  when #NF_Fiscal.TransType = '13'  then  OINV.PeyMethod
			  when #NF_Fiscal.TransType = '14'  then  ORIN.PeyMethod	  
			  when #NF_Fiscal.TransType = '203' then  ODPI.PeyMethod
			  else ''
		end 'PeyMethodNF' 
		--OINV.Installmnt 
		,case 
			  when #NF_Fiscal.TransType = '13'  then  OINV.Installmnt
			  when #NF_Fiscal.TransType = '14'  then  ORIN.Installmnt
			  when #NF_Fiscal.TransType = '203' then  ODPI.Installmnt
			  else null
		end 'Installmnt '
		,orct.Comments 'OrctComments'
		,ODSC.BankName
		,ODSC.BankCode
		,case
			when #NF_Fiscal.TransType = '24'  then  (select top 1 RCT2.baseAbs  from RCT2 where RCT2.invtype='13' and RCT2.DocNum=#NF_Fiscal.[BaseRef])
			else 0
		end 'DocEntryNFS',
		case when ISNULL(ORCT.DocEntry, 0) <> 0
			THEN case when ISNULL(ORCT.BoeSum, 0) > 0 
				then 
					CASE WHEN @validaDiaUtil = 'Y' AND (SELECT [CVA].[dbo].[fncDia_Util](dateadd(day, 1, OBOT.TaxDate))) = 0 
						THEN DATEDIFF(DAY, (SELECT [CVA].[dbo].[fncProximo_Dia_Util](dateadd(day, 1, OBOE.[DueDate]))), OBOT.TaxDate)
						ELSE DATEDIFF(DAY, OBOE.[DueDate], OBOT.TaxDate)
					END
				ELSE DATEDIFF(DAY, #NF_Fiscal.[DueDate], ORCT.DocDueDate)
				END
			ELSE 0
		END LateDays
	  from 
		#NF_Fiscal
		left join OINV on OINV.DocEntry = #NF_Fiscal.BaseRef and #NF_Fiscal.TransType='13'
		left join rct2 on  RCT2.InvType = 13 and RCT2.DocEntry = OINV.DocEntry and RCT2.DocTransId = #NF_Fiscal.TransId and RCT2.DocLine = #NF_Fiscal.Line_ID
		left join ORCT ORCT_NF on ORCT_NF.DocNum = RCT2.DocNum

		left join OJDT on OJDT.TransId  = #NF_Fiscal.BaseRef and #NF_Fiscal.TransType='30'  
		LEFT JOIN JDT1 on JDT1.TransId = OJDT.TransId
		left join ORIN on ORIN.Docentry = #NF_Fiscal.BaseRef and #NF_Fiscal.TransType='14'
		left join ORCT on ORCT.DocEntry = #NF_Fiscal.BaseRef and #NF_Fiscal.TransType='24' 
		left join OBOE on OBOE.boenum = ORCT.boenum 
		LEFT JOIN (SELECT BoeNumber, OBOT.TaxDate FROM BOT1 
					INNER JOIN OBOT ON OBOT.AbsEntry = BOT1.AbsEntry AND StatusFrom = 'D' AND StatusTo = 'P'
					) OBOT ON OBOT.BOENumber = OBOE.BoeNum
		left join OCHH on OCHH.RcptNum = ORCT.DocNum
		left join ODSC on (ODSC.BankCode=OBOE.BPBankCod or ODSC.BankCode = OCHH.BankCode)

		left join ODPI on ODPI.DocEntry = #NF_Fiscal.BaseRef and #NF_Fiscal.TransType='203'
		WHERE ISNULL(ORCT_NF.CANCELED, 'N') = 'N'
) TB	
	left join ODSC on ODSC.BankCode=(select top 1 BnkDflt  from OPYM where PayMethCod=PeyMethodNF ) 
	where (@BankCode=(select top 1 BnkDflt  from OPYM where PayMethCod=PeyMethodNF )) or isnull(@BankCode, '*')='*'
	and (PeyMethodNF = @formaPagamento OR isnull(@formaPagamento, '*') = '*')r
	
order by 1	  

drop table #NF_Fiscal