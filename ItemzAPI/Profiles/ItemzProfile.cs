// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemzApp.API.Profiles
{
    public class ItemzProfile : Profile
    {
        public ItemzProfile ()
        {
            CreateMap<Entities.Itemz, Models.GetItemzDTO>();    // Used for creating GetItemzDTO based on Itemz object.
            CreateMap<Models.CreateItemzDTO, Entities.Itemz>(); // Used for creating Itemz based on CreateItemzDTO object.
            CreateMap<Models.UpdateItemzDTO, Entities.Itemz>(); // Used for updating Itemz based on UpdateItemzDTO object.
            CreateMap<Entities.Itemz,Models.UpdateItemzDTO>();  // Used for updating UpdateItemzDTO based on Itemz object.
        }

    }
}
