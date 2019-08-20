using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ReceiveMessageRabbit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Receive message");
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "hello",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                        // to create the the set of model property
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>//BasicDeliverEventArgs ea   one kind of parameter
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body);

                            Console.WriteLine(" [x] Received {0}", message);
                        };
                        channel.BasicConsume(queue: "hello",
                                             autoAck: true,
                                             consumer: consumer);

                        Console.WriteLine(" Press [enter] to exit.");
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
