using Services.DTOs;
using Services.DTOs.Youtube;

namespace Services.Interfaces.Youtube
{
    public interface IYoutube
    {
        Task<YoutubeVideosDTO> GetVideos(string tema, string dataInicial, string dataFinal, string regiao);
        Task<YoutubeVideosDetailsDTO> GetVideosDetails(string idsVideos);
    }
}
