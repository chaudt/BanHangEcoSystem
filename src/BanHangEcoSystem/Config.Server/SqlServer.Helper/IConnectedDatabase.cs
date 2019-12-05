using System.Data;

namespace Config.Server
{
    public interface IConnectedDatabase
    {
        IDbConnection GetConnection();
    }
}
