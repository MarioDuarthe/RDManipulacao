using Services.DTOs;
using Services.DTOs.Youtube;

namespace Services.Interfaces.RDManipulacao
{
    public interface IRDManipulacao
    {
        Task<IDictionary<string, Boolean>> InsertYoutubeVideos(string tema, string dataInicial, string dataFinal, string regiao);
        Task UpdateYoutubeVideos(YoutubeVideosDTO objYoutubeVideos);
        Task<IDictionary<string, Boolean>> DeleteYoutubeVideos(int id);
        Task<List<RetornoYoutubeVideosDTO>> GetYoutubeVideos(string titulo, string duracao, string autor, DateTime? dataInicio, DateTime? dataFim, string q);
    }
}
