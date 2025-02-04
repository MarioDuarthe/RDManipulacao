using Domain.Entities;
using Domain.Interfaces.Data.DapperConfig;
using Domain.Interfaces.Data.Repositories;

namespace Data.Repositories
{
    public class AccessRepository : IAccessRepository
    {
        private readonly IConnection _db;
        public AccessRepository(IConnection db)
        {
            this._db = db;
        }

        public async Task CreateUser(UsuarioModel objUsuario)
        {
            try
            {
                string sqlConsulta = @"SELECT COUNT(1) FROM usuarios WHERE nome = @nome AND senha = @senha";

                var qtde = await Task.Run(() => _db.ExecuteGet<int>(sqlConsulta, objUsuario));

                if (qtde.Equals(0))
                {
                    string sql = @"INSERT INTO usuarios (nome, usuario, senha)
                               VALUES (@nome, @usuario, @senha)";

                    await Task.Run(() => _db.ExecuteQuery(sql, objUsuario));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(">> Insert :: tabela usuarios " + ex.Message);
            }
        }

        public async Task<UsuarioModel> GetUsuario(UsuarioModel objUsuario)
        {
            try
            {
                string sql = @"SELECT nome, usuario, senha FROM usuarios WHERE usuario = @usuario AND senha = @senha LIMIT 1";

                return await Task.Run(() => _db.ExecuteGet<UsuarioModel>(sql, objUsuario));
            }
            catch (Exception ex)
            {
                throw new Exception(">> Consulta :: tabela usuarios " + ex.Message);
            }
        }
    }
}
