// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

//using ItemzApp.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItemzApp.WebUI.Client.SharedModels
{
    /// <summary>
    /// UpdateItemzDTO shall be used for sending in request for updating
    /// existing Itemz. It will expose necessary properties to allow existing Itemz 
    /// to be updated with new values for those properties.
    /// </summary>
    public class UpdateItemzDTO : ManipulateItemzDTO
    {

    }
}
