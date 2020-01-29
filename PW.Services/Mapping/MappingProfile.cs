using AutoMapper;
using PW.DataTransferObjects.Transactions;
using PW.DataTransferObjects.Users;
using PW.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PW.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PwUser, UserBalanceDto>();
            CreateMap<PwTransaction, TransactionDto>()
                .ForMember(t => t.DateTime, opt => opt.MapFrom(src => src.TransactionDateTime.ToString("G", new CultureInfo("RU-ru"))));                
        }
    }
}
