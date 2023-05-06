using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Quartz.Impl.AdoJobStore.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http.Filters;
using PLANTIILLA.DEDOMENA.Clases;
using PLANTIILLA.DEDOMENA.Data;
using PLANTIILLA.DEDOMENA.Helpers;
using PLANTIILLA.DEDOMENA.Models;

namespace PLANTIILLA.DEDOMENA.Repositories
{
    public class RepositoryEmpleo
    {
        DataContext _context;
        DataManager _provinciasManager;
        SwitchEmpleo swEmpleo;

        RepositoryWords _filtro;

        public RepositoryEmpleo(DataContext context, DataManager _provinciasManager, SwitchEmpleo swEmpleo, RepositoryWords filtro)
        {
            this.swEmpleo = swEmpleo;
            this._provinciasManager = _provinciasManager;
            _context = context;
            _filtro = filtro;
        }
        internal bool IsInBlackList(string frase)
        {
            //se transforma frase y lista en lowercase para facilitar busqueda
            frase = frase.ToLower();
            List<string> palabras = _filtro.GetWords();
            palabras = palabras.Select(word => word.ToLower()).ToList();

            return palabras.Any(word => frase.Contains(word));
        }
        public Empleo InsertEmpleo(string titulo, string Descripcion, int Salario, string url, string provincia, string Categoria, string telefono, string email)
        {
            if(IsInBlackList(titulo) || IsInBlackList(Descripcion))
            {
                return null;
            }
            else
            {
                string format = "dd/MM/yyyy HH:mm:ss tt";
                DateTime fecha = DateTime.UtcNow;
                //String format = "dddd,dd MMMM, yyyy";
                //DateTime fecha = DateTime.Now;
                string fechaString = fecha.ToString(format);
                Empleo empleo = new Empleo()
                {
                    Id = GetNewMaxId(),
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
                    Publicado = 0

                };
                try
                {
                    _context.Empleos.Add(empleo);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _context.Empleos.Remove(empleo);
                }


                int idmax = GetNewMaxId() - 1;
                Empleo inserted = SearchEmpleo(idmax);
                return inserted;
            }
        }
        public Empleo InsertEmpleo(string titulo,
            string Descripcion, 
            int Salario, 
            string url, 
            string provincia, 
            string Categoria, 
            string telefono, 
            string email,
            string fechaString)
        {
            string format = "dd/MM/yyyy HH:mm:ss tt";
            DateTime fecha = DateTime.UtcNow;

            CultureInfo provider = CultureInfo.InvariantCulture;

            Empleo empleo = new Empleo()
            {
                Id = GetNewMaxId(),
                Titulo = titulo,
                Descripcion = Descripcion,
                Salario = Salario,
                Url = url,
                FechaPub = DateTime.ParseExact(fechaString,format, provider),
                Provincia = provincia,
                Categoria = Categoria,
                Telefono = telefono,
                Email = email,
                FechaString = fecha.ToString(format),
                //FechaString = fechaString,
                Publicado = 0

            };
            try
            {
                _context.Empleos.Add(empleo);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _context.Empleos.Remove(empleo);
            }


            int idmax = GetNewMaxId() - 1;
            Empleo inserted = SearchEmpleo(idmax);
            return inserted;
        }

