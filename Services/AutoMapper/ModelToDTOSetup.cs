using AutoMapper;
using Domain.Entities;
using Services.DTOs.Access;
using Services.DTOs.Youtube;
using Item = Services.DTOs.Item;
using ItemDetails = Services.DTOs.Youtube.Item;

namespace Services.AutoMapper
{
    public class ModelToDTOSetup : Profile
    {
        public ModelToDTOSetup()
        {

            //CreateMap<YoutubeVideosDTO, List<YoutubeVideosModel>>()
            // .ConstructUsing(src => src.items.Select(item =>
            //     new YoutubeVideosModel(
            //         item.id.kind,
            //         item.id.videoId,
            //         item.snippet.publishedAt,
            //         item.snippet.channelId,
            //         item.snippet.title,
            //         item.snippet.description,
            //         item.snippet.channelTitle,
            //         item.snippet.liveBroadcastContent,
            //         item.snippet.publishTime,
            //         item.ativo.HasValue ? (int)item.ativo : 0
            //         )).ToList()).ReverseMap();

            //CreateMap<YoutubeVideosDetailsDTO, List<YoutubeVideosDetailsModel>>()
            //        .ConstructUsing(src => src.items.Select(item =>
            //           new YoutubeVideosDetailsModel(
            //               item.kind,
            //               item.id,
            //               item.snippet.publishedAt,
            //               item.snippet.channelId,
            //               item.snippet.title,
            //               item.snippet.description,
            //               item.snippet.categoryId,
            //               item.contentDetails.duration,
            //               item.contentDetails.dimension,
            //               item.contentDetails.definition,
            //               item.statistics.viewCount,
            //               item.statistics.likeCount,
            //               item.statistics.favoriteCount,
            //               item.statistics.commentCount
            //               )).ToList()).ReverseMap();

            //CreateMap<RetornoYoutubeVideosDTO, YoutubeVideosModel>()
            //    .ConstructUsing(m => new YoutubeVideosModel(m.kind, m.videoId, m.publishedAt, m.channelId, m.title, m.description, m.channelTitle, m.liveBroadcastContent, m.publishTime, m.ativo))
            //    .ReverseMap();

            //CreateMap<RetornoYoutubeVideosDetalheDTO, YoutubeVideosDetailsModel>()
            //    .ConstructUsing(m => new YoutubeVideosDetailsModel(m.kind, m.videoId, m.publishedAt, m.channelId, m.title, m.description, m.categoryId, m.duration, m.dimension, m.definition,
            //    m.viewCount, m.likeCount, m.favoriteCount, m.commentCount))
            //    .ReverseMap();

            CreateMap<Item, YoutubeVideosModel>()
                .ForMember(c => c.id, r => r.MapFrom(d => d.idControle))
                .ConstructUsing(src => new YoutubeVideosModel(src.idControle, src.kind, src.id.videoId, src.snippet.publishedAt, src.snippet.channelId,
                src.snippet.title, src.snippet.description, src.snippet.channelTitle, src.snippet.liveBroadcastContent, src.snippet.publishTime, 
                src.ativo.HasValue ? (int)src.ativo : 0))
                .ReverseMap();

            CreateMap<ItemDetails, YoutubeVideosDetailsModel>()
                .ForMember(c => c.id, r => r.MapFrom(d => d.idControle))
                .ConstructUsing(src => new YoutubeVideosDetailsModel(src.idControle, src.fk_youtubeVideos, src.snippet.categoryId, src.contentDetails.duration, src.contentDetails.dimension,
                src.contentDetails.definition, src.statistics.viewCount, src.statistics.likeCount, src.statistics.favoriteCount, src.statistics.commentCount))
                .ReverseMap();

            CreateMap<UsuarioDTO, UsuarioModel>()
                .ConstructUsing(u => new UsuarioModel(u.nome, u.usuario, u.senha))
                .ReverseMap();

            CreateMap<RetornoYoutubeVideosDTO, YoutubeVideosModel>()
                .ConstructUsing(m => new YoutubeVideosModel(m.id, m.kind, m.videoId, m.publishedAt, m.channelId, m.title, m.description, m.channelTitle, m.liveBroadcastContent,
                m.publishTime,  m.ativo))
                .ReverseMap();

            CreateMap<RetornoYoutubeVideosDetalheDTO, YoutubeVideosDetailsModel>()
                .ConstructUsing(m => new YoutubeVideosDetailsModel(m.id, m.fk_youtubeVideos, m.categoryId, m.duration, m.dimension, m.definition, m.viewCount, m.likeCount, m.favoriteCount, m.commentCount))
                .ReverseMap();

        }
    }
}
