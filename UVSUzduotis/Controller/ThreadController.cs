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
        private static SymbolGeneratorController _symbolGeneratorController = new SymbolGeneratorController();
        private static readonly object _locker = new object();


        public ThreadController(UVSDBContext context)
        {
            _context = context;
        }
        public void ThreadSymbolGeneration(int threadAmountChoice)//Thread not safe with context, need to call DBcontext inside. ThreadID=Thread[i]. Also need a lock(lockobject), so only one thread per 0.5-2 seconds can access the DB add/DB savechanges.
        {

            Thread[] threadArary = new Thread[threadAmountChoice];

            lock (_locker)
            {
                using(var context = new UVSDBContext())
                {
                    for (int i = 0; i < threadAmountChoice; i++)
                    {
                        int threadID = i + 1;
                        threadArary[i] = new Thread(() =>
                        {
                            try
                            {
                                string symbols = _symbolGeneratorController.GenerateSymbols(5, 10);
                                lock (_locker)
                                {
                                    Console.WriteLine($"Thread {threadID} generated symbol: {symbols}");
                                }

                                Random sleepTime = new Random();
                                int sleepTimeNumber = sleepTime.Next(500, 2000);
                                Thread.Sleep(sleepTimeNumber);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error in thread {threadID}: {ex.Message}");
                            }


                        });
                        threadArary[i].Start();

                    }
                }
            }

            foreach(Thread thread in threadArary)
            {
                thread.Join();
            }
            
        }

    }
}
/* using (var context = new UVSDBContext())
            {
                Thread[] threadArray = new Thread[threadAmountChoice];


            }*/