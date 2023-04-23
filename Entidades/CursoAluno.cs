using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class CursoAluno
    {
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
        public int Nota { get; set; }
    }
}
