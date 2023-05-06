using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PLANTILLA.Helpers
{
    public class FileService
    {
        PathProvider provider;
        public FileService(PathProvider provider)
        {
            this.provider = provider;
        }
        public async Task<String> UploadFileAsync(IFormFile fichero, Folders folder)
        {
            String filename = fichero.FileName;
            if(folder == Folders.None)
            {
                filename = "sitemap.xml";
            }
            String path = provider.MapPath(filename, folder);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await fichero.CopyToAsync(stream);
            }
            return path;
        }
        public async Task<String> UploadFileAsync(String content,String filename, Folders folder)
        {
            String path = provider.MapPath(filename+".xml", folder);
            using StreamWriter file = new(path);

            await file.WriteAsync(content);
            return path;
        }
        public async Task<String[]> GetFilesAsync(Folders folder)
        {
            String path = provider.MapPathRoot(folder);
            string[] files = Directory.GetFiles(path);

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileName(files[i]);
            }
            return files;
        }


    }
}
