using AutoMapper;
using IbgeDesafio.Api.Auth;
using IbgeDesafio.Api.Data;
using IbgeDesafio.Api.DTOs;
using IbgeDesafio.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbgeDesafio.Api.Controllers;

[ApiController]
[Route("v1/auth")]
[AllowAnonymous]
public sealed class AuthController : ControllerBase
{
    private readonly IbgeDesafioContext _ibgeDesafioContext;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;

    public AuthController(IbgeDesafioContext ibgeDesafioContext, IMapper mapper, IJwtService jwtService)
    {
        _ibgeDesafioContext = ibgeDesafioContext;
        _mapper = mapper;
        _jwtService = jwtService;
    }
    
    //Métodos assincronos retornam uma Task
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserRequestDto registerUserRequest)
    {
        try
        {
            //SingleOrDefault x Single:
            //Single da erro caso o retorno seja vazio
            //SingleOrDefault retorna null caso seja vazio
            var userExists = await _ibgeDesafioContext.Users
                .SingleOrDefaultAsync(u => u.Email == registerUserRequest.Email);

            if (userExists is not null)
                throw new Exception("Usuário já existente");
            
            //Mappeando os dados do payload para User para conseguir adicionar ao banco
            var user = _mapper.Map<User>(registerUserRequest);
            user.Id = Guid.NewGuid();
            
            //Adicionando dados ao banco e persistindo com SaveChangesAsync
            await _ibgeDesafioContext.Users.AddAsync(user);
            if (await _ibgeDesafioContext.SaveChangesAsync() <= 0)
                throw new Exception("Erro ao tentar criar usuário");
            
            //Gerando token com os dados mapeados do payload
            var token = _jwtService.GenerateToken(user);
            
            //Mapeando a respota para o usuário
            var response = _mapper.Map<AuthUserResponseDto>(user);
            
            //Adicionando dados além do obrigatório (Email)
            return Ok(response with
            {
                Token = token,
            });
        }
        catch (Exception e)
        {
            return e.InnerException is not null
                ? BadRequest($"Erro: {e.Message} | Erro interno: {e.InnerException.Message}")
                : BadRequest(e.Message);
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserRequestDto loginUserRequest)
    {
        try
        {
            var userExists = await _ibgeDesafioContext.Users
                .SingleOrDefaultAsync(u => u.Email == loginUserRequest.Email 
                                           && u.Password == loginUserRequest.Password);

            if (userExists is null)
                throw new Exception("Email ou senha incorretos");

            var token = _jwtService.GenerateToken(userExists);
            var response = _mapper.Map<AuthUserResponseDto>(userExists);
            
            return Ok(response with
            {
                Token = token
            });
        }
        catch (Exception e)
        {
            return e.InnerException is not null
                ? BadRequest($"Erro: {e.Message} | Erro interno: {e.InnerException.Message}")
                : BadRequest(e.Message);
        }
    }
}