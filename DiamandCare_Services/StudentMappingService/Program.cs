using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StudentMappingService
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        public static ManualResetEvent ResetEvent { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service()
            };

            // start as a windows application (in debug mode or console app mode)
            bool isConsoleApp = GetConsoleWindow() != IntPtr.Zero;
            if (Debugger.IsAttached || isConsoleApp)
            {
                ResetEvent = new ManualResetEvent(false);

                Service svc = ServicesToRun[0] as Service;

                Console.WriteLine("{0}: Starting...", DateTime.Now.ToString("MM/dd/yy hh:mm:ss"));
                svc.StartSvc();
                Console.Write("{0}: Started. Press ENTER to exit...", DateTime.Now.ToString("MM/dd/yy hh:mm:ss"));

                // exit if user presses ENTER
                if (isConsoleApp)
                {
                    while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Enter)
                        Thread.Sleep(10);

                    Console.WriteLine();
                    Console.WriteLine("{0}: Stopping. please wait...", DateTime.Now.ToString("MM/dd/yy hh:mm:ss"));
                    svc.StopSvc();
                    Console.WriteLine("{0}: Stopped...", DateTime.Now.ToString("MM/dd/yy hh:mm:ss"));
                }
                else
                {
                    ResetEvent.WaitOne();
                    svc.StopSvc(); // it may not come here
                }
            }
            else
                ServiceBase.Run(ServicesToRun);
        }
    }
}
