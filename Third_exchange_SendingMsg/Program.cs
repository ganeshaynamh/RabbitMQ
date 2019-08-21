using RabbitMQ.Client;
using System;
using System.Text;

namespace Third_exchange_SendingMsg
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Sending Exchange Message");
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                    var message = GetMessage(args);
                    var Convert_message = Encoding.UTF8.GetBytes(message.ToString());

                    channel.BasicPublish(exchange: "logs",
                        routingKey: "",
                        basicProperties: null,
                        body: Convert_message
                        );
                    Console.WriteLine("[x] sent message is {0}", message);

                }

                Console.WriteLine("presss enter to close the Window");
                Console.ReadLine();

            }
            catch (Exception)
            {
                ///s
            }
        }

        private static object GetMessage(string[] args)
        {
            if (args.Length > 0)
            {
                return string.Join(" ", args);
            }
            return "hello jagdish this message is empty";
        }
    }
}
