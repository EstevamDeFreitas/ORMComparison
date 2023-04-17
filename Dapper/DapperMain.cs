﻿using Entidades;
using System.Data.SqlClient;

namespace Dapper;

public class DapperMain
{
    private string _connectionString;
    public DapperMain(string connectionString) 
    { 
        _connectionString = connectionString;
    }

    public void InitTest()
    {
        SqlConnection connection = new SqlConnection(_connectionString);

        connection.Open();

        RunInsertTest(connection);
    }

    public void getData(SqlConnection conn) 
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
}
