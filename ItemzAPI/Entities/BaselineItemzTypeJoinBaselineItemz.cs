// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;

namespace ItemzApp.API.Entities
{
    public class BaselineItemzTypeJoinBaselineItemz
    {
        public Guid BaselineItemzTypeId { get; set; }
        public BaselineItemzType? BaselineItemzType { get; set; }

        public Guid BaselineItemzId { get; set; }
        public BaselineItemz? BaselineItemz { get; set; }
    }
}
