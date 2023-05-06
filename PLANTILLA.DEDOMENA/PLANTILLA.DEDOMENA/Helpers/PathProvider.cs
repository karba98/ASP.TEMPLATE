using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace PLANTIILLA.DEDOMENA.Helpers
{
    public enum Folders
    {
        Images = 0, Documents = 1, None = 2
    }

    public class PathProvider
    {
        private IWebHostEnvironment environment;

        public PathProvider(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public string MapPath(string filename, Folders folder)
        {
            string carpeta = "";
            string assetsfolder = "assets";
            if (folder == Folders.Documents)
            {
                carpeta = "Documents";
            }
            else if (folder == Folders.Images)
            {
                carpeta = "images";
            }
            else
            {
                return Path.Combine(environment.WebRootPath, filename);
            }
            string ruta = Path.Combine(environment.WebRootPath, assetsfolder, carpeta, filename);
            return ruta;
        }
        public string MapPathRoot(Folders folder)
        {
            string carpeta = "";
            string assetsfolder = "assets";
            if (folder == Folders.Documents)
            {
                carpeta = "Documents";
            }
            else if (folder == Folders.Images)
            {
                carpeta = "images";
            }
            string ruta = Path.Combine(environment.WebRootPath, assetsfolder, carpeta);
            return ruta;
        }
    }
}