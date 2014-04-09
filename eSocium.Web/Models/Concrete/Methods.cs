using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eSocium.Web.Models.Abstract;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;

namespace eSocium.Web.Models.Concrete
{
    class Methods
    {
        static public IWorksheet GetWorksheetFromHttpFile(HttpPostedFileBase xlsFile, int sheet_num)
        {
            if (xlsFile == null)
            {
                throw new Exception("No file uploaded");
            }
            if (xlsFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new WorksheetXlsx(new ExcelPackage(xlsFile.InputStream), sheet_num);
            }
            if (xlsFile.ContentType == "application/vnd.ms-excel")
            {
                return new WorksheetXls(new HSSFWorkbook(xlsFile.InputStream), sheet_num);
            }
            throw new Exception("Wrong file type");
        }
    }
}