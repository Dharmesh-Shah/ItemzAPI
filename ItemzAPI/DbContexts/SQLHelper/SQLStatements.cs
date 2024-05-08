namespace ItemzApp.API.DbContexts.SQLHelper
{
    public static class SQLStatements
    {
        #region ItemzChangeHistoryByItemzType

        public static readonly string SQLStatementFor_ItemzChangeHistoryByItemzType =
            "select count(ItemzId) from ItemzChangeHistory " +
            "where ItemzId in (select distinct(ItemzId) " +
            "from ItemzTypeJoinItemz where ItemzTypeId = @__ItemzTypeId__)";

        public static readonly string SQLStatementFor_ItemzChangeHistoryByItemzTypeWithUptoDateTime  = 
            "select count(ItemzId) from ItemzChangeHistory " +
            "where CreatedDate < @__GetUptoDateTime__ " +
            "and ItemzId in " +
            "(select distinct(ItemzId) from ItemzTypeJoinItemz " +
            "where ItemzTypeId = @__ItemzTypeId__)";

        #endregion ItemzChangeHistoryByItemzType

        #region ItemzChangeHistoryByProject

        public static readonly string SQLStatementFor_ItemzChangeHistoryByProject = 
            "select count(ItemzId) from ItemzChangeHistory " +
            "where ItemzId in (select distinct(ItemzId) " +
            "from ItemzTypeJoinItemz " +
            "where ItemzTypeId in " +
            "(select ItemzTypes.Id from ItemzTypes " +
            "where ProjectId = @__ProjectId__))";

        public static readonly string SQLStatementFor_ItemzChangeHistoryByProjectWithUptoDateTime =
            "select count(ItemzId) from ItemzChangeHistory " +
            "where CreatedDate < @__GetUptoDateTime__ " +
            "and ItemzId in " +
            "(select distinct(ItemzId) from ItemzTypeJoinItemz " +
            "where ItemzTypeId in " +
            "(select ItemzTypes.Id from ItemzTypes " +
            "where ProjectId = @__ProjectId__))";

        #endregion ItemzChangeHistoryByProject

        #region ItemzChangeHistoryByRepository

        public static readonly string SQLStatementFor_ItemzChangeHistoryByRepository =
            "select count(ItemzId) from ItemzChangeHistory";

        public static readonly string SQLStatementFor_ItemzChangeHistoryByRepositoryWithUptoDateTime =
            "select count(ItemzId) from ItemzChangeHistory " +
            "where CreatedDate < @__GetUptoDateTime__ ";

        #endregion ItemzChangeHistoryByRepository

        #region ProjectItemzCount

        public static readonly string SQLStatementFor_GetItemzCountByProject = 
            "select count(Id) from Itemzs " +
            "where Id in (select distinct(ItemzId) " +
            "from ItemzTypeJoinItemz " +
            "where ItemzTypeId in (select distinct(Id) from ItemzTypes " +
            "where ProjectId = @__ProjectID__))";

        #endregion ProjectItemzCount

        #region ItemzTypeItemzCount

        public static readonly string SQLStatementFor_GetItemzCountByItemzType =
            "select count(Id) from Itemzs " +
            "where Id in (select distinct(ItemzId) " +
            "from ItemzTypeJoinItemz " +
            "where ItemzTypeId = @__ItemzTypeID__)";

        #endregion ItemzTypeItemzCount

        #region GetBaselineItemz

        public static readonly string SQLStatementFor_GetBaselineItemzByItemzIdOrderByCreatedDate =
                "SELECT [bi].* " +
                "FROM [dbo].[Baseline] as [b] " +
                "LEFT JOIN [dbo].[BaselineItemzType] as [tbl_bit] on [tbl_bit].BaselineId = [b].Id " +
                "LEFT JOIN [dbo].[BaselineItemzTypeJoinBaselineItemz] as [bitjbi] on [bitjbi].BaselineItemzTypeId = [tbl_bit].Id " +
                "LEFT JOIN [dbo].[BaselineItemz] as [bi] on [bi].Id = [bitjbi].BaselineItemzId " +
                "WHERE [bi].ItemzId = @__ItemzID__ AND [bi].id IS NOT NULL " +
                "Order By [b].CreatedDate ";


        #endregion GetBaselineItemz

        #region BaselineItemzCount

        public static readonly string SQLStatementFor_GetBaselineItemzTraceCountByBaseline =
            "SELECT count(1) as BaselineItemzTraceCount " +
            "from BaselineItemzJoinItemzTrace bijit " +
            "WHERE bijit.BaselineFromItemzId in " +
                "(SELECT bi.id from BaselineItemz bi " +
                    "INNER JOIN BaselineItemzTypeJoinBaselineItemz bitjbi " +
                    "on bitjbi.BaselineItemzId = bi.Id " +
                    "INNER JOIN BaselineItemzType bitype " +
                    "on bitype.id = bitjbi.BaselineItemzTypeId " +
                    "INNER JOIN Baseline b on b.id = bitype.BaselineId " +
                    "WHERE b.id = @__BaselineID__  " +
                    "AND bi.isIncluded = @__IsIncluded_IsTrue__ )" +
            "AND " +
            "bijit.BaselineToItemzId in " +
                "(SELECT bi.id from BaselineItemz bi " +
                    "INNER JOIN BaselineItemzTypeJoinBaselineItemz bitjbi " +
                    "on bitjbi.BaselineItemzId = bi.Id " +
                    "INNER JOIN BaselineItemzType bitype " +
                    "on bitype.id = bitjbi.BaselineItemzTypeId " +
                    "INNER JOIN Baseline b on b.id = bitype.BaselineId " +
                    "WHERE b.id = @__BaselineID__ " +
                    "AND bi.isIncluded = @__IsIncluded_IsTrue__ )";

        public static readonly string SQLStatementFor_GetIncludedBaselineItemzCountByBaseline =

            "select count(Id) from BaselineItemz " +
            "where isIncluded = 1 AND Id in (select distinct(BaselineItemzId) " +
            "from BaselineItemzTypeJoinBaselineItemz " +
            "where BaselineItemzTypeId in ( " +
                "select distinct(Id) from BASELINEITEMZTYPE " +
                "where BaselineId = @__BaselineID__) "+ 
                ")";
        
        public static readonly string SQLStatementFor_GetExcludedBaselineItemzCountByBaseline =

            "select count(Id) from BaselineItemz " +
            "where isIncluded = 0 AND Id in (select distinct(BaselineItemzId) " +
            "from BaselineItemzTypeJoinBaselineItemz " +
            "where BaselineItemzTypeId in ( " +
                "select distinct(Id) from BASELINEITEMZTYPE " +
                "where BaselineId = @__BaselineID__) " +
                ")";

        // NOTE: With respect to SQLStatementFor_GetBaselineItemzCountByBaseline
        // System is ignoring IsIncluded property of BaselineItemz while cloning. 
        // i.e. we copy all BaselineItemz without filtering data out based on IsIncluded
        public static readonly string SQLStatementFor_GetBaselineItemzCountByBaseline =

            "select count(Id) from BaselineItemz " +
            "where Id in (select distinct(BaselineItemzId) " +
            "from BaselineItemzTypeJoinBaselineItemz " +
            "where BaselineItemzTypeId in ( " +
                "select distinct(Id) from BASELINEITEMZTYPE " +
                "where BaselineId = @__BaselineID__) " +
                ")";

        public static readonly string SQLStatementFor_GetBaselineItemzByItemzId =
            "select count(Id) from [dbo].[BaselineItemz] as bi " +
            "where bi.ItemzId = @__ItemzId__";

        public static readonly string SQLStatementFor_GetTotalBaselineItemzInRepository =
            "select count(1) from BaselineItemz";

        #endregion BaselineItemzCount

        #region BaselineItemzCountByBaselineItemzType

        public static readonly string SQLStatementFor_GetBaselineItemzCountByBaselineItemzType =
            "select count(Id) from BaselineItemz " +
            "where Id in (select distinct(BaselineItemzId) " +
            "from BaselineItemzTypeJoinBaselineItemz " +
            "where BaselineItemzTypeId = @__BaselineItemzTypeID__ )";

        #endregion BaselineItemzCountByBaselineItemzType

        #region Baseline_OrphanedBaseilneItemzCount

        public static readonly string SQLStatementFor_GetOrphanedBaselineItemzCount =
            "SELECT COUNT(bi.id) " +
            "FROM [dbo].[BaselineItemz] as bi " +
            "LEFT JOIN[dbo].[BaselineItemzTypeJoinBaselineItemz] as bitjbi " +
            "ON bitjbi.BaselineItemzId = bi.id " +
            "WHERE bitjbi.BaselineItemzId IS NULL";
            
        #endregion Baseline_OrphanedBaseilneItemzCount

    }
}
