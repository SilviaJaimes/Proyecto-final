using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IGamaProducto : IGenericRepoStr<GamaProducto>
{
    Task<IEnumerable<Object>> GamasPorCliente();
}