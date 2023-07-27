
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
channel.ExchangeDeclare(exchange: "topic_exchange_1",type: ExchangeType.Topic,durable: true, autoDelete: false);

// kuyruk
Console.Write("Topic belirtin :");
string topic = Console.ReadLine() ?? "konu_1";

var queueName = channel.QueueDeclare(durable: true, exclusive: false, autoDelete: false).QueueName;

// Queue exchange'e bind edilir.
channel.QueueBind(queue: queueName, exchange: "topic_exchange_1", routingKey: topic);

// consumer
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

// tüket
consumer.Received += (sender, e) => 
{
    string mesaj = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine(mesaj);
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
};

Console.Read();




