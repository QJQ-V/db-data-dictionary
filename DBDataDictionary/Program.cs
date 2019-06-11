using DBDataDictionary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataDictionaryService = new DataDictionaryService();

             dataDictionaryService.CreateHtml();

            Console.ReadKey();
        }
    }
}
