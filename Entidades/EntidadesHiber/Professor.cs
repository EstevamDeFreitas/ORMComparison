using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesHiber
{
    public class Professor
    {
        public virtual Guid Id { get; set; }
        public virtual string Especializacao { get; set; }
        public virtual Guid PessoaId { get; set; }

        public virtual Pessoa Pessoa { get; set; }
        public virtual ICollection<Curso> Cursos { get; set; }
    }
}
