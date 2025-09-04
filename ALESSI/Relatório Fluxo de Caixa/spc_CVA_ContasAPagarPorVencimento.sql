IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spc_CVA_ContasAPagarPorVencimento')
	DROP PROCEDURE spc_CVA_ContasAPagarPorVencimento
GO
CREATE PROC [dbo].[spc_CVA_ContasAPagarPorVencimento] (
	@CarCode nvarchar(15),
	@dateini date,
	@datefim date,
	@tpdata  varchar(2),
	@provisoes char(1),
	@formaPagamento	nvarchar(200)
)
as 
begin

if @CarCode='*' begin
	set @CarCode=null
end

if @dateini is null
begin
	SET @dateini = '1990-01-01'
end
if @datefim is null
begin
	SET @datefim = '2050-01-01'
end

--select SUM(saldo) from (
select 
	tb.ShortName
	,tb.CardName
	,tb.Lancamento
	,tb.Vencimento
	,tb.Origem
	,CAST(tb.OrigemNr AS NVARCHAR(100)) OrigemNr
	,tb.Parcela
	,tb.ParcelaTotal
	,tb.Serial
	,tb.LineMemo
	,tb.Debit
	,tb.Credit
	,tb.Saldo
	,tb.DueDate

	,case 
			when Origem='18'  then  OPCH.PeyMethod
			when Origem='204'  then  ODPO.PeyMethod	  
			when Origem='19' then  ORPC.PeyMethod
			--else 'N/A'
	end 'PeyMethodNF' 
 from (
			SELECT 	
				T0.[ShortName],
				--ocrd.CardFName,
				ocrd.CardName,
				T0.[RefDate] as Lancamento, 
				T0.[DueDate] as Vencimento,
				T0.[TransType] as Origem,
				T0.[CreatedBy] as OrigemNr,
				T0.[SourceLine] as Parcela,
				OPCH.Installmnt as ParcelaTotal,
				OPCH.Serial,
				T0.[LineMemo],
				T0.[Debit], T0.[Credit],
				(T0.[Debit] - T0.[Credit])*-1 as Saldo
				,T0.DueDate
			FROM  
				[dbo].[JDT1] T0   LEFT OUTER  JOIN [dbo].[OUSR] T1  ON  T0.[UserSign] = T1.[USERID]    
				LEFT OUTER  JOIN (
						SELECT 
							T0.[TransId] AS 'TransId', T0.[TransRowId] AS 'TransRowId', MAX(T0.[ReconNum]) AS 'MaxReconNum' 
						FROM  
							[dbo].[ITR1] T0  
						GROUP BY 
							T0.[TransId], T0.[TransRowId]
				) T2  ON  T0.[TransId] = T2.[TransId]  AND  T0.[Line_ID] = T2.[TransRowId]   
				inner join ocrd on ocrd.CardCode=T0.[ShortName] and ocrd.CardType='S'
				left join OPCH on opch.docentry=T0.[CreatedBy] and T0.[TransType]='18'
	
	
			WHERE 
				(
				(
					((T0.[DueDate] >= (@dateini) and T0.[DueDate] <=(@datefim))  and @tpData='V')
					and (
						((/*T0.[RefDate] >= (@dateIni) and */T0.[RefDate] <= ((CAST(CAST(GETDATE() AS DATE) AS DATETIME))   )) and @tpData='V' )
					)
				)

				or
				((T0.[RefDate] >= (@dateIni) and T0.[RefDate] <= (@datefim)) and @tpData='LC' )

				)

				AND T0.[ShortName] = ISNULL(@CarCode,T0.[ShortName])

				AND  (T0.[BalDueDeb] <> 0  OR  T0.[BalDueCred] <> 0 OR  T0.[BalFcDeb] <> 0  OR  T0.[BalFcCred] <> 0 ) 
	
	
				--AND  (T0.[RefDate] >= @LancamentoIni  AND  T0.[RefDate] <= @LancamentoFim)
				--and (T0.[DueDate] >=@VencimentoIni and T0.[DueDate] <=@VencimentoFim )	
) as tb	
	left join OPCH on OPCH.DocEntry=OrigemNr and Origem='18'
	left join ODPO on ODPO.DocEntry=OrigemNr and Origem='204'
	left join ORPC on ODPO.DocEntry=OrigemNr and Origem='19'
	WHERE	(Origem='18'	and OPCH.PeyMethod = @formaPagamento)
	or		(Origem='204'	and ODPO.PeyMethod = @formaPagamento)
	or		(Origem='19'	and ORPC.PeyMethod = @formaPagamento)
	or		isnull(@formaPagamento, '*') = '*'
union all
SELECT DISTINCT
	T0.ShortName,
	T0.CardName,
	T0.Lancamento,
	T0.Vencimento,
	T0.Origem,
	T0.OrigemNr,
	T0.Parcela,
	T0.ParcelaTotal,
	T0.Serial,
	T0.LineMemo,
	T0.Debit,
	T0.Credit,
	T0.Saldo,
	T0.DueDate,
	T0.PeyMethodNF
FROM [dbo].[fnc_CVA_ProvisoesFluxoCaixa](@dateini, @datefim, @tpdata) T0
WHERE (
		(
			((T0.Lancamento >= @dateini and T0.Lancamento <= @datefim) and @tpdata = 'LC')
		) 
		or
		(
			((T0.Vencimento >= @dateini and T0.Vencimento <= @datefim) and @tpdata = 'V')
		)
	)
	and T0.ShortName = ISNULL(@CarCode, T0.ShortName)
	and @provisoes = 'Y'
ORDER BY 
	ShortName,DueDate asc
END