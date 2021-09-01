using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CustomerRabbitMQ
{
    class Program
    {
        static async Task Main(string[] args)
        {

            HubConnection hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:44381/messagehub").Build();
            await hubConnection.StartAsync();

            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://wfasjlev:m8MvYmb5sMLYCMPG7Ag-2iE-cXDLT64S@fish.rmq.cloudamqp.com/wfasjlev");
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            //Sıra adı, belleğe mesaj kaydedilsin mi olası hata durumunda korumak için, birden fazla kanal olsun mu, tüm mesajlar bitince kuyruk otomatik silinsin mi
            channel.QueueDeclare("Sıra adı", false, false, false);

            EventingBasicConsumer customer = new EventingBasicConsumer(channel);
            //autoAck gidewn mesaj silinsin mi kuyruktan
            channel.BasicConsume("Sıra adı", true, customer);

            customer.Received +=async (s, e) =>
            {
                //Email operasyonu burada olacak... e.Body.Span gelen mesaj olacak

                string serializeData=Encoding.UTF8.GetString(e.Body.Span);
                User user=JsonSerializer.Deserialize<User>(serializeData);
                EMailSender.Send(user.Email, user.Message);
                Console.WriteLine($"{user.Email} mail gönderilmiştir.");

                await hubConnection.InvokeAsync("SendMessageAsync", $"{user.Email} mail gönderilmiştir.");
            };
            Console.Read();
        }
    }
}
