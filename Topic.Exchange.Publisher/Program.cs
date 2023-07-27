
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

// exchange
channel.ExchangeDeclare(exchange: "topic_exchange_1", type: ExchangeType.Topic, durable: true, autoDelete:false);
IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;

// mesaj gönder
while(true)
{
    Console.Write("Mesaj giriniz:");
    string mesaj = Console.ReadLine() ?? "mesaj Boş";
    Console.Write("Topic giriniz:");
    string topic = Console.ReadLine() ?? "konu_1";
    byte[] byteMesaj = Encoding.UTF8.GetBytes(mesaj);
    channel.BasicPublish(exchange: "topic_exchange_1",routingKey:topic, basicProperties: properties, body: byteMesaj);
}




