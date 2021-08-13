// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace ItemzApp.API.Models
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
        /// Id of the BaselineItemz represented by a GUID.
        /// </summary>
        public IEnumerable<Guid>? BaselineItemzIds { get; set; }
    }
}
