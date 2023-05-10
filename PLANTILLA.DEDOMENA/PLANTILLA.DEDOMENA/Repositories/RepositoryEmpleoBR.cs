using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PLANTIILLA.DEDOMENA.Data;
using PLANTIILLA.DEDOMENA.Helpers;
using PLANTIILLA.DEDOMENA.Models;

namespace PLANTIILLA.DEDOMENA.Repositories
{
    public class RepositoryEmpleoBR
    {
        DataContext _context;
        DataManager _provinciasManager;

        RepositoryWords _filtro;

        public RepositoryEmpleoBR(DataContext context, DataManager _provinciasManager, RepositoryWords filter)
        {
            this._provinciasManager = _provinciasManager;
            this._context = context;
            _filtro = filter;
        }
        internal bool IsInBlackList(string frase)
        {
            //se transforma frase y lista en lowercase para facilitar busqueda
            frase = frase.ToLower();
            List<string> palabras = _filtro.GetWords();
            palabras = palabras.Select(word => word.ToLower()).ToList();

            return palabras.Any(word => frase.Contains(word));
        }
        public bool InsertEmpleoBR(String titulo, String Descripcion, int Salario, String url, String provincia, String Categoria, String telefono, String email, string? modo, DateTime? FechaPub)
        {
            if (!IsInBlackList(titulo) && !IsInBlackList(Descripcion))
            {
                String format = "dd/MM/yyyy HH:mm:ss tt";
                DateTime fecha = DateTime.UtcNow.AddHours(1);

                String fechaString = fecha.ToString(format, CultureInfo.InvariantCulture);
                EmpleoBR EmpleoBR = new EmpleoBR()
                {
                    Id = GetMaxId(),
                    Titulo = titulo,
                    Descripcion = Descripcion,
                    Salario = Salario,
                    Url = url,
                    FechaPub = fecha,
                    Provincia = provincia,
                    Categoria = Categoria,
                    Telefono = telefono,
                    Email = email,
                    FechaString = fechaString,
                    Modo = modo
                };
                try
                {
                    _context.EmpleosBR.Add(EmpleoBR);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                    _context.EmpleosBR.Remove(EmpleoBR);
                    Console.WriteLine(ex.StackTrace);
                }
            }
            else
            {
                return false;
            }
        }


        internal void EditEmpleoBR(int id, string Descripcion, int Salario, string url, string Categoria, string telefono, string email, string Titulo, string provincia, string provinciaName)
        {

            EmpleoBR EmpleoBR = SearchEmpleoBR(id);

            EmpleoBR.Descripcion = Descripcion;
            EmpleoBR.Salario = Salario;
            EmpleoBR.Url = url;
            EmpleoBR.Categoria = Categoria;
            EmpleoBR.Telefono = telefono;
            EmpleoBR.Email = email;
            EmpleoBR.Titulo = Titulo;
            EmpleoBR.Provincia = provincia;
            EmpleoBR.ProvinciaName = provinciaName;


            _context.SaveChanges();
        }


