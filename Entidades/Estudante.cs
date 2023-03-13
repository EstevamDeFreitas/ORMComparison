using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Estudante
    {
        public Guid Id { get; set; }
        public Guid PessoaId { get; set; }
        public string Descricao { get; set; }

        public Pessoa Pessoa { get; set; }
    }
}
