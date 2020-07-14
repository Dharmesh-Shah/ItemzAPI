﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace ItemzApp.API.Models
{
    /// <summary>
    /// ManipulateItemzTypeDTO is used for updating ItemzType
    /// </summary>
    public class ManipulateItemzTypeDTO
    {
        /// <summary>
        /// Name of the ItemzType
        /// </summary>
        [Required] // Such attributes are important as they are used for Validating incoming API calls.
        [MaxLength(128)]
        public string Name { get; set; }
        /// <summary>
        /// Status of the ItemzType
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Status { get; set; }
        /// <summary>
        /// Description of the ItemzType
        /// </summary>
        [MaxLength(1028)]
        public string Description { get; set; }
    }
}
