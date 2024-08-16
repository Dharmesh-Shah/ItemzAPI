
IF OBJECT_ID ( 'userProcDeleteSingleItemzByItemzID', 'P' ) IS NOT NULL
    DROP PROCEDURE userProcDeleteSingleItemzByItemzID
GO

CREATE PROCEDURE userProcDeleteSingleItemzByItemzID
@ItemzId [uniqueidentifier]
AS
BEGIN
BEGIN TRY
BEGIN TRANSACTION

-- DELETE ITEMZ TRACES FOR THOSE ITEMZ WHERE 
-- ITEMZ IS A PARENT - FROM TRACES
DELETE FROM [dbo].[ItemzJoinItemzTrace] 
where [dbo].[ItemzJoinItemzTrace].FromItemzId = @ItemzId
    
-- DELETE ITEMZ TRACES FOR THOSE ITEMZ WHERE 
-- ITEMZ IS A PARENT - TO TRACES
DELETE FROM [dbo].[ItemzJoinItemzTrace] 
where [dbo].[ItemzJoinItemzTrace].ToItemzId = @ItemzId

-- NOW FINALLY DELETE Itemz itself.   
DELETE FROM [dbo].[Itemzs] 
where [dbo].[Itemzs].Id = @ItemzID

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
