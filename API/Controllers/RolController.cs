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
[Authorize]

public class RolController : BaseApiController
{
    private readonly IUnitOfWork unitofwork;
    private readonly  IMapper mapper;

    public RolController( IUnitOfWork unitofwork, IMapper mapper)
    {
        this.unitofwork = unitofwork;
        this.mapper = mapper;
    }

    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<RolDto>>> Get()
    {
        var entidad = await unitofwork.Roles.GetAllAsync();
        return mapper.Map<List<RolDto>>(entidad);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RolDto>> Get(int id)
    {
        var entidad = await unitofwork.Roles.GetByIdAsync(id);
        if (entidad == null){
            return NotFound();
        }
        return this.mapper.Map<RolDto>(entidad);
    }

    [HttpGet]
    [MapToApiVersion("1.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<RolDto>>> GetPaginacion([FromQuery] Params rolParams)
    {
        var entidad = await unitofwork.Roles.GetAllAsync(rolParams.PageIndex, rolParams.PageSize, rolParams.Search);
        var listEntidad = mapper.Map<List<RolDto>>(entidad.registros);
        return new Pager<RolDto>(listEntidad, entidad.totalRegistros, rolParams.PageIndex, rolParams.PageSize, rolParams.Search);
    }

    /* [HttpGet("consulta-2/{Laboratorio}")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> MedicamentoLaboratorio(string Laboratorio)
    {
        var entidad = await unitofwork.Laboratorios.MedicamentoLaboratorio(Laboratorio);
        var dto = mapper.Map<IEnumerable<object>>(entidad);
        return Ok(dto);
    }

    [HttpGet("consulta-2/{Laboratorio}")]
    [MapToApiVersion("1.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<object>>> MedicamentoLaboratorioPaginated(string Laboratorio, [FromQuery] Params laboratorioParams)
    {
        var entidad = await unitofwork.Laboratorios.MedicamentoLaboratorioPaginated(Laboratorio, laboratorioParams.PageIndex, laboratorioParams.PageSize, laboratorioParams.Search);
        var listEntidad = mapper.Map<List<object>>(entidad.registros);
        return new Pager<object>(listEntidad, entidad.totalRegistros, laboratorioParams.PageIndex, laboratorioParams.PageSize, laboratorioParams.Search);
    }
 */
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Rol>> Post(RolDto entidadDto)
    {
        var entidad = this.mapper.Map<Rol>(entidadDto);
        this.unitofwork.Roles.Add(entidad);
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
    public async Task<ActionResult<RolDto>> Put(int id, [FromBody]RolDto entidadDto){
        if(entidadDto == null)
        {
            return NotFound();
        }
        var entidad = this.mapper.Map<Rol>(entidadDto);
        unitofwork.Roles.Update(entidad);
        await unitofwork.SaveAsync();
        return entidadDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id){
        var entidad = await unitofwork.Roles.GetByIdAsync(id);
        if(entidad == null)
        {
            return NotFound();
        }
        unitofwork.Roles.Remove(entidad);
        await unitofwork.SaveAsync();
        return NoContent();
    }
}