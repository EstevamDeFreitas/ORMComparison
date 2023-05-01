
using ADONET;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using EFCore;
using OrmUtilities;

internal class Program
{
    private static void Main(string[] args)
    {

        BenchmarkRunner.Run<AdoNetMain>();



        //EFCoreMain efcoremain = new EFCoreMain();
        //efcoremain.InitTest();

        //var entitiesInfo = new EntitiesInfo();

    }

}
