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
        private SchoolExcelService SchoolExcelService;
        private CompanyExcelService CompanyExcelService;

        public SampleController(SchoolExcelService schoolExcelService, CompanyExcelService companyExcelService)
        {
            SchoolExcelService = schoolExcelService;
            CompanyExcelService = companyExcelService;
        }

  

        [HttpPost("MappingTest")]
        public async Task<IActionResult> MappingTest()
        {
            try
            {          
                var excel = SchoolExcelService.CreateExcel();

                SchoolExcelService.ExportExcelFile(excel, "C:\\Work\\Git\\AAAA.xlsx");

                excel = CompanyExcelService.CreateExcel();

                CompanyExcelService.ExportExcelFile(excel, "C:\\Work\\Git\\BBB.xlsx");

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public class Student
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Sex { get; set; }
            public DateTime BirthDay { get; set; }
        }
    }
}
