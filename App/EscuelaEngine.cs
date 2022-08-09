using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.Entidades;
using CoreEscuela.Util;
using curso_csharp_platzi.Entidades;

namespace CoreEscuela
{
    public sealed class EscuelaEngine
    {
        public Escuela Escuela { get; set; }

        public EscuelaEngine()
        {

        }

        public void Inicializar()
        {
            Escuela = new Escuela("Platzi Academy", 2012, TiposEscuela.Primaria,
            ciudad: "Bogotá", pais: "Colombia"
            );

            CargarCursos();
            CargarAsignaturas();
            CargarEvaluaciones();

        }

        public void ImprimirDiccionario(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase> > dic, bool imprimirEval = false){
            foreach(var objDic in dic){                
                Printer.WriteTitle(objDic.Key.ToString());
                foreach(var val in objDic.Value){
                    
                    switch(objDic.Key){
                        case LlaveDiccionario.Evaluacion:
                            if(imprimirEval){
                                Console.WriteLine(val);        
                            }
                        break;
                        case LlaveDiccionario.Escuela:
                            Console.WriteLine("Escuela:" + val);
                        break;
                        case LlaveDiccionario.Alumno:
                            Console.WriteLine("Alumno: " + val.Nombre);
                        break;
                        case LlaveDiccionario.Curso:
                            var cursoTmp = val as Curso;
                            if(cursoTmp != null){
                                int count = cursoTmp.Alumnos.Count;
                                Console.WriteLine("Curso: " + val.Nombre + "Cantidad Alumnos: " + count) ;
                            }
                        break;
                        default:
                            Console.WriteLine("Escuela:" + val);
                        break;
                    }                    
                }
            }
        }
        public Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase> > GetDiccionarioObjetos(){
            
            Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> diccionario = new Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>>();
            diccionario.Add(LlaveDiccionario.Escuela, new[] {Escuela});
            diccionario.Add(LlaveDiccionario.Curso, Escuela.Cursos.Cast<ObjetoEscuelaBase>());

            var listTmpEvaluaciones = new List<Evaluación>();
            var listTmpAsignaturas = new List<Asignatura>();
            var listTmpAlumnos = new List<Alumno>();

            foreach(var curso in Escuela.Cursos){
                listTmpAsignaturas.AddRange(curso.Asignaturas);
                listTmpAlumnos.AddRange(curso.Alumnos);
                foreach(var alumno in curso.Alumnos){
                    listTmpEvaluaciones.AddRange(alumno.Evaluaciones);
                }
            }
            diccionario.Add(LlaveDiccionario.Asignatura, listTmpAsignaturas.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlaveDiccionario.Alumno, listTmpAlumnos.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlaveDiccionario.Evaluacion, listTmpEvaluaciones.Cast<ObjetoEscuelaBase>());

            return diccionario;
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(            
            bool traeEvaluaciones = true,
            bool traerAlumnos = true,
            bool traerAsignaturas = true,
            bool traerCursos = true)
        {
            int dummy;
            return GetObjetosEscuela(out dummy, out dummy, out dummy, out dummy,traeEvaluaciones,traerAlumnos,traerAsignaturas,traerCursos);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            out int conteoEvaluaciones,            
            bool traeEvaluaciones = true,
            bool traerAlumnos = true,
            bool traerAsignaturas = true,
            bool traerCursos = true)
        {
            int dummy;
            return GetObjetosEscuela(out conteoEvaluaciones, out dummy, out dummy, out dummy,traeEvaluaciones,traerAlumnos,traerAsignaturas,traerCursos);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            out int conteoEvaluaciones,
            out int conteoCursos,            
            bool traeEvaluaciones = true,
            bool traerAlumnos = true,
            bool traerAsignaturas = true,
            bool traerCursos = true)
        {
            int dummy;
            return GetObjetosEscuela(out conteoEvaluaciones, out conteoCursos, out dummy, out dummy,traeEvaluaciones,traerAlumnos,traerAsignaturas,traerCursos);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            out int conteoEvaluaciones,
            out int conteoCursos,
            out int conteoAsignaturas,         
            bool traeEvaluaciones = true,
            bool traerAlumnos = true,
            bool traerAsignaturas = true,
            bool traerCursos = true)
        {
            int dummy;
            return GetObjetosEscuela(out conteoEvaluaciones, out conteoCursos, out conteoAsignaturas, out dummy,traeEvaluaciones,traerAlumnos,traerAsignaturas,traerCursos);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            out int conteoEvaluaciones,
            out int conteoCursos,
            out int conteoAsignaturas,
            out int conteoAlumnos,
            bool traeEvaluaciones = true,
            bool traerAlumnos = true,
            bool traerAsignaturas = true,
            bool traerCursos = true)
        {
            conteoEvaluaciones = conteoAsignaturas = conteoAlumnos = 0;

            var listaObj = new List<ObjetoEscuelaBase>();
            listaObj.Add(Escuela);

            if(traerCursos){
                listaObj.AddRange(Escuela.Cursos);
            }

            conteoCursos = Escuela.Cursos.Count;
            foreach (var curso in Escuela.Cursos)
            {                   
                conteoAsignaturas += curso.Asignaturas.Count;
                conteoAlumnos += curso.Alumnos.Count;
                if(traerAsignaturas){
                    listaObj.AddRange(curso.Asignaturas);
                }
                if(traerAlumnos){
                    listaObj.AddRange(curso.Alumnos);
                }

                if(traeEvaluaciones){
                    foreach (var alumno in curso.Alumnos)
                    {
                        listaObj.AddRange(alumno.Evaluaciones);
                        conteoEvaluaciones += alumno.Evaluaciones.Count;
                    }
                }
            }

            return listaObj.AsReadOnly();
        }        

        #region Métodos de Carga
        private void CargarEvaluaciones()
        {

            var rnd = new Random();
            foreach (var curso in Escuela.Cursos)
            {
                foreach (var asignatura in curso.Asignaturas)
                {
                    foreach (var alumno in curso.Alumnos)
                    {

                        for (int i = 0; i < 5; i++)
                        {
                            var ev = new Evaluación
                            {
                                Asignatura = asignatura,
                                Nombre = $"{asignatura.Nombre} Ev#{i + 1}",
                                Nota = MathF.Round(5 * (float)rnd.NextDouble(),2),
                                Alumno = alumno
                            };
                            alumno.Evaluaciones.Add(ev);
                        }
                    }
                }
            }

        }



        private void CargarAsignaturas()
        {
            foreach (var curso in Escuela.Cursos)
            {
                var listaAsignaturas = new List<Asignatura>(){
                            new Asignatura{Nombre="Matemáticas"} ,
                            new Asignatura{Nombre="Educación Física"},
                            new Asignatura{Nombre="Castellano"},
                            new Asignatura{Nombre="Ciencias Naturales"}
                };
                curso.Asignaturas = listaAsignaturas;
            }
        }

        private List<Alumno> GenerarAlumnosAlAzar(int cantidad)
        {
            string[] nombre1 = { "Alba", "Felipa", "Eusebio", "Farid", "Donald", "Alvaro", "Nicolás" };
            string[] apellido1 = { "Ruiz", "Sarmiento", "Uribe", "Maduro", "Trump", "Toledo", "Herrera" };
            string[] nombre2 = { "Freddy", "Anabel", "Rick", "Murty", "Silvana", "Diomedes", "Nicomedes", "Teodoro" };

            var listaAlumnos = from n1 in nombre1
                               from n2 in nombre2
                               from a1 in apellido1
                               select new Alumno { Nombre = $"{n1} {n2} {a1}" };

            return listaAlumnos.OrderBy((al) => al.UniqueId).Take(cantidad).ToList();
        }

        private void CargarCursos()
        {
            Escuela.Cursos = new List<Curso>(){
                        new Curso(){ Nombre = "101", Jornada = TiposJornada.Mañana },
                        new Curso() {Nombre = "201", Jornada = TiposJornada.Mañana},
                        new Curso{Nombre = "301", Jornada = TiposJornada.Mañana},
                        new Curso(){ Nombre = "401", Jornada = TiposJornada.Tarde },
                        new Curso() {Nombre = "501", Jornada = TiposJornada.Tarde},
            };

            Random rnd = new Random();
            foreach (var c in Escuela.Cursos)
            {
                int cantRandom = rnd.Next(5, 20);
                c.Alumnos = GenerarAlumnosAlAzar(cantRandom);
            }
        }
    }
    #endregion
}