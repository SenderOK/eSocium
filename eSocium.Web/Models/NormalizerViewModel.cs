using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eSocium.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;


namespace eSocium.Web.Models
{
    public class NormalizerViewModel
    {
        public IEnumerable<LinkConfiguration> Configurations {get; set;}

        [Display(Name = "Select Configuration")]
        public int SelectedConfigurationId { get; set; }

        public int SelectedQuestionId { get; set; }
    }
}