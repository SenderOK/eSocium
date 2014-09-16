using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
namespace eSocium.Domain.Entities
{
    public class LinkConfiguration
    {
        public LinkConfiguration ()
        {
            Lemmas = new List<Lemma>();           
        }

        [Key]
        [HiddenInput(DisplayValue = false)]
        public int LinkConfigurationID { get; set; }

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
        
        [HiddenInput(DisplayValue = false)]
        public string Links { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public virtual ICollection<Lemma> Lemmas { get; set; }
    }
}
