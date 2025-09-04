ALTER PROCEDURE SBO_SP_PostTransactionNotice
(
	in object_type nvarchar(30), 				-- SBO Object Type
	in transaction_type nchar(1),			-- [A]dd, [U]pdate, [D]elete, [C]ancel, C[L]ose
	in num_of_cols_in_key int,
	in list_of_key_cols_tab_del nvarchar(255),
	in list_of_cols_val_tab_del nvarchar(255)
)
LANGUAGE SQLSCRIPT
AS
-- Return values
error  int;				-- Result (0 for no error)
error_message nvarchar (200); 		-- Error string to be displayed
begin

error := 0;
error_message := N'Ok';

--------------------------------------------------------------------------------------------------------------------------------

--	ADD	YOUR	CODE	HERE

--------------------------------------------------------------------------------------------------------------------------------

--------------------------------------------------------------------------------------------------------------------------------
-- CVA Add-on Consignação
--------------------------------------------------------------------------------------------------------------------------------
IF object_type = '14' THEN
	DECLARE canceled nvarchar(1);
	DECLARE draftEntry int;
	DECLARE draftKey int;
	
	SELECT "CANCELED", "ImportEnt", "draftKey" INTO canceled, draftEntry, draftKey FROM ORIN WHERE "DocEntry" = list_of_cols_val_tab_del;
	IF canceled = 'C' OR canceled = 'Y' THEN
		DELETE FROM "@CVA_CONSIGNMENT" WHERE "U_DraftEntry" IN (COALESCE(draftEntry, 0), COALESCE(draftKey, 0));
	END IF;
END IF;

IF object_type = '112' AND transaction_type = 'D' THEN
	DELETE FROM "@CVA_CONSIGNMENT" WHERE "U_DraftEntry" = list_of_cols_val_tab_del;
END IF;

-- Select the return values
select :error, :error_message FROM dummy;

end;