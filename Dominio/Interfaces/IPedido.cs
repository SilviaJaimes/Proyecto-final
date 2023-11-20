using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IPedido : IGenericRepository<Pedido>
{
    Task<IEnumerable<Object>> EstadosPedido();
    Task<(int totalRegistros, IEnumerable<Object> registros)> EstadosPedidoPaginated(int pageIndex, int pageSize, string search = null);
    Task<IEnumerable<Object>> SinEntregarATiempo();
    Task<(int totalRegistros, IEnumerable<Object> registros)> SinEntregarATiempoPaginated(int pageIndex, int pageSize, string search = null);
    Task<IEnumerable<object>> DosDiasAntesFechaEsperada();
    Task<IEnumerable<Object>> PedidosRechazadosEn2009();
    Task<IEnumerable<Object>> PedidosEntregadosEnEnero();
    Task<IEnumerable<object>> PedidosPorEstado();
    Task<IEnumerable<object>> ProductosDiferentesPorPedido();
    Task<IEnumerable<object>> CantidadTotalDeProductosPorPedido();
}