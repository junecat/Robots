using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDocNumbers.Models {
    class CertPostRequest {
        public int size { get; set; }
        public int page { get; set; }
        public CertFilter filter { get; set; }
        public List<CertColumnsSort> columnsSort { get; set; }
    }

    public class CertFilter {
        public CertRegDate regDate { get; set; }
        public CertEndDate endDate { get; set; }
        public List<object> columnsSearch { get; set; }
    }

    public class CertRegDate {
        public string minDate { get; set; }
        public string maxDate { get; set; }
    }

    public class CertEndDate {
        public string minDate { get; set; }
        public string maxDate { get; set; }
    }

    public class CertColumnsSort {
        public string column { get; set; }
        public string sort { get; set; }
    }


}
