
using ADONET;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using EFCore;
using OrmUtilities;

internal class Program
{
    private static void Main(string[] args)
    {

        //AdoNetMain adomain = new AdoNetMain("Data Source=DESKTOP-L42IOG5;Initial Catalog=orm_comparissondb;User ID=orm_user;Password=123456");

        //adomain.InitTest();



        /*BenchmarkRunner.Run<AdoNetMain>();



        EFCoreMain efcoremain = new EFCoreMain();
        efcoremain.InitTest();*/

        var entitiesInfo = new EntitiesInfo();

    }

}
