// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ItemzApp.API.Models
{
    public class ItemzTraceDTO
    {
        /// <summary>
        /// Id of the From Trace Itemz representated by a GUID.
        /// </summary>
        public Guid FromTraceItemzId { get; set; }

        /// <summary>
        /// Id of the To Trace Itemz representated by a GUID.
        /// </summary>
        public Guid ToTraceItemzId { get; set; }
    }
}
