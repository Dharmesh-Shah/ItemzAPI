﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemzApp.API.Models
{
    public class BaselineItemzParentAndChildTraceDTO
    {
        public BaselineItemzParentAndChildTraceDTO()
        {
            SingleBaselineItemzAllTrace__DTO singleBaselineItemzAllTrace__DTO = new SingleBaselineItemzAllTrace__DTO();
            BaselineItemz = singleBaselineItemzAllTrace__DTO;
        }

        public SingleBaselineItemzAllTrace__DTO? BaselineItemz { get; set; }

    }

    public class SingleBaselineItemzAllTrace__DTO
    {
        public Guid ID { get; set; }
        public List<ParentTraceBaselineItemz__DTO>? ParentBaselineItemz { get; set; }
        public List<ChildTraceBaselineItemz__DTO>? ChildBaselineItemz { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ChildTraceBaselineItemz__DTO
    {
        public Guid BaselineItemzID { get; set; }
    }

    public class ParentTraceBaselineItemz__DTO
    {
        public Guid BaselineItemzID { get; set; }
    }
}
