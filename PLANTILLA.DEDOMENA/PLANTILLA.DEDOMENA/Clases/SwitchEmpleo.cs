using PLANTIILLA.DEDOMENA.Models;

namespace PLANTIILLA.DEDOMENA.Clases
{
    public class SwitchEmpleo
    {
        public EmpleoBR EmpleoToEmpleoBR(Empleo emp)
        {
            EmpleoBR empleo = new EmpleoBR()
            {
                Categoria = emp.Categoria,
                Descripcion = emp.Descripcion,
                Email = emp.Email,
                FechaPub = emp.FechaPub,
                Id = emp.Id,
                Provincia = emp.Provincia,
                Salario = emp.Salario,
                FechaString = emp.FechaString,
                ProvinciaName = emp.ProvinciaName,
                Telefono = emp.Telefono,
                Titulo = emp.Titulo,
                Url = emp.Url
            };
            return empleo;
        }
        

        public Empleo EmpleoBRToEmpleo(EmpleoBR emp)
        {
            Empleo empleo = new Empleo()
            {
                Categoria = emp.Categoria,
                Descripcion = emp.Descripcion,
                Email = emp.Email,
                FechaPub = emp.FechaPub,
                Id = emp.Id,
                Provincia = emp.Provincia,
                Salario = emp.Salario,
                FechaString = emp.FechaString,
                ProvinciaName = emp.ProvinciaName,
                Telefono = emp.Telefono,
                Titulo = emp.Titulo,
                Url = emp.Url
            };
            return empleo;
        }
        
    }
}
