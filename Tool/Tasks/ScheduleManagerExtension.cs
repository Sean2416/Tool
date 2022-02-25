using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using Tradevan_Hangfire;

namespace Tool.Tasks
{
    public class ScheduleManagerExtension : ScheduleManager
    {
        public ScheduleManagerExtension(ILogger<RecurringJobScheduler> logger) : base(logger)
        {
        }

        public override void RegisterSchedule()
        {
            RegisterSchedule<Sample1>("Sample1", x => x.Excute());
            RegisterSchedule<Sample1>("Sample2", x => x.Excute());

        }
    }
}
