using System;
using System.Collections.Generic;
using System.Linq;
using PLANTIILLA.DEDOMENA.Data;
using PLANTIILLA.DEDOMENA.Models;

namespace PLANTIILLA.DEDOMENA.Repositories
{
    public class RepositoryEmpresas
    {
        DataContext _context;

        public RepositoryEmpresas(DataContext context)
        {
            this._context = context;
        }
        public List<Empresa> GetEmpresas()
        {
            return _context.Empresas.Distinct().ToList();
        }
       
    }
}
