
namespace Domain.Entities
{
    public class YoutubeVideosDetailsModel
    {
        public int id {  get; private set; }
        public int fk_youtubeVideos { get; private set; }
        public string categoryId { get; private set; }
        public string duration { get; private set; }
        public string dimension { get; private set; }
        public string definition { get; private set; }
        public string viewCount { get; private set; }
        public string likeCount { get; private set; }
        public string favoriteCount { get; private set; }
        public string commentCount { get; private set; }

        public YoutubeVideosDetailsModel() {}

        public YoutubeVideosDetailsModel(int id,
            int fk_youtubeVideos,
            string categoryId,
            string duration,
            string dimension,
            string definition,
            string viewCount,
            string likeCount,
            string favoriteCount,
            string commentCount)
        {
            this.id = id;
            this.fk_youtubeVideos = fk_youtubeVideos;
            this.categoryId = categoryId;
            this.duration = duration;
            this.dimension = dimension;
            this.definition = definition;
            this.viewCount = viewCount;
            this.likeCount = likeCount;
            this.favoriteCount = favoriteCount;
            this.commentCount = commentCount;    
        }
    }
}
