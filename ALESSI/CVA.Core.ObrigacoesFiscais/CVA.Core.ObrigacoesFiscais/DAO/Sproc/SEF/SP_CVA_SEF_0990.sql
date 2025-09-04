IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_CVA_SEF_0990')
	DROP PROCEDURE SP_CVA_SEF_0990
GO

-- exec SP_CVA_SEF_0990 1, '2018-01-01', '2018-31-01'
CREATE PROCEDURE SP_CVA_SEF_0990
(
	@Filial			INT,
	@DataDe			DATETIME,
	@DataAte		DATETIME
)
AS
BEGIN
	DECLARE @RowCount INT = 1

	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_0000 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_0001 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_0005 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_0030 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_0100 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_0150 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_0400 (@Filial, @DataDe, @DataAte)
	SELECT @RowCount = @RowCount + COUNT(*) FROM FN_CVA_SEF_0450 (@Filial, @DataDe, @DataAte)
	
	SELECT @RowCount QTL_LIN
END