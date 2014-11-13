using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace eSocium.Domain.Entities
{
    public class Lemma
    {
        [Key]
        public int LemmaID { get; set; }

        [Required]
        public int OpenCorporaLemma { get; set; }

        [Required]
        public string Word { get; set; }

        public int? AnswerID { get; set; }
        [ForeignKey("AnswerID")]
        public virtual Answer Answer { get; set; }

        public int? LinkConfigurationID { get; set; }
        [ForeignKey("LinkConfigurationID")]
        public virtual LinkConfiguration LinkConfiguration { get; set; }
    }
}

