using AutoMapper;
using PaymentSystem.API.Models;
using PaymentSystem.Application.DTOs;
using PaymentSystem.Domain.Models;

namespace PaymentSystem.API.Common
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MerchantCreateModel, MerchantCreateDto>()
                .ReverseMap();
            CreateMap<MerchantUpdateModel, MerchantUpdateDto>()
                .ReverseMap();
            CreateMap<MerchantCreateDto, Merchant>()
                .ReverseMap();
            CreateMap<MerchantUpdateDto, Merchant>()
                .ReverseMap();
            CreateMap<TransactionCreateDto, Transaction>()
                .ReverseMap();
            CreateMap<PaymentModel, TransactionCreateDto>()
                .ReverseMap();
            CreateMap<TransactionCreateModel, TransactionCreateDto>()
                .ReverseMap();
            CreateMap<TransactionUpdateModel, TransactionUpdateDto>().
                ReverseMap();
        }
    }
}
