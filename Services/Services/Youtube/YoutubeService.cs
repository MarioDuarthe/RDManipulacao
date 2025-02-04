using Domain.Security;
using Services.DTOs;
using Services.DTOs.Youtube;
using Services.Interfaces.Requests;
using Services.Interfaces.Youtube;
using System.Text.Json;

namespace Services.Services.Youtube
{
    public class YoutubeService : IYoutube
    {
        private readonly IRequestManager _requestService;
        YoutubeConfiguration _youtubeCredencial = new YoutubeConfiguration();
        private string _credencial = "";
        public YoutubeService(IRequestManager requestService, YoutubeConfiguration youtubeCredencial)
        {
            this._requestService = requestService;
            this._youtubeCredencial = youtubeCredencial;
            _credencial = Environment.GetEnvironmentVariable(_youtubeCredencial.Credencial);
        }

        public async Task<YoutubeVideosDTO> GetVideos(string tema, string dataInicial, string dataFinal, string regiao)
        {
            try
            {
                string url = $@"https://www.googleapis.com/youtube/v3/search?part=snippet&q={tema}&publishedAfter={dataInicial}Z&publishedBefore={dataFinal}Z&regionCode={regiao}&key={_credencial}";

                IDictionary<string, string> dicHeaderAuth = new Dictionary<string, string>();
                IDictionary<string, string> response = null;

                response = await _requestService.GetMethod(url, null, 60000, dicHeaderAuth);

                if (response.Any())
                {
                    if (response["httpResponse"].ToString().Equals("200"))
                    {
                        string body = response["Body"].ToString();
                        JsonDocument doc = JsonDocument.Parse(body);
                        return JsonSerializer.Deserialize<YoutubeVideosDTO>(doc) ?? new YoutubeVideosDTO();
                    }
                    else { return new YoutubeVideosDTO(); }
                }
                else { return new YoutubeVideosDTO(); }
            }
            catch (Exception ex)
            {
                throw new Exception(">> Erro no método GetVideo :: " + ex.Message);
            }
        }

        public async Task<YoutubeVideosDetailsDTO> GetVideosDetails(string idsVideos)
        {
            try
            {
                string url = $@"https://www.googleapis.com/youtube/v3/videos?part=snippet,contentDetails,statistics&id={idsVideos}&key={_credencial}";

                IDictionary<string, string> dicHeaderAuth = new Dictionary<string, string>();
                IDictionary<string, string> response = null;

                response = await _requestService.GetMethod(url, null, 60000, dicHeaderAuth);

                if (response.Any())
                {
                    if (response["httpResponse"].ToString().Equals("200"))
                    {
                        string body = response["Body"].ToString();
                        JsonDocument doc = JsonDocument.Parse(body);
                        return JsonSerializer.Deserialize<YoutubeVideosDetailsDTO>(doc) ?? new YoutubeVideosDetailsDTO();
                    }
                    else { return new YoutubeVideosDetailsDTO(); }
                }
                else { return new YoutubeVideosDetailsDTO(); }
            }
            catch (Exception ex)
            {
                throw new Exception(">> Erro no método GetVideosDetails :: " + ex.Message);
            }
        }
    }
}
