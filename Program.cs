using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FolderWatcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //var service = new FileWatcherConsole();
            //service.OnStart(new string[] { });
            //Console.Read();
            //return;

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FileWatcherService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
