using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Tool.Excel
{
    public class ExcelInfo
    {
        public ExcelInfo()
        {
            Stream = new MemoryStream();
            using var document = SpreadsheetDocument.Create(Stream, SpreadsheetDocumentType.Workbook);
            WorkbookPart = document.AddWorkbookPart();
            WorkbookPart.Workbook = new Workbook();

            WorksheetPart = WorkbookPart.AddNewPart<WorksheetPart>();
            WorksheetPart.Worksheet = new Worksheet(new SheetData());
        }

        public MemoryStream Stream { get; set; }
               
        public WorkbookPart WorkbookPart { get; set; }
               
        public WorksheetPart WorksheetPart { get; set; }
    }
}
