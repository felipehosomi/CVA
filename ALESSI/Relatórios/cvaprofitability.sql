ALTER  view [dbo].[cvaprofitability]
AS
 -- =========================================================
-- Autor:			Luis Neves
-- Criação:			12/10/2017
-- Descrição:		Relatório de Rentabilidade - SQL Principal
-- Versao:			1.0.0.0
-- Data Versao:     12/10/2017
-- =========================================================

-----------------------------------------------------------------------------------------------------------------------------------
----------------------------------------NOTA FISCAL DE SAIDA-----------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------
  SELECT 
		INV1.Docentry               AS 'NRDOCUMENTO',
		'NOTA FISCAL DE SAIDA'      AS 'DOCUMENTO',
		OINV.Serial                 AS 'NRNOTA',
		OINV.CardCode               AS 'COD',
		OINV.Cardname               AS 'NOMEPN',
		OOND.IndName                AS 'PORTECLIENTE',
		[@CVA_GRUPOECON].Name       AS 'GRUPOECONOMICO',
		OINV.DocDate                AS 'DATADOCUMENTO',
		INV12.States                AS 'ESTADO',
		INV12.CityS                 AS 'CIDADE',
		OSLP.SlpCode                AS 'IDVENDEDOR',
		OSLP.SlpName                AS 'VENDEDOR',
		(SELECT htm1.teamid
					FROM htm1
				   inner join ohtm
					  ON htm1.teamid = ohtm.teamid
				   WHERE htm1.empid = (SELECT OHEM.empid FROM OHEM WHERE OHEM.salesPrson = OSLP.SlpCode)
					 and ISNULL(htm1.U_akbactive, '') <> 'N'
				   ) AS 'IDEQUIPE', 
		ISNULL(
			   SUBSTRING(
			   (
				  SELECT ',[' + RTRIM(ohtm.name) + ']' as [text()]
					FROM htm1
				   inner join ohtm
					  ON htm1.teamid = ohtm.teamid
				   WHERE htm1.empid = (SELECT OHEM.empid FROM OHEM WHERE OHEM.salesPrson = OSLP.SlpCode)
					 and ISNULL(htm1.U_akbactive, '') <> 'N'
				   ORDER BY 1
				   FOR XML PATH('')
			   ), 2, 9999), '')      AS 'EQUIPE',
	    CVAMANAGER.Supervisor        AS 'GERENTE',  
	    OUSG.Usage                   AS 'UTILIZAÇÃO',
		INV1.LineNum                 AS 'NRLINHA',
		[@CVA_LINHA].Code            AS 'IDLINHA',
		[@CVA_LINHA].Name            AS 'LINHA',
		[@CVA_ITEM_SEGMENTO].Code    AS 'IDSEGMENTO',
		[@CVA_ITEM_SEGMENTO].Name    AS 'SEGMENTO',
		[@CVA_ACABAMENTO].Code       AS 'IDACABAMENTO', 
		[@CVA_ACABAMENTO].Name       AS 'ACABAMENTO',
		[@CVA_ITEM_CLASS].Code       AS 'IDCLASSIF',
		[@CVA_ITEM_CLASS].Name       AS 'CLASSIF',
		[@CVA_ITEM_DISP].Code        AS 'IDDISPONIBILIDADE',
		[@CVA_ITEM_DISP].Name        AS 'DISPONIBILIDADE',
		[@CVA_ITEM_SISTEMA].Code     AS 'IDSISTEMA',
		[@CVA_ITEM_SISTEMA].Name     AS 'SISTEMA',
		[@CVA_ITEM_MARCA].Code       AS 'IDMARCA',
		[@CVA_ITEM_MARCA].Name       AS 'MARCA',
		OITB.ItmsGrpCod              AS 'IDGRUPO',
		OITB.ItmsGrpNam              AS 'GRUPO',
		INV1.Weight1                 AS 'PESO',
		CASE WHEN OITM.U_CONTEUDO2 = 'L' THEN 
			CASE WHEN ISNULL(OITM.U_CONTEUDO1,'')='' THEN 1 ELSE 
				CASE WHEN ISNUMERIC(OITM.U_CONTEUDO1) = 1 THEN (replace(OITM.U_CONTEUDO1,',', '.')) * INV1.Quantity ELSE ISNULL(OITM.U_CONTEUDO1,'0') END 
			END  
		WHEN OITM.U_CONTEUDO2 = 'G' THEN 
			CASE WHEN ISNULL(OITM.U_CONTEUDO1,'')='' THEN 1 ELSE 
				CASE WHEN ISNUMERIC(OITM.U_CONTEUDO1) = 1 THEN (((replace(OITM.U_CONTEUDO1,',', '.')) * INV1.Quantity)/1000) / CASE WHEN dbo.fncCvaWebAPIGetDensity (INV1.ItemCode) = 0.00 THEN dbo.fncCvaWebAPIGetDensityMaterialList (INV1.ItemCode)
				ELSE dbo.fncCvaWebAPIGetDensity (INV1.ItemCode) END  ELSE ISNULL(OITM.U_CONTEUDO1,'0') END	 
			END  	
		ELSE 
			CASE WHEN ISNUMERIC(OITM.U_CONTEUDO1) = 1 THEN ((replace(OITM.U_CONTEUDO1,',', '.'))* INV1.Quantity) / CASE WHEN dbo.fncCvaWebAPIGetDensity (INV1.ItemCode) = 0.00 THEN dbo.fncCvaWebAPIGetDensityMaterialList (INV1.ItemCode) 
			ELSE dbo.fncCvaWebAPIGetDensity (INV1.ItemCode) END  ELSE ISNULL(OITM.U_CONTEUDO1,'0') END	 
		END							 AS 'LITROS',
		0.00                         AS 'MEDLITROS',
		OITM.U_CONTEUDO1             AS 'CONTEUDO',
		INV1.ItemCode                AS 'ITEM',
		INV1.Dscription              AS 'DESCITEM',
		INV1.U_AKBPRICE              AS 'VLRORIGINAL',
		INV1.U_akbdiscperc           AS 'DSCORIGINAL',
		INV1.U_akbdiscvalue          AS 'VLRDSCORIGINAL',
		OINV.DiscSum                 AS 'DESCONTOGERAL',
		INV1.Price                   AS 'VLRUNIT',
		INV1.Quantity                AS 'QUANTIDADE',
        INV1.Quantity * INV1.Price   AS 'TOTAL',
		(SELECT  dbo.fncArkabDocTaxPercentual (OINV.ObjType,OINV.DocEntry,'ICMS', INV1.LineNum))          AS 'PERICMS',
		CASE WHEN ISNULL(cvafreight.FreteItem,0) = 0 THEN isnull(INV1.U_akbfreight,0) ELSE ISNULL(cvafreight.FreteItem,0) END  AS 'FRETE',
		''                           AS 'TRANSPORTADORA',
		''                           AS 'NOMETRANSP',
		''                           AS 'CTE',
		ISNULL(INV1.U_akbexpenses,0) AS 'ACRESCIMO',
		OSHP.TrnspName               AS 'TIPOFRETE',
		ISNULL(INV1.U_cva_fretemedio,0) AS 'FRETEMEDIO',
		ISNULL(INV1.U_CVA_Frete_Estimado,0)    AS 'FRETEESTIMADO',
		ISNULL((CASE WHEN INV12.State = 'PR' THEN 
					(CASE WHEN ISNULL(INV1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = INV1.ItemCode)*INV1.Quantity) ELSE INV1.U_cva_cmv * INV1.Quantity END )
				ELSE 
				(CASE WHEN ISNULL(OITM.U_RedST,0) > 0 THEN dbo.FN_CVA_CUSTORENTABILIDADE (INV1.ItemCode)* INV1.Quantity ELSE (((CASE WHEN ISNULL(INV1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = INV1.ItemCode)*INV1.Quantity) ELSE INV1.U_cva_cmv * INV1.Quantity END ))) END)  
			END),0)  AS 'CMV',
		(INV1.U_cva_comissao/100)     AS 'PERCOMISSÃO',
		CASE WHEN ISNULL(INV1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE INV1.U_cva_custovar END AS 'CUSTOVARIAVEL',
		(CASE WHEN ISNULL(INV1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE INV1.U_cva_custovar END) + (CASE WHEN ISNULL(U_cva_comissao,0) = 0 THEN 0.00 ELSE U_cva_comissao END  ) + (dbo.fncArkabDocTaxPercentual(OINV.ObjType,OINV.DocEntry,'ICMS', INV1.LineNum)) AS 'PERCUSTOVARIAVEL',
		(((CASE WHEN ISNULL(INV1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE INV1.U_cva_custovar END) + (CASE WHEN ISNULL(U_cva_comissao,0) = 0 THEN 0.00 ELSE U_cva_comissao END  ) + (dbo.fncArkabDocTaxPercentual(OINV.ObjType,OINV.DocEntry,'ICMS', INV1.LineNum)))/100 * (INV1.Quantity * INV1.Price))    AS 'VLRCUSTOVARIAVEL',
		CASE WHEN ISNULL(INV1.U_cva_custofixo,0) = 0 THEN OITM.U_cva_custofixo ELSE INV1.U_cva_custofixo  END AS      'PERCUSTOFIXO',
		CASE WHEN ISNULL(INV1.U_cva_custofixo,0) = 0 THEN ((OITM.U_cva_custofixo/100) * (INV1.Quantity * INV1.Price)) ELSE (INV1.U_cva_custofixo/100) * (INV1.Quantity * INV1.Price) END AS 'VLRCUSTOFIXO',
		((CASE WHEN ISNULL(INV1.U_cva_porcustodia,0) = 0 THEN (Select top 1  (U_cva_custodia/100) from [@CVA_CUSTODIA]) ELSE INV1.U_cva_porcustodia/100 END))       AS 'PERCUSTODIA',
		(((CASE WHEN ISNULL(INV1.U_cva_porcustodia,0) = 0 THEN (Select top 1  (U_cva_custodia/100) from [@CVA_CUSTODIA]) ELSE INV1.U_cva_porcustodia/100 END)) * INV6.[Prazo Medio] * (INV1.Quantity * INV1.Price)) AS 'VLRCUSTODIA',
		INV6.[Prazo Medio]           AS 'PRAZOMEDIO',
		(INV6.[Prazo Medio] * (INV1.Quantity * INV1.Price))    AS 'PRAZOMEDIOCALULO',

		  ((INV1.Quantity * INV1.Price) -- VALOR MERCADORIAS
		- CASE WHEN ISNULL(cvafreight.FreteItem,0) = 0 THEN isnull(INV1.U_akbfreight,0) ELSE ISNULL(cvafreight.FreteItem,0) END  -- FRETE
	    - (((CASE WHEN ISNULL(INV1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE INV1.U_cva_custovar END) + (CASE WHEN ISNULL(U_cva_comissao,0) = 0 THEN 0.00 ELSE U_cva_comissao END  ) + (dbo.fncArkabDocTaxPercentual(OINV.ObjType,OINV.DocEntry,'ICMS', INV1.LineNum)))/100 * (INV1.Quantity * INV1.Price)) -- CUSTO VARIAVEL
		- (((CASE WHEN ISNULL(INV1.U_cva_porcustodia,0) = 0 THEN (Select top 1  (U_cva_custodia/100) from [@CVA_CUSTODIA]) ELSE INV1.U_cva_porcustodia/100 END)) * INV6.[Prazo Medio] * (INV1.Quantity * INV1.Price)) -- VALOR CUSTO DIA 
		) AS  'VENDALIQUIDA',


		  ((INV1.Quantity * INV1.Price) -- VALOR MERCADORIAS
		- CASE WHEN ISNULL(cvafreight.FreteItem,0) = 0 THEN isnull(INV1.U_akbfreight,0) ELSE ISNULL(cvafreight.FreteItem,0) END  -- FRETE
		- ((CASE WHEN INV12.State = 'PR' THEN 
							(CASE WHEN ISNULL(INV1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = INV1.ItemCode)*INV1.Quantity) ELSE INV1.U_cva_cmv * INV1.Quantity END )
						ELSE 
						(CASE WHEN OITM.U_RedST > 0 THEN dbo.FN_CVA_CUSTORENTABILIDADE (INV1.ItemCode)* INV1.Quantity ELSE (((CASE WHEN ISNULL(INV1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = INV1.ItemCode)*INV1.Quantity) ELSE INV1.U_cva_cmv * INV1.Quantity END ))) END)  
				 END)) -- CMV
	    - (((CASE WHEN ISNULL(INV1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE INV1.U_cva_custovar END) + (CASE WHEN ISNULL(U_cva_comissao,0) = 0 THEN 0.00 ELSE U_cva_comissao END  ) + (dbo.fncArkabDocTaxPercentual(OINV.ObjType,OINV.DocEntry,'ICMS', INV1.LineNum)))/100 * (INV1.Quantity * INV1.Price)) -- CUSTO VARIAVEL
		- (((CASE WHEN ISNULL(INV1.U_cva_porcustodia,0) = 0 THEN (Select top 1  (U_cva_custodia/100) from [@CVA_CUSTODIA]) ELSE INV1.U_cva_porcustodia/100 END)) * INV6.[Prazo Medio] * (INV1.Quantity * INV1.Price)) -- VALOR CUSTO DIA 
		 ) AS 'VLRCONTRIBUICAO',

	
		(((INV1.Quantity * INV1.Price) -- VALOR MERCADORIAS
		- CASE WHEN ISNULL(cvafreight.FreteItem,0) = 0 THEN isnull(INV1.U_akbfreight,0) ELSE ISNULL(cvafreight.FreteItem,0) END  -- FRETE
		- ((CASE WHEN INV12.State = 'PR' THEN 
							(CASE WHEN ISNULL(INV1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = INV1.ItemCode)*INV1.Quantity) ELSE INV1.U_cva_cmv * INV1.Quantity END )
						ELSE 
						(CASE WHEN OITM.U_RedST > 0 THEN dbo.FN_CVA_CUSTORENTABILIDADE (INV1.ItemCode)* INV1.Quantity ELSE (((CASE WHEN ISNULL(INV1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = INV1.ItemCode)) ELSE INV1.U_cva_cmv * INV1.Quantity END ))) END)  
				 END))-- CMV
	    - (((CASE WHEN ISNULL(INV1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE INV1.U_cva_custovar END) + (CASE WHEN ISNULL(U_cva_comissao,0) = 0 THEN 0.00 ELSE U_cva_comissao END  ) + (dbo.fncArkabDocTaxPercentual(OINV.ObjType,OINV.DocEntry,'ICMS', INV1.LineNum)))/100 * (INV1.Quantity * INV1.Price)) -- CUSTO VARIAVEL
		- (((CASE WHEN ISNULL(INV1.U_cva_porcustodia,0) = 0 THEN (Select top 1  (U_cva_custodia/100) from [@CVA_CUSTODIA]) ELSE INV1.U_cva_porcustodia/100 END)) * INV6.[Prazo Medio] * (INV1.Quantity * INV1.Price))  -- VALOR CUSTO DIA 
		)/(INV1.Quantity * INV1.Price))*100
		  AS 'PERCONTRIBUICAO',

		  ((INV1.Quantity * INV1.Price) -- VALOR MERCADORIAS
		- CASE WHEN ISNULL(cvafreight.FreteItem,0) = 0 THEN isnull(INV1.U_akbfreight,0) ELSE ISNULL(cvafreight.FreteItem,0) END  -- FRETE
		- ((CASE WHEN INV12.State = 'PR' THEN 
							(CASE WHEN ISNULL(INV1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = INV1.ItemCode)*INV1.Quantity) ELSE INV1.U_cva_cmv * INV1.Quantity END )
						ELSE 
						(CASE WHEN OITM.U_RedST > 0 THEN dbo.FN_CVA_CUSTORENTABILIDADE (INV1.ItemCode)* INV1.Quantity ELSE (((CASE WHEN ISNULL(INV1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = INV1.ItemCode)) ELSE INV1.U_cva_cmv * INV1.Quantity END ))) END)  
				 END)) -- CMV
	     - (((CASE WHEN ISNULL(INV1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE INV1.U_cva_custovar END) + (CASE WHEN ISNULL(U_cva_comissao,0) = 0 THEN 0.00 ELSE U_cva_comissao END  ) + (dbo.fncArkabDocTaxPercentual(OINV.ObjType,OINV.DocEntry,'ICMS', INV1.LineNum)))/100 * (INV1.Quantity * INV1.Price))  -- CUSTO VARIAVEL
		- CASE WHEN ISNULL(INV1.U_cva_custofixo,0) = 0 THEN ((OITM.U_cva_custofixo/100) * (INV1.Quantity * INV1.Price)) ELSE (INV1.U_cva_custofixo/100) * (INV1.Quantity * INV1.Price) END-- VALOR CUSTO FIXO
		- (((CASE WHEN ISNULL(INV1.U_cva_porcustodia,0) = 0 THEN (Select top 1  (U_cva_custodia/100) from [@CVA_CUSTODIA]) ELSE INV1.U_cva_porcustodia/100 END)) * INV6.[Prazo Medio] * (INV1.Quantity * INV1.Price))  -- VALOR CUSTO DIA 
		 )  AS 'RENTABILIDADE',


		(((INV1.Quantity * INV1.Price) -- VALOR MERCADORIAS
		- CASE WHEN ISNULL(cvafreight.FreteItem,0) = 0 THEN isnull(INV1.U_akbfreight,0) ELSE ISNULL(cvafreight.FreteItem,0) END  -- FRETE
		- ((CASE WHEN INV12.State = 'PR' THEN 
							(CASE WHEN ISNULL(INV1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = INV1.ItemCode)*INV1.Quantity) ELSE INV1.U_cva_cmv * INV1.Quantity END )
						ELSE 
						(CASE WHEN OITM.U_RedST > 0 THEN dbo.FN_CVA_CUSTORENTABILIDADE (INV1.ItemCode)* INV1.Quantity ELSE (((CASE WHEN ISNULL(INV1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = INV1.ItemCode)) ELSE INV1.U_cva_cmv * INV1.Quantity END ))) END)  
				 END)) -- CMV
	    - (((CASE WHEN ISNULL(INV1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE INV1.U_cva_custovar END) + (CASE WHEN ISNULL(U_cva_comissao,0) = 0 THEN 0.00 ELSE U_cva_comissao END  ) + (dbo.fncArkabDocTaxPercentual(OINV.ObjType,OINV.DocEntry,'ICMS', INV1.LineNum)))/100 * (INV1.Quantity * INV1.Price))  -- CUSTO VARIAVEL
		- CASE WHEN ISNULL(INV1.U_cva_custofixo,0) = 0 THEN ((OITM.U_cva_custofixo/100) * (INV1.Quantity * INV1.Price)) ELSE (INV1.U_cva_custofixo/100) * (INV1.Quantity * INV1.Price) END -- VALOR CUSTO FIXO
		- (((CASE WHEN ISNULL(INV1.U_cva_porcustodia,0) = 0 THEN (Select top 1  (U_cva_custodia/100) from [@CVA_CUSTODIA]) ELSE INV1.U_cva_porcustodia/100 END)) * INV6.[Prazo Medio] * (INV1.Quantity * INV1.Price)) -- VALOR CUSTO DIA 
		)/(INV1.Quantity * INV1.Price))*100 AS 'PERRENTABILIDADE'

       FROM INV1
			INNER JOIN OINV  ON OINV.docentry = INV1.docentry
			INNER JOIN INV12 ON OINV.docentry = inv12.docentry
			LEFT  JOIN OSLP  ON OINV.SlpCode = OSLP.SlpCode
			LEFT  JOIN OUSG  ON OUSG.ID = INV1.Usage
			INNER  JOIN OITM ON OITM.ItemCode = INV1.ItemCode
			LEFT  JOIN [@CVA_LINHA]         ON [@CVA_LINHA].Code = OITM.U_linha
			LEFT  JOIN [@CVA_ITEM_SEGMENTO] ON [@CVA_ITEM_SEGMENTO].Code = OITM.U_segmento
			LEFT  JOIN [@CVA_ITEM_CLASS]    ON [@CVA_ITEM_CLASS].Code = OITM.U_classifitem
			LEFT  JOIN [@CVA_ACABAMENTO]    ON [@CVA_ACABAMENTO].Code = OITM.U_acabamento
			LEFT  JOIN [@CVA_ITEM_DISP]     ON [@CVA_ITEM_DISP].Code = OITM.U_disponibilidade
			LEFT  JOIN [@CVA_ITEM_SISTEMA]  ON [@CVA_ITEM_SISTEMA].Code = OITM.U_sistema
			LEFT  JOIN [@CVA_ITEM_MARCA]    ON [@CVA_ITEM_MARCA].Code = OITM.U_marca
			LEFT  JOIN OITB ON OITM.ItmsGrpCod = OITB.ItmsGrpCod
			INNER JOIN (SELECT INV6.DocEntry, AVG(DATEDIFF(DAY, OINV.DocDate, INV6.DueDate)) [Prazo Medio]
					FROM INV6 WITH(NOLOCK)
						INNER JOIN OINV WITH(NOLOCK)
							ON OINV.DocEntry = INV6.DocEntry
					GROUP BY INV6.DocEntry
				) INV6 ON INV6.DocEntry = OINV.DocEntry
			LEFT JOIN (SELECT Docentry,LineNum, SUM(FreteItem) AS FreteItem FROM cvafreight GROUP BY Docentry,LineNum ) cvafreight ON cvafreight.Docentry = INV1.DocEntry AND cvafreight.LineNum = INV1.LineNum
			LEFT JOIN OSHP   ON OINV.TrnspCode = OSHP.TrnspCode
			LEFT JOIN CVAMANAGER ON OINV.SlpCode = CVAMANAGER.salesPrson
			LEFT JOIN OCRD ON OCRD.Cardcode = OINV.Cardcode
			LEFT JOIN OOND ON OCRD.IndustryC = OOND.IndCode
			LEFT JOIN [@CVA_GRUPOECON] ON OCRD.U_CVA_GRUPOECON = [@CVA_GRUPOECON].CODE
			
		WHERE OINV.canceled = 'N'
			 AND OINV.slpcode in (select slpcode from OSLP)
			 AND ISNULL(OINV.SlpCode, 0) > 0
			 AND INV1.Usage = '9'
			 AND INV1.LineTotal > 0
			--AND INV1.DocEntry = '2891'
			 AND NOT EXISTS
			 (
					SELECT TOP 1 1 FROM [CVA_Portal].[dbo].CVA_MARKUP
					WHERE CVA_MARKUP.NRDOCUMENTO = INV1.DocEntry AND CVA_MARKUP.NRLINHA = INV1.LineNum AND CVA_MARKUP.DOCUMENTO = 'NOTA FISCAL DE SAIDA'
			 )

