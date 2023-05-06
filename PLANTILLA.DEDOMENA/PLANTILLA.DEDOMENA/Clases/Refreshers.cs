using System.Collections.Generic;
using System.Threading.Tasks;
using PLANTIILLA.DEDOMENA.Models;
using PLANTIILLA.DEDOMENA.Repositories;
using PLANTIILLA.DEDOMENA.Services;

namespace PLANTIILLA.DEDOMENA.Clases
{
    public class Refreshers
    {
        readonly Service_ARSEPRI _service_arsepri;
        readonly Service_INFOJOBS _service_infojobs;
        readonly Service_INDEED _service_indeed;

        readonly RepositoryEmpleoBR _repoEmpleoBR;
        readonly RepositoryEmpleo _repoEmpleo;
        readonly Service_JOOBLE _Service_JOOBLE;
        readonly SwitchEmpleo swEmpleo;

        public Refreshers(
            RepositoryEmpleoBR repoEmpleoBR,
            RepositoryEmpleo _repoEmpleo,

            Service_ARSEPRI _service_arsepri,
            Service_INFOJOBS _service_infojobs,
            Service_INDEED _service_indeed,
            Service_JOOBLE _Service_JOOBLE,
            SwitchEmpleo swEmpleo)
        {
            this._repoEmpleoBR = repoEmpleoBR;
            this._repoEmpleo = _repoEmpleo;

            this._service_arsepri = _service_arsepri;
            this._service_indeed = _service_indeed;
            this._service_infojobs = _service_infojobs;
            this._Service_JOOBLE = _Service_JOOBLE;
            this.swEmpleo = swEmpleo;

        }
        public async Task<List<EmpleoBR>> RefreshOfertasJooble(){
            List<EmpleoBR> empleo_jooble = await _Service_JOOBLE.GetSecurityGuardJobs();
            if(empleo_jooble.Count > 0){
                empleo_jooble = _repoEmpleo.CompareAllWithDB(empleo_jooble);
                empleo_jooble = _repoEmpleoBR.CompareAllWithDB(empleo_jooble);
                //Metemos las nuevas ofertas en borradores
                foreach (EmpleoBR e in empleo_jooble)
                {
                    bool inserted = _repoEmpleoBR.InsertEmpleoBR(
                        e.Titulo,
                        e.Descripcion,
                        e.Salario,
                        e.Url,
                        e.Provincia,
                        e.Categoria,
                        e.Telefono,
                        e.Email, "B", e.FechaPub);
                    if (inserted == false)
                    {
                        empleo_jooble.Remove(e);
                    }
                }
                return empleo_jooble;
            }
            else
            {
                return new List<EmpleoBR>();
            }
        }
        public async Task<List<EmpleoBR>> RefreshOfertasInfojobs()
        {
            List<EmpleoBR> empleo_infojobs = await _service_infojobs.GetFeed();

            //actualiza borradores con las nmuevas de infojobs
            if (empleo_infojobs.Count > 0)
            {
                //devuelve una lista de empleos que no estan en la base de datos de borradores ni publicas
                empleo_infojobs = _repoEmpleo.CompareInfojobsWithDB(empleo_infojobs);
                empleo_infojobs = _repoEmpleoBR.CompareInfojobsWithDB(empleo_infojobs);
                if (empleo_infojobs.Count > 0)
                {
                    //Metemos las nuevas ofertas en borradores
                    foreach (EmpleoBR e in empleo_infojobs)
                    {
                        bool inserted = _repoEmpleoBR.InsertEmpleoBR(
                            e.Titulo,
                            e.Descripcion,
                            e.Salario,
                            e.Url,
                            e.Provincia,
                            e.Categoria,
                            e.Telefono,
                            e.Email, "B", e.FechaPub);
                        if (inserted == false)
                        {
                            empleo_infojobs.Remove(e);
                        }
                    }
                }
                return empleo_infojobs;
            }
            else
            {
                return new List<EmpleoBR>();
            }
        }
        public async Task<List<EmpleoBR>> RefreshOfertasIndeed()
        {
            List<EmpleoBR> empleo_indeed = await _service_indeed.GetFeed();

            //actualiza borradores con las nmuevas de indeed
            if (empleo_indeed.Count > 0)
            {
                //devuelve una lista de empleos que no estan en la base de datos de borradores ni publicas
                empleo_indeed = _repoEmpleo.CompareIndeedWithDB(empleo_indeed);
                empleo_indeed = _repoEmpleoBR.CompareIndeedWithDB(empleo_indeed);
                if (empleo_indeed.Count > 0)
                {
                    //Metemos las nuevas ofertas en borradores
                    foreach (EmpleoBR e in empleo_indeed)
                    {
                        bool inserted = _repoEmpleoBR.InsertEmpleoBR(
                            e.Titulo,
                            e.Descripcion,
                            e.Salario,
                            e.Url,
                            e.Provincia,
                            e.Categoria,
                            e.Telefono,
                            e.Email, "B", e.FechaPub);
                        if (inserted == false)
                        {
                            empleo_indeed.Remove(e);
                        }
                    }
                }
                return empleo_indeed;
            }
            else
            {
                return new List<EmpleoBR>();
            }
        }
        public async Task<List<EmpleoBR>> RefreshOfertasArsepri()
        {
            List<Empleo> empleo_arsepri_rss = await _service_arsepri.GetFeed(null);
            List<EmpleoBR> empleo_arsepri = new List<EmpleoBR>();
            foreach (Empleo e in empleo_arsepri_rss)
            {
                empleo_arsepri.Add(swEmpleo.EmpleoToEmpleoBR(e));
            }

            //actualiza borradores con las nmuevas de indeed
            if (empleo_arsepri.Count > 0)
            {
                //devuelve una lista de empleos que no estan en la base de datos de borradores ni publicas
                empleo_arsepri = _repoEmpleo.CompareAllWithDB(empleo_arsepri);
                empleo_arsepri = _repoEmpleoBR.CompareAllWithDB(empleo_arsepri);
                if (empleo_arsepri.Count > 0)
                {
                    //Metemos las nuevas ofertas en borradores
                    foreach (EmpleoBR e in empleo_arsepri)
                    {
                        bool inserted = _repoEmpleoBR.InsertEmpleoBR(
                            e.Titulo,
                            e.Descripcion,
                            e.Salario,
                            e.Url,
                            e.Provincia,
                            e.Categoria,
                            e.Telefono,
                            e.Email, "B", e.FechaPub);
                        if (inserted == false)
                        {
                            empleo_arsepri.Remove(e);
                        }
                    }
                }
                return empleo_arsepri;
            }
            else
            {
                return new List<EmpleoBR>();
            }
        }
    }
}
