using Microsoft.Data.SqlClient;

namespace ReservasPruebaUNI.Services
{
    public class DbFactory
    {
        private readonly string _connectionString;

        public DbFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public SqlConnection Create()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
