using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UVSUzduotis.Data;
using System.Diagnostics;
using UVSUzduotis.Model;

namespace UVSUzduotis.Controller
{
    public class ThreadController
    {
        private readonly UVSDBContext _context;

        public ThreadController(UVSDBContext context)
        {
            _context = context;
        }
        public void ThreadTest()//Thread not safe with context, need to call DBcontext inside.
        {
            

            Thread thread1 = new Thread(() =>
            {
                string randomSymbols = SymbolGeneratorController.GenerateSymbols(10);
                Console.WriteLine($"Thread generated symbols: {randomSymbols}");
                
            });
            
        }

    }
}
