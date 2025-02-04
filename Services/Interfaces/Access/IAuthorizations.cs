using Services.DTOs.Access;

namespace Services.Interfaces.Access
{
    public interface IAuthorizations
    {
        Task<object> CheckAccess(UsuarioDTO objUsuario);
    }
}
