using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IEmpleado : IGenericRepository<Empleado>
{
    Task<IEnumerable<object>> EmpleadoConJefes();
    Task<IEnumerable<object>> EmpleadosSinClienteAsociado();
}