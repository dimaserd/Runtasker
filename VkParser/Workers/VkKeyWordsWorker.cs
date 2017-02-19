using Logic.Extensions.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using VkParser.Contexts;
using VkParser.Entities;
using VkParser.Workers.Base;

namespace VkParser.Workers
{
    public class VkKeyWordsWorker : BaseVkParseContextWorker
    {
        #region Constructors
        public VkKeyWordsWorker(VkParseContext context) : base (context)
        {

        }


        #endregion

        

        #region Public Methods

        public IEnumerable<VkKeyWord> GetVkKeyWords()
        {
            return Context.VkKeyWords.ToList();
        }

        public VkKeyWord GetVkKeyWord(int id)
        {
            return Context.VkKeyWords.FirstOrDefault(g => g.Id == id);
        }

        public VkKeyWord AddNewVkKeyWord(VkKeyWord vkKeyWord)
        {
            Context.VkKeyWords.Add(vkKeyWord);
            Context.SaveChanges();

            return vkKeyWord;
        }

        #region Update methods

        public WorkerResult UpdateKeyWord(VkKeyWord vkKeyWord)
        {
            //сохранение в базе
            Context.VkKeyWords.Attach(vkKeyWord);
            var entry = Context.Entry(vkKeyWord);
            entry.Property(e => e.MainForm).IsModified = true;
            entry.Property(e => e.OtherWordForms).IsModified = true;
            entry.Property(e => e.Subject).IsModified = true;
            Context.SaveChanges();

            return new WorkerResult
            {
                Succeeded = true
            };
        }
        #endregion

        public async Task<WorkerResult> CreateKeyWordAsync(VkKeyWord vkKeyWord)
        {
            string foundWord = string.Empty;
            if(await IsThereAnyEqualWordInKeyWordsFromDbAsync(vkKeyWord, foundWord))
            {
                return new WorkerResult($"Найдено такое же слово в базе данных! слово : '{foundWord}'");
            }
            Context.VkKeyWords.Add(vkKeyWord);
            Context.SaveChanges();

            return new WorkerResult
            {
                Succeeded = true
            };
        }
        #endregion

        #region Help Methods
        public async Task<bool> IsThereAnyEqualWordInKeyWordsFromDbAsync(VkKeyWord vkKeyWord, string foundWord)
        {
            string[] words = vkKeyWord.OtherWordForms
                .Split(separator: new string[] { "," }, options: StringSplitOptions.RemoveEmptyEntries);

            string[] wordsInDb = await Context.VkKeyWords.Select(x => x.OtherWordForms).ToArrayAsync();

            bool result = false;
            
            foreach(string word in words)
            {
                if (wordsInDb.Any(x => string.Compare(x, word, false) == 0))
                {
                    result = true;
                    //передаем что слово найдено
                    foundWord = word;
                    break;
                }
            } 
               
            return result;
        }
        #endregion
    }
}
