using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eSocium.Web.Models.Abstract
{
    public static class IWorksheetExtension
    {
        public static int RowCount(this IWorksheet worksheet)
        {
            int answer = 0;
            while (worksheet[answer, 0] != null)
            {
                ++answer;
            }
            return answer;
        }
        public static int ColumnCount(this IWorksheet worksheet)
        {
            int answer = 0;
            while (worksheet[0, answer] != null)
            {
                ++answer;
            }
            return answer;
        }
        public static string[] Header(this IWorksheet worksheet)
        {
            string[] result = new string[worksheet.ColumnCount() - 1];
            for (int curr_column = 1; curr_column < worksheet.ColumnCount(); ++curr_column)
            {
                result[curr_column - 1] = worksheet[0, curr_column];
            }
            return result;
        }
    }
}