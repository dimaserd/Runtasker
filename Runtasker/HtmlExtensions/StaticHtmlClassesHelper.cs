using Runtasker.Logic.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;

namespace Runtasker.HtmlExtensions
{
    public static class StaticHtmlClassesHelper
    {
        public static string GetLangCode()
        {
            string lang = Thread.CurrentThread.CurrentCulture.DisplayName;
            string language = (lang.Contains("Рус")) || (lang.Contains("Rus")) ? "ru" : "en";

            return language;
        }

        #region Enums methods
        //метод возвращающий отсортированный список перечисления
        //по предметам (в базе логично хранить другой предмет с кодом 0)
        //но в представлении ставить его в конец

        public static IEnumerable<SelectListItem> GetSortedSubjectEnumList()
        {
            List<SelectListItem> subjects = new List<SelectListItem>();

            foreach (Subject item in Enum.GetValues(typeof(Subject)))
            {
                if (item != Subject.Other)
                {
                    subjects.Add(new SelectListItem { Value = ((int)item).ToString(), Text = item.ToDescriptionString(), Selected = (item == Subject.ForeignLanguage) });
                }
            }
            subjects.Add(new SelectListItem { Value = "0", Text = Subject.Other.ToDescriptionString(), });

            return subjects;
        }

        public static IEnumerable<SelectListItem> GetWorkTypeEnumList()
        {
            List<SelectListItem> workTypes = new List<SelectListItem>();
            foreach (OrderWorkType item in Enum.GetValues(typeof(OrderWorkType)))
            {
                workTypes.Add(new SelectListItem { Value = ((int)item).ToString(), Text = item.ToDescriptionString(), Selected = (int)item == 0 });
            }

            return workTypes;
        }
        #endregion
    }
}