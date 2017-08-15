using Newtonsoft.Json;
using Runtasker.Logic.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Runtasker.Logic.Models.ManageModels
{
    

    public class OtherUserInfo
    {

        public string UserId { get; set; }

        public string VkDomain { get; set; }

        public string VkId { get; set; }

        public string Specialization { get; set; }


        
    }


    #region Методы расширения
    public static class OtherUserInfoExtensions
    {
        public static OtherUserInfo GetOtherInfo(this ApplicationUser user)
        {
            return new OtherUserInfo
            {
                UserId = user.Id,
                Specialization = user.Specialization,
                VkDomain = user.VkDomain,
                VkId = user.VkId
            };
        }

        public static ApplicationUser UpdateUserInfo(this ApplicationUser user, OtherUserInfo info)
        {
            if(user.Id != info.UserId)
            {
                throw new Exception("Id пользователя и дополнительной информации не совпадают!");

            }

            user.Specialization = info.Specialization;
            user.VkDomain = info.Specialization;
            user.VkId = info.VkId;

            return user;
        }

        public static IEnumerable<Subject> GetSubjects(this OtherUserInfo info)
        {
            List<Subject> allSubjects = Enum.GetValues(typeof(Subject)).Cast<Subject>().ToList();

            List<int> subjectInts = GetSubjectInts(info);

            List<Subject> result = new List<Subject>();

            foreach (Subject subject in allSubjects)
            {
                if (subjectInts.Contains((int)subject))
                {
                    result.Add(subject);
                }
            }

            return result;
        }

        public static IEnumerable<Order> GetOrdersBySpecialization(this OtherUserInfo info, IEnumerable<Order> orders)
        {
            List<Order> result = new List<Order>();

            foreach(Order order in orders)
            {
                if (info.Specialization.Contains(((int)order.Subject).ToString()))
                {
                    result.Add(order);
                }
            }
            return result;
        }

        public static IEnumerable<OrderAndMessageCount> GetOrdersBySpecialization(this OtherUserInfo info, IEnumerable<OrderAndMessageCount> orders)
        {
            List<OrderAndMessageCount> result = new List<OrderAndMessageCount>();

            foreach (OrderAndMessageCount order in orders)
            {
                if (info.Specialization.Contains(((int)order.Order.Subject).ToString()))
                {
                    result.Add(order);
                }
            }
            return result;
        }

        #region Вспомогательные методы
        static List<int> GetSubjectInts(this OtherUserInfo info)
        {
            return info.Specialization
                .Split(separator: new string[] { "," }, options: StringSplitOptions.RemoveEmptyEntries)
                .Select(x =>
                {
                    int parseResult;
                    if (int.TryParse(x, out parseResult))
                    {
                        return parseResult;
                    }
                    else
                    {
                        return 0;
                    }
                }).Where(x => x > 0).ToList();
        }
        #endregion
    }
    #endregion
}
