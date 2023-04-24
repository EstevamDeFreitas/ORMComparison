using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Data.SqlClient;
using OrmUtilities;
using NHibernate.Hql.Ast.ANTLR.Tree;

namespace ADONET
{
    [MemoryDiagnoser]
    [SimpleJob(launchCount:1, warmupCount:1, iterationCount:1, invocationCount:1, baseline:true)]
    public class AdoNetMain: ITestBase
    {
        private static string ConnectionString { get; } = "Data Source=DESKTOP-L42IOG5;Initial Catalog=orm_comparisson;User ID=orm_user;Password=123456";

        private SqlConnection _connection { get; set; }

        private EntitiesInfo entitiesInfo { get; set; }

        [Params(500, 1000, 2000, 5000, 10000)]
        public int TestAmount { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _connection = new SqlConnection(ConnectionString);
            _connection.Open();

            entitiesInfo = new EntitiesInfo();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            InsertTestCleanup();

            _connection.Close();
        }

        [IterationCleanup(Target = nameof(RunInsertTest))]
        public void InsertTestCleanup()
        {
            var deleteStatement = "DELETE Endereco";
            SqlCommand command = new SqlCommand(deleteStatement, _connection);

            command.ExecuteNonQuery();
        }

        [Benchmark]
        public void RunInsertTest()
        {
            var insertStatement = "INSERT INTO Endereco (Id, Pais, Estado, Cidade, Rua, Numero) VALUES (@id, @pais, @estado, @cidade, @rua, @numero)";

            for (int i = 0; i < TestAmount; i++)
            {
                SqlCommand command = new SqlCommand(insertStatement, _connection);

                command.Parameters.AddWithValue("@id", entitiesInfo.Enderecos[i].Id);
                command.Parameters.AddWithValue("@pais", entitiesInfo.Enderecos[i].Pais);
                command.Parameters.AddWithValue("@estado", entitiesInfo.Enderecos[i].Estado);
                command.Parameters.AddWithValue("@cidade", entitiesInfo.Enderecos[i].Cidade);
                command.Parameters.AddWithValue("@rua", entitiesInfo.Enderecos[i].Rua);
                command.Parameters.AddWithValue("@numero", entitiesInfo.Enderecos[i].Numero);

                command.ExecuteNonQuery();
            }

        }

        public void RunInsertStudent()
        {
            throw new NotImplementedException();
        }

        public void RunUpdateStudent()
        {
            throw new NotImplementedException();
        }

        public void RunDeleteStudent()
        {
            throw new NotImplementedException();
        }

        public void RunGetStudent()
        {
            throw new NotImplementedException();
        }

        public void RunInsertTeacher()
        {
            throw new NotImplementedException();
        }

        public void RunUpdateTeacher()
        {
            throw new NotImplementedException();
        }

        public void RunDeleteTeacher()
        {
            throw new NotImplementedException();
        }

        public void RunGetTeacher()
        {
            throw new NotImplementedException();
        }
    }
}