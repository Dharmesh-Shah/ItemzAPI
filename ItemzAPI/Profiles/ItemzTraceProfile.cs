// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;

namespace ItemzApp.API.Profiles
{
    public class ItemzTraceProfile : Profile
    {
        public ItemzTraceProfile()
        {
            CreateMap<Entities.ItemzJoinItemzTrace, Models.ItemzTraceDTO>(); // Used for creating ItemzTraceDTO based on ItemzTrace object.
            CreateMap<Models.ItemzTraceDTO, Entities.ItemzJoinItemzTrace>(); // Used for creating ItemzTrace based on ItemzTraceDTO object.
        }
    }
}
