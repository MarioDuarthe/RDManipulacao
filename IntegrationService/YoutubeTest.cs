using Moq;
using Services.Interfaces.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationService
{
    public class YoutubeTest
    {
        private readonly Mock<IRequestManager> _mockRequestManager;
        //private readonly YoutubeService _youtubeService;

        public YoutubeTest()
        {
            _mockRequestManager = new Mock<IRequestManager>();
            //_youtubeService = new YoutubeService(_mockRequestManager.Object);
        }

        [Fact]
        public async Task GetVideo()
        {

        }
    }
}
