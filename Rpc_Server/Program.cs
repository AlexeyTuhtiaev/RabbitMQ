using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Rpc_Server
{
    class Program
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "rpc_queue", durable: false,
                  exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue",
                  autoAck: false, consumer: consumer);

                consumer.Received += (model, ea) =>
                {
                    string response = null;

                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);

                        int n = int.Parse(message);
                        if (n%5 == 0)
                        {
                            throw new Exception("Values n%5 are forbidden");
                        }

                        Console.WriteLine($"[Got parameters] fib({message}) at {DateTime.Now}");
                        Console.WriteLine("Calculating . . .");
                        response = Fib(n).ToString();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ERROR");
                        response = e.Message;
                        Console.WriteLine($"An exception was thrown at {DateTime.Now}");
                    }
                    finally
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                          basicProperties: replyProps, body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag,
                          multiple: false);
                        Console.WriteLine($"Result was sent at {DateTime.Now}");
                        Console.WriteLine();
                    }
                };

                Console.ReadLine();
            }
        }
        private static int Fib(int n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }

            return Fib(n - 1) + Fib(n - 2);
        }
    }
}
