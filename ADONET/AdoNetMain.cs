using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Data.SqlClient;
using OrmUtilities;
using NHibernate.Hql.Ast.ANTLR.Tree;

namespace ADONET
{
    [MemoryDiagnoser]
    [SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 1, invocationCount: 1, baseline: true)]
    public class AdoNetMain : ITestBase
    {
        private static string ConnectionString { get; } = "Data Source=DESKTOP-QAM9OUA\\SQLEXPRESS;Initial Catalog=orm_comparisson;User ID=orm_user;Password=123456";

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

            IterationCleanup();

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

        [IterationCleanup]
        public void IterationCleanup()
        {
            var deleteCursoStatement = "DELETE Curso";
            SqlCommand commandCurso = new SqlCommand(deleteCursoStatement, _connection);
            commandCurso.ExecuteNonQuery();

            var deleteProfessorStatement = "DELETE Professor";
            SqlCommand commandProfessor = new SqlCommand(deleteProfessorStatement, _connection);
            commandProfessor.ExecuteNonQuery();

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

        [IterationSetup(Target = nameof(RunUpdateStudent))]
        public void UpdateStudentSetup()
        {
            RunInsertStudent();
        }

        [Benchmark]
        public void RunUpdateStudent()
        {
            for (int i = 0; i < TestAmount; i++)
            {
                entitiesInfo.Estudantes[i].Pessoa.Endereco.Rua = "TESTE";
                entitiesInfo.Estudantes[i].Pessoa.Endereco.Numero = "1546";
                entitiesInfo.Estudantes[i].Pessoa.Endereco.Cidade = "CIDADE TESTE";
                entitiesInfo.Estudantes[i].Pessoa.Endereco.Estado = "Estado TESTE";
                entitiesInfo.Estudantes[i].Pessoa.Endereco.Pais = "PAIS TESTE";

                var updateAddressStatement = "UPDATE Endereco SET Rua = @rua, Numero = @numero, Cidade = @cidade, Estado = @estado, Pais = @pais WHERE Id = @id";
                SqlCommand commandAddress = new SqlCommand(updateAddressStatement, _connection);

                commandAddress.Parameters.AddWithValue("@pais", entitiesInfo.Estudantes[i].Pessoa.Endereco.Pais);
                commandAddress.Parameters.AddWithValue("@estado", entitiesInfo.Estudantes[i].Pessoa.Endereco.Estado);
                commandAddress.Parameters.AddWithValue("@cidade", entitiesInfo.Estudantes[i].Pessoa.Endereco.Cidade);
                commandAddress.Parameters.AddWithValue("@rua", entitiesInfo.Estudantes[i].Pessoa.Endereco.Rua);
                commandAddress.Parameters.AddWithValue("@numero", entitiesInfo.Estudantes[i].Pessoa.Endereco.Numero);
                commandAddress.Parameters.AddWithValue("@id", entitiesInfo.Estudantes[i].Pessoa.Endereco.Id);

                commandAddress.ExecuteNonQuery();

                entitiesInfo.Estudantes[i].Pessoa.NumeroTelefone = "12345678";
                entitiesInfo.Estudantes[i].Pessoa.PrimeiroNome = "PESSOA TESTE";
                entitiesInfo.Estudantes[i].Pessoa.DataNascimento = DateTime.Now;
                entitiesInfo.Estudantes[i].Pessoa.UltimoNome = "SOBRENOME TESTE";

                var updatePessoaStatement = "UPDATE Pessoa SET NumeroTelefone = @numeroTel, PrimeiroNome = @primeiroNome, DataNascimento = @dataNasc, UltimoNome = @ultimoNome WHERE Id = @id";
                SqlCommand commandPessoa = new SqlCommand(updatePessoaStatement, _connection);

                commandPessoa.Parameters.AddWithValue("@primeiroNome", entitiesInfo.Estudantes[i].Pessoa.PrimeiroNome);
                commandPessoa.Parameters.AddWithValue("@ultimoNome", entitiesInfo.Estudantes[i].Pessoa.UltimoNome);
                commandPessoa.Parameters.AddWithValue("@numeroTel", entitiesInfo.Estudantes[i].Pessoa.NumeroTelefone);
                commandPessoa.Parameters.AddWithValue("@dataNasc", entitiesInfo.Estudantes[i].Pessoa.DataNascimento);
                commandPessoa.Parameters.AddWithValue("@id", entitiesInfo.Estudantes[i].Pessoa.Id);

                commandPessoa.ExecuteNonQuery();

            }
        }

        [IterationSetup(Target = nameof(RunDeleteStudent))]
        public void DeleteStudentSetup()
        {
            RunInsertStudent();
        }

        [Benchmark]
        public void RunDeleteStudent()
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

        [IterationSetup(Target = nameof(RunGetStudent))]
        public void GetStudentSetup()
        {
            RunInsertStudent();
        }

        [Benchmark]
        public void RunGetStudent()
        {
            var getStudentStatement = "SELECT * FROM Estudante est INNER JOIN Pessoa p ON p.Id = est.PessoaId INNER JOIN Endereco ende ON ende.Id = p.EnderecoId";

            SqlCommand commandGetStudent = new SqlCommand(getStudentStatement, _connection);

            SqlDataReader reader = commandGetStudent.ExecuteReader();

            reader.Close();
        }

        [Benchmark]
        public void RunInsertTeacher()
        {
            for (int i = 0; i < TestAmount; i++)
            {
                #region Insert Address
                var insertAddressStatement = "INSERT INTO Endereco (Id, Pais, Estado, Cidade, Rua, Numero) VALUES (@id, @pais, @estado, @cidade, @rua, @numero)";

                SqlCommand commandAddress = new SqlCommand(insertAddressStatement, _connection);

                commandAddress.Parameters.AddWithValue("@id", entitiesInfo.Professores[i].Pessoa.Endereco.Id);
                commandAddress.Parameters.AddWithValue("@pais", entitiesInfo.Professores[i].Pessoa.Endereco.Pais);
                commandAddress.Parameters.AddWithValue("@estado", entitiesInfo.Professores[i].Pessoa.Endereco.Estado);
                commandAddress.Parameters.AddWithValue("@cidade", entitiesInfo.Professores[i].Pessoa.Endereco.Cidade);
                commandAddress.Parameters.AddWithValue("@rua", entitiesInfo.Professores[i].Pessoa.Endereco.Rua);
                commandAddress.Parameters.AddWithValue("@numero", entitiesInfo.Professores[i].Pessoa.Endereco.Numero);

                commandAddress.ExecuteNonQuery();
                #endregion

                #region Insert Person
                var insertPersonStatement = "INSERT INTO Pessoa (Id,PrimeiroNome,UltimoNome,NumeroTelefone,DataNascimento,EnderecoId) VALUES (@id,@primeiroNome,@ultimoNome,@numeroTel,@dataNasc,@enderecoId)";

                SqlCommand commandPerson = new SqlCommand(insertPersonStatement, _connection);

                commandPerson.Parameters.AddWithValue("@id", entitiesInfo.Professores[i].Pessoa.Id);
                commandPerson.Parameters.AddWithValue("@primeiroNome", entitiesInfo.Professores[i].Pessoa.PrimeiroNome);
                commandPerson.Parameters.AddWithValue("@ultimoNome", entitiesInfo.Professores[i].Pessoa.UltimoNome);
                commandPerson.Parameters.AddWithValue("@numeroTel", entitiesInfo.Professores[i].Pessoa.NumeroTelefone);
                commandPerson.Parameters.AddWithValue("@dataNasc", entitiesInfo.Professores[i].Pessoa.DataNascimento);
                commandPerson.Parameters.AddWithValue("@enderecoId", entitiesInfo.Professores[i].Pessoa.EnderecoId);

                commandPerson.ExecuteNonQuery();
                #endregion


                var insertTeacherStatement = "INSERT INTO Professor (Id,Especializacao,PessoaId) VALUES (@id, @especializacao, @pessoaId)";

                SqlCommand commandTeacher = new SqlCommand(insertTeacherStatement, _connection);

                commandTeacher.Parameters.AddWithValue("@id", entitiesInfo.Professores[i].Id);
                commandTeacher.Parameters.AddWithValue("@especializacao", entitiesInfo.Professores[i].Especializacao);
                commandTeacher.Parameters.AddWithValue("@pessoaId", entitiesInfo.Professores[i].PessoaId);

                commandTeacher.ExecuteNonQuery();


                var insertCursoStatement = "INSERT INTO Curso (Id,Nome,Preco,Descricao,ProfessorId) VALUES(@id,@nome,@preco,@descricao,@professorId)";

                SqlCommand commandCurso = new SqlCommand(insertCursoStatement, _connection);

                commandCurso.Parameters.AddWithValue("@id", entitiesInfo.Cursos.First(x => x.ProfessorId == entitiesInfo.Professores[i].Id).Id);
                commandCurso.Parameters.AddWithValue("@nome", entitiesInfo.Cursos.First(x => x.ProfessorId == entitiesInfo.Professores[i].Id).Nome);
                commandCurso.Parameters.AddWithValue("@preco", entitiesInfo.Cursos.First(x => x.ProfessorId == entitiesInfo.Professores[i].Id).Preco);
                commandCurso.Parameters.AddWithValue("@descricao", entitiesInfo.Cursos.First(x => x.ProfessorId == entitiesInfo.Professores[i].Id).Descricao);
                commandCurso.Parameters.AddWithValue("@professorId", entitiesInfo.Cursos.First(x => x.ProfessorId == entitiesInfo.Professores[i].Id).ProfessorId);

                commandCurso.ExecuteNonQuery();
            }

        }

        [IterationSetup(Target = nameof(RunUpdateTeacher))]
        public void UpdateTeacherSetup()
        {
            RunInsertTeacher();
        }


        [Benchmark]
        public void RunUpdateTeacher()
        {
            for (int i = 0; i < TestAmount; i++)
            {
                entitiesInfo.Professores[i].Pessoa.Endereco.Rua = "TESTE";
                entitiesInfo.Professores[i].Pessoa.Endereco.Numero = "1546";
                entitiesInfo.Professores[i].Pessoa.Endereco.Cidade = "CIDADE TESTE";
                entitiesInfo.Professores[i].Pessoa.Endereco.Estado = "Estado TESTE";
                entitiesInfo.Professores[i].Pessoa.Endereco.Pais = "PAIS TESTE";

                var updateAddressStatement = "UPDATE Endereco SET Rua = @rua, Numero = @numero, Cidade = @cidade, Estado = @estado, Pais = @pais WHERE Id = @id";
                SqlCommand commandAddress = new SqlCommand(updateAddressStatement, _connection);

                commandAddress.Parameters.AddWithValue("@pais", entitiesInfo.Professores[i].Pessoa.Endereco.Pais);
                commandAddress.Parameters.AddWithValue("@estado", entitiesInfo.Professores[i].Pessoa.Endereco.Estado);
                commandAddress.Parameters.AddWithValue("@cidade", entitiesInfo.Professores[i].Pessoa.Endereco.Cidade);
                commandAddress.Parameters.AddWithValue("@rua", entitiesInfo.Professores[i].Pessoa.Endereco.Rua);
                commandAddress.Parameters.AddWithValue("@numero", entitiesInfo.Professores[i].Pessoa.Endereco.Numero);
                commandAddress.Parameters.AddWithValue("@id", entitiesInfo.Professores[i].Pessoa.Endereco.Id);

                commandAddress.ExecuteNonQuery();


                var updateCursoStatement = "UPDATE Curso SET Descricao = @descricao WHERE ProfessorId = @professorId";
                SqlCommand commandCurso = new SqlCommand(updateCursoStatement, _connection);

                commandCurso.Parameters.AddWithValue("@descricao", "TESTE DESCRIÇÃO");
                commandCurso.Parameters.AddWithValue("@professorId", entitiesInfo.Professores[i].Id);

                commandCurso.ExecuteNonQuery();

            }
        }

        [IterationSetup(Target = nameof(RunDeleteTeacher))]
        public void DeleteTeacherSetup()
        {
            RunInsertTeacher();
        }

        [Benchmark]
        public void RunDeleteTeacher()
        {
            var deleteCursoStatement = "DELETE Curso";
            SqlCommand commandCurso = new SqlCommand(deleteCursoStatement, _connection);
            commandCurso.ExecuteNonQuery();

            var deleteProfessorStatement = "DELETE Professor";
            SqlCommand commandProfessor = new SqlCommand(deleteProfessorStatement, _connection);
            commandProfessor.ExecuteNonQuery();

            var deletePessoaStatement = "DELETE Pessoa";
            SqlCommand commandPessoa = new SqlCommand(deletePessoaStatement, _connection);
            commandPessoa.ExecuteNonQuery();

            var deleteEnderecoStatement = "DELETE Endereco";
            SqlCommand commandEndereco = new SqlCommand(deleteEnderecoStatement, _connection);
            commandEndereco.ExecuteNonQuery();
        }

        [IterationSetup(Target = nameof(RunGetTeacher))]
        public void GetTeacherSetup()
        {
            RunInsertTeacher();
        }

        [Benchmark]
        public void RunGetTeacher()
        {
            var getProfessorStatement = "SELECT * FROM Professor P INNER JOIN Pessoa Pe ON Pe.Id = P.PessoaId INNER JOIN Endereco E ON E.Id = Pe.EnderecoId INNER JOIN Curso C ON C.ProfessorId = P.Id";

            SqlCommand commandGetProfessor = new SqlCommand(getProfessorStatement, _connection);

            SqlDataReader reader = commandGetProfessor.ExecuteReader();

            reader.Close();
        }
    }
}