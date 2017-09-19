using Runtaker.LocaleBuiders.Entities;
using Runtaker.LocaleBuiders.Workers;
using Runtasker.LocaleBuilders.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ResourceFileModel> rus = ResourceModelCreator.GetModelsByLang(Lang.Russian).ToList();

            List<ResourceFileModel> eng = ResourceModelCreator.GetModelsByLang(Lang.English).ToList();

            List<ResourceFileModel> ch = ResourceModelCreator.GetModelsByLang(Lang.Chinese).ToList();

            List<ResourceFileModel> all = ResourceModelCreator.GetModels().ToList();

            bool res = all.Count == (rus.Count + eng.Count + ch.Count);

            Console.ReadLine();
        }
    }
}
