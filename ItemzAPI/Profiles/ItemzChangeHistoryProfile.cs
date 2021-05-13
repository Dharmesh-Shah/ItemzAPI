// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

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

#nullable disable
