using System;
using System.Collections.Generic;
using System.Linq;
using PLANTIILLA.DEDOMENA.Data;
using PLANTIILLA.DEDOMENA.Helpers;
using PLANTIILLA.DEDOMENA.Models;

namespace PLANTIILLA.DEDOMENA.Repositories
{
    public class RepositoryWords
    {
        DataContext _context;


        public RepositoryWords(DataContext context)
        {
            this._context = context;
        }
        internal void Insert(string word)
        {

            _context.Words.Add(new Word() {Id=0, word = word });
            _context.SaveChanges();
        }
        internal void Delete(string word)
        {
            Word w = _context.Words.Where(x => x.word == word).FirstOrDefault();
            _context.Words.Remove(w);
            _context.SaveChanges();
        }
        internal List<string> GetWords()
        {
            try
            {
                List<Word> words = _context.Words.ToList();
                List<string> palabras = new List<string>();
                if (words.Count > 0)
                {
                    foreach (Word w in words)
                    {
                        palabras.Add(w.word);
                    }
                }
                return palabras;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
