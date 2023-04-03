using EFCore;
using Entidades;
using Microsoft.EntityFrameworkCore;

namespace EFCore
{
    public class EFCoreMain
    {
        private string ConnectionString { get; set; }

        public EFCoreMain(string connectionString)
        {
            ConnectionString = connectionString;
        }

    }

    public class MeuContexto : DbContext
    {
        public MeuContexto(DbContextOptions<MeuContexto> options) : base(options)
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