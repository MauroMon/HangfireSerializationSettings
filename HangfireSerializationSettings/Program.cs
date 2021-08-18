using Hangfire;
using Hangfire.LiteDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HangfireSerializationSettings
{
    class Program
    {
        static void Main(string[] args)
        {
            try { 
            GlobalConfiguration.Configuration
                   .UseSerializerSettings(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })
                   .UseRecommendedSerializerSettings((settings) =>
                   {
                       settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                   })
                   .UseActivator(new HangfireJobActivator())                   
                   .UseColouredConsoleLogProvider()
                   .LiteDbStorage("hangfire.db");

            var server = new BackgroundJobServer(new BackgroundJobServerOptions() { WorkerCount = 1, SchedulePollingInterval = TimeSpan.FromMilliseconds(5000) });

            var classA = new ClassA();
            var classB = new ClassB();
            classB.ClassA = classA;
            classA.ClassB = classB;
            DummyJob dummyJob = new DummyJob();
            BackgroundJob.Schedule(() => dummyJob.DummyJobExecution(classA), TimeSpan.FromSeconds(1));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }
    }
}
