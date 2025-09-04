IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_SEF_E990')
	DROP PROCEDURE SP_CVA_SEF_E990
GO

-- exec SP_CVA_SEF_E990 1, '2018-01-01', '2018-01-31'
CREATE PROCEDURE SP_CVA_SEF_E990
(
	@Filial			INT,
	@DataDe			DATETIME,
	@DataAte		DATETIME
)
AS
BEGIN
	DECLARE @RowCount INT = 1

	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E001 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E020 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E100 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E105 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E120 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E300 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E310 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E330 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E340 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E350 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E360 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E500 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E520 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E525 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E540 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_E560 (@Filial, @DataDe, @DataAte)

	SELECT @RowCount QTL_LIN_E
END