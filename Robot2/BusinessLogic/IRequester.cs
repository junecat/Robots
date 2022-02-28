using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDocNumbers.BusinessLogic {
    public interface IRequester {
        Task<Tuple<int, bool>> CertCounterRequestProcAsync(DateTime dt);
    }
}
