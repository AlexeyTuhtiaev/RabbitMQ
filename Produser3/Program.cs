using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace Produser3
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
                    channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                    //there is no queue here

                    string message = $"[{count}] The current time is {DateTime.Now}";
                    var body = Encoding.UTF8.GetBytes(message);
                    
                    channel.BasicPublish(exchange: "logs",
                                         routingKey: "",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine($"[{count++}] sent message into 'logs' Exchange");
                }
            } while (true);
        }
    }
}
