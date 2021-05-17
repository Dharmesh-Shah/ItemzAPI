using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    }
}
