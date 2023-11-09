using AutoMapper;
using IbgeDesafio.Api.Data;
using IbgeDesafio.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbgeDesafio.Api.Controllers;

[Authorize]
[ApiController]
[Route("v1/locale")]
public class LocaleController : ControllerBase
{
    private readonly IbgeDesafioContext _ibgeDesafioContext;
    private readonly IMapper _mapper;

    public LocaleController(IbgeDesafioContext ibgeDesafioContext, IMapper mapper)
    {
        _ibgeDesafioContext = ibgeDesafioContext;
        _mapper = mapper;
    }

    #region Queries
    [HttpGet]
    public async Task<IActionResult> GetLocale() => Ok(await _ibgeDesafioContext.Locales.ToListAsync());
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLocaleById(string id) => Ok( await _ibgeDesafioContext.Locales
        .SingleOrDefaultAsync(locale => locale.Id == id));

    [HttpGet("byCity/{city}")]
    public async Task<IActionResult> GetLocaleByCity(string city) => Ok(await _ibgeDesafioContext.Locales
        .SingleOrDefaultAsync(locale => locale.City == city));
    
    [HttpGet("byState/{state}")]
    public async Task<IActionResult> GetLocaleByState(string state) => Ok(await _ibgeDesafioContext.Locales
        .SingleOrDefaultAsync(locale => locale.State == state));
    #endregion

    #region Commands
    [HttpPost]
    public async Task<IActionResult> CreateLocale([FromBody] CreateLocaleRequestDto createLocaleRequestDto)
    {
        try
        {
            var localeExists = _ibgeDesafioContext.Locales
                .SingleOrDefault(locale => locale.City == createLocaleRequestDto.City
                                           && locale.State == createLocaleRequestDto.State);

            if (localeExists is not null)
                throw new Exception("Local já existente");

            var locale = _mapper.Map<Locale>(createLocaleRequestDto);

            await _ibgeDesafioContext.AddAsync(locale);
            if (await _ibgeDesafioContext.SaveChangesAsync() <= 0)
                throw new Exception("Erro ao criar localidade");

            return Ok("Localicade criada com sucesso");
        }
        catch (Exception e)
        {
            return e.InnerException is not null
                ? BadRequest($"Erro: {e.Message} | Erro interno: {e.InnerException.Message}")
                : BadRequest(e.Message);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLocale(string id, [FromBody] EditLocaleRequestDto editLocaleRequest)
    {
        try
        {
            var localeExistsDuplicate = _ibgeDesafioContext.Locales
                .SingleOrDefault(locale => locale.City == editLocaleRequest.City
                                           && locale.State == editLocaleRequest.State && locale.Id != id);
            
            if (localeExistsDuplicate is not null)
                throw new Exception("Localidade com cidade e estado ja existente");
            
            var localeExists = _ibgeDesafioContext.Locales                                                 
                .SingleOrDefault(locale => locale.Id == id);

            if (localeExists is null)
                throw new Exception("Local não existente");

            var locale = _mapper.Map(editLocaleRequest, localeExists);
            
             _ibgeDesafioContext.Update(locale);
             
            if (await _ibgeDesafioContext.SaveChangesAsync() <= 0)
                throw new Exception("Erro ao editar localidade");

            return Ok("Localicade editar com sucesso");
        }
        catch (Exception e)
        {
            return e.InnerException is not null
                ? BadRequest($"Erro: {e.Message} | Erro interno: {e.InnerException.Message}")
                : BadRequest(e.Message);
        }
        
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocale(string id)
    {
        try
        {
            var localeExists = _ibgeDesafioContext.Locales.SingleOrDefault(locale => locale.Id == id);

            if (localeExists is null)
                return NotFound();
            _ibgeDesafioContext.Remove(localeExists);
            if (await _ibgeDesafioContext.SaveChangesAsync() <= 0)
                throw new Exception("Erro ao remover localidade");  

            return Ok("Localidade deletada com sucesso");
        }
        catch (Exception e)
        {
            return e.InnerException is not null
                ? BadRequest($"Erro: {e.Message} | Erro interno: {e.InnerException.Message}")
                : BadRequest(e.Message);  
        }
    }
    #endregion

    
}