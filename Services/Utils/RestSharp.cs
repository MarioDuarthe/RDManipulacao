using Domain.Interfaces.Utils;
using RestSharp;
using System.Net;

namespace Services.Utils
{
    public class RestSharp : IRestSharp
    {
        public RestSharp()
        {
            
        }
        public async Task<IDictionary<string, string>> Get(string sUrl, object body, IDictionary<string, string> HeadersApi, int iTimeOut)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | SecurityProtocolType.Tls11
                       | SecurityProtocolType.Tls12;

                var options = new RestClientOptions(sUrl)
                {
                    MaxTimeout = iTimeOut
                };
                var client = new RestClient(options);

                var request = new RestRequest();

                request.AddHeader("Content-Type", "application/json");

                foreach (var header in HeadersApi)
                {
                    request.AddHeader(header.Key.ToString(), header.Value.ToString());
                }

                if (body != null)
                {
                    request.AddBody(body);
                }

                var responseRest = await client.ExecuteGetAsync(request);

                IDictionary<string, string> response = new Dictionary<string, string>();

                if (!responseRest.IsSuccessful)
                {
                    return response;
                }

                try
                {
                    response.Add("Body", responseRest.Content.ToString());
                }
                catch (Exception ex)
                {
                    throw new Exception(">> Erro ao converter content em dictionary " + ex.Message);
                }

                response.Add("httpResponse", responseRest.StatusCode.GetHashCode().ToString());

                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(">> Erro no metodo Get " + ex.Message);
            }
        }

        public async Task<IDictionary<string, string>> Post<T>(string sUrl, T dados, IDictionary<string, string> HeadersApi, int iTimeOut)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | SecurityProtocolType.Tls11
                       | SecurityProtocolType.Tls12;

                var options = new RestClientOptions(sUrl)
                {
                    MaxTimeout = iTimeOut
                };
                var client = new RestClient(options);

                var request = new RestRequest();

                request.AddHeader("Content-Type", "application/json");

                foreach (var header in HeadersApi)
                {
                    request.AddHeader(header.Key.ToString(), header.Value.ToString());
                }

                request.AddBody(dados);

                var responseRest = await client.ExecutePostAsync(request);

                IDictionary<string, string> response = new Dictionary<string, string>();

                try
                {
                    response.Add("Body", responseRest.Content.ToString());
                }
                catch (Exception ex)
                {

                }

                response.Add("httpResponse", responseRest.StatusCode.GetHashCode().ToString());

                return response;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
