
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

//Pub-Sub
// mesaj exchange'e gönderileceği için exchange oluşturulur
channel.ExchangeDeclare(exchange: "pub_sub_exchange_1", type:ExchangeType.Fanout,durable: true, autoDelete: false);
IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;

// mesaj gönderilir
while(true)
{
    Console.Write("Mesaj giriniz:");
    string mesaj = Console.ReadLine() ?? "Mesaj girilmedi";
    byte[] byteMesaj = Encoding.UTF8.GetBytes(mesaj);
    channel.BasicPublish(exchange: "pub_sub_exchange_1", routingKey: string.Empty, body: byteMesaj, basicProperties: properties);
}


