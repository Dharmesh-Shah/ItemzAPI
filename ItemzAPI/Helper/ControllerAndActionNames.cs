// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ItemzApp.API.Helper
{
    public static class ControllerAndActionNames
    {
        public static string GetFormattedControllerAndActionNames(ControllerContext controllerContext)
        {
            if(controllerContext.ActionDescriptor.ControllerName is not null &&
                controllerContext.ActionDescriptor.ActionName is not null)
                return $"::{controllerContext.ActionDescriptor.ControllerName}-{controllerContext.ActionDescriptor.ActionName}:: ";
            
            return "";
        }
    }
}
#nullable disable

