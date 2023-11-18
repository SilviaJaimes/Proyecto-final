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