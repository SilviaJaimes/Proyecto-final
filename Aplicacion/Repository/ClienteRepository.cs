using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class ClienteRepository : GenericRepository<Cliente>, ICliente
{
    private readonly ApiContext _context;

    public ClienteRepository(ApiContext context) : base(context)
    {
        _context = context;
    }

    //Consulta 1
    public async Task<IEnumerable<Object>> ClientesEspañoles()
    {
        var clientes = await (
            from c in _context.Clientes
            where c.Pais == "Spain"
            select new
            {
                NombreCliente = c.NombreCliente,
                Telefono = c.Telefono,
                Ciudad = c.Ciudad
            }).ToListAsync();

        return clientes;
    }

     //Consulta 3
    public async Task<IEnumerable<Object>> Pago2008()
    {
        var pagos = await (
            from p in _context.Pagos
            join c in _context.Clientes on p.CodigoCliente equals c.Id
            where p.FechaPago.Year == 2008
            group p by p.CodigoCliente into Grupo
            select new
            {
                CodigoCliente = Grupo.Key
            }).ToListAsync();

        return pagos;
    }

    //Consulta 10
    public async Task<IEnumerable<Object>> ClientesMadridYRVConCodigo11O30()
    {
        var clientes = await (
            from c in _context.Clientes
            where c.Ciudad == "Madrid".ToLower() && c.CodigoEmpleado == 11 || c.CodigoEmpleado == 30
            select new
            {
                NombreCliente = c.NombreCliente,
                Telefono = c.Telefono,
                Ciudad = c.Ciudad,
                CodigoRepresentante =c.CodigoEmpleado
            }).ToListAsync();

        return clientes;
    }

    //Consulta 11
    public async Task<IEnumerable<object>> ClienteConSuRepresentante()
    {
        var clientes = await (
            from c in _context.Clientes
            select new
            {
                NombreCliente = c.NombreCliente,
                RepresentanteDeVentas = (
                    from e in _context.Empleados
                    where e.Id == c.CodigoEmpleado
                    select new
                    {
                        Nombre = e.Nombre,
                        Apellido = e.Apellido1
                    }
                ).ToList()
            }
        ).ToListAsync();

        return clientes;
    }

    //Consulta 12
    public async Task<IEnumerable<object>> ClientesConPagos()
    {
        var clientes = await (
            from p in _context.Pagos
            join c in _context.Clientes on p.CodigoCliente equals c.Id
            where p.CodigoCliente == c.Id
            group c by c.Id into Grupo
            select new
            {
                NombreCliente = Grupo.First().NombreCliente,
                RepresentanteDeVentas = (
                    from e in _context.Empleados
                    where e.Id == Grupo.First().CodigoEmpleado
                    select new
                    {
                        Nombre = e.Nombre
                    }
                ).ToList()
            }
        ).ToListAsync();

        return clientes;
    }

    //Consulta 13
    public async Task<IEnumerable<object>> ClientesSinPagos()
    {
        var clientesSinPagos = await (
            from c in _context.Clientes
            join p in _context.Pagos on c.Id equals p.CodigoCliente into pagosGroup
            where !pagosGroup.Any()
            select new
            {
                NombreCliente = c.NombreCliente,
                RepresentanteDeVentas = (
                    from e in _context.Empleados
                    where e.Id == c.CodigoEmpleado
                    select new
                    {
                        Nombre = e.Nombre
                    }
                ).ToList()
            }
        ).ToListAsync();

        return clientesSinPagos;
    }

    //Consulta 14
    public async Task<IEnumerable<object>> ClientesConPagosRepresentanteYOficina()
    {
        var clientes = await (
            from c in _context.Clientes
            join p in _context.Pagos on c.Id equals p.CodigoCliente into pagosGroup
            where pagosGroup.Any()
            select new
            {
                NombreCliente = c.NombreCliente,
                RepresentanteDeVentas = (
                    from e in _context.Empleados
                    where e.Id == c.CodigoEmpleado
                    select new
                    {
                        Nombre = e.Nombre,
                        Oficina = e.Oficina.Ciudad
                    }
                ).ToList()
            }
        ).ToListAsync();

        return clientes;
    }

    //Consulta 15
    public async Task<IEnumerable<object>> ClientesSinPagosRepresentanteYOficina()
    {
        var clientes = await (
            from c in _context.Clientes
            join p in _context.Pagos on c.Id equals p.CodigoCliente into pagosGroup
            where !pagosGroup.Any()
            select new
            {
                NombreCliente = c.NombreCliente,
                RepresentanteDeVentas = (
                    from e in _context.Empleados
                    where e.Id == c.CodigoEmpleado
                    select new
                    {
                        Nombre = e.Nombre,
                        Oficina = e.Oficina.Ciudad
                    }
                ).ToList()
            }
        ).ToListAsync();

        return clientes;
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

    //Consulta 17
    public async Task<IEnumerable<object>> ClientesConPedidoTardio()
    {
        var clientes = await (
            from p in _context.Pedidos
            join c in _context.Clientes on p.CodigoCliente equals c.Id
            where p.FechaEntrega > p.FechaEsperada
            group c by c.Id into Grupo
            select new
            {
                NombreCliente = Grupo.First().NombreCliente
            }
        ).ToListAsync();

        return clientes;
    }

    public override async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return await _context.Clientes
            .ToListAsync();
    }

    public override async Task<Cliente> GetByIdAsync(int id)
    {
        return await _context.Clientes
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }

    public override async Task<(int totalRegistros, IEnumerable<Cliente> registros)> GetAllAsync(int pageIndez, int pageSize, string search)
    {
        var query = _context.Clientes as IQueryable<Cliente>;

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