// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ItemzApp.API.Models
{
    public class HierarchyIdRecordDetailsDTO
    {
        /// <summary>
        /// Parent Record ID representated by a GUID.
        /// </summary>
        public Guid RecordId { get; set; }

        /// <summary>
        /// Hierarchy ID in string format for RecordId e.g. "/3/2/1"
        /// </summary>
        public string? HierarchyId { get; set; }

        /// <summary>
        /// Hierarchy Level for RecordId
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        /// Record Type within Hierarchy for RecordId
        /// </summary>
        public string? RecordType { get; set; }


        /// <summary>
        /// Hierarchy Id for the TOP Hierarchy Record within a given Parent Hierarchy Record
        /// </summary>
        public string? TopChildHierarchyId { get; set; } = "";

        /// <summary>
        /// Hierarchy Id for the BOTTOM Hierarchy Record within a given Parent Hierarchy Record
        /// </summary>
        public string? BottomChildHierarchyId { get; set; } = "";

        /// <summary>
        /// Total Number of Child nodes under RecordId
        /// </summary>
        public int? NumberOfChildNodes { get; set; } = 0;


    }
}
