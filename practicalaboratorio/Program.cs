using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Semana11Tema6
{
    internal class Program
    {
        static int max_estudiantes = 40;
        static int max_cursos = 4;
        static string[] nombre_cursos = { "Matematicas", "Programacion", "Base de Datos", "Fisica" };
        static string[] estudiantes = new string[max_estudiantes];
        static double[,] matrizNotas = new double[max_estudiantes, max_cursos];
        static int total_estudiantes = 0;
        static string rutaArchivo = "notas_cursos.txt";

        public static void RegistrarEstudiantes()
        {
            Console.Clear();
            Console.WriteLine("**********************");
            Console.WriteLine("Registro de estudiante");
            Console.WriteLine("**********************");

            if (total_estudiantes >= max_estudiantes)
            {
                Console.WriteLine("Capacidad maxima de estudiantes alcanzada.");
                Console.ReadKey();
                return;
            }
            string nombre = Validador.leerTexto("Ingresar nombres y apellidos: ").ToUpper();
            estudiantes[total_estudiantes] = nombre;
            Console.WriteLine("Ingresar notas para cada curso [0 - 20 ]");
            for (int i = 0; i < max_cursos; i++)
            {
                matrizNotas[total_estudiantes, i] = Validador.leerDecimal($"-{nombre_cursos[i]}: ");
            }
            total_estudiantes++;
            Console.WriteLine("Estudiante registrado..!!");
            Console.ReadKey();
        }

        public static void GuardarArchivo()
        {
            Console.Clear();
            if (total_estudiantes == 0)
            {
                Console.WriteLine("No hay estudiantes registrados para guardar...");
                Console.ReadKey();
                return;
            }

            using (StreamWriter escribir = new StreamWriter(rutaArchivo))
            {
                for (int i = 0; i < total_estudiantes; i++)
                {
                    string Linea = estudiantes[i];
                    for (int j = 0; j < max_cursos; j++)
                    {
                        Linea += $",{matrizNotas[i, j]}";
                    }
                    escribir.WriteLine(Linea);
                }
            }

            Console.WriteLine("Registros guardados exitosamente..!!!");
            Console.ReadKey();
        }

        public static void CargarRegistros()
        {
            Console.Clear();
            Console.WriteLine("Cargar registros desde el archivo");
            Console.WriteLine("Cargando....");

            if (!File.Exists(rutaArchivo))
            {
                Console.WriteLine("No se encontro ningun archivo para cargar los datos...");
                Console.ReadKey();
                return;
            }

            total_estudiantes = 0;
            using (StreamReader lector = new StreamReader(rutaArchivo))
            {
                string Linea;
                while ((Linea = lector.ReadLine()) != null && total_estudiantes < max_estudiantes)
                {
                    string[] partes = Linea.Split(',');
                    // partes[0] es el nombre, el resto son las 4 notas (total 5 elementos)
                    if (partes.Length >= max_cursos + 1)
                    {
                        estudiantes[total_estudiantes] = partes[0];

                        // CORREGIDO: Se usa max_cursos para evitar IndexOutOfRangeException
                        for (int i = 0; i < max_cursos; i++)
                        {
                            matrizNotas[total_estudiantes, i] = Convert.ToDouble(partes[i + 1]);
                        }
                        total_estudiantes++;
                    }
                }
            }
            Console.WriteLine($"Se cargaron {total_estudiantes} estudiantes con sus notas.");
            Console.ReadKey();
        }

        public static void mostrarRegistros()
        {
            Console.Clear();
            Console.WriteLine(new string('=', 100));
            Console.WriteLine("\t\tREPORTE GENERAL DE NOTAS POR CURSOS");
            Console.WriteLine(new string('=', 100));

            if (total_estudiantes == 0)
            {
                Console.WriteLine("No hay registros..");
                Console.ReadKey();
                return;
            }

            Console.Write($"{"ESTUDIANTE",-25}|");
            for (int i = 0; i < max_cursos; i++)
            {
                Console.Write($"{nombre_cursos[i],12}|");
            }
            Console.WriteLine("PROMEDIO / ESTADO");
            Console.WriteLine(new string('-', 100));

            for (int i = 0; i < total_estudiantes; i++)
            {
                Console.Write($"{estudiantes[i],-25}|");
                double suma = 0;
                for (int j = 0; j < max_cursos; j++)
                {
                    Console.Write($"{matrizNotas[i, j],12:F1}|");
                    suma += matrizNotas[i, j];
                }
                double promedio = suma / max_cursos;
                string estado = promedio >= 12 ? "APROBADO" : "DESAPROBADO";

                // CORREGIDO: Sintaxis correcta de interpolación para decimales y texto
                Console.WriteLine($"{promedio,8:F2} [ {estado} ]");
            }

            Console.WriteLine(new string('=', 100));
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            int opc = 0;

            while (opc != 5)
            {
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("BIENVENIDO AL SISTEMA DE REGISTRO");
                Console.WriteLine("=================================");

                Console.WriteLine("[1] Registrar Estudiante");
                Console.WriteLine("[2] Guardar en archivo");
                Console.WriteLine("[3] Cargar el archivo");
                Console.WriteLine("[4] Mostrar los registros");
                Console.WriteLine("[5] Salir");

                opc = Validador.leerEnteros("Seleccione la opcion [ 1 - 5 ]: ");

                switch (opc)
                {
                    case 1:
                        RegistrarEstudiantes();
                        break;
                    case 2:
                        GuardarArchivo();
                        break;
                    case 3:
                        CargarRegistros();
                        break;
                    case 4:
                        mostrarRegistros();
                        break;
                    case 5:
                        Console.WriteLine("Muchos exitos en su examen final!!!");
                        break;
                    default:
                        Console.WriteLine("Ingrese un numero valido: [ 1 - 5 ]");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}