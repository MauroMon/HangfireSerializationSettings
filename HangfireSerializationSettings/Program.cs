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
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

                var classA = new ClassA();
                var classB = new ClassB();
                classB.ClassA = classA;
                classA.ClassB = classB;

                string test = JsonConvert.SerializeObject(classA, settings);

                Console.WriteLine(test);

                GlobalConfiguration.Configuration
                       .UseSerializerSettings(settings)
                       .UseActivator(new HangfireJobActivator())
                       .UseColouredConsoleLogProvider()
                       .LiteDbStorage("hangfire.db");

                var server = new BackgroundJobServer(new BackgroundJobServerOptions() { WorkerCount = 1, SchedulePollingInterval = TimeSpan.FromMilliseconds(5000) });

                DummyJob dummyJob = new DummyJob();
                BackgroundJob.Schedule(() => dummyJob.DummyJobExecution(classA), TimeSpan.FromSeconds(1));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }
    }
}
