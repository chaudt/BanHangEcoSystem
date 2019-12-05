using System.Data;
using System.Data.SqlClient;

namespace Config.Server
{
    public class ConnectedDatabase : IConnectedDatabase
    {
        #region IConnectedDatabase Members

        public IDbConnection GetConnection()
        {
            //return new SqlConnection(@"Server=113.166.120.51;Database=demo_message_bus;User Id=sa;Password=ssc1234%;");
            return new SqlConnection(@"Server=TCIS-SSC-50127\MSSQLSERVER1;Database=demo_message_bus;User Id=sa;Password=Viet1234%;");
        }

        #endregion
    }
}
