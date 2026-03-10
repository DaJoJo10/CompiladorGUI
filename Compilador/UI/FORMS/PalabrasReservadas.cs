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
        { "WHILE",   108 }
    };

        // 101 será el token para ID, PALABRAS, IDENTIFICADORES, VARIABLES
        public int ObtenerToken(string lexema)
        {
            if (string.IsNullOrWhiteSpace(lexema))
                return 101;

            var clave = lexema.ToUpperInvariant();
            return _mapa.TryGetValue(clave, out int token)
                ? token
                : 101; // identificador
        }
    }
}
