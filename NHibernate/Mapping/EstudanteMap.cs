using EntidadesHiber;
using FluentNHibernate.Mapping;

namespace NHiber.Mapping;

public class EstudanteMap : ClassMap<Estudante>
{
    public EstudanteMap()
    {
        Id(x => x.Id);
        Map(x => x.Descricao);
        References(x => x.Pessoa).Column("PessoaId");

        Table("Estudante");
    }
}
