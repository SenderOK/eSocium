using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eSocium.Web.Models
{
    public class ResultViewModel
    {        
        public ResultViewModel()
        {
            MostCommonWords = new List<KeyValuePair<string, int>>();
            UnknownWords = new List<string>();
            Clustering = new List<List<string>>();            
            var tmp = new List<string>();
            tmp.Add("Remove");
            tmp.Add("Fix");
            tmp.Add("New Cluster");
            tmp.Add("Free");
            Actions = tmp;
        }
        public List<KeyValuePair<string, int> > MostCommonWords { get; set; }
        public List<string> UnknownWords { get; set; }
        public List<List<string>> Clustering { get; set; }
        public List<List<KeyValuePair<string, double>>> TypicalWords { get; set; }
        public List<List<bool>> Selected { get; set; }

        [Display(Name = "Select Action")]
        public int SelectedActionId { get; set; }

        public IEnumerable<string> Actions { get; set; }
    }
}