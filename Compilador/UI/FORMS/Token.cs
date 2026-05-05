using System;

namespace Compilador.UI.FORMS
{
    public class Token
    {
        // Identificadores y palabras reservadas (100-199)
        public const int ID = 101;
        public const int VAR = 102;
        public const int INT = 103;
        public const int FLOAT_KW = 104;
        public const int IF = 105;
        public const int THEN = 106;
        public const int ELSE = 107;
        public const int WHILE = 108;
        public const int PRINT = 109;
        public const int BEGIN = 111;
        public const int END = 112;
        public const int PROGRAM = 120;
        public const int DO = 113;
        public const int WRITE = 114;
        public const int PROCEDURE = 115;

        // Numeros (200-299)
        public const int NUM_INT = 200;
        public const int NUM_REAL = 201;

        // Simbolos y operadores simples (300-399)
        public const int SEMICOLON = 300;
        public const int ASSIGN = 301;  // =
        public const int DIVIDE = 302;  // /
        public const int PLUS = 303;  // +
        public const int MINUS = 304;  // -
        public const int MULTIPLY = 305;  // *
        public const int GT = 306;  // >
        public const int LT = 307;  // <
        public const int COLON = 308;  // :
        public const int LPAREN = 309;  // (
        public const int RPAREN = 310;  // )
        public const int LBRACE = 311;  // {
        public const int RBRACE = 312;  // }
        public const int COMMA = 313;  // ,
        public const int QUOTE = 314;  // '

        // Operadores compuestos
        public const int EQ = 315;  // ==
        public const int GTE = 316;  // >=
        public const int LTE = 317;  // <=

        // Comentario de una linea
        public const int COMMENT = 318;  // //


        // Errores (500+)
        public const int ERROR = 500;

        public int Tipo { get; }
        public string Lexema { get; }
        public int Linea { get; }

        public Token(int tipo, string lexema, int linea)
        {
            Tipo = tipo;
            Lexema = lexema;
            Linea = linea;
        }

        public override string ToString()
            => $"[Linea {Linea,3}]  Tipo = {Tipo,3}  Lexema = '{Lexema}'";
    }
}
