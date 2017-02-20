﻿using Logic.Extensions.Models;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Models.VkNotificater;
using Runtasker.Logic.Workers.BaseWorkers;
using System;
using System.Collections.Generic;
using System.Linq;
using VkParser.MessageSenders;
using VkParser.Models.MessageSenderModels;

namespace Runtasker.Logic.Workers.Notifications.Orders.VkNotifications
{
    /// <summary>
    /// Класс который создает и отправляет текстовые уведомления для иссполнителей
    /// </summary>
    public class VkPerformerNotificater : IDisposable
    {
        #region Constructor
        public VkPerformerNotificater(IEnumerable<VkUserInfo> userInfos)
        {
            VkInfoList = userInfos.ToList();
        }
        #endregion

        #region Properties
        public List<VkUserInfo> VkInfoList { get; set; }
        #endregion

        #region Public methods

        #region Методы для еще свободного заказа
        public void OnCustomerAddedOrder(Order order)
        {
            List<VkMessage> messages = new List<VkMessage>();
            foreach(VkUserInfo vkInfo in VkInfoList)
            {
                VkMessage message = new VkMessage
                {
                    Text = $"Пользователь добавил заказ по предмету {order.Subject.ToDescriptionString()}\n"
                + $"Проверьте его на наличие ошибок и оцените его стоимость, помните что"
                + $"заказ нужно завершить к {order.FinishDate.ToShortDateString()}",
                    UserDomain = vkInfo.VkDomain,
                    UserId = vkInfo.VkId,
                };
                messages.Add(message);
            }

            using (VkMessageSender sender = new VkMessageSender())
            {
                sender.SendMessagesToVkUsers(messages);
            }
            

        }

        public void OnCustomerChangedOrderDescription(Order order)
        {
            List<VkMessage> messages = new List<VkMessage>();
            foreach (VkUserInfo vkInfo in VkInfoList)
            {
                VkMessage message = new VkMessage
                {
                    Text = $"В заказе №{order.Id} было изменено описание, проверьте\n"
                    + $"Пользователь изменил описание заказа по вашей просьбе! "
                + $"Проверьте и помните, что его нужно выполнить {order.FinishDate.ToString("d MMM yyyy")}",
                    UserDomain = vkInfo.VkDomain,
                    UserId = vkInfo.VkId,
                };
                messages.Add(message);
            }

            using (VkMessageSender sender = new VkMessageSender())
            {
                sender.SendMessagesToVkUsers(messages);
            }


        }

        public void OnCustomerAddedFiles(Order order)
        {
            List<VkMessage> messages = new List<VkMessage>();
            foreach (VkUserInfo vkInfo in VkInfoList)
            {
                VkMessage message = new VkMessage
                {
                    Text =  $"Заказчик добавил файлы к заказу №{order.Id}\n" 
                    + $"Проверьте работу и приступайте к выполнению работы," +
                $"помните, что работа должна быть выполнена к сроку {order.FinishDate.ToString("d MMM yyyy")}",
                    UserDomain = vkInfo.VkDomain,
                    UserId = vkInfo.VkId,
                };
                messages.Add(message);
            }

            using (VkMessageSender sender = new VkMessageSender())
            {
                sender.SendMessagesToVkUsers(messages);
            }


        }

        public void OnCustomerPaidFirstHalf(Order order)
        {
            List<VkMessage> messages = new List<VkMessage>();
            foreach (VkUserInfo vkInfo in VkInfoList)
            {
                VkMessage message = new VkMessage
                {
                    Text = "Пользователь оплатил половину заказа\n"
                    + $"Заказ №{order.Id} оплачен наполовину, приступайте немедленно и успейте к сроку {order.FinishDate}",
                    UserDomain = vkInfo.VkDomain,
                    UserId = vkInfo.VkId,
                };
                messages.Add(message);
            }
        }
        #endregion



        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~VkPerformerNotificater() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        void IDisposable.Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}