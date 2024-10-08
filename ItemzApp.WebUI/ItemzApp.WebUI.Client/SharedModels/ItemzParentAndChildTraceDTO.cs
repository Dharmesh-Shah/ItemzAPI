using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemzApp.WebUI.Client.SharedModels
{
    public class ItemzParentAndChildTraceDTO
    {
        public ItemzParentAndChildTraceDTO()
        {
            SingleItemzAllTrace__DTO singleItemzAllTrace__DTO = new SingleItemzAllTrace__DTO();
            Itemz = singleItemzAllTrace__DTO;
        }

        public SingleItemzAllTrace__DTO? Itemz { get; set; }

    }

    public class SingleItemzAllTrace__DTO
    {
        public Guid ID { get; set; }
        public List<ParentTraceItemz__DTO>? ParentItemz { get; set; }
        public List<ChildTraceItemz__DTO>? ChildItemz { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ChildTraceItemz__DTO
    {
        public Guid ItemzID { get; set; }
    }

    public class ParentTraceItemz__DTO
    {
        public Guid ItemzID { get; set; }
    }
}
