
using Fastnet.Common;
using Fastnet.EventSystem;
//using Fastnet.EventSystem.Log4NetAppender;
//using Fastnet.Services.Common;
//using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fastnet.Timer.Services
{
    public class ServiceManager
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private Thread thread;
        //private bool disableSiteBackup;
        //private bool disableFolderBackup;
        //private bool disableApi;
        //private bool disableDatabaseBackup;
        //private bool disableFolderReplication;
        ////private List<ServiceLoop> services;
        //private bool defaultDisableSetting = true;
        private int interval;
        private CancellationTokenSource cancellationTokenSource;
        private Task processTask;
        private bool logTimerEvents;
        //private ConcurrentBag<Task> currentTasks;
        public async void Start()
        {
            //currentTasks = new ConcurrentBag<Task>();
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken ct = cancellationTokenSource.Token;
            logTimerEvents = ApplicationSettings.Key("LogTimerEvents", false);
            Log.Write("Timer Event logging is {0}", logTimerEvents ? "on" : "off");
            Log.SetApplicationName("Timer Services");
            interval = ApplicationSettings.Key("TimerInterval", 1);
            int remainder = GetRemainingInterval();
            Log.Write("Timer Services {0}, {1} minute interval, initial wait for {2} seconds", GetVersion(), interval, remainder);

            await Task.Delay(TimeSpan.FromSeconds(remainder), ct);
            if (ct.IsCancellationRequested)
            {
                ct.ThrowIfCancellationRequested();
            }
            //RunTimer(ct);
            processTask = Task.Run(() => Process(ct), ct);
        }

        public void Stop()
        {

            try
            {
                //Log.Write("Stopping ... {0} tasks outstanding", currentTasks.Count());
                if (processTask != null)
                {
                    cancellationTokenSource.Cancel();
                    Task.WaitAll(processTask);
                }
                Log.Write("Timer Services stopped");
            }
            catch (TaskCanceledException)
            {
            }
            catch (AggregateException ae)
            {
                if (!(ae.InnerException is TaskCanceledException))
                {
                    Log.Write(ae.InnerException);
                    Log.Write("Timer Services stopping [l0] ... exception {0}", ae.Message);
                }
            }
            catch (Exception xe)
            {
                Log.Write(xe);
                Log.Write("Timer Services stopping [l1] ... exception {0}", xe.Message);
            }

            

        }

        private async void PollUrls(CancellationToken ct)
        {
            ConfigurationManager.RefreshSection("appSettings");
            string urls = ApplicationSettings.Key<string>("PollUrls", null);
            if (urls != null)
            {
                string[] pollurls = urls.Split('|');
                foreach (string url in pollurls)
                {
                    try
                    {
                        HttpClient client = new HttpClient();
                        await client.GetAsync(url, ct);
                        if (ct.IsCancellationRequested)
                        {
                            ct.ThrowIfCancellationRequested();
                        }
                        if (logTimerEvents)
                        {
                            Log.Write("called {0}", url);
                        }
                    }
                    catch (Exception xe)
                    {
                        Log.Write(xe);
                    }
                }
            }
        }
        private string GetVersion()
        {
            Assembly thisAssembly = this.GetType().Assembly;
            string name = thisAssembly.FullName;
            string[] mainParts = name.Split(',');
            string[] versionPart = mainParts[1].Split('=');
            return versionPart[1].Trim();
        }
        private async void Process(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (logTimerEvents)
                {
                    Log.Write("Timer fired at {0}", DateTime.Now.ToString("ddMMMyyyy HH:mm:ss"));
                }
                PollUrls(token);
                try
                {
                    int remainder = GetRemainingInterval();
                    await Task.Delay(TimeSpan.FromSeconds(remainder), token);
                    token.ThrowIfCancellationRequested();
                }
                catch { }
            }
        }
        private int GetRemainingInterval()
        {
            // returns seconds to the end of the interval
            DateTime now = DateTime.Now;
            int seconds = now.Minute * 60 + now.Second;
            int remainder = (interval * 60) - (seconds % (interval * 60));
            return remainder;
        }
    }
}
