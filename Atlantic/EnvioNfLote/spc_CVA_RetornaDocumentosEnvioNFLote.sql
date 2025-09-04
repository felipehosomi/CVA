CREATE PROCEDURE [dbo].[spc_CVA_RetornaDocumentosEnvioNFLote](@bplId INT, @dataInicial DATETIME = null, @dataFinal DATETIME = null, @documentoInicial INT = null, @documentoFinal INT = null, @status INT = null)
AS
BEGIN
	CREATE TABLE #tbAux
	(
		DocEntry INT, 
		CardCode NVARCHAR(50), 
		CardName NVARCHAR(100), 
		DocDate DATETIME, 
		Serial INT, 
		DocTotal NUMERIC(19,6), 
		EmailPadrao NVARCHAR(MAX), 
		Email NVARCHAR(MAX)
	)

	INSERT INTO #tbAux
	SELECT DISTINCT 
		T0.DocEntry, 
		T0.CardCode, 
		T0.CardName, 
		T0.DocDate, 
		T0.Serial, 
		T0.DocTotal, 
		T2.E_Mail EmailPadrao, 
		CASE WHEN T2.CntctPrsn = T3.Name THEN NULL ELSE T3.E_MailL END AS Email
	FROM OINV T0 
		INNER JOIN INV1 T1 ON T0.DocEntry = T1.DocEntry AND T1.TargetType <> N'14' 
		INNER JOIN OCRD T2 ON T0.CardCode = T2.CardCode 
		LEFT  JOIN OCPR T3 ON T2.CardCode = T3.CardCode
		INNER JOIN [@SKL25NFE] T4 ON T0.DocEntry = T4.U_DocEntry AND T4.U_tipoDocumento = 'NS' AND T4.U_ChaveAcesso IS NOT NULL
	WHERE T0.CANCELED = 'N' AND T0.BPLId = @bplId
	AND ((T0.DocDate >= @dataInicial OR @dataInicial IS NULL) AND (T0.DocDate <= @dataFinal OR @dataFinal IS NULL))
	AND ((T0.DocEntry >= @documentoInicial OR @documentoInicial IS NULL) AND (T0.DocEntry <= @documentoFinal OR @documentoFinal IS NULL))
	AND (ISNULL(T0.U_CVA_DocEnviado, 0) = @status OR @status IS NULL)

	SELECT DISTINCT T0.DocEntry, T0.CardCode, T0.CardName, T0.DocDate, T0.Serial, T0.DocTotal, T0.EmailPadrao, STUFF((SELECT ';' + T1.Email FROM #tbAux T1 WHERE T1.DocEntry = T0.DocEntry ORDER BY T1.DocEntry FOR XML PATH('')),1,1,'') AS EmailsList
	FROM #tbAux T0 WHERE T0.EmailPadrao IS NOT NULL GROUP BY T0.DocEntry, T0.CardCode, T0.CardName, T0.DocDate, T0.Serial, T0.DocTotal, T0.EmailPadrao

	DROP TABLE #tbAux
END