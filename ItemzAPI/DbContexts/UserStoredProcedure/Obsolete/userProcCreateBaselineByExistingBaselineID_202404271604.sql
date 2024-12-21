
IF OBJECT_ID ( 'userProcCreateBaselineByExistingBaselineID', 'P' ) IS NOT NULL
    DROP PROCEDURE userProcCreateBaselineByExistingBaselineID
GO

CREATE PROCEDURE userProcCreateBaselineByExistingBaselineID
@BaselineId [uniqueidentifier],
@Name [nvarchar](128),
@Description [nvarchar](1028),
@CreatedBy [nvarchar](128) = N'Some User',
@OUTPUT_Id [uniqueidentifier] out

AS

-- FIRST set @OUTPUT_Id to be Empty uniqueidentifier.
SET @OUTPUT_Id = (SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER))

BEGIN
BEGIN TRY
BEGIN TRANSACTION
DECLARE @NewBaselineID [uniqueidentifier]
SET @NewBaselineID = NEWID()

DECLARE @TempBaselineItemzTypeIterator int
DECLARE @TempBaselineItemzTypeNumberOfRows int
DECLARE @TempBaselineItemzNumberOfRows int
DECLARE @TempBaselineItemzNumberOfTrace int
DECLARE @TempNewlyCreatedBaselineItemzNumberOfTrace int
DECLARE @CurrentBaselineItemzTypeID [uniqueidentifier]
DECLARE @CurrentItemzTypeID [uniqueidentifier]

	INSERT into [dbo].[Baseline] (Id, Name, Description, CreatedBy, ProjectId)
	SELECT @NewBaselineID, @Name, @Description, @CreatedBy, ProjectId from Baseline where id = @BaselineId


	INSERT into [dbo].[BaselineItemzType] (ItemzTypeId, Name, Status, Description, CreatedBy,CreatedDate,IsSystem,BaselineId)	
	SELECT ItemzTypeId, Name, Status, Description, CreatedBy, CreatedDate, IsSystem, @NewBaselineID
	FROM [dbo].[BaselineItemzType]
	WHERE BaselineId = @BaselineId


	DECLARE @TempBaselineItemzType TABLE (
		idx int Primary Key IDENTITY(1,1),
		TempBaselineItemzTypeID [uniqueidentifier],
		TempItemzTypeID [uniqueidentifier])

	INSERT @TempBaselineItemzType
	SELECT Id, ItemzTypeId from [dbo].[BaselineItemzType] WHERE BaselineId=@NewBaselineId

	-- Enumerate through @TempBaselineItemzType
	SET @TempBaselineItemzTypeIterator = 1
	SET @TempBaselineItemzTypeNumberOfRows = (SELECT COUNT(*) FROM @TempBaselineItemzType)
	SET @TempBaselineItemzNumberOfRows = 0
	SET @TempBaselineItemzNumberOfTrace = 0
	set @TempNewlyCreatedBaselineItemzNumberOfTrace = 0

	IF @TempBaselineItemzTypeNumberOfRows > 0
		WHILE (@TempBaselineItemzTypeIterator <= @TempBaselineItemzTypeNumberOfRows)
		BEGIN
			-- get the next BaselineItemzType
			SET @CurrentBaselineItemzTypeID = (SELECT TempBaselineItemzTypeID from  @TempBaselineItemzType WHERE idx = @TempBaselineItemzTypeIterator)
			SET @CurrentItemzTypeID = (SELECT TempItemzTypeID from  @TempBaselineItemzType WHERE idx = @TempBaselineItemzTypeIterator)
			
			-- Insert records in BaselineItemz from BaselineItemzItemz table itself.
			INSERT into [dbo].[BaselineItemz] (ItemzId, Name, Status, Priority, Description, CreatedBy,CreatedDate,Severity,IgnoreMeBaselineItemzTypeId,isIncluded)   
			SELECT blitz.ItemzId, blitz.Name, blitz.Status, blitz.Priority, blitz.Description, blitz.CreatedBy, blitz.CreatedDate, blitz.Severity , @CurrentBaselineItemzTypeID, blitz.isIncluded
			FROM [dbo].[BaselineItemz] as blitz
			LEFT JOIN BaselineItemzTypeJoinBaselineItemz as bitjbi on bitjbi.BaselineItemzId = blitz.Id
			LEFT JOIN BaselineItemzType as tbl_bit on tbl_bit.id = bitjbi.BaselineItemzTypeId
			WHERE tbl_bit.BaselineId = @BaselineId
				AND tbl_bit.ItemzTypeId = @CurrentItemzTypeID
				AND blitz.id IS NOT NULL
				AND tbl_bit.ID IS NOT NULL


			-- Insert records into BaselineItemzTypeJoinBaselineItemz
			-- EXPLANATION: Because we have just added records in 
			-- BaselineItemz table that included details about BaselineItemzType as
			-- part of [dbo].[BaselineItemz].IgnoreMeBaselineItemzTypeId column, we are
			-- now able to run a simple Select Querty as part of INSERT INTO command
			-- that identifies all the latest records that were added into BaselineItemz. 
			-- Remember that BaselineItemz table auto generates GUID for it's ID column. This ID
			-- is unknown to the Stored Procedure and instead of creating temporary table
			-- we decided to include a new column called as IgnoreMeBaselineItemzTypeId. Now we
			-- are able to query the actual BaselineItemz that were added as part of the currently
			-- processed Itemz.

			INSERT INTO [dbo].[BaselineItemzTypeJoinBaselineItemz] (BaselineItemzTypeId,BaselineItemzId)
			SELECT blitz.IgnoreMeBaselineItemzTypeId, blitz.id
			FROM [dbo].[BaselineItemz] AS blitz
			Where blitz.IgnoreMeBaselineItemzTypeId = @CurrentBaselineItemzTypeID
			
			--increment Number of BaselineItemz created in this iteration of BaselineType
			SET @TempBaselineItemzNumberOfRows = @TempBaselineItemzNumberOfRows + ( 
					SELECT count(1) 
					FROM [dbo].[BaselineItemz] AS blitz
					Where blitz.IgnoreMeBaselineItemzTypeId =  @CurrentBaselineItemzTypeID)

			-- increment counter for next BaselineItemzTypeIterator
			SET @TempBaselineItemzTypeIterator = @TempBaselineItemzTypeIterator + 1
		END

		/* START copying BaselineItemz TRACE data from source baseline to new target baseline */

		SET @TempBaselineItemzNumberOfTrace = 
			(
			Select count(1) 
			from BaselineItemzJoinItemzTrace bijit
				INNER JOIN dbo.BaselineItemzTypeJoinBaselineItemz bitjbi_01 
					on bitjbi_01.BaselineItemzId = bijit.BaselineFromItemzId 
				INNER JOIN dbo.BaselineItemzTypeJoinBaselineItemz bitjbi_02 
					on bitjbi_02.BaselineItemzId = bijit.BaselineToItemzId 
				INNER JOIN 
					(
					SELECT bit_inner_01.Id 
					FROM dbo.BaselineItemzType bit_inner_01
					WHERE bit_inner_01.BaselineId = @BaselineId
					) bit_01 
					on bit_01.Id = bitjbi_01.BaselineItemzTypeId
				INNER JOIN 
					(
					SELECT bit_inner_02.Id 
					FROM dbo.BaselineItemzType bit_inner_02
					WHERE bit_inner_02.BaselineId = @BaselineId
					) bit_02 
					on bit_02.Id = bitjbi_02.BaselineItemzTypeId
			)


		IF @TempBaselineItemzNumberOfTrace > 0
		BEGIN 
			INSERT into [dbo].[BaselineItemzJoinItemzTrace] (BaselineFromItemzId,BaselineToItemzId, BaselineId)
			SELECT 
				NewFromData.NewFromId AS NewBaselineFromId,
				NewToData.NewToId AS NewBaselineToId,
				@NewBaselineID
			FROM BaselineItemzJoinItemzTrace bijit
			INNER JOIN 
				(
					SELECT bi_Inner_01.Id,bi_Inner_01.ItemzId -- Projection of REQUIRED columns
					FROM dbo.BaselineItemz bi_Inner_01
				) bi_01 
			ON bi_01.Id = bijit.BaselineFromItemzId
			INNER JOIN 
				(
					SELECT bi_Inner_02.Id,bi_Inner_02.ItemzId -- Projection of REQUIRED columns
					FROM dbo.BaselineItemz bi_Inner_02
				) bi_02 
			ON bi_02.id = bijit.BaselineToItemzId
			INNER JOIN dbo.BaselineItemzTypeJoinBaselineItemz bitjbi_01 
				ON bitjbi_01.BaselineItemzId = bijit.BaselineFromItemzId 
			INNER JOIN dbo.BaselineItemzTypeJoinBaselineItemz bitjbi_02 
				ON bitjbi_02.BaselineItemzId = bijit.BaselineToItemzId 
			INNER JOIN 
				(
					SELECT bit_inner_01.Id					-- Projection of REQUIRED columns
					FROM dbo.BaselineItemzType bit_inner_01
					WHERE bit_inner_01.BaselineId = @BaselineId -- SOURCE BASELINE ID
				) bit_01 
				ON bit_01.Id = bitjbi_01.BaselineItemzTypeId
			INNER JOIN 
				(
					SELECT bit_inner_02.Id					-- Projection of REQUIRED columns
					FROM dbo.BaselineItemzType bit_inner_02
					WHERE bit_inner_02.BaselineId = @BaselineId -- SOURCE BASELINE ID
				) bit_02 
				ON bit_02.Id = bitjbi_02.BaselineItemzTypeId
			INNER JOIN 
				(
					SELECT 
						NewFrom.Id AS NewFromId, 
						NewFrom.ItemzId AS NewFromItemzId, 
						NewFrom.Name 
					FROM BaselineItemz NewFrom 
					INNER JOIN dbo.BaselineItemzTypeJoinBaselineItemz bitjbi_NewFrom 
						ON bitjbi_NewFrom.BaselineItemzId = NewFrom.Id
					INNER JOIN dbo.BaselineItemzType bit_NewFrom 
						ON bit_NewFrom.Id = bitjbi_NewFrom.BaselineItemzTypeId
					WHERE bit_NewFrom.BaselineId = @NewBaselineID -- NEW TARGET BASELINE ID
				) NewFromData 
				ON NewFromData.NewFromItemzId = bi_01.ItemzId
			INNER JOIN 
				(
					SELECT 
						NewTo.Id AS NewToId, 
						NewTo.ItemzId AS NewToItemzId, 
						NewTo.Name 
					FROM BaselineItemz NewTo 
					INNER JOIN dbo.BaselineItemzTypeJoinBaselineItemz bitjbi_NewTo 
						ON bitjbi_NewTo.BaselineItemzId = NewTo.Id
					INNER JOIN dbo.BaselineItemzType bit_NewTo 
						ON bit_NewTo.Id = bitjbi_NewTo.BaselineItemzTypeId
					WHERE bit_NewTo.BaselineId = @NewBaselineID -- NEW TARGET BASELINE ID
				) NewToData 
				ON NewToData.NewToItemzId = bi_02.ItemzId
		END

		SET @TempNewlyCreatedBaselineItemzNumberOfTrace = 
			(
				Select count(1) 
				from BaselineItemzJoinItemzTrace bijit
					INNER JOIN dbo.BaselineItemzTypeJoinBaselineItemz bitjbi_01 
						on bitjbi_01.BaselineItemzId = bijit.BaselineFromItemzId 
					INNER JOIN dbo.BaselineItemzTypeJoinBaselineItemz bitjbi_02 
						on bitjbi_02.BaselineItemzId = bijit.BaselineToItemzId 
					INNER JOIN 
						(
						SELECT bit_inner_01.Id 
						FROM dbo.BaselineItemzType bit_inner_01
						WHERE bit_inner_01.BaselineId = @NewBaselineID
						) bit_01 
						on bit_01.Id = bitjbi_01.BaselineItemzTypeId
					INNER JOIN 
						(
						SELECT bit_inner_02.Id 
						FROM dbo.BaselineItemzType bit_inner_02
						WHERE bit_inner_02.BaselineId = @NewBaselineID
						) bit_02 
						on bit_02.Id = bitjbi_02.BaselineItemzTypeId
			)


		IF (@TempBaselineItemzNumberOfTrace != @TempNewlyCreatedBaselineItemzNumberOfTrace)
		BEGIN
		RAISERROR (N'Number of traces between old baseline and new baseline did not match and so rolling back creation of new baseline', -- Message text.  
					16, -- Severity.  
					1 -- State.  
					)
		END

if @TempBaselineItemzNumberOfRows = 0
	BEGIN
	RAISERROR (N'ZERO Number of Itemzs records found for the new baseline and so cancelling Baseline Creation operation', -- Message text.  
				16, -- Severity.  
				1 -- State.  
				)
	END

SET @OUTPUT_Id = @NewBaselineID
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
