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
        InsertTestCleanup();

        _connection.Close();
    }

    [IterationCleanup(Target = nameof(RunInsertTest))]
    public void InsertTestCleanup()
    {
        var deleteStatement = "DELETE FROM Endereco";
        _connection.Execute(deleteStatement);
    }

    public void InitTest()
    {
        RunInsertTest(_connection);
    }

    public void RunGetTest(SqlConnection conn)
    {
        string sql = "SELECT Id, Pais, Estado, Cidade, Rua, Numero FROM Enderecos";

        List<Endereco> enderecos = conn.Query<Endereco>(sql).ToList();

        Console.WriteLine(enderecos.Count);

        foreach (Endereco endereco in enderecos)
        {
            Console.WriteLine($"Endereço {endereco.Id}: {endereco.Rua}, {endereco.Numero}, {endereco.Cidade}, {endereco.Estado}, {endereco.Pais}");
        }
    }

    public void RunInsertTest(SqlConnection conn)
    {

        var endereco = new Endereco
        {
            Id = Guid.NewGuid(),
            Pais = "Brasil",
            Estado = "São Paulo",
            Cidade = "São Paulo",
            Rua = "Rua Teste",
            Numero = "421"
        };

        string sql = "INSERT INTO Enderecos (Id, Pais, Estado, Cidade, Rua, Numero) VALUES (@Id, @Pais, @Estado, @Cidade, @Rua, @Numero)";

        int affectedRows = conn.Execute(sql, endereco);

        Console.WriteLine(affectedRows);

    }

    private void RunDeleteTest(SqlConnection connection, Guid enderecoId)
    {

        string sql = "DELETE FROM Enderecos WHERE Id = @EnderecoId";

        int affectedRows = connection.Execute(sql, new { EnderecoId = enderecoId });

        Console.WriteLine($"Foram excluídas {affectedRows} linhas da tabela Enderecos.");
    }

    private void RunUpdateTest(SqlConnection connection, Guid enderecoId)
    {
        Endereco enderecoAtualizado = new Endereco
        {
            Id = enderecoId,
            Pais = "Brasil",
            Estado = "São Paulo",
            Cidade = "São Paulo",
            Rua = "Rua Nova",
            Numero = "123"
        };

        string sql = "UPDATE Enderecos SET Pais = @Pais, Estado = @Estado, Cidade = @Cidade, Rua = @Rua, Numero = @Numero WHERE Id = @EnderecoId";

        int affectedRows = connection.Execute(sql, enderecoAtualizado);

        Console.WriteLine($"Foram atualizadas {affectedRows} linhas da tabela Enderecos.");
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