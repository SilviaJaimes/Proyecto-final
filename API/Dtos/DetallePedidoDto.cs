using Dominio.Entities;

namespace API.Dtos;

public class DetallePedidoDto : BaseEntity
{
    public int CodigoPedido { get; set; }
    public PedidoDto Pedido { get; set; }
    public string CodigoProducto { get; set; }
    public ProductoDto Producto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnidad { get; set; }
    public short NumeroLinea { get; set; }
}