using EasyNetQ;
using RabbitMQApi.Controllers;
using System;

namespace RabbitMQ.OrderProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var messageBus = RabbitHutch.CreateBus("host=localhost");
            messageBus.Subscribe<CarOrder>("app_subscription_id", msg=> {
                Console.WriteLine("Processing order: {0} -- Model: {1} -- Color: {2}", msg.CustomerEmail, msg.Model, msg.Color);
            });
        }
    }
}
