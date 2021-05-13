// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;

#nullable enable

namespace ItemzApp.API.Profiles
{
    public class ItemzTypeProfile : Profile
    {
        public ItemzTypeProfile()
        {
            CreateMap<Entities.ItemzType, Models.GetItemzTypeDTO>(); // Used for creating GetItemzTypeDTO based on ItemzType object.
            CreateMap<Models.CreateItemzTypeDTO, Entities.ItemzType>(); // Used for creating ItemzType based on CreateItemzTypeDTO object.
            CreateMap<Models.UpdateItemzTypeDTO, Entities.ItemzType>(); // Used for updating ItemzType based on UpdateItemzTypeDTO object.
            CreateMap<Entities.ItemzType, Models.UpdateItemzTypeDTO>();  // Used for updating UpdateItemzTypeDTO based on ItemzType object.
        }
    }
}

#nullable disable
