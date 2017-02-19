using Logic.Extensions.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Notifications;
using Runtasker.Logic.Workers.Payments;
using System;
using System.Linq;

namespace Runtasker.Logic.Workers.Invitations
{
    public class InvitationWorker
    {
        #region Constructors
        public InvitationWorker(string userGuid, MyDbContext context)
        {
            Construct(userGuid, context);
        }

        void Construct(string userGuid, MyDbContext context)
        {
            Context = context;
            UserGuid = userGuid;

            Notificater = new InvitationNotificationMethods(Context);
        }
        #endregion

        #region Properties
        MyDbContext Context { get; set; }

        string UserGuid { get; set; }

        InvitationNotificationMethods Notificater { get; set; }
        #endregion

        #region Public Methods

        #region SendInvitation Methods
        public SendInvitationModel GetSendInvitationModel()
        {
            return new SendInvitationModel
            {
                UserGuid = UserGuid
            };
        }

        public WorkerResult SendInvitation(SendInvitationModel model)
        {
            

            

            #region Feature to get extra 50 roubles
            string userEmail = GetUserEmail();
            if (model.ReceiverEmail == userEmail)
            {
                Invitation IForMe = Context.Invitations.FirstOrDefault(
                    i => i.SenderGuid == UserGuid
                    && i.ReceiverGuid == UserGuid);

                if(IForMe == null)
                {
                    Invitation newIForMe = new Invitation
                    {
                        Id = Guid.NewGuid().ToString(),
                        SenderGuid = UserGuid,
                        ReceiverGuid = UserGuid,
                        ReceiverEmail = userEmail,
                        Date = DateTime.Now,
                        Status = InvitationStatus.Paid
                    };
                    Context.Invitations.Add(newIForMe);
                    Context.SaveChanges();

                    
                    new InvitationPaymentMethods(Context).OnInvitedFeatureRevealed(50, UserGuid);
                    PlusUserBalance(50);
                    Notificater.OnUserSentAnInvitationToHimSelf(newIForMe);
                }

                return new WorkerResult("You can't send an invitation to yourself!");
            }
            #endregion

            if (!Context.Orders.Any(o => o.UserGuid == model.UserGuid && o.Status == OrderStatus.Appreciated))
            {
                return new WorkerResult("You don't have any completed orders!");
            }

            if(Context.Users.Any(u => u.Email == model.ReceiverEmail))
            {
                return new WorkerResult($"User {model.ReceiverEmail} is already on Runtasker!");
            }

            Invitation duplicateI = Context.Invitations.FirstOrDefault(i => i.ReceiverEmail == model.ReceiverEmail);
            if (duplicateI != null)
            {
                return new WorkerResult($"An invitation for {duplicateI.ReceiverEmail} was sent!");
            }

            Invitation I = new Invitation
            {
                Id = Guid.NewGuid().ToString(),
                SenderGuid = model.UserGuid,
                ReceiverEmail = model.ReceiverEmail,
                Date = DateTime.Now,
                Status = InvitationStatus.Sent
            };

            Context.Invitations.Add(I);
            Context.SaveChanges();

            //Notification methods
            Notificater.OnUserSentAnInvitation(I);

            return new WorkerResult { Succeeded = true };
        }
        #endregion

        public WorkerResult OnUserRegisteredViaInvitation(ApplicationUser user)
        {
            Invitation I = Context.Invitations.FirstOrDefault(i => i.ReceiverEmail == user.Email);

            if(I == null)
            {
                return new WorkerResult("Invitation not found!");
            }

            I.ReceiverGuid = user.Id;
            I.Status = InvitationStatus.UserRegistered;
            Context.SaveChanges();

            Notificater.OnInvitedUserRegistered(user, I);

            return new WorkerResult
            {
                Succeeded = true
            };
        }

        #endregion

        #region Help Methods
        string GetUserEmail()
        {
            return Context.Users.FirstOrDefault(u => u.Id == UserGuid).Email;
        }

        void PlusUserBalance(decimal sum)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));
            var user = userManager.FindById(UserGuid);
            if (user != null)
            {
                user.Balance += sum;
            }
            
            userManager.Update(user);
        }
        #endregion
    }
}
