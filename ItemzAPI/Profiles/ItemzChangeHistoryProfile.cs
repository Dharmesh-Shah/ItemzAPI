using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemzApp.API.Profiles
{
    public class ItemzChangeHistoryProfile : Profile
    {
        public ItemzChangeHistoryProfile()
        {
            CreateMap<Entities.ItemzChangeHistory, Models.GetItemzChangeHistoryDTO>(); // Used for creating GetItemzChangeHistoryDTO based on ItemzChangeHistory object.
        }
    }
}
