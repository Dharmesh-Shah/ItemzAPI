// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemzApp.API.Entities
{
    /// <summary>
    /// Itemz Severity list values as ENUM
    /// </summary>
    // [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemzSeverity
    {
        High = 1 , Medium = 2, Low = 3
    }
}
