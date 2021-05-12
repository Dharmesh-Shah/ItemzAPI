// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

#nullable enable

namespace ItemzApp.API.Models
{
    public class ItemzTypeItemzDTO
    {
        /// <summary>
        /// Id of the Itemz representated by a GUID.
        /// </summary>
        public Guid ItemzId { get; set; }

        /// <summary>
        /// Id of the ItemzType representated by a GUID.
        /// </summary>
        public Guid ItemzTypeId { get; set; }

    }
}

#nullable disable
