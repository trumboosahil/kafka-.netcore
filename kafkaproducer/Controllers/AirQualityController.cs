using Confluent.Kafka;
using kafkaproducer.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace kafkaproducer.Controllers
{
    public class AirQualityController : ControllerBase
    {
        private readonly string bootstrapServers = "localhost:9092";
        private readonly string topic = "airqualityts";
        [Route("airquality/Add")]
        [HttpPost]
        public async Task<IActionResult> AddReading([FromBody] Airqualityts airQualityts)
        {
            airQualityts.stationid = Guid.NewGuid();
            string message = JsonSerializer.Serialize(airQualityts);
            return Ok(await SendOrderRequest(topic, message));
        }


        private async Task<bool> SendOrderRequest(string topic, string message)
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName()
            };

            try
            {
                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {
                    var result = await producer.ProduceAsync
                    (topic, new Message<Null, string>
                    {
                        Value = message
                    });
                    Debug.WriteLine($"Delivery Timestamp:{ result.Timestamp.UtcDateTime}");
                    return await Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
            }

            return await Task.FromResult(false);
        }
    }
}