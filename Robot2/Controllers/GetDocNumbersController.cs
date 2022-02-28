using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Serilog;
using System.Threading;
using GetDocNumbers.BusinessLogic;

namespace GetDocNumbers.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class GetDocNumbersController : ControllerBase {

        [HttpGet]
        public async Task<IActionResult> Get() {
            // сюда GET'ом передаётся дата, за которую нужно посчитать кол-во документов
            Log.Information($"GetDocNumbersController.Get() called");

            var request = HttpContext.Request;
            var query = request.Query;
            var allItems = query.SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();
            var dtStr = allItems.Where(p => p.Key.ToLower() == "date").ToList();
            bool parseOk = DateTime.TryParseExact(dtStr[0].Value, "yyyy-MM-dd", null, 0, out var dt);
            if (parseOk) {
                Requester requester = new();
                var answ = await requester.CertCounterRequestProcAsync(dt);
                if ( answ.Item2) {
                    Log.Information($"answer: {dt.ToShortDateString()} - {answ.Item1} returned");
                    var tupl = new { date=dt, value=answ.Item1 };
                    return Ok(tupl);
                }
            }

            Log.Error("Error returned");
            return NoContent();

            //await Task.Run ( () => TestSyncMethod() );
            //return Ok("Hi from Get!");
        }

        private string TestSyncMethod() {
            // Do stuff    
            Thread.Sleep(5000);

            return "55";
        }

    }

}
