using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace eSocium.Domain.Entities
{
    public class Answer
    {
        [Key]
        public int AnswerID { get; set; }

        [Required]
        [Display(Name = "Text of the answer")]
        public string Text { get; set; }

        [Required]
        [HiddenInput(DisplayValue = true)]
        [Display(Name = "RespondentID")]
        public int UserRespondentID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? QuestionID { get; set; }
        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? RespondentID { get; set; }
        [ForeignKey("RespondentID")]
        public virtual Respondent Respondent { get; set; }
    }
}
