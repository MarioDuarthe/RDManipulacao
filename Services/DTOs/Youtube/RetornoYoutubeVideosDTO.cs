
namespace Services.DTOs.Youtube
{
    public class RetornoYoutubeVideosDTO
    {
        public int id { get; set; }
        public string kind { get;  set; }
        public string videoId { get;  set; }
        public DateTime publishedAt { get;  set; }
        public string channelId { get;  set; }
        public string title { get;  set; }
        public string description { get;  set; }
        public string channelTitle { get;  set; }
        public string liveBroadcastContent { get;  set; }
        public DateTime publishTime { get;  set; }
        public int ativo { get; set; }
        public RetornoYoutubeVideosDetalheDTO detalhesVideo { get; set; }
    }

    public class RetornoYoutubeVideosDetalheDTO
    {
        public int id { get; set; }
        public int fk_youtubeVideos { get; set; }
        public string categoryId { get;  set; }
        public string duration { get;  set; }
        public string dimension { get;  set; }
        public string definition { get;  set; }
        public string viewCount { get;  set; }
        public string likeCount { get;  set; }
        public string favoriteCount { get;  set; }
        public string commentCount { get;  set; }
    }
}
