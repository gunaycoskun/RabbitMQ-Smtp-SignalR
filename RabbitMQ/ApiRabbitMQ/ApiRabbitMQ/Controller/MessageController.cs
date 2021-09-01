using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApiRabbitMQ.Models;
using RabbitMQ.Client;

namespace ApiRabbitMQ.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromForm]User user)
        {
            
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://wfasjlev:m8MvYmb5sMLYCMPG7Ag-2iE-cXDLT64S@fish.rmq.cloudamqp.com/wfasjlev");
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            //Sıra adı, belleğe mesaj kaydedilsin mi olası hata durumunda korumak için, birden fazla kanal olsun mu, tüm mesajlar bitince kuyruk otomatik silinsin mi
            channel.QueueDeclare("Sıra adı", false, false, false);

            //doğrudan model çevrilemiyor
            string model=JsonSerializer.Serialize(user);

            byte[] data = Encoding.UTF8.GetBytes(model);
            channel.BasicPublish("", "Sıra adı",body:data);


            return Ok();
        }
    }
}
