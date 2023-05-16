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
            //RunDeleteStudent();
            //RunGetStudent();

            //RunInsertTeacher();
            //RunUpdateTeacher();
            RunDeleteTeacher();

            //RunGetTeacher();
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

            using (var contexto = new MeuContexto())
            {
                var estudantesCompletos = contexto.Estudantes
                                   .Include(e => e.Pessoa)
                                   .ThenInclude(p => p.Endereco)
                                   .ToList();

                Console.WriteLine(estudantesCompletos.Count);
            }

           

        }

        public void RunInsertTeacher()
        {

            using (var contexto = new MeuContexto())
            {
                for (int i = 0; i < TestAmount; i++)
                {

                    Endereco enderecoNovo = new Endereco
                    {
                        Id = entitiesInfo.Professores[i].Pessoa.Endereco.Id,
                        Pais = entitiesInfo.Professores[i].Pessoa.Endereco.Pais,
                        Estado = entitiesInfo.Professores[i].Pessoa.Endereco.Estado,
                        Cidade = entitiesInfo.Professores[i].Pessoa.Endereco.Cidade,
                        Rua = entitiesInfo.Professores[i].Pessoa.Endereco.Rua,
                        Numero = entitiesInfo.Professores[i].Pessoa.Endereco.Numero
                    };

                    contexto.Enderecos.Add(enderecoNovo);


                    Pessoa pessoaNovo = new Pessoa
                    {
                        Id = entitiesInfo.Professores[i].Pessoa.Id,
                        PrimeiroNome = entitiesInfo.Professores[i].Pessoa.PrimeiroNome,
                        UltimoNome = entitiesInfo.Professores[i].Pessoa.UltimoNome,
                        NumeroTelefone = entitiesInfo.Professores[i].Pessoa.NumeroTelefone,
                        DataNascimento = entitiesInfo.Professores[i].Pessoa.DataNascimento,
                        EnderecoId = entitiesInfo.Professores[i].Pessoa.EnderecoId
                    };

                    contexto.Pessoas.Add(pessoaNovo);

                 
                    Professor professorNovo = new Professor
                    {
                        Id = entitiesInfo.Professores[i].Id,
                        PessoaId = entitiesInfo.Professores[i].PessoaId,
                        Especializacao = entitiesInfo.Professores[i].Especializacao,
                    };

                    contexto.Professores.Add(professorNovo);

                    Curso cursoNovo = new Curso
                    {
                        Id = entitiesInfo.Cursos[i].Id,
                        Nome = entitiesInfo.Cursos[i].Nome,
                        Preco = entitiesInfo.Cursos[i].Preco,
                        Descricao = entitiesInfo.Cursos[i].Descricao,
                        ProfessorId = entitiesInfo.Cursos.First(x => x.ProfessorId == entitiesInfo.Professores[i].Id).ProfessorId,
                };

                    contexto.Cursos.Add(cursoNovo);


                }

                contexto.SaveChanges();

            }
            
        }

        public void RunUpdateTeacher()
        {
            using (var contexto = new MeuContexto())
            {
                var professores = contexto.Professores
    .Include(e => e.Pessoa)
        .ThenInclude(p => p.Endereco)
    .ToList();

                
                foreach (var professor in professores)
                {
                    professor.Especializacao = "Nova especialização";

                    professor.Pessoa.PrimeiroNome = "Novo primeiro nome";
                    professor.Pessoa.UltimoNome = "Novo ultimo nome";
                    professor.Pessoa.NumeroTelefone = "Novo numero de cel";
                    professor.Pessoa.DataNascimento = DateTime.Now;


                    professor.Pessoa.Endereco.Pais = "Australia";
                    professor.Pessoa.Endereco.Estado = "novo estado";
                    professor.Pessoa.Endereco.Cidade = "Nova cidade";
                    professor.Pessoa.Endereco.Rua = "Nova rua";
                    professor.Pessoa.Endereco.Numero = "1";

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

        public void RunDeleteTeacher()
        {
            using (var contexto = new MeuContexto())
            {

                var professores = contexto.Professores
   .Include(p => p.Pessoa)
       .ThenInclude(e => e.Endereco)
   .Include(p => p.Cursos)
   .ToList();

                var cursos = contexto.Cursos
    .Include(c => c.Professor)
        .ThenInclude(p => p.Pessoa)
            .ThenInclude(e => e.Endereco)
    .ToList();



                foreach (var curso in cursos)
                {
                    if (curso != null)
                    {

                        contexto.Remove(curso);
                        contexto.Remove(curso.Professor);
                        contexto.Remove(curso.Professor.Pessoa);
                        contexto.Remove(curso.Professor.Pessoa.Endereco);
                        contexto.SaveChanges();
                    }
                }

            }

        }

        public void RunGetTeacher()
        {

            using (var contexto = new MeuContexto())
            {
                var professorCompletos = contexto.Professores
                                   .Include(e => e.Pessoa)
                                   .ThenInclude(p => p.Endereco)
                                   .ToList();

                Console.WriteLine(professorCompletos.Count);
            }

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