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
        /*
        [Params(500, 1000, 2000, 5000, 10000)]
        public int TestAmount { get; set; }*/

        /*FOI NECESSARIO USAR UM VALOR MOCADO NO TESTAMOUNT POIS O NOTE DO CHICO NÃO RODA OO BENCHMARK*/
        public int TestAmount = 1000;

        private EntitiesInfo entitiesInfo { get; set; }

        private static DbContextOptions<MeuContexto> ObterOpcoesDbContext()
        {

            var optionsBuilder = new DbContextOptionsBuilder<MeuContexto>();
            optionsBuilder.UseSqlServer(_stringConexao);

            return optionsBuilder.Options;
        }


        public EFCoreMain()
        {

            entitiesInfo = new EntitiesInfo();

        }

        public void InitTest()
        {
            //RunInsertStudent();
            //RunUpdateStudent();

            RunDeleteStudent(); 
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
            using (var contexto = new MeuContexto())
            {
                for (int i = 0; i < TestAmount; i++)
                {

                    Endereco enderecoNovo = new Endereco
                    {
                        Id = entitiesInfo.Estudantes[i].Pessoa.Endereco.Id,
                        Pais = entitiesInfo.Estudantes[i].Pessoa.Endereco.Pais,
                        Estado = entitiesInfo.Estudantes[i].Pessoa.Endereco.Estado,
                        Cidade = entitiesInfo.Estudantes[i].Pessoa.Endereco.Cidade,
                        Rua = entitiesInfo.Estudantes[i].Pessoa.Endereco.Rua,
                        Numero = entitiesInfo.Estudantes[i].Pessoa.Endereco.Numero
                    };

                    contexto.Enderecos.Add(enderecoNovo);


                    Pessoa pessoaNovo = new Pessoa
                    {
                        Id = entitiesInfo.Estudantes[i].Pessoa.Id,
                        PrimeiroNome = entitiesInfo.Estudantes[i].Pessoa.PrimeiroNome,
                        UltimoNome = entitiesInfo.Estudantes[i].Pessoa.UltimoNome,
                        NumeroTelefone = entitiesInfo.Estudantes[i].Pessoa.NumeroTelefone,
                        DataNascimento = entitiesInfo.Estudantes[i].Pessoa.DataNascimento,
                        EnderecoId = entitiesInfo.Estudantes[i].Pessoa.EnderecoId
                    };

                    contexto.Pessoas.Add(pessoaNovo);

                    Estudante estudanteNovo = new Estudante
                    {
                        Id = entitiesInfo.Estudantes[i].Id,
                        PessoaId = entitiesInfo.Estudantes[i].PessoaId,
                        Descricao = entitiesInfo.Estudantes[i].Descricao,
                    };

                    contexto.Estudantes.Add(estudanteNovo);

                }

                contexto.SaveChanges();

            }

            }

        public void RunUpdateStudent()
        {
            using (var contexto = new MeuContexto())
            {
                var estudantes = contexto.Estudantes
    .Include(e => e.Pessoa)
        .ThenInclude(p => p.Endereco)
    .ToList();

                foreach (var estudante in estudantes)
                {
                    estudante.Descricao = "Nova descrição";

                    estudante.Pessoa.PrimeiroNome = "Novo primeiro nome";
                    estudante.Pessoa.UltimoNome = "Novo ultimo nome";
                    estudante.Pessoa.NumeroTelefone = "Novo numero de cel";
                    estudante.Pessoa.DataNascimento = DateTime.Now;


                    estudante.Pessoa.Endereco.Pais = "Australia";
                    estudante.Pessoa.Endereco.Estado = "novo estado";
                    estudante.Pessoa.Endereco.Cidade = "Nova cidade";
                    estudante.Pessoa.Endereco.Rua = "Nova rua";
                    estudante.Pessoa.Endereco.Numero = "1";

                    /*
                       Pais = "Brasil",
            Estado = "São Paulo",
            Cidade = "São Paulo",
            Rua = "Rua Nova",
            Numero = "123"
                    
                       public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public string NumeroTelefone { get; set; }
        public DateTime DataNascimento { get; set; }*/


                }


                contexto.SaveChanges();
            }

        }

            public void RunDeleteStudent()
        {

            using (var contexto = new MeuContexto())
            {
                var estudantes = contexto.Estudantes
    .Include(e => e.Pessoa)
        .ThenInclude(p => p.Endereco)
    .ToList();

                foreach (var estudante in estudantes)
                {
                    if (estudante != null)
                    {
                   
                        contexto.Remove(estudante);
                        contexto.Remove(estudante.Pessoa);
                        contexto.Remove(estudante.Pessoa.Endereco);
                        contexto.SaveChanges();
                    }
                }

                }

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