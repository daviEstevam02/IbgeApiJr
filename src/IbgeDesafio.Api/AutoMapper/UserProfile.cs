using AutoMapper;
using IbgeDesafio.Api.DTOs;
using IbgeDesafio.Api.Models;

namespace IbgeDesafio.Api.AutoMapper;

public class UserProfile : Profile
{
   /*AutoMapper: Mapeia props de dois objetos diferentes e transforma o
   objeto de entrada de um tipo no objeto de saida do outro*/
   
    public UserProfile()
    {
        CreateMap<RegisterUserRequestDto, User>();
        CreateMap<User, AuthUserResponseDto>();
        CreateMap<LoginUserRequestDto, User>();
    }
}