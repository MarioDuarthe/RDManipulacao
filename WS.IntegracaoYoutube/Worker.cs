using Domain.Security;
using Services.Interfaces.RDManipulacao;
using System.Diagnostics;

namespace WS.IntegracaoYoutube
{
    public class Worker : BackgroundService
    {
        private IRDManipulacao _rdManipulacao;
        YoutubeConfiguration _youtubeCredencial = new YoutubeConfiguration();

        public Worker(IRDManipulacao rdManipulacao, YoutubeConfiguration youtubeCredencial)
        {
            this._youtubeCredencial = youtubeCredencial;
            this._rdManipulacao = rdManipulacao;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    EventLog.WriteEntry("(Servi�o Integra��o Youtube) Inicio", "vers�o 1.0.0.0");
                    await _rdManipulacao.InsertYoutubeVideos(_youtubeCredencial.VideosTemes, _youtubeCredencial.vidoesPeriodoAfter, _youtubeCredencial.videosPeriodoBefore, _youtubeCredencial.VideosRegion);

                    EventLog.WriteEntry("(Servi�o Integra��o Youtube) Fim", "vers�o 1.0.0.0");

                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("(Servi�o Integra��o Youtube) Falha ao iniciar", String.Format(ex.Message.ToString()));
                }

                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
    }
}
