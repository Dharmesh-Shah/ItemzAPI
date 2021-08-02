// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;

namespace ItemzApp.API.Profiles
{
    public class BaselineItemzProfile : Profile
    {
        public BaselineItemzProfile()
        {
            CreateMap<Entities.BaselineItemz, Models.GetBaselineItemzDTO>(); // Used for creating GetBaselineItemzDTO based on BaselineItemz object.
        }
    }
}
