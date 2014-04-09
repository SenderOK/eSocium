using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eSocium.Web.Models.Abstract;

namespace eSocium.Web.Models.Concrete
{
    public class RespondentAnswerTable
    {
        // Массив словарей (для каждого вопроса набор пар)
        public Dictionary<int, string>[] answers { get; private set; }
        // Метки столбцов листа
        public string[] questionLabels { get; private set; }

        // public int[] UserRespondentID { get; private set; }
        public RespondentAnswerTable(IWorksheet worksheet, bool hasHeader)
        {
            int question_count = worksheet.ColumnCount() - 1;
            if (question_count == 0) {
                throw new Exception("No questions found");
            }
            int row_count = (hasHeader) ? (worksheet.RowCount() - 1) : worksheet.RowCount();
            int first_row = (hasHeader) ? 1 : 0;
            questionLabels = (hasHeader) ? (worksheet.Header()) : (new string[0]);

            answers = new Dictionary<int, string>[question_count];
            for (int i = 0; i < answers.Length; ++i)
                answers[i] = new Dictionary<int, string>();

            //UserRespondentID = new int[row_count];
            //Конец инициализации и переход к копированию таблицы

            const int RespCol = 0;
            for (int row = first_row; worksheet[row, RespCol] != null; ++row)
            {
                try
                {
                    int resp_id = int.Parse(worksheet[row, RespCol]);
                    //UserRespondentID[row - first_row] = resp_id;
                    for (int question = 1; question <= question_count; ++question)
                    {
                        string resp_answer = worksheet[row, question];
                        if (!String.IsNullOrWhiteSpace(resp_answer))
                            answers[question - 1].Add(resp_id, resp_answer); // throws an exception when resp_id has duplicates
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("Error parsing row {0} . Probably wrong file format.\nException is {1}", row.ToString(), e.ToString()));
                }
            }
        }
    }
}