
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
DECLARE @TotalExpectedInBaseline as int
DECLARE @TotalFoundInBaseline as int

-- FIRST set @OUTPUT_Id to 0 (ZERO) so that default is set to value false.
SET @OUTPUT_Success = 0; -- 1 means true and 0 means false

SET @TotalExpectedInBaseline = (SELECT count(1) from 
		(Select DISTINCT(RTRIM(LTRIM(value))) as value from 
			(SELECT value FROM STRING_SPLIT( @BaselineItemzIds, ','))
		as valuelist) as tableWithBaselineItemzs)

SET @TotalFoundInBaseline =  (select COUNT(1)
		FROM BaselineItemz as bi
		LEFT JOIN BaselineItemzTypeJoinBaselineItemz as bitjbi on bitjbi.BaselineItemzId = bi.id
		LEFT JOIN BaselineItemzType as tbl_bit on tbl_bit.Id = bitjbi.BaselineItemzTypeId
		LEFT JOIN Baseline as b on b.Id = tbl_bit.BaselineId 
		WHERE  bi.id in (
				SELECT DISTINCT(RTRIM(LTRIM(value))) from (
					SELECT value FROM STRING_SPLIT( @BaselineItemzIds, ',') 
					) as valuelist
				)
			AND (b.id = @BaselineId) 
			AND NOT (b.id IS NULL)
			)
IF (@TotalExpectedInBaseline = @TotalFoundInBaseline)
	BEGIN
		IF (@ShouldBeIncluded = 1 OR @ShouldBeIncluded = 0)
		BEGIN 
			UPDATE BaselineItemz
			SET isIncluded = @ShouldBeIncluded
			WHERE Id in (Select DISTINCT(RTRIM(LTRIM(value))) from ( SELECT value FROM STRING_SPLIT( @BaselineItemzIds, ',') ) as valuelist)
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
		BEGIN
		RAISERROR (N'Could not find one or more BaselineItemzs within target Baseline', -- Message text.  
					16, -- Severity.  
					1 -- State.  
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
