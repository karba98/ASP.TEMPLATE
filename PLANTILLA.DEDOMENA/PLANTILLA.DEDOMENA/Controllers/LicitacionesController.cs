using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PLANTIILLA.DEDOMENA.Models;
using PLANTIILLA.DEDOMENA.Repositories;
using PLANTIILLA.DEDOMENA.Services;

namespace PLANTIILLA.DEDOMENA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LicitacionesController : ControllerBase
    {
        readonly ILogger<EmpresasController> _logger;
        readonly Service_ARSEPRI_LICIT _serviceArsepriRSS;

        public LicitacionesController(
        ILogger<EmpresasController> logger,
        Service_ARSEPRI_LICIT serviceArsepriRSS)
        {
            this._logger = logger;
            this._serviceArsepriRSS = serviceArsepriRSS;
        }

        [HttpGet("[action]")]
        public async Task<List<Licitacion>> GetLicitaciones(string provincia)
        {
            List<Licitacion> licitaciones = await _serviceArsepriRSS.GetLicitaciones(provincia);
            return licitaciones;
        }
        [HttpGet("[action]")]
        public async Task<Licitacion> GetLicitacion(string provincia,string title)
        {
            Licitacion licitacion = await _serviceArsepriRSS.GetLicitacion(provincia,title);
            return licitacion;
        }

    }
}
