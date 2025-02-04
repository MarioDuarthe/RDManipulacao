using Domain.Entities;

namespace Domain.Interfaces.Data.Repositories
{
    public interface IAccessRepository
    {
        Task CreateUser(UsuarioModel objUsuario);
        Task<UsuarioModel> GetUsuario(UsuarioModel objUsuario);
    }
}
