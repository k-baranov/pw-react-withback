using AutoMapper;
using PW.DataTransferObjects;
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
                //.AfterMap((s, d, _, context) =>
                //{
                //    if (source.Payee.Email == email)
                //    {
                //        result.Name = source.Recipient.UserName;
                //        result.Amount = -source.Amount;
                //        result.Balance = source.ResultingPayeeBalance;
                //    }
                //    else
                //    {
                //        result.Name = source.Payee.UserName;
                //        result.Amount = source.Amount;
                //        result.Balance = source.ResultingRecipientBalance;
                //    }
                //});
        }
    }
}
