// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


using System.ComponentModel.DataAnnotations;

namespace ItemzApp.WebUI.Client.SharedModels
{
    /// <summary>
    /// CreateItemzTypeDTO shall be used for sending in request for creating new ItemzType.
    /// It will expose necessary properties to allow successful creation of the ItemzType.
    /// </summary>
    public class CreateItemzTypeDTO : ManipulateItemzTypeDTO
    {
        /// <summary>
        /// Project ID in the Guid form. 
        /// New ItemzType shall be created this project.
        /// </summary>
        [Required] // Such attributes are important as they are used for Validating incoming API calls.
        public System.Guid ProjectId { get; set; }
    }
}
