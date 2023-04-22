﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Data.SqlClient;

namespace ADONET
{
    [MemoryDiagnoser]
    [SimpleJob(launchCount:1, warmupCount:1, iterationCount:1, invocationCount:1, baseline:true)]
    public class AdoNetMain
    {
        private static string ConnectionString { get; } = "Data Source=DESKTOP-L42IOG5;Initial Catalog=orm_comparissondb;User ID=orm_user;Password=123456";

        [GlobalSetup]
        public void Setup()
        {

        }

        [Benchmark]
        public void InitTest()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            connection.Open();

            RunInsertTest(connection);

            connection.Close();

            Console.WriteLine("Teste realizado");
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