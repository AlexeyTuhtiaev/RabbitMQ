using EasyNetQ;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RabbitMQApi.Controllers
{
    public class OrderController : ApiController
    {
        public HttpResponseMessage Post([FromBody] Order order)
        {
            var messageBus = RabbitHutch.CreateBus("host=localhost");

            using (var publishChannel = messageBus)
            {
                publishChannel.Publish(order);
            }
            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}
