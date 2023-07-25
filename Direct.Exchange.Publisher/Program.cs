
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
channel.ExchangeDeclare(exchange: "direct_exchange_1", type: ExchangeType.Direct);
IBasicProperties properties = channel.CreateBasicProperties(); // mesaj kalıcılığı
properties.Persistent = true;

while (true)
{
    Console.Write("Mesaj : ");
    string mesaj = Console.ReadLine() ?? "Boş Mesaj";
    byte[] byteMesaj = Encoding.UTF8.GetBytes(mesaj);

    channel.BasicPublish(
        exchange: "direct_exchange_1",
        routingKey: "direct_queue_1",
        body: byteMesaj,
        basicProperties: properties); // property setlenir

}


