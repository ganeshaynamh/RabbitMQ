
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Queue_ReceivingMsgRabbit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Receive message");
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                Console.WriteLine("[*] waiting for a message from sender");


                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine("[x] Received message {0} :", message);

                    int sp = message.Split('.').Length - 1;
                    Thread.Sleep(sp * 1000);

                    Console.WriteLine("  [x] Done");

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);


                };

                channel.BasicConsume(queue: "queue", autoAck: false, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();

            }
        }
    }
}
