using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemzApp.API.Models
{
    public class DeleteItemzChangeHistoryDTO
    {
        /// <summary>
        /// itemzId of the Itemz representated by a GUID.
        /// </summary>
        public Guid ItemzId { get; set; }
        /// <summary>
        /// Date and Time upto which Itemz Change History data has to be deleted for given ItemzId.
        /// </summary>
        public DateTimeOffset UptoDateTime { get; set; }
    }
}
