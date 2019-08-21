using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace Fitfth_Topic_ExchangeSending
{
    class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "Topic_Exchange", type: "topic");

                var routingkey = (args.Length > 0) ? args[0] : "Routing Key value not asssign";

                var message = (args.Length > 1) ? string.Join(" ", args.Skip(1).ToArray()) : "Message not assign";

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "Topic_Exchange", routingKey: routingkey, basicProperties: null, body: body);

                Console.WriteLine("[x] sent routing key {0}, and message is {1}", routingkey, message);
            }
        }
    }
}
