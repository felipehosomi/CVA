IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'P' AND name = 'spc_CVA_Balancete')
	DROP PROCEDURE spc_CVA_Balancete
GO
CREATE procedure [dbo].[spc_CVA_Balancete]  
  @caixa char(1) = 'N',
  @saldoInicial	numeric(19, 2)
AS
BEGIN
	SELECT 'Saldo' 'AcctName', @saldoInicial 'Saldo'
	
	UNION ALL

	SELECT 
		OACT.AcctCode  + ' ' + upper(OACT.AcctName) 'AcctName',
		OACT.CurrTotal as 'Saldo'  
	FROM OACT   
	WHERE( OACT.FINANSE = 'Y' )
END