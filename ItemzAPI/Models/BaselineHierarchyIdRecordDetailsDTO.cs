// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ItemzApp.API.Models
{
    public class BaselineHierarchyIdRecordDetailsDTO
    {
        /// <summary>
        /// Parent Record ID representated by a GUID.
        /// </summary>
        public Guid RecordId { get; set; }

        /// <summary>
        /// Baseline Hierarchy ID in string format for RecordId e.g. "/3/2/1"
        /// </summary>
        public string? BaselineHierarchyId { get; set; }

        /// <summary>
        /// Source Itemz Hierarchy ID in string format for RecordId e.g. "/3/2/1"
        /// </summary>
        public string? SourceHierarchyId { get; set; }

        /// <summary>
        /// Baseline Hierarchy Level for RecordId
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        /// Record Type within Baseline Hierarchy for RecordId
        /// </summary>
        public string? RecordType { get; set; }


        /// <summary>
        /// Baseline Hierarchy Id for the TOP Baseline Hierarchy Record within a given Parent Baseline Hierarchy Record
        /// </summary>
        public string? TopChildBaselineHierarchyId { get; set; } = "";

        /// <summary>
        /// Baseline Hierarchy Id for the BOTTOM Baseline Hierarchy Record within a given Parent Baseline Hierarchy Record
        /// </summary>
        public string? BottomChildBaselineHierarchyId { get; set; } = "";

        /// <summary>
        /// Total Number of Child nodes under RecordId
        /// </summary>
        public int? NumberOfChildNodes { get; set; } = 0;

        /// <summary>
        /// TRUE if included in the baseline and FALSE if excluded from the baseline. 
        /// </summary>
        public bool IsIncluded { get; set; } 


    }
}
