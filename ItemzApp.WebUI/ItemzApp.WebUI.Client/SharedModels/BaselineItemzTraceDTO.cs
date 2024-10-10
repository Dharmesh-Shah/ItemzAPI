// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ItemzApp.WebUI.Client.SharedModels
{
    public class BaselineItemzTraceDTO
    {
        /// <summary>
        /// Id of the From Trace Baseline Itemz representated by a GUID.
        /// </summary>
        public Guid FromTraceBaselineItemzId { get; set; }

        /// <summary>
        /// Id of the To Trace Baseline Itemz representated by a GUID.
        /// </summary>
        public Guid ToTraceBaselineItemzId { get; set; }
    }
}
