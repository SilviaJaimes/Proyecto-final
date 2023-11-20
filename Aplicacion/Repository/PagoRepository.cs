using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class PagoRepository : GenericRepoStr<Pago>, IPago
{
    private readonly ApiContext _context;

    public PagoRepository(ApiContext context) : base(context)
    {
        _context = context;
    }

    //Consulta 8
    public async Task<IEnumerable<Object>> PagosEn2008()
    {
        var pagos = await _context.Pagos
            .Where (p => p.FechaPago.Year == 2008 && p.FormaPago == "PayPal".ToLower())
            .Select(p => new
            {
                Total = p.Total
            }).OrderByDescending(pa => pa.Total)
            .ToListAsync();

        return pagos;
    }

    //Consulta 8 con paginación
    public async Task<(int totalRegistros, IEnumerable<Object> registros)> PagosEn2008Paginated(int pageIndex, int pageSize, string search = null)
    {
        var query = _context.Pagos
                    .Where (p => p.FechaPago.Year == 2008 && p.FormaPago == "PayPal".ToLower())
                    .Select(p => new
                    {
                        Total = p.Total
                    }).OrderByDescending(pa => pa.Total);

        if (!string.IsNullOrEmpty(search))
        {
            var lowerSearch = search.ToLower();
        }

        int totalRegistros = await query.CountAsync();

        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }

    //Consulta 9
    public async Task<IEnumerable<Object>> FormasPago()
    {
        var pagos = await (
            from p in _context.Pagos
            group p by p.FormaPago into Grupo
            select new
            {
                FormaPago = Grupo.Key
            }).ToListAsync();

        return pagos;
    }

    //Consulta 9 con paginación
    public async Task<(int totalRegistros, IEnumerable<Object> registros)> FormasPagoPaginated(int pageIndex, int pageSize, string search = null)
    {
        var query = from p in _context.Pagos
                    group p by p.FormaPago into Grupo
                    select new
                    {
                        FormaPago = Grupo.Key
                    };

        if (!string.IsNullOrEmpty(search))
        {
            var lowerSearch = search.ToLower();
            query = query.Where(m => m.FormaPago.ToString().Contains(lowerSearch));
        }

        int totalRegistros = await query.CountAsync();

        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }

    //Consulta 31
    public async Task<decimal> PagoMedio2009()
    {
        var pagos = await (
            from p in _context.Pagos
            where p.FechaPago.Year == 2009
            select p.Total
            ).AverageAsync();

        return pagos;
    }

    //Consulta 44
    public async Task<IEnumerable<object>> TotalPagosPorAño()
    {
        var clientes = await (
            from p in _context.Pagos
            group p by p.FechaPago.Year into grupo
            select new
            {
                Año = grupo.Key,
                Total = grupo.Sum(tp => tp.Total)
            }
        ).ToListAsync();

        return clientes;
    }

    public override async Task<IEnumerable<Pago>> GetAllAsync()
    {
        return await _context.Pagos
            .ToListAsync();
    }

    public override async Task<Pago> GetByIdAsync(string id)
    {
        return await _context.Pagos
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }

    public override async Task<(int totalRegistros, IEnumerable<Pago> registros)> GetAllAsync(int pageIndez, int pageSize, string search)
    {
        var query = _context.Pagos as IQueryable<Pago>;

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