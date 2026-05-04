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
        private int _posicionActual;
        private Token _tokenActual;

        public List<string> Errores { get; } = new List<string>();

        // 🔹 TOKENS
        private const int TKN_ID = 101;
        private const int TKN_PROGRAM = 120;
        private const int TKN_EOF = 999; // 🔥 IMPORTANTE

        public void Parse(List<Token> tokensTotales)
        {
            _tokens = tokensTotales.ToList();

            Errores.Clear();

            if (_tokens.Count == 0)
            {
                Errores.Add("El código fuente está vacío o no generó tokens válidos.");
                return;
            }

            _posicionActual = 0;
            _tokenActual = _tokens[_posicionActual];

            try
            {
                ParserPrograma();

                // ✔ VALIDACIÓN CORRECTA DE FIN
                if (_tokenActual.Tipo != TKN_EOF)
                {
                    Errores.Add($"Error en línea {_tokenActual.Linea}: Tokens inesperados después del final del programa.");
                }
            }
            catch (Exception ex)
            {
                Errores.Add(ex.Message);
            }
        }

        // 🔹 AVANZAR
        private void Avanzar()
        {
            _posicionActual++;

            if (_posicionActual < _tokens.Count)
            {
                _tokenActual = _tokens[_posicionActual];
            }
            else
            {
                // ✔ EOF consistente
                _tokenActual = new Token(TKN_EOF, "EOF", _tokenActual.Linea);
            }
        }

        // 🔹 MATCH POR TIPO
        private void MatchTipo(int tipoEsperado, string mensajeError)
        {
            if (_tokenActual.Tipo == tipoEsperado)
            {
                Avanzar();
            }
            else
            {
                throw new Exception(
                    $"Error sintáctico en línea {_tokenActual.Linea}: {mensajeError}. Encontrado '{_tokenActual.Lexema}'");
            }
        }

        // 🔹 MATCH POR LEXEMA
        private void MatchLexema(string lexemaEsperado, string mensajeError)
        {
            if (_tokenActual.Lexema.Equals(lexemaEsperado, StringComparison.OrdinalIgnoreCase))
            {
                Avanzar();
            }
            else
            {
                throw new Exception(
                    $"Error sintáctico en línea {_tokenActual.Linea}: {mensajeError}. Encontrado '{_tokenActual.Lexema}'");
            }
        }

        // 🔥 REGLA PRINCIPAL
        private void ParserPrograma()
        {
            // PROGRAM ID ; BLOQUE .
            MatchTipo(TKN_PROGRAM, "Se esperaba 'PROGRAM'");
            MatchTipo(TKN_ID, "Se esperaba identificador del programa");
            MatchLexema(";", "Falta ';' después del identificador");

            ParserBloque(); // vacío por ahora

            MatchLexema(".", "Falta '.' al finalizar el programa");
        }

        private void ParserBloque()
        {
            // Se implementa en la siguiente actividad
        }
    }
}