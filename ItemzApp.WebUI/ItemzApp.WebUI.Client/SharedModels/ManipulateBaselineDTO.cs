﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace ItemzApp.WebUI.Client.SharedModels
{
    /// <summary>
    /// ManipulateBaselineDTO is used for updating Baseline
    /// </summary>
  
    public class ManipulateBaselineDTO
    {
        /// <summary>
        /// Name or Title of the Baseline
        /// </summary>
        [Required] // Such attributes are important as they are used for Validating incoming API calls.
        [MaxLength(128)]
        public string? Name { get; set; }
        /// <summary>
        /// Description of the Baseline
        /// </summary>
        [MaxLength(1028)]
        public string? Description { get; set; }
    }
}
