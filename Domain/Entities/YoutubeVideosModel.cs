namespace Domain.Entities
{
    public class YoutubeVideosModel
    {
        public int id {  get; private set; }
        public string kind { get; private set; }
        public string videoId { get; private set; }
        public DateTime publishedAt { get; private set; }
        public string channelId { get; private set; }
        public string title { get; private set; }
        public string description { get; private set; }
        public string channelTitle { get; private set; }
        public string liveBroadcastContent { get; private set; }
        public DateTime publishTime { get; private set; }
        public int ativo { get; private set; }

        public YoutubeVideosModel(){}

        public YoutubeVideosModel(
            int id,
            string kind,
            string videoId,
            DateTime publishedAt,
            string channelId,
            string title,
            string description,
            string channelTitle,
            string liveBroadcastContent,
            DateTime publishTime,
            int ativo) 
        { 
            this.id = id;
            this.kind = kind;
            this.videoId = videoId;
            this.publishedAt = publishedAt;
            this.channelId = channelId;
            this.title = title;
            this.description = description;
            this.channelTitle = channelTitle;
            this.liveBroadcastContent = liveBroadcastContent;
            this.publishTime = publishTime;
            this.ativo = ativo;
        }
    }
}
