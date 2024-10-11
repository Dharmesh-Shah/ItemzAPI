// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace ItemzApp.WebUI.Client.SharedModels
{
    /// <summary>
    /// BaselineItemz are mainly readonly objects. 
    /// Purpose of this UpdateBaselineItemzDTO is to allow setting property for 
    /// inclusion or exclusion of BaselineItemz from a given baseline itself.
    /// </summary>
    public class UpdateBaselineItemzDTO
    {
        /// <summary>
        /// Id of the Baseline represented by a GUID
        /// </summary>
        public Guid BaselineId { get; set; }
        /// <summary>
        /// True if action is to include BaselineItemzs otherwise False
        /// </summary>
        public bool ShouldBeIncluded;
        /// <summary>
        /// True if action is to include only Single BaselineItemzs Node without its child breakdown structure nodes. This property will be ignored when ShouldBeIncluded is set to false.
        /// </summary>
        public bool SingleNodeInclusion = false;
        /// <summary>
        /// Id of the BaselineItemz represented by a GUID.
        /// </summary>
        public IEnumerable<Guid>? BaselineItemzIds { get; set; }
    }
}
