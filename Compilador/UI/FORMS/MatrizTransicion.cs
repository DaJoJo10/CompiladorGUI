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
        // 0 = Letra o '_'
        // 1 = Dígito
        // 2 = Espacio en blanco
        // 3 = Otro símbolo (operadores, delimitadores, etc.)

        // Estados o RENGLONeS:
        // 0 = inicial
        // 1 = leyendo ID / palabra reservada
        // 2 = leyendo número entero

        // Valores ACEPTADOS:
        // 100 = aceptar ID/palabra reservada
        // 200 = aceptar número entero
        // >500 = error léxico

        private readonly int[,] _matriz =
        {
        //      L     D    ESP  OTRO
        /*0*/  { 1,   2,    0,  501 }, // inicio
        /*1*/  { 1,   1,  100,  100 }, // ID o Palabra Reservada
        /*2*/  {501,  2,  200,  200 }  // número entero
    };

        public int ObtenerColumna(char c)
        {
            if (char.IsLetter(c) || c == '_')
                return 0; // columna de letras/underscore

            if (char.IsDigit(c))
                return 1; // dígito

            if (char.IsWhiteSpace(c))
                return 2; // espacios

            return 3; // otros caracteres
        }

        public int SiguienteEstado(int estado, int columna)
        {
            if (estado < 0 || estado > 2)
                throw new ArgumentOutOfRangeException(nameof(estado));

            return _matriz[estado, columna];
        }
    }
}
