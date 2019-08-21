using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Fifth_Topic_ExchangeReceive
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

                var QueueName = channel.QueueDeclare().QueueName;

                //argument value check the lessa than 1
                if (args.Length < 1)
                {
                    Console.Error.WriteLine("error was {0}  ", Environment.GetCommandLineArgs()[0]);

                    Console.WriteLine("Press Enter to Exit.");

                    Console.ReadLine();
                    Environment.ExitCode = 1;
                    return;

                }

                foreach (var item in args)
                {
                    channel.QueueBind(exchange: "Topic_Exchange", queue: QueueName, routingKey: item);
                }

                Console.WriteLine("waiting");

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var routingkey = ea.RoutingKey;

                    Console.WriteLine("[x] Received the Routing key :: {0} and Receive Message :: {1}", routingkey, message);

                };
                channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);

                Console.WriteLine("press Enter to Exit window");

                Console.ReadLine();
            }
        }
    }
}
