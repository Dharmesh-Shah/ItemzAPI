﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;


#nullable enable

namespace ItemzApp.API.Models
{
    /// <summary>
    /// Itemz class containing several properties that represents it.
    /// This Itemz class is returned by the "ItemzApp.API" in most of the cases when
    /// user sends request to read an Itemz record.
    /// </summary>
    public class GetItemzDTO
    {
        /// <summary>
        /// Id of the Itemz representated by a GUID.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name or Title of the Itemz
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Status of the itemz
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// Priority of the Itemz
        /// </summary>
        public string? Priority { get; set; }
        /// <summary>
        /// Description of the Itemz
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// User who created the Itemz
        /// </summary>
        public string? CreatedBy { get; set; }
        /// <summary>
        /// Date and Time when Itemz was created
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }
        /// <summary>
        /// Severity of the Itemz
        /// </summary>
        public string? Severity { get; set; }
    }
}

#nullable disable