        public List<EmpleoBR> Ofertas()
        {
            return _context.EmpleosBR.OrderByDescending(x => x.Id).ToList();
        }
        public List<EmpleoBR> OfertasInfojobs()
        {
            return _context.EmpleosBR.Where(x => x.Categoria.Equals("infojobs")).OrderByDescending(x => x.FechaPub).ToList();
        }
        public List<EmpleoBR> OfertasIndeed()
        {
            return _context.EmpleosBR.Where(x => x.Categoria.Equals("indeed")).OrderByDescending(x => x.FechaPub).ToList();
        }
        public List<EmpleoBR> CompareInfojobsWithDB(List<EmpleoBR> rssList)
        {
            List<EmpleoBR> databaseList = OfertasInfojobs();
            List<EmpleoBR> returnedList = new List<EmpleoBR>();
            foreach (EmpleoBR emp in rssList)
            {
                string provincia = emp.Provincia;
                string titulo = emp.Titulo;
                bool containsEmp = false;
                foreach (EmpleoBR e in databaseList)
                {
                    //if (e.Provincia == provincia && e.Titulo == titulo) 

                    try
                    {
                        if (e.Url.Equals(emp.Url))
                        {
                            containsEmp = true;
                            break;
                        }
                    }
                    catch (Exception) { }
                }
                if (!containsEmp)
                {
                    returnedList.Add(emp);
                }
            }
            return returnedList;
        }
        public List<EmpleoBR> CompareAllWithDB(List<EmpleoBR> rssList)
        {
            List<EmpleoBR> databaseList = Ofertas();
            List<EmpleoBR> returnedList = new List<EmpleoBR>();
            foreach (EmpleoBR emp in rssList)
            {
                string provincia = emp.Provincia;
                string titulo = emp.Titulo;
                bool containsEmp = false;
                foreach (EmpleoBR e in databaseList)
                {
                    //if (e.Provincia == provincia && e.Titulo == titulo) 

                    try
                    {
                        if (e.Url != null && emp.Url != null &&
                            e.Url != "" && emp.Url != "" && e.Url.Equals(emp.Url))
                        {

                            containsEmp = true;
                            break;

                        }
                        if (e.Titulo != null && emp.Titulo != null && e.Titulo != ""
                            && emp.Titulo != "" && e.Titulo.Equals(emp.Titulo))
                        {
                            containsEmp = true;
                            break;
                        }
                    }
                    catch (Exception) { }
                }
                if (!containsEmp)
                {
                    returnedList.Add(emp);
                }
            }
            return returnedList;
        }
        public List<EmpleoBR> CompareIndeedWithDB(List<EmpleoBR> rssList)
        {
            try
            {
                List<EmpleoBR> databaseList = OfertasIndeed();
                List<EmpleoBR> returnedList = new List<EmpleoBR>();
                foreach (EmpleoBR emp in rssList)
                {
                    string provincia = emp.Provincia;
                    string titulo = emp.Titulo;
                    bool containsEmp = false;
                    foreach (EmpleoBR e in databaseList)
                    {

                        try
                        {
                            string url_received = emp.Url.Substring(0, emp.Url.LastIndexOf("&jk="));
                            string url_db = "";
                            try
                            {
                                url_db = e.Url.Substring(0, e.Url.LastIndexOf("&jk="));
                                if (url_received.Equals(url_db))
                                {
                                    containsEmp = true;
                                    break;
                                }

                            }
                            catch (Exception)
                            {
                            }
                        }
                        catch (Exception) { }
                    }
                    if (!containsEmp)
                    {
                        returnedList.Add(emp);
                    }
                }
                return returnedList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int GetCategoriaCount(string categoria)
        {
            return _context.EmpleosBR.Where(x => x.Categoria.Equals(categoria)).Count();
        }

        public void DeleteOferta(int id)
        {
            EmpleoBR e = _context.EmpleosBR.FirstOrDefault(x => x.Id == id);
            _context.EmpleosBR.Remove(e);
            _context.SaveChanges();
        }
        public List<EmpleoBR> OfertasProvincia(String provincia)
        {
            return _context.EmpleosBR.Where(x => x.Provincia == provincia).OrderByDescending(x => x.Id).ToList();
        }
        public List<EmpleoBR> GetLast3Ofertas()
        {
            return _context.EmpleosBR.OrderByDescending(x => x.Id).Skip(1).Take(3).ToList();
        }
        public List<EmpleoBR> ShowLast3EmpleoBRs()
        {
            return _context.EmpleosBR.OrderByDescending(x => x.Id).Take(3).ToList();
        }

        private int GetMaxId()
        {
            if (_context.EmpleosBR.Count() > 0)
            {
                int id = _context.EmpleosBR.Max(x => x.Id);
                return id + 1;
            }
            else return 1;
        }

        internal EmpleoBR SearchEmpleoBR(int id)
        {
            return _context.EmpleosBR.FirstOrDefault(x => x.Id == id);
        }
        //internal EmpleoBR SearchEmpleoBR(string titulo)
        //{
        //    return _context.EmpleoBRs.FirstOrDefault(x=> x.Titulo == titulo);
        //}
        internal EmpleoBR SearchEmpleoBR(string fechastring, string provincia)
        {
            string p = provincia;
            p = _provinciasManager.GetProvinciaRoute(provincia);
            if (p == null)
            {
                provincia = _provinciasManager.GetProvinciaName(provincia);
            }
            //return _context.EmpleoBRs.FirstOrDefault(x=> x.FechaString == fechastring);
            EmpleoBR e = _context.EmpleosBR.FirstOrDefault(x => x.FechaString == fechastring);
            if (e == null) { return null; }
            else if (e.Provincia == provincia)
            {
                return e;
            }
            else
            {
                return null;
            }
        }
        public EmpleoBR GetLastEmpleoBR()
        {
            try
            {
                EmpleoBR e = _context.EmpleosBR.OrderByDescending(x => x.Id).FirstOrDefault();
                if (e == null)
                {
                    return null;
                }
                else { return e; }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public void Organize()
        {
            List<EmpleoBR> emp = _context.EmpleosBR.Where(x => x.FechaPub < DateTime.Now.AddDays(-14)).ToList();
            if (emp != null)
            {
                _context.EmpleosBR.RemoveRange(emp);
                _context.SaveChanges();
            }

        }
    }
}
