using Dominio.Entities;

namespace Dominio.Interfaces;

public interface ICliente : IGenericRepository<Cliente>
{
    Task<IEnumerable<Object>> ClientesEspañoles();
    Task<IEnumerable<Object>> Pago2008();
    Task<IEnumerable<Object>> ClientesMadridYRVConCodigo11O30();
    Task<IEnumerable<Object>> ClienteConSuRepresentante();
    Task<IEnumerable<object>> ClientesConPagos();
    Task<IEnumerable<object>> ClientesSinPagos();
    Task<IEnumerable<object>> ClientesConPagosRepresentanteYOficina();
    Task<IEnumerable<object>> ClientesSinPagosRepresentanteYOficina();
    Task<IEnumerable<object>> EmpleadoConJefes();
    Task<IEnumerable<object>> ClientesConPedidoTardio();
}