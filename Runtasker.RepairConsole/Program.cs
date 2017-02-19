using Newtonsoft.Json.Linq;
using Runtasker.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using VkParser.Entities;
using VkParser.Workers.Api;

namespace Runtasker.RepairConsole
{

    class Program
    {
        static void Main(string[] args)
        {
            //// присваивает переменной a результат поиска по аудио с данными параметрами
            //var a = API.audio.search({ "q":"Beatles","count":3});
            // присваивает переменной b список владельцев найденных аудиозаписей
            //var b = a.items@.owner_id;
            // присваивает переменной с данные о страницах владельцев из списка b
            //var c = API.users.get({ "user_ids":b});
            // возвращает список фамилий из данных о владельцах
            //return c@.last_name;
            // пример цикла
            //var a = 1; var b = 10; while (b != 0) { b = b - 1; a = a + 1; }; return a;

            MyDbContext db = new MyDbContext();
            List<VkGroup> vkGroups = new List<VkGroup>();
            vkGroups.Add(new VkGroup { GroupId = 58897819 });
            vkGroups.Add(new VkGroup { GroupId = 61055670 });

            VkApiWorkerBase apiBase = new VkApiWorkerBase();
            

            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < 2; i++)
            {
                sb.Append($"var a{i}=")
                .Append("API.wall.get({" + $"owner_id:{-vkGroups[i].GroupId},count:{100},offset:0,extended=1" + "})");
            }
            sb.Append("return a0 + a1");

            string code = sb.ToString();


            string paramsString = $"code={code}";
            JObject resp = apiBase.Request(method: "execute", paramsString: paramsString);

            Console.ReadLine();
        }
    }
}
