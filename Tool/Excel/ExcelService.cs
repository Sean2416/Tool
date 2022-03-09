using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tool.Excel
{
    public abstract class ExcelService
    {
        private WorkbookPart WorkbookPart { get; set; }

        private Sheets Sheets { get; set; }

        public abstract void SetData();

        public MemoryStream CreateExcel()
        {
            var memoryStream = new MemoryStream();

            using var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);

            WorkbookPart = document.AddWorkbookPart();
            WorkbookPart.Workbook = new Workbook();
            Sheets = WorkbookPart.Workbook.AppendChild(new Sheets());

            SetData();

            memoryStream.Seek(0, SeekOrigin.Begin);

            WorkbookPart.Workbook.Save();
            document.Close();

            return memoryStream;
        }

        public void ExportExcelFile(MemoryStream stream, string fullName)
        {
            using var fileStream = new FileStream(fullName, FileMode.Create);
            stream.WriteTo(fileStream);
        }

        protected void InsertRowData<TSource>(ExcelMapper<TSource> mapper, IEnumerable<TSource> source, string sheetName ="Sheet 1", bool withColName = true)
        {
            if (withColName)
                InsertRowData(mapper.MapperConfigs.Select(r=> (object)r.ColumnName).ToList(), sheetName);

            foreach (var s in source)
                InsertRowData(mapper.MapperConfigs.Select(r => r.Func(s)).ToList(), sheetName);
        }

        protected void InsertRowData(List<object> list, string sheetName = "Sheet 1")
        {
            var worksheetPart = GetWorkSheetPart(sheetName);

            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            var row = new Row();

            foreach (var item in list)
            {
                row.Append(GetNewCell(item));
            }
            sheetData.AppendChild(row);
        }

        protected WorksheetPart GetWorkSheetPart(string sheetName)
        {
            var worksheetPart = RetrieveSheetPartByName(sheetName);

            if (worksheetPart != null)
                return worksheetPart;

            worksheetPart = WorkbookPart.AddNewPart<WorksheetPart>();
            Worksheet workSheet = new Worksheet();
            SheetData sheetData = new SheetData();

            workSheet.AppendChild(sheetData);
            worksheetPart.Worksheet = workSheet;

            uint sheetId = 1;

            if (Sheets.Elements<Sheet>().Any())
            {
                sheetId = Sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            Sheets.Append(new Sheet()
            {
                Id = WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = sheetId,
                Name = sheetName
            });


            return RetrieveSheetPartByName(sheetName);
        }

        protected WorksheetPart RetrieveSheetPartByName(string sheetName)
        {
            if (!WorkbookPart.Workbook.ChildElements.Any())
                return null;

            IEnumerable<Sheet> sheets = WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).ToList();

            if (!sheets.Any())
                return null;

            var part = (WorksheetPart)WorkbookPart.GetPartById(sheets.First().Id.Value);

            return part;
        }

        protected Cell GetNewCell(object value)
        {
            CellValue cellValue = new();
            var type = CellValues.String;

            if (value.GetType() == typeof(DateTime))
            {
                cellValue = new CellValue((DateTime)value);
                type = CellValues.Date;
            }
            else if (value.GetType() == typeof(decimal))
            {
                cellValue = new CellValue((decimal)value);
                type = CellValues.Number;
            }
            else if ( value.GetType() == typeof(double))
            {
                cellValue = new CellValue((double)value);
                type = CellValues.Number;
            }
            else if (value.GetType() == typeof(int))
            {
                cellValue = new CellValue((int)value);
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
