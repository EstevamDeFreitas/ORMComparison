using EFCore;
using Entidades;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EFCore
{
    public class EFCoreMain
    {
        private static  string _stringConexao = "";

        private static DbContextOptions<MeuContexto> ObterOpcoesDbContext()
        {
            var builder = new SqlConnectionStringBuilder(_stringConexao);

            var optionsBuilder = new DbContextOptionsBuilder<MeuContexto>();
            optionsBuilder.UseSqlServer(builder.ConnectionString);

            return optionsBuilder.Options;
        }


        public EFCoreMain(string connectionString)
        {
            _stringConexao = connectionString;
        }

        public void InitTest()
        {
            using (var contexto = new MeuContexto())
            {
                /*
                // Cria uma nova instância de uma entidade e salva no banco de dados
                var curso = new Curso
                {
                    Nome = "Introdução ao EF Core",
                    Descricao = "Curso de introdução ao EF Core"
                };

                contexto.Cursos.Add(curso);
                contexto.SaveChanges();*/
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
        }
    }
}