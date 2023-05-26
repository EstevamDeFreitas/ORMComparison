using EntidadesHiber;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHiber.Mapping;

public class ProfessorMap : ClassMap<Professor>
{
    public ProfessorMap()
    {
        Id(x => x.Id);
        Map(x => x.Especializacao);
        References(x => x.Pessoa).Column("PessoaId");
        HasMany(x => x.Cursos).KeyColumn("ProfessorId");

        Table("Professor");
    }
}
