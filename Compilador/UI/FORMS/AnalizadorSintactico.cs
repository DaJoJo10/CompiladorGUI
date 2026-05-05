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
        private const int TKN_VAR = 102;
        private const int TKN_INT = 103;
        private const int TKN_FLOAT = 104;
        private const int TKN_BEGIN = 111;
        private const int TKN_END = 112;
        private const int TKN_IF = 105;
        private const int TKN_THEN = 106;
        private const int TKN_ELSE = 107;
        private const int TKN_WHILE = 108;
        private const int TKN_DO = 113;
        private const int TKN_WRITE = 114;
        private const int TKN_PROCEDURE = 115;
        private const int TKN_EOF = 999;

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

        private void Avanzar()
        {
            _posicionActual++;

            if (_posicionActual < _tokens.Count)
                _tokenActual = _tokens[_posicionActual];
            else
                _tokenActual = new Token(TKN_EOF, "EOF", _tokenActual.Linea);
        }

        private void MatchTipo(int tipoEsperado, string mensajeError)
        {
            if (_tokenActual.Tipo == tipoEsperado)
                Avanzar();
            else
                throw new Exception($"Error sintáctico en línea {_tokenActual.Linea}: {mensajeError}. Encontrado '{_tokenActual.Lexema}'");
        }

        private void MatchLexema(string lexemaEsperado, string mensajeError)
        {
            if (_tokenActual.Lexema.Equals(lexemaEsperado, StringComparison.OrdinalIgnoreCase))
                Avanzar();
            else
                throw new Exception($"Error sintáctico en línea {_tokenActual.Linea}: {mensajeError}. Encontrado '{_tokenActual.Lexema}'");
        }

        private void ParserPrograma()
        {
            MatchTipo(TKN_PROGRAM, "Se esperaba 'PROGRAM'");
            MatchTipo(TKN_ID, "Se esperaba identificador del programa");
            MatchLexema(";", "Falta ';'");

            ParserBloque();

            MatchLexema(".", "Falta '.' al final");
        }

        private void ParserBloque()
        {
            if (_tokenActual.Tipo == TKN_VAR)
                ParserDeclaracionesVariables();

            while (_tokenActual.Tipo == TKN_PROCEDURE)
                ParserDeclaracionProcedimiento();

            MatchTipo(TKN_BEGIN, "Se esperaba 'BEGIN'");
            ParserInstrucciones();
            MatchTipo(TKN_END, "Se esperaba 'END'");
        }

        private void ParserDeclaracionesVariables()
        {
            MatchTipo(TKN_VAR, "Se esperaba 'VAR'");

            MatchTipo(TKN_ID, "Se esperaba identificador");

            while (_tokenActual.Lexema == ",")
            {
                Avanzar();
                MatchTipo(TKN_ID, "Se esperaba identificador");
            }

            MatchLexema(":", "Falta ':'");

            ParserTipo();

            MatchLexema(";", "Falta ';'");
        }

        private void ParserTipo()
        {
            if (_tokenActual.Tipo == TKN_INT || _tokenActual.Tipo == TKN_FLOAT)
                Avanzar();
            else
                throw new Exception($"Error en línea {_tokenActual.Linea}: tipo inválido");
        }

        private void ParserInstrucciones()
        {
            ParserInstruccion();

            while (_tokenActual.Lexema == ";")
            {
                Avanzar();

                if (_tokenActual.Tipo == TKN_END)
                    return;

                ParserInstruccion();
            }
        }

        private void ParserInstruccion()
        {
            // 🔹 ASIGNACIÓN
            if (_tokenActual.Tipo == TKN_ID)
            {
                Avanzar();
                MatchLexema(":=", "Se esperaba ':='");
                ParserExpresion();
            }

            // 🔹 IF
            else if (_tokenActual.Tipo == TKN_IF)
            {
                Avanzar();
                ParserExpresion();
                MatchTipo(TKN_THEN, "Se esperaba THEN");
                ParserInstruccion();

                if (_tokenActual.Tipo == TKN_ELSE)
                {
                    Avanzar();
                    ParserInstruccion();
                }
            }

            // 🔹 WHILE
            else if (_tokenActual.Tipo == TKN_WHILE)
            {
                Avanzar();
                ParserExpresion();
                MatchTipo(TKN_DO, "Se esperaba DO");
                ParserInstruccion();
            }

            // 🔹 BEGIN ANIDADO
            else if (_tokenActual.Tipo == TKN_BEGIN)
            {
                Avanzar();
                ParserInstrucciones();
                MatchTipo(TKN_END, "Se esperaba END");
            }

            // 🔹 WRITE
            else if (_tokenActual.Tipo == TKN_WRITE)
            {
                Avanzar();
                MatchLexema("(", "Falta '('");

                ParserExpresion();

                while (_tokenActual.Lexema == ",")
                {
                    Avanzar();
                    ParserExpresion();
                }

                MatchLexema(")", "Falta ')'");
            }

            else
            {
                throw new Exception($"Error en línea {_tokenActual.Linea}: instrucción inválida");
            }
        }

        private void ParserExpresion()
        {
            ParserTermino();

            while (_tokenActual.Lexema == "+" || _tokenActual.Lexema == "-")
            {
                Avanzar();
                ParserTermino();
            }
        }

        private void ParserTermino()
        {
            ParserFactor();

            while (_tokenActual.Lexema == "*" || _tokenActual.Lexema == "/")
            {
                Avanzar();
                ParserFactor();
            }
        }

        private void ParserFactor()
        {
            if (_tokenActual.Tipo == TKN_ID ||
                _tokenActual.Tipo == Token.NUM_INT ||
                _tokenActual.Tipo == Token.NUM_REAL)
            {
                Avanzar();
            }
            else if (_tokenActual.Lexema == "(")
            {
                Avanzar();
                ParserExpresion();
                MatchLexema(")", "Falta ')'");
            }
            else
            {
                throw new Exception($"Error en línea {_tokenActual.Linea}: factor inválido");
            }
        }

        private void ParserDeclaracionProcedimiento()
        {
            MatchTipo(TKN_PROCEDURE, "Se esperaba PROCEDURE");
            MatchTipo(TKN_ID, "Se esperaba nombre");

            MatchLexema(";", "Falta ';'");

            ParserBloque();

            MatchLexema(";", "Falta ';'");
        }
    }
}