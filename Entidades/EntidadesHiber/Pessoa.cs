using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesHiber
{
    public class Pessoa
    {
        public virtual Guid Id { get; set; }
        public virtual string PrimeiroNome { get; set; }
        public virtual string UltimoNome { get; set; }
        public virtual string NumeroTelefone { get; set; }
        public virtual DateTime DataNascimento { get; set; }
        public virtual Guid EnderecoId { get; set; }

        public virtual Endereco Endereco { get; set; }
    }
}
