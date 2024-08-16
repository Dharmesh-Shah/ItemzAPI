// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ItemzApp.API.Entities
{
    public class BaselineItemzJoinItemzTrace
    {
        public Guid BaselineFromItemzId { get; set; }
        public virtual BaselineItemz? BaselineFromItemz { get; set; }

        public Guid BaselineToItemzId { get; set; }
        public virtual BaselineItemz? BaselineToItemz { get; set; }

        public Guid BaselineId { get; set; }
    }
}
