using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IProducto : IGenericRepoStr<Producto>
{
    Task<IEnumerable<Object>> ProductosOrnamentalesYMás100Unidades();
}