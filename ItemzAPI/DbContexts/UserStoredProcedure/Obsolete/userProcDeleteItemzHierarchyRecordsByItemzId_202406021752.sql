
IF OBJECT_ID ( 'userProcDeleteItemzHierarchyRecordsByItemzId', 'P' ) IS NOT NULL
    DROP PROCEDURE userProcDeleteItemzHierarchyRecordsByItemzId
GO

CREATE PROCEDURE userProcDeleteItemzHierarchyRecordsByItemzId
@ItemzId [uniqueidentifier],
@OUTPUT_Success [bit] OUT

AS

BEGIN
BEGIN TRY


DECLARE @ItemzAsRoot hierarchyID;
DECLARE @RecordTypeToBeItemz NVARCHAR(128)
DECLARE @HierarchyLevel int
DECLARE @ItemzHierarchyRowCount int

-- FIRST set @OUTPUT_Success to 0 (ZERO) so that default is set to value false.
SET @OUTPUT_Success = 0; -- 1 means true and 0 means false

SELECT @ItemzAsRoot = ItemzHierarchyId
		, @RecordTypeToBeItemz = RecordType
		,  @HierarchyLevel = ItemzHierarchyId.GetLevel()
FROM ItemzHierarchy
WHERE id = @ItemzId

set @ItemzHierarchyRowCount = @@rowcount

-- IT IS POSSIBLE THAT ITEMZ ID BASED HIERARCHY RECORD WAS NOT PRESENT BECAUSE ITEMZ IS AN ORPHAND ITEMZ.
-- THIS IS THE REASON WHY WE SIMPLY RETURN BACK FROM THE STORED PROCEDURE WHEN RECORD COUNT IS ZERO

If(@ItemzHierarchyRowCount = 0)
	BEGIN
		RETURN
	END
END TRY
BEGIN CATCH
	RETURN
END CATCH

BEGIN TRY
BEGIN TRANSACTION  -- NOW THE ACTUAL TRANSACTION BEGINS WHEN RECORD COUNT IS MORE THEN 0

-- WE ARE NOT GOING TO RAISE ERROR WHEN RowCount is not exactly 1 for ITEMZ because
-- WE MAY HAVE ORPHANED ITEMZ WHICH IS NOT PART OF ANY PROJECT OR ITEMZ TYPE
-- AND IT STILL NEEDS TO BE DELETED.

--If (@ItemzHierarchyRowCount <> 1) -- If RowCount is not exactly 1 then raise error
--	BEGIN
--		SET @OUTPUT_Success = 0
--		BEGIN
--		RAISERROR (N'Root element in ItemzHierarchy is not exactly one. Check if the ItemzHierarchy record is found for the given Itemz ID in the Database.', -- Message text.  
--					16, -- Severity.  
--					1 -- State.  
--					)
--		END
--		SET @OUTPUT_Success = 1
--	END


IF (@HierarchyLevel = 0)
	BEGIN
		SET @OUTPUT_Success = 0
		BEGIN
		-- TODO improve this error message as how we have done it in itemz type user procedure
		RAISERROR (N'Actual record found is a root record without any parents. System is not designed to delete the parent most [root] hierarchy record as it is expected to be of type ''Repository''' -- Message text.  
					, 16 -- Severity.  
					, 1 -- State.  
					)
		END
		SET @OUTPUT_Success = 1
	END	
IF (@RecordTypeToBeItemz <> 'Itemz')
	BEGIN
		SET @OUTPUT_Success = 0
		BEGIN
		-- TODO improve this error message as how we have done it in itemz type user procedure
		RAISERROR (N'Actual record found by provided ItemzId within ItemzHierarchy is not of Type ''Itemz'' instead it is of type ''%s''' -- Message text.  
					, 16 -- Severity.  
					, 1 -- State.  
					, @RecordTypeToBeItemz
					)
		END
		SET @OUTPUT_Success = 1
	END


-- DELETE ALL RECORDS WITHIN ITMEZ HIERARCHY FOR ALL DECENDENTS OF A GIVEN ITEMZ AS ROOT.

DELETE FROM ItemzHierarchy 
WHERE ItemzHierarchyId.IsDescendantOf(@ItemzAsRoot) = 1

COMMIT TRANSACTION
END TRY
BEGIN CATCH

IF @@TRANCOUNT > 0
	ROLLBACK TRAN --RollBack in case of Error

DECLARE @ErrorMessage NVARCHAR(4000);  
DECLARE @ErrorSeverity INT;  
DECLARE @ErrorState INT;  
  
SELECT   
    @ErrorMessage = ERROR_MESSAGE(),  
    @ErrorSeverity = ERROR_SEVERITY(),  
    @ErrorState = ERROR_STATE();  
  
-- Use RAISERROR inside the CATCH block to return error  
-- information about the original error that caused  
-- execution to jump to the CATCH block.  
RAISERROR (@ErrorMessage, -- Message text.  
            @ErrorSeverity, -- Severity.  
            @ErrorState -- State.  
            );  
END CATCH
END
