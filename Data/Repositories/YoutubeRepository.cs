using Domain.Entities;
using Domain.Interfaces.Data.DapperConfig;
using Domain.Interfaces.Data.Repositories;
using Domain.Interfaces.Data.Repositories.CreateTables;
using System.Text;


namespace Data.Repositories
{
    public class YoutubeRepository : IYoutubeRepository
    {
        private readonly IConnection _db;
        private readonly IScripts _scripts;
        private readonly IYoutubeRepository _youtubeRepository;
        public YoutubeRepository(IScripts scripts, IConnection db)
        {
            this._scripts = scripts;
            _scripts.CreateTables();
            this._db = db;
        }

        public async Task<int> SaveYoutubeVideos(YoutubeVideosModel objVideos)
        {
            try
            {
                string sql = @"INSERT INTO youtubeVideos (kind, videoId, publishedAt, channelId, title, description, channelTitle, liveBroadcastContent, publishTime, ativo)
                               VALUES (@kind, @videoId, @publishedAt, @channelId, @title, @description, @channelTitle, @liveBroadcastContent, @publishTime, @ativo)";

                await Task.Run(() => _db.ExecuteQuery(sql, objVideos));

                string lastInsertIdQuery = "SELECT last_insert_rowid()";

                return await Task.Run(() => _db.ExecuteGet<int>(lastInsertIdQuery, null));
            }
            catch (Exception ex)
            {
                throw new Exception(">> Insert :: tabela youtubeVideos " + ex.Message);
            }
        }
        public async Task SaveYoutubeVideosDetails(YoutubeVideosDetailsModel objVideosDetalhes)
        {
            try
            {
                string sql = @"INSERT INTO youtubeVideosDetails (fk_youtubeVideos, categoryId, duration, dimension, definition,
                                                                 viewCount, likeCount, favoriteCount, commentCount )
                               VALUES (@fk_youtubeVideos, @categoryId, @duration, @dimension, @definition,
                                                                 @viewCount, @likeCount, @favoriteCount, @commentCount )";

                await Task.Run(() => _db.ExecuteQuery(sql, objVideosDetalhes));
            }
            catch (Exception ex)
            {
                throw new Exception(">> Insert :: tabela youtubeVideos " + ex.Message);
            }
        }
        public async Task<List<YoutubeVideosModel>> GetYoutubeVideos(string titulo, string duracao, string autor, DateTime? dataInicio, DateTime? dataFim, string q)
        {
            try
            {
                StringBuilder and = new StringBuilder();

                if (!String.IsNullOrEmpty(titulo))                
                    and.Append($" AND y.title = '{titulo}' ");
                if (!String.IsNullOrEmpty(duracao))
                    and.Append($" AND d.duration = '{duracao}' ");
                if (!String.IsNullOrEmpty(autor))
                    and.Append($" AND y.channelTitle = '{autor}' ");
                if (dataInicio.HasValue && dataFim.HasValue)
                    and.Append($" AND y.publishedAt BETWEEN '{dataInicio}' AND '{dataFim}' ");

                StringBuilder orderBy = new StringBuilder();

                switch(q.ToUpper())
                {
                    case "TITULO":
                        orderBy.Append($" ORDER BY y.title ");
                        break;
                    case "DESCRICAO":
                        orderBy.Append($" ORDER BY y.description ");
                        break;
                    case "NOME DO CANAL":
                        orderBy.Append($" ORDER BY y.channelTitle ");
                        break;
                    default:
                        orderBy.Append($" ORDER BY y.videoId ");
                        break;
                }

                string sql = $@"SELECT DISTINCT  y.*
                FROM youtubeVideos y 
                 JOIN youtubeVideosDetails d
                 ON y.id = d.fk_youtubeVideos
                 WHERE y.ativo = 1
                 {and}
                 {orderBy}";


                return await Task.Run(() => _db.ExecuteGetList<YoutubeVideosModel>(sql, null));
            }
            catch (Exception ex)
            {
                throw new Exception(">> Select :: tabela youtubeVideos " + ex.Message);
            }
        }

        public async Task<YoutubeVideosDetailsModel> GetYoutubeVideosDetails(int fk_youtubeVideos)
        {
            try
            {
                string sql = @"SELECT * FROM youtubeVideosDetails WHERE fk_youtubeVideos = @fk_youtubeVideos ";

                var param = new
                {
                    fk_youtubeVideos
                };

                return await Task.Run(() => _db.ExecuteGet<YoutubeVideosDetailsModel>(sql, param));
            }
            catch (Exception ex)
            {
                throw new Exception(">> Select :: tabela youtubeVideosDetails " + ex.Message);
            }
        }

        public async Task DeleteYoutubeVideosDetails(int id)
        {
            try
            {
                string sql = @"UPDATE youtubeVideos SET ativo = 0 WHERE id = @id";

                var param = new
                {
                    id
                };

                await Task.Run(() => _db.ExecuteQuery(sql, param));
            }
            catch (Exception ex)
            {
                throw new Exception(">> Delete :: tabela youtubeVideos " + ex.Message);
            }
        }
        public async Task UpdateYoutubeVideos(List<YoutubeVideosModel> objListaVideos)
        {
            try
            {
                string sql = @"UPDATE youtubeVideos SET kind = @kind , publishedAt = @publishedAt, channelId = @channelId, title = @title , description = @description , channelTitle = @channelTitle, 
                                   liveBroadcastContent = @liveBroadcastContent, publishTime = @publishTime WHERE videoId = @videoId";

                await Task.Run(() => _db.ExecuteQuery(sql, objListaVideos));
            }
            catch (Exception ex)
            {
                throw new Exception(">> Update :: tabela youtubeVideos " + ex.Message);
            }
        }
    }
}
