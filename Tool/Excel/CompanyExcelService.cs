using System;
using System.Collections.Generic;

namespace Tool.Excel
{
    public class CompanyExcelService : ExcelService
    {
        public override void SetData()
        {
            InsertRowData(new List<object> {
                "姓名","年齡","電話","薪資"
            });


            InsertRowData(new List<object> {
                "陳00",18,"2882-5252",36000
            });

            InsertRowData(new List<object> {
                "王00",30,"2882-1111",55000
            });

            InsertRowData(new List<object> {
                "裝00",20,"2222-1111",88000
            });
        }


    }
}
