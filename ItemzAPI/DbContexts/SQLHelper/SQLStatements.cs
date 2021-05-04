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
    }
}
