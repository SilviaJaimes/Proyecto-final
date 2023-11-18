using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class ProductoRepository : GenericRepoStr<Producto>, IProducto
{
    private readonly ApiContext _context;

    public ProductoRepository(ApiContext context) : base(context)
    {
        _context = context;
    }

    //Consulta 10
    public async Task<IEnumerable<Object>> ProductosOrnamentalesYMás100Unidades()
    {
        var productos = await _context.Productos
            .Where (p => p.Gama == "Ornamentales".ToLower() && p.CantidadStock >= 100)
            .Select(p => new
            {
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Cantidad = p.CantidadStock,
                PrecioVenta = p.PrecioVenta
            }).OrderByDescending(pv => pv.PrecioVenta)
            .ToListAsync();

        return productos;
    }

    //Consulta 24
    public async Task<IEnumerable<object>> ProductosSinPedido()
    {
        var productos = await (
            from p in _context.Productos
            join dp in _context.DetallePedidos on p.Id equals dp.CodigoProducto into Grupo
            where !Grupo.Any()
            select new
            {
                CodigoProducto = p.Id,
                Nombre = p.Nombre
            }
        ).ToListAsync();

        return productos;
    }

    //Consulta 25
    public async Task<IEnumerable<object>> ProductosSinPedidoDescripcion()
    {
        var productos = await (
            from p in _context.Productos
            join dp in _context.DetallePedidos on p.Id equals dp.CodigoProducto into Grupo
            where !Grupo.Any()
            select new
            {
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Imagen = p.GamaProducto.Imagen
            }
        ).ToListAsync();

        return productos;
    }

    public override async Task<IEnumerable<Producto>> GetAllAsync()
    {
        return await _context.Productos
            .ToListAsync();
    }

    public override async Task<Producto> GetByIdAsync(string id)
    {
        return await _context.Productos
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }

    public override async Task<(int totalRegistros, IEnumerable<Producto> registros)> GetAllAsync(int pageIndez, int pageSize, string search)
    {
        var query = _context.Productos as IQueryable<Producto>;

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