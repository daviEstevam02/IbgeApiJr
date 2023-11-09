using AutoMapper;
using IbgeDesafio.Api.Controllers;
using IbgeDesafio.Api.DTOs;
using IbgeDesafio.Api.Models;

namespace IbgeDesafio.Api.AutoMapper;

public class LocaleProfile : Profile
{
    /*AutoMapper: Mapeia props de dois objetos diferentes e transforma o
    objeto de entrada de um tipo no objeto de saida do outro*/
   
    public LocaleProfile()
    {
        CreateMap<CreateLocaleRequestDto, Locale>();
        CreateMap<EditLocaleRequestDto, Locale>();
    }
}