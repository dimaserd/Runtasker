using System;
using System.Collections.Generic;
using System.Linq;
using VkParser.Entities;

namespace Runtasker.Logic.Workers.VkParse
{
    public class VkPostChecker
    {
        #region Constructors
        public VkPostChecker(IEnumerable<VkKeyWord> words)
        {
            KeyWords = words;
        }
        #endregion

        #region Properties
        public IEnumerable<VkKeyWord> KeyWords { get; set; }
        #endregion

        #region Public Methods
        public bool CheckVkPostForMatchingOtherFormsFromVkKeyWord(VkFoundPost vkPost, VkKeyWord vkKeyWord, out string foundWord)
        {
            bool result = false;
            foundWord = "";
            string[] separators = new string[] { "," };
            List<string> otherWords = vkKeyWord.OtherWordForms.Split(separator: separators, options: StringSplitOptions.None).ToList();
            foreach (string word in otherWords)
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
        #endregion

        #region Help Methods
        IEnumerable<string> GetWordsFromVkKeyWords(IEnumerable<VkKeyWord> vkKeyWords)
        {
            List<string> result = new List<string>();

            string[] separator = new string[] { "," };
            //сортировка по убыванию чтобы первые слова не попадали
            foreach (VkKeyWord vkWord in vkKeyWords.OrderByDescending(x=>x.Subject))
            {
                result.AddRange(vkWord.OtherWordForms.Split(separator, StringSplitOptions.None).ToList());
            }

            return result;
        }
        #endregion
    }
}
