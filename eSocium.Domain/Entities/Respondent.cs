using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eSocium.Domain.Entities
{
    public class Respondent
    {
        public Respondent()
        {
            Answers = new List<Answer>();
        }

        [Key]
        public int RespondentID { get; set; }

        [Required]
        public int UserRespondentID { get; set; }

        public int? SurveyID { get; set; }
        [ForeignKey("SurveyID")]
        public virtual Survey Survey { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}