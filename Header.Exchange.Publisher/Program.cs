
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
channel.ExchangeDeclare(exchange: "header_exchange_1", type: ExchangeType.Headers,durable: true, autoDelete: false);
IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;

while(true)
{
    byte[] byteMesaj = Encoding.UTF8.GetBytes(Console.ReadLine() ?? "Boş mesaj");
    Console.Write("Header id value gir:");
    string idValue = Console.ReadLine() ?? "value_1";
    Console.Write("Header ad value gir:");
    string nameValue = Console.ReadLine() ?? "value_2";
    properties.Headers = new Dictionary<string, object> ()
    {
       ["id"] = idValue,
       ["name"] = nameValue

    };

    channel.BasicPublish(exchange: "header_exchange_1", routingKey: string.Empty, basicProperties: properties, body: byteMesaj);
}




