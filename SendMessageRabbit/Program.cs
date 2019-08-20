﻿using RabbitMQ.Client;
using System;
using System.Text;

namespace SendMessageRabbit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sending messsage!");
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null
                        );


                    Console.Write("Enter the message to sent : ");
                    string message = Console.ReadLine();
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
