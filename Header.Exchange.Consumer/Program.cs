
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
channel.ExchangeDeclare(exchange: "header_exchange_1", type: ExchangeType.Headers, durable: true, autoDelete: false);

// kuyruk oluşturulur
var queueName = channel.QueueDeclare(durable: true, exclusive: false, autoDelete: false).QueueName;

// header bilgileri alınır
Console.Write("Header id value gir:");
string idValue = Console.ReadLine() ?? "value_1";
Console.Write("Header ad value gir:");
string nameValue = Console.ReadLine() ?? "value_2";
Console.Write("x-match gir:");
string xmatchTur = Console.ReadLine() ?? "any";

// kuyruk exchange bind edilir
channel.QueueBind(queue: queueName, exchange: "header_exchange_1",routingKey: string.Empty, new Dictionary<string, object>{
    ["x-match"] = xmatchTur,
    ["id"] = idValue,
    ["name"] = nameValue
});

// consumer
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

// mesaj al
consumer.Received += (sender, e) => 
{
    string mesaj = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine(mesaj);
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
};

Console.Read();




