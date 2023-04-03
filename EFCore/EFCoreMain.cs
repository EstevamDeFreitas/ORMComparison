using EFCore;
using Entidades;
using Microsoft.EntityFrameworkCore;

namespace EFCore
{
    public class EFCoreMain
    {

        static void Main(string[] args)
        {
            EFCoreMain main = new EFCoreMain();
            // faça chamadas aos métodos da classe EFCoreMain aqui
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