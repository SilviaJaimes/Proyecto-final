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
    /* public async Task<IEnumerable<object>> ProductosMasVendidosAgrupadaPorCodigo()
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
        .OrderBy(dp => dp.CodigoProducto)
        .Take(20)
        .ToListAsync();

        return productosMasVendidos;
    } */

    //Consulta 42
    /* public async Task<IEnumerable<object>> ProductosMasVendidosAgrupadaPorCodigoEmpiecenPorOR()
    {
        var productosMasVendidos = await (
            from dp in _context.DetallePedidos
            where dp.CodigoProducto.StartsWith("OR")
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
        .OrderBy(dp => dp.CodigoProducto)
        .ToListAsync();

        return productosMasVendidos;
    } */

    //Consulta 43
    /* public async Task<IEnumerable<Object>> ProductosMásDe3000E()
    {
        var productos = await _context.Pagos
            .Where (p => p.Total <= 3000)
            .Select(p => new
            {
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Cantidad = p.CantidadStock,
                PrecioVenta = p.PrecioVenta
            }).OrderByDescending(pv => pv.PrecioVenta)
            .ToListAsync();

        return productos;
    } */

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