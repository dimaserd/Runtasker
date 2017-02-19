using Extensions.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkParser.Contexts;
using VkParser.Entities;
using VkParser.Enumerations;
using VkParser.Extensions;
using VkParser.Models;

namespace VkParser.Workers.KeyWords
{
    /// <summary>
    /// этот класс получает пост разбивает его на слова
    /// затем выдает строчку содержащую все найденные слова
    /// записанные через запятую и далее по этим словам выдает
    /// свой вердикт по поводу предмета к котору принадлежит просмотренный
    /// пост. Также в дальнейшем можно добавить перечисление определяющее
    /// слова которые будут отсекать найденные посты в какие либо другие группы
    /// например (пересдача, и так далее)
    /// </summary>
    public class NewVkKeyWordsChecker
    {
        #region Constructor
        public NewVkKeyWordsChecker(IEnumerable<VkKeyWord> vkKeyWords)
        {
            VkKeyWords = vkKeyWords.ToList();
        }
        #endregion

        #region Properties
        List<VkKeyWord> VkKeyWords { get; set; }
        #endregion

        #region Public methods
        /// <summary>
        /// Возвращает сам пост внутри которого содержится коллекция
        /// ключевых слов в нем найденных
        /// </summary>
        public bool CheckPostForKeyWords(VkFoundPost vkPost)
        {
            string keyWordsString = string.Empty;

            List<VkKeyWord> keyWordFromPost = GetVkKeyWordsFromVkPost(vkPost, out keyWordsString);
            
            vkPost.VkKeyWords = keyWordFromPost;
            vkPost.FoundKeyWords = keyWordsString;

            //означает что какие то  ключевые слова были найдены
            return keyWordFromPost.Count > 0;
        }
        #endregion

        #region Help Methods
        

        List<VkKeyWord> GetVkKeyWordsFromApiPost(VkAPIFoundPostModel apiPost)
        {
            List<VkKeyWord> result = new List<VkKeyWord>();

            List<string> wordsFromPost = apiPost.Text.GetWordsFromText();

            VkKeyWord vkKeyWord;
            foreach (string word in wordsFromPost)
            {
                if(isThatWordAKeyWord(word, out vkKeyWord))
                {
                    //если данное слово является ключевым 
                    //добавляем его к нашему результату
                    result.Add(vkKeyWord);
                }
            }
            return result;
        }

        List<VkKeyWord> GetVkKeyWordsFromVkPost(VkFoundPost vkPost, out string keyWordsString)
        {
            List<VkKeyWord> result = new List<VkKeyWord>();

            List<string> wordsFromPost = vkPost.Text.GetWordsFromText();
            
            //слова которые являются ключевыми далее из них
            //мы получим строку со словами разбитыми запятой
            //чтобы потом выделить их желтым
            List<string> wordsThatAreKeyWords = new List<string>();

            VkKeyWord vkKeyWord;
            foreach (string word in wordsFromPost)
            {
                if (isThatWordAKeyWord(word, out vkKeyWord))
                {
                    //если данное слово является ключевым 
                    //добавляем его к нашему результату
                    result.Add(vkKeyWord);

                    wordsThatAreKeyWords.Add(word);
                }
            }

            //передаем строку
            keyWordsString = GetWordsStringFromList(wordsThatAreKeyWords);

            if(keyWordsString != string.Empty)
            {
                System.Diagnostics.Trace.WriteLine("что-то нашли!");
            }
            return result;
        }

        bool isThatWordAKeyWord(string word, out VkKeyWord vkKeyWord)
        {
            bool result = false;
            vkKeyWord = null;

            foreach(VkKeyWord keyWord in VkKeyWords)
            {
                if(isThatWordThatKeyWord(word, keyWord))
                {
                    result = true;
                    //отдаем по ссылке обратно
                    vkKeyWord = keyWord;
                    break;
                }
            }

            return result;
        }

        bool isThatWordThatKeyWord(string word, VkKeyWord keyWord)
        {
            bool result = false;
            List<string> wordForms = keyWord.OtherWordForms
                .Split(separator: new string[] { "," }, options: StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            foreach(string wordForm in wordForms)
            {
                if(string.Compare(word, wordForm, false) == 0)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        string GetWordsStringFromList(List<string> words)
        {
            StringBuilder sb = new StringBuilder();
            foreach(string word in words)
            {
                sb.Append($"{word},");
            }

            return sb.ToString();
        }
        #endregion
    }
}
