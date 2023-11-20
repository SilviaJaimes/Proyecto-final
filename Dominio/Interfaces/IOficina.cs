using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IOficina : IGenericRepoStr<Oficina>
{
    Task<IEnumerable<object>> OficinaSinEmpleadoRepresentante();
    Task<(int totalRegistros, IEnumerable<Object> registros)> OficinaSinEmpleadoRepresentantePaginated(int pageIndex, int pageSize, string search = null);
}