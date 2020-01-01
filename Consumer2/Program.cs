using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Consumer2
{
    class Program
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "task_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                //don't dispatch a new message to a worker until it has processed 
                //and acknowledged the previous one
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    int timeToSleep =  new Random().Next(1000, 2000);
                    Thread.Sleep(timeToSleep); //imitation of work 

                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    // manually send a proper acknowledgment from the proceeding  process
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                    Console.WriteLine("Processed message: {0}", message);
                };
                channel.BasicConsume(queue: "task_queue",
                                     autoAck: false,// manually send a proper acknowledgment from the proceeding  process
                                     consumer: consumer);

                Console.WriteLine("Subscribed to the queue <task_queue>");
                Console.WriteLine("Listerning . . .");
                Console.ReadLine();
            }
        }
    }
}
