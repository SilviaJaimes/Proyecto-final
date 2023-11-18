using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class OficinaRepository : GenericRepoStr<Oficina>, IOficina
{
    private readonly ApiContext _context;

    public OficinaRepository(ApiContext context) : base(context)
    {
        _context = context;
    }

    //Consulta 26
    public async Task<IEnumerable<object>> OficinaSinEmpleadoRepresentante()
    {
        var oficinas = await (
            from o in _context.Oficinas
            where !_context.Empleados.Any(e => _context.Clientes
                .Where(c => c.CodigoEmpleado == e.Id)
                .Any(c => c.Pedidos
                    .Any(p => p.DetallePedidos
                        .Any(dp => dp.Producto.GamaProducto.Id == "Frutales"))))
            select new
            {
                Oficina = o.Id
            }
        ).ToListAsync();

        return oficinas;
    }

    public override async Task<IEnumerable<Oficina>> GetAllAsync()
    {
        return await _context.Oficinas
            .ToListAsync();
    }

    public override async Task<Oficina> GetByIdAsync(string id)
    {
        return await _context.Oficinas
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }

    public override async Task<(int totalRegistros, IEnumerable<Oficina> registros)> GetAllAsync(int pageIndez, int pageSize, string search)
    {
        var query = _context.Oficinas as IQueryable<Oficina>;

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