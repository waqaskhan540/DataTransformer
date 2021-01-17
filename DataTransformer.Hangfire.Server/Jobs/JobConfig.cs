using Hangfire.RecurringJobExtensions;
using System;

namespace DataTransformer.Hangfire.Server.Jobs
{
    public class JobConfig
    {
        public static void RegisterJobs()
        {

            string cron = "0 */30 * ? * *";

            //CronJob.AddOrUpdate(GenerateJobInfo(typeof(JsonTransformationJob), cron));
            CronJob.AddOrUpdate(GenerateJobInfo(typeof(SqlTransformationJob), cron));
        }

        private static RecurringJobInfo GenerateJobInfo(Type type, string cron , string queue = "default" , bool enabled = true)
        {
            return new RecurringJobInfo
            {
                Cron = cron,
                Method = type.GetMethod("Execute"),
                RecurringJobId = type.Name,
                TimeZone = TimeZoneInfo.Utc,
                Queue = queue,
                Enable = enabled
            };
        }
    }
}
