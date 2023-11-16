using Dominio.Entities;

namespace Dominio.Interfaces;

public interface ICliente : IGenericRepository<Cliente>
{
    Task<IEnumerable<Object>> ClientesEspañoles();
    Task<IEnumerable<Object>> Pago2008();
    Task<IEnumerable<Object>> ClientesMadridYRVConCodigo11O30();
}