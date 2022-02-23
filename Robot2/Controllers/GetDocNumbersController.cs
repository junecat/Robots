using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Serilog;
using System.Threading;

namespace GetDocNumbers.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class GetDocNumbersController : ControllerBase {

        [HttpGet]
        public async Task<IActionResult> Get() {
            Log.Information($"GetDocNumbersController.Get() called");


            
            await Task.Run ( () => TestSyncMethod() );

            return Ok("Hi from Get!");
        
        }

        private string TestSyncMethod() {
            // Do stuff    
            Thread.Sleep(5000);

            return "55";
        }

    }

}
