using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IGamaProducto : IGenericRepoStr<GamaProducto>
{
    Task<IEnumerable<Object>> GamasPorCliente();
    Task<(int totalRegistros, IEnumerable<Object> registros)> GamasPorClientePaginated(int pageIndex, int pageSize, string search = null);
}