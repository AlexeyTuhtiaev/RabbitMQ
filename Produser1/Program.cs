using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace Produser1
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 0;
            do
            {
                int timeToSleep = new Random().Next(1000, 2500);
                Thread.Sleep(timeToSleep);

                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "qName1",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    string message = $"The current time is {DateTime.Now}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "qName1",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine($" [{count++}] sent message into Default Exchange");
                }
            } while (true);
        }
    }
}
