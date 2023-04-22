using Entidades;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;

namespace NHibernate
{
    public class NHibernateMain
    {
        private static ISessionFactory _sessionFactory;
        public NHibernateMain(string connectionString)
        {
            var dbConfig = MsSqlConfiguration.MsSql2012.ConnectionString(connectionString);
            var fluentConfig = Fluently.Configure()
                .Database(dbConfig)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Estudante>());

            _sessionFactory = fluentConfig.BuildSessionFactory();
        }

        public void InitTest()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    /*
                    // Cria uma nova instância de uma entidade e salva no banco de dados
                    var cliente = new Cliente
                    {
                        Nome = "João",
                        Sobrenome = "Silva",
                        Idade = 30
                    };

                    session.Save(cliente);
                    */

                    tx.Commit();
                }
            }
        }

        public static void BuildSchema(string connectionString)
        {
            var fluentConfig = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Estudante>());

            var export = new SchemaExport(fluentConfig.BuildConfiguration());
            export.Create(false, true);
        }

    }
}

