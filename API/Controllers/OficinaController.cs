using API.Dtos;
using API.Helpers.Errors;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiVersion("1.0")]
[ApiVersion("1.1")]
/* [Authorize] */

public class OficinaController : BaseApiController
{
    private readonly IUnitOfWork unitofwork;
    private readonly  IMapper mapper;

    public OficinaController( IUnitOfWork unitofwork, IMapper mapper)
    {
        this.unitofwork = unitofwork;
        this.mapper = mapper;
    }

    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<OficinaDto>>> Get()
    {
        var entidad = await unitofwork.Oficinas.GetAllAsync();
        return mapper.Map<List<OficinaDto>>(entidad);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OficinaDto>> Get(string id)
    {
        var entidad = await unitofwork.Oficinas.GetByIdAsync(id);
        if (entidad == null){
            return NotFound();
        }
        return this.mapper.Map<OficinaDto>(entidad);
    }

    [HttpGet]
    [MapToApiVersion("1.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<OficinaDto>>> GetPaginacion([FromQuery] Params entidadParams)
    {
        var entidad = await unitofwork.Oficinas.GetAllAsync(entidadParams.PageIndex, entidadParams.PageSize, entidadParams.Search);
        var listEntidad = mapper.Map<List<OficinaDto>>(entidad.registros);
        return new Pager<OficinaDto>(listEntidad, entidad.totalRegistros, entidadParams.PageIndex, entidadParams.PageSize, entidadParams.Search);
    }

    [HttpGet("consulta-26")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> OficinaSinEmpleadoRepresentante()
    {
        var entidad = await unitofwork.Oficinas.OficinaSinEmpleadoRepresentante();
        var dto = mapper.Map<IEnumerable<object>>(entidad);
        return Ok(dto);
    }

    [HttpGet("consulta-26")]
    [MapToApiVersion("1.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<object>>> OficinaSinEmpleadoRepresentantePaginated([FromQuery] Params entidadparams)
    {
        var entidad = await unitofwork.Oficinas.OficinaSinEmpleadoRepresentantePaginated(entidadparams.PageIndex, entidadparams.PageSize, entidadparams.Search);
        var listEntidad = mapper.Map<List<object>>(entidad.registros);
        return new Pager<object>(listEntidad, entidad.totalRegistros, entidadparams.PageIndex, entidadparams.PageSize, entidadparams.Search);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Oficina>> Post(OficinaDto entidadDto)
    {
        var entidad = this.mapper.Map<Oficina>(entidadDto);
        this.unitofwork.Oficinas.Add(entidad);
        await unitofwork.SaveAsync();
        if(entidad == null)
        {
            return BadRequest();
        }
        entidadDto.Id = entidad.Id;
        return CreatedAtAction(nameof(Post), new {id = entidadDto.Id}, entidadDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OficinaDto>> Put(string id, [FromBody]OficinaDto entidadDto){
        if(entidadDto == null)
        {
            return NotFound();
        }
        var entidad = this.mapper.Map<Oficina>(entidadDto);
        unitofwork.Oficinas.Update(entidad);
        await unitofwork.SaveAsync();
        return entidadDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id){
        var entidad = await unitofwork.Oficinas.GetByIdAsync(id);
        if(entidad == null)
        {
            return NotFound();
        }
        unitofwork.Oficinas.Remove(entidad);
        await unitofwork.SaveAsync();
        return NoContent();
    }
}