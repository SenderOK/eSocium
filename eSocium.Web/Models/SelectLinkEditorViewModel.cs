using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eSocium.Web.Models
{
    public class SelectLinkEditorViewModel
    {
        public bool Selected { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }    
    }
}