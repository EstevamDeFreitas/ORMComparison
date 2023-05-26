using BenchmarkDotNet.Attributes;
using EntidadesHiber;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHiber.Mapping;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using OrmUtilities;

namespace NHiber;

[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 1, invocationCount: 1, baseline: true)]
public class HiberMain : ITestBase, IDisposable
{
    private EntitiesInfoHiber entitiesInfo { get; set; }
    private ISessionFactory sessionFactory;

    [Params(500, 1000, 2000, 5000, 10000)]
    public int TestAmount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        sessionFactory = Fluently.Configure()
            .Database(MsSqlConfiguration.MsSql2012.ConnectionString(
                cs => cs.Server("DESKTOP-QAM9OUA\\SQLEXPRESS")
                    .Database("orm_comparisson")
                    .Username("orm_user")
                    .Password("123456")))
            .Mappings(m =>
            {
                m.FluentMappings.AddFromAssemblyOf<EstudanteMap>();
                m.FluentMappings.AddFromAssemblyOf<EnderecoMap>();
                m.FluentMappings.AddFromAssemblyOf<PessoaMap>();
                m.FluentMappings.AddFromAssemblyOf<ProfessorMap>();
                m.FluentMappings.AddFromAssemblyOf<CursoMap>();
            })
            .ExposeConfiguration(cfg => new SchemaExport(cfg)
            .Create(false, false))
            .BuildSessionFactory();

        entitiesInfo = new EntitiesInfoHiber();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        IterationCleanup();
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        using (ISession session = sessionFactory.OpenSession())
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                string hql = "delete from Curso";
                session.CreateQuery(hql).ExecuteUpdate();

                hql = "delete from Professor";
                session.CreateQuery(hql).ExecuteUpdate();

                hql = "delete from Estudante";
                session.CreateQuery(hql).ExecuteUpdate();

                hql = "delete from Pessoa";
                session.CreateQuery(hql).ExecuteUpdate();

                hql = "delete from Endereco";
                session.CreateQuery(hql).ExecuteUpdate();

                transaction.Commit();
            }
        }
    }


    [Benchmark]
    public void RunInsertStudent()
    {
        using (ISession session = sessionFactory.OpenSession())
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                for (int i = 0; i < 500; i++)
                {
                    session.Save(entitiesInfo.Estudantes[i].Pessoa.Endereco);
                    session.Save(entitiesInfo.Estudantes[i].Pessoa);
                    session.Save(entitiesInfo.Estudantes[i]);
                }

                transaction.Commit();
            }
        }
    }

    [IterationSetup(Target = nameof(RunUpdateStudent))]
    public void UpdateStudentSetup()
    {
        RunInsertStudent();
    }

    [Benchmark]
    public void RunUpdateStudent()
    {
        using (var session = sessionFactory.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                for (int i = 0; i < 500; i++)
                {
                    var estudante = session.Get<Estudante>(entitiesInfo.Estudantes[i].Id);

                    if (estudante != null)
                    {
                        estudante.Pessoa.Endereco.Rua = "TESTE";
                        estudante.Pessoa.Endereco.Numero = "1546";
                        estudante.Pessoa.Endereco.Cidade = "CIDADE TESTE";
                        estudante.Pessoa.Endereco.Estado = "Estado TESTE";
                        estudante.Pessoa.Endereco.Pais = "PAIS TESTE";

                        estudante.Pessoa.PrimeiroNome = "Nome novo";

                        session.Update(estudante);
                    }
                }

                transaction.Commit();
            }
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
        using (ISession session = sessionFactory.OpenSession())
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                string hql = "delete from Estudante";
                session.CreateQuery(hql).ExecuteUpdate();

                hql = "delete from Pessoa";
                session.CreateQuery(hql).ExecuteUpdate();

                hql = "delete from Endereco";
                session.CreateQuery(hql).ExecuteUpdate();

                transaction.Commit();
            }
        }
    }

    [IterationSetup(Target = nameof(RunGetStudent))]
    public void GetStudentSetup()
    {
        RunInsertStudent();
    }

    [Benchmark]
    public void RunGetStudent()
    {
        using (ISession session = sessionFactory.OpenSession())
        {
            var estudantes = session.Query<Estudante>().ToList();

        }
    }

    [Benchmark]
    public void RunInsertTeacher()
    {
        using (ISession session = sessionFactory.OpenSession())
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                for (int i = 0; i < 500; i++)
                {
                    var professor = entitiesInfo.Professores[i];
                    var endereco = professor.Pessoa.Endereco;

                    session.Save(entitiesInfo.Professores[i].Pessoa.Endereco);
                    session.Save(entitiesInfo.Professores[i].Pessoa);
                    session.Save(entitiesInfo.Professores[i]);

                    Curso curso = new Curso()
                    {
                        Id = Guid.NewGuid(), 
                        Nome = "Nome do curso", 
                        Preco = 15.50m,
                        Descricao = "Descrição do curso", 
                        ProfessorId = professor.Id,
                        Professor = professor
                    };

                    session.Save(curso);

                }

                transaction.Commit();
            }
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
        using (var session = sessionFactory.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                for (int i = 0; i < 500; i++)
                {
                    var professor = session.Get<Professor>(entitiesInfo.Professores[i].Id);

                    if (professor != null)
                    {
                        professor.Pessoa.Endereco.Rua = "TESTE";
                        professor.Pessoa.Endereco.Numero = "1546";
                        professor.Pessoa.Endereco.Cidade = "CIDADE TESTE";
                        professor.Pessoa.Endereco.Estado = "Estado TESTE";
                        professor.Pessoa.Endereco.Pais = "PAIS TESTE";

                        professor.Pessoa.PrimeiroNome = "Nome novo";

                        session.Update(professor);
                    }
                }

                transaction.Commit();
            }
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
        using (ISession session = sessionFactory.OpenSession())
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                string hql = "delete from Curso";
                session.CreateQuery(hql).ExecuteUpdate();

                hql = "delete from Professor";
                session.CreateQuery(hql).ExecuteUpdate();

                hql = "delete from Pessoa";
                session.CreateQuery(hql).ExecuteUpdate();

                hql = "delete from Endereco";
                session.CreateQuery(hql).ExecuteUpdate();

                transaction.Commit();
            }
        }
    }

    [IterationSetup(Target = nameof(RunGetTeacher))]
    public void GetTeacherSetup()
    {
        RunInsertTeacher();
    }

    [Benchmark]
    public void RunGetTeacher()
    {
        using (ISession session = sessionFactory.OpenSession())
        {
            var professores = session.Query<Professor>().ToList();
        }
    }

    public void Dispose()
    {
        sessionFactory.Close();
    }
}
