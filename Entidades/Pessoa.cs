using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Pessoa
    {
        public Guid Id { get; set; }
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public string NumeroTelefone { get; set; }
        public DateOnly DataNascimento { get; set; }
        public Guid EnderecoId { get; set; }

        public Endereco Endereco { get; set; }
    }
}
