using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PLANTIILLA.DEDOMENA.Helpers;
using PLANTIILLA.DEDOMENA.Models;
using PLANTIILLA.DEDOMENA.Repositories;

namespace PLANTIILLA.DEDOMENA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentosController : ControllerBase
    {
        RepositoryDocuments _repo;
        private readonly FileService fileService;
        private readonly PathProvider pathProvider;


        readonly ILogger<DocumentosController> _logger;

        public DocumentosController(RepositoryDocuments repo,
            ILogger<DocumentosController> logger,
            FileService fileService)
        {
            this._repo = repo;
            this._logger = logger;
            this.fileService = fileService;
            this.pathProvider = pathProvider;
        }
        [HttpGet("[action]")]
        public List<Fichero> GetFicheros()
        {
            try
            {
                List<Fichero> documentos = _repo.GetFicheros();
                return documentos;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message+"\n"+ex.StackTrace);
                return null;
            }
        }
        [Authorize]
        [HttpPost("[action]")]
        public bool InsertFichero(string FileName, string Path, string Descripcion)
        {
            try
            {
                _repo.InsertFichero(FileName, Path, Descripcion);
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message+"\n"+ex.StackTrace);
                return false;
            }
        }
        [Authorize]
        [HttpDelete("[action]")]
        public bool DeleteFichero(string FileName)
        {
            try
            {
                Fichero file = _repo.GetFichero(FileName);
                _repo.DeletetFichero(file);
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message+"\n"+ex.StackTrace);
                return false;
            }
        }
        [Authorize]
        [HttpGet("[action]")]
        public Fichero GetFichero(string FileName)
        {
            try
            {
                Fichero file = _repo.GetFichero(FileName);
                return file; 
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message+"\n"+ex.StackTrace);
                return null;
            }
        }
        [Authorize]
        [HttpPut("[action]")]
        public async Task<bool> RewriteIndeedRSSFile(Stackdata stack)
        {
            try
            {
                _logger.LogWarning(ToolKit.Serialize(stack));
                await fileService.UploadFileAsync(stack.xmltext, "indeed", Folders.None);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message+"\n"+ex.StackTrace);
                return false;
            }
        }
        public class Stackdata
        {
            public string xmltext { get; set; }
        }
    }
}
