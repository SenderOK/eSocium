using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eSocium.Domain.Entities;

namespace eSocium.Web.Models
{
    public class QuestionsViewModel
    {
        public IEnumerable<Question> Questions { get; set; }
        public string SurveyName { get; set; }
        public int SurveyID { get; set; }
    }
}