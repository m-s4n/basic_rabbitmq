

using System.Text;
using RabbitMQ.Client;

// Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.HostName = "localhost";
factory.Port = 5672;
factory.UserName = "musti";
factory.Password = "gopher";

//Bağlantı Aktifleştirme
using IConnection connection = factory.CreateConnection();

// Kanal Açma
using IModel channel = connection.CreateModel();

// Queue Oluşturma 
channel.QueueDeclare(queue: "gs", exclusive: false, durable: true, autoDelete: false);

// Mesaj Gönderme
// RabbitMQ mesajları byte[] olarak kabul eder
while (true)
{
    string mesaj = Console.ReadLine() ?? "mesaj girilmedi";
    byte[] byteMesaj = Encoding.UTF8.GetBytes(mesaj);
    channel.BasicPublish(exchange: "", routingKey: "gs", body: byteMesaj);
}




