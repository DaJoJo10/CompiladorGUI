using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.UI.FORMS
{
    public class Token
    {
        public int Tipo { get; }
        public string Lexema { get; }
        public int Linea { get; }

        public Token(int tipo, string lexema, int linea)
        {
            Tipo = tipo;
            Lexema = lexema;
            Linea = linea;
        }

        public override string ToString()
        {
            return $"[{Linea}] Tipo={Tipo} Lexema='{Lexema}'";
        }
    }
}
