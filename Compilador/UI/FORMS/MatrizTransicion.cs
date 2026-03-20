using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.UI.FORMS
{
    public class MatrizTransicion
    {
        // Columnas:
        // 0=Letra/'_'  1=Dígito  2=Espacio  3=Punto  4=Símbolo  5=Otro
        // Estados:
        // 0=inicial  1=ID/PR  2=Entero  3=Real
        // Valores aceptados:
        // 100=ID/PR  200=Entero  201=Real  399=Símbolo  >500=Error
        private readonly int[,] _matriz =
        {
    //       L    D   ESP    .   SYM  OTRO
    /*0*/ {  1,   2,   0,  501, 399, 501 },
    /*1*/ {  1,   1, 100,  100, 100, 100 },
    /*2*/ {501,   2, 200,    3, 200, 200 },
    /*3*/ {501,   3, 201,  501, 201, 201 }
    };

        private static readonly HashSet<char> _simbolos =
            new HashSet<char>(";=/+-*><:(){},");

        private static readonly Dictionary<char, int> _tokenSimbolo =
            new Dictionary<char, int>
        {
        { ';', 300 }, { '=', 301 }, { '/', 302 }, { '+', 303 },
        { '-', 304 }, { '*', 305 }, { '>', 306 }, { '<', 307 },
        { ':', 308 }, { '(', 309 }, { ')', 310 }, { '{', 311 },
        { '}', 312 }, { ',', 313 }
        };

        public int ObtenerColumna(char c)
        {
            if (char.IsLetter(c) || c == '_') return 0;
            if (char.IsDigit(c)) return 1;
            if (char.IsWhiteSpace(c)) return 2;
            if (c == '.') return 3;
            if (_simbolos.Contains(c)) return 4;
            return 5;
        }
        public int SiguienteEstado(int estado, int columna)
        {
            if (estado < 0 || estado > 3)
                throw new ArgumentOutOfRangeException(nameof(estado));
            return _matriz[estado, columna];
        }

        public int ObtenerTokenSimbolo(char c) =>
            _tokenSimbolo.TryGetValue(c, out int t) ? t : 501;
    }
}

