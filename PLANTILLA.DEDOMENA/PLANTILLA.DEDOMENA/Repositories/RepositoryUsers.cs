using System;
using System.Linq;
using PLANTIILLA.DEDOMENA.Data;
using PLANTIILLA.DEDOMENA.Helpers;
using PLANTIILLA.DEDOMENA.Models;

namespace PLANTIILLA.DEDOMENA.Repositories
{
    public class RepositoryUsers
    {
        DataContext _context;


        public RepositoryUsers(DataContext context)
        {
            this._context = context;
        }
        internal void Register(string username, String password)
        {
            string salt = Helpers.CypherService.GetSalt();
            byte[] pass = Helpers.CypherService.Encrypt(password, salt);
            int id = 1;
            if (_context.Usuarios.Count() > 0)
            {
                id = _context.Usuarios.Max(x => x.Id) + 1;
            }

            User u = new User()
            {
                Id = id,
                Password = pass,
                Salt = salt,
                UserName = username,
            };
            _context.Usuarios.Add(u);
            _context.SaveChanges();
        }
        public User CheckUser(string username, string password)
        {
            User u = _context.Usuarios.Where(x => x.UserName == username).FirstOrDefault();
            if (u == null)
            {
                return null ;
            }
            else
            {
                String salt = u.Salt;
                byte[] passwordDB = u.Password;
                byte[] passuser = CypherService.Encrypt(password, salt);
                bool valid = ToolKit.CompareBytes(passuser, passwordDB);
                if (valid) return u ;
                else return null;
            }
        }
    }
}
