using System;

namespace HangfireSerializationSettings
{
    public class HangfireJobActivator : Hangfire.JobActivator
    {
        public override object ActivateJob(Type jobType)
        {
            if(jobType == typeof(DummyJob))
            {
                return new DummyJob();
            }
            return null;
        }
    }
}