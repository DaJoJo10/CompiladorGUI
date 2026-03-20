using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            resultado.AgregarAviso("LÉXICO INICIADO");

            try
            {
                for (int numLinea = 1; numLinea <= fuente.NumeroLineas; numLinea++)
                {
                    ProcesarLinea(fuente.ObtenerLinea(numLinea), numLinea, resultado);
                }

                resultado.AgregarAviso("LÉXICO FINALIZADO EXITOSAMENTE");
            }
            catch (Exception ex)
            {
                resultado.AgregarAviso("ERROR GRAVE EN LÉXICO: " + ex.Message);
            }

            return resultado;
        }

        private void ProcesarLinea(string lineaOriginal, int numLinea, ResultadoLexico resultado)
        {
            int estado = 0;
            var lexema = new StringBuilder();

            string linea = (lineaOriginal ?? string.Empty) + " ";

            for (int i = 0; i < linea.Length; i++)
            {
                char c = linea[i];
                int columna = _matriz.ObtenerColumna(c);
                int valorMatriz = _matriz.SiguienteEstado(estado, columna);

                if (valorMatriz == 0)
                {
                    estado = 0; lexema.Clear();
                }
                else if (valorMatriz < 100)
                {
                    // Estado intermedio: seguir acumulando
                    estado = valorMatriz;
                    lexema.Append(c);
                }
                else if (valorMatriz == 100)
                {
                    // Aceptar ID / Palabra reservada
                    string lex = lexema.ToString();
                    int token = _palabrasReservadas.ObtenerToken(lex);
                    resultado.Tokens.Add(new Token(token, lex, numLinea));
                    Console.WriteLine($"Token: {token} | Lexema: {lex} | Línea: {numLinea}");
                    lexema.Clear(); estado = 0;
                    i--; // reprocesar el carácter que terminó el lexema
                }
                else if (valorMatriz == 200)
                {
                    // Aceptar número ENTERO
                    string lex = lexema.ToString();
                    resultado.Tokens.Add(new Token(200, lex, numLinea));
                    Console.WriteLine($"Token: 200 | Lexema: {lex} | Línea: {numLinea}");
                    lexema.Clear(); estado = 0;
                    i--;
                }
                else if (valorMatriz == 201)
                {
                    // Aceptar número REAL
                    string lex = lexema.ToString();
                    resultado.Tokens.Add(new Token(201, lex, numLinea));
                    Console.WriteLine($"Token: 201 | Lexema: {lex} | Línea: {numLinea}");
                    lexema.Clear(); estado = 0;
                    i--;
                }
                else if (valorMatriz == 399)
                {
                    // Aceptar SÍMBOLO de un solo carácter (desde estado 0)
                    int token = _matriz.ObtenerTokenSimbolo(c);
                    resultado.Tokens.Add(new Token(token, c.ToString(), numLinea));
                    Console.WriteLine($"Token: {token} | Lexema: {c} | Línea: {numLinea}");
                    estado = 0; lexema.Clear();
                }
                else if (valorMatriz > 500)
                {
                    string msg = $"ERROR: En línea [{numLinea}] símbolo no reconocido: '{c}'";
                    resultado.AgregarAviso(msg);
                    Console.WriteLine(msg);
                    estado = 0; lexema.Clear();
                }
            }
        }

    }

}
