using Hangfire.Dashboard.Management.Metadata;
using Hangfire.Dashboard.Management.Support;
using System;
using System.ComponentModel;
using TradevanHangfire;

namespace Tool.Hangfire
{
    [ManagementPage("測試排程")]
    public class TestSchedule1 : IRecurringSchedule
    {
        [Job]
        [DisplayName("呼叫外部服務")]
        [Description("呼叫外部服務")]

        public void Excute()
        {
            Console.WriteLine("Test1");
        }
    }

}
