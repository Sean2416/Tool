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

  

        [HttpPost("MappingTest")]
        public async Task<IActionResult> MappingTest()
        {
            try
            {
                List<Student> students = new List<Student>
                  {
                      new Student{ Id = 1,Name="夫子",Sex="男",BirthDay=new DateTime(1999,10,11) },
                      new Student{ Id = 2,Name="餘簾",Sex="女",BirthDay=new DateTime(1999,12,12) },
                      new Student{ Id = 3,Name="李慢慢",Sex="男",BirthDay=new DateTime(1999,11,11) },
                      new Student{ Id = 4,Name="葉紅魚",Sex="女",BirthDay=new DateTime(1999,10,10) }
                  };

               var mapper =  new ExcelMapper<Student>()
                .Map("Name", r => r.Name)
                .Map("Type", r => r.Sex);

                new ExcelInfo().InsertData(mapper, students)
                    .GetMemoryStream().ExportExcelFile("D:\\work\\test.xlsx");

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
