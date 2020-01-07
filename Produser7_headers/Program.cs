using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Produser7_headers
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(2000);
            int count = 0;
            Task.Run(() =>
            {                
                do
                {
                    int timeToSleep = new Random().Next(2000, 7000);
                    Thread.Sleep(timeToSleep);

                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    //factory.setUsername("");
                    //factory.setPassword("");
                    //factory.setVirtualHost("");
                    //factory.setPort(15672);
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: "headers_ex", type: "headers");

                        Dictionary<string, object> headers = new Dictionary<string, object>();
                        headers.Add("x-match", "all");
                        headers.Add("fileExtension", "zip");

                        var properties = channel.CreateBasicProperties();
                        properties.Headers = headers;

                        string message = $"[{count}] sent [from first produser] at {DateTime.Now}";
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "headers_ex",
                                             routingKey: "doesn't matter",
                                             basicProperties: properties,
                                             body: body);

                        Console.WriteLine($"[{count++}] sent [from 1] contains [zip]");
                    }
                } while (true);
            });

            Task.Run(() =>
            {
                do
                {
                    int timeToSleep = new Random().Next(2000, 5000);
                    Thread.Sleep(timeToSleep);

                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: "headers_ex", type: "headers");

                        Dictionary<string, object> headers = new Dictionary<string, object>();
                        headers.Add("x-match", "any");//we can ignore adding that header if it's "any"
                        headers.Add("fileExtension", "zip");
                        headers.Add("secrecyLevel", "simple");
                        headers.Add("receiver", "consumer A");

                        var properties = channel.CreateBasicProperties();
                        properties.Headers = headers;

                        string message = $"[{count}] sent [from second produser] at {DateTime.Now}";
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "headers_ex",
                                             routingKey: "doesn't matter",
                                             basicProperties: properties,
                                             body: body);

                        Console.WriteLine($"[{count++}] sent [from 2] contains [consumer A]");
                    }
                } while (true);
            });

            Task.Run(() =>
            {
                do
                {
                    int timeToSleep = new Random().Next(3000, 6000);
                    Thread.Sleep(timeToSleep);

                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: "headers_ex", type: "headers");

                        Dictionary<string, object> headers = new Dictionary<string, object>();
                        headers.Add("x-match", "any");//we can ignore adding that header if it's "any"                        
                        headers.Add("fileExtension", "jpeg");
                        headers.Add("secrecyLevel", "S9");
                        headers.Add("receiver", "consumer B");

                        var properties = channel.CreateBasicProperties();
                        properties.Headers = headers;

                        string message = $"[{count}] sent [from third produser] at {DateTime.Now}";
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "headers_ex",
                                             routingKey: "doesn't matter",
                                             basicProperties: properties,
                                             body: body);

                        Console.WriteLine($"[{count++}] sent [from 3] contains [jpeg]");
                    }
                } while (true);
            });
            Console.ReadKey();
        }
    }
}
