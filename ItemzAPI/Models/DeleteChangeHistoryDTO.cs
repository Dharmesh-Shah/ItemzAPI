// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

#nullable enable

namespace ItemzApp.API.Models
{
    public class BaseChangeHistoryDTO
    {
        /// <summary>
        /// itemzId of the Itemz representated by a GUID.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Date and Time upto which Itemz Change History data has to be deleted for given ItemzId.
        /// </summary>
        public DateTimeOffset UptoDateTime { get; set; }
    }

    public class DeleteChangeHistoryDTO : BaseChangeHistoryDTO
    {
    }

    public class GetNumberOfChangeHistoryDTO : BaseChangeHistoryDTO
    {
    }
}

#nullable disable
