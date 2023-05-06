using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace PLANTILLA.Helpers
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

        public String MapPath(String filename, Folders folder)
        {
            String carpeta = "";
            String assetsfolder = "assets";
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
            String ruta = Path.Combine(environment.WebRootPath, assetsfolder, carpeta, filename);
            return ruta;
        }
        public String MapPathRoot(Folders folder)
        {
            String carpeta = "";
            String assetsfolder = "assets";
            if (folder == Folders.Documents)
            {
                carpeta = "Documents";
            }
            else if (folder == Folders.Images)
            {
                carpeta = "images";
            }
            String ruta = Path.Combine(environment.WebRootPath, assetsfolder, carpeta);
            return ruta;
        }
    }
}