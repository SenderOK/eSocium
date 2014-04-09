using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eSocium.Web.Models.Abstract;
using OfficeOpenXml;

namespace eSocium.Web.Models.Concrete
{
    public class WorksheetXlsx : IWorksheet
    {
        ExcelRange cells;
        public WorksheetXlsx(ExcelPackage package, int sheet_number)
        {
            if (package.Workbook.Worksheets.Count < sheet_number || sheet_number < 0)
                //maybe || sheet_number < 0
            {
                throw new Exception("Sheet number is not valid");
            }
            cells = package.Workbook.Worksheets[sheet_number].Cells;
        }
        public string this[int row, int column]
        {
            get
            {
                var cellContents = cells[row + 1, column + 1].Value;
                return (cellContents == null) ? null : cellContents.ToString();
            }
        }
    }
}