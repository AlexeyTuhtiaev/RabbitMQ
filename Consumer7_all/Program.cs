using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Consumer7_all
{
    class Program
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "headers_ex", type: ExchangeType.Headers);

                Dictionary<string, object> headers = new Dictionary<string, object>();
                headers.Add("x-match", "all");
                headers.Add("fileExtension", "zip");

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName,
                                  exchange: "headers_ex",
                                  routingKey: "doesn't matter 132etgwtyu",
                                  arguments: headers);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    StringBuilder extractedHeaders = new StringBuilder();

                    var keys = ea.BasicProperties.Headers.Keys;
                    foreach (string headerKey in keys)
                    {
                        byte[] value = ea.BasicProperties.Headers[headerKey] as byte[];
                        extractedHeaders.Append("Header key: ")
                                        .Append(headerKey)
                                        .Append(", value: ")
                                        .Append(Encoding.UTF8.GetString(value))
                                        .Append("; ");
                    }
                    Console.WriteLine("Processed message: {0}", message);
                    Console.WriteLine("All matches");
                    //Console.WriteLine($"Headers: {extractedHeaders}");
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine($"Subscribed to the queue <{queueName}>");
                Console.WriteLine($"'x - match' =  'all'");
                Console.ReadLine();
            }
        }
    }
}
