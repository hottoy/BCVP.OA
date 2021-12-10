using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.ViewModels
{
    public class LayselectView
    {
        public string code { get; set; }

        public string codeName { get; set; }

        public int status { get; set; }

        public string select { get; set; }
        public string groupName { get; set; }

        public string groupChilddren { get; set; }
    }
}
