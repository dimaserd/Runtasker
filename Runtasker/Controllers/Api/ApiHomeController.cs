using Runtasker.Logic.Entities;
using Runtasker.Logic.Models.Orders;
using Runtasker.Logic.Models.WokerResults;
using Runtasker.Resources.Views.Orders.Create;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Runtasker.Controllers.Api
{
    [RoutePrefix("api/home")]
    public class ApiHomeController : ApiController
    {
        [Route("CheckKnowPriceModel")]
        public WorkerPropertyResult CheckKnowPriceModel(AnonymousKnowThePrice model)
        {
            //создаем список ошибок при проверке
            List<PropertyError> errorsList = new List<PropertyError>();

            //проверяем дату к которой работа должна быть выполнена
            switch (model.WorkType)
            {
                case OrderWorkType.Ordinary:
                    if ((model.CompletionDate - DateTime.Now).TotalDays < 3)
                    {
                        //ModelState.AddModelError("", Resources.Views.Orders.Create.Create.FinishDateErrorOrdinary);
                        errorsList.Add(new PropertyError
                        {
                            ErrorMessage = Create.FinishDateErrorOrdinary,
                            PropertyName = "CompletionDate"
                        });
                    }
                    break;

                case OrderWorkType.Essay:
                    if ((model.CompletionDate - DateTime.Now).TotalDays < 7)
                    {
                        errorsList.Add(new PropertyError
                        {
                            ErrorMessage = Create.FinishDateErrorEssay,
                            PropertyName = "CompletionDate"
                        });
                    }
                    break;

                case OrderWorkType.CourseWork:
                    if ((model.CompletionDate - DateTime.Now).TotalDays < 30)
                    {
                        errorsList.Add(new PropertyError
                        {
                            ErrorMessage = Create.FinishDateErrorCourseWork,
                            PropertyName = "CompletionDate"
                        });
                    }
                    break;

                default:
                    break;
            }

            return new WorkerPropertyResult(errorsList);
        }
    }
}
