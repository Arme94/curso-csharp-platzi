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

        public IEnumerable<Evaluación>  GetListaEvaluaciones(){
            var lista = _diccionario.GetValueOrDefault(LlaveDiccionario.Evaluacion);

            return lista.Cast<Evaluación>();
        }

        public IEnumerable<Asignatura>  GetListaAsignaturas(){
            var listaEval = GetListaEvaluaciones();

            return (from Evaluación ev in listaEval
                    select ev.Asignatura ).DistinctBy(Asig => Asig.Nombre);

        }
    }
}