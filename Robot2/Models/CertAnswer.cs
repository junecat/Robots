using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDocNumbers.Models {
    class CertAnswer {
        public List<CertShort> items { get; set; }
        public int total { get; set; }
        public int size { get; set; }
    }
}
