using System;

namespace kafkaconsumer.Model
{
    public class Airqualityts
    {
        public Guid stationid { get; set; }
        public string parametername { get; set; }
        public long measurementdate { get; set; }
        public decimal parametervalue { get; set; }
    }
}
