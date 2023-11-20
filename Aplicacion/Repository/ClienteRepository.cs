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

    //Consulta 1 con paginación
    public async Task<(int totalRegistros, IEnumerable<Object> registros)> ClientesEspañolesPaginated(int pageIndex, int pageSize, string search = null)
    {
        var query = from c in _context.Clientes
                    where c.Pais == "Spain"
                    select new
                    {
                        NombreCliente = c.NombreCliente,
                        Telefono = c.Telefono,
                        Ciudad = c.Ciudad
                    };

        if (!string.IsNullOrEmpty(search))
        {
            var lowerSearch = search.ToLower();
            query = query.Where(m => m.NombreCliente.ToLower().Contains(lowerSearch));
        }

        int totalRegistros = await query.CountAsync();

        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
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

    //Consulta 3 con paginación
    public async Task<(int totalRegistros, IEnumerable<Object> registros)> Pago2008Paginated(int pageIndex, int pageSize, string search = null)
    {
        var query = from p in _context.Pagos
                    join c in _context.Clientes on p.CodigoCliente equals c.Id
                    where p.FechaPago.Year == 2008
                    group p by p.CodigoCliente into Grupo
                    select new
                    {
                        CodigoCliente = Grupo.Key
                    };

        if (!string.IsNullOrEmpty(search))
        {
            var lowerSearch = search.ToLower();
            query = query.Where(m => m.CodigoCliente.ToString().Contains(lowerSearch));
        }

        int totalRegistros = await query.CountAsync();

        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }

    //Consulta 11
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

    //Consulta 12
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

    //Consulta 13
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

    //Consulta 14
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

    //Consulta 15
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

    //Consulta 16
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

    //Consulta 18
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

    //Consulta 20
    public async Task<IEnumerable<object>> ClientesSinPago()
    {
        var clientes = await (
            from c in _context.Clientes
            join p in _context.Pagos on c.Id equals p.CodigoCliente into GrupoPagos
            where !GrupoPagos.Any()
            select new
            {
                NombreCliente = c.NombreCliente
            }
        ).ToListAsync();

        return clientes;
    }

    //Consulta 21
    public async Task<IEnumerable<object>> ClientesSinPagoYSinPedido()
    {
        var clientes = await (
            from c in _context.Clientes
            where !_context.Pagos.Any(p => p.CodigoCliente == c.Id) && !_context.Pedidos.Any(pe => pe.CodigoCliente == c.Id)
            select new
            {
                NombreCliente = c.NombreCliente
            }
        ).ToListAsync();

        return clientes;
    }

    //Consulta 27
    public async Task<IEnumerable<object>> ClientesConPedidoYSinPago()
    {
        var clientes = await (
            from c in _context.Clientes
            where _context.Pedidos.Any(p => p.CodigoCliente == c.Id) && !_context.Pagos.Any(pe => pe.CodigoCliente == c.Id)
            select new
            {
                Id = c.Id,
                NombreCliente = c.NombreCliente
            }
        ).ToListAsync();

        return clientes;
    }

    //Consulta 30
    public async Task<IEnumerable<object>> TotalClientesPorPais()
    {
        var clientes = await (
            from c in _context.Clientes
            group c by c.Pais into grupo
            select new
            {
                Pais = grupo.Key,
                CantidadClientes = grupo.Count()
            }
        ).ToListAsync();

        return clientes;
    }

    //Consulta 33
    public async Task<int> ClientesEnMadrid()
    {
        var clientes = await _context.Clientes
            .Where(c => c.Ciudad == "Madrid")
            .CountAsync();

        return clientes;
    }

    //Consulta 34
    public async Task<IEnumerable<object>> ClientesPorCiudadM()
    {
        var clientes = await (
            from c in _context.Clientes
            where c.Ciudad.StartsWith("M") || c.Ciudad.StartsWith("m")
            group c by c.Ciudad into grupo
            select new
            {
                Ciudad = grupo.Key,
                CantidadClientes = grupo.Count()
            }
        ).ToListAsync();

        return clientes;
    }

    //Consulta 36
    public async Task<int> ClientesSinRepresentante()
    {
        var clientes = await _context.Clientes
            .Where(r => r.CodigoEmpleado == null)
            .CountAsync();

        return clientes;
    }

    //Consulta 37
    public async Task<IEnumerable<object>> PrimerYUltimoPagoPorCliente()
    {
        var pagos = await (
            from c in _context.Clientes
            join pago in _context.Pagos on c.Id equals pago.CodigoCliente into Grupo
            select new
            {
                Cliente = c.NombreCliente,
                PrimerPago = Grupo.Min(p => (DateOnly?)p.FechaPago),
                UltimoPago = Grupo.Max(p => (DateOnly?)p.FechaPago)
            }
        ).ToListAsync();

        return pagos;
    }

    //Consulta 45
    public async Task<string> ClienteConMayorLimiteDeCredito()
    {
        var clienteConMayorLimite = await _context.Clientes
            .Where(c => c.LimiteCredito == _context.Clientes.Max(x => x.LimiteCredito))
            .Select(c => c.NombreCliente)
            .FirstOrDefaultAsync();

        return clienteConMayorLimite;
    }

    //Consulta 48
    public async Task<IEnumerable<object>> ClientesConLimiteDeCreditoMayorAPagos()
    {
        var clientes = await _context.Clientes
            .Where(c => c.Pagos.Any())
            .Where(c => c.LimiteCredito < c.Pagos.Sum(p => p.Total))
            .Select(c => c.NombreCliente)
            .ToListAsync();

        return clientes;
    }

    //Consulta 49
    public async Task<string> ClienteConMayorLimite()
    {
        var clienteConMayorLimite = await _context.Clientes
            .Where(c => c.LimiteCredito == _context.Clientes.Max(x => x.LimiteCredito))
            .Select(c => c.NombreCliente)
            .FirstOrDefaultAsync();

        return clienteConMayorLimite;
    }

    //Consulta 51
    public async Task<IEnumerable<object>> ClientesQueNoHanRealizadoNingunPago()
    {
        var clientes = await (
            from c in _context.Clientes
            join p in _context.Pagos on c.Id equals p.CodigoCliente into pagosGroup
            where !pagosGroup.Any()
            select new
            {
                NombreCliente = c.NombreCliente
            }
        ).ToListAsync();

        return clientes;
    }

    //Consulta 52
    public async Task<IEnumerable<object>> ClientesQueSiHanRealizadoAlgunPago()
    {
        var clientes = await (
            from c in _context.Clientes
            join p in _context.Pagos on c.Id equals p.CodigoCliente into pagos
            where pagos.Any()
            select new
            {
                NombreCliente = c.NombreCliente
            }
        ).ToListAsync();

        return clientes;
    }

    //Consulta 55
    public async Task<IEnumerable<object>> ClientesQueNoHanRealizadoPagos()
    {
        var clientes = await (
            from c in _context.Clientes
            join p in _context.Pagos on c.Id equals p.CodigoCliente into grupoDePagos
            where !grupoDePagos.Any()
            select new
            {
                NombreCliente = c.NombreCliente
            }
        ).ToListAsync();

        return clientes;
    }

    //Consulta 56
    public async Task<IEnumerable<object>> ClientesQueSiHanRealizadoPagos()
    {
        var clientes = await (
            from c in _context.Clientes
            join p in _context.Pagos on c.Id equals p.CodigoCliente into grupo
            where grupo.Any()
            select new
            {
                NombreCliente = c.NombreCliente
            }
        ).ToListAsync();

        return clientes;
    }

    //Consulta 57
    public async Task<IEnumerable<object>> ClientesYPedidos()
    {
        var clientes = await (
            from c in _context.Clientes
            select new
            {
                NombreCliente = c.NombreCliente,
                CantidadPedidos = c.Pedidos.Count()
            }
        ).ToListAsync();

        return clientes;
    }

    //Consulta 58
    public async Task<IEnumerable<object>> ClientesConPedidosEn2008()
    {
        var pedidos = await (
            from p in _context.Pedidos
            join c in _context.Clientes on p.CodigoCliente equals c.Id
            where p.FechaPedido.Year == 2008
            group p by p.CodigoCliente into Grupo
            select new
            {
                NombreCliente = Grupo.First().Cliente.NombreCliente
            })
            .OrderBy(cliente => cliente.NombreCliente)
            .ToListAsync();

        return pedidos;
    }

    //Consulta 59
    public async Task<IEnumerable<object>> InfoRepresentanteDeClientesSinPagos()
    {
        var representantes = await (
            from c in _context.Clientes
            join p in _context.Pagos on c.Id equals p.CodigoCliente into grupoDePagos
            where !grupoDePagos.Any()
            select new
            {
                NombreCliente = c.NombreCliente,
                RepresentanteDeVentas = (
                    from e in _context.Empleados
                    where e.Id == c.CodigoEmpleado  
                    select new 
                    {
                        Nombre = e.Nombre,
                        PrimerApellido = e.Apellido1,
                        NumOficina = e.Oficina.Telefono
                    }).FirstOrDefault()
            }
        ).ToListAsync();

        return representantes;
    }

    //Consulta 60
    public async Task<IEnumerable<object>> InfoRepresentanteDeClientes()
    {
        var representantes = await (
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
                        PrimerApellido = e.Apellido1,
                        CiudadOficina = e.Oficina.Ciudad
                    }).FirstOrDefault()
            }
        ).ToListAsync();

        return representantes;
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