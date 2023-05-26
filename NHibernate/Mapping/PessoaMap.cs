using FluentNHibernate.Mapping;
using EntidadesHiber;

namespace NHiber.Mapping;


public class PessoaMap : ClassMap<Pessoa>
{
    public PessoaMap()
    {
        Id(x => x.Id);
        Map(x => x.PrimeiroNome);
        Map(x => x.UltimoNome);
        Map(x => x.NumeroTelefone);
        Map(x => x.DataNascimento);
        References(x => x.Endereco).Column("EnderecoId");

        Table("Pessoa");
    }
}
