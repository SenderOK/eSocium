using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eSocium.Web.Models.Abstract
{
    public interface IWorksheet
    {
        string this[int row, int column] { get; }
    }
}