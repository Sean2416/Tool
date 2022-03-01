using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using Tool.Mail;
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

            RegisterSchedule<MailManager>("Sample2", r => r.SendRegisterMail("測試排程"));

        }
    }
}
