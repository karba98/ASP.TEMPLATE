using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PLANTIILLA.DEDOMENA.Clases;
using PLANTIILLA.DEDOMENA.Helpers;
using PLANTIILLA.DEDOMENA.Models;
using PLANTIILLA.DEDOMENA.Repositories;
using PLANTIILLA.DEDOMENA.Services;

namespace PLANTIILLA.DEDOMENA.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class EmpleoBRController : ControllerBase
    {
        readonly ILogger<EmpleoController> _logger;
        readonly RepositoryEmpleoBR _repoEmpleoBR;
        readonly RepositoryEmpleo _repoEmpleo;

        readonly Service_ARSEPRI _service_arsepri;
        readonly Service_INFOJOBS _service_infojobs;
        readonly Service_INDEED _service_indeed;

        readonly SwitchEmpleo swEmpleo;
        readonly EmpleoUpdater _emp_updater;

        public EmpleoBRController(
        ILogger<EmpleoController> logger,
        RepositoryEmpleoBR repoEmpleoBR,
        RepositoryEmpleo _repoEmpleo,
        Service_ARSEPRI _service_arsepri,
        Service_INFOJOBS _service_infojobs,
        Service_INDEED _service_indeed,
        SwitchEmpleo swEmpleo,
        EmpleoUpdater _emp_updater)
        {
            this._emp_updater = _emp_updater;
            this._repoEmpleoBR = repoEmpleoBR;
            this._repoEmpleo = _repoEmpleo;
            this._logger = logger;
            this._service_arsepri = _service_arsepri;
            this._service_indeed = _service_indeed;
            this._service_infojobs = _service_infojobs;
            this.swEmpleo = swEmpleo;
        }
       
        [Authorize]
        [HttpPost("[action]")]
        public async Task<bool> InsertEmpleo(EmpleoBR emp)
        {
            try
            {
                bool inserted = _repoEmpleoBR.InsertEmpleoBR(
                emp.Titulo, emp.Descripcion, emp.Salario, emp.Url, emp.Provincia, emp.Categoria, emp.Telefono, emp.Email,emp.Modo,emp.FechaPub);
                return inserted;
            }catch(Exception ex)
            {
                return false;
            }
            
        }
        [Authorize]
        [HttpPut]
        [Route("[action]")]
        public async Task<bool> EditEmpleo(EmpleoBR emp)
        {
            try
            {
                _repoEmpleoBR.EditEmpleoBR(emp.Id,emp.Descripcion,emp.Salario,emp.Url,emp.Categoria,emp.Telefono,emp.Email,emp.Titulo,emp.Provincia, emp.ProvinciaName);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        [Authorize]
        [HttpDelete]
        [Route("[action]")]
        public async Task<bool> DeleteEmpleo(int id)
        {
            try
            {
                EmpleoBR emp = _repoEmpleoBR.SearchEmpleoBR(id);

                _repoEmpleoBR.DeleteOferta(emp.Id);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<Empleo> Publicar(int id)
        {
            try
            {
                EmpleoBR emp = _repoEmpleoBR.SearchEmpleoBR(id);
                Empleo empleo_insertado = _repoEmpleo.InsertEmpleo(emp.Titulo, emp.Descripcion, emp.Salario, emp.Url, emp.Provincia, emp.Categoria, emp.Telefono, emp.Email,emp.FechaString);
                _repoEmpleoBR.DeleteOferta(id);
                return empleo_insertado;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<List<EmpleoBR>> Ofertas()
        {
            try
            {
                List<EmpleoBR> ofertas = _repoEmpleoBR.Ofertas();
                return ofertas;
            }
            catch (Exception ex)
            {
                return null;
            }


        }
       
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<List<EmpleoBR>> OfertasInfojobs()
        {
            try
            {
                List<EmpleoBR> ofertas = _repoEmpleoBR.OfertasInfojobs();
                return ofertas;
            }
            catch (Exception ex)
            {
                return null ;
            }
        }
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<List<EmpleoBR>> OfertasIndeed()
        {
            try
            {
                List<EmpleoBR> ofertas = _repoEmpleoBR.OfertasIndeed();
                return ofertas;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<int> GetCategoriaCount(string categoria)
        {
            try
            {
                int categoria_count = _repoEmpleoBR.GetCategoriaCount(categoria);
                return categoria_count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<List<EmpleoBR>> OfertasProvincia(string provincia)
        {
            try
            {
                List<EmpleoBR> ofertas = _repoEmpleoBR.OfertasProvincia(provincia);
                return ofertas;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<EmpleoBR> SearchEmpleo(string fechastring, string provincia)
        {
            try
            {
                EmpleoBR empleo = _repoEmpleoBR.SearchEmpleoBR(fechastring, provincia);
                return empleo;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<EmpleoBR> SearchEmpleoId(int id)
        {
            try
            {
                EmpleoBR empleo = _repoEmpleoBR.SearchEmpleoBR(id);
                return empleo;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<EmpleoBR> GetLastEmpleo()
        {
            try
            {
                EmpleoBR empleo = _repoEmpleoBR.GetLastEmpleoBR();
                return empleo;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<bool> Organize()
        {
            try
            {
                _repoEmpleoBR.Organize();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #region RSSs
        [Authorize]
        [HttpGet("[action]")]
        public async Task<int> RefreshRSSs()
        {
            int nuevas_ofertas = await _emp_updater.Update();
            return nuevas_ofertas;
        }
        
        #endregion
    }

}
