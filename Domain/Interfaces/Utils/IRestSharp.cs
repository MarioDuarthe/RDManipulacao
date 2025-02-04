namespace Domain.Interfaces.Utils
{
    public interface IRestSharp
    {
        Task<IDictionary<string, string>> Get(string sUrl, object body, IDictionary<string, string> HeadersApi, int iTimeOut);
        Task<IDictionary<string, string>> Post<T>(string sUrl, T dados, IDictionary<string, string> HeadersApi, int iTimeOut);
    }
}
