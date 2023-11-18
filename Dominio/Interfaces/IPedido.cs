using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IPedido : IGenericRepository<Pedido>
{
    Task<IEnumerable<Object>> EstadosPedido();
    Task<IEnumerable<Object>> SinEntregarATiempo();
    Task<IEnumerable<object>> DosDiasAntesFechaEsperada();
    Task<IEnumerable<Object>> PedidosRechazadosEn2009();
    Task<IEnumerable<Object>> PedidosEntregadosEnEnero();
    Task<IEnumerable<object>> PedidosPorEstado();
        Task<IEnumerable<object>> ProductosDiferentesPorPedido();
}