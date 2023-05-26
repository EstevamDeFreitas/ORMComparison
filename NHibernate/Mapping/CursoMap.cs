using EntidadesHiber;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHiber.Mapping;

public class CursoMap : ClassMap<Curso>
{
    public CursoMap()
    {
        Id(x => x.Id);
        Map(x => x.Nome);
        Map(x => x.Preco);
        Map(x => x.Descricao);
        References(x => x.Professor).Column("ProfessorId");

        Table("Curso");
    }
}
