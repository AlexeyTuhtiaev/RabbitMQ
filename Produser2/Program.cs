using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace Produser2
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 0;
            do
            {
                int timeToSleep = 1000;
                Thread.Sleep(timeToSleep);

                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "task_queue",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    string message = $"[{count}] The current time is {DateTime.Now}";
                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "",
                                         routingKey: "task_queue",
                                         basicProperties: properties,
                                         body: body);
                    
                    Console.WriteLine($"[{count++}] sent message into Default Exchange");
                }
            } while (true);
        }
    }
}
