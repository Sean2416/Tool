using System;
using System.Collections.Generic;

namespace Tool.Excel
{
    public class SchoolExcelService : ExcelService
    {
        public override void SetData()
        {
            InsertStudentData();
            InsertTeachData();
        }

        public void InsertStudentData()
        {
            List<Student> students = new List<Student>
            {
                new Student{ Id = 1,Name="夫子",Sex="男",BirthDay=new DateTime(1999,10,11) },
                new Student{ Id = 2,Name="餘簾",Sex="女",BirthDay=new DateTime(1999,12,12) },
                new Student{ Id = 3,Name="李慢慢",Sex="男",BirthDay=new DateTime(1999,11,11) },
                new Student{ Id = 4,Name="葉紅魚",Sex="女",BirthDay=new DateTime(1999,10,10) }
            };

            var mapper = new ExcelMapper<Student>()
             .Map("名稱", r => r.Name)
             .Map("性別", r => r.Sex)
             .Map("生日", r => r.BirthDay);

            InsertRowData(mapper, students, "學生資料");
        }

        public void InsertTeachData()
        {
            InsertRowData(new List<object> {
                "姓名","年齡","科別"
            }, "老師資料");

            InsertRowData(new List<object> {
                "陳XX", 18,"地理"
            }, "老師資料");

            InsertRowData(new List<object> {
                "王XX", 25.62,"英文"
            }, "老師資料");

            InsertRowData(new List<object> {
                "黃XX", 48,"國文"
            }, "老師資料");

            InsertRowData(new List<object> {
                "張XX", 66,"物理"
            }, "老師資料");

            InsertRowData(new List<object> {
                "吳XXX", 44,"美術"
            }, "老師資料");
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
