using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace Tool.Excel
{
    public static class ExcelManager
    {
        public static ExcelInfo InsertData<TSource>(this ExcelInfo excel, ExcelMapper<TSource> mapper, IEnumerable<TSource> source, bool withColName = true)
        {
            var sheets = excel.WorkbookPart.Workbook.AppendChild(new Sheets());

            sheets.Append(new Sheet()
            {
                Id = excel.WorkbookPart.GetIdOfPart(excel.WorksheetPart),
                SheetId = 1,
                Name = "Sheet 1"
            });

            var sheetData = excel.WorksheetPart.Worksheet.GetFirstChild<SheetData>();

            foreach (var item in mapper.MapperConfigs)
            {
                var row = new Row();
                foreach (var s in source)
                {
                    row.Append(GetNewCell(item.Func(s)));
                }
                sheetData.AppendChild(row);
            }

            return excel;
        }

        public static MemoryStream GetMemoryStream(this ExcelInfo excel)
        {
             excel.Stream.Seek(0, SeekOrigin.Begin);

            return excel.Stream;
        }

        public static void ExportExcelFile(this MemoryStream stream, string fullName)
        {
            using var fileStream = new FileStream(fullName, FileMode.Create);
            stream.WriteTo(fileStream);
        }

        public static Cell GetNewCell(object value)
        {
            CellValue cellValue = new();
            var type = CellValues.String;

            if (value.GetType() == typeof(DateTime))
            {
                cellValue = new CellValue((DateTime)value);
                type = CellValues.Date;
            }
            else if (value.GetType() == typeof(decimal) || value.GetType() == typeof(double) || value.GetType() == typeof(int))
            {
                cellValue = new CellValue((decimal)value);
                type = CellValues.Number;
            }
            else
                cellValue = new CellValue((string)value);

            return new Cell()
            {
                CellValue = cellValue,
                DataType = type
            };
        }

    }
}
