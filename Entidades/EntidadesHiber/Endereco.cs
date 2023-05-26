using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesHiber
{
    public class Endereco
    {
        public virtual Guid Id { get; set; }
        public virtual string Pais { get; set; }
        public virtual string Estado { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Rua { get; set; }
        public virtual string Numero { get; set; }
    }
}
