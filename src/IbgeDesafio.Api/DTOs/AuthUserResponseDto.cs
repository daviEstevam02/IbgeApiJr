namespace IbgeDesafio.Api.DTOs;

//Criando DTOs: DTOs tem o objetivo de definir como os dados serão retornados ao cliente
public record AuthUserResponseDto(string Email)
{
    public string?  Id { get; set; }
    
    public string? Username { get; set; }
    public string Token { get; set; }
}