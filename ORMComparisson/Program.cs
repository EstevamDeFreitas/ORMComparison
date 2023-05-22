
using ADONET;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using EFCore;
using OrmUtilities;

internal class Program
{
    private static void Main(string[] args)
    {

        BenchmarkRunner.Run<EFCoreMain>();

        //var entitiesInfo = new EntitiesInfo();

    }

}
