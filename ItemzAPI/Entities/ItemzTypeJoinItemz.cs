// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

# nullable enable

namespace ItemzApp.API.Entities
{
    public class ItemzTypeJoinItemz
    {
        public Guid ItemzTypeId { get; set; }
        public ItemzType? ItemzType { get; set; }

        public Guid ItemzId { get; set; }
        public Itemz? Itemz { get; set; }
    }
}

# nullable disable
