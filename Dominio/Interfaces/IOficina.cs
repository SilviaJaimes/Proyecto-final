using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IOficina : IGenericRepoStr<Oficina>
{
    Task<IEnumerable<object>> OficinaSinEmpleadoRepresentante();
}