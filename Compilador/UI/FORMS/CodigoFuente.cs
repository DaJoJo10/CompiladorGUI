using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.UI.FORMS
{
    public class CodigoFuente
    {
        private readonly List<string> _lineas;

        private CodigoFuente(List<string> lineas)
        {
            _lineas = lineas;
        }

        public static CodigoFuente DesdeTexto(string texto)
        {
            if (texto == null)
                texto = string.Empty;

            texto = texto.Replace("\r\n", "\n").Replace("\r", "\n");
            var arreglo = texto.Split('\n');
            return new CodigoFuente(arreglo.ToList());
        }

        public int NumeroLineas => _lineas.Count;

        // Índice 1-base: 1,2,3...
        public string ObtenerLinea(int numeroLinea)
        {
            if (numeroLinea < 1 || numeroLinea > _lineas.Count)
                throw new ArgumentOutOfRangeException(nameof(numeroLinea));

            return _lineas[numeroLinea - 1];
        }

        public IEnumerable<string> ObtenerTodasLasLineas() => _lineas;
    }
}
