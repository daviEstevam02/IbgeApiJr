using AutoMapper;

namespace IbgeDesafio.Api.AutoMapper;

public class GenericProfile : Profile
{
    public GenericProfile()
    {
        
        CreateMap<Guid, string>().ConstructUsing(guid => guid == Guid.Empty
            ?  string.Empty : guid.ToString());
    }
}