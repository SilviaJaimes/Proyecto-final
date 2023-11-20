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

    //Consulta 17
    public async Task<IEnumerable<object>> EmpleadoConJefes()
    {
        var empleados = await _context.Empleados
                    .Include( e=> e.Jefe)
                    .Where(e => e.CodigoJefe != null && e.Jefe != null && e.Jefe.Jefe != null)
                    .Select(emp => new
                    {
                        emp.Id,
                        emp.Nombre,
                        jefe = new 
                        {
                            emp.Jefe.Id,
                            emp.Jefe.Nombre,
                            jefe = new
                            {
                                emp.Jefe.Jefe.Id,
                                emp.Jefe.Jefe.Nombre
                            }
                        }
                    }).ToListAsync();

        return empleados;
    }

    //Consulta 17 con paginación
    public async Task<(int totalRegistros, IEnumerable<Object> registros)> EmpleadoConJefesPaginated(int pageIndex, int pageSize, string search = null)
    {
        var query = _context.Empleados
                    .Include( e=> e.Jefe)
                    .Where(e => e.CodigoJefe != null && e.Jefe != null && e.Jefe.Jefe != null)
                    .Select(emp => new
                    {
                        emp.Id,
                        emp.Nombre,
                        jefe = new 
                        {
                            emp.Jefe.Id,
                            emp.Jefe.Nombre,
                            jefe = new
                            {
                                emp.Jefe.Jefe.Id,
                                emp.Jefe.Jefe.Nombre
                            }
                        }
                    });

        if (!string.IsNullOrEmpty(search))
        {
            var lowerSearch = search.ToLower();
            query = query.Where(m => m.Id.ToString().Contains(lowerSearch));
        }

        int totalRegistros = await query.CountAsync();

        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }

    //Consulta 22
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

    //Consulta 22 con paginación
    public async Task<(int totalRegistros, IEnumerable<Object> registros)> EmpleadosSinClienteAsociadoPaginated(int pageIndex, int pageSize, string search = null)
    {
        var query = from e in _context.Empleados
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
                    };

        if (!string.IsNullOrEmpty(search))
        {
            var lowerSearch = search.ToLower();
            query = query.Where(m => m.NombreEmpleado.ToString().Contains(lowerSearch));
        }

        int totalRegistros = await query.CountAsync();

        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }

    //Consulta 23
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

    //Consulta 28
    public async Task<IEnumerable<object>> EmpleadoSinCliente()
    {
        var empleados = await (
            from e in _context.Empleados
            join c in _context.Clientes on e.Id equals c.CodigoEmpleado into Grupo
            where !Grupo.Any()
            select new
            {
                NombreEmpleado = e.Nombre,
                JefeAsociado = e.Jefe.Nombre
            }
        ).ToListAsync();

        return empleados;
    }

    //Consulta 29
    public async Task<int> TotalEmpleados()
    {
        int totalEmpleados = await _context.Empleados
            .CountAsync();

        return totalEmpleados;
    }

    //Consulta 35
    public async Task<IEnumerable<object>> RepresentanteVentasConCantidadClientes()
    {
        var representantes = await (
            from e in _context.Empleados
            where e.Puesto.ToLower().Contains("Representante Ventas")
            join c in _context.Clientes on e.Id equals c.CodigoEmpleado into Grupo
            select new
            {
                RepresentateDeVentas = e.Nombre,
                CantidadClientes = Grupo.Count()
            }
        ).ToListAsync();

        return representantes;
    }

    //Consulta 54
    public async Task<IEnumerable<object>> EmpleadosQueNoSeanRepresentantesDeVentas()
    {
        var empleados = await (
            from e in _context.Empleados
            where e.Puesto == "Representante Ventas".ToLower() 
            join c in _context.Clientes on e.Id equals c.CodigoEmpleado into representantes
            where !representantes.Any()
            select new
            {
                Id = e.Id,
                NombreEmpleado = e.Nombre
            }
        ).ToListAsync();

        return empleados;
    }

    //Consulta 61
    public async Task<IEnumerable<object>> InfoEmpleadosQueNoSeanRepresentantes()
    {
        var empleados = await (
            from e in _context.Empleados
            where e.Puesto == "Representante Ventas".ToLower() 
            join c in _context.Clientes on e.Id equals c.CodigoEmpleado into repre
            where !repre.Any()
            select new
            {
                NombreEmpleado = e.Nombre,
                PrimerApellido = e.Apellido1,
                SegundoApeddio = e.Apellido2,
                Puesto = e.Puesto,
                NumOficina = e.Oficina.Telefono
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