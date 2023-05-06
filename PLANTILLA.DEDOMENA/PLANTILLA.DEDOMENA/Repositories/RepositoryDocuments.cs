using System;
using System.Collections.Generic;
using System.Linq;
using PLANTIILLA.DEDOMENA.Clases;
using PLANTIILLA.DEDOMENA.Data;
using PLANTIILLA.DEDOMENA.Helpers;
using PLANTIILLA.DEDOMENA.Models;

namespace PLANTIILLA.DEDOMENA.Repositories
{
    public class RepositoryDocuments
    {
        DataContext _context;


        public RepositoryDocuments(DataContext context)
        {
            this._context = context;
        }

        public void InsertFichero(string fileName, string path, string Descripcion)
        {
            Fichero file = _context.Documentos.FirstOrDefault(x => x.Name == fileName);
            if (file == null)
            {

                Fichero f = new Fichero()
                {
                    Description = Descripcion,
                    Id = 0,
                    Name = fileName,
                    Path = path,
                    Img = null
                };
                _context.Documentos.Add(f);
                _context.SaveChanges();
            }
            else
            {
                file.Description = Descripcion;
                _context.SaveChanges();
            }
        }
        public void DeletetFichero(Fichero file)
        {
            Fichero f = _context.Documentos.FirstOrDefault(x => x.Id == file.Id);
            if (f != null)
            {
                _context.Documentos.Remove(f);
                _context.SaveChanges();
            }
        }
        public string GetFicheroDescripcion(string fileName)
        {
            Fichero f = _context.Documentos.FirstOrDefault(x => x.Name == fileName);
            if (f != null)
            {
                return f.Description;
            }
            else
            {
                return default(string);
            }
        }
        public List<Fichero> GetFicheros()
        {
            return _context.Documentos.OrderByDescending(x => x.Id).ToList();
        }
        public Fichero GetFichero(string FileName)
        {
            return _context.Documentos.FirstOrDefault(x => x.Name.Equals(FileName));
        }
    }
}
