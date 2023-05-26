using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesHiber
{
    public class Curso
    {
        public virtual Guid Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual decimal Preco { get; set; }
        public virtual string Descricao { get; set; }
        public virtual Guid ProfessorId { get; set; }

        public virtual Professor Professor { get; set; }
    }
}
