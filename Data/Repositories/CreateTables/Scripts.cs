using Domain.Interfaces.Data.DapperConfig;
using Domain.Interfaces.Data.Repositories.CreateTables;

namespace Data.Repositories.CreateTables
{
    public class Scripts : IScripts
    {
        private readonly IConnection _connection;
        public Scripts(IConnection connection)
        {
            _connection = connection;
        }

        public async Task CreateTables()
        {
            try
            {
                List<string> listaTabelas =  await Tabelas();

                foreach(var tabela in listaTabelas)
                {
                    await _connection.ExecuteCreateCommand(tabela);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(">> Scripts :: Erro ao criar Tabelas " + ex.Message);
            }
        }

        private async Task<List<string>> Tabelas()
        {
            List<string> listaTabelas = new List<string>();

            listaTabelas.Add(@"
                 CREATE TABLE IF NOT EXISTS usuarios (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    nome TEXT NOT NULL,
                    usuario TEXT NOT NULL,
                    senha TEXT NOT NULL
                );");

            listaTabelas.Add(@"
                 CREATE TABLE IF NOT EXISTS youtubeVideos (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    kind TEXT NOT NULL,
                    videoId TEXT NOT NULL,
                    publishedAt TEXT NOT NULL,
                    channelId TEXT NOT NULL,
                    title TEXT NOT NULL,
                    description TEXT,
                    channelTitle TEXT NOT NULL,
                    liveBroadcastContent TEXT,
                    publishTime TEXT NOT NULL,
                    ativo INTEGER NOT NULL
                );");

            listaTabelas.Add(@"
                 CREATE TABLE IF NOT EXISTS youtubeVideosDetails (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    fk_youtubeVideos INTEGER NOT NULL,
                    categoryId TEXT,                    
                    duration TEXT,
                    dimension TEXT,
                    definition TEXT,
                    viewCount TEXT,
                    likeCount TEXT,
                    favoriteCount TEXT,
                    commentCount TEXT
                );");

            return listaTabelas;
        }
    }
}
