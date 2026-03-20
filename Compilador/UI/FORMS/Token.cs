using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.UI.FORMS
{
    public class Token
    {
        public int Tipo { get; }
        public string Lexema { get; }
        public int Linea { get; }

        public int Id { get; }

        public string PalabraReservada { get; }

        public int NumeroEntero { get; }

        public int NumeroReal { get; }

        public string Simbolos {  get; }

        public Token(int tipo, string lexema, int linea, int id, string palabraReservada, int numeroEntero, int numeroReal, string simbolos)
        {
            Tipo = tipo;
            Lexema = lexema;
            Linea = linea;
            Id = id;
            PalabraReservada = palabraReservada;
            NumeroEntero = numeroEntero;
            NumeroReal = numeroReal;
            Simbolos = simbolos;

        }

        public override string ToString()
        {
            return $"[{Linea}] Tipo={Tipo} Lexema='{Lexema}'";
        }
    }
}
