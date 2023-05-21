using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PLANTIILLA.DEDOMENA.Models;
using PLANTIILLA.DEDOMENA.Repositories;

namespace PLANTIILLA.DEDOMENA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "GROUPNAME")]

    public class EmpresasController : ControllerBase
    {
        readonly ILogger<EmpresasController> _logger;
        readonly RepositoryEmpresas _repo;
        public EmpresasController(
        ILogger<EmpresasController> logger,
        RepositoryEmpresas rpeo)
        {
            this._logger = logger;
            this._repo = rpeo;
        }

        [HttpGet("[action]")]
        public async Task<List<Empresa>> GetEmpresas()
        {
            try
            {
                return _repo.GetEmpresas();

            }
            catch(Exception ex)
            {
                return null;
            }
        }

    }
}
