using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class GamaProductoRepository : GenericRepoStr<GamaProducto>, IGamaProducto
{
    private readonly ApiContext _context;

    public GamaProductoRepository(ApiContext context) : base(context)
    {
        _context = context;
    }

    //Consulta 19
    public async Task<IEnumerable<Object>> GamasPorCliente()
    {
        var gamas = await (
            from c in _context.Clientes
            select new
            {
                Cliente = c.NombreCliente,
                Gamas = (
                    from p in _context.Pedidos
                    join dp in _context.DetallePedidos on p.Id equals dp.CodigoPedido
                    join pr in _context.Productos on dp.CodigoProducto equals pr.Id
                    join gp in _context.GamaProductos on pr.Gama equals gp.Id
                    where p.CodigoCliente == c.Id
                    where p.Estado.Contains("Entregado")
                    select new
                    {
                        Nombre = gp.Id
                    }
                ).Distinct().ToList()
            }
        ).ToListAsync();

        return gamas;
    }

    //Consulta 19 con paginación
    public async Task<(int totalRegistros, IEnumerable<Object> registros)> GamasPorClientePaginated(int pageIndex, int pageSize, string search = null)
    {
        var query = from c in _context.Clientes
                    select new
                    {
                        Cliente = c.NombreCliente,
                        Gamas = (
                            from p in _context.Pedidos
                            join dp in _context.DetallePedidos on p.Id equals dp.CodigoPedido
                            join pr in _context.Productos on dp.CodigoProducto equals pr.Id
                            join gp in _context.GamaProductos on pr.Gama equals gp.Id
                            where p.CodigoCliente == c.Id
                            select new
                            {
                                Nombre = gp.Id
                            }
                        ).Distinct().ToList()
                    };

        if (!string.IsNullOrEmpty(search))
        {
            var lowerSearch = search.ToLower();
            query = query.Where(m => m.Cliente.ToString().Contains(lowerSearch));
        }

        int totalRegistros = await query.CountAsync();

        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }

    public override async Task<IEnumerable<GamaProducto>> GetAllAsync()
    {
        return await _context.GamaProductos
            .ToListAsync();
    }

    public override async Task<GamaProducto> GetByIdAsync(string id)
    {
        return await _context.GamaProductos
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }

    public override async Task<(int totalRegistros, IEnumerable<GamaProducto> registros)> GetAllAsync(int pageIndez, int pageSize, string search)
    {
        var query = _context.GamaProductos as IQueryable<GamaProducto>;

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