        internal void EditEmpleo(int id, string Descripcion, int Salario, string url, string Categoria, string telefono, string email, string Titulo, string provincia, string provinciaName)
        {

            Empleo empleo = SearchEmpleo(id);

            empleo.Descripcion = Descripcion;
            empleo.Salario = Salario;
            empleo.Url = url;
            empleo.Categoria = Categoria;
            empleo.Telefono = telefono;
            empleo.Email = email;
            empleo.Titulo = Titulo;
            empleo.Provincia = provincia;
            empleo.ProvinciaName = provinciaName;


            _context.SaveChanges();
        }
        public List<EmpleoBR> CompareInfojobsWithDB(List<EmpleoBR> rssList)
        {
            List<Empleo> databaseList = OfertasInfojobs();
            //transformo empleo publicado en br para facilitar la comparacion
            List<EmpleoBR> databaseBRList = new List<EmpleoBR>();
            foreach (Empleo e in databaseList)
            {
                EmpleoBR emp = swEmpleo.EmpleoToEmpleoBR(e);
                databaseBRList.Add(emp);
            }
            List<EmpleoBR> returnedList = new List<EmpleoBR>();
            foreach (EmpleoBR emp in rssList)
            {
                string titulo = emp.Titulo;
                string prov = emp.Provincia;
                bool containsEmp = false;
                foreach (EmpleoBR e in databaseBRList)
                {

                    if (e.Url != null && emp.Url != null && e.Url.Equals(emp.Url))
                    {
                        containsEmp = true;
                        break;
                    }

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
            List<Empleo> databaseList = Ofertas();
            //transformo empleo publicado en br para facilitar la comparacion
            List<EmpleoBR> databaseBRList = new List<EmpleoBR>();
            foreach (Empleo e in databaseList)
            {
                EmpleoBR emp = swEmpleo.EmpleoToEmpleoBR(e);
                databaseBRList.Add(emp);
            }
            List<EmpleoBR> returnedList = new List<EmpleoBR>();
            foreach (EmpleoBR emp in rssList)
            {
                string titulo = emp.Titulo;
                string prov = emp.Provincia;
                bool containsEmp = false;
                foreach (EmpleoBR e in databaseBRList)
                {

                    try
                    {
                        if (e.Url != null && emp.Url != null &&
                            e.Url != "" && emp.Url != "" && e.Url.Equals(emp.Url))
                        {

                            containsEmp = true;
                            break;

                        }
                        if (e.Titulo != null && emp.Titulo != null && e.Titulo.Equals(emp.Titulo))
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
                List<Empleo> databaseList = OfertasIndeed();
                //transformo empleo publicado en br para facilitar la comparacion
                List<EmpleoBR> databaseBRList = new List<EmpleoBR>();
                foreach (Empleo e in databaseList)
                {
                    EmpleoBR emp = swEmpleo.EmpleoToEmpleoBR(e);
                    databaseBRList.Add(emp);
                }
                List<EmpleoBR> returnedList = new List<EmpleoBR>();
                foreach (EmpleoBR emp in rssList)
                {
                    string titulo = emp.Titulo;
                    string prov = emp.Provincia;
                    bool containsEmp = false;
                    foreach (EmpleoBR e in databaseBRList)
                    {
                        string url_received = emp.Url.Substring(0, emp.Url.LastIndexOf("&jk="));
                        string url_db = "";
                        try
                        {
                            url_db = e.Url.Substring(0, e.Url.LastIndexOf("&jk="));
                            if (e.Url != null && emp.Url != null && url_received.Equals(url_db))
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
            catch (Exception ex)
            {
                return new List<EmpleoBR>();
            }

        }

        public List<Empleo> Ofertas()
        {
            return _context.Empleos.OrderByDescending(x => x.Id).ToList();
        }
        public List<Empleo> OfertasTop20()
        {
            return _context.Empleos.OrderByDescending(x => x.Id).Take(20).ToList();
        }
        public List<Empleo> OfertasInfojobs()
        {
            return _context.Empleos.Where(x => x.Categoria.Equals("infojobs")).OrderByDescending(x => x.Id).ToList();
        }
        public List<Empleo> OfertasIndeed()
        {
            return _context.Empleos.Where(x => x.Categoria.Equals("indeed")).OrderByDescending(x => x.Id).ToList();
        }
        public int GetCategoriaCount(string categoria)
        {
            return _context.Empleos.Where(x => x.Categoria.Equals(categoria)).Count();
        }
        public void DeleteOferta(int id)
        {
            Empleo e = _context.Empleos.FirstOrDefault(x => x.Id == id);
            _context.Empleos.Remove(e);
            _context.SaveChanges();
        }
        public List<Empleo> OfertasProvincia(string provincia)
        {
            return _context.Empleos.Where(x => x.Provincia == provincia).OrderByDescending(x => x.Id).ToList();
        }
        public List<Empleo> Last3OfertasProvincia(string provincia)
        {
            return _context.Empleos.Where(x => x.Provincia == provincia).OrderByDescending(x => x.Id).Take(3).ToList();
        }
        public List<Empleo> GetLast3Ofertas()
        {
            return _context.Empleos.OrderByDescending(x => x.Id).Skip(1).Take(3).ToList();
        }
        public List<Empleo> ShowLast3Empleos()
        {
            return _context.Empleos.OrderByDescending(x => x.Id).Take(3).ToList();
        }

        private int GetNewMaxId()
        {
            if (_context.Empleos.Count() > 0)
            {
                int id = _context.Empleos.Max(x => x.Id);
                return id + 1;
            }
            else return 1;
        }

        internal Empleo SearchEmpleo(int id)
        {
            return _context.Empleos.FirstOrDefault(x => x.Id == id);
        }
        //internal Empleo SearchEmpleo(string titulo)
        //{
        //    return _context.Empleos.FirstOrDefault(x=> x.Titulo == titulo);
        //}
        internal Empleo SearchEmpleo(string fechastring, string provincia)
        {
            string p = provincia;
            p = _provinciasManager.GetProvinciaRoute(provincia);
            if (p == null)
            {
                provincia = _provinciasManager.GetProvinciaName(provincia);
            }
            //return _context.Empleos.FirstOrDefault(x=> x.FechaString == fechastring);
            Empleo e = _context.Empleos.FirstOrDefault(x => x.FechaString.Equals(fechastring) 
                && x.Provincia.Equals(provincia));
            if (e == null) { return null; }
            else return e;
        }
        public Empleo GetLastEmpleo()
        {
            try
            {
                Empleo e = _context.Empleos.OrderByDescending(x => x.Id).FirstOrDefault();
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
            List<Empleo> emp = _context.Empleos.Where(x => x.FechaPub < DateTime.Now.AddDays(-14)).ToList();
            if (emp != null)
            {
                _context.Empleos.RemoveRange(emp);
                _context.SaveChanges();
            }

        }

    }
}
