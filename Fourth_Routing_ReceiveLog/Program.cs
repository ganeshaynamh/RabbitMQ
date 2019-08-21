using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Fourth_Routing_ReceiveLog
{
    class Program
    {
        public static void Main(string[] args)
        {

            
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");

                    var queue = channel.QueueDeclare().QueueName;

                    if (args.Length < 1)
                    {
                        Console.Error.WriteLine("Erro display: {0} [info] [warning] [error]", Environment.GetCommandLineArgs()[0]);
                        Console.WriteLine(" Press enter to close the windows.");
                        Console.ReadLine();
                        Environment.ExitCode = 1;
                        return;
                    }

                    foreach (var severity in args)
                    {
                        channel.QueueBind(queue: queue,
                                          exchange: "direct_logs",
                                          routingKey: severity);
                    }

                    Console.WriteLine(" [*] Waiting for messages.");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        Console.WriteLine(" [x] Received '{0}':'{1}'",
                                          routingKey, message);
                    };
                    channel.BasicConsume(queue: queue,
                                         autoAck: true,
                                         consumer: consumer);

                    Console.WriteLine(" Press enter to close windows");
                    Console.ReadLine();
                }
            }
            catch (Exception)
            {
                /// catch Exception,lsdflsdm
            }
        }
    }
}
