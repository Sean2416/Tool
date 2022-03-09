using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tool.Excel;

namespace Tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        public SampleController()
        {
        }

  

        [HttpPost("Test")]
        public async Task<IActionResult> Test()
        {
            try
            {
               

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
