using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class EmpleadoRepository : GenericRepository<Empleado>, IEmpleado
{
    private readonly ApiContext _context;

    public EmpleadoRepository(ApiContext context) : base(context)
    {
        _context = context;
    }

    //Consulta 16
    public async Task<IEnumerable<object>> EmpleadoConJefes()
    {
        var empleados = await (
            from e in _context.Empleados
            select new
            {
                NombreEmpleado = e.Nombre,
                Jefe = e.Jefe.Nombre
            }
        ).ToListAsync();

        return empleados;
    }

    //Consulta 21
    public async Task<IEnumerable<object>> EmpleadosSinClienteAsociado()
    {
        var empleados = await (
            from e in _context.Empleados
            join c in _context.Clientes on e.Id equals c.CodigoEmpleado into Grupo
            where !Grupo.Any()
            select new
            {
                NombreEmpleado = e.Nombre,
                Oficina = (from o in _context.Oficinas
                            where e.CodigoOficina == o.Id 
                            select new {
                                Codigo = o.Id,
                                Ciudad = o.Ciudad,
                                Telefono = o.Telefono
                            }).ToList()
            }
        ).ToListAsync();

        return empleados;
    }

    //Consulta 22
    public async Task<IEnumerable<object>> EmpleadoSinClienteYSinOficina()
    {
        var empleados = await (
            from e in _context.Empleados
            join c in _context.Clientes on e.Id equals c.CodigoEmpleado into Grupo
            where !Grupo.Any() || e.CodigoOficina == null
            select new
            {
                Id = e.Id,
                NombreEmpleado = e.Nombre
            }
        ).ToListAsync();

        return empleados;
    }

    public override async Task<IEnumerable<Empleado>> GetAllAsync()
    {
        return await _context.Empleados
            .ToListAsync();
    }

    public override async Task<Empleado> GetByIdAsync(int id)
    {
        return await _context.Empleados
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }

    public override async Task<(int totalRegistros, IEnumerable<Empleado> registros)> GetAllAsync(int pageIndez, int pageSize, string search)
    {
        var query = _context.Empleados as IQueryable<Empleado>;

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