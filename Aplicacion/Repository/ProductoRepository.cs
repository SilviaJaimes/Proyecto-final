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

    //Consulta 10 con paginación
    public async Task<(int totalRegistros, IEnumerable<Object> registros)> ProductosOrnamentalesYMás100UnidadesPaginated(int pageIndex, int pageSize, string search = null)
    {
        var query = _context.Productos
                    .Where(p => p.Gama == "Ornamentales".ToLower() && p.CantidadStock >= 100)
                    .Select(p => new
                    {
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
                        Cantidad = p.CantidadStock,
                        PrecioVenta = p.PrecioVenta
                    });

        if (!string.IsNullOrEmpty(search))
        {
            var lowerSearch = search.ToLower();
            query = query.Where(m => m.Nombre.ToLower().Contains(lowerSearch) || m.Descripcion.ToLower().Contains(lowerSearch));
        }

        var orderedQuery = query.OrderByDescending(pv => pv.PrecioVenta);

        int totalRegistros = await orderedQuery.CountAsync();

        int totalPages = (int)Math.Ceiling((double)totalRegistros / pageSize);

        pageIndex = Math.Min(pageIndex, totalPages);

        var registros = await orderedQuery
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
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

    //Consulta 24 con paginación
    public async Task<(int totalRegistros, IEnumerable<Object> registros)> ProductosSinPedidoPaginated(int pageIndex, int pageSize, string search = null)
    {
        var query = from p in _context.Productos
                    join dp in _context.DetallePedidos on p.Id equals dp.CodigoProducto into Grupo
                    where !Grupo.Any()
                    select new
                    {
                        CodigoProducto = p.Id,
                        Nombre = p.Nombre
                    };

        if (!string.IsNullOrEmpty(search))
        {
            var lowerSearch = search.ToLower();
            query = query.Where(m => m.CodigoProducto.ToString().Contains(lowerSearch));
        }

        int totalRegistros = await query.CountAsync();

        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
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

    //Consulta 40
    public async Task<IEnumerable<object>> ProductosMasVendidos()
    {
        var productosMasVendidos = await (
            from dp in _context.DetallePedidos
            group dp by dp.CodigoProducto into grupo
            select new
            {
                CodigoProducto = grupo.Key,
                CantidadVendida = grupo.Sum(dp => dp.Cantidad)
            }
            into resultado
            orderby resultado.CantidadVendida descending
            select resultado
        )
        .Take(20)
        .ToListAsync();

        return productosMasVendidos;
    }

    //Consulta 41
    public async Task<IEnumerable<object>> ProductosMasVendidosPorCodigo()
    {
        return await (
            from detallePedido in _context.DetallePedidos
            group detallePedido by detallePedido.CodigoProducto into grp
            orderby grp.Sum(dp => dp.Cantidad) descending
            select new
            {
                CodigoProducto = grp.Key,
                TotalUnidadesVendidas = grp.Sum(dp => dp.Cantidad)
            }
        ).Take(20).ToListAsync();

    }

    //Consulta 42
    public async Task<IEnumerable<object>> ProductosMasVendidosPorCodigoFiltrados()
    {
        return await (
            from detallePedido in _context.DetallePedidos
            where detallePedido.Producto.Id.StartsWith("OR")
            group detallePedido by detallePedido.CodigoProducto into grp
            orderby grp.Sum(dp => dp.Cantidad) descending
            select new
            {
                CodigoProducto = grp.Key,
                TotalUnidadesVendidas = grp.Sum(dp => dp.Cantidad)
            }
        ).Take(20).ToListAsync();

    }

    //Consulta 43
    public async Task<IEnumerable<object>> VentasProductosMas3000Euros()
    {
        var ventasProductos = await _context.DetallePedidos
            .Include(dp => dp.Producto)
            .GroupBy(
                dp => new { dp.CodigoProducto, dp.Producto.Nombre, dp.Producto.PrecioVenta },
                (key, group) => new
                {
                    key.CodigoProducto,
                    key.Nombre,
                    key.PrecioVenta,
                    TotalFacturado = group.Sum(dp => (float)dp.Cantidad * (float)dp.Producto.PrecioVenta)
                })
            .Where(result => result.TotalFacturado * 1.21 > 3000)
            .ToListAsync();

        var resultadoFinal = ventasProductos
            .Select(item => new
            {
                item.Nombre,
                UnidadesVendidas = _context.DetallePedidos
                    .Where(dp => dp.CodigoProducto == item.CodigoProducto)
                    .Sum(dp => dp.Cantidad),
                item.TotalFacturado,
                TotalConImpuestos = item.TotalFacturado * 1.21
            })
            .ToList();

        return resultadoFinal;
    }

    //Consulta 46
    public async Task<string> ProductoConPrecioMasCaro()
    {
        var productoMasCaro = await _context.Productos
            .Where(p => p.PrecioVenta == _context.Productos.Max(x => x.PrecioVenta))
            .Select(p => p.Nombre)
            .FirstOrDefaultAsync();

        return productoMasCaro;
    }

    //Consulta 47
    public async Task<string> ProductoConMayorDeUnidadesVendidas()
    {
        var productoMasCaro = await _context.Productos
            .Where(p => p.DetallePedidos.Any())
            .Where(p => p.CantidadStock == p.DetallePedidos.Max(p => p.Cantidad))
            .Select(p => p.Nombre)
            .FirstOrDefaultAsync();

        return productoMasCaro;
    }

    //Consulta 50
    public async Task<string> ProductoConPrecioDeVentaMasCaro()
    {
        var productoMasCaro = await _context.Productos
            .Where(p => p.PrecioVenta == _context.Productos.Max(x => x.PrecioVenta))
            .Select(p => p.Nombre)
            .FirstOrDefaultAsync();

        return productoMasCaro;
    }

    //Consulta 53
    public async Task<IEnumerable<object>> ProductosQueNuncaHanAparecidoEnPedidos()
    {
        var productos = await (
            from p in _context.Productos
            join dp in _context.DetallePedidos on p.Id equals dp.CodigoProducto into pedidos
            where !pedidos.Any()
            select new
            {
                Id = p.Id,
                Producto = p.Nombre
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