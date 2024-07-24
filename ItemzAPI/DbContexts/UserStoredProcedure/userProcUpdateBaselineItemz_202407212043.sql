
IF OBJECT_ID ( 'userProcUpdateBaselineItemz', 'P' ) IS NOT NULL
    DROP PROCEDURE userProcUpdateBaselineItemz
GO

CREATE PROCEDURE userProcUpdateBaselineItemz
@BaselineId [uniqueidentifier],
@ShouldBeIncluded [bit],
@BaselineItemzIds [varchar](max),
@OUTPUT_Success [bit] out

AS

BEGIN
BEGIN TRY
DECLARE @TempInputBaselineItemzIterator int
DECLARE @TempInputBaselineItemzNumberOfRows int
DECLARE @FoundInputBaselineItemzWithinBaseline int
DECLARE @CurrentInputBaselineItemzId [uniqueidentifier] 

DECLARE @TempInputBaselineItemzIdsTable TABLE (
	idx int Primary Key IDENTITY(1,1),
	TempInputBaselineItemzId [uniqueidentifier])

-- FIRST set @OUTPUT_Id to 0 (ZERO) so that default is set to value false.
SET @OUTPUT_Success = 0; -- 1 means true and 0 means false
SET @TempInputBaselineItemzIterator = 1
SET @TempInputBaselineItemzNumberOfRows = 0

INSERT into @TempInputBaselineItemzIdsTable (TempInputBaselineItemzId)
SELECT DISTINCT(RTRIM(LTRIM(value))) from (
					SELECT value FROM STRING_SPLIT( @BaselineItemzIds, ',') 
					) as TempInputBaselineItemzId

SET @TempInputBaselineItemzNumberOfRows = (SELECT COUNT(1) FROM @TempInputBaselineItemzIdsTable)

IF @TempInputBaselineItemzNumberOfRows > 0
	WHILE (@TempInputBaselineItemzIterator < = @TempInputBaselineItemzNumberOfRows)
	BEGIN
		SET @CurrentInputBaselineItemzId = (SELECT TempInputBaselineItemzId from @TempInputBaselineItemzIdsTable WHERE idx = @TempInputBaselineItemzIterator)

		SET @FoundInputBaselineItemzWithinBaseline = (SELECT count(1) from BaselineItemzHierarchy
														where id = @CurrentInputBaselineItemzId
														AND BaselineItemzHierarchyId.GetLevel() > 3
														AND RecordType = 'BaselineItemz'
														AND BaselineItemzHierarchyId.IsDescendantOf(
														(select BaselineItemzHierarchyiD FROM BaselineItemzHierarchy 
															WHERE id = @BaselineId
															AND RecordType = 'Baseline'
															AND BaselineItemzHierarchyId.GetLevel() = 2
														) ) = 1)

		IF (@FoundInputBaselineItemzWithinBaseline = 1)
		BEGIN
			IF (@ShouldBeIncluded = 1 OR @ShouldBeIncluded = 0)
				BEGIN 
					UPDATE BaselineItemzHierarchy
					SET isIncluded = @ShouldBeIncluded
					where BaselineItemzHierarchyId.IsDescendantOf(
						(select BaselineItemzHierarchyiD FROM BaselineItemzHierarchy WHERE id = @CurrentInputBaselineItemzId) ) = 1

					UPDATE BaselineItemz
					SET isIncluded = @ShouldBeIncluded
					Where id in ( select id
									from BaselineItemzHierarchy 
									where BaselineItemzHierarchyId.IsDescendantOf(
										(select BaselineItemzHierarchyiD FROM BaselineItemzHierarchy WHERE id = @CurrentInputBaselineItemzId) ) = 1
								)

				SET @TempInputBaselineItemzIterator = @TempInputBaselineItemzIterator + 1
				END
			ELSE
				BEGIN
					SET @OUTPUT_Success = 0
					RAISERROR (N'Value for isIncluded column shall either be 0 (ZERO) or 1 (ONE)', -- Message text.  
								16, -- Severity.  
								1 -- State.  
								)			
				END

			SET @OUTPUT_Success = 1
		END
		ELSE
		BEGIN
			SET @OUTPUT_Success = 0
			
			DECLARE @CurrentInputBaselineItemzIdAsString as NVARCHAR(36) = CAST(@CurrentInputBaselineItemzId AS NVARCHAR(36))
			DECLARE @BaselineIdAsString as NVARCHAR(36) = CAST(@BaselineId AS NVARCHAR(36))
			RAISERROR (N'Could not find BaselineItemz with ID %s as child of Baseline with ID %s', -- Message text.  
						16, -- Severity.  
						1, -- State.  
						@CurrentInputBaselineItemzIdAsString,
						@BaselineIdAsString
						)			
		END
	END
END TRY
BEGIN CATCH

--  TODO: TRANSACTION ROLLBACK LOGIC SHOULD BE COVERED VIA WEB API LAYER AS WE ARE BATCHING CHANGES.
--	WE HAVE TO OTHERWISE EVALUATE NESTED TRANSACTION LOG.

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
