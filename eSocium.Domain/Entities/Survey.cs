using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace eSocium.Domain.Entities
{
    public class Survey
    {
        public Survey ()
        {
            Questions = new List<Question>();
            Respondents = new List<Respondent>();
        }

        [Key]
        [HiddenInput(DisplayValue = false)]
        public int SurveyID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Created by")]
        [HiddenInput(DisplayValue = true)]
        public string CreatorName { get; set; }

        [Required]
        [Display(Name = "Created on")]
        [HiddenInput(DisplayValue = true)]
        public DateTime CreationTime { get; set; }

        [Required]
        [Display(Name = "Last Modified")]
        [HiddenInput(DisplayValue = true)]
        public DateTime LastModificationTime { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Region")]
        public string SurveyRegionDescription { get; set; }

        [Display(Name = "Conduction Time")]
        public string SurveyTimeDescription { get; set; }

        // these arrays are emptied by Edit operation
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Respondent> Respondents { get; set; }
    }
}