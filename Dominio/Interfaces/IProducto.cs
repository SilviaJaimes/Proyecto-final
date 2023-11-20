using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IProducto : IGenericRepoStr<Producto>
{
    Task<IEnumerable<Object>> ProductosOrnamentalesYMás100Unidades();
    Task<(int totalRegistros, IEnumerable<Object> registros)> ProductosOrnamentalesYMás100UnidadesPaginated(int pageIndex, int pageSize, string search = null);
    Task<IEnumerable<object>> ProductosSinPedido();
    Task<(int totalRegistros, IEnumerable<Object> registros)> ProductosSinPedidoPaginated(int pageIndex, int pageSize, string search = null);
    Task<IEnumerable<object>> ProductosSinPedidoDescripcion();
    Task<IEnumerable<object>> ProductosMasVendidos();
    Task<IEnumerable<object>> ProductosMasVendidosPorCodigo();
    Task<IEnumerable<object>> ProductosMasVendidosPorCodigoFiltrados();
    Task<IEnumerable<Object>> VentasProductosMas3000Euros();
    Task<string> ProductoConPrecioMasCaro();
    Task<string> ProductoConMayorDeUnidadesVendidas();
    Task<string> ProductoConPrecioDeVentaMasCaro();
    Task<IEnumerable<object>> ProductosQueNuncaHanAparecidoEnPedidos();
}