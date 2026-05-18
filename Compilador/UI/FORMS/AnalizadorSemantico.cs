using System;
using System.Collections.Generic;
using System.Linq;

namespace Compilador.UI.FORMS
{
    public class Simbolo
    {
        public string Nombre { get; set; }
        public string TipoDato { get; set; }
        public int LineaDeclaracion { get; set; }

        public Simbolo(string nombre, string tipoDato, int lineaDeclaracion)
        {
            Nombre = nombre;
            TipoDato = tipoDato;
            LineaDeclaracion = lineaDeclaracion;
        }
    }

    public class AnalizadorSemantico
    {
        private List<Token> _tokens;
        private int _posicionActual;
        private Token _tokenActual;

        public List<Simbolo> TablaSimbolos { get; } = new List<Simbolo>();
        public List<string> Errores { get; } = new List<string>();

        public void Analizar(List<Token> tokensTotales)
        {
            _tokens = tokensTotales.ToList();
            TablaSimbolos.Clear();
            Errores.Clear();

            if (_tokens.Count == 0)
                return;

            _posicionActual = 0;
            _tokenActual = _tokens[_posicionActual];

            RecolectarDeclaraciones();

            _posicionActual = 0;
            _tokenActual = _tokens[_posicionActual];

            ValidarUsoVariables();
        }

        private void Avanzar()
        {
            _posicionActual++;
            if (_posicionActual < _tokens.Count)
                _tokenActual = _tokens[_posicionActual];
        }

        private void RecolectarDeclaraciones()
        {
            while (_posicionActual < _tokens.Count)
            {
                if (_tokenActual.Tipo == Token.VAR)
                {
                    Avanzar();
                    ProcesarBloqueVar();
                }
                else
                {
                    Avanzar();
                }
            }
        }

        private void ProcesarBloqueVar()
        {
            while (_posicionActual < _tokens.Count && _tokenActual.Tipo == Token.ID)
            {
                var nombres = new List<(string nombre, int linea)>();
                nombres.Add((_tokenActual.Lexema, _tokenActual.Linea));
                Avanzar();

                while (_posicionActual < _tokens.Count && _tokenActual.Lexema == ",")
                {
                    Avanzar();
                    if (_tokenActual.Tipo == Token.ID)
                    {
                        nombres.Add((_tokenActual.Lexema, _tokenActual.Linea));
                        Avanzar();
                    }
                }

                if (_posicionActual < _tokens.Count && _tokenActual.Lexema == ":")
                    Avanzar();

                string tipoDato = "DESCONOCIDO";
                if (_posicionActual < _tokens.Count)
                {
                    if (_tokenActual.Tipo == Token.INT)
                        tipoDato = "INT";
                    else if (_tokenActual.Tipo == Token.FLOAT_KW)
                        tipoDato = "FLOAT";
                    Avanzar();
                }

                foreach (var (nombre, linea) in nombres)
                {
                    if (TablaSimbolos.Any(s => s.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
                    {
                        Errores.Add($"Error semántico en línea {linea}: La variable '{nombre}' ya fue declarada.");
                    }
                    else
                    {
                        TablaSimbolos.Add(new Simbolo(nombre, tipoDato, linea));
                    }
                }

                if (_posicionActual < _tokens.Count && _tokenActual.Lexema == ";")
                    Avanzar();
            }
        }

        private Simbolo BuscarSimbolo(string nombre)
        {
            return TablaSimbolos.FirstOrDefault(s => s.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
        }

        private void ValidarUsoVariables()
        {
            while (_posicionActual < _tokens.Count)
            {
                if (_tokenActual.Tipo == Token.ID)
                {
                    string nombre = _tokenActual.Lexema;
                    int linea = _tokenActual.Linea;

                    if (EsNombrePrograma(nombre))
                    {
                        Avanzar();
                        continue;
                    }

                    var simbolo = BuscarSimbolo(nombre);
                    if (simbolo == null)
                    {
                        Errores.Add($"Error semántico en línea {linea}: La variable '{nombre}' no ha sido declarada.");
                        Avanzar();
                        continue;
                    }

                    Avanzar();

                    if (_posicionActual < _tokens.Count && _tokenActual.Lexema == ":=")
                    {
                        Avanzar();
                        string tipoExpr = InferirTipoExpresion();

                        if (tipoExpr != "DESCONOCIDO" && simbolo.TipoDato != tipoExpr)
                        {
                            if (!(simbolo.TipoDato == "FLOAT" && tipoExpr == "INT"))
                            {
                                Errores.Add($"Error semántico en línea {linea}: No se puede asignar un valor de tipo '{tipoExpr}' a la variable '{nombre}' de tipo '{simbolo.TipoDato}'.");
                            }
                        }
                    }
                }
                else
                {
                    Avanzar();
                }
            }
        }

        private bool EsNombrePrograma(string nombre)
        {
            for (int i = 0; i < _tokens.Count - 1; i++)
            {
                if (_tokens[i].Tipo == Token.PROGRAM && _tokens[i + 1].Tipo == Token.ID)
                    return _tokens[i + 1].Lexema.Equals(nombre, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private string InferirTipoExpresion()
        {
            string tipo = "DESCONOCIDO";
            bool tieneReal = false;
            bool tieneEntero = false;

            while (_posicionActual < _tokens.Count &&
                   _tokenActual.Lexema != ";" &&
                   _tokenActual.Tipo != Token.END &&
                   _tokenActual.Tipo != Token.THEN &&
                   _tokenActual.Tipo != Token.DO)
            {
                if (_tokenActual.Tipo == Token.NUM_INT)
                    tieneEntero = true;
                else if (_tokenActual.Tipo == Token.NUM_REAL)
                    tieneReal = true;
                else if (_tokenActual.Tipo == Token.ID)
                {
                    var sim = BuscarSimbolo(_tokenActual.Lexema);
                    if (sim != null)
                    {
                        if (sim.TipoDato == "FLOAT")
                            tieneReal = true;
                        else if (sim.TipoDato == "INT")
                            tieneEntero = true;
                    }
                }

                Avanzar();
            }

            if (tieneReal)
                tipo = "FLOAT";
            else if (tieneEntero)
                tipo = "INT";

            return tipo;
        }
    }
}
