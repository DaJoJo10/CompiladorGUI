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
        // 2 = Punto '.'
        // 3 = Espacio en blanco
        // 4 = Operador (+ - * / < > = :)
        // 5 = Separador (; { } ( ) [ ])
        // Estados o RENGLONeS:
        // 0 = inicial
        // 1 = leyendo ID / palabra reservada
        // 2 = leyendo número entero
        // 3 = leyendo número real
        // 4 = leyendo operador
        // 5 = leyendo separador
        // Valores ACEPTADOS:
        // 100 = aceptar ID/palabra reservada
        // 200 = aceptar número entero
        // 300 = aceptar número real
        // 400 = aceptar operador
        // 500 = aceptar separador
        // >500 = error léxico
        private readonly int[,] _matriz =
        {
            //      L     D    .   ESP  OP   SEP
            /*0*/  { 1,   2,  600, 0,  4,   5 },      // inicio
            /*1*/  { 1,   1,  600, 100, 100, 100 },   // ID o Palabra Reservada
            /*2*/  { 600, 2,  3,  200, 200, 200 },    // número entero
            /*3*/  { 600, 3,  600, 300, 300, 300 },   // número real
            /*4*/  { 600, 600, 600, 400, 600, 400 },  // operador
            /*5*/  { 600, 600, 600, 500, 600, 600 }   // separador
        };
        public int ObtenerColumna(char c)
        {
            if (char.IsLetter(c) || c == '_')
                return 0; // columna de letras/underscore
            if (char.IsDigit(c))
                return 1; // dígito
            if (c == '.')
                return 2; // punto para números reales
            if (char.IsWhiteSpace(c))
                return 3; // espacios
            if ("+-*/<>=:".Contains(c.ToString()))
                return 4; // operadores
            if (";(){}[]".Contains(c.ToString()))
                return 5; // separadores
            return 600; // otros caracteres = error
        }
        public int SiguienteEstado(int estado, int columna)
        {
            if (estado < 0 || estado > 5)
                throw new ArgumentOutOfRangeException(nameof(estado));
            return _matriz[estado, columna];
        }
    }
}
