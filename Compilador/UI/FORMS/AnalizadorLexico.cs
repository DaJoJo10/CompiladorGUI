using System;
using System.Collections.Generic;
using System.Text;

namespace Compilador.UI.FORMS
{
    public class AnalizadorLexico
    {
        private readonly MatrizTransicion _matriz;
        private readonly PalabrasReservadas _palabrasReservadas;

        public AnalizadorLexico()
        {
            _matriz = new MatrizTransicion();
            _palabrasReservadas = new PalabrasReservadas();
        }

        public ResultadoLexico Analizar(CodigoFuente fuente)
        {
            var resultado = new ResultadoLexico();
            resultado.AgregarAviso("LEXICO INICIADO");

            try
            {
                for (int numLinea = 1; numLinea <= fuente.NumeroLineas; numLinea++)
                    ProcesarLinea(fuente.ObtenerLinea(numLinea), numLinea, resultado);

                resultado.AgregarAviso("LEXICO FINALIZADO EXITOSAMENTE");
            }
            catch (Exception ex)
            {
                resultado.AgregarAviso("ERROR GRAVE EN LEXICO: " + ex.Message);
            }

            resultado.Tokens.Add(new Token(999, "EOF", fuente.NumeroLineas));
            return resultado;
        }

        private void ProcesarLinea(string lineaOriginal, int numLinea, ResultadoLexico resultado)
        {
            int estado = 0;
            var lexema = new StringBuilder();

            // Agrega espacio al final para forzar cierre del ultimo lexema
            string linea = (lineaOriginal ?? string.Empty) + " ";

            for (int i = 0; i < linea.Length; i++)
            {
                char c = linea[i];
                int columna = _matriz.ObtenerColumna(c);
                int valor = _matriz.SiguienteEstado(estado, columna);

                // ── Estado 0: ignorar / reset ────────────────────────────────
                if (valor == 0)
                {
                    estado = 0;
                    lexema.Clear();
                }
                // ── Estados intermedios (acumular) ───────────────────────────
                else if (valor < 100)
                {
                    estado = valor;
                    lexema.Append(c);
                }
                // ── Aceptacion ID / palabra reservada (con retroceso) ────────
                else if (valor == 100)
                {
                    string lex = lexema.ToString();
                    EmitirToken(_palabrasReservadas.ObtenerToken(lex), lex, numLinea, resultado);
                    lexema.Clear();
                    estado = 0;
                    i--;
                }
                // ── Aceptacion NUM_INT (con retroceso) ───────────────────────
                else if (valor == 200)
                {
                    string lex = lexema.ToString();
                    EmitirToken(Token.NUM_INT, lex, numLinea, resultado);
                    lexema.Clear();
                    estado = 0;
                    i--;
                }
                // ── Aceptacion NUM_REAL (con retroceso) ──────────────────────
                else if (valor == 201)
                {
                    string lex = lexema.ToString();
                    EmitirToken(Token.NUM_REAL, lex, numLinea, resultado);
                    lexema.Clear();
                    estado = 0;
                    i--;
                }
                // ── Simbolos simples (sin retroceso) ─────────────────────────
                else if (valor >= 300 && valor <= 399)
                {
                    // Si habia lexema acumulado, emitirlo primero
                    if (lexema.Length > 0)
                    {
                        string lex = lexema.ToString();
                        EmitirToken(_palabrasReservadas.ObtenerToken(lex), lex, numLinea, resultado);
                        lexema.Clear();
                    }
                    EmitirToken(valor, c.ToString(), numLinea, resultado);
                    estado = 0;
                }
                // ── Inicio de posible '==' ───────────────────────────────────
                else if (valor == 400)
                {
                    // Mirar el siguiente caracter
                    if (i + 1 < linea.Length && linea[i + 1] == '=')
                    {
                        i++; // consumir el segundo '='
                        EmitirToken(Token.EQ, "==", numLinea, resultado);
                    }
                    else
                    {
                        EmitirToken(Token.ASSIGN, "=", numLinea, resultado);
                    }
                    lexema.Clear();
                    estado = 0;
                }
                // ── Inicio de posible '>=' ───────────────────────────────────
                else if (valor == 401)
                {
                    if (i + 1 < linea.Length && linea[i + 1] == '=')
                    {
                        i++;
                        EmitirToken(Token.GTE, ">=", numLinea, resultado);
                    }
                    else
                    {
                        EmitirToken(Token.GT, ">", numLinea, resultado);
                    }
                    lexema.Clear();
                    estado = 0;
                }
                // ── Inicio de posible '<=' ───────────────────────────────────
                else if (valor == 402)
                {
                    if (i + 1 < linea.Length && linea[i + 1] == '=')
                    {
                        i++;
                        EmitirToken(Token.LTE, "<=", numLinea, resultado);
                    }
                    else
                    {
                        EmitirToken(Token.LT, "<", numLinea, resultado);
                    }
                    lexema.Clear();
                    estado = 0;
                }
                // ── Inicio de posible '//' (comentario) ─────────────────────
                else if (valor == 403)
                {
                    if (i + 1 < linea.Length && linea[i + 1] == '/')
                    {
                        // Es un comentario: ignorar el resto de la linea
                        break;
                    }
                    else
                    {
                        EmitirToken(Token.DIVIDE, "/", numLinea, resultado);
                    }
                    lexema.Clear();
                    estado = 0;
                }
                // ── Errores lexicos ──────────────────────────────────────────
                else if (valor >= 500)
                {
                    resultado.AgregarAviso(
                        $"ERROR LEXICO [linea {numLinea}]: caracter no reconocido '{c}'");
                    estado = 0;
                    lexema.Clear();
                }
            }
        }

        private void EmitirToken(int tipo, string lex, int linea, ResultadoLexico resultado)
        {
            resultado.Tokens.Add(new Token(tipo, lex, linea));
        }
    }
}
