using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ResourceMatcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args == null || args.Count() == 0)
            {
                //Installed service mode
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			    { 
				    new ResourceMatcherService() 
			    };
                ServiceBase.Run(ServicesToRun);    

            }
            else
            {
                //Console mode
                var service = new ResourceMatcherService();
                service.StartLoop();
                Console.WriteLine();
                service.StopLoop();
            }
            
        }
    }
}
