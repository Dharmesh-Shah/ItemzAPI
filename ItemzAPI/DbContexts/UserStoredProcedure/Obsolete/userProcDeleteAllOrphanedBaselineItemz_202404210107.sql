
IF OBJECT_ID ( 'userProcDeleteAllOrphanedBaselineItemz', 'P' ) IS NOT NULL
    DROP PROCEDURE userProcDeleteAllOrphanedBaselineItemz
GO

CREATE PROCEDURE userProcDeleteAllOrphanedBaselineItemz
AS
BEGIN
BEGIN TRY
BEGIN TRANSACTION

-- DELETE BASELINE ITEMZ TRACES FOR THOSE ITEMZ WHERE 
-- ORPHANED BASELINE ITEMZ IS A PARENT - FROM TRACES
DELETE FROM [dbo].[BaselineItemzJoinItemzTrace] 
where [dbo].[BaselineItemzJoinItemzTrace].BaselineFromItemzId IN
    (SELECT BI.Id
    FROM [dbo].[BaselineItemz] as bi 
    LEFT JOIN [dbo].[BaselineItemzTypeJoinBaselineItemz] as bitjbi 
    ON bitjbi.BaselineItemzId = bi.id 
    WHERE bitjbi.BaselineItemzId IS NULL)

-- DELETE BASELINE ITEMZ TRACES FOR THOSE ITEMZ WHERE 
-- ORPHANED BASELINE ITEMZ IS A CHILD - TO TRACES
DELETE FROM [dbo].[BaselineItemzJoinItemzTrace] 
where [dbo].[BaselineItemzJoinItemzTrace].BaselineToItemzId IN
    (SELECT BI.Id
    FROM [dbo].[BaselineItemz] as bi 
    LEFT JOIN [dbo].[BaselineItemzTypeJoinBaselineItemz] as bitjbi 
    ON bitjbi.BaselineItemzId = bi.id 
    WHERE bitjbi.BaselineItemzId IS NULL)

Delete from [dbo].[BaselineItemz] 
where [dbo].[BaselineItemz].id IN 
    (SELECT bi.id 
    FROM [dbo].[BaselineItemz] as bi 
    LEFT JOIN [dbo].[BaselineItemzTypeJoinBaselineItemz] as bitjbi 
    ON bitjbi.BaselineItemzId = bi.id 
    WHERE bitjbi.BaselineItemzId IS NULL)

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
