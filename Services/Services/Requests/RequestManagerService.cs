using Domain.Interfaces.Utils;
using Services.Interfaces.Requests;

namespace Services.Services.Requests
{
    public class RequestManagerService : IRequestManager
    {
        private IRestSharp _restSharp;
        public RequestManagerService(IRestSharp restSharp)
        {
            this._restSharp = restSharp;
        }

        public async Task<IDictionary<string, string>> GetMethod(string sUrl, object body, int timeOut, IDictionary<string, string> dicHeader)
        {
            try
            {
                return await _restSharp.Get(sUrl, body, dicHeader, timeOut);
            }
            catch (Exception ex)
            {
                throw new Exception(">> (Serviço Integração Youtube) Erro ao fazer a requisição :: " + ex.Message);
            }
        }
    }
}
