using Confluent.Kafka;
using kafkaconsumer.Model;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace kafkaconsumer
{
    public class KafkaAirqualityConsumerService : IHostedService
    {
        private readonly string topic = "airqualityts";
        private readonly string groupId = "airqualityts_group";
        private readonly string bootstrapServers = "localhost:9092";
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            try
            {
                using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumerBuilder.Subscribe(topic);
                    var cancelToken = new CancellationTokenSource();

                    try
                    {
                        while (true)
                        {
                            var consumer = consumerBuilder.Consume
                               (cancelToken.Token);
                            var airq = JsonSerializer.Deserialize<Airqualityts>(consumer.Message.Value);
                            Debug.WriteLine($"Processing Order Id:{ airq.stationid}");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumerBuilder.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

          
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
           
        }
    }
}
