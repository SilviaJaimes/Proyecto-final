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