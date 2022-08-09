using System;
using System.Linq;
using System.Collections.Generic;
using CoreEscuela.Entidades;

namespace CoreEscuela.App
{
    public class Reporteador
    {   
        Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> _diccionario;
        public Reporteador(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObjEscuela){

            if(dicObjEscuela == null){
                throw new ArgumentNullException(nameof (dicObjEscuela));
            }
            _diccionario = dicObjEscuela;
        }

        public IEnumerable<Escuela>  GetListaEvaluaciones(){
            var lista = _diccionario.GetValueOrDefault(LlaveDiccionario.Escuela);

            return lista.Cast<Escuela>();
        }
    }
}