using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UVSUzduotis.Data;
using System.Diagnostics;
using UVSUzduotis.Model;
using System.Windows.Threading;

namespace UVSUzduotis.Controller
{
    public class ThreadController
    {
        private readonly UVSDBContext _context;
        private static SymbolGeneratorController _symbolGeneratorController = new SymbolGeneratorController();
        private static readonly object _locker = new object();
        private readonly Dispatcher _dispatcher;


        public ThreadController(UVSDBContext context, Dispatcher dispatcher)
        {
            _context = context;
            _dispatcher = dispatcher;
        }
        public void ThreadSymbolGeneration(int threadAmountChoice, bool isRunning)
        {

            Thread[] threadArary = new Thread[threadAmountChoice];

           
                using(var context = new UVSDBContext())
                {
                    for (int i = 0; i < threadAmountChoice; i++)
                {

                    int threadID = i + 1;//assign thread ID
                        threadArary[i] = new Thread(() =>
                        {
                            while(isRunning)//Run while 
                            {
                                try
                                {
                                    string symbols = _symbolGeneratorController.GenerateSymbols(5, 10);
                                    lock (_locker)// Lock for thread safe database operations.
                                    {
                                        _dispatcher.Invoke(() =>
                                        {
                                            Console.WriteLine($"Thread {threadID} generated symbol: {symbols}");

                                            UVSUzduotisModel threadModelTest = new UVSUzduotisModel()
                                            {
                                                ThreadID = threadID,
                                                TimeCreated = DateTime.Now,
                                                GeneratedSymbols = symbols,

                                            };

                                            _context.UVSThreadTable.Add(threadModelTest);
                                            _context.SaveChanges();
                                        });
                                        Random sleepTime = new Random();
                                        int sleepTimeNumber = sleepTime.Next(500, 2000); //0.5 to 2 seconds thread wait.
                                        Thread.Sleep(sleepTimeNumber);
                                    } 
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error in thread {threadID}: {ex.Message}");
                                }
                            }
                        });
                        threadArary[i].IsBackground = true;
                        threadArary[i].Start();

                    }
                }

            foreach(Thread thread in threadArary)
            {
                thread.Join();
            }
            

        }


    }
}