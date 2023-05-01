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
        private static string ConnectionString { get; } = "Data Source=DESKTOP-QAM9OUA\\SQLEXPRESS;Initial Catalog=orm_comparissondb;User ID=orm_user;Password=123456";

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
            //InsertTestCleanup();

            InsertStudentCleanup();

            _connection.Close();
        }

        

        //[Benchmark]
        //public void RunInsertTest()
        //{
        //    var insertStatement = "INSERT INTO Endereco (Id, Pais, Estado, Cidade, Rua, Numero) VALUES (@id, @pais, @estado, @cidade, @rua, @numero)";

        //    for (int i = 0; i < TestAmount; i++)
        //    {
        //        SqlCommand command = new SqlCommand(insertStatement, _connection);

        //        command.Parameters.AddWithValue("@id", entitiesInfo.Enderecos[i].Id);
        //        command.Parameters.AddWithValue("@pais", entitiesInfo.Enderecos[i].Pais);
        //        command.Parameters.AddWithValue("@estado", entitiesInfo.Enderecos[i].Estado);
        //        command.Parameters.AddWithValue("@cidade", entitiesInfo.Enderecos[i].Cidade);
        //        command.Parameters.AddWithValue("@rua", entitiesInfo.Enderecos[i].Rua);
        //        command.Parameters.AddWithValue("@numero", entitiesInfo.Enderecos[i].Numero);

        //        command.ExecuteNonQuery();
        //    }

        //}

        //[IterationCleanup(Target = nameof(RunInsertTest))]
        //public void InsertTestCleanup()
        //{
        //    var deleteStatement = "DELETE Endereco";
        //    SqlCommand command = new SqlCommand(deleteStatement, _connection);

        //    command.ExecuteNonQuery();
        //}

        [Benchmark]
        public void RunInsertStudent()
        {
            for (int i = 0; i < TestAmount; i++)
            {
                #region Insert Address
                var insertAddressStatement = "INSERT INTO Endereco (Id, Pais, Estado, Cidade, Rua, Numero) VALUES (@id, @pais, @estado, @cidade, @rua, @numero)";

                SqlCommand commandAddress = new SqlCommand(insertAddressStatement, _connection);

                commandAddress.Parameters.AddWithValue("@id", entitiesInfo.Estudantes[i].Pessoa.Endereco.Id);
                commandAddress.Parameters.AddWithValue("@pais", entitiesInfo.Estudantes[i].Pessoa.Endereco.Pais);
                commandAddress.Parameters.AddWithValue("@estado", entitiesInfo.Estudantes[i].Pessoa.Endereco.Estado);
                commandAddress.Parameters.AddWithValue("@cidade", entitiesInfo.Estudantes[i].Pessoa.Endereco.Cidade);
                commandAddress.Parameters.AddWithValue("@rua", entitiesInfo.Estudantes[i].Pessoa.Endereco.Rua);
                commandAddress.Parameters.AddWithValue("@numero", entitiesInfo.Estudantes[i].Pessoa.Endereco.Numero);

                commandAddress.ExecuteNonQuery();
                #endregion

                #region Insert Person
                var insertPersonStatement = "INSERT INTO Pessoa (Id,PrimeiroNome,UltimoNome,NumeroTelefone,DataNascimento,EnderecoId) VALUES (@id,@primeiroNome,@ultimoNome,@numeroTel,@dataNasc,@enderecoId)";

                SqlCommand commandPerson = new SqlCommand(insertPersonStatement, _connection);

                commandPerson.Parameters.AddWithValue("@id", entitiesInfo.Estudantes[i].Pessoa.Id);
                commandPerson.Parameters.AddWithValue("@primeiroNome", entitiesInfo.Estudantes[i].Pessoa.PrimeiroNome);
                commandPerson.Parameters.AddWithValue("@ultimoNome", entitiesInfo.Estudantes[i].Pessoa.UltimoNome);
                commandPerson.Parameters.AddWithValue("@numeroTel", entitiesInfo.Estudantes[i].Pessoa.NumeroTelefone);
                commandPerson.Parameters.AddWithValue("@dataNasc", entitiesInfo.Estudantes[i].Pessoa.DataNascimento);
                commandPerson.Parameters.AddWithValue("@enderecoId", entitiesInfo.Estudantes[i].Pessoa.EnderecoId);

                commandPerson.ExecuteNonQuery();
                #endregion

                #region Insert Student
                var insertStudentStatement = "INSERT INTO Estudante (Id,PessoaId,Descricao) VALUES (@id,@pessoaId,@descricao)";

                SqlCommand commandStudent = new SqlCommand(insertStudentStatement, _connection);
                commandStudent.Parameters.AddWithValue("@id", entitiesInfo.Estudantes[i].Id);
                commandStudent.Parameters.AddWithValue("@pessoaId", entitiesInfo.Estudantes[i].PessoaId);
                commandStudent.Parameters.AddWithValue("@descricao", entitiesInfo.Estudantes[i].Descricao);

                commandStudent.ExecuteNonQuery();

                #endregion
            }
        }

        [IterationCleanup(Target = nameof(RunInsertStudent))]
        public void InsertStudentCleanup()
        {
            var deleteEstudanteStatement = "DELETE Estudante";
            SqlCommand commandEstudante = new SqlCommand(deleteEstudanteStatement, _connection);
            commandEstudante.ExecuteNonQuery();

            var deletePessoaStatement = "DELETE Pessoa";
            SqlCommand commandPessoa = new SqlCommand(deletePessoaStatement, _connection);
            commandPessoa.ExecuteNonQuery();

            var deleteEnderecoStatement = "DELETE Endereco";
            SqlCommand commandEndereco = new SqlCommand(deleteEnderecoStatement, _connection);
            commandEndereco.ExecuteNonQuery();
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