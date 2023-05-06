using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using PLANTIILLA.DEDOMENA.Controllers;

namespace PLANTIILLA.DEDOMENA.Helpers
{
    public class FileService
    {
        PathProvider provider;
        readonly ILogger<FileService> _logger;

        public FileService(PathProvider provider, ILogger<FileService> logger)
        {
            this._logger = logger;
            this.provider = provider;
        }
        public async Task<string> UploadFileAsync(IFormFile fichero, Folders folder)
        {
            string filename = fichero.FileName;
            string path = provider.MapPath(filename, folder);
            _logger.LogWarning("path: " + path);
            _logger.LogWarning("filename: " + filename);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await fichero.CopyToAsync(stream);
            }
            return path;
        }
        public async Task<string> UploadFileAsync(string content, string filename, Folders folder)
        {
            string path = provider.MapPath(filename + ".xml", folder);
            using StreamWriter file = new(path);

            await file.WriteAsync(content);
            return path;
        }
        public async Task<string[]> GetFilesAsync(Folders folder)
        {
            string path = provider.MapPathRoot(folder);
            string[] files = Directory.GetFiles(path);

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileName(files[i]);
            }
            return files;
        }


    }
}
