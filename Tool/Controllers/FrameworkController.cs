using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrameworkController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Get()
        {
            return "Framework Get";
        }

        [HttpPost]
        public async Task<string> Post()
        {
            return "Framework Post";
        }
    }
}
