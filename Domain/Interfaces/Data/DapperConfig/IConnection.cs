
namespace Domain.Interfaces.Data.DapperConfig
{
    public interface IConnection
    {
        Task ExecuteCreateCommand(string sqlQuery);
        Task<List<T>> ExecuteProcGetList<T>(string sqlQuery, object param);
        Task ExecuteQuery(string sqlQuery, object param);
        Task<List<T>> ExecuteGetList<T>(string sqlQuery, object param);
        Task<T> ExecuteGet<T>(string sqlQuery, object param);
    }
}
