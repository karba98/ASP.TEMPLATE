using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PLANTIILLA.DEDOMENA.Models;
using PLANTIILLA.DEDOMENA.Repositories;
using PLANTIILLA.DEDOMENA.Services;

namespace PLANTIILLA.DEDOMENA.Clases
{
    public class EmpleoUpdater
    {
        readonly ILogger<EmpleoUpdater> _logger;
        readonly RepositoryEmpleoBR _repoEmpleoBR;
        readonly RepositoryEmpleo _repoEmpleo;

        readonly Service_INFOJOBS _service_infojobs;
        readonly Service_INDEED _service_indeed;
        readonly Service_ICARO _service_icaro;
        readonly Service_JOOBLE _Service_JOOBLE;


        readonly SwitchEmpleo swEmpleo;

        public EmpleoUpdater(
        ILogger<EmpleoUpdater> logger,
        RepositoryEmpleoBR repoEmpleoBR,
        RepositoryEmpleo _repoEmpleo,
        Service_INFOJOBS _service_infojobs,
        Service_INDEED _service_indeed,
        Service_ICARO service_icaro,
        Service_JOOBLE _Service_JOOBLE,
        SwitchEmpleo swEmpleo)
        {
            this._repoEmpleoBR = repoEmpleoBR;
            this._repoEmpleo = _repoEmpleo;
            this._logger = logger;
            this._service_indeed = _service_indeed;
            this._service_infojobs = _service_infojobs;
            this._service_icaro = service_icaro;
            this._Service_JOOBLE = _Service_JOOBLE;

            this.swEmpleo = swEmpleo;
        }


        public async Task<int> Update()
        {
            int timeout = 10000;
            List<EmpleoBR> nuevo_empleo = new List<EmpleoBR>();
            Refreshers refresh = new Refreshers(
                _repoEmpleoBR,
                _repoEmpleo,
                _service_infojobs, 
                _service_indeed, 
                _Service_JOOBLE,
                swEmpleo);
            try
            {
                var task = refresh.RefreshOfertasJooble();
                if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                {
                    nuevo_empleo.AddRange(task.Result);
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
            }
            try
            {
                var task = refresh.RefreshOfertasInfojobs();
                if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                {
                    nuevo_empleo.AddRange(task.Result);
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
            }
            try
            {
                var task2 = refresh.RefreshOfertasIndeed();
                if (await Task.WhenAny(task2, Task.Delay(timeout)) == task2)
                {
                    nuevo_empleo.AddRange(task2.Result);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
            }

            if (nuevo_empleo.Count > 0) { return nuevo_empleo.Count; }
            else return 0;
        }
        public async Task<bool> SendAndPublish()
        {
            try
            {
                List<EmpleoBR> empleoBRs = _repoEmpleoBR.Ofertas();
                if (empleoBRs.Count() > 0)
                {
                    string token_icaro = await _service_icaro.GetToken("SRVICARO", "##ICARO786*ÑHNJVFAÑ{");
                    if (token_icaro == null || token_icaro.Equals(""))
                    {
                        _logger.LogInformation("Error acceso ICARO");
                        return false;
                    }
                    foreach (EmpleoBR e in empleoBRs)
                    {
                        if(e.Provincia.Equals("NF") ||
                            e.Provincia.Equals("")||
                            e.FechaString.Equals("")
                            )
                        {
                            _logger.LogInformation("Datos incorrectos - " + e.Titulo + " - "+ e.Provincia+" - "+e.ProvinciaName+" [" + e.FechaString + "]");
                            continue;
                        }
                        else
                        {
                            Empleo emp = swEmpleo.EmpleoBRToEmpleo(e);
                            Empleo oferta = _repoEmpleo.InsertEmpleo(emp.Titulo, emp.Descripcion, emp.Salario, emp.Url, emp.Provincia, emp.Categoria, emp.Telefono, emp.Email);
                            _repoEmpleoBR.DeleteOferta(emp.Id);

                            if (token_icaro != null)
                            {
                                var fecha = oferta.FechaString;
                                fecha = System.Net.WebUtility.UrlEncode(fecha);

                                string publicado = await _service_icaro.CallApi<string>(
                                  "poster/PostOferta", token_icaro,
                                  "https://../Empleo/Oferta?provincia=" + oferta.Provincia + "&fecha=" + fecha,
                                  oferta.Provincia,
                                  oferta.Titulo)
                                    .ConfigureAwait(false);
                                _logger.LogInformation(publicado + " - " + oferta.Titulo + " [" + oferta.FechaString + "]");
                                await Task.Delay(15000);
                            }
                        }
                        
                    }
                    _logger.LogInformation("Ofertas procesadas y enviadas");
                }
                else _logger.LogInformation("No habia ofertas en borradores");
                return true;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.StackTrace + "\n" + ex.Message);
                return false;
            }
        }
        public void CleanOfertas()
        {
            _repoEmpleo.Organize();
        }
    }
}
