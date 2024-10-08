
IF OBJECT_ID ( 'userProcCreateBaselineByItemzTypeID', 'P' ) IS NOT NULL
		DROP PROCEDURE userProcCreateBaselineByItemzTypeID
GO

CREATE PROCEDURE userProcCreateBaselineByItemzTypeID
@ProjectId [uniqueidentifier],
@ItemzTypeId [uniqueidentifier],
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
DECLARE @CurrentBaselineItemzTypeID [uniqueidentifier]
DECLARE @CurrentItemzTypeID [uniqueidentifier]
DECLARE @FoundItemzTypeInItemzHierarchy hierarchyid

	INSERT into [dbo].[Baseline] (Id, Name, Description, CreatedBy, ProjectId)
	VALUES (@NewBaselineID, @Name, @Description, @CreatedBy, @ProjectId)


	INSERT into [dbo].[BaselineItemzType] (ItemzTypeId, Name, Status, Description, CreatedBy,CreatedDate,IsSystem,BaselineId)	
	SELECT Id, Name, Status, Description, CreatedBy, CreatedDate, IsSystem, @NewBaselineID
	FROM [dbo].[ItemzTypes]
	WHERE ProjectId=@ProjectId and Id = @ItemzTypeId

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

	IF @TempBaselineItemzTypeNumberOfRows > 0
		WHILE (@TempBaselineItemzTypeIterator <= @TempBaselineItemzTypeNumberOfRows)
		BEGIN
			-- get the next BaselineItemzType
			SET @CurrentBaselineItemzTypeID = (SELECT TempBaselineItemzTypeID from  @TempBaselineItemzType WHERE idx = @TempBaselineItemzTypeIterator)
			SET @CurrentItemzTypeID = (SELECT TempItemzTypeID from  @TempBaselineItemzType WHERE idx = @TempBaselineItemzTypeIterator)
			
			-- Insert records in BaselineItemz from Itemzs table.

			-- EXPLANATION: Local declare of variables within WHILE block

			SET @FoundItemzTypeInItemzHierarchy = (Select ItemzHierarchyId FROM [dbo].[ItemzHierarchy] where Id = @CurrentItemzTypeID)

			-- EXPLANATION: We are explicitely looking for Itemz within ItemzType where GetLevel() is EQUAL to 3. 
			-- This way, we only insert immediate child of ItemzType instead of entire tree. This is important because
			-- we need to establish BaselineItemz and BaselineItemzTypeJoinBaselinItemz relationship 
			-- before inserting remaining Itemzchild hierarchy tree nodes. 

			INSERT into [dbo].[BaselineItemz] (ItemzId, Name, Status, Priority, Description, CreatedBy,CreatedDate,Severity,IgnoreMeBaselineItemzTypeId,isIncluded)   
			SELECT itz.Id, itz.Name, itz.Status, itz.Priority, itz.Description, itz.CreatedBy, itz.CreatedDate, itz.Severity , @CurrentBaselineItemzTypeID, 1 as isIncluded
			FROM [dbo].[Itemzs] as itz
			WHERE itz.Id in ( 
								SELECT ith.id
								FROM [dbo].[ItemzHierarchy] as ith
								WHERE ItemzHierarchyId.IsDescendantOf(@FoundItemzTypeInItemzHierarchy) = 1 
								and RecordType = 'Itemz' 
								and ith.ItemzHierarchyId.GetLevel() = 3
							)

			--EXPLANATION: Marking following INSERT into statement as commented becauase we are now switching over to ItemzHierarchy to find all tree Itemz nodes below a given ItemzType.
			--INSERT into [dbo].[BaselineItemz] (ItemzId, Name, Status, Priority, Description, CreatedBy,CreatedDate,Severity,IgnoreMeBaselineItemzTypeId,isIncluded)   
			--SELECT itz.Id, itz.Name, itz.Status, itz.Priority, itz.Description, itz.CreatedBy, itz.CreatedDate, itz.Severity , @CurrentBaselineItemzTypeID, 1 as isIncluded
			--FROM [dbo].[Itemzs] as itz
			--LEFT JOIN [dbo].[ItemzTypeJoinItemz] as itji ON itji.ItemzId = itz.id
			--WHERE itji.ItemzTypeId=@CurrentItemzTypeID

			-- Insert records into BaselineItemzTypeJoinBaselineItemz
			-- EXPLAINATION: Because we have just added records in 
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

			-- EXPLANATION: We are explicitely looking for Itemz within ItemzType where GetLevel() is GREATER THEN 3. 
			-- This way, we insert remaining child node tree Itemz into BaselineItemz. 

			INSERT into [dbo].[BaselineItemz] (ItemzId, Name, Status, Priority, Description, CreatedBy,CreatedDate,Severity,IgnoreMeBaselineItemzTypeId,isIncluded)   
			SELECT itz.Id, itz.Name, itz.Status, itz.Priority, itz.Description, itz.CreatedBy, itz.CreatedDate, itz.Severity , @CurrentBaselineItemzTypeID, 1 as isIncluded
			FROM [dbo].[Itemzs] as itz
			WHERE itz.Id in ( 
								SELECT ith.id
								FROM [dbo].[ItemzHierarchy] as ith
								WHERE ItemzHierarchyId.IsDescendantOf(@FoundItemzTypeInItemzHierarchy) = 1 
								and RecordType = 'Itemz' 
								and ith.ItemzHierarchyId.GetLevel() > 3
							)

			--increment Number of BaselineItemz created in this iteration of BaselineType
			SET @TempBaselineItemzNumberOfRows = @TempBaselineItemzNumberOfRows + ( 
					SELECT count(1) 
					FROM [dbo].[BaselineItemz] AS blitz
					Where blitz.IgnoreMeBaselineItemzTypeId =  @CurrentBaselineItemzTypeID)

			-- increment counter for next employee
			SET @TempBaselineItemzTypeIterator = @TempBaselineItemzTypeIterator + 1
		END

		---- Inserting BaselineItemzJoinItemzTraces

		INSERT INTO [dbo].[BaselineItemzJoinItemzTrace] (BaselineFromItemzId,BaselineToItemzId,BaselineId)
		SELECT bi1.Id as BaselineItemzFromIdTrace , bi2.id as BaselineItemzToIdTrace , @NewBaselineID as NewBaselineID
		FROM [dbo].[ItemzJoinItemzTrace] ijit
		INNER JOIN BaselineItemz bi1 on bi1.ItemzId = ijit.FromItemzId
		INNER JOIN BaselineItemz bi2 on bi2.ItemzId = ijit.ToItemzId
		Where ijit.FromItemzId in (
			select BI.ItemzId from BaselineItemz bi
			INNER join BaselineItemzTypeJoinBaselineItemz bitjbi on bitjbi.BaselineItemzId = bi.Id
			INNER JOIN BaselineItemzType baselineit on baselineit.id = bitjbi.BaselineItemzTypeId
			INNER JOIN Baseline b on b.id = baselineit.BaselineId
			where BaselineID = @NewBaselineID
			)
			AND
			ijit.ToItemzId in (
			select BI.ItemzId from BaselineItemz bi
			INNER JOIN BaselineItemzTypeJoinBaselineItemz bitjbi on bitjbi.BaselineItemzId = bi.Id
			INNER JOIN BaselineItemzType baselineit on baselineit.id = bitjbi.BaselineItemzTypeId
			INNER JOIN Baseline b on b.id = baselineit.BaselineId
			where BaselineID = @NewBaselineID
			)
			AND 
			bi1.Id in (
			select BI.Id from BaselineItemz bi
			INNER JOIN BaselineItemzTypeJoinBaselineItemz bitjbi on bitjbi.BaselineItemzId = bi.Id
			INNER JOIN BaselineItemzType baselineit on baselineit.id = bitjbi.BaselineItemzTypeId
			INNER JOIN Baseline b on b.id = baselineit.BaselineId
			where BaselineID = @NewBaselineID
			)
			AND
			bi2.Id in (
			select BI.Id from BaselineItemz bi
			INNER JOIN BaselineItemzTypeJoinBaselineItemz bitjbi on bitjbi.BaselineItemzId = bi.Id
			INNER JOIN BaselineItemzType baselineit on baselineit.id = bitjbi.BaselineItemzTypeId
			INNER JOIN Baseline b on b.id = baselineit.BaselineId
			where BaselineID = @NewBaselineID
			)

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
