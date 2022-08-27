using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Classes
{
    public class SearchData
    {
        public string searchStr { get; set; }

        public SearchData(string searchStr)
        {
            this.searchStr = searchStr;
        }
    }
}
