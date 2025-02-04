
namespace Services.Interfaces.Requests
{
    public interface IRequestManager
    {
        Task<IDictionary<string, string>> GetMethod(string sUrl, object body, int timeOut, IDictionary<string, string> dicHeader);
    }
}
