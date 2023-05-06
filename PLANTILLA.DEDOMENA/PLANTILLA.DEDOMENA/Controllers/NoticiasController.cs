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
    public class NoticiasController : ControllerBase
    {
        readonly ILogger<EmpleoController> _logger;

        readonly Service_VYP_NOTICIAS service;
        public NoticiasController(
        ILogger<EmpleoController> logger,
        Service_VYP_NOTICIAS service)
        {
            this._logger = logger;
            this.service = service;
        }
        [HttpGet("[action]")]
        public async Task<Article> GetLastNoticia()
        {
            Article article = await service.GetLastNoticia();
            return article;
        }
        [HttpGet("[action]")]
        public async Task<Article> GetLastNoticiaPolicia()
        {
            Article article = await service.GetLastNoticiaPolicia();
            return article;
        }
        [HttpGet("[action]")]
        public async Task<Article> GetNoticia(string fecha, string titulo)
        {
            Article article = await service.GetNoticia(fecha, titulo);
            return article;
        }
        [HttpGet("[action]")]
        public async Task<Article> GetSentencia(string fecha, string titulo)
        {
            Article article = await service.GetSentencia(fecha, titulo);
            return article;
        }
        [HttpGet("[action]")]
        public async Task<Article> GetArticuloCiberSeguridad(string fecha, string titulo)
        {
            Article article = await service.GetArticuloCiber(fecha, titulo);
            return article;
        }
        [HttpGet("[action]")]
        public async Task<Article> GetArticuloSLaboral(string fecha, string titulo)
        {
            Article article = await service.GetArticuloSLaboral(fecha, titulo);
            return article;
        }
        [HttpGet("[action]")]
        public async Task<List<Article>> GetArticulos(string url)
        {
            List<Article> articulos = await service.GetArticles(url);
            return articulos;
        }

        
    }

}
