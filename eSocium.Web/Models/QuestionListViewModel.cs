using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eSocium.Domain.Entities;

namespace eSocium.Web.Models
{
    // this class is needed to know which survey are we working with while 
    // listing all its questions
    public class QuestionListViewModel
    {
        public IEnumerable<Question> Questions { get; set; }
        public Survey Survey { get; set; }
    }
}