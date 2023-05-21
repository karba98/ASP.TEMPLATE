using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    [ApiExplorerSettings(GroupName = "GROUPNAME")]

    public class CursosController : ControllerBase
    {
        readonly ILogger<EmpleoController> _logger;

        readonly Service_VYP_CURSOS serviceCursos;
        public CursosController(
        ILogger<EmpleoController> logger,
        Service_VYP_CURSOS service)
        {
            this._logger = logger;
            this.serviceCursos = service;
        }
        [HttpGet("[action]")]
        public async Task<Curso> GetLastCurso()
        {
            Curso curso = await serviceCursos.GetLastCurso();
            return curso;
        }
        [HttpGet("[action]")]
        public async Task<Curso> GetCurso(string fecha, string titulo)
        {
            Curso curso = await serviceCursos.GetCurso(fecha,titulo);
            return curso;
        }

        [HttpGet("[action]")]
        public async Task<List<Curso>> GetCursos(string categoria)
        {
            List<Curso> crusos = await serviceCursos.GetCursos(categoria);
            return crusos;
        }
        [HttpGet("[action]")]
        public async Task<List<Curso>> GetAllCursos()
        {
            List<Curso> cursos = await serviceCursos.GetCursos();
            var rnd = new Random();
            if (cursos != null)
            {
                cursos = cursos.OrderBy(item => rnd.Next()).ToList();
            }
            return cursos;
        }
    }

}
