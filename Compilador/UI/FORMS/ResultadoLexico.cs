using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.UI.FORMS
{
    public class ResultadoLexico
    {
        public List<Token> Tokens { get; } = new List<Token>();
        public List<string> Avisos { get; } = new List<string>();

        public void AgregarAviso(string mensaje)
        {
            Avisos.Add(mensaje);
        }
    }
}
