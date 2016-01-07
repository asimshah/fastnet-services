using Fastnet.EventSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
//[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Fastnet.Timer.Services
{    
    class Program
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            Log.SetApplicationName("Chronos");
            Log.Write("Fastnet Timer Services started");
            //log.DebugFormat("Fastnet.BackgroundServices: main()");
            HostFactory.Run(hc =>
            {
                hc.Service<ServiceManager>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(name => new ServiceManager());
                    serviceConfigurator.WhenStarted(sm => sm.Start());
                    serviceConfigurator.WhenStopped(sm =>
                    {
                        sm.Stop();
                        // And dispose or release any component containers (e.g. Castle) 
                        // or items resolved from the container.
                        //Log.Write("Fastnet Timer Services stopped");
                    });
                });
                hc.RunAsLocalSystem();
                hc.SetDescription("Fastnet Timer Services");
                hc.SetDisplayName("Fastnet Timer Services");
                hc.SetServiceName("Chronos"); // No spaces allowed
                hc.EnableShutdown();
                hc.EnableServiceRecovery(rc =>
                    {
                        rc.RestartService(1);
                    });
                hc.StartAutomatically();
            });

        }
    }
}
