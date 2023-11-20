using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IPago : IGenericRepoStr<Pago>
{
    Task<IEnumerable<Object>> PagosEn2008();
    Task<(int totalRegistros, IEnumerable<Object> registros)> PagosEn2008Paginated(int pageIndex, int pageSize, string search = null);
    Task<IEnumerable<Object>> FormasPago();
    Task<(int totalRegistros, IEnumerable<Object> registros)> FormasPagoPaginated(int pageIndex, int pageSize, string search = null);
    Task<decimal> PagoMedio2009();
    Task<IEnumerable<object>> TotalPagosPorAño();
}