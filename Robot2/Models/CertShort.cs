using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDocNumbers.Models {
    class CertShort {
        public int id { get; set; }
        public int idStatus { get; set; }
        public string number { get; set; }
        public string date { get; set; }
        public DateTime? dateD { get; set; }
        public string endDate { get; set; }
        public DateTime? endDateD { get; set; }
        public string blankNumber { get; set; }
        public string technicalReglaments { get; set; }
        public string group { get; set; }
        public string certType { get; set; }
        public string certObjectType { get; set; }
        public string applicantLegalSubjectType { get; set; }
        public string applicantType { get; set; }
        public string applicantName { get; set; }
        public string applicantOpf { get; set; }
        public string applicantFilialFullNames { get; set; }
        public string manufacterLegalSubjectType { get; set; }
        public string manufacterType { get; set; }
        public string manufacterName { get; set; }
        public string manufacterOpf { get; set; }
        public string manufacterFilialFullNames { get; set; }
        public int? idRalCertificationAuthority { get; set; }
        public string certificationAuthorityAttestatRegNumber { get; set; }
        public string productOrig { get; set; }
        public string productFullName { get; set; }
        public string productBatchSize { get; set; }
        public string productIdentificationName { get; set; }
        public string productIdentificationType { get; set; }
        public string productIdentificationTrademark { get; set; }
        public string productIdentificationModel { get; set; }
        public string productIdentificationArticle { get; set; }
        public string productIdentificationSort { get; set; }
        public string productIdentificationGtin { get; set; }
        public string expertFio { get; set; }
        public string expertSnils { get; set; }
    }
}
