using RabbitMQ.Client;
using System;
using System.Text;

namespace Queue
{
    public class Program
    {
        public static void Main(string[] args)
        {

            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "queue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments:null
                        );
                    var message = GetMessage(args);

                    var body = Encoding.UTF8.GetBytes(message.ToString());

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;


                    channel.BasicPublish(exchange: "",
                        routingKey: "queue",
                        basicProperties: properties,
                        body: body
                        );

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
            catch (Exception)
            {
                //
            }
        }
            
       


        private static object GetMessage(string[] args)
        {
            if (args.Length > 0)
            {
                return string.Join("", args);
            }
            return "hello world";

            //return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
    }
}
