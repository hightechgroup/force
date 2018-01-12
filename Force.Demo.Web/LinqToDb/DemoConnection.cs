using System.Data;
using System.Data.SqlClient;
using Force.Demo.Domain;
using LinqToDB;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.SqlServer;
using StackExchange.Profiling;

namespace Force.Demo.Web.LinqToDb
{
    public class DemoConnection : LinqToDB.Data.DataConnection
    {
        public DemoConnection() : base(GetDataProvider(), GetConnection()) { }
        
        public ITable<Product> Products => GetTable<Product>();
        public ITable<Category> Categories => GetTable<Category>();

        private static IDataProvider GetDataProvider()
        {
            return new SqlServerDataProvider("", SqlServerVersion.v2012);
        }

        private static IDbConnection GetConnection()
        {
            LinqToDB.Common.Configuration.AvoidSpecificDataProviderAPI = true;

            var dbConnection = new SqlConnection(@"Server=.;Database=force;Trusted_Connection=True;Enlist=False;");
            return new StackExchange.Profiling.Data.ProfiledDbConnection(dbConnection, MiniProfiler.Current);
        }
    }
}