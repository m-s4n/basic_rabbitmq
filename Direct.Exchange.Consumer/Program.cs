
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
channel.ExchangeDeclare(exchange: "direct_exchange_1", type: ExchangeType.Direct);

var queueName = channel.QueueDeclare(durable: true, autoDelete: false, exclusive: false).QueueName;

channel.QueueBind(queue: queueName, exchange: "direct_exchange_1", routingKey: "direct_queue_1");


EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

consumer.Received += (sender, e) => 
{
    string mesaj = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(mesaj);
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false); // mesaj işlendi onayı
};

Console.Read();





