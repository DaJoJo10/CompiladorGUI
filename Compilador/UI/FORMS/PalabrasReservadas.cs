using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.UI.FORMS
{
    public class PalabrasReservadas
    {
        private readonly Dictionary<string, int> _mapa = new Dictionary<string, int>
    {
        { "VAR",     102 },
        { "INTEGER", 103 },
        { "FLOAT",   104 },
        { "IF",      105 },
        { "THEN",    106 },
        { "ELSE",    107 },
        { "WHILE",   108 },
        { "PRINT",   109 },
        { "INT",     110 }   // ← nuevo
    };

        public int ObtenerToken(string lexema)
        {
            if (string.IsNullOrWhiteSpace(lexema)) return 101;
            return _mapa.TryGetValue(lexema.ToUpperInvariant(), out int token)
                ? token
                : 101;
        }
    }
}
