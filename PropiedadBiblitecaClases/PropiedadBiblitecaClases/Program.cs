using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropiedadBiblitecaClases
{
    class Program
    {
        static void Main(string[] args)
        {
            Persona persona = new Persona();  // Crear una nueva persona
            persona.Nombre = "Juan";         // Usar la propiedad para asignar el nombre

            Console.WriteLine($"Nombre de la persona: {persona.Nombre}");
        }
    }
}
