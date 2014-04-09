using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace eSocium.Domain.Entities
{
    public class Question
    {
        public Question()
        {
            Answers = new List<Answer>();
        }

        [Key]
        [HiddenInput(DisplayValue = false)]
        public int QuestionID { get; set; }

        [Required]
        [Display(Name = "Wording")]
        public string Wording { get; set; }

        [Required]
        [Display(Name = "Created on")]
        [HiddenInput(DisplayValue = true)]
        public DateTime CreationTime { get; set; }

        [Required]
        [Display(Name = "Last Modified")]
        [HiddenInput(DisplayValue = true)]
        public DateTime LastModificationTime { get; set; }

        [Display(Name = "Label")]
        public string Label { get; set; }

        [Display(Name = "Additional Info")]
        [DataType(DataType.MultilineText)]
        public string AdditionalInfo { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? SurveyID { get; set; }
        [ForeignKey("SurveyID")]
        public virtual Survey Survey { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        // last two virtual fields are aslo emptied by Edit operations
    }
}