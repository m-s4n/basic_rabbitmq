
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
channel.ExchangeDeclare(exchange: "fanout-exchange-1", type: ExchangeType.Fanout, durable: true, autoDelete: false);

// kuyruk oluşturulur
Console.Write("Kuyruk adını giriniz : ");
string queueName = Console.ReadLine() ?? "queue_name";
channel.QueueDeclare(queue: queueName, durable: true, autoDelete: false, exclusive: false);

// kuyruk exchange'e bind edilir.
channel.QueueBind(queue: queueName, exchange: "fanout-exchange-1", routingKey: string.Empty);

//mesajları tüketmek için consumer oluşturulur
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

consumer.Received += (sender, e) => 
{
    string mesaj = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(mesaj);
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
};

Console.Read();






