using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.UI.FORMS
{
    public class AnalizadorSintactico
    {
        private List<Token> _tokens;
        private int _posicion;
        private Token _actual;

        public List<string> Errores { get; } = new List<string>();

        public void Parse(List<Token> tokens)
        {
            _tokens = tokens;
            _posicion = 0;
            _actual = _tokens[_posicion];

            Programa();

            if (_actual.Tipo != 999)
            {
                Errores.Add("Error: código extra después del final del programa");
            }
        }

        private void Avanzar()
        {
            if (_posicion < _tokens.Count - 1)
            {
                _posicion++;
                _actual = _tokens[_posicion];
            }
        }

        private void Match(int tipoEsperado)
        {
            if (_actual.Tipo == tipoEsperado)
            {
                Avanzar();
            }
            else
            {
                Errores.Add($"Error en línea {_actual.Linea}: se esperaba {tipoEsperado} y se encontró {_actual.Tipo}");
            }
        }

        // 🔥 REGLA INICIAL (ajústala luego)
        private void Programa()
        {
            // Ejemplo simple:
            // VAR x;

            Match(102); // VAR
            Match(101); // ID
            Match(300); // ;

        }
    }
}
