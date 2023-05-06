using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PLANTILLA.Filters;
using PLANTILLA.Helpers;
using PLANTILLA.Models;
using PLANTILLA.Services;

namespace PLANTILLA.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly FileService fileService;
        private readonly PathProvider pathProvider;
        private readonly Service_DEDOMENA _service;
        private readonly IWebHostEnvironment _env;
        readonly ILogger<DocumentsController> _logger;

        public DocumentsController(PathProvider pathProvider, IWebHostEnvironment environment,
            ILogger<DocumentsController> _logger, Service_DEDOMENA service)
        {
            this._service = service;
            this._env = environment;
            this.pathProvider = pathProvider;
            this.fileService = new FileService(pathProvider);
            this._logger = _logger;
        }
        #region ADMINISTRACION
        [AuthorizeUsers]
        public async Task<IActionResult> DocumentsManager()
        {
            try
            {
                List<Fichero> ficheros = await _service.CallApi<List<Fichero>>(
               "Documentos/GetFicheros",
               null);

                return View(ficheros);
            }catch(Exception ex)
            {
                return View(null);
            }
        }

        
        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> DocumentsManager(List<IFormFile> postedFiles, string descripcion)
        {
            try
            {
                if (postedFiles.Count > 0)
                {
                    IFormFile file = postedFiles.First();
                    if (file.FileName.EndsWith(".pdf"))
                    {
                        try
                        {
                            await fileService.UploadFileAsync(file, Folders.Documents);
                            //repo.InsertFichero(file.FileName, pathProvider.MapPath(file.FileName, Folders.Documents), descripcion);

                            bool inserted = await _service.CallApi<bool>(
                              "Documentos/InsertFichero",
                              HttpContext.Session.GetString("TOKEN_DEDOMENA"),
                              new Dictionary<string, string>() {
                                  {"FileName" , file.FileName},
                                  {"Path",pathProvider.MapPath(file.FileName, Folders.Documents) },
                                  {"Descripcion",descripcion }
                                  },
                              null,
                              "POST");
                            return RedirectToAction("DocumentsManager");
                        }
                        catch (Exception) { return RedirectToAction("DocumentsManager"); }
                    }
                    return RedirectToAction("DocumentsManager");
                }
                else
                {
                    return RedirectToAction("DocumentsManager");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                return RedirectToAction("DocumentsManager");
            }
        }
        [AuthorizeUsers]
        public async Task<IActionResult> DocumentDelete(string filename)
        {
            string filepath = pathProvider.MapPath(filename, Folders.Documents);
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
            //repo.DeletetFichero(filename);
            bool deleted = await _service.CallApi<bool>(
                "Documentos/DeleteFichero",
                HttpContext.Session.GetString("TOKEN_DEDOMENA"),
                new Dictionary<string, string>() {
                    {"FileName" , filename}
                    },
                null,
                "DELETE");
            return RedirectToAction("DocumentsManager");
        }
        [AuthorizeUsers]
        public async Task<IActionResult> RSSIndeed()
        {
            return View();

        }
       
        #endregion
        #region USUARIO
        public async Task<IActionResult> DocumentsList()
        {
            List<Fichero> files = await _service.CallApi<List<Fichero>>(
                "Documentos/GetFicheros",
                null);

            ViewBag.MetaTitle = "Documentos";
            ViewBag.Title = "Documentos";
            ViewBag.MetaDescription = "Documentos varios acerca de seguridad privada";
            ViewBag.MetaImg = "https://vigilanciayproteccion.website/assets/images/logo_card.png";

            return View(files);
        }
        public async Task<FileResult> DocumentDownload(string filename)
        {
            Fichero fichero = await _service.CallApi<Fichero>(
                "Documentos/GetFichero",
                HttpContext.Session.GetString("TOKEN_DEDOMENA"),
                new Dictionary<string, string>() {
                    {"FileName" , filename}
                    },
                null,
                "GET");
            byte[] bytes = System.IO.File.ReadAllBytes(fichero.Path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", filename);
        }
        #endregion
        public class Stackdata
        {
            public string xmltext { get; set; }
        }
    }
}
