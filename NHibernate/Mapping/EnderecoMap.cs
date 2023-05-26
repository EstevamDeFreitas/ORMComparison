using EntidadesHiber;
using FluentNHibernate.Mapping;

namespace NHiber.Mapping;

public class EnderecoMap : ClassMap<Endereco>
{
    public EnderecoMap() 
    {
        Id(x => x.Id);
        Map(x => x.Pais);
        Map(x => x.Estado);
        Map(x => x.Cidade);
        Map(x => x.Rua);
        Map(x => x.Numero);

        Table("Endereco");
    }
}
