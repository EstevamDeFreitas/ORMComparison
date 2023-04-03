using System.Data.SqlClient;

namespace ADONET
{
    public class AdoNetMain
    {
        private string ConnectionString { get; set; }

        public AdoNetMain(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void InitTest()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            connection.Open();

            string sql = "SELECT * FROM pessoa";

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetGuid(0));
            }

            reader.Close();
        }
    }
}