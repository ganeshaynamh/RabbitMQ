using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Third_exchange_ReceiveMSg
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Exchange Receiving Message");
                var factory = new ConnectionFactory() { HostName = "localhost" };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                    //declaring Queue
                    var queue = channel.QueueDeclare().QueueName;

                    //binding the queue
                    channel.QueueBind(queue: queue, exchange: "logs", routingKey: "");

                    // pritnting the waiting logs

                    Console.WriteLine("[*] waiting logs ");

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var getmessage = Encoding.UTF8.GetString(body);

                        Console.WriteLine("[x] received message {0}", getmessage);


                    };
                    channel.BasicConsume(queue: queue,
                                     autoAck: true,
                                     consumer: consumer);

                    Console.WriteLine(" Press [enter] to close the windows.");
                    Console.ReadLine();


                }
            }
            catch (Exception)
            {
                //
            }


        }
    }
}
