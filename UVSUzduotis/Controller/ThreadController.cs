using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using UVSUzduotis.Data;
using UVSUzduotis.Model;

namespace UVSUzduotis.Controller
{
    public class ThreadController
    {
        private readonly UVSDBContext _context;
        private static SymbolGeneratorController _symbolGeneratorController = new SymbolGeneratorController();
        private static readonly object _locker = new object();

        private Thread[] threads;
        private CancellationTokenSource cancellationToken;
        private bool isRunning;

        public ObservableCollection<UVSUzduotisModel> UVSModels { get; set; }
        public ThreadController(UVSDBContext context)
        {
            _context = context;
            cancellationToken = new CancellationTokenSource();
            UVSModels = new ObservableCollection<UVSUzduotisModel>();
        }

        //Main method to generate symbols.
        public void ThreadSymbolGeneration(int threadAmountChoice, bool start)
        {
            if (start && !isRunning)
            {
                cancellationToken = new CancellationTokenSource();
                StartThreads(threadAmountChoice);
            }
            else if(!start && isRunning)
            {
                cancellationToken.Cancel();
                StopThreads();
            }
        }      
        //Method to start the Threads.
        private void StartThreads(int threadAmountChoice)
        {
            threads = new Thread[threadAmountChoice];
            isRunning = true;
            //Based on the Thread amount picked by user, will generate symbols using X amount of threads constantly.
            for (int i = 0; i < threadAmountChoice; i++)
            {
                int threadID = i + 1;
                threads[i] = new Thread(() => GenerateSymbols(threadID));
                threads[i].IsBackground = true; //allows prompt exit of application if needed and long-running task prevent of closing the app.
                threads[i].Start();
            }
        }

        private void StopThreads()
        {
            isRunning = false;

            try {
                if(isRunning)
                {
                    foreach (var thread in threads)
                    {
                        if (thread != null && thread.IsAlive)//If threads are still alive and not null, join().
                        {
                            thread.Join();
                        }
                    }
                }
               
            } catch(Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
            }
                       
        }

        private void GenerateSymbols(int threadID)
        {
            while (!cancellationToken.IsCancellationRequested)//If the cancelation is not requested, keeps running.
            {
                try
                {
                    string symbols = _symbolGeneratorController.GenerateSymbols(5, 10);//Generates between 5-10 symbols.
                    lock (_locker)//Lock to make sure it's multi-thread safe for database, makes thread access one by one.
                    {
                        Console.WriteLine($"Thread {threadID} generated symbol: {symbols}"); //This is for monitoring trough console, can be removed if needed.

                        UVSUzduotisModel threadModelTest = new UVSUzduotisModel()//DB model, used to save data to database.
                        {
                            ThreadID = threadID,
                            TimeCreated = DateTime.Now,
                            GeneratedSymbols = symbols,
                        };

                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            if (UVSModels.Count >= 20)
                            {
                                UVSModels.RemoveAt(0); // Remove the oldest item if the collection has reached 20 items.
                            }
                            UVSModels.Add(threadModelTest);
                        });

                        _context.UVSThreadTable.Add(threadModelTest);
                        _context.SaveChanges();
                    }

                    Random sleepTime = new Random();
                    int sleepTimeNumber = sleepTime.Next(500, 2000);
                    Thread.Sleep(sleepTimeNumber);//Makes threads sleep betwenn 0.5-2 seconds to avoid massive data output.
                }
                catch (Exception ex)
                {
                    //can write more complex 
                    Console.WriteLine($"Error in thread {threadID}: {ex.Message}");
                }
            }
        }
    }
}