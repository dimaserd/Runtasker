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
            List<ResourceFileModel> rus = ResourceModelCreator.GetModels(Lang.Russian).ToList();

            List<ResourceString> rusStrings = ResourceModelCreator.GetStringsFromResxFile(rus.First());

            List<ResourceFileModel> eng = ResourceModelCreator.GetModels(Lang.English).ToList();

            List<ResourceFileModel> ch = ResourceModelCreator.GetModels(Lang.Chinese).ToList();

            Console.ReadLine();
        }
    }
}
