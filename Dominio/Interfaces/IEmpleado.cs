using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IEmpleado : IGenericRepository<Empleado>
{
    Task<IEnumerable<object>> EmpleadoConJefes();
    Task<(int totalRegistros, IEnumerable<Object> registros)> EmpleadoConJefesPaginated(int pageIndex, int pageSize, string search = null);
    Task<IEnumerable<object>> EmpleadosSinClienteAsociado();
    Task<(int totalRegistros, IEnumerable<Object> registros)> EmpleadosSinClienteAsociadoPaginated(int pageIndex, int pageSize, string search = null);
    Task<IEnumerable<object>> EmpleadoSinClienteYSinOficina();
    Task<IEnumerable<object>> EmpleadoSinCliente();
    Task<int> TotalEmpleados();
    Task<IEnumerable<object>> RepresentanteVentasConCantidadClientes();
    Task<IEnumerable<object>> EmpleadosQueNoSeanRepresentantesDeVentas();
    Task<IEnumerable<object>> InfoEmpleadosQueNoSeanRepresentantes();
}