
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

// exchange
channel.ExchangeDeclare(exchange: "fanout-exchange-1", type: ExchangeType.Fanout, durable: true, autoDelete: false);

IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;

// mesaj yayınlanır
while (true)
{
    string mesaj = Console.ReadLine() ?? "Mesaj Boş";
    byte[] byteMesaj = Encoding.UTF8.GetBytes(mesaj);
    channel.BasicPublish(exchange: "fanout-exchange-1", routingKey: string.Empty, body: byteMesaj, basicProperties: properties);
}




