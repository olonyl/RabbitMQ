using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Producer
{
   public static class HeaderExchangePubilsher
    {
        public static void Publish(IModel channel)
        {
            var ttl = new Dictionary<string, object> {
                {"x-message-ttl",30000 }
            };
            var header = new Dictionary<string, object> { { "account", "new" } };

            channel.ExchangeDeclare("demo-header-exchange", ExchangeType.Headers, arguments: ttl);
            var count = 0;
            while (true)
            {
                var message = new { Name = "Producer", Mesage = $"Hello, Count: {count}" };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                var properties = channel.CreateBasicProperties();
                properties.Headers = header;
                
                channel.BasicPublish("demo-header-exchange", String.Empty, properties, body);
                count++;
                //Thread.Sleep(1000);
            }
        }
    }
}
