using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace Producer6
{
    class Program
    {
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
                    channel.ExchangeDeclare(exchange: "fanout_ex", type: "fanout");

                    string routingKey = count % 4 == 0
                        ? "Tesla.red.fast.ecological" :
                        count % 5 == 0 ? "Mersedes.executive.expensive.ecological" : "whatever";

                    string message = $"[{count}] [{routingKey}] sent at {DateTime.Now}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "fanout_ex",
                                         routingKey: routingKey,
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine($"[{count++}] sent message with [{routingKey}] into 'fanout_ex' Exchange");
                }
            } while (true);
        }
    }
}
