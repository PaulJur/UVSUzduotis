using System;
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

        private Thread[] _threads;
        private ManualResetEvent _stopEvent; //used to signal the Threads. For Multi-Threading apps.
        private bool _isRunning;

        public ThreadController(UVSDBContext context)
        {
            _context = context;
            _stopEvent = new ManualResetEvent(false); //Makes the Event non-signaled on call.
        }

        //Main method to generate symbols.
        public void ThreadSymbolGeneration(int threadAmountChoice, bool isRunning)
        {
            if (isRunning)
            {
                StartThreads(threadAmountChoice);
            }
            else
            {
                StopThreads();
            }
        }      
        //Method to start the Threads.
        private void StartThreads(int threadAmountChoice)
        {
            _threads = new Thread[threadAmountChoice];
            _stopEvent.Reset(); //Resets the event to non-signaled.
            _isRunning = true;
            //Based on the Thread amount picked by user, will generate symbols using X amount of threads constantly.
            for (int i = 0; i < threadAmountChoice; i++)
            {
                int threadID = i + 1;
                _threads[i] = new Thread(() => GenerateSymbols(threadID));
                _threads[i].IsBackground = true; //allows prompt exit of application if needed and long-running task prevent of closing the app.
                _threads[i].Start();
            }
        }

        private void StopThreads()
        {
            _isRunning = false;
            _stopEvent.Set();// Sets to Signaled state, allowing all threads to go and exit;


            try {
                if(_isRunning)
                {
                    foreach (var thread in _threads)
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
            while (_isRunning && !_stopEvent.WaitOne(0))//Check if the Threads are running and the even is signaled.
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