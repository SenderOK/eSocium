using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eSocium.Domain.Entities;

namespace eSocium.Web.Models
{
    public class AnswersViewModel
    {
        public int SurveyID { get; set; }
        public int QuestionID { get; set; }
        public string QuestionWording { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
    }
}