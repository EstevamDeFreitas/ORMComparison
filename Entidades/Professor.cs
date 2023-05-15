using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Professor
    {
        public Guid Id { get; set; }
        public string Especializacao { get; set; }
        public Guid PessoaId { get; set; }

        public Pessoa Pessoa { get; set; }
        public List<Curso> Cursos { get; set; }
    }
}
