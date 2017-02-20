using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Runtasker.Logic.Entities
{
    #region Extension methods
    public static class OtherUserInfoExtensionMethods
    {
        public static IEnumerable<Subject> GetPerformerSubjects(this OtherUserInfo info)
        {
            List<Subject> allSubjects = Enum.GetValues(typeof(Subject)).Cast<Subject>().ToList();

            List<int> subjectInts = GetSubjectIntsFromSpecializationString(info.Specialization);

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


        public static IEnumerable<Order> GetOrdersForPerformerByInfo(this OtherUserInfo info, IEnumerable<Order> orders)
        {
            List<Subject> performerSubjects = GetPerformerSubjects(info).ToList();

            List<Order> result = new List<Order>();
            foreach(Order order in orders)
            {
                //если в списке предметов исполнителя содержиться
                //предмет заказа или предмет точно не указан
                //то добавляем предмет в результат
                if(performerSubjects.Any(x => x == order.Subject) || (order.Subject == Subject.Other))
                {
                    result.Add(order);
                }
            }

            return result;
        }

        #region Help Methods
        static List<int> GetSubjectIntsFromSpecializationString(string specString)
        {
            return specString
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

    public class OtherUserInfo
    {
        public OtherUserInfo()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public string VkDomain { get; set; }

        public string VkId { get; set; }

        public string Specialization { get; set; }

        [Required]
        [StringLength(128)]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }
    }
}
