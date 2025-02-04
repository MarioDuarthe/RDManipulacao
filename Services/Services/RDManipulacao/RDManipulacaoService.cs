using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Data.Repositories;
using Services.DTOs;
using Services.DTOs.Access;
using Services.DTOs.Youtube;
using Services.Interfaces.RDManipulacao;
using Services.Interfaces.Youtube;
using Services.Utils;

namespace Services.Services.RDManipulacao
{
    public class RDManipulacaoService : IRDManipulacao
    {
        private IMapper _mapper;
        private readonly IYoutubeRepository _youtubeRepository;
        private readonly IYoutube _youtubeService;
        private readonly IAccessRepository _accessRepository;
        public RDManipulacaoService(IMapper mapper, IYoutubeRepository youtubeRepository, IYoutube youtubeService, IAccessRepository accessRepository)
        {
            this._mapper = mapper;
            this._youtubeRepository = youtubeRepository;
            this._youtubeService = youtubeService;
            this._accessRepository = accessRepository;
        }
        public async Task<IDictionary<string, Boolean>> InsertYoutubeVideos(string tema, string dataInicial, string dataFinal, string regiao)
        {
            try
            {
                #region cria um usuario no banco caso ainda não exista
                UsuarioDTO usuario = new UsuarioDTO()
                {
                    nome = "administrador",
                    usuario = "admin",
                    senha = "youtube"
                };

                var objUsuarioModel = _mapper.Map<UsuarioModel>(usuario);

                await _accessRepository.CreateUser(objUsuarioModel);
                #endregion

                var dadosYoutube = await _youtubeService.GetVideos(tema, dataInicial, dataFinal, regiao);

                foreach (var dadoYoutube in dadosYoutube.items)
                {
                    dadoYoutube.ativo = 1;
                    var objVideoModel = _mapper.Map<YoutubeVideosModel>(dadoYoutube);

                    int id = await _youtubeRepository.SaveYoutubeVideos(objVideoModel);

                    await InsertYoutubeVideosDetails(dadoYoutube.id.videoId, id);
                }

                IDictionary<string, Boolean> retorno = new Dictionary<string, Boolean>();
                retorno.Add("Sucesso", true);
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(">> (Serviço Youtube) Erro ao inserir dados do youtube :: " + ex.Message);
            }
        }
        public async Task UpdateYoutubeVideos(YoutubeVideosDTO objYoutubeVideos)
        {
            try
            {
                if (objYoutubeVideos != null)
                {
                    var listaObjVideo = _mapper.Map<List<YoutubeVideosModel>>(objYoutubeVideos);
                    await _youtubeRepository.UpdateYoutubeVideos(listaObjVideo);

                    //await InsertYoutubeVideosDetails(String.Join(",", objYoutubeVideos.items.Select(v => v.id.videoId).ToList()));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(">> (Serviço Youtube) Erro ao atualizar dados do youtube :: " + ex.Message);
            }
        }
        public async Task<IDictionary<string, Boolean>> DeleteYoutubeVideos(int id)
        {
            try
            {
                if (!id.Equals(0))
                {
                    await _youtubeRepository.DeleteYoutubeVideosDetails(id);

                    IDictionary<string, Boolean> retorno = new Dictionary<string, Boolean>();
                    retorno.Add("Sucesso", true);
                    return retorno;
                }
                else
                {
                    IDictionary<string, Boolean> retorno = new Dictionary<string, Boolean>();
                    retorno.Add("Sucesso", false);
                    return retorno;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(">> (Serviço Youtube) Erro ao deletar dados do youtube :: " + ex.Message);
            }
        }
        public async Task<List<RetornoYoutubeVideosDTO>> GetYoutubeVideos(string titulo, string duracao, string autor, DateTime? dataInicio, DateTime? dataFim, string q)
        {
            try
            {
                var objYoutubeVideosModel = await _youtubeRepository.GetYoutubeVideos(titulo, duracao, autor, dataInicio, dataFim, Common.LimpaCaracteresEspeciais(q));

                if (objYoutubeVideosModel.Any())
                {
                    var listaObjVideoDTO = _mapper.Map<List<RetornoYoutubeVideosDTO>>(objYoutubeVideosModel);

                    foreach(var item in listaObjVideoDTO)
                    {
                        var objDetalheModel = await _youtubeRepository.GetYoutubeVideosDetails(item.id);
                        item.detalhesVideo = _mapper.Map<RetornoYoutubeVideosDetalheDTO>(objDetalheModel);
                    }
                    return listaObjVideoDTO;
                }
                else
                {
                    return new List<RetornoYoutubeVideosDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(">> (Serviço Youtube) Erro ao atualizar dados do youtube :: " + ex.Message);
            }
        }
        private async Task InsertYoutubeVideosDetails(string videoId, int id)
        {
            try
            {
                var dadosYoutubeDetalhes = await _youtubeService.GetVideosDetails(videoId);                

                foreach (var item in dadosYoutubeDetalhes.items)
                {
                    item.fk_youtubeVideos = id;
                    var objVideoDetalhes = _mapper.Map<YoutubeVideosDetailsModel>(item);
                    await _youtubeRepository.SaveYoutubeVideosDetails(objVideoDetalhes);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(">> (Serviço Youtube) Erro ao inserir detalhes dos videos :: " + ex.Message);
            }
        }
    }
}
