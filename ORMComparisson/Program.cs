﻿
using ADONET;
using EFCore;

internal class Program
{
    private static void Main(string[] args)
    {
        /*
        AdoNetMain adomain = new AdoNetMain("Data Source=DESKTOP-L42IOG5;Initial Catalog=orm_comparissondb;User ID=orm_user;Password=123456");

        adomain.InitTest();*/

        EFCoreMain efcoremain = new EFCoreMain();
        efcoremain.InitTest();
    }
}