UNION ALL 

-----------------------------------------------------------------------------------------------------------------------------------
----------------------------------------DEVOLUÇÕES NOTAS FISCAIS DE SAIDA----------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------
SELECT 
		RIN1.Docentry           AS 'NRDOCUMENTO',
		'DEVOLUÇÃO'                 AS 'DOCUMENTO',
		ORIN.Serial                 AS 'NRNOTA',
		ORIN.CardCode               AS 'COD',
		ORIN.Cardname               AS 'NOMEPN',
	    OOND.IndName                AS 'PORTECLIENTE',
		[@CVA_GRUPOECON].Name       AS 'GRUPOECONOMICO',
		ORIN.DocDate                AS 'DATADOCUMENTO',
		RIN12.States                AS 'ESTADO',
		RIN12.CityS                 AS 'CIDADE',
		OSLP.SlpCode                AS 'IDVENDEDOR',
		OSLP.SlpName                AS 'VENDEDOR',
		(SELECT htm1.teamid
					FROM htm1
				   inner join ohtm
					  ON htm1.teamid = ohtm.teamid
				   WHERE htm1.empid = (SELECT OHEM.empid FROM OHEM WHERE OHEM.salesPrson = OSLP.SlpCode)
					 and ISNULL(htm1.U_akbactive, '') <> 'N'
				   ) AS 'IDEQUIPE', 
		ISNULL(
			   SUBSTRING(
			   (
				  SELECT ',[' + RTRIM(ohtm.name) + ']' as [text()]
					FROM htm1
				   inner join ohtm
					  ON htm1.teamid = ohtm.teamid
				   WHERE htm1.empid = (SELECT OHEM.empid FROM OHEM WHERE OHEM.salesPrson = OSLP.SlpCode)
					 and ISNULL(htm1.U_akbactive, '') <> 'N'
				   ORDER BY 1
				   FOR XML PATH('')
			   ), 2, 9999), '')      AS 'EQUIPE',
	    CVAMANAGER.Supervisor        AS 'GERENTE',    
	    OUSG.Usage                   AS 'UTILIZAÇÃO',
		RIN1.LineNum                 AS 'NRLINHA',
		[@CVA_LINHA].Code            AS 'IDLINHA',
		[@CVA_LINHA].Name            AS 'LINHA',
		[@CVA_ITEM_SEGMENTO].Code    AS 'IDSEGMENTO',
		[@CVA_ITEM_SEGMENTO].Name    AS 'SEGMENTO',
		[@CVA_ACABAMENTO].Code       AS 'IDACABAMENTO', 
		[@CVA_ACABAMENTO].Name       AS 'ACABAMENTO',
		[@CVA_ITEM_CLASS].Code       AS 'IDCLASSIF',
		[@CVA_ITEM_CLASS].Name       AS 'CLASSIF',
		[@CVA_ITEM_DISP].Code        AS 'IDDISPONIBILIDADE',
		[@CVA_ITEM_DISP].Name        AS 'DISPONIBILIDADE',
		[@CVA_ITEM_SISTEMA].Code     AS 'IDSISTEMA',
		[@CVA_ITEM_SISTEMA].Name     AS 'SISTEMA',
		[@CVA_ITEM_MARCA].Code       AS 'IDMARCA',
		[@CVA_ITEM_MARCA].Name       AS 'MARCA',
		OITB.ItmsGrpCod              AS 'IDGRUPO',
		OITB.ItmsGrpNam              AS 'GRUPO',
		RIN1.Weight1                 AS 'PESO',
		CASE WHEN OITM.U_CONTEUDO2 = 'L' THEN 
			CASE WHEN ISNULL(OITM.U_CONTEUDO1,'')='' THEN 1 ELSE 
				CASE WHEN ISNUMERIC(OITM.U_CONTEUDO1) = 1 THEN (replace(OITM.U_CONTEUDO1,',', '.')) * RIN1.Quantity ELSE ISNULL(OITM.U_CONTEUDO1,'0') END 
			END  
		WHEN OITM.U_CONTEUDO2 = 'G' THEN 
			CASE WHEN ISNULL(OITM.U_CONTEUDO1,'')='' THEN 1 ELSE 
				CASE WHEN ISNUMERIC(OITM.U_CONTEUDO1) = 1 THEN (((replace(OITM.U_CONTEUDO1,',', '.')) * RIN1.Quantity)/1000) / CASE WHEN dbo.fncCvaWebAPIGetDensity (RIN1.ItemCode) = 0.00 THEN dbo.fncCvaWebAPIGetDensityMaterialList (RIN1.ItemCode)
				ELSE dbo.fncCvaWebAPIGetDensity (RIN1.ItemCode) END  ELSE ISNULL(OITM.U_CONTEUDO1,'0') END	 
			END  	
		ELSE 
			CASE WHEN ISNUMERIC(OITM.U_CONTEUDO1) = 1 THEN ((replace(OITM.U_CONTEUDO1,',', '.'))* RIN1.Quantity) / CASE WHEN dbo.fncCvaWebAPIGetDensity (RIN1.ItemCode) = 0.00 THEN dbo.fncCvaWebAPIGetDensityMaterialList (RIN1.ItemCode) 
			ELSE dbo.fncCvaWebAPIGetDensity (RIN1.ItemCode) END  ELSE ISNULL(OITM.U_CONTEUDO1,'0') END	 
		END							 AS 'LITROS',
		0.00                         AS 'MEDLITROS',
		OITM.U_CONTEUDO1             AS 'CONTEUDO',
		RIN1.ItemCode                AS 'ITEM',
		RIN1.Dscription              AS 'DESCITEM',
		ISNULL(RIN1.U_AKBPRICE*-1,0) AS 'VLRORIGINAL',
		ISNULL(RIN1.U_akbdiscperc*-1,0)           AS 'DSCORIGINAL',
		ISNULL(RIN1.U_akbdiscvalue*-1,0)          AS 'VLRDSCORIGINAL',
		ORIN.DiscSum                 AS 'DESCONTOGERAL',
		ISNULL(RIN1.Price*-1,0)      AS 'VLRUNIT',
		RIN1.Quantity*-1             AS 'QUANTIDADE',
        (RIN1.Quantity * RIN1.Price)*-1   AS 'TOTAL',
		(SELECT  dbo.fncArkabDocTaxPercentual (ORIN.ObjType,ORIN.DocEntry,'ICMS', RIN1.LineNum))          AS 'PERICMS',
		ISNULL(RIN1.U_akbfreight*-1,0)            AS 'FRETE',
		''                                        AS 'TRANSPORTADORA',
		''                                        AS 'NOMETRANSP',
		''                                        AS 'CTE',
		ISNULL(RIN1.U_akbexpenses*-1,0)           AS 'ACRESCIMO',
		OSHP.TrnspName                            AS 'TIPOFRETE',
		ISNULL(RIN1.U_cva_fretemedio*-1,0)        AS 'FRETEMEDIO',
		ISNULL(RIN1.U_CVA_Frete_Estimado*-1,0)    AS 'FRETEESTIMADO',
		ISNULL((CASE WHEN RIN12.State = 'PR' THEN 
							(CASE WHEN ISNULL(RIN1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = RIN1.ItemCode)*RIN1.Quantity) ELSE RIN1.U_cva_cmv * RIN1.Quantity END )
						ELSE 
						(CASE WHEN ISNULL(OITM.U_RedST,0) > 0 THEN dbo.FN_CVA_CUSTORENTABILIDADE (RIN1.ItemCode)* RIN1.Quantity ELSE (((CASE WHEN ISNULL(RIN1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = RIN1.ItemCode)*RIN1.Quantity) ELSE RIN1.U_cva_cmv * RIN1.Quantity END ))) END)  
				 END)*-1,0)  AS 'CMV',
		ISNULL((RIN1.U_cva_comissao/100)*-1,0)          AS 'PERCOMISSÃO',
		CASE WHEN ISNULL(RIN1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE RIN1.U_cva_custovar END AS 'CUSTOVARIAVEL',
		(CASE WHEN ISNULL(RIN1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE RIN1.U_cva_custovar END) + (CASE WHEN ISNULL(U_cva_comissao,0) = 0 THEN 0.00 ELSE U_cva_comissao END  ) + (dbo.fncArkabDocTaxPercentual(ORIN.ObjType,ORIN.DocEntry,'ICMS', RIN1.LineNum)) AS 'PERCUSTOVARIAVEL',
		(((CASE WHEN ISNULL(RIN1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE RIN1.U_cva_custovar END) + (CASE WHEN ISNULL(U_cva_comissao,0) = 0 THEN 0.00 ELSE U_cva_comissao END  ) + (dbo.fncArkabDocTaxPercentual(ORIN.ObjType,ORIN.DocEntry,'ICMS', RIN1.LineNum)))/100 * (RIN1.Quantity * RIN1.Price))*-1    AS 'VLRCUSTOVARIAVEL',
		CASE WHEN ISNULL(RIN1.U_cva_custofixo,0) = 0 THEN OITM.U_cva_custofixo ELSE RIN1.U_cva_custofixo  END AS      'PERCUSTOFIXO',
		CASE WHEN ISNULL(RIN1.U_cva_custofixo,0) = 0 THEN ((OITM.U_cva_custofixo/100) * (RIN1.Quantity * RIN1.Price))*-1 ELSE (RIN1.U_cva_custofixo/100) * (RIN1.Quantity * RIN1.Price)*-1 END AS 'VLRCUSTOFIXO',

		((CASE WHEN ISNULL(RIN1.U_cva_porcustodia,0) = 0 THEN (Select top 1  (U_cva_custodia/100) from [@CVA_CUSTODIA]) ELSE RIN1.U_cva_porcustodia/100 END))       AS 'PERCUSTODIA',
		(((CASE WHEN ISNULL(RIN1.U_cva_porcustodia,0) = 0 THEN (Select top 1  (U_cva_custodia/100) from [@CVA_CUSTODIA]) ELSE RIN1.U_cva_porcustodia/100 END)) * RIN6.[Prazo Medio] * (RIN1.Quantity * RIN1.Price)) * -1 AS 'VLRCUSTODIA',
		RIN6.[Prazo Medio]                        AS 'PRAZOMEDIO',
		RIN6.[Prazo Medio] * (RIN1.Quantity * RIN1.Price)         AS 'PRAZOMEDIOCALULO',


		((RIN1.Quantity * RIN1.Price) -- VALOR MERCADORIAS
		- ISNULL(RIN1.U_akbfreight,0) -- FRETE
	    - (((CASE WHEN ISNULL(RIN1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE RIN1.U_cva_custovar END) + (CASE WHEN ISNULL(U_cva_comissao,0) = 0 THEN 0.00 ELSE U_cva_comissao END  ) + (dbo.fncArkabDocTaxPercentual(ORIN.ObjType,ORIN.DocEntry,'ICMS', RIN1.LineNum)))/100 * (RIN1.Quantity * RIN1.Price)) -- CUSTO VARIAVEL
		- (((CASE WHEN ISNULL(RIN1.U_cva_porcustodia,0) = 0 THEN (Select top 1  (U_cva_custodia/100) from [@CVA_CUSTODIA]) ELSE RIN1.U_cva_porcustodia/100 END)) * RIN6.[Prazo Medio] * (RIN1.Quantity * RIN1.Price)) -- VALOR CUSTO DIA 
		)*-1 AS  'VENDALIQUIDA',

		((RIN1.Quantity * RIN1.Price) -- VALOR MERCADORIAS
		- ISNULL(RIN1.U_akbfreight,0) -- FRETE
		- ((CASE WHEN RIN12.State = 'PR' THEN 
							(CASE WHEN ISNULL(RIN1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = RIN1.ItemCode)*RIN1.Quantity) ELSE RIN1.U_cva_cmv * RIN1.Quantity END )
						ELSE 
						(CASE WHEN OITM.U_RedST > 0 THEN dbo.FN_CVA_CUSTORENTABILIDADE (RIN1.ItemCode)* RIN1.Quantity ELSE (((CASE WHEN ISNULL(RIN1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = RIN1.ItemCode)*RIN1.Quantity) ELSE RIN1.U_cva_cmv * RIN1.Quantity END ))) END)  
				 END)) -- CMV
	    - (((CASE WHEN ISNULL(RIN1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE RIN1.U_cva_custovar END) + (CASE WHEN ISNULL(U_cva_comissao,0) = 0 THEN 0.00 ELSE U_cva_comissao END  ) + (dbo.fncArkabDocTaxPercentual(ORIN.ObjType,ORIN.DocEntry,'ICMS', RIN1.LineNum)))/100 * (RIN1.Quantity * RIN1.Price)) -- CUSTO VARIAVEL
		- (((CASE WHEN ISNULL(RIN1.U_cva_porcustodia,0) = 0 THEN (Select top 1  (U_cva_custodia/100) from [@CVA_CUSTODIA]) ELSE RIN1.U_cva_porcustodia/100 END)) * RIN6.[Prazo Medio] * (RIN1.Quantity * RIN1.Price)) -- VALOR CUSTO DIA 
		 ) * -1 AS 'VLRCONTRIBUICAO', 

		(((RIN1.Quantity * RIN1.Price) -- VALOR MERCADORIAS
		- ISNULL(RIN1.U_akbfreight,0) -- FRETE
		- ((CASE WHEN RIN12.State = 'PR' THEN 
							(CASE WHEN ISNULL(RIN1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = RIN1.ItemCode)*RIN1.Quantity) ELSE RIN1.U_cva_cmv * RIN1.Quantity END )
						ELSE 
						(CASE WHEN OITM.U_RedST > 0 THEN dbo.FN_CVA_CUSTORENTABILIDADE (RIN1.ItemCode)* RIN1.Quantity ELSE (((CASE WHEN ISNULL(RIN1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = RIN1.ItemCode) * RIN1.Quantity) ELSE RIN1.U_cva_cmv * RIN1.Quantity END ))) END)  
				 END))-- CMV
	    - (((CASE WHEN ISNULL(RIN1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE RIN1.U_cva_custovar END) + (CASE WHEN ISNULL(U_cva_comissao,0) = 0 THEN 0.00 ELSE U_cva_comissao END  ) + (dbo.fncArkabDocTaxPercentual(ORIN.ObjType,ORIN.DocEntry,'ICMS', RIN1.LineNum)))/100 * (RIN1.Quantity * RIN1.Price)) -- CUSTO VARIAVEL
		- (((CASE WHEN ISNULL(RIN1.U_cva_porcustodia,0) = 0 THEN (Select top 1  (U_cva_custodia/100) from [@CVA_CUSTODIA]) ELSE RIN1.U_cva_porcustodia/100 END)) * RIN6.[Prazo Medio] * (RIN1.Quantity * RIN1.Price))  -- VALOR CUSTO DIA 
		)/(RIN1.Quantity * RIN1.Price))*100
		  AS 'PERCONTRIBUICAO',

		((RIN1.Quantity * RIN1.Price) -- VALOR MERCADORIAS
		- ISNULL(RIN1.U_akbfreight,0)  -- FRETE
		- ((CASE WHEN RIN12.State = 'PR' THEN 
							(CASE WHEN ISNULL(RIN1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = RIN1.ItemCode)*RIN1.Quantity) ELSE RIN1.U_cva_cmv * RIN1.Quantity END )
						ELSE 
						(CASE WHEN OITM.U_RedST > 0 THEN dbo.FN_CVA_CUSTORENTABILIDADE (RIN1.ItemCode)* RIN1.Quantity ELSE (((CASE WHEN ISNULL(RIN1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = RIN1.ItemCode) * RIN1.Quantity) ELSE RIN1.U_cva_cmv * RIN1.Quantity END ))) END)  
				 END)) -- CMV
	     - (((CASE WHEN ISNULL(RIN1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE RIN1.U_cva_custovar END) + (CASE WHEN ISNULL(U_cva_comissao,0) = 0 THEN 0.00 ELSE U_cva_comissao END  ) + (dbo.fncArkabDocTaxPercentual(ORIN.ObjType,ORIN.DocEntry,'ICMS', RIN1.LineNum)))/100 * (RIN1.Quantity * RIN1.Price))  -- CUSTO VARIAVEL
		- CASE WHEN ISNULL(RIN1.U_cva_custofixo,0) = 0 THEN ((OITM.U_cva_custofixo/100) * (RIN1.Quantity * RIN1.Price)) ELSE (RIN1.U_cva_custofixo/100) * (RIN1.Quantity * RIN1.Price) END-- VALOR CUSTO FIXO
		- (((CASE WHEN ISNULL(RIN1.U_cva_porcustodia,0) = 0 THEN (Select top 1  (U_cva_custodia/100) from [@CVA_CUSTODIA]) ELSE RIN1.U_cva_porcustodia/100 END)) * RIN6.[Prazo Medio] * (RIN1.Quantity * RIN1.Price))  -- VALOR CUSTO DIA 
		 ) * -1 AS 'RENTABILIDADE',

		((((RIN1.Quantity * RIN1.Price) -- VALOR MERCADORIAS
		- ISNULL(RIN1.U_akbfreight,0) -- FRETE
		- ((CASE WHEN RIN12.State = 'PR' THEN 
							(CASE WHEN ISNULL(RIN1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = RIN1.ItemCode)*RIN1.Quantity) ELSE RIN1.U_cva_cmv * RIN1.Quantity END )
						ELSE 
						(CASE WHEN OITM.U_RedST > 0 THEN dbo.FN_CVA_CUSTORENTABILIDADE (RIN1.ItemCode)* RIN1.Quantity ELSE (((CASE WHEN ISNULL(RIN1.U_cva_cmv,0) = 0 THEN ((SELECT PRICE FROM ITM1 WHERE PriceList = 4 AND ItemCode = RIN1.ItemCode) * RIN1.Quantity) ELSE RIN1.U_cva_cmv * RIN1.Quantity END ))) END)  
				 END)) -- CMV
	    - (((CASE WHEN ISNULL(RIN1.U_cva_custovar,0) = 0 THEN OITM.U_cva_custovar ELSE RIN1.U_cva_custovar END) + (CASE WHEN ISNULL(U_cva_comissao,0) = 0 THEN 0.00 ELSE U_cva_comissao END  ) + (dbo.fncArkabDocTaxPercentual(ORIN.ObjType,ORIN.DocEntry,'ICMS', RIN1.LineNum)))/100 * (RIN1.Quantity * RIN1.Price))  -- CUSTO VARIAVEL
		- CASE WHEN ISNULL(RIN1.U_cva_custofixo,0) = 0 THEN ((OITM.U_cva_custofixo/100) * (RIN1.Quantity * RIN1.Price)) ELSE (RIN1.U_cva_custofixo/100) * (RIN1.Quantity * RIN1.Price) END -- VALOR CUSTO FIXO
		- (((CASE WHEN ISNULL(RIN1.U_cva_porcustodia,0) = 0 THEN (Select top 1  (U_cva_custodia/100) from [@CVA_CUSTODIA]) ELSE RIN1.U_cva_porcustodia/100 END)) * RIN6.[Prazo Medio] * (RIN1.Quantity * RIN1.Price)) -- VALOR CUSTO DIA 
		)/(RIN1.Quantity * RIN1.Price))*100) * -1 AS 'PERRENTABILIDADE'

       FROM RIN1
			INNER JOIN ORIN  ON ORIN.docentry = RIN1.docentry
			INNER JOIN RIN12 ON ORIN.docentry = RIN12.docentry
			LEFT  JOIN OSLP  ON ORIN.SlpCode = OSLP.SlpCode
			LEFT  JOIN OUSG  ON OUSG.ID = RIN1.Usage
			INNER  JOIN OITM ON OITM.ItemCode = RIN1.ItemCode
			LEFT  JOIN [@CVA_LINHA]         ON [@CVA_LINHA].Code = OITM.U_linha
			LEFT  JOIN [@CVA_ITEM_SEGMENTO] ON [@CVA_ITEM_SEGMENTO].Code = OITM.U_segmento
			LEFT  JOIN [@CVA_ITEM_CLASS]    ON [@CVA_ITEM_CLASS].Code = OITM.U_classifitem
			LEFT  JOIN [@CVA_ACABAMENTO]    ON [@CVA_ACABAMENTO].Code = OITM.U_acabamento
			LEFT  JOIN [@CVA_ITEM_DISP]     ON [@CVA_ITEM_DISP].Code = OITM.U_disponibilidade
			LEFT  JOIN [@CVA_ITEM_SISTEMA]  ON [@CVA_ITEM_SISTEMA].Code = OITM.U_sistema
			LEFT  JOIN [@CVA_ITEM_MARCA]    ON [@CVA_ITEM_MARCA].Code = OITM.U_marca
			LEFT  JOIN OITB ON OITM.ItmsGrpCod = OITB.ItmsGrpCod
			INNER JOIN (SELECT RIN6.DocEntry, AVG(DATEDIFF(DAY, ORIN.DocDate, RIN6.DueDate)) [Prazo Medio]
					FROM RIN6 WITH(NOLOCK)
						INNER JOIN ORIN WITH(NOLOCK)
							ON ORIN.DocEntry = RIN6.DocEntry
					GROUP BY RIN6.DocEntry
				) RIN6 ON RIN6.DocEntry = ORIN.DocEntry
			LEFT JOIN OSHP   ON ORIN.TrnspCode = OSHP.TrnspCode
			LEFT JOIN CVAMANAGER ON ORIN.SlpCode = CVAMANAGER.salesPrson
			LEFT JOIN OCRD ON OCRD.Cardcode = ORIN.Cardcode
			LEFT JOIN OOND ON OCRD.IndustryC = OOND.Indcode
			LEFT JOIN [@CVA_GRUPOECON] ON OCRD.U_CVA_GRUPOECON = [@CVA_GRUPOECON].CODE	
		WHERE ORIN.canceled = 'N'
			 AND ORIN.slpcode in (select slpcode from OSLP)
			 AND ISNULL(ORIN.SlpCode, 0) > 0
			 AND RIN1.Usage = '9'
			 AND RIN1.LineTotal > 0
			 --AND ORIN.docentry = 37


			 AND NOT EXISTS
			 (
					SELECT TOP 1 1 FROM [CVA_Portal].[dbo].CVA_MARKUP
					WHERE CVA_MARKUP.NRDOCUMENTO = RIN1.DocEntry AND CVA_MARKUP.NRLINHA = RIN1.LineNum AND CVA_MARKUP.DOCUMENTO = 'DEVOLUÇÃO'
			 )
