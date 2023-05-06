using System;
using System.Linq;
using System.Text;
using PLANTILLA.ICARO.Data;
using PLANTILLA.ICARO.Helpers;
using PLANTILLA.ICARO.Models;

namespace PLANTILLA.ICARO.Repositories
{
    public class RepositoryTokens
    {
        DataContext _context;


        public RepositoryTokens(DataContext context)
        {
            this._context = context;
        }
        public void RegisterCode(string key, string token)
        {
            string cr_token = StringCipher.Encrypt(token, key);

            Token tk = new Token()
            {
                Id = 0,
                Key = key,
                Code = cr_token,
            };
            _context.Tokens.Add(tk);
            _context.SaveChanges();
        }
        public string GetCode(string key)
        {
            Token token =  _context.Tokens.Where(x => x.Key.Equals(key)).FirstOrDefault();
            if(token != null)
            {
                return StringCipher.Decrypt(token.Code, key);
            }
            else { return "Invalid KEY"; }
        }
        public string UpdateCode(string key,string newToken)
        {
            Token token = _context.Tokens.Where(x => x.Key.Equals(key)).FirstOrDefault();
            string oldtoken = null;
            if (token != null)
            {
                oldtoken = StringCipher.Decrypt(token.Code, key);
            }
            if (oldtoken == null)
            {
                return "Invalid KEY";
            }
            token.Code = StringCipher.Encrypt(newToken,key);
            _context.SaveChanges();
            return "Updated CODE";
        }
    }
}
