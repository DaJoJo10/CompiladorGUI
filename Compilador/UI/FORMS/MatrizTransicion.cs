using System;

namespace Compilador.UI.FORMS
{
    /// <summary>
    /// Matriz de transicion del AFD del analizador lexico.
    ///
    /// Columnas (20):
    ///  0=Letra/_, 1=Digito, 2=Punto, 3=Espacio, 4=;, 5==, 6=/, 7=+, 8=-,
    ///  9=*, 10=>, 11=<, 12=:, 13=(, 14=), 15={, 16=}, 17=,, 18=', 19=OTRO
    ///
    /// Convenciones de valores:
    ///   0        = estado inicial (reset + ignorar caracter)
    ///   1..19    = estado intermedio (acumular lexema)
    ///   100      = aceptar ID / palabra reservada (retroceso)
    ///   200      = aceptar NUM_INT               (retroceso)
    ///   201      = aceptar NUM_REAL              (retroceso)
    ///   300..399 = aceptar simbolo simple        (sin retroceso)
    ///   400      = inicio posible operador compuesto == (se maneja en el analizador)
    ///   401      = inicio posible >= 
    ///   402      = inicio posible <=
    ///   403      = inicio posible // (comentario)
    ///   500+     = error lexico
    /// </summary>
    public class MatrizTransicion
    {
        private const int NUM_COLS = 20;

        //                    L    D    .  ESP   ;    =    /    +    -    *    >    <    :    (    )    {    }    ,    '  OTRO
        private readonly int[,] _matriz =
        {
           /*0 Inicial */ {   1,   2, 501,   0, 300, 400, 403, 303, 304, 305, 401, 402, 308, 309, 310, 311, 312, 313, 314, 500 },
           /*1 ID       */{   1,   1, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100 },
           /*2 NUM_INT  */{ 200,   2,   3, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 502 },
           /*3 NUM_REAL */{ 201,   3, 503, 201, 201, 201, 201, 201, 201, 201, 201, 201, 201, 201, 201, 201, 201, 201, 201, 503 },
        };

        /// <summary>Devuelve el indice de columna para el caracter dado.</summary>
        public int ObtenerColumna(char c)
        {
            if (char.IsLetter(c) || c == '_') return 0;
            if (char.IsDigit(c)) return 1;
            if (c == '.') return 2;
            if (char.IsWhiteSpace(c)) return 3;

            switch (c)
            {
                case ';': return 4;
                case '=': return 5;
                case '/': return 6;
                case '+': return 7;
                case '-': return 8;
                case '*': return 9;
                case '>': return 10;
                case '<': return 11;
                case ':': return 12;
                case '(': return 13;
                case ')': return 14;
                case '{': return 15;
                case '}': return 16;
                case ',': return 17;
                case '\'': return 18;
                default: return 19;
            }
        }

        public int SiguienteEstado(int estado, int columna)
        {
            if (estado < 0 || estado >= _matriz.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(estado),
                    $"Estado {estado} fuera de rango.");
            if (columna < 0 || columna >= NUM_COLS)
                throw new ArgumentOutOfRangeException(nameof(columna),
                    $"Columna {columna} fuera de rango.");

            return _matriz[estado, columna];
        }
    }
}
