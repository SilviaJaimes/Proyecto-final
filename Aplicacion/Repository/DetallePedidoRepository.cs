using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class DetallePedidoRepository : GenericRepository<DetallePedido>, IDetallePedido
{
    private readonly ApiContext _context;

    public DetallePedidoRepository(ApiContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<DetallePedido>> GetAllAsync()
    {
        return await _context.DetallePedidos
            .ToListAsync();
    }

    public override async Task<DetallePedido> GetByIdAsync(int id)
    {
        return await _context.DetallePedidos
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }

    public override async Task<(int totalRegistros, IEnumerable<DetallePedido> registros)> GetAllAsync(int pageIndez, int pageSize, string search)
    {
        var query = _context.DetallePedidos as IQueryable<DetallePedido>;

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