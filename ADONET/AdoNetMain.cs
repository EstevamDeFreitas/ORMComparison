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

            RunInsertTest(connection);
        }

        public void RunInsertTest(SqlConnection conn)
        {
            string query = "INSERT INTO Endereco (Id, Pais, Estado, Cidade, Rua, Numero) VALUES (@enderecoId, 'brasil', 'São Paulo', 'São Paulo', 'Rua A', '123')";
            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@enderecoId", Guid.NewGuid());

            var result = command.ExecuteNonQuery();

            Console.WriteLine("Linhas afetadas: " + result);
        }
    }
}