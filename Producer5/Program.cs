using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Producer5
{
    class Program
    {
        private static readonly List<string> cars = new List<string> { "BMW", "Audi", "Tesla", "Mersedes" };
        private static readonly List<string> colors = new List<string> { "red", "white", "black" };
        static void Main(string[] args)
        {
            int count = 0;
            do
            {
                int timeToSleep = new Random().Next(1000, 2000);
                Thread.Sleep(timeToSleep);

                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");

                    string routingKey = count % 4 == 0 
                        ? "Tesla.red.fast.ecological" :
                        count % 5 == 0 ? "Mersedes.executive.expensive.ecological" : GenerateRoutingKey();

                    string message = $"[{count}] [{routingKey}] sent at {DateTime.Now}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "topic_logs",
                                         routingKey: routingKey,
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine($"[{count++}] sent message with [{routingKey}] into 'topic_logs' Exchange");
                }
            } while (true);
        }

        private static string GenerateRoutingKey()
        {
            return $"{cars[new Random().Next(0, 3)]}.{colors[new Random().Next(0, 2)]}";
        }
    }
}
