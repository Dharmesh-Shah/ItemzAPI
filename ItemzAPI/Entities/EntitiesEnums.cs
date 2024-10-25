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
		// EXPLANATION - We have added Medium as 0 (ZERO) becauase that's the default value to be used by EF Core if no value is provided for Severity
		// This is called sentinel value as far as .NET compilation warning is concerned.
        // See comment by use David Liang at ... https://stackoverflow.com/a/77978854
		Medium = 0, High = 1 , Low = 2  
    }
}
