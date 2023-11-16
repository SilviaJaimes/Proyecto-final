using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IPedido : IGenericRepository<Pedido>
{
    Task<IEnumerable<Object>> EstadosPedido();
    Task<IEnumerable<Object>> SinEntregarATiempo();
    Task<IEnumerable<object>> DosDiasAntesFechaEsperada();
    Task<IEnumerable<Object>> PedidosEntregadosEnEnero();
}