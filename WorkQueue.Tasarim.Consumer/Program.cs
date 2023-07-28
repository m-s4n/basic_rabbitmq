
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

//Work queue
// mesaj direkt olarak kuyruğa gönderileceği için direkt kuyruk oluşturulur.
channel.QueueDeclare(queue: "work_queue_1", durable: true, exclusive: false, autoDelete: false);
channel.BasicQos(prefetchCount: 1, prefetchSize: 0, global: false);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "work_queue_1", autoAck: false, consumer: consumer);

consumer.Received += (sender, e) => 
{
    string mesaj = Encoding.UTF8.GetString(e.Body.Span);
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
    Console.WriteLine(mesaj);
};

Console.Read();