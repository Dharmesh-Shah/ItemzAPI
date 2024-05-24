
IF OBJECT_ID ( 'userProcDeleteItemzHierarchyRecordsByProjectId', 'P' ) IS NOT NULL
    DROP PROCEDURE userProcDeleteItemzHierarchyRecordsByProjectId
GO

CREATE PROCEDURE userProcDeleteItemzHierarchyRecordsByProjectId
@ProjectId [uniqueidentifier],
@OUTPUT_Success [bit] OUT

AS

BEGIN
BEGIN TRY
BEGIN TRANSACTION

-- FIND ITEMZ HIERARCHY RECORD BY PROJECT ID 
-- IT SHOULD ONLY BE ONE UNIQUE RECORD IN THE ITEMZ HIERARCHY DB
-- BECAUSE id FIELD IN ItemzHierarchy TABLE IS A PRIMARY KEY...
-- WHEN WE SEARCH BY A SINGLE @ProjectID THEN ONLY ONE RECORD SHOULD BE RETURNED

DECLARE @ProjectAsRoot hierarchyID;
DECLARE @RecordTypeToBeProject NVARCHAR(128)
declare @ItemzHierarchyRowCount int

-- FIRST set @OUTPUT_Id to 0 (ZERO) so that default is set to value false.
SET @OUTPUT_Success = 0; -- 1 means true and 0 means false


SELECT @ProjectAsRoot = ItemzHierarchyId, @RecordTypeToBeProject = RecordType
FROM ItemzHierarchy
WHERE id = @ProjectId

set @ItemzHierarchyRowCount = @@rowcount

If (@ItemzHierarchyRowCount <> 1) -- If RowCount is not exactly 1 then raise error
	BEGIN
		SET @OUTPUT_Success = 0
		BEGIN
		RAISERROR (N'Root element in ItemzHierarchy is not exactly one. Check if the ItemzHierarchy record is found for the given Project ID in the Database.', -- Message text.  
					16, -- Severity.  
					1 -- State.  
					)
		END
		SET @OUTPUT_Success = 1
	END

IF (@RecordTypeToBeProject <> 'Project')
	BEGIN
		SET @OUTPUT_Success = 0
		BEGIN
		RAISERROR (N'Found record in ItemzHierarchy is not of Type Project', -- Message text.  
					16, -- Severity.  
					1 -- State.  
					)
		END
		SET @OUTPUT_Success = 1
	END


-- DELETE ALL RECORDS WITHIN ITMEZ HIERARCHY FOR ALL DECENDENTS OF A GIVEN PROJECT AS ROOT.

DELETE FROM ItemzHierarchy 
WHERE ItemzHierarchyId.IsDescendantOf(@ProjectAsRoot) = 1

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
