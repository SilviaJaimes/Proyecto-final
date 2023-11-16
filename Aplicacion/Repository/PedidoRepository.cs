using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class PedidoRepository : GenericRepository<Pedido>, IPedido
{
    private readonly ApiContext _context;

    public PedidoRepository(ApiContext context) : base(context)
    {
        _context = context;
    }

    //Consulta 2
    public async Task<IEnumerable<Object>> EstadosPedido()
    {
        var pedidos = await (
            from p in _context.Pedidos
            group p by p.Estado into Grupo
            select new
            {
                Estado = Grupo.Key
            }).ToListAsync();

        return pedidos;
    }

    //Consulta 4
    public async Task<IEnumerable<Object>> SinEntregarATiempo()
    {
        var pedidos = await (
            from p in _context.Pedidos
            where p.FechaEsperada < p.FechaEntrega
            select new
            {
                CodigoPedido = p.Id,
                CodigoCliente = p.CodigoCliente,
                FechaEsperada = p.FechaEsperada,
                FechaEntrega = p.FechaEntrega
            }).ToListAsync();

        return pedidos;
    }

    //Consulta 5
    public async Task<IEnumerable<object>> DosDiasAntesFechaEsperada()
    {
        var pedidos = await (
            from p in _context.Pedidos
            where p.FechaEntrega.HasValue && (p.FechaEsperada.DayNumber - p.FechaEntrega.Value.DayNumber) >= 2
            select new
            {
                CodigoPedido = p.Id,
                CodigoCliente = p.CodigoCliente,
                FechaEsperada = p.FechaEsperada,
                FechaEntrega = p.FechaEntrega
            }).ToListAsync();

        return pedidos;
    }

    //Consulta 6
    public async Task<IEnumerable<Object>> PedidosEntregadosEnEnero()
    {
        var pedidos = await (
            from p in _context.Pedidos
            where p.FechaEntrega.Value.Month == 01
            select new
            {
                CodigoPedido = p.Id,
                CodigoCliente = p.CodigoCliente,
                FechaEntrega = p.FechaEntrega
            }).ToListAsync();

        return pedidos;
    }

    public override async Task<IEnumerable<Pedido>> GetAllAsync()
    {
        return await _context.Pedidos
            .ToListAsync();
    }

    public override async Task<Pedido> GetByIdAsync(int id)
    {
        return await _context.Pedidos
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }

    public override async Task<(int totalRegistros, IEnumerable<Pedido> registros)> GetAllAsync(int pageIndez, int pageSize, string search)
    {
        var query = _context.Pedidos as IQueryable<Pedido>;

        if(!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Id.ToString().ToLower().Contains(search));
        }

        query = query.OrderBy(p => p.Id);
        var totalRegistros = await query.CountAsync();
        var registros = await query 
            .Skip((pageIndez - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }
}