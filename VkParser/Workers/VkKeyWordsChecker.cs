using System;
using System.Collections.Generic;
using System.Linq;
using VkParser.Contexts;
using VkParser.Entities;
using VkParser.Enumerations;

namespace VkParser.Workers
{
    //класс который проверяет пост на ключевые слова
    public class VkKeyWordsChecker : IDisposable
    {
        #region Constructors
        public VkKeyWordsChecker()
        {
            Construct();
        }

        void Construct()
        {
        }
        #endregion

        #region Properties
        
        IEnumerable<VkKeyWord> KeyWords { get; set; }
        #endregion

        #region Public Methods

        public bool CheckPostForWords(VkFoundPost vkPost, out WordType subject, out string word)
        {
            CheckForKeyWords();

            WordType subjectResult = WordType.OtherWord;

            word = "";
            bool result = false;

            foreach(VkKeyWord keyWord in KeyWords)
            {
                if(CheckVkPostForMatchingOtherFormsFromVkKeyWord(vkKeyWord: keyWord, vkPost: vkPost, foundWord: out word))
                {
                    result = true;
                    
                    //отправляем  код предмета для другого метода чтобы это записалось в базу
                    subjectResult = keyWord.Subject;
                    break;
                }
            }
            subject = subjectResult;
            return result;
        }
        #endregion

        #region Help Methods
        void CheckForKeyWords()
        {
            if(KeyWords == null)
            {
                SetKeyWords();
            }
        }

        void SetKeyWords()
        {
            using (VkParseContext context = new VkParseContext())
            {
                KeyWords = context.VkKeyWords.ToList();
            }
        }

        public bool CheckVkPostForMatchingOtherFormsFromVkKeyWord(VkFoundPost vkPost, VkKeyWord vkKeyWord, out string foundWord)
        {
            bool result = false;
            foundWord = "";
            string[] separators = new string[] { "," };
            List<string> otherWords = vkKeyWord.OtherWordForms.Split(separator: separators, options: StringSplitOptions.None).ToList();
            foreach(string word in otherWords)
            {
                if (vkPost.Text.ToLower().Contains(word))
                {
                    foundWord = word;
                    result = true;
                    break;
                }
            }
            return result;
        }

        IEnumerable<string> GetWordsFromVkKeyWords(IEnumerable<VkKeyWord> vkKeyWords)
        {
            List<string> result = new List<string>();

            string[] separator = new string[] { "," };
            foreach(VkKeyWord vkWord in vkKeyWords)
            {
                result.AddRange(vkWord.OtherWordForms.Split(separator, StringSplitOptions.None).ToList());
            }

            return result;
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        public virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.
                KeyWords = null;

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~VkKeyWordsChecker() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
