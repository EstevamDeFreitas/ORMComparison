using BenchmarkDotNet.Attributes;
using Entidades;
using OrmUtilities;
using System.Data.SqlClient;

namespace Dapper;

[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 1, invocationCount: 1, baseline: true)]
public class DapperMain : ITestBase
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

        IterationCleanup();

        _connection.Close();
    }

    [Benchmark]
    public void RunInsertStudent()
    {
        for (int i = 0; i < TestAmount; i++)
        {
            #region Insert Endereco
            var insertAddressStatement = @"
                INSERT INTO Endereco (Id, Pais, Estado, Cidade, Rua, Numero) 
                VALUES (@Id, @Pais, @Estado, @Cidade, @Rua, @Numero)";

            Endereco enderecoNovo = new Endereco
            {
                Id = entitiesInfo.Estudantes[i].Pessoa.Endereco.Id,
                Pais = entitiesInfo.Estudantes[i].Pessoa.Endereco.Pais,
                Estado = entitiesInfo.Estudantes[i].Pessoa.Endereco.Estado,
                Cidade = entitiesInfo.Estudantes[i].Pessoa.Endereco.Cidade,
                Rua = entitiesInfo.Estudantes[i].Pessoa.Endereco.Rua,
                Numero = entitiesInfo.Estudantes[i].Pessoa.Endereco.Numero
            };

            _connection.Execute(insertAddressStatement, enderecoNovo);

            #endregion

            #region Insert Pessoa
            var insertPersonStatement = @"
                INSERT INTO Pessoa (Id,PrimeiroNome,UltimoNome,NumeroTelefone,DataNascimento,EnderecoId) 
                VALUES (@Id,@PrimeiroNome,@UltimoNome,@NumeroTelefone,@DataNascimento,@EnderecoId)";

            Pessoa pessoaNovo = new Pessoa
            {
                Id = entitiesInfo.Estudantes[i].Pessoa.Id,
                PrimeiroNome = entitiesInfo.Estudantes[i].Pessoa.PrimeiroNome,
                UltimoNome = entitiesInfo.Estudantes[i].Pessoa.UltimoNome,
                NumeroTelefone = entitiesInfo.Estudantes[i].Pessoa.NumeroTelefone,
                DataNascimento = entitiesInfo.Estudantes[i].Pessoa.DataNascimento,
                EnderecoId = entitiesInfo.Estudantes[i].Pessoa.EnderecoId
            };

            _connection.Execute(insertPersonStatement, pessoaNovo);

            #endregion

            #region Insert Student
            var insertStudentStatement = "INSERT INTO Estudante (Id,PessoaId,Descricao) VALUES (@Id,@PessoaId,@Descricao)";

            Estudante estudanteNovo = new Estudante
            {
                Id = entitiesInfo.Estudantes[i].Id,
                PessoaId = entitiesInfo.Estudantes[i].PessoaId,
                Descricao = entitiesInfo.Estudantes[i].Descricao,
            };

            _connection.Execute(insertStudentStatement, estudanteNovo);

            #endregion
        }
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        var deleteCursoStatement = "DELETE FROM Curso";
        _connection.Execute(deleteCursoStatement);

        var deleteProfessorStatement = "DELETE FROM Professor";
        _connection.Execute(deleteProfessorStatement);

        var deleteEstudanteStatement = "DELETE FROM Estudante";
        _connection.Execute(deleteEstudanteStatement);

        var deletePessoaStatement = "DELETE FROM Pessoa";
        _connection.Execute(deletePessoaStatement);

        var deleteEnderecoStatement = "DELETE FROM Endereco";
        _connection.Execute(deleteEnderecoStatement);

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

            var updateAddressStatement = "UPDATE Endereco SET Rua = @Rua, Numero = @Numero, Cidade = @Cidade, Estado = @Estado, Pais = @Pais WHERE Id = @Id";

            Endereco enderecoUpdate = new Endereco
            {
                Pais = entitiesInfo.Estudantes[i].Pessoa.Endereco.Pais,
                Estado = entitiesInfo.Estudantes[i].Pessoa.Endereco.Estado,
                Cidade = entitiesInfo.Estudantes[i].Pessoa.Endereco.Cidade,
                Rua = entitiesInfo.Estudantes[i].Pessoa.Endereco.Rua,
                Numero = entitiesInfo.Estudantes[i].Pessoa.Endereco.Numero,
                Id = entitiesInfo.Estudantes[i].Pessoa.Endereco.Id
            };

            _connection.Execute(updateAddressStatement, enderecoUpdate);


            entitiesInfo.Estudantes[i].Pessoa.NumeroTelefone = "12345678";
            entitiesInfo.Estudantes[i].Pessoa.PrimeiroNome = "PESSOA TESTE";
            entitiesInfo.Estudantes[i].Pessoa.DataNascimento = DateTime.Now;
            entitiesInfo.Estudantes[i].Pessoa.UltimoNome = "SOBRENOME TESTE";

            var updatePessoaStatement = "UPDATE Pessoa SET NumeroTelefone = @NumeroTel, PrimeiroNome = @PrimeiroNome, DataNascimento = @DataNasc, UltimoNome = @UltimoNome WHERE Id = @Id";

            _connection.Execute(updatePessoaStatement, new { 
                PrimeiroNome = entitiesInfo.Estudantes[i].Pessoa.PrimeiroNome,
                UltimoNome = entitiesInfo.Estudantes[i].Pessoa.UltimoNome,
                NumeroTel = entitiesInfo.Estudantes[i].Pessoa.NumeroTelefone,
                DataNasc = entitiesInfo.Estudantes[i].Pessoa.DataNascimento,
                Id = entitiesInfo.Estudantes[i].Pessoa.Id
            });
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
        var deleteEstudanteStatement = "DELETE FROM Estudante";
        _connection.Execute(deleteEstudanteStatement);

        var deletePessoaStatement = "DELETE FROM Pessoa";
        _connection.Execute(deletePessoaStatement);

        var deleteEnderecoStatement = "DELETE FROM Endereco";
        _connection.Execute(deleteEnderecoStatement);
    }

    [IterationSetup(Target = nameof(RunGetStudent))]
    public void GetStudentSetup()
    {
        RunInsertStudent();
    }

    [Benchmark]
    public void RunGetStudent()
    {
        var getStudentStatement = @"
        SELECT *
        FROM Estudante est
        INNER JOIN Pessoa p ON p.Id = est.PessoaId
        INNER JOIN Endereco ende ON ende.Id = p.EnderecoId";

        using (var reader = _connection.ExecuteReader(getStudentStatement))
        {
        }

    }

    [Benchmark]
    public void RunInsertTeacher()
    {
        for (int i = 0; i < TestAmount; i++)
        {
            #region Insert Endereco
            var insertAddressStatement = @"
                INSERT INTO Endereco (Id, Pais, Estado, Cidade, Rua, Numero) 
                VALUES (@Id, @Pais, @Estado, @Cidade, @Rua, @Numero)";

            Endereco enderecoNovo = new Endereco
            {
                Id = entitiesInfo.Professores[i].Pessoa.Endereco.Id,
                Pais = entitiesInfo.Professores[i].Pessoa.Endereco.Pais,
                Estado = entitiesInfo.Professores[i].Pessoa.Endereco.Estado,
                Cidade = entitiesInfo.Professores[i].Pessoa.Endereco.Cidade,
                Rua = entitiesInfo.Professores[i].Pessoa.Endereco.Rua,
                Numero = entitiesInfo.Professores[i].Pessoa.Endereco.Numero
            };

            _connection.Execute(insertAddressStatement, enderecoNovo);

            #endregion

            #region Insert Pessoa
            var insertPersonStatement = @"
                    INSERT INTO Pessoa (Id,PrimeiroNome,UltimoNome,NumeroTelefone,DataNascimento,EnderecoId) 
                    VALUES (@Id,@PrimeiroNome,@UltimoNome,@NumeroTelefone,@DataNascimento,@EnderecoId)";

            Pessoa pessoaNovo = new Pessoa
            {
                Id = entitiesInfo.Professores[i].Pessoa.Id,
                PrimeiroNome = entitiesInfo.Professores[i].Pessoa.PrimeiroNome,
                UltimoNome = entitiesInfo.Professores[i].Pessoa.UltimoNome,
                NumeroTelefone = entitiesInfo.Professores[i].Pessoa.NumeroTelefone,
                DataNascimento = entitiesInfo.Professores[i].Pessoa.DataNascimento,
                EnderecoId = entitiesInfo.Professores[i].Pessoa.EnderecoId
            };

            _connection.Execute(insertPersonStatement, pessoaNovo);

            #endregion

            #region Insert Teacher

            var insertTeacherStatement = @"
                    INSERT INTO Professor (Id,Especializacao,PessoaId)
                    VALUES (@Id, @Especializacao, @PessoaId)";

            Professor professorNovo = new Professor
            {
                Id = entitiesInfo.Professores[i].Id,
                Especializacao = entitiesInfo.Professores[i].Especializacao,
                PessoaId = entitiesInfo.Professores[i].PessoaId
            };

            _connection.Execute(insertTeacherStatement, professorNovo);

            #endregion

            #region Insert Curso

            var insertCursoStatement = @"
                     INSERT INTO Curso (Id,Nome,Preco,Descricao,ProfessorId)
                     VALUES (@Id,@Nome,@Preco,@Descricao,@ProfessorId)";

            Curso cursoNovo = new Curso
            {
                Id = entitiesInfo.Cursos.First(x => x.ProfessorId == entitiesInfo.Professores[i].Id).Id,
                Nome = entitiesInfo.Cursos.First(x => x.ProfessorId == entitiesInfo.Professores[i].Id).Nome,
                Preco = entitiesInfo.Cursos.First(x => x.ProfessorId == entitiesInfo.Professores[i].Id).Preco,
                Descricao = entitiesInfo.Cursos.First(x => x.ProfessorId == entitiesInfo.Professores[i].Id).Descricao,
                ProfessorId = entitiesInfo.Cursos.First(x => x.ProfessorId == entitiesInfo.Professores[i].Id).ProfessorId
            };

            _connection.Execute(insertCursoStatement, cursoNovo);

            #endregion
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

            var updateAddressStatement = @"
                        UPDATE Endereco
                        SET Rua = @Rua, Numero = @Numero, Cidade = @Cidade, Estado = @Estado, Pais = @Pais
                        WHERE Id = @Id";

            Endereco enderecoUpdate = new Endereco
            {
                Pais = entitiesInfo.Professores[i].Pessoa.Endereco.Pais,
                Estado = entitiesInfo.Professores[i].Pessoa.Endereco.Estado,
                Cidade = entitiesInfo.Professores[i].Pessoa.Endereco.Cidade,
                Rua = entitiesInfo.Professores[i].Pessoa.Endereco.Rua,
                Numero = entitiesInfo.Professores[i].Pessoa.Endereco.Numero,
                Id = entitiesInfo.Professores[i].Pessoa.Endereco.Id
            };

            _connection.Execute(updateAddressStatement, enderecoUpdate);


            var updateCursoStatement = "UPDATE Curso SET Descricao = @Descricao WHERE ProfessorId = @ProfessorId";

            _connection.Execute(updateCursoStatement, new { 
                Descricao = "TESTE DESCRIÇÃO",
                ProfessorId = entitiesInfo.Professores[i].Id
            });

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
        var deleteCursoStatement = "DELETE FROM Curso";
        _connection.Execute(deleteCursoStatement);

        var deleteProfessorStatement = "DELETE FROM Professor";
        _connection.Execute(deleteProfessorStatement);

        var deletePessoaStatement = "DELETE FROM Pessoa";
        _connection.Execute(deletePessoaStatement);

        var deleteEnderecoStatement = "DELETE FROM Endereco";
        _connection.Execute(deleteEnderecoStatement);
    }

    [IterationSetup(Target = nameof(RunGetTeacher))]
    public void GetTeacherSetup()
    {
        RunInsertTeacher();
    }

    [Benchmark]
    public void RunGetTeacher()
    {
        var getProfessorStatement = @"
                    SELECT *
                    FROM Professor P
                    INNER JOIN Pessoa Pe ON Pe.Id = P.PessoaId
                    INNER JOIN Endereco E ON E.Id = Pe.EnderecoId
                    INNER JOIN Curso C ON C.ProfessorId = P.Id";

        using (var reader = _connection.ExecuteReader(getProfessorStatement))
        {
        }
    }
}