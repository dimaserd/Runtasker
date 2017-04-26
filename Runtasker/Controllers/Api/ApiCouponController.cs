using Logic.Extensions.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic;
using Runtasker.Logic.Entities;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Runtasker.Controllers.Api
{
    [Authorize]
    public class ApiCouponController : ApiController
    {

        MyDbContext _db;

        MyDbContext Db
        {
            get
            {
                if(_db == null)
                {
                    _db = new MyDbContext();
                }
                return _db;

            }
        }
        string UserGuid
        {
            get { return User.Identity.GetUserId(); }
        }

        

        public async Task<WorkerResult> TryCoupon(string couponName)
        {
            //получаю купон и всех пользователей по нему
            Coupon coupon = await Db.Coupons.Include(x => x.Users).FirstOrDefaultAsync(x => x.Name == couponName);

            //если счетчик купона меньше чем кол-во пользователей его применивших
            if(coupon.Count < coupon.Users.Count)
            {

                //получаем пользователя
                ApplicationUser user = await Db.Users.FirstOrDefaultAsync(x => x.Id == UserGuid);

                coupon.Users.Add(user);


                if (user != null)
                {
                    //меняем номер телефона пользователя
                    //user.Balance = 

                    //соханяем изменения
                    //await UserManager.UpdateAsync(user);

                    //обновляем куки
                    //SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                }

                
            }

            return new WorkerResult
            {
                Succeeded = true
            };
        }
    }
}
