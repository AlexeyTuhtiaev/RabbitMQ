using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Producer4
{
    class Program
    {
        static void Main(string[] args)
        {
            //Publish errors
            Task.Run(() =>
            {
                int count = 0;
                do
                {
                    int timeToSleep = new Random().Next(1000, 12000);
                    Thread.Sleep(timeToSleep);

                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");

                        //there is no queue here

                        string message = $"[{count}] [ERROR] The current time is {DateTime.Now}";
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "direct_logs",
                                             routingKey: "error",
                                             basicProperties: null,
                                             body: body);

                        Console.WriteLine($"[{count++}] sent message [ERROR] into 'direct_logs' Exchange");
                    }
                } while (true);
            });

            //Publish infos
            Task.Run(() =>
            {
                int count = 0;
                do
                {
                    int timeToSleep = new Random().Next(1000, 10000);
                    Thread.Sleep(timeToSleep);

                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");

                        //there is no queue here

                        string message = $"[{count}] [INFO] The current time is {DateTime.Now}";
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "direct_logs",
                                             routingKey: "info",
                                             basicProperties: null,
                                             body: body);

                        Console.WriteLine($"[{count++}] sent message [INFO] into 'direct_logs' Exchange");
                    }
                } while (true);
            });

            //Publish warnings
            Task.Run(() =>
            {
                int count = 0;
                do
                {
                    int timeToSleep = new Random().Next(1000, 8000);
                    Thread.Sleep(timeToSleep);

                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");

                        //there is no queue here

                        string message = $"[{count}] [WARNING] The current time is {DateTime.Now}";
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "direct_logs",
                                             routingKey: "warning",
                                             basicProperties: null,
                                             body: body);

                        Console.WriteLine($"[{count++}] sent message [WARNING] into 'direct_logs' Exchange");
                    }
                } while (true);
            });

            Console.ReadKey();
        }
    }
}
