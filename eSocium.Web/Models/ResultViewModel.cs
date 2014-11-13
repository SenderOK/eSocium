using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eSocium.Web.Models
{
    public class ResultViewModel
    {        
        public ResultViewModel()
        {
            MostCommonWords = new List<KeyValuePair<string, int>>();
            UnknownWords = new List<string>();
            Clustering = new List<List<string>>();
        }
        public List<KeyValuePair<string, int> > MostCommonWords { get; set; }
        public List<string> UnknownWords { get; set; }
        public List<List<string>> Clustering { get; set; }
    }
}