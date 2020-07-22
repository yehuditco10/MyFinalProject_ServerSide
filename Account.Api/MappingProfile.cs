using Account.Api.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, Services.Models.Customer>();
            CreateMap<Services.Models.Customer, Customer>();
            CreateMap< Entities.Customer,Services.Models.Customer>();
            CreateMap<Services.Models.Customer,Entities.Customer>();
            CreateMap<Entities.Account, Services.Models.Account>();
            CreateMap<Services.Models.Account, Entities.Account>();      
        }
    }
}
