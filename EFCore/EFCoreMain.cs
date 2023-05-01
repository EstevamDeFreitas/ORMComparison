using BenchmarkDotNet.Attributes;
using EFCore;
using EFCore.Mapping;
using Entidades;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrmUtilities;

namespace EFCore
{

    [MemoryDiagnoser]
    [SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 1, invocationCount: 1, baseline: true)]

    public class EFCoreMain : ITestBase
    {
        private static  string _stringConexao = "Initial Catalog=ORMComparison;Data Source=DESKTOP-GPE9S1B\\SQLEXPRESS;User ID=orm_user;Password=123456;TrustServerCertificate=True";

        private static DbContextOptions<MeuContexto> ObterOpcoesDbContext()
        {

            var optionsBuilder = new DbContextOptionsBuilder<MeuContexto>();
            optionsBuilder.UseSqlServer(_stringConexao);

            return optionsBuilder.Options;
        }


        public EFCoreMain()
        {
        }

        public void InitTest()
        {
            using (var contexto = new MeuContexto())
            {

                // Cria uma nova instância de uma entidade e salva no banco de dados
                // (@enderecoId, 'brasil', 'São Paulo', 'São Paulo', 'Rua A', '123')
                /*
                 *    public Guid Id { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
                 */
                var endereco = new Endereco
                {
                    Id = Guid.NewGuid(),
                    Pais = "Brasil",
                    Estado="SP",
                    Cidade ="Mogi das Cruzes",
                    Rua = "Astrea Barral Nébias",
                    Numero = "99"
                };

                Console.WriteLine(endereco.Id);
                Console.WriteLine(endereco.Pais);
                Console.WriteLine(endereco.Estado);
                Console.WriteLine(endereco.Cidade);
                Console.WriteLine(endereco.Rua);
                Console.WriteLine(endereco.Numero);

                contexto.Enderecos.Add(endereco);
                contexto.SaveChanges();
            }
        }

        public class MeuContexto : DbContext
        {
            public MeuContexto() : base(ObterOpcoesDbContext())
            {
            }

            public DbSet<Curso> Cursos { get; set; }
            public DbSet<CursoAluno> CursoAlunos { get; set; }
            public DbSet<Endereco> Enderecos { get; set; }
            public DbSet<Estudante> Estudantes { get; set; }
            public DbSet<Pessoa> Pessoas { get; set; }
            public DbSet<Professor> Professores { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.ApplyConfiguration(new CursoAlunoMapping());
                modelBuilder.ApplyConfiguration(new CursoMapping());
                modelBuilder.ApplyConfiguration(new EnderecoMapping());
                modelBuilder.ApplyConfiguration(new EstudanteMapping());
                modelBuilder.ApplyConfiguration(new PessoaMapping());
                modelBuilder.ApplyConfiguration(new ProfessorMapping());
            }
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

        public void Setup()
        {
            throw new NotImplementedException();
        }

        public void Cleanup()
        {
            throw new NotImplementedException();
        }
    }
}