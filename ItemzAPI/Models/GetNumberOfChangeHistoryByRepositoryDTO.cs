// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ItemzApp.API.Models
{
    public class GetNumberOfChangeHistoryByRepositoryDTO
    {
        /// <summary>
        /// Date and Time upto which Itemz Change History data has to be deleted for given ItemzId.
        /// </summary>
        public DateTimeOffset UptoDateTime { get; set; }
    }
}
