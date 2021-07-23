// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;

namespace ItemzApp.API.Profiles
{
    public class BaselineItemzTypeProfile : Profile
    {
        public BaselineItemzTypeProfile()
        {
            CreateMap<Entities.BaselineItemzType, Models.GetBaselineItemzTypeDTO>(); // Used for creating GetBaselineItemzTypeDTO based on BaselineItemzType object.
        }
    }
}
