using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tool.Mail;

namespace Tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly MailManager mailService;
        public MailController(MailManager mailService)
        {
            this.mailService = mailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMail()
        {
            try
            {
                BackgroundJob.Enqueue<MailManager>(r=>r.SendRegisterMail("你是胖子"));
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
