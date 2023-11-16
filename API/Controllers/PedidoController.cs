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

public class PedidoController : BaseApiController
{
    private readonly IUnitOfWork unitofwork;
    private readonly  IMapper mapper;

    public PedidoController( IUnitOfWork unitofwork, IMapper mapper)
    {
        this.unitofwork = unitofwork;
        this.mapper = mapper;
    }

    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PedidoDto>>> Get()
    {
        var entidad = await unitofwork.Pedidos.GetAllAsync();
        return mapper.Map<List<PedidoDto>>(entidad);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PedidoDto>> Get(int id)
    {
        var entidad = await unitofwork.Pedidos.GetByIdAsync(id);
        if (entidad == null){
            return NotFound();
        }
        return this.mapper.Map<PedidoDto>(entidad);
    }

    [HttpGet]
    [MapToApiVersion("1.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<PedidoDto>>> GetPaginacion([FromQuery] Params entidadParams)
    {
        var entidad = await unitofwork.Pedidos.GetAllAsync(entidadParams.PageIndex, entidadParams.PageSize, entidadParams.Search);
        var listEntidad = mapper.Map<List<PedidoDto>>(entidad.registros);
        return new Pager<PedidoDto>(listEntidad, entidad.totalRegistros, entidadParams.PageIndex, entidadParams.PageSize, entidadParams.Search);
    }

    [HttpGet("consulta-2")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> EstadosPedido()
    {
        var entidad = await unitofwork.Pedidos.EstadosPedido();
        var dto = mapper.Map<IEnumerable<object>>(entidad);
        return Ok(dto);
    }

    [HttpGet("consulta-4")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> SinEntregarATiempo()
    {
        var entidad = await unitofwork.Pedidos.SinEntregarATiempo();
        var dto = mapper.Map<IEnumerable<object>>(entidad);
        return Ok(dto);
    }

    [HttpGet("consulta-5")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> DosDiasAntesFechaEsperada()
    {
        var entidad = await unitofwork.Pedidos.DosDiasAntesFechaEsperada();
        var dto = mapper.Map<IEnumerable<object>>(entidad);
        return Ok(dto);
    }

    [HttpGet("consulta-6")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> PedidosEntregadosEnEnero()
    {
        var entidad = await unitofwork.Pedidos.PedidosEntregadosEnEnero();
        var dto = mapper.Map<IEnumerable<object>>(entidad);
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pedido>> Post(PedidoDto entidadDto)
    {
        var entidad = this.mapper.Map<Pedido>(entidadDto);
        this.unitofwork.Pedidos.Add(entidad);
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
    public async Task<ActionResult<PedidoDto>> Put(int id, [FromBody]PedidoDto entidadDto){
        if(entidadDto == null)
        {
            return NotFound();
        }
        var entidad = this.mapper.Map<Pedido>(entidadDto);
        unitofwork.Pedidos.Update(entidad);
        await unitofwork.SaveAsync();
        return entidadDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id){
        var entidad = await unitofwork.Pedidos.GetByIdAsync(id);
        if(entidad == null)
        {
            return NotFound();
        }
        unitofwork.Pedidos.Remove(entidad);
        await unitofwork.SaveAsync();
        return NoContent();
    }
}