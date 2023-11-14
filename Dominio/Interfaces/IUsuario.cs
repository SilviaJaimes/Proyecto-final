using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IUsuario : IGenericRepository<Usuario>
{
    Task<Usuario> GetByUsernameAsync(string nombre);
    Task<Usuario> GetByRefreshTokenAsync(string nombre);
}