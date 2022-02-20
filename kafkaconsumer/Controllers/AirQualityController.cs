using kafkaconsumer.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace kafkaconsumer.Controllers
{
    

    public class AirQualityController : ControllerBase
    {
        [Route("airquality/Add")]
        [HttpPost]
        public async Task<IActionResult> AddReading([FromBody] Airqualityts airQualityts)
        {
            //await airQuality.SaveAirQualityTs(airQualityts);

            return Ok("done");
        }
    }
}
