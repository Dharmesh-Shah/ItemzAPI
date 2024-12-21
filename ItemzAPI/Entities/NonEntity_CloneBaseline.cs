﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItemzApp.API.Entities
{
    /// <summary>
    /// NonEntity_CloneBaseline is used to pass in necessary properties to custom User Stored Procedure
    /// via repository service that clones existing baseline to create a new Baseline from it.
    /// 
    /// This is NOT used for mapping entities to the database via EF Core DBContext. 
    /// 
    /// We are making sure that we follow the convention of not letting DTO be sent directly to the 
    /// Database for processing user request. For now, CloneBaselineDTO and NonEntity_CloneBaseline has 
    /// similar properties and we keep them seperated to make sure that we follow necessary best practices.
    /// </summary>
    public class NonEntity_CloneBaseline
    {
        /// <summary>
        /// Source Baseline's Id for creating new baseline by cloning it.
        /// </summary>
        [Required]
        public Guid BaselineId { get; set; }

        /// <summary>
        /// Name or Title of the Baseline
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string? Name { get; set; }

		/// <summary>
		/// Description of the Baseline
		/// </summary>
		[Column(TypeName = "VARCHAR(MAX)")]
		public string? Description { get; set; }

    }
}
