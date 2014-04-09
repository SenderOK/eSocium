using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using eSocium.Web.Models.Abstract;

namespace eSocium.Web.Models.Concrete
{
    public class WorksheetXls : IWorksheet
    {
        ISheet workbook;
        public WorksheetXls(HSSFWorkbook package, int sheet_number)
        {
            --sheet_number;
            if (package.Workbook.NumSheets <= sheet_number || sheet_number < 0)
            {
                throw new Exception("Sheet number is not valid");
            }
            workbook = package.GetSheetAt(sheet_number);
        }
        public string this[int row, int column]
        {
            get
            {
                try
                {
                    ICell cell = workbook.GetRow(row).Cells[column];
                    return cell.ToString();
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}