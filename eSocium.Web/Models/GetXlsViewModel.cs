using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eSocium.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace eSocium.Web.Models
{
    public class GetXlsViewModel
    {
        [Required]
        public bool hasHeader { get; set; }

        [Required]
        [Display(Name = "Sheet with survey responses")]
        public int sheetNumber { get; set; }

        [Required]
        [Display(Name = "Column with survey responses")]
        public int Column { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public int SurveyID { get; set; }
        //[HiddenInput(DisplayValue = false)]
        //public int QuestionID { get; set; }
    }
}