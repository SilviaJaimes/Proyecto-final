using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IPago : IGenericRepoStr<Pago>
{
    Task<IEnumerable<Object>> PagosEn2008();
    Task<IEnumerable<Object>> FormasPago();
    Task<decimal> PagoMedio2009();
}