using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVSUzduotis.Controller
{
    public class SymbolGeneratorController
    {

        public static string GenerateSymbols(int lenght)
        {
            Random random = new Random();

           const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            
            //A task that repeats the output based on the lenght input and randomises the output with Random().
            return new string(Enumerable.Repeat(chars, lenght).Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
