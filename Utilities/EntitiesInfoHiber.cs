using Bogus;
using EntidadesHiber;
using System.Diagnostics;

namespace OrmUtilities;

public class EntitiesInfoHiber
{
    private Faker<Endereco> enderecoFaker = new Faker<Endereco>("pt_BR")
                                                .RuleFor(x => x.Numero, f => f.Address.BuildingNumber())
                                                .RuleFor(x => x.Rua, f => f.Address.StreetName())
                                                .RuleFor(x => x.Cidade, f => f.Address.City())
                                                .RuleFor(x => x.Estado, f => f.Address.State())
                                                .RuleFor(x => x.Id, f => f.Random.Guid())
                                                .RuleFor(x => x.Pais, f => f.Address.Country());

    private Faker<Pessoa> pessoaFaker = new Faker<Pessoa>("pt_BR")
                                            .RuleFor(x => x.DataNascimento, f => f.Person.DateOfBirth)
                                            .RuleFor(x => x.Id, f => f.Random.Guid())
                                            .RuleFor(x => x.NumeroTelefone, f => f.Phone.PhoneNumber())
                                            .RuleFor(x => x.PrimeiroNome, f => f.Person.FirstName)
                                            .RuleFor(x => x.UltimoNome, f => f.Person.LastName);

    private Faker<Estudante> estudanteFaker = new Faker<Estudante>("pt_BR")
                                            .RuleFor(x => x.Id, f => f.Random.Guid())
                                            .RuleFor(x => x.Descricao, f => f.Lorem.Word());

    private Faker<Professor> professorFaker = new Faker<Professor>("pt_BR")
                                            .RuleFor(x => x.Especializacao, f => f.Lorem.Word())
                                            .RuleFor(x => x.Id, f => f.Random.Guid());

    private Faker<Curso> cursoFaker = new Faker<Curso>("pt_BR")
                                            .RuleFor(x => x.Id, f => f.Random.Guid())
                                            .RuleFor(x => x.Nome, f => f.Lorem.Word())
                                            .RuleFor(x => x.Descricao, f => f.Lorem.Word());

    private Faker<CursoAluno> cursoAlunoFaker = new Faker<CursoAluno>("pt_BR")
                                            .RuleFor(x => x.Nota, f => f.Random.Int(0, 10));


    public EntitiesInfoHiber()
    {
        Enderecos = new List<Endereco>(10000);

        for (int i = 0; i < Enderecos.Capacity; i++)
        {
            Enderecos.Add(enderecoFaker.Generate());
        }


        Pessoas = new List<Pessoa>(10000);

        for (int i = 0; i < Pessoas.Capacity; i++)
        {
            var pessoa = pessoaFaker.Generate();
            pessoa.EnderecoId = Enderecos[i].Id;
            pessoa.Endereco = Enderecos[i];
            Pessoas.Add(pessoa);
        }

        Estudantes = new List<Estudante>(10000);

        for (int i = 0; i < Estudantes.Capacity; i++)
        {
            var estudante = estudanteFaker.Generate();
            estudante.PessoaId = Pessoas[i].Id;
            estudante.Pessoa = Pessoas[i];
            Estudantes.Add(estudante);
        }

        Professores = new List<Professor>(10000);

        for (int i = 0; i < Professores.Capacity; i++)
        {
            var professor = professorFaker.Generate();

            professor.PessoaId = Pessoas[i].Id;
            professor.Pessoa = Pessoas[i];
            Professores.Add(professor);

            
        }

        Cursos = new List<Curso>(10000);

        for (int i = 0; i < Cursos.Capacity; i++)
        {
            var curso = cursoFaker.Generate();
            curso.ProfessorId = Professores[i].Id;
            curso.Professor = Professores[i];
            Cursos.Add(curso);

        }

        CursosAlunos = new List<CursoAluno>(10000);

        for (int i = 0; i < CursosAlunos.Capacity; i++)
        {
            var cursoAluno = cursoAlunoFaker.Generate();
            cursoAluno.AlunoId = Estudantes[i].Id;
            cursoAluno.CursoId = Cursos[i].Id;
            cursoAluno.Aluno = Estudantes[i];
            cursoAluno.Curso = Cursos[i];
            CursosAlunos.Add(cursoAluno);
        }
    }

    public List<Endereco> Enderecos { get; set; }
    public List<Pessoa> Pessoas { get; set; }
    public List<Estudante> Estudantes { get; set; }
    public List<Professor> Professores { get; set; }
    public List<Curso> Cursos { get; set; }
    public List<CursoAluno> CursosAlunos { get; set; }
}
