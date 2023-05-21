
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    [ApiExplorerSettings(GroupName = "GROUPNAME")]
    public class VYPController : ControllerBase
    {
        readonly ILogger<EmpresasController> _logger;
        readonly Service_VYPW_RSS _servicevypwRSS;
        readonly Service_VYP_NOTICIAS service_noticias;
        readonly Service_VYP_CURSOS service_cursos;

        readonly RepositoryEmpleo _repoEmpleo;

        public VYPController(
        ILogger<EmpresasController> logger,
        Service_VYPW_RSS servicevypwRSS,
        Service_VYP_NOTICIAS service_noticias,
        Service_VYP_CURSOS service_cursos,
        RepositoryEmpleo repoEmpleo)
        {
            this.service_noticias = service_noticias;
            this.service_cursos = service_cursos;
            this._logger = logger;
            this._servicevypwRSS = servicevypwRSS;

            this._repoEmpleo = repoEmpleo;
        }
        /// <summary>
        /// Gets the posts for RSS items.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IEnumerable<Post>> GetOfertasAsPosts()
        {
            IEnumerable<Post> posts = await _servicevypwRSS.GetOfertasAsPosts();
            return posts;
        }
        [HttpGet("[action]")]
        public async Task<IEnumerable<Post>> GetNoticiasAsPosts()
        {
            IEnumerable<Post> posts = await _servicevypwRSS.GetNoticiasAsPosts();
            return posts;
        }
        [HttpGet("[action]")]
        public async Task<DataContainer> GetData()
        {
            try
            {
                List<Empleo> ofertas = null;
                List<Article> noticias = null;
                List<Article> otras_noticias = null;
                List<Curso> cursos = null;

                Article noticia;
                Curso curso;
                Article otra_noticia;
                Empleo oferta;

                noticias = await service_noticias
                    .GetArticles("https://..//category/noticias/noticias-de-seguridad/feed/");

                ofertas = _repoEmpleo.GetLast3Ofertas();

                otras_noticias = await service_noticias
                    .GetArticles("https://..//category/noticias/otras-noticias/feed/");

                cursos = await service_cursos.GetCursos();
                var rnd = new Random();
                if (cursos != null)
                {
                    cursos = cursos.OrderBy(item => rnd.Next()).ToList();
                }


                noticia = await service_noticias.GetLastNoticia();

                curso = await service_cursos.GetLastCurso();
                otra_noticia = await service_noticias.GetLastNoticiaPolicia();
                oferta = _repoEmpleo.GetLastEmpleo();

                DataContainer datos = new DataContainer()
                {
                    cursos = cursos,
                    ofertas = ofertas,
                    noticias = noticias,
                    otras_noticias = otras_noticias,
                    noticia = noticia,
                    oferta = oferta,
                    otra_noticia = otra_noticia,
                    curso = curso
                };
                return datos;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return null;
            }

        }
    }
}

