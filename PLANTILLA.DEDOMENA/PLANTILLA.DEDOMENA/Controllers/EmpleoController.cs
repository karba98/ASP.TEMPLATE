using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PLANTIILLA.DEDOMENA.Helpers;
using PLANTIILLA.DEDOMENA.Models;
using PLANTIILLA.DEDOMENA.Repositories;
using PLANTIILLA.DEDOMENA.Services;

namespace PLANTIILLA.DEDOMENA.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class EmpleoController : ControllerBase
    {
        readonly ILogger<EmpleoController> _logger;
        readonly RepositoryEmpleo _repoEmpleo;
        readonly RepositoryEmpleoBR _repoEmpleoBR;
        readonly RepositoryWords _repofilter;
        readonly Service_ICARO _service_icaro;
        readonly DataManager _datamanager;

        public EmpleoController(
        ILogger<EmpleoController> logger,
        RepositoryEmpleo repoEmpleo,
        RepositoryEmpleoBR repoEmpleoBR,
        RepositoryWords repofilter,
        Service_ICARO service_icaro, DataManager datamanager)
        {
            this._service_icaro = service_icaro;
            this._repoEmpleo = repoEmpleo;
            this._repoEmpleoBR = repoEmpleoBR;
            this._repofilter = repofilter;
            this._logger = logger;
            this._datamanager = datamanager;
        }
        #region PUBLIC
        [HttpGet]
        [Route("[action]")]
        public async Task<List<Empleo>> Ofertas()
        {
            try
            {
                List<Empleo> ofertas = _repoEmpleo.Ofertas();
                return ofertas;
            }
            catch (Exception ex)
            {
                return null;
            }


        }
        [HttpGet]
        [Route("[action]")]
        public async Task<List<Empleo>> OfertasTop20()
        {
            try
            {
                List<Empleo> ofertas = _repoEmpleo.OfertasTop20();
                return ofertas;
            }
            catch (Exception ex)
            {
                return null;
            }


        }
        [HttpGet]
        [Route("[action]")]
        public async Task<List<Empleo>> OfertasInfojobs()
        {
            try
            {
                List<Empleo> ofertas = _repoEmpleo.OfertasInfojobs();
                return ofertas;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<List<Empleo>> OfertasIndeed()
        {
            try
            {
                List<Empleo> ofertas = _repoEmpleo.OfertasIndeed();
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
                int categoria_count = _repoEmpleo.GetCategoriaCount(categoria);
                return categoria_count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<List<Empleo>> OfertasProvincia(string provincia)
        {
            try
            {
                List<Empleo> ofertas = _repoEmpleo.OfertasProvincia(provincia);
                return ofertas;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [HttpGet]
        [Route("[action]")]
        public async Task<List<Empleo>> Last30OfertasProvincia(string provincia)
        {
            try
            {
                List<Empleo> ofertas = _repoEmpleo.Last3OfertasProvincia(provincia);
                return ofertas;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [HttpGet]
        [Route("[action]")]
        public async Task<List<Empleo>> Last30Ofertas()
        {
            try
            {
                List<Empleo> ofertas = _repoEmpleo.GetLast3Ofertas();
                return ofertas;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [HttpGet]
        [Route("[action]")]
        public async Task<List<Empleo>> ShowLast3Empleos()
        {
            try
            {
                List<Empleo> ofertas = _repoEmpleo.ShowLast3Empleos();
                return ofertas;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [HttpGet]
        [Route("[action]")]
        public async Task<Empleo> SearchEmpleo(string fechastring, string provincia)
        {
            try
            {
                Empleo empleo = _repoEmpleo.SearchEmpleo(fechastring, provincia);
                return empleo;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [HttpGet]
        [Route("[action]")]
        public async Task<Empleo> SearchEmpleoId(int id)
        {
            try
            {
                Empleo empleo = _repoEmpleo.SearchEmpleo(id);
                return empleo;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Empleo> GetLastEmpleo()
        {
            try
            {
                Empleo empleo = _repoEmpleo.GetLastEmpleo();
                return empleo;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        
        #endregion
        #region ADMINI PANEL
        [Authorize]
        [HttpPost("[action]")]
        public async Task<bool> InsertEmpleo(Empleo emp)
        {
            try
            {
                Empleo inserted = _repoEmpleo.InsertEmpleo(
                emp.Titulo, emp.Descripcion, emp.Salario, emp.Url, emp.Provincia, emp.Categoria, emp.Telefono, emp.Email);
                if (inserted != null) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [Authorize]
        [HttpPut("[action]")]
        public async Task<bool> EditEmpleo(Empleo emp)
        {
            try
            {
                _repoEmpleo.EditEmpleo(emp.Id, emp.Descripcion, emp.Salario, emp.Url, emp.Categoria, emp.Telefono, emp.Email, emp.Titulo, emp.Provincia, emp.ProvinciaName);
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
                Empleo emp = _repoEmpleo.SearchEmpleo(id);
                _repoEmpleo.DeleteOferta(emp.Id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [Authorize]
        [HttpGet("[action]")]
        public async Task<bool> Organize()
        {
            try
            {
                _repoEmpleo.Organize();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        [Authorize]
        [HttpPost("[action]")]
        public async Task<bool> PostOferta(OfertaModelPub oferta)
        {
            //recibimos el token ICARO a traves de los headers
            string token = Request.Headers.Where(x => x.Key.Equals("token_icaro")).FirstOrDefault().Value;
            //LLamado desde JavaScript Empleo.ManageEmpleo.ICARO
            if (oferta.url == null || oferta.provincia == null || oferta.titulo == null)
            {
                return false;
            }
            oferta.provincia = _datamanager.GetProvinciaImg(oferta.provincia);
            if (token != null)
            {
                bool activetoken = await _service_icaro.CheckAccess(token); //TOKEN ACTIVO EN SERVIDOR
                if (activetoken == true)
                {
                    string publicado = await _service_icaro.CallApi<string>("poster/PostOferta", token, oferta.url, oferta.provincia, oferta.titulo).ConfigureAwait(false);
                    return publicado == "Publicado" ? true : false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<List<Dictionary<string, int>>> GetAllOfertas()
        {
            try
            {
                Dictionary<string, int> dataBorradores = new Dictionary<string, int>();
                Dictionary<string, int> dataPublicas = new Dictionary<string, int>();
                foreach (string categoria in _datamanager.GetCategorias())
                {
                    dataBorradores.Add(categoria, _repoEmpleoBR.GetCategoriaCount(categoria));
                    dataPublicas.Add(categoria, _repoEmpleo.GetCategoriaCount(categoria));

                }
                List<Dictionary<string, int>> data = new List<Dictionary<string, int>>();
                data.Add(dataBorradores);
                data.Add(dataPublicas);

                return data;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<List<string>> InsertWordFilter(string word)
        {
            try
            {
                if(!word.Equals(" ") && !word.IsNullOrEmpty())_repofilter.Insert(word);
                return _repofilter.GetWords();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
            }
            return _repofilter.GetWords();
        }
        [Authorize]
        [HttpDelete]
        [Route("[action]")]
        public async Task<List<string>> DeleteWordFilter(string word)
        {
            try
            {
                if(!word.Equals(" ") && !word.IsNullOrEmpty())_repofilter.Delete(word);
                return _repofilter.GetWords();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
            }
            return _repofilter.GetWords();
        }
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<List<string>> GetWordsFilter()
        {
            try
            {
                return _repofilter.GetWords();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }
        #endregion
    }

}
