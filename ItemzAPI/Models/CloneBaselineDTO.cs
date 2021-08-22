// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;

namespace ItemzApp.API.Models
{
    /// <summary>
    /// CloneBaselineDTO shall be used for sending in request for creating new Baseline 
    /// based on existing baseline by cloning the same.
    /// It will expose necessary properties to allow successful creation of the Baseline.
    /// </summary>
    public class CloneBaselineDTO : ManipulateBaselineDTO
    {
        /// <summary>
        /// Source Baseline's Id for creating new baseline by cloning it.
        /// </summary>
        [Required]
        public Guid BaselineId { get; set; }
    }
}
