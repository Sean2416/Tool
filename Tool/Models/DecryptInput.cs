using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tool.Models
{
    public class DecryptInput
    {
        public string Key { get; set; }

        public string IV { get; set; }

        public string DecryptString { get; set; }
    }
}
