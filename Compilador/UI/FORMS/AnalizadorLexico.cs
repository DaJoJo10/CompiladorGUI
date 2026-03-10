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
            int token = 0;
            // Agregar un espacio al final para forzar el cierre de lexemas
            string linea = (lineaOriginal ?? string.Empty) + " ";

            for (int i = 0; i < linea.Length; i++)
            {
                char c = linea[i];
                int columna = _matriz.ObtenerColumna(c);
                int valorMatriz = _matriz.SiguienteEstado(estado, columna);

                if (valorMatriz == 0)
                {
                    // Reset
                    estado = 0;
                    lexema.Clear();
                    token = 0;
                }
                else if (valorMatriz < 100)
                {
                    // Estado interno (leyendo ID o número)
                    estado = valorMatriz;
                    lexema.Append(c);
                    token = 0;
                }
                else if (valorMatriz == 100)
                {
                    // Fin de ID / palabra reservada
                    string lex = lexema.ToString();
                    token = _palabrasReservadas.ObtenerToken(lex);
                    resultado.Tokens.Add(new Token(token, lex, numLinea));
                    Console.WriteLine($"Token: {token} | Lexema: {lex} | Línea: {numLinea}");
                    lexema.Clear();
                    estado = 0;
                    i--; // reprocesar el mismo carácter en el estado 0
                }
                else if (valorMatriz == 200)
                {
                    // Fin de número entero
                    string lex = lexema.ToString();
                    token = 200;
                    resultado.Tokens.Add(new Token(token, lex, numLinea));
                    Console.WriteLine($"Token: {token} | Lexema: {lex} | Línea: {numLinea}");
                    lexema.Clear();
                    estado = 0;
                    i--; // reprocesar el mismo carácter en el estado 0
                }
                else if (valorMatriz > 500)
                {
                    // Error léxico
                    string mensaje = $"ERROR: En línea [{numLinea}] símbolo no reconocido: '{c}'";
                    resultado.AgregarAviso(mensaje);
                    Console.WriteLine(mensaje);
                    // Reset tras el error
                    estado = 0;
                    lexema.Clear();
                    token = valorMatriz;
                }
            }
        }
    }

}
