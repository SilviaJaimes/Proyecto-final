using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IProducto : IGenericRepoStr<Producto>
{
    Task<IEnumerable<Object>> ProductosOrnamentalesYMás100Unidades();
    Task<IEnumerable<object>> ProductosSinPedido();
    Task<IEnumerable<object>> ProductosSinPedidoDescripcion();
    Task<IEnumerable<object>> ProductosMasVendidos();
    /* Task<IEnumerable<object>> ProductosMasVendidosAgrupadaPorCodigo();
    Task<IEnumerable<object>> ProductosMasVendidosAgrupadaPorCodigoEmpiecenPorOR();
    Task<IEnumerable<Object>> ProductosMásDe3000E(); */
    Task<string> ProductoConPrecioMasCaro();
    Task<string> ProductoConMayorDeUnidadesVendidas();
    Task<string> ProductoConPrecioDeVentaMasCaro();
    Task<IEnumerable<object>> ProductosQueNuncaHanAparecidoEnPedidos();
}