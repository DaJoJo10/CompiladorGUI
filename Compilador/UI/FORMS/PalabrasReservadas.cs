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
            { "VAR",    Token.VAR      },   // 102
            { "INT",    Token.INT      },   // 103
            { "FLOAT",  Token.FLOAT_KW },   // 104
            { "IF",     Token.IF       },   // 105
            { "THEN",   Token.THEN     },   // 106
            { "ELSE",   Token.ELSE     },   // 107
            { "WHILE",  Token.WHILE    },   // 108
            { "PRINT",  Token.PRINT    },   // 109
        };

        public int ObtenerToken(string lexema)
        {
            if (string.IsNullOrWhiteSpace(lexema))
                return Token.ID;

            var clave = lexema.ToUpperInvariant();
            return _mapa.TryGetValue(clave, out int token) ? token : Token.ID;
        }
    }
}
