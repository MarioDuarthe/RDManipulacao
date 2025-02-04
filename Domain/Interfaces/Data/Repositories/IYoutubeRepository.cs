
using Domain.Entities;

namespace Domain.Interfaces.Data.Repositories
{
    public interface IYoutubeRepository
    {
        Task<int> SaveYoutubeVideos(YoutubeVideosModel objVideos);
        Task<List<YoutubeVideosModel>> GetYoutubeVideos(string titulo, string duracao, string autor, DateTime? dataInicio, DateTime? dataFim, string q);
        Task SaveYoutubeVideosDetails(YoutubeVideosDetailsModel objVideosDetalhes);
        Task<YoutubeVideosDetailsModel> GetYoutubeVideosDetails(int fk_youtubeVideos);
        Task DeleteYoutubeVideosDetails(int id);
        Task UpdateYoutubeVideos(List<YoutubeVideosModel> objListaVideos);
    }
}
