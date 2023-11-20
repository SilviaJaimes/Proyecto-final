using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IEmpleado : IGenericRepository<Empleado>
{
    Task<IEnumerable<object>> EmpleadoConJefes();
    Task<IEnumerable<object>> EmpleadosSinClienteAsociado();
    Task<IEnumerable<object>> EmpleadoSinClienteYSinOficina();
    Task<IEnumerable<object>> EmpleadoSinCliente();
    Task<int> TotalEmpleados();
    Task<IEnumerable<object>> RepresentanteVentasConCantidadClientes();
    Task<IEnumerable<object>> EmpleadosQueNoSeanRepresentantesDeVentas();
    Task<IEnumerable<object>> InfoEmpleadosQueNoSeanRepresentantes();
}