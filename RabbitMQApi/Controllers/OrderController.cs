using EasyNetQ;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RabbitMQApi.Controllers
{
    public class OrderController : ApiController
    {
        static int counter = 1;
        public HttpResponseMessage Post([FromBody] CarOrder order)
        {
            var messageBus = RabbitHutch.CreateBus("host=localhost");
            order.OrderNumber = counter;
            using (var publishChannel = messageBus)
            {
                publishChannel.Publish(order);
            }
            counter++;
            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}
