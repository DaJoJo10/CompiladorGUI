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
        // OPERADORES ← NUEVO
        { "+",       401 },
        { "-",       402 },
        { "*",       403 },
        { "/",       404 },
        { "=",       405 },
        { "<",       406 },
        { ">",       407 },
        { ":",       408 },
        // SEPARADORES ← NUEVO
        { ";",       409 },
        { "(",       501 },
        { ")",       502 },
        { "{",       503 },
        { "}",       504 },
        { "[",       505 },
        { "]",       506 }
    };
        // 101 será el token para ID, PALABRAS, IDENTIFICADORES, VARIABLES
        // 200 = Número entero
        // 300 = Número real
        // 401-408 = Operadores
        // 409, 501-506 = Separadores
